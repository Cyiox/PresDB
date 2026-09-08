using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class MassHousingFoaArchive
{
    public int ImportId { get; set; }

    public DateTime? ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? ProjectNameProjectMaster { get; set; }

    public string? ProjectIdProjectMaster { get; set; }

    public string? HudProjectNumberProjectMaster { get; set; }

    public string? HudRiskshareProjectNumberProjectMaster { get; set; }

    public string? Section8ContractNumberSection8Contracts { get; set; }

    public int? PrimaryAddressSiteAddress { get; set; }

    public string? SiteAddressSiteAddress { get; set; }

    public string? CityTownNameCityTownSiteAddress { get; set; }

    public string? StateSiteAddress { get; set; }

    public string? ZipCode1SiteAddress { get; set; }

    public string? ZipCode2SiteAddress { get; set; }

    public int? RentalHoUnits { get; set; }

    public int? TotalRentalUnitsProjectMaster { get; set; }

    public int? TotalHoUnitsProjectMaster { get; set; }

    public int? RentalUnits236ProjectMaster { get; set; }

    public int? RentalUnits13aProjectMaster { get; set; }

    public string? MortgageInsuranceCodeProjectMaster { get; set; }

    public int? EligibleBasisAmtProjectMaster { get; set; }

    public int? LihtcUnitsProjectMaster { get; set; }

    public int? RentalUnitsMktRateProjectMaster { get; set; }

    public int? RentalUnitsSec8ProjectMaster { get; set; }

    public int? RentalUnitsRapProjectMaster { get; set; }

    public int? RentalUnitsRsProjectMaster { get; set; }

    public int? RentalUnitsMrvpBuProjectMaster { get; set; }

    public int? RentalUnitsMrvpPbProjectMaster { get; set; }

    public int? RentalUnitsElderlyRestrictedProjectMaster { get; set; }

    public int? RentalUnitsUnrestrictedProjectMaster { get; set; }

    public int? RentalUnitsStudentProjectMaster { get; set; }

    public int? RentalUnitsOtherRestrictedProjectMaster { get; set; }

    public int? RentalUnits0BrProjectMaster { get; set; }

    public int? RentalUnits1BrProjectMaster { get; set; }

    public double? RentalUnits2BrProjectMaster { get; set; }

    public int? RentalUnits3BrProjectMaster { get; set; }

    public int? RentalUnits4BrProjectMaster { get; set; }

    public int? RentalUnits5BrProjectMaster { get; set; }

    public int? RentalUnits6BrProjectMaster { get; set; }

    public int? CommercialProjectMaster { get; set; }

    public string? ProgramTypeProgramType { get; set; }

    public string? PrincipalProgramProjectMaster { get; set; }

    public string? PrincipalProgramDescriptionPrincipalProgram { get; set; }

    public string? DebtIndicator { get; set; }

    public int? FoaTotalUnitCount { get; set; }

    public int? LowIncomeRentalUnits { get; set; }

    public int? ModerateIncomeRentalUnits { get; set; }

    public string? MortgageInsuranceDesc { get; set; }

    public int? NonRevenueRentalUnits { get; set; }

    public string? Section8AdministrationType { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
