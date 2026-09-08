using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class Tbl40Tdocument
{
    public int Id { get; set; }

    public int? PropertyId { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? DateReceivedatCedac { get; set; }

    public DateTime? DateMailed { get; set; }

    public string? NoticeType { get; set; }

    public string? Notes { get; set; }

    public bool? DhcdConfirm { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectAddress { get; set; }

    public string? ProjectCity { get; set; }

    public int? TotalUnits { get; set; }

    public int? AffordableUnits { get; set; }
}
