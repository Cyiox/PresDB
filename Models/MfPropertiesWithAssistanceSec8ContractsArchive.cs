using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class MfPropertiesWithAssistanceSec8ContractsArchive
{
    public int ImportId { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? HubNameText { get; set; }

    public string? ServicingSiteNameText { get; set; }

    public string? PropertyId { get; set; }

    public string? PropertyNameText { get; set; }

    public string? PropertyPhoneNumber { get; set; }

    public string? AddressLine1Text { get; set; }

    public string? AddressLine2Text { get; set; }

    public string? CityNameText { get; set; }

    public string? StateCode { get; set; }

    public string? ZipCode { get; set; }

    public string? Zip4Code { get; set; }

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

    public string? IsOpportunityZoneInd { get; set; }

    public string? OwnershipEffectiveDate { get; set; }

    public int? OwnerParticipantId { get; set; }

    public string? OwnerCompanyType { get; set; }

    public string? OwnerIndividualFirstName { get; set; }

    public string? OwnerIndividualMiddleName { get; set; }

    public string? OwnerIndividualLastName { get; set; }

    public string? OwnerIndividualFullName { get; set; }

    public string? OwnerIndividualTitleText { get; set; }

    public string? OwnerOrganizationName { get; set; }

    public string? OwnerAddressLine1 { get; set; }

    public string? OwnerAddressLine2 { get; set; }

    public string? OwnerCityName { get; set; }

    public string? OwnerStateCode { get; set; }

    public string? OwnerZipCode { get; set; }

    public string? OwnerZip4Code { get; set; }

    public string? OwnerMainPhoneNumberText { get; set; }

    public string? OwnerMainFaxNumberText { get; set; }

    public string? OwnerEmailText { get; set; }

    public int? MgmtAgentParticipantId { get; set; }

    public string? MgmtAgentCompanyType { get; set; }

    public string? MgmtAgentIndvFirstName { get; set; }

    public string? MgmtAgentIndvLastName { get; set; }

    public string? MgmtAgentIndvMiddleName { get; set; }

    public string? MgmtAgentFullName { get; set; }

    public string? MgmtAgentIndvTitleText { get; set; }

    public string? MgmtAgentOrgName { get; set; }

    public string? MgmtAgentAddressLine1 { get; set; }

    public string? MgmtAgentAddressLine2 { get; set; }

    public string? MgmtAgentCityName { get; set; }

    public string? MgmtAgentStateCode { get; set; }

    public string? MgmtAgentZipCode { get; set; }

    public string? MgmtAgentZip4Code { get; set; }

    public string? MgmtAgentMainPhoneNumber { get; set; }

    public string? MgmtAgentMainFaxNumber { get; set; }

    public string? MgmtAgentEmailText { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
