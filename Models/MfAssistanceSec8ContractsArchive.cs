using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class MfAssistanceSec8ContractsArchive
{
    public int ImportId { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? ContractNumber { get; set; }

    public string? PropertyId { get; set; }

    public string? PropertyNameText { get; set; }

    public DateTime? TracsEffectiveDate { get; set; }

    public DateTime? TracsOverallExpirationDate { get; set; }

    public int? TracsOverallExpFiscalYear { get; set; }

    public string? TracsOverallExpireQuarter { get; set; }

    public DateTime? TracsCurrentExpirationDate { get; set; }

    public string? TracsStatusName { get; set; }

    public int? ContractTermMonthsQty { get; set; }

    public int? AssistedUnitsCount { get; set; }

    public string? IsHudAdministeredInd { get; set; }

    public string? IsAccOldInd { get; set; }

    public string? IsAccPerformanceBasedInd { get; set; }

    public string? ContractDocTypeCode { get; set; }

    public string? ProgramTypeName { get; set; }

    public string? ProgramTypeGroupCode { get; set; }

    public string? ProgramTypeGroupName { get; set; }

    public double? RentToFmrRatio { get; set; }

    public string? RentToFmrDescription { get; set; }

    public int? _0brCount { get; set; }

    public int? _1brCount { get; set; }

    public int? _2brCount { get; set; }

    public int? _3brCount { get; set; }

    public int? _4brCount { get; set; }

    public int? _5plusBrCount { get; set; }

    public int? _0brFmr { get; set; }

    public int? _1brFmr { get; set; }

    public int? _2brFmr { get; set; }

    public int? _3brFmr { get; set; }

    public int? _4brFmr { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
