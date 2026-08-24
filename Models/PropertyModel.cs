namespace WebPresDB.Models;

public class PropertyModel
{
    public int PropertyId { get; set; }
    public int? HudId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public int? TotalUnits { get; set; }
}

public sealed class PropertyDetailsModel
{
    public PropertyModel Property { get; init; } = new();
    public IReadOnlyList<PropertyContractModel> Contracts { get; init; } = Array.Empty<PropertyContractModel>();
    public string Status { get; init; } = string.Empty;
    public string PrimaryFinancingType { get; init; } = string.Empty;
    public int? UnitsAssisted { get; init; }
    public int? UnitsAtRisk { get; init; }
    public string Comments { get; init; } = string.Empty;
}

public sealed class PropertyContractModel
{
    public int ImportId { get; init; }
    public DateTime? ImportDateTime { get; init; }
    public string ContractNumber { get; init; } = string.Empty;
    public DateTime? EffectiveDate { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public int? AssistedUnits { get; init; }
    public string ProgramType { get; init; } = string.Empty;
}