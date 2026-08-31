using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class AtRiskReportDataArchive
{
    public int Id { get; set; }

    public DateTime ReportRunDateTime { get; set; }

    public DateTime ReportAtRiskDate { get; set; }

    public string? ReportName { get; set; }

    public string? ReportUsername { get; set; }

    public int? PropertyId { get; set; }

    public string? PropertyName { get; set; }

    public string? Street { get; set; }

    public string? Zip { get; set; }

    public string? City { get; set; }

    public string? Agency { get; set; }

    public string? LocalUseRestrictionNotes { get; set; }

    public int? TotalUnits { get; set; }

    public int? S8Pbaunits { get; set; }

    public DateTime? S8ExpDate { get; set; }

    public int? OrigSubsidyUnits { get; set; }

    public int? SubsidyUnitsLost { get; set; }

    public int? NewAffordableUnits { get; set; }

    public int? NetUnitsLost { get; set; }

    public int? CurrentUnitsAssisted { get; set; }

    public int? UnitsAtRisk { get; set; }

    public string? UnitsAtRiskStatus { get; set; }

    public string? PreservedByProgram { get; set; }

    public DateTime? PreservedThroughDate { get; set; }
}
