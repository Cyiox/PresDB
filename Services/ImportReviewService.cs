using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebPresDB.Models;

namespace WebPresDB.Services;

public sealed class ImportReviewService(
    PreservationTestContext db,
    ILogger<ImportReviewService> logger) : IImportReviewService
{
    public async Task<IReadOnlyList<ImportReviewSource>> GetSourcesAsync(CancellationToken cancellationToken = default)
    {
        var configurations = await db.DataSourceConfigurations.AsNoTracking()
            .Where(x => x.DbtableName != null && x.ImportIdfieldName != null)
            .OrderBy(x => x.DataSourceTitle ?? x.DataSource)
            .ToListAsync(cancellationToken);
        var result = new List<ImportReviewSource>();
        foreach (var configuration in configurations)
        {
            var columns = await GetColumnsAsync(configuration.DbtableName!, cancellationToken);
            if (!columns.Contains("ImportID") || !columns.Contains("ImportDateTime") ||
                !columns.Contains("ExpUsePropertyID") || !columns.Contains(configuration.ImportIdfieldName!))
                continue;
            var table = QuoteIdentifier(configuration.DbtableName!);
            var dateColumn = QuoteIdentifier("ImportDateTime");
            var (latestDate, count) = await ExecuteSingleAsync(
                $"SELECT MAX({dateColumn}), COUNT(*) FROM dbo.{table} WHERE {dateColumn} = (SELECT MAX({dateColumn}) FROM dbo.{table}) AND ExpUsePropertyID IS NULL",
                null, cancellationToken);
            result.Add(new ImportReviewSource
            {
                DataSource = configuration.DataSource,
                Title = configuration.DataSource.Equals("DHCD", StringComparison.OrdinalIgnoreCase)
                    ? "EOHLC"
                    : configuration.DataSourceTitle ?? configuration.DataSource,
                TableName = configuration.DbtableName!,
                LatestImportDate = latestDate is DBNull ? null : (DateTime?)latestDate,
                UnmatchedCount = count is DBNull ? 0 : Convert.ToInt32(count)
            });
        }
        return result;
    }

    public async Task<IReadOnlyList<ImportReviewRow>> GetUnmatchedAsync(
        string dataSource, CancellationToken cancellationToken = default)
    {
        var configuration = await GetConfigurationAsync(dataSource, cancellationToken);
        var table = QuoteIdentifier(configuration.DbtableName!);
        var id = QuoteIdentifier("ImportID");
        var externalId = QuoteIdentifier(configuration.ImportIdfieldName!);
        var date = QuoteIdentifier("ImportDateTime");
        var columns = await GetColumnsAsync(configuration.DbtableName!, cancellationToken);
        var name = SelectColumn(columns, "PropertyName", "property_name_text", "project", "ProjectName",
            "Project Name", "PROPERTY NAME", "Project Name (Project_Master)", "loan name");
        var street = SelectColumn(columns, "Street", "address_line1_text", "proj_add", "Project Address",
            "PROPERTY STREET", "Main_Address_Line1", "Project Address", "Site Address (Site Address)");
        var city = SelectColumn(columns, "City", "city_name_text", "proj_cty", "PROPERTY CITY", "City",
            "Community", "City Town Name (City Town -> Site Address)");
        var state = SelectColumn(columns, "State", "state_code", "proj_st", "PROPERTY STATE",
            "State_Abbreviation", "State (Site Address)");
        var zip = SelectColumn(columns, "Zip", "zip_code", "proj_zip", "PROPERTY ZIP", "Zip_Code",
            "Zip Code +4", "Zip Code 1 (Site Address)");

        var latest = await GetLatestDateAsync(table, date, cancellationToken);
        if (latest is null)
            return [];

        await EnsureReviewTableAsync(cancellationToken);
        var sql = $"""
            SELECT a.{id}, a.{date}, a.{externalId} AS ExternalId, {SelectOrNull(name)} AS PropertyName,
                   {SelectOrNull(street)} AS Street, {SelectOrNull(city)} AS City,
                   {SelectOrNull(state)} AS State, {SelectOrNull(zip)} AS Zip,
                   a.ExpUsePropertyID
            FROM dbo.{table} a
            WHERE a.{date} = @date AND a.ExpUsePropertyID IS NULL
              AND NOT EXISTS (
                  SELECT 1 FROM dbo.ImportReviewDecision d
                  WHERE d.DataSource = @source AND d.ImportId = a.{id} AND d.Decision = 'Ignore')
            ORDER BY PropertyName, a.{id}
            """;

        var rows = new List<ImportReviewRow>();
        await using var command = await CreateCommandAsync(sql, cancellationToken);
        command.Parameters.Add(new SqlParameter("@date", latest.Value));
        command.Parameters.Add(new SqlParameter("@source", dataSource));
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            rows.Add(new ImportReviewRow
            {
                ImportId = Convert.ToInt32(reader.GetValue(0)),
                ImportDateTime = reader.GetDateTime(1),
                ExternalId = ReadString(reader, 2),
                PropertyName = ReadString(reader, 3),
                Street = ReadString(reader, 4),
                City = ReadString(reader, 5),
                State = ReadString(reader, 6),
                Zip = ReadString(reader, 7)
            });
        }
        return rows;
    }

    public async Task LinkAsync(string dataSource, int importId, int propertyId, CancellationToken cancellationToken = default)
    {
        if (!await db.Properties.AsNoTracking().AnyAsync(x => x.PropertyId == propertyId, cancellationToken))
            throw new InvalidOperationException($"Property {propertyId} does not exist.");
        var configuration = await GetConfigurationAsync(dataSource, cancellationToken);
        var sql = $"UPDATE dbo.{QuoteIdentifier(configuration.DbtableName!)} SET ExpUsePropertyID = @propertyId WHERE {QuoteIdentifier("ImportID")} = @importId";
        var affected = await ExecuteNonQueryAsync(sql, [new("@propertyId", propertyId), new("@importId", importId)], cancellationToken);
        if (affected != 1)
            throw new InvalidOperationException($"Import row {importId} could not be linked.");
    }

    public async Task<int> CreatePropertyAndLinkAsync(string dataSource, ImportReviewRow row, string user, CancellationToken cancellationToken = default)
    {
        await GetConfigurationAsync(dataSource, cancellationToken);
        var property = new Property
        {
            PropertyName = row.PropertyName,
            Street = row.Street,
            City = row.City,
            State = row.State,
            Zip = row.Zip,
            RecAddDate = DateTime.UtcNow,
            RecAddBy = user
        };
        db.Properties.Add(property);
        await db.SaveChangesAsync(cancellationToken);
        await LinkAsync(dataSource, row.ImportId, property.PropertyId, cancellationToken);
        return property.PropertyId;
    }

    public async Task IgnoreAsync(string dataSource, int importId, string user, CancellationToken cancellationToken = default)
    {
        await EnsureReviewTableAsync(cancellationToken);
        await ExecuteNonQueryAsync(
            "INSERT INTO dbo.ImportReviewDecision (DataSource, ImportId, Decision, DecisionDate, DecisionBy) VALUES (@source, @id, 'Ignore', SYSUTCDATETIME(), @user)",
            [new("@source", dataSource), new("@id", importId), new("@user", user)], cancellationToken);
    }

    public async Task<int> ImportCsvAsync(
        string dataSource,
        Stream file,
        string fileName,
        string user,
        CancellationToken cancellationToken = default)
    {
        if (!fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("New imports currently require a CSV file.");

        var configuration = await GetConfigurationAsync(dataSource, cancellationToken);
        var tableName = configuration.DbtableName!;
        var columns = await GetColumnsAsync(tableName, cancellationToken);
        using var reader = new StreamReader(file, Encoding.UTF8, leaveOpen: true);
        var header = ParseCsvLine(await reader.ReadLineAsync(cancellationToken) ?? string.Empty);
        var importColumns = header
            .Select((value, index) => (Name: value.Trim(), Index: index))
            .Where(x => !string.IsNullOrWhiteSpace(x.Name))
            .ToArray();
        if (importColumns.Length == 0 || importColumns.Any(x => !columns.Contains(x.Name)))
            throw new InvalidOperationException("The CSV headers do not match columns in the configured archive table.");

        var blocked = new[] { "ImportID", "ImportDateTime", "ExpUsePropertyID" };
        if (importColumns.Any(x => blocked.Contains(x.Name, StringComparer.OrdinalIgnoreCase)))
            throw new InvalidOperationException("ImportID, ImportDateTime, and ExpUsePropertyID are managed by the application and must not be uploaded.");

        var quotedColumns = string.Join(", ", importColumns.Select(x => QuoteIdentifier(x.Name)));
        var parameterNames = string.Join(", ", importColumns.Select((_, index) => $"@p{index}"));
        var sql = $"INSERT INTO dbo.{QuoteIdentifier(tableName)} ({quotedColumns}, [ImportDateTime]) VALUES ({parameterNames}, @importDate)";
        var count = 0;
        await using var command = await CreateCommandAsync(sql, cancellationToken);
        command.Parameters.AddRange(importColumns.Select((_, index) => new SqlParameter($"@p{index}", SqlDbType.NVarChar, -1)).ToArray());
        command.Parameters.Add(new SqlParameter("@importDate", DateTime.UtcNow));
        string? line;
        while ((line = await reader.ReadLineAsync(cancellationToken)) is not null)
        {
            var values = ParseCsvLine(line);
            if (values.All(string.IsNullOrWhiteSpace))
                continue;
            for (var index = 0; index < importColumns.Length; index++)
                command.Parameters[$"@p{index}"].Value = index < values.Count && !string.IsNullOrEmpty(values[index])
                    ? values[index] : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
            count++;
        }

        logger.LogInformation("Imported {Count} rows into {Table} for {User}.", count, tableName, user);
        return count;
    }

    private async Task<DataSourceConfiguration> GetConfigurationAsync(string dataSource, CancellationToken cancellationToken) =>
        await db.DataSourceConfigurations.AsNoTracking().SingleOrDefaultAsync(x => x.DataSource == dataSource, cancellationToken)
        ?? throw new InvalidOperationException($"Unknown data source '{dataSource}'.");

    private async Task<DateTime?> GetLatestDateAsync(string table, string date, CancellationToken cancellationToken)
    {
        var value = await ExecuteScalarAsync($"SELECT MAX({date}) FROM dbo.{table} WHERE {date} IS NOT NULL", null, cancellationToken);
        return value is DateTime dateValue ? dateValue : null;
    }

    private async Task<HashSet<string>> GetColumnsAsync(string table, CancellationToken cancellationToken)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await using var command = await CreateCommandAsync(
            "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID(@table)", cancellationToken);
        command.Parameters.Add(new SqlParameter("@table", $"dbo.{table}"));
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            result.Add(reader.GetString(0));
        return result;
    }

    private async Task EnsureReviewTableAsync(CancellationToken cancellationToken) =>
        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'dbo.ImportReviewDecision', N'U') IS NULL
            BEGIN
                CREATE TABLE dbo.ImportReviewDecision (
                    DataSource NVARCHAR(255) NOT NULL,
                    ImportId INT NOT NULL,
                    Decision NVARCHAR(20) NOT NULL,
                    DecisionDate DATETIME2 NOT NULL,
                    DecisionBy NVARCHAR(255) NULL,
                    CONSTRAINT PK_ImportReviewDecision PRIMARY KEY (DataSource, ImportId)
                );
            END
            """, cancellationToken);

    private async Task<SqlCommand> CreateCommandAsync(string sql, CancellationToken cancellationToken)
    {
        var connection = (SqlConnection)db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);
        return new SqlCommand(sql, connection);
    }

    private async Task<object?> ExecuteScalarAsync(string sql, IEnumerable<SqlParameter>? parameters, CancellationToken cancellationToken)
    {
        await using var command = await CreateCommandAsync(sql, cancellationToken);
        if (parameters is not null) command.Parameters.AddRange(parameters.ToArray());
        return await command.ExecuteScalarAsync(cancellationToken);
    }

    private async Task<(object?, object?)> ExecuteSingleAsync(string sql, IEnumerable<SqlParameter>? parameters, CancellationToken cancellationToken)
    {
        await using var command = await CreateCommandAsync(sql, cancellationToken);
        if (parameters is not null) command.Parameters.AddRange(parameters.ToArray());
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);
        return (reader.GetValue(0), reader.GetValue(1));
    }

    private async Task<int> ExecuteNonQueryAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken)
    {
        await using var command = await CreateCommandAsync(sql, cancellationToken);
        command.Parameters.AddRange(parameters.ToArray());
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static string? SelectColumn(HashSet<string> columns, params string[] names) =>
        names.FirstOrDefault(columns.Contains);

    private static string SelectOrNull(string? column) => column is null ? "NULL" : $"a.{QuoteIdentifier(column)}";
    private static string QuoteIdentifier(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException("Database identifier cannot be empty.")
            : $"[{value.Replace("]", "]]")}]";
    private static string? ReadString(IDataRecord reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToString(reader.GetValue(ordinal));

    private static IReadOnlyList<string> ParseCsvLine(string line)
    {
        var values = new List<string>();
        var value = new StringBuilder();
        var quoted = false;
        for (var i = 0; i < line.Length; i++)
        {
            var character = line[i];
            if (character == '"' && quoted && i + 1 < line.Length && line[i + 1] == '"')
            {
                value.Append('"');
                i++;
            }
            else if (character == '"')
                quoted = !quoted;
            else if (character == ',' && !quoted)
            {
                values.Add(value.ToString());
                value.Clear();
            }
            else
                value.Append(character);
        }
        values.Add(value.ToString());
        return values;
    }
}
