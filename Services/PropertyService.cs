using Microsoft.Data.SqlClient;
using System.Data;
using WebPresDB.Models;

namespace WebPresDB.Services;

public class PropertyService : IPropertyService
{
    private readonly string connectionString;
    private readonly ILogger<PropertyService> logger;

    public PropertyService(IConfiguration configuration, ILogger<PropertyService> logger)
    {
        connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default is not configured.");
        this.logger = logger;
    }

    public async Task<PropertySearchPage> SearchPropertiesAsync(
        string searchTerm,
        string city,
        string propertyId,
        int offset,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        searchTerm = searchTerm.Trim();
        city = city.Trim();
        propertyId = propertyId.Trim();

        logger.LogInformation(
            "Property search started. SearchTerm={SearchTerm}, City={City}, PropertyId={PropertyId}, Offset={Offset}, PageSize={PageSize}",
            searchTerm, city, propertyId, offset, pageSize);

        if (string.IsNullOrWhiteSpace(searchTerm) &&
            string.IsNullOrWhiteSpace(city) &&
            string.IsNullOrWhiteSpace(propertyId))
        {
            return new PropertySearchPage(Array.Empty<PropertyModel>(), 0);
        }

        const string sql = """
            SELECT
                PropertyID,
                HUDID,
                PropertyName,
                Street,
                City,
                State,
                Zip,
                property_total_unit_count,
                COUNT(*) OVER() AS TotalCount
            FROM dbo.Properties
            WHERE (@city = '' OR City LIKE '%' + @city + '%')
              AND (@propertyId = '' OR TRY_CONVERT(int, @propertyId) = PropertyID)
              AND (
                    @searchTerm = ''
                    OR PropertyName LIKE '%' + @searchTerm + '%'
                    OR City LIKE '%' + @searchTerm + '%'
                    OR Street LIKE '%' + @searchTerm + '%'
                    OR CONVERT(nvarchar(20), HUDID) LIKE '%' + @searchTerm + '%'
                    OR CONVERT(nvarchar(20), PropertyID) LIKE '%' + @searchTerm + '%'
                  )
            ORDER BY PropertyID
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
            """;

        var properties = new List<PropertyModel>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        logger.LogDebug("SQL Server connection opened for property search.");
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@searchTerm", SqlDbType.NVarChar, 255).Value = searchTerm;
        command.Parameters.Add("@city", SqlDbType.NVarChar, 255).Value = city;
        command.Parameters.Add("@propertyId", SqlDbType.NVarChar, 50).Value = propertyId;
        command.Parameters.Add("@offset", SqlDbType.Int).Value = Math.Max(0, offset);
        command.Parameters.Add("@pageSize", SqlDbType.Int).Value = Math.Clamp(pageSize, 1, 500);

        var totalCount = 0;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            properties.Add(new PropertyModel
            {
                PropertyId = reader.GetInt32(reader.GetOrdinal("PropertyID")),
                HudId = reader.IsDBNull(reader.GetOrdinal("HUDID")) ? null : reader.GetInt32(reader.GetOrdinal("HUDID")),
                Name = reader.IsDBNull(reader.GetOrdinal("PropertyName")) ? "" : reader.GetString(reader.GetOrdinal("PropertyName")),
                Address = reader.IsDBNull(reader.GetOrdinal("Street")) ? "" : reader.GetString(reader.GetOrdinal("Street")),
                City = reader.IsDBNull(reader.GetOrdinal("City")) ? "" : reader.GetString(reader.GetOrdinal("City")),
                State = reader.IsDBNull(reader.GetOrdinal("State")) ? "" : reader.GetString(reader.GetOrdinal("State")),
                Zip = reader.IsDBNull(reader.GetOrdinal("Zip")) ? "" : reader.GetString(reader.GetOrdinal("Zip")),
                TotalUnits = reader.IsDBNull(reader.GetOrdinal("property_total_unit_count")) ? null : reader.GetInt32(reader.GetOrdinal("property_total_unit_count"))
            });
        }

        logger.LogInformation("Property search completed. Returned={Returned}, TotalCount={TotalCount}", properties.Count, totalCount);
        return new PropertySearchPage(properties, totalCount);
    }

    public async Task<PropertyDetailsModel?> GetPropertyDetailsAsync(
        int propertyId,
        CancellationToken cancellationToken = default)
    {
        const string propertySql = """
            SELECT PropertyID, HUDID, PropertyName, Street, City, State, Zip,
                   property_total_unit_count, Units_Assisted, units_at_risk_num, UnitsAtRiskStatus,
                   primary_financing_type, [Comment: Preservation Status, Type or other new Affordability Te]
            FROM dbo.Properties
            WHERE PropertyID = @propertyId;
            """;

        const string contractSql = """
            SELECT ImportID, ImportDateTime, contract_number, tracs_effective_date,
                   tracs_overall_expiration_date, tracs_status_name,
                   assisted_units_count, program_type_name
            FROM dbo.[MF_Assistance_&_Sec8_Contracts_archive]
            WHERE ExpUsePropertyID = @propertyId
            ORDER BY ImportDateTime DESC;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var propertyCommand = new SqlCommand(propertySql, connection);
        propertyCommand.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;

        await using var propertyReader = await propertyCommand.ExecuteReaderAsync(cancellationToken);
        if (!await propertyReader.ReadAsync(cancellationToken))
        {
            return null;
        }

        var property = ReadProperty(propertyReader);
        var details = new PropertyDetailsModel
        {
            Property = property,
            Status = ReadString(propertyReader, "UnitsAtRiskStatus"),
            PrimaryFinancingType = ReadString(propertyReader, "primary_financing_type"),
            UnitsAssisted = ReadNullableInt(propertyReader, "Units_Assisted"),
            UnitsAtRisk = ReadNullableInt(propertyReader, "units_at_risk_num"),
            Comments = ReadString(propertyReader, "Comment: Preservation Status, Type or other new Affordability Te")
        };
        await propertyReader.CloseAsync();

        await using var contractCommand = new SqlCommand(contractSql, connection);
        contractCommand.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;
        var contracts = new List<PropertyContractModel>();
        await using var contractReader = await contractCommand.ExecuteReaderAsync(cancellationToken);
        while (await contractReader.ReadAsync(cancellationToken))
        {
            contracts.Add(new PropertyContractModel
            {
                ImportId = contractReader.GetInt32(contractReader.GetOrdinal("ImportID")),
                ImportDateTime = ReadNullableDate(contractReader, "ImportDateTime"),
                ContractNumber = ReadString(contractReader, "contract_number"),
                EffectiveDate = ReadNullableDate(contractReader, "tracs_effective_date"),
                ExpirationDate = ReadNullableDate(contractReader, "tracs_overall_expiration_date"),
                Status = ReadString(contractReader, "tracs_status_name"),
                AssistedUnits = ReadNullableInt(contractReader, "assisted_units_count"),
                ProgramType = ReadString(contractReader, "program_type_name")
            });
        }

        return new PropertyDetailsModel
        {
            Property = details.Property,
            Status = details.Status,
            PrimaryFinancingType = details.PrimaryFinancingType,
            UnitsAssisted = details.UnitsAssisted,
            UnitsAtRisk = details.UnitsAtRisk,
            Comments = details.Comments,
            Contracts = contracts
        };
    }

    private static PropertyModel ReadProperty(SqlDataReader reader) => new()
    {
        PropertyId = reader.GetInt32(reader.GetOrdinal("PropertyID")),
        HudId = ReadNullableInt(reader, "HUDID"),
        Name = ReadString(reader, "PropertyName"),
        Address = ReadString(reader, "Street"),
        City = ReadString(reader, "City"),
        State = ReadString(reader, "State"),
        Zip = ReadString(reader, "Zip"),
        TotalUnits = ReadNullableInt(reader, "property_total_unit_count")
    };

    private static string ReadString(SqlDataReader reader, string name)
        => reader.IsDBNull(reader.GetOrdinal(name)) ? string.Empty : reader.GetString(reader.GetOrdinal(name));

    private static int? ReadNullableInt(SqlDataReader reader, string name)
        => reader.IsDBNull(reader.GetOrdinal(name)) ? null : reader.GetInt32(reader.GetOrdinal(name));

    private static DateTime? ReadNullableDate(SqlDataReader reader, string name)
        => reader.IsDBNull(reader.GetOrdinal(name)) ? null : reader.GetDateTime(reader.GetOrdinal(name));
}