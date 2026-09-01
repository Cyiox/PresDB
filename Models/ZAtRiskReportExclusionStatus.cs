using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class ZAtRiskReportExclusionStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PropertiesExcludedFromAtRiskReport> PropertiesExcludedFromAtRiskReports { get; set; } = new List<PropertiesExcludedFromAtRiskReport>();
}
