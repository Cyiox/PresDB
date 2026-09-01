using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class PropertiesExcludedFromAtRiskReport
{
    public int PropertyId { get; set; }

    public int AtRiskReportStatusId { get; set; }

    public string? StatusNote { get; set; }

    public int? DuplicateOfPropertyId { get; set; }

    public DateTime? AddDate { get; set; }

    public string? AddUser { get; set; }

    public DateTime? ModDate { get; set; }

    public string? ModUser { get; set; }

    public virtual ZAtRiskReportExclusionStatus AtRiskReportStatus { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;
}
