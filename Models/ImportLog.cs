using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class ImportLog
{
    public int ImportId { get; set; }

    public string? ImportName { get; set; }

    public DateTime? ImportDate { get; set; }

    public string? ImportResults { get; set; }

    public string? User { get; set; }
}
