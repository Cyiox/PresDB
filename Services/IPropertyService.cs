using WebPresDB.Models;

namespace WebPresDB.Services;

public interface IPropertyService
{
    Task<PropertySearchPage> SearchPropertiesAsync(
        string searchTerm,
        string city,
        string propertyId,
        IReadOnlyCollection<string> hudProgramTypeGroupCodes,
        int offset,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<PropertyDetailsModel?> GetPropertyDetailsAsync(int propertyId, CancellationToken cancellationToken = default);

    Task SaveCommentAsync(int propertyId, string comment, CancellationToken cancellationToken = default);
}

public sealed record PropertySearchPage(IReadOnlyList<PropertyModel> Items, int TotalCount);