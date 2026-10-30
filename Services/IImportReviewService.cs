using WebPresDB.Models;

namespace WebPresDB.Services;

public interface IImportReviewService
{
    Task<IReadOnlyList<ImportReviewSource>> GetSourcesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ImportReviewRow>> GetUnmatchedAsync(string dataSource, CancellationToken cancellationToken = default);
    Task LinkAsync(string dataSource, int importId, int propertyId, CancellationToken cancellationToken = default);
    Task<int> CreatePropertyAndLinkAsync(string dataSource, ImportReviewRow row, string user, CancellationToken cancellationToken = default);
    Task IgnoreAsync(string dataSource, int importId, string user, CancellationToken cancellationToken = default);
    Task<int> ImportCsvAsync(string dataSource, Stream file, string fileName, string user, CancellationToken cancellationToken = default);
}
