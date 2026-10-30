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
    public string PropertyCategoryName { get; set; } = string.Empty;
    public string OwnerOrganizationName { get; set; } = string.Empty;
    public string ManagementOrganizationName { get; set; } = string.Empty;
}

public sealed class PropertyDetailsModel
{
    public PropertyModel Property { get; init; } = new();
    public IReadOnlyList<PropertyContractModel> Contracts { get; init; } = Array.Empty<PropertyContractModel>();
    public IReadOnlyList<LihtcDetailsModel> LihtcRecords { get; init; } = Array.Empty<LihtcDetailsModel>();
    public IReadOnlyList<DhcdDetailsModel> DhcdRecords { get; init; } = Array.Empty<DhcdDetailsModel>();
    public IReadOnlyList<MortgageDetailsModel> HudActMortgages { get; init; } = Array.Empty<MortgageDetailsModel>();
    public IReadOnlyList<MortgageDetailsModel> HudTerminatedMortgages { get; init; } = Array.Empty<MortgageDetailsModel>();
    public IReadOnlyList<UsdaDetailsModel> UsdaRecords { get; init; } = Array.Empty<UsdaDetailsModel>();
    public IReadOnlyList<MhpDetailsModel> MhpRecords { get; init; } = Array.Empty<MhpDetailsModel>();
    public IReadOnlyList<FoaDetailsModel> FoaRecords { get; init; } = Array.Empty<FoaDetailsModel>();
    public IReadOnlyList<PbvDetailsModel> PbvRecords { get; init; } = Array.Empty<PbvDetailsModel>();
    public string Status { get; init; } = string.Empty;
    public string PrimaryFinancingType { get; init; } = string.Empty;
    public int? UnitsAssisted { get; init; }
    public int? UnitsAtRisk { get; set; }
    public string Comments { get; init; } = string.Empty;
    public IReadOnlyList<PropertyCommentHistoryModel> CommentHistory { get; init; } = Array.Empty<PropertyCommentHistoryModel>();
    public int? S8Units { get; set; }
    public int? OriginalSubsidyUnits { get; set; }
    public DateTime? RestrictionOverrideDate { get; set; }
    public string ProgramTypeGroupCode { get; set; } = string.Empty;
    public int? HudAffordableUnits { get; set; }
    public int? EohlcTotalUnits { get; set; }
    public int? EohlcAffordableUnits { get; set; }
    public int? LihtcTotalUnits { get; set; }
    public int? LihtcAffordableUnits { get; set; }
    public int? LihtcPisYear { get; set; }
    public int? LihtcAllocationYear { get; set; }
    public int? PbvTotalUnits { get; set; }
    public DateTime? PbvLatestEndDate { get; set; }
    public int? AwardedPbvTotal { get; set; }
}

public sealed class PropertyCommentHistoryModel
{
    public DateTime EditedAt { get; init; }
    public string EditedBy { get; init; } = string.Empty;
    public string CommentText { get; init; } = string.Empty;
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
    public string ProgramTypeGroupCode { get; init; } = string.Empty;
}

public sealed class LihtcDetailsModel
{
    public DateTime? ImportDateTime { get; init; }
    public string HudId { get; init; } = string.Empty;
    public string Project { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public int? TotalUnits { get; set; }
    public int? LihtcUnits { get; init; }
    public int? LowIncomeUnits { get; init; }
    public int? AllocationYear { get; init; }
    public int? PlacedInServiceYear { get; init; }
    public int? CreditType { get; init; }
    public int? BondFinancing { get; init; }
    public int? RentAssistance { get; init; }
}

public sealed class DhcdDetailsModel
{
    public DateTime? ImportDateTime { get; init; }
    public string ProjectId { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public string BorrowerSponsor { get; init; } = string.Empty;
    public int? AffordableUnits { get; init; }
    public int? TotalUnits { get; init; }
    public decimal? NinePercentAwarded { get; init; }
    public decimal? FourPercentAwarded { get; init; }
    public decimal? StateLihtcAwarded { get; init; }
    public DateTime? AcquisitionPisDate { get; init; }
    public DateTime? RehabPisDate { get; init; }
}

public sealed class MortgageDetailsModel
{
    public DateTime? ImportDateTime { get; init; }
    public string HudProjectNumber { get; init; } = string.Empty;
    public string PremiseId { get; init; } = string.Empty;
    public int? Units { get; init; }
    public DateTime? InitialEndorsementDate { get; init; }
    public DateTime? FinalEndorsementDate { get; init; }
    public double? OriginalMortgageAmount { get; init; }
    public DateTime? MaturityDate { get; init; }
    public string SectionOfActCode { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime? TermDate { get; init; }
}

public sealed class UsdaDetailsModel
{
    public DateTime? ImportDateTime { get; init; }
    public string ProjectId { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int? ProjectSize { get; init; }
    public int? RentalAssistanceUnits { get; init; }
    public DateTime? TaxCreditExpires { get; init; }
    public DateTime? RestrictiveClauseExpires { get; init; }
}

public sealed class MhpDetailsModel
{
    public DateTime? ImportDateTime { get; init; }
    public string MhpId { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public double? LoanAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime? ClosingDate { get; init; }
    public int? AffordableUnits { get; init; }
    public int? TotalUnits { get; init; }
    public string Stage { get; init; } = string.Empty;
}

public sealed class FoaDetailsModel
{
    public DateTime? ImportDateTime { get; init; }
    public string ProjectId { get; init; } = string.Empty;
    public string HudProjectNumber { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public int? TotalUnits { get; init; }
    public int? LihtcUnits { get; init; }
    public int? Section8Units { get; init; }
    public string PrincipalProgram { get; init; } = string.Empty;
    public string MortgageInsurance { get; init; } = string.Empty;
}

public sealed class PbvDetailsModel
{
    public string Project { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public int? TotalUnits { get; init; }
    public int? PbvUnits { get; init; }
    public int? AwardedPbvTotal { get; init; }
    public DateTime? LatestEndDate { get; init; }
    public string ContractNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public int MatchConfidence { get; init; }
    public string MatchMethod { get; init; } = string.Empty;
}