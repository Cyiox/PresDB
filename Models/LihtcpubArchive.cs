using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class LihtcpubArchive
{
    public int ImportId { get; set; }

    public DateTime ImportDateTime { get; set; }

    public int? ExpUsePropertyId { get; set; }

    public string? HudId { get; set; }

    public string? Project { get; set; }

    public string? ProjAdd { get; set; }

    public string? ProjCty { get; set; }

    public string? ProjSt { get; set; }

    public string? ProjZip { get; set; }

    public string? StateId { get; set; }

    public string? Contact { get; set; }

    public string? Company { get; set; }

    public string? CoAdd { get; set; }

    public string? CoCty { get; set; }

    public string? CoSt { get; set; }

    public string? CoZip { get; set; }

    public string? CoTel { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? Reg { get; set; }

    public string? Msa { get; set; }

    public string? Cbsa { get; set; }

    public string? Placece { get; set; }

    public string? Placefp { get; set; }

    public string? Cosubcur { get; set; }

    public string? Fips1990 { get; set; }

    public string? St1990 { get; set; }

    public string? Cnty1990 { get; set; }

    public string? Trct1990 { get; set; }

    public string? Fips2000 { get; set; }

    public string? St2000 { get; set; }

    public string? Cnty2000 { get; set; }

    public string? Trct2000 { get; set; }

    public string? Bg2000 { get; set; }

    public string? Fips2010 { get; set; }

    public string? St2010 { get; set; }

    public string? Cnty2010 { get; set; }

    public string? Trct2010 { get; set; }

    public string? Fips2020 { get; set; }

    public int? Scattered { get; set; }

    public int? Resynd { get; set; }

    public int? Allocamt { get; set; }

    public int? NUnits { get; set; }

    public int? LiUnits { get; set; }

    public int? N0br { get; set; }

    public int? N1br { get; set; }

    public int? N2br { get; set; }

    public int? N3br { get; set; }

    public int? N4br { get; set; }

    public int? NTotal { get; set; }

    public int? IncCeil { get; set; }

    public int? LowCeil { get; set; }

    public int? Ceilunit { get; set; }

    public int? YrPis { get; set; }

    public int? YrAlloc { get; set; }

    public int? NonProf { get; set; }

    public int? Basis { get; set; }

    public int? Bond { get; set; }

    public int? MffRa { get; set; }

    public int? MffRaId { get; set; }

    public int? Fmha514 { get; set; }

    public int? Rdnum514 { get; set; }

    public int? Fmha515 { get; set; }

    public string? Rdnum515 { get; set; }

    public int? Fmha538 { get; set; }

    public int? Rdnum538 { get; set; }

    public int? Home { get; set; }

    public int? HomeAmt { get; set; }

    public int? HomeIdisid { get; set; }

    public int? Tcap { get; set; }

    public int? TcapAmt { get; set; }

    public int? TcapIdisid { get; set; }

    public int? Cdbg { get; set; }

    public int? CdbgAmt { get; set; }

    public int? CdbgIdisid { get; set; }

    public int? Fha { get; set; }

    public int? FhaNum { get; set; }

    public int? Hopevi { get; set; }

    public int? HpviAmt { get; set; }

    public int? Tcep { get; set; }

    public int? TcepAmt { get; set; }

    public int? Rentassist { get; set; }

    public int? TrgtPop { get; set; }

    public int? TrgtFam { get; set; }

    public int? TrgtEld { get; set; }

    public int? TrgtDis { get; set; }

    public int? TrgtHml { get; set; }

    public int? TrgtOther { get; set; }

    public string? TrgtSpc { get; set; }

    public int? Type { get; set; }

    public int? Credit { get; set; }

    public int? NUnitsr { get; set; }

    public int? LiUnitr { get; set; }

    public int? Metro { get; set; }

    public int? Dda { get; set; }

    public int? Qct { get; set; }

    public int? Nonprog { get; set; }

    public string? Datanote { get; set; }

    public string? RecordStatus { get; set; }

    public double? X { get; set; }

    public double? Y { get; set; }

    public double? Z { get; set; }

    public int? Yrmisflg { get; set; }

    public int? Place2010 { get; set; }

    public string? RentAssistContract { get; set; }

    public string? Place1990 { get; set; }

    public string? Place2000 { get; set; }

    public string? Place2020 { get; set; }

    public int? Htf { get; set; }

    public int? HtfAmt { get; set; }

    public int? Rad { get; set; }

    public int? Qozf { get; set; }

    public int? QozfAmt { get; set; }

    public int? NlmReason { get; set; }

    public string? NlmSpc { get; set; }

    public DateTime? RecModDate { get; set; }

    public string? RecModBy { get; set; }

    public virtual Property? ExpUseProperty { get; set; }
}
