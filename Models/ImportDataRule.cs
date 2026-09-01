using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class ImportDataRule
{
    public int Id { get; set; }

    public string DataSource { get; set; } = null!;

    public DateTime ReviewAsOf { get; set; }

    public string FieldName { get; set; } = null!;

    public string IssueDecription { get; set; } = null!;

    public string? AutoProcessInstruction { get; set; }

    public bool? ShowOnPropertiesForm { get; set; }

    public virtual DataSourceConfiguration DataSourceNavigation { get; set; } = null!;

    public virtual ICollection<ImportDataIssue> ImportDataIssues { get; set; } = new List<ImportDataIssue>();
}
