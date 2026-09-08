using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class MassHousingBenedictArchive
{
    public int ImportId { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? ProjId { get; set; }

    public string? LoanName { get; set; }

    public string? LoanNumber { get; set; }

    public string? LoanType { get; set; }

    public DateTime? ClosingDate { get; set; }

    public DateTime? FirstDueDate { get; set; }

    public DateTime? MaturityDate { get; set; }

    public decimal? OrgprinbalCalculated { get; set; }

    public decimal? Curprinbal { get; set; }

    public string? Taxtyp { get; set; }

    public DateTime? Mthenddt { get; set; }

    public string? Loanstat { get; set; }

    public bool? XMatch { get; set; }

    public DateTime? XExportDateTime { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
