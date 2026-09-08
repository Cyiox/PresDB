using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class DhcdArchive
{
    public int ImportId { get; set; }

    public string? ProjId { get; set; }

    public int? OldProjId { get; set; }

    public string? BorrowerSponsor { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectAddress { get; set; }

    public string? City { get; set; }

    public string? ZipCode4 { get; set; }

    public int? NinePctAwarded { get; set; }

    public decimal? AhtfAwarded { get; set; }

    public decimal? FourPctAwarded { get; set; }

    public int? StateLihtcAwarded { get; set; }

    public int? CatnhpAwarded { get; set; }

    public int? CipfAwarded { get; set; }

    public decimal? CbhAwarded { get; set; }

    public decimal? HifAwarded { get; set; }

    public decimal? HomeAwarded { get; set; }

    public decimal? HsfAwarded { get; set; }

    public int? TodAwarded { get; set; }

    public int? TcapAwarded { get; set; }

    public decimal? FcfDdsAwarded { get; set; }

    public decimal? FcfDmhAwarded { get; set; }

    public int? HpstfAwarded { get; set; }

    public int? TcxAwarded { get; set; }

    public int? NumberOfExtremelyLowIncomeUnits { get; set; }

    public DateTime? AcquisitionCreditPisDate { get; set; }

    public int? NumberOfAffordableUnits { get; set; }

    public int? NumberOfTotalUnits { get; set; }

    public DateTime? RehabCreditPisDate { get; set; }

    public DateTime? DateCatnhpLoanClosed { get; set; }

    public DateTime? DateCipfLoanClosed { get; set; }

    public DateTime? DateHifLoanClosed { get; set; }

    public DateTime? DateHomeLoanClosed { get; set; }

    public DateTime? DateHsfLoanClosed { get; set; }

    public DateTime? DateCbhLoanClosed { get; set; }

    public DateTime? DateFcfDdsLoanClosed { get; set; }

    public DateTime? DateFcfDmhLoanClosed { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public int? Pre2016Total { get; set; }

    public DateTime? Pre2016Pis { get; set; }

    public int? Pre2016SlihtcState { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public decimal? NhtfAwarded { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
