using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class MtgAArchive
{
    public int ImportId { get; set; }

    public DateTime? ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? HudProjectNumber { get; set; }

    public string? PremiseId { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyStreet { get; set; }

    public string? PropertyCity { get; set; }

    public string? PropertyState { get; set; }

    public string? PropertyZip { get; set; }

    public int? Units { get; set; }

    public DateTime? InitialEndorsementDate { get; set; }

    public DateTime? FinalEndorsementDate { get; set; }

    public double? OriginalMortgageAmount { get; set; }

    public DateTime? FirstPaymentDate { get; set; }

    public DateTime? MaturityDate { get; set; }

    public string? TermInMonths { get; set; }

    public double? InterestRate { get; set; }

    public decimal? CurrentPrincipalAndInterest { get; set; }

    public double? AmortizedPrincipalBalance { get; set; }

    public string? HolderName { get; set; }

    public string? HolderCity { get; set; }

    public string? HolderState { get; set; }

    public string? ServicerName { get; set; }

    public string? ServicerCity { get; set; }

    public string? ServicerState { get; set; }

    public string? SectionOfActCode { get; set; }

    public string? SoaCategorySubCategory { get; set; }

    public string? Te { get; set; }

    public string? Tc { get; set; }

    public bool? Ximported { get; set; }

    public DateTime? XarchiveDateTime { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
