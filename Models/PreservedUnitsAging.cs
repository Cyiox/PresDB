using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class PreservedUnitsAging
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public DateTime PreservedThroughDate { get; set; }

    public int? TotalUnits { get; set; }

    public int? S8Pbaunits { get; set; }

    public DateTime? S8ExpDate { get; set; }

    public int? OrigSubsidyUnits { get; set; }

    public int? SubsidyUnitsLost { get; set; }

    public int? NewAffordableUnits { get; set; }

    public int? UnitsAtRisk { get; set; }

    public int? PreservedByProgram { get; set; }

    public DateTime? RecAddDate { get; set; }

    public string? RecAddBy { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property Property { get; set; } = null!;
}
