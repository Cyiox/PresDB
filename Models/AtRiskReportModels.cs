namespace WebPresDB.Models;

public enum ReportDataSnapshot
{
    MostRecent,
    OldestAvailable
}

public sealed class AtRiskReportRow
{
    public int PropertyId { get; init; }
    public string PropertyName { get; init; } = "";
    public string City { get; init; } = "";
    public string Street { get; init; } = "";
    public string Zip { get; init; } = "";
    public string Agency { get; init; } = "";
    public int? TotalUnits { get; init; }
    public int? UnitsAtRisk { get; init; }
    public DateTime? PreservedThroughDate { get; init; }
    public string Status { get; init; } = "";
    public string PreservationRule { get; init; } = "";
    public bool HasDhcdAwards { get; init; }
    public string DhcdProjectName { get; init; } = "";
    public decimal? DhcdStateLihtcAwarded { get; init; }
    public decimal? DhcdNinePctAwarded { get; init; }
    public int? DhcdAffordableUnits { get; init; }
    public DateTime? DhcdImportDate { get; init; }
}

public sealed class AtRiskReportResult
{
    public DateTime ForecastDate { get; init; }
    public IReadOnlyList<AtRiskReportRow> Rows { get; init; } = Array.Empty<AtRiskReportRow>();
}