namespace WebPresDB.Models;

public sealed class ImportReviewRow
{
    public int ImportId { get; init; }
    public DateTime ImportDateTime { get; init; }
    public string? ExternalId { get; init; }
    public string? PropertyName { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Zip { get; init; }
    public int? LinkedPropertyId { get; init; }
    public int? SelectedPropertyId { get; set; }
}

public sealed class ImportReviewSource
{
    public string DataSource { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string TableName { get; init; } = null!;
    public DateTime? LatestImportDate { get; init; }
    public int UnmatchedCount { get; init; }
}
