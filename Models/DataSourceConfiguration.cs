using System;
using System.Collections.Generic;

namespace WebPresDB.Models;

public partial class DataSourceConfiguration
{
    public string DataSource { get; set; } = null!;

    public string? DataSourceTitle { get; set; }

    public string? ExpectedFieldList { get; set; }

    public string? LinkedTableName { get; set; }

    public string? ImportIdfieldName { get; set; }

    public string? ImportIdfieldType { get; set; }

    public string? DbtableName { get; set; }

    public DateTime RecModDate { get; set; }

    public string? DataSourceDesc { get; set; }

    public string? FieldsToIgnoreForDiff { get; set; }

    public string? MatchingFormSql { get; set; }

    public string? DetailsSubformName { get; set; }

    public string? DataFileVerificationWord { get; set; }

    public string? DataFileExtension { get; set; }

    public string? AppendCriteria { get; set; }

    public string? SourceTableName { get; set; }

    public string? AppendFromFieldReplacements { get; set; }

    public string? AppendIntoFieldReplacements { get; set; }

    public virtual ICollection<ImportDataRule> ImportDataRules { get; set; } = new List<ImportDataRule>();
}
