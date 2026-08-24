from django.shortcuts import render,get_object_or_404
from django.db.models import Q
from django.core.paginator import Paginator
from apps.home.models import Property


def index(request):
    """Renders the Home page."""
    return render(request, 'index.html')


def property_search(request):
    """HTMX endpoint: filters properties and returns the results partial inline on the home page."""
    q = request.GET.get('q', '').strip()
    project_city = request.GET.get('project_city', '').strip()
    property_id = request.GET.get('property_id', '').strip()

    queryset = Property.objects.prefetch_related('sec8_contracts').all().order_by('exp_use_property_id')
    query_performed = bool(q or project_city or property_id)

    if project_city:
        queryset = queryset.filter(city__icontains=project_city)

    if property_id and property_id.isdigit():
        queryset = queryset.filter(exp_use_property_id=int(property_id))

    if q:
        q_filter = (
            Q(property_name__icontains=q) |
            Q(city__icontains=q) |
            Q(street__icontains=q) |
            Q(hud_id__icontains=q)
        )
        if q.isdigit():
            q_filter |= Q(exp_use_property_id=int(q))
        queryset = queryset.filter(q_filter)

    # Top 20 results for quick inline display
    results = queryset[:20] if query_performed else Property.objects.none()

    context = {
        'properties': results,
        'total_count': queryset.count() if query_performed else 0,
        'query_performed': query_performed,
    }
    return render(request, 'partials/_search_results.html', context)


def reports_view(request):
    """Standalone reports page with full table and pagination."""
    queryset = Property.objects.prefetch_related('sec8_contracts').all().order_by('exp_use_property_id')

    q = request.GET.get('q', '').strip()
    project_city = request.GET.get('project_city', '').strip()
    project_name = request.GET.get('project_name', '').strip()
    property_id = request.GET.get('property_id', '').strip()

    if project_city:
        queryset = queryset.filter(city__icontains=project_city)

    if project_name:
        queryset = queryset.filter(property_name__icontains=project_name)

    if property_id and property_id.isdigit():
        queryset = queryset.filter(exp_use_property_id=int(property_id))

    if q:
        q_filter = (
            Q(property_name__icontains=q) |
            Q(city__icontains=q) |
            Q(street__icontains=q) |
            Q(hud_id__icontains=q)
        )
        if q.isdigit():
            q_filter |= Q(exp_use_property_id=int(q))
        queryset = queryset.filter(q_filter)

    paginator = Paginator(queryset, 25)
    page_number = request.GET.get('page')
    page_obj = paginator.get_page(page_number)

    query_params = request.GET.copy()
    if 'page' in query_params:
        del query_params['page']

    context = {
        'page_obj': page_obj,
        'total_count': paginator.count,
        'q': q,
        'project_city': project_city,
        'project_name': project_name,
        'property_id': property_id,
        'query_string': query_params.urlencode(),
    }
    return render(request, 'reports.html', context)

def property_detail(request, pk):
    """Inspector view: Executive summary header + MS Access-style tabs + Record navigator."""
    property_obj = get_object_or_404(
        Property.objects.prefetch_related('sec8_contracts'), 
        exp_use_property_id=pk
    )

    contracts = list(property_obj.sec8_contracts.all().order_by('-import_datetime'))
    total_records = len(contracts)

    # Manage bottom record navigator index (0-based indexing for 1 of N)
    try:
        record_idx = int(request.GET.get('record', 0))
    except ValueError:
        record_idx = 0

    if total_records > 0:
        record_idx = max(0, min(record_idx, total_records - 1))
        current_contract = contracts[record_idx]
    else:
        current_contract = None

    context = {
        'property': property_obj,
        'contracts': contracts,
        'current_contract': current_contract,
        'record_idx': record_idx,
        'record_num': record_idx + 1 if total_records > 0 else 0,
        'total_records': total_records,
        'prev_idx': max(0, record_idx - 1),
        'next_idx': min(total_records - 1, record_idx + 1) if total_records > 0 else 0,
    }

    # If requested via HTMX for record navigation, render just the tab/record partial
    if request.headers.get('HX-Request') and 'record' in request.GET:
        return render(request, 'partials/_property_tab_content.html', context)

    return render(request, 'property_detail.html', context)