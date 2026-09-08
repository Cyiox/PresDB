using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class ImportDataIssue
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public DateTime? IssueDate { get; set; }

    public int DataRuleId { get; set; }

    public string FromFieldValue { get; set; } = null!;

    public string ToFieldValue { get; set; } = null!;

    public DateTime FromImportDate { get; set; }

    public DateTime ToImportDate { get; set; }

    public string? AutoProcessInstructionExecuted { get; set; }

    public virtual ImportDataRule DataRule { get; set; } = null!;
}
