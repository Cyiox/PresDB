using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class MhpArchive
{
    public int ImportId { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? MhpId { get; set; }

    public string? DoNotModifyOpportunity { get; set; }

    public string? DoNotModifyRowChecksum { get; set; }

    public DateTime? DoNotModifyModifiedOn { get; set; }

    public string? ProjectName { get; set; }

    public string? Account { get; set; }

    public double? LoanAmount { get; set; }

    public string? MhpStatus { get; set; }

    public DateTime? MhpClosingDate { get; set; }

    public string? _1Beds { get; set; }

    public int? _2Beds { get; set; }

    public int? _3Beds { get; set; }

    public int? _30AmiUnits { get; set; }

    public string? _4Beds { get; set; }

    public string? _50AmiUnits { get; set; }

    public int? _60AmiUnits { get; set; }

    public string? _80AmiUnits { get; set; }

    public int? AffordableUnits { get; set; }

    public string? Community { get; set; }

    public string? ProjectAddress { get; set; }

    public string? Sros { get; set; }

    public string? X20201015TaxCredit { get; set; }

    public int? Units { get; set; }

    public string? Unrestricted { get; set; }

    public string? Type { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public string? RecordType { get; set; }

    public string? _100AmiUnits { get; set; }

    public string? ProjectCharacteristics { get; set; }

    public bool ProjChar40b { get; set; }

    public bool ProjChar40r { get; set; }

    public bool ProjChar5plus5 { get; set; }

    public bool ProjCharCondo { get; set; }

    public bool ProjCharEquityTakeout { get; set; }

    public bool ProjCharGhhprospect { get; set; }

    public bool ProjCharInHouseClosing { get; set; }

    public bool ProjCharLongTermLoan { get; set; }

    public bool ProjCharMissionCritical { get; set; }

    public bool ProjCharNewStabilizeAtRisk { get; set; }

    public bool ProjCharOneSource { get; set; }

    public bool ProjCharPreservation { get; set; }

    public bool ProjCharRap { get; set; }

    public bool ProjCharSmallLoan { get; set; }

    public bool ProjCharTaxCredit { get; set; }

    public string? Stage { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
