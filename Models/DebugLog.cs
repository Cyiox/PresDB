using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class DebugLog
{
    public int Id { get; set; }

    public string? ItemName { get; set; }

    public DateTime? ItemDate { get; set; }

    public string? ItemDescription { get; set; }

    public string? User { get; set; }
}
