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
        var connectionName = configuration["Database:ConnectionStringName"] ?? "Default";
        connectionString = configuration.GetConnectionString(connectionName)
            ?? throw new InvalidOperationException($"ConnectionStrings:{connectionName} is not configured.");
        this.logger = logger;
    }

    public async Task<PropertySearchPage> SearchPropertiesAsync(
        string searchTerm,
        string city,
        string propertyId,
        IReadOnlyCollection<string> hudProgramTypeGroupCodes,
        int offset,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        searchTerm = searchTerm.Trim();
        city = city.Trim();
        propertyId = propertyId.Trim();
        var selectedHudCodes = hudProgramTypeGroupCodes
            .Where(code => code is "PRAC" or "202")
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        logger.LogInformation(
            "Property search started. SearchTerm={SearchTerm}, City={City}, PropertyId={PropertyId}, HUDCodes={HUDCodes}, Offset={Offset}, PageSize={PageSize}",
            searchTerm, city, propertyId, selectedHudCodes, offset, pageSize);

        if (string.IsNullOrWhiteSpace(searchTerm) &&
            string.IsNullOrWhiteSpace(city) &&
            string.IsNullOrWhiteSpace(propertyId) &&
            selectedHudCodes.Length == 0)
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
                            AND (
                                        (@filterPrac = 0 AND @filter202 = 0)
                                        OR EXISTS (
                                                SELECT 1
                                                FROM dbo.[MF_Assistance_&_Sec8_Contracts_archive] AS hudContract
                                                WHERE hudContract.ExpUsePropertyID = Properties.PropertyID
                                                    AND ((@filterPrac = 1 AND hudContract.program_type_group_code LIKE '%PRAC%')
                                                             OR (@filter202 = 1 AND hudContract.program_type_group_code LIKE '%202%'))
                                        )
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
        command.Parameters.Add("@filterPrac", SqlDbType.Bit).Value = selectedHudCodes.Contains("PRAC", StringComparer.OrdinalIgnoreCase);
        command.Parameters.Add("@filter202", SqlDbType.Bit).Value = selectedHudCodes.Contains("202", StringComparer.OrdinalIgnoreCase);
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

             const string archiveSql = """
                 SELECT TOP (5) ImportDateTime, HUD_ID AS HudId, project AS Project, company AS Company,
                     n_total AS TotalUnits, li_units AS LihtcUnits, li_unitr AS LowIncomeUnits,
                     yr_alloc AS AllocationYear, yr_pis AS PlacedInServiceYear, credit AS CreditType,
                     bond AS BondFinancing, rentassist AS RentAssistance
                 FROM dbo.LIHTCPUB_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;

                     SELECT TOP (5) ImportDateTime, [Proj ID] AS ProjectId, ProjectName, [Borrower Sponsor] AS BorrowerSponsor,
                         [Number of Affordable Units] AS AffordableUnits, [Number of Total Units] AS TotalUnits,
                         [Nine Pct Awarded] AS NinePctAwarded, [Four Pct Awarded] AS FourPctAwarded,
                         [State LIHTC Awarded] AS StateLihtcAwarded, [Acquisition Credit PIS Date] AS AcquisitionCreditPisDate,
                         [Rehab Credit PIS Date] AS RehabCreditPisDate
                 FROM dbo.DHCD_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;

                 SELECT TOP (5) ImportDateTime, [HUD PROJECT NUMBER] AS HudProjectNumber, [PREMISE ID] AS PremiseId,
                     UNITS AS Units, [INITIAL ENDORSEMENT DATE] AS InitialEndorsementDate,
                     [FINAL ENDORSEMENT DATE] AS FinalEndorsementDate, [ORIGINAL MORTGAGE AMOUNT] AS OriginalMortgageAmount,
                     [MATURITY DATE] AS MaturityDate, [SECTION OF ACT CODE] AS SectionOfActCode
                 FROM dbo.mtg_a_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;

                 SELECT TOP (5) ImportDateTime, [HUD PROJECT NUMBER] AS HudProjectNumber, [PREMISE ID] AS PremiseId,
                     UNITS AS Units, [INITIAL ENDORSEMENT DATE] AS InitialEndorsementDate,
                     [ORIGINAL MORTGAGE AMOUNT] AS OriginalMortgageAmount, [MATURITY DATE] AS MaturityDate,
                     [SECTION OF ACT CODE] AS SectionOfActCode, Status, TERM_DATE AS TermDate
                 FROM dbo.mtg_t_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;

                     SELECT TOP (5) ImportDateTime, [Project_ID] AS ProjectId, [Project_Name] AS ProjectName,
                         [Project_Size] AS ProjectSize, [Rental_Assistance_Units] AS RentalAssistanceUnits,
                         [Date_Tax_Credit_Expires] AS TaxCreditExpires, [Date_Restrictive_Clause_Expires] AS RestrictiveClauseExpires
                 FROM dbo.USDA_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;

                     SELECT TOP (5) ImportDateTime, MHP_ID AS MhpId, [Project Name] AS ProjectName, [Loan Amount] AS LoanAmount,
                         [MHP Status] AS Status, [MHP Closing Date] AS ClosingDate, [Affordable Units] AS AffordableUnits,
                         Units AS TotalUnits, Stage
                 FROM dbo.MHP_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;

                 SELECT TOP (5) ImportDateTime, [Project Id (Project_Master)] AS ProjectId,
                     [Hud Project Number (Project_Master)] AS HudProjectNumber,
                     [Project Name (Project_Master)] AS ProjectName, [FOA Total Unit Count] AS TotalUnits,
                     [LIHTC Units (Project_Master)] AS LihtcUnits, [Rental Units Sec8 (Project_Master)] AS Section8Units,
                     [Principal Program (Project_Master)] AS PrincipalProgram,
                     [Mortgage Insurance Desc] AS MortgageInsurance
                 FROM dbo.MassHousing_FOA_archive WHERE ExpUsePropertyID = @propertyId ORDER BY ImportDateTime DESC;
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
        await contractReader.CloseAsync();

        await using var archiveCommand = new SqlCommand(archiveSql, connection);
        archiveCommand.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;
        var archiveData = await ReadArchiveDataAsync(archiveCommand, cancellationToken);

        const string commentHistorySql = """
            SELECT EditedAt, EditedBy, CommentText
            FROM dbo.PropertyCommentHistory
            WHERE PropertyID = @propertyId
            ORDER BY EditedAt DESC;
            """;
        await using var historyCommand = new SqlCommand(commentHistorySql, connection);
        historyCommand.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;
        var commentHistory = new List<PropertyCommentHistoryModel>();
        await using var historyReader = await historyCommand.ExecuteReaderAsync(cancellationToken);
        while (await historyReader.ReadAsync(cancellationToken))
        {
            commentHistory.Add(new PropertyCommentHistoryModel
            {
                EditedAt = historyReader.GetDateTime(historyReader.GetOrdinal("EditedAt")),
                EditedBy = ReadString(historyReader, "EditedBy"),
                CommentText = ReadString(historyReader, "CommentText")
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
            Contracts = contracts,
            LihtcRecords = archiveData.LihtcRecords,
            DhcdRecords = archiveData.DhcdRecords,
            HudActMortgages = archiveData.HudActMortgages,
            HudTerminatedMortgages = archiveData.HudTerminatedMortgages,
            UsdaRecords = archiveData.UsdaRecords,
            MhpRecords = archiveData.MhpRecords,
            FoaRecords = archiveData.FoaRecords,
            CommentHistory = commentHistory
        };
    }

    private static async Task<PropertyArchiveData> ReadArchiveDataAsync(SqlCommand command, CancellationToken cancellationToken)
    {
        var result = new PropertyArchiveData();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            result.LihtcRecords.Add(new LihtcDetailsModel
            {
                ImportDateTime = ReadNullableDate(reader, "ImportDateTime"), HudId = ReadString(reader, "HudId"),
                Project = ReadString(reader, "Project"), Company = ReadString(reader, "Company"),
                TotalUnits = ReadNullableInt(reader, "TotalUnits"), LihtcUnits = ReadNullableInt(reader, "LihtcUnits"),
                LowIncomeUnits = ReadNullableInt(reader, "LowIncomeUnits"), AllocationYear = ReadNullableInt(reader, "AllocationYear"),
                PlacedInServiceYear = ReadNullableInt(reader, "PlacedInServiceYear"), CreditType = ReadNullableInt(reader, "CreditType"),
                BondFinancing = ReadNullableInt(reader, "BondFinancing"), RentAssistance = ReadNullableInt(reader, "RentAssistance")
            });
        }

        await reader.NextResultAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.DhcdRecords.Add(new DhcdDetailsModel
            {
                ImportDateTime = ReadNullableDate(reader, "ImportDateTime"), ProjectId = ReadString(reader, "ProjectId"),
                ProjectName = ReadString(reader, "ProjectName"), BorrowerSponsor = ReadString(reader, "BorrowerSponsor"),
                AffordableUnits = ReadNullableInt(reader, "AffordableUnits"), TotalUnits = ReadNullableInt(reader, "TotalUnits"),
                NinePercentAwarded = ReadNullableDecimal(reader, "NinePctAwarded"), FourPercentAwarded = ReadNullableDecimal(reader, "FourPctAwarded"),
                StateLihtcAwarded = ReadNullableDecimal(reader, "StateLihtcAwarded"), AcquisitionPisDate = ReadNullableDate(reader, "AcquisitionCreditPisDate"),
                RehabPisDate = ReadNullableDate(reader, "RehabCreditPisDate")
            });
        }

        await reader.NextResultAsync(cancellationToken);
        await ReadMortgagesAsync(reader, result.HudActMortgages, false, cancellationToken);
        await reader.NextResultAsync(cancellationToken);
        await ReadMortgagesAsync(reader, result.HudTerminatedMortgages, true, cancellationToken);

        await reader.NextResultAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.UsdaRecords.Add(new UsdaDetailsModel
            {
                ImportDateTime = ReadNullableDate(reader, "ImportDateTime"), ProjectId = ReadString(reader, "ProjectId"),
                ProjectName = ReadString(reader, "ProjectName"), ProjectSize = ReadNullableInt(reader, "ProjectSize"),
                RentalAssistanceUnits = ReadNullableInt(reader, "RentalAssistanceUnits"), TaxCreditExpires = ReadNullableDate(reader, "TaxCreditExpires"),
                RestrictiveClauseExpires = ReadNullableDate(reader, "RestrictiveClauseExpires")
            });
        }

        await reader.NextResultAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.MhpRecords.Add(new MhpDetailsModel
            {
                ImportDateTime = ReadNullableDate(reader, "ImportDateTime"), MhpId = ReadString(reader, "MhpId"),
                ProjectName = ReadString(reader, "ProjectName"), LoanAmount = ReadNullableDouble(reader, "LoanAmount"),
                Status = ReadString(reader, "Status"), ClosingDate = ReadNullableDate(reader, "ClosingDate"),
                AffordableUnits = ReadNullableInt(reader, "AffordableUnits"), TotalUnits = ReadNullableInt(reader, "TotalUnits"), Stage = ReadString(reader, "Stage")
            });
        }

        await reader.NextResultAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.FoaRecords.Add(new FoaDetailsModel
            {
                ImportDateTime = ReadNullableDate(reader, "ImportDateTime"), ProjectId = ReadString(reader, "ProjectId"),
                HudProjectNumber = ReadString(reader, "HudProjectNumber"), ProjectName = ReadString(reader, "ProjectName"),
                TotalUnits = ReadNullableInt(reader, "TotalUnits"), LihtcUnits = ReadNullableInt(reader, "LihtcUnits"),
                Section8Units = ReadNullableInt(reader, "Section8Units"), PrincipalProgram = ReadString(reader, "PrincipalProgram"),
                MortgageInsurance = ReadString(reader, "MortgageInsurance")
            });
        }

        return result;
    }

    private static async Task ReadMortgagesAsync(SqlDataReader reader, List<MortgageDetailsModel> records, bool terminated, CancellationToken cancellationToken)
    {
        while (await reader.ReadAsync(cancellationToken))
        {
            records.Add(new MortgageDetailsModel
            {
                ImportDateTime = ReadNullableDate(reader, "ImportDateTime"), HudProjectNumber = ReadString(reader, "HudProjectNumber"),
                PremiseId = ReadString(reader, "PremiseId"), Units = ReadNullableInt(reader, "Units"),
                InitialEndorsementDate = ReadNullableDate(reader, "InitialEndorsementDate"),
                FinalEndorsementDate = terminated ? null : ReadNullableDate(reader, "FinalEndorsementDate"),
                OriginalMortgageAmount = ReadNullableDouble(reader, "OriginalMortgageAmount"), MaturityDate = ReadNullableDate(reader, "MaturityDate"),
                SectionOfActCode = ReadString(reader, "SectionOfActCode"), Status = terminated ? ReadString(reader, "Status") : string.Empty,
                TermDate = terminated ? ReadNullableDate(reader, "TermDate") : null
            });
        }
    }

    public async Task SaveCommentAsync(
        int propertyId,
        string comment,
        CancellationToken cancellationToken = default)
    {
        comment = comment.Trim();
        var editedBy = Environment.UserName;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        const string currentCommentSql = """
            SELECT [Comment: Preservation Status, Type or other new Affordability Te]
            FROM dbo.Properties
            WHERE PropertyID = @propertyId;
            """;
        await using var currentCommand = new SqlCommand(currentCommentSql, connection, transaction);
        currentCommand.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;
        var currentValue = await currentCommand.ExecuteScalarAsync(cancellationToken);
        if (currentValue is null)
        {
            throw new InvalidOperationException($"Property {propertyId} was not found.");
        }

        var currentComment = currentValue == DBNull.Value ? string.Empty : Convert.ToString(currentValue) ?? string.Empty;
        if (string.Equals(currentComment.Trim(), comment, StringComparison.Ordinal))
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        const string updateCommentSql = """
            UPDATE dbo.Properties
            SET [Comment: Preservation Status, Type or other new Affordability Te] = @comment,
                RecModDate = SYSUTCDATETIME(),
                RecModBy = @editedBy
            WHERE PropertyID = @propertyId;

            INSERT INTO dbo.PropertyCommentHistory (PropertyID, CommentText, EditedAt, EditedBy)
            VALUES (@propertyId, @comment, SYSUTCDATETIME(), @editedBy);
            """;
        await using var updateCommand = new SqlCommand(updateCommentSql, connection, transaction);
        updateCommand.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;
        updateCommand.Parameters.Add("@comment", SqlDbType.NVarChar, -1).Value = comment;
        updateCommand.Parameters.Add("@editedBy", SqlDbType.NVarChar, 255).Value = editedBy;
        await updateCommand.ExecuteNonQueryAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
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

    private static decimal? ReadNullableDecimal(SqlDataReader reader, string name)
        => reader.IsDBNull(reader.GetOrdinal(name)) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal(name)));

    private static double? ReadNullableDouble(SqlDataReader reader, string name)
        => reader.IsDBNull(reader.GetOrdinal(name)) ? null : Convert.ToDouble(reader.GetValue(reader.GetOrdinal(name)));

    private sealed class PropertyArchiveData
    {
        public List<LihtcDetailsModel> LihtcRecords { get; } = new();
        public List<DhcdDetailsModel> DhcdRecords { get; } = new();
        public List<MortgageDetailsModel> HudActMortgages { get; } = new();
        public List<MortgageDetailsModel> HudTerminatedMortgages { get; } = new();
        public List<UsdaDetailsModel> UsdaRecords { get; } = new();
        public List<MhpDetailsModel> MhpRecords { get; } = new();
        public List<FoaDetailsModel> FoaRecords { get; } = new();
    }
}