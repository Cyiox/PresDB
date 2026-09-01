using WebPresDB.Models;

namespace WebPresDB.Services;

public interface IPropertyService
{
    Task<PropertySearchPage> SearchPropertiesAsync(
        string searchTerm,
        string city,
        string propertyId,
        int offset,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PropertyDetailsModel?> GetPropertyDetailsAsync(int propertyId, CancellationToken cancellationToken = default);
}

public sealed record PropertySearchPage(IReadOnlyList<PropertyModel> Items, int TotalCount);