using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class UsdaArchive
{
    public int ImportId { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? BorrowerProjectId { get; set; }

    public string? BorrowerId { get; set; }

    public string? ProjectId { get; set; }

    public int? ProjectCheckDigit { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? StateCountyFipsCode { get; set; }

    public string? ProjectName { get; set; }

    public string? MainAddressLine1 { get; set; }

    public string? MainAddressLine2 { get; set; }

    public string? MainAddressLine3 { get; set; }

    public string? City { get; set; }

    public string? StateAbbreviation { get; set; }

    public string? ZipCode { get; set; }

    public int? ProjectSize { get; set; }

    public string? RentalCode { get; set; }

    public int? LaborHousingType { get; set; }

    public string? RevitilizationIndicator { get; set; }

    public string? TaxStatusIndicator { get; set; }

    public DateTime? DateTaxCreditExpires { get; set; }

    public int? ProfitTypeCode { get; set; }

    public string? ManagementName { get; set; }

    public DateTime? DateOfOperation { get; set; }

    public DateTime? DateRestrictiveClauseExpires { get; set; }

    public int? Total1BedroomUnits { get; set; }

    public int? Total2BedroomUnits { get; set; }

    public int? Total3BedroomUnits { get; set; }

    public int? Total4BedroomUnits { get; set; }

    public int? Total5BedroomUnits { get; set; }

    public int? Total6BedroomUnits { get; set; }

    public int? TotalHandicappedUnits { get; set; }

    public int? VacantUnits { get; set; }

    public int? RentalAssistanceUnits { get; set; }

    public string? ProjectMfisIdKey { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
