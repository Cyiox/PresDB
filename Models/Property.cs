using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class Property
{
    public int PropertyId { get; set; }

    public int? Hudid { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyPhoneNumber { get; set; }

    public string? Street { get; set; }

    public string? Street2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public string? Zip4 { get; set; }

    public string? LocalUseRestrictionNotes { get; set; }

    public string? UnitsAtRiskStatus { get; set; }

    public string? Agency { get; set; }

    public string? TitleIiVi { get; set; }

    public string? NghdC { get; set; }

    public string? AgencyC { get; set; }

    public DateTime? AddressModifiedDate { get; set; }

    public DateTime? RecAddDate { get; set; }

    public string? RecAddBy { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public DateTime? VerifiedThroughDate { get; set; }

    public double? Lat { get; set; }

    public double? Lng { get; set; }

    public string? HubNameText { get; set; }

    public string? ServicingSiteNameText { get; set; }

    public string? CountyCode { get; set; }

    public string? CountyNameText { get; set; }

    public string? MsaCode { get; set; }

    public string? MsaNameText { get; set; }

    public string? CongressionalDistrictCode { get; set; }

    public string? PlacedBaseCityNameText { get; set; }

    public int? PropertyTotalUnitCount { get; set; }

    public string? PropertyCategoryName { get; set; }

    public string? PrimaryFinancingType { get; set; }

    public string? AssociatedFinancingNumber { get; set; }

    public string? IsInsuredInd { get; set; }

    public string? Is202811Ind { get; set; }

    public string? IsHudHeldInd { get; set; }

    public string? IsHudOwnedInd { get; set; }

    public string? IsHospitalInd { get; set; }

    public string? IsNursingHomeInd { get; set; }

    public string? IsBoardAndCareInd { get; set; }

    public string? IsAssistedLivingInd { get; set; }

    public string? IsRefinancedInd { get; set; }

    public string? Is221d3Ind { get; set; }

    public string? Is221d4Ind { get; set; }

    public string? Is236Ind { get; set; }

    public string? IsNonInsuredInd { get; set; }

    public string? IsBmirInd { get; set; }

    public string? IsRiskSharingInd { get; set; }

    public string? IsMipInd { get; set; }

    public string? IsCoInsuredInd { get; set; }

    public DateTime? OwnershipEffectiveDate { get; set; }

    public int? OwnerParticipantId { get; set; }

    public int? MgmtAgentParticipantId { get; set; }

    public bool? _121aTerminatedC { get; set; }

    public DateTime? _121aDateTerminatedC { get; set; }

    public DateTime? _121aAgreeDateC { get; set; }

    public int? _121aFirstYearC { get; set; }

    public int? _121aFinalYearC { get; set; }

    public int? _121aDurationC { get; set; }

    public int? UnitsNonElderlyC { get; set; }

    public int? UnitsElderlyC { get; set; }

    public string? SpecialPopulationTypeC { get; set; }

    public int? Units0brC { get; set; }

    public int? Units1brC { get; set; }

    public int? Units2brC { get; set; }

    public int? Units3brC { get; set; }

    public int? Units4brC { get; set; }

    public int? Units5brC { get; set; }

    public string? InsuranceTypeC { get; set; }

    public int? TaxCreditDeveloperC { get; set; }

    public string? RentSubsidyFormerVsNowC { get; set; }

    public int? OldS8eurIdC { get; set; }

    public string? EurCatC { get; set; }

    public int? OwnerC { get; set; }

    public int? MgmtAgentC { get; set; }

    public string? NonprofitStatusC { get; set; }

    public string? ImportMissingProjCodeC { get; set; }

    public DateTime? HudaddDate { get; set; }

    public DateTime? HudupdateDate { get; set; }

    public bool? HideHuddiscrepancy { get; set; }

    public int? UnitsAssisted { get; set; }

    public string? ProjectAka { get; set; }

    public string? MortgageStatusTcAndOtherRestrictions { get; set; }

    public string? CommentPreservationStatusTypeOrOtherNewAffordabilityTe { get; set; }

    public string? RiskStatusCategoryComment { get; set; }

    public string? AssociatedFinancingNumber2C { get; set; }

    public string? AssociatedFinancingNumber3C { get; set; }

    public string? AssociatedFinancingNumber4C { get; set; }

    public bool? IsAtRisk { get; set; }

    public bool? IsAtRisk2 { get; set; }

    public bool? IsAtRisk3 { get; set; }

    public DateTime? TitleIiatRiskDate { get; set; }

    public int? UnitsLostC { get; set; }

    public int? UnitsRetainedC { get; set; }

    public int? OrigUnitsAssistedC { get; set; }

    public int? S8PbaUnitsOverrideC { get; set; }

    public DateTime? EarliestTracsOverallExpDateC { get; set; }

    public int? UnitsAtRiskNum { get; set; }

    public string? UnitsAtRiskStatus2 { get; set; }

    public int? UnitsAtRiskNum2 { get; set; }

    public string? UnitsAtRiskStatus3 { get; set; }

    public int? UnitsAtRiskNum3 { get; set; }

    public bool? OtherPreserved { get; set; }

    public string? OtherAffordabilityRestriction { get; set; }

    public DateTime? OtherAtRiskDate { get; set; }

    public string? PropertyComments { get; set; }

    public string? ReacScore { get; set; }

    public DateTime? ReacDate { get; set; }

    public string? TotalHoUnitsMh { get; set; }

    public string? Units236Mh { get; set; }

    public string? Units13aMh { get; set; }

    public string? UnitsMktRateMh { get; set; }

    public string? UnitsSec8Mh { get; set; }

    public string? UnitsRapMh { get; set; }

    public string? UnitsSuppMh { get; set; }

    public string? UnitsMrvpBackupMh { get; set; }

    public string? UnitsMrvpPbMh { get; set; }

    public string? UnitsUnrestrictedFamilyMh { get; set; }

    public string? UnitsStudentMh { get; set; }

    public string? CommlSpaceNotUpdatedMh { get; set; }

    public string? UnitsRiskShareMh { get; set; }

    public string? ProjectIdMh { get; set; }

    public int? ProjectIdLending { get; set; }

    public bool? _40bTerminatedC { get; set; }

    public DateTime? _40bDateTerminatedC { get; set; }

    public DateTime? _40bAgreeDateC { get; set; }

    public int? _40bFirstYearC { get; set; }

    public int? _40bFinalYearC { get; set; }

    public int? _40bDurationC { get; set; }

    public string? _40bRestrictionNotesC { get; set; }

    public bool? _40bPerpetuity { get; set; }

    public DateTime? S8ExpirationDateOverrideC { get; set; }

    public bool? _40b { get; set; }

    public bool? _121a { get; set; }

    public virtual ICollection<DhcdArchive> DhcdArchives { get; set; } = new List<DhcdArchive>();

    public virtual ICollection<LihtcpubArchive> LihtcpubArchives { get; set; } = new List<LihtcpubArchive>();

    public virtual ICollection<MassHousingBenedictArchive> MassHousingBenedictArchives { get; set; } = new List<MassHousingBenedictArchive>();

    public virtual ICollection<MassHousingFoaArchive> MassHousingFoaArchives { get; set; } = new List<MassHousingFoaArchive>();

    public virtual ICollection<MfAssistanceSec8ContractsArchive> MfAssistanceSec8ContractsArchives { get; set; } = new List<MfAssistanceSec8ContractsArchive>();

    public virtual ICollection<MfPropertiesWithAssistanceSec8ContractsArchive> MfPropertiesWithAssistanceSec8ContractsArchives { get; set; } = new List<MfPropertiesWithAssistanceSec8ContractsArchive>();

    public virtual ICollection<MhpArchive> MhpArchives { get; set; } = new List<MhpArchive>();

    public virtual ICollection<MtgAArchive> MtgAArchives { get; set; } = new List<MtgAArchive>();

    public virtual ICollection<MtgTArchive> MtgTArchives { get; set; } = new List<MtgTArchive>();

    public virtual ICollection<PreservedUnitsAging> PreservedUnitsAgings { get; set; } = new List<PreservedUnitsAging>();

    public virtual PropertiesExcludedFromAtRiskReport? PropertiesExcludedFromAtRiskReport { get; set; }

    public virtual ICollection<UsdaArchive> UsdaArchives { get; set; } = new List<UsdaArchive>();
}
