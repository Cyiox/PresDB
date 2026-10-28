using Microsoft.EntityFrameworkCore;
using WebPresDB.Models;

namespace WebPresDB.Services;

public sealed class AtRiskReportService(PreservationTestContext db, ILogger<AtRiskReportService> logger) : IAtRiskReportService
{
    private static readonly string[] RiskSharingCodes = ["YHE", "YHN"];
    private static readonly string[] PreservingMortgageCodes = ["ZPC", "ZPD", "ZPE", "ZPF", "ZPH", "ZPI"];

    public async Task<AtRiskReportResult> GenerateAsync(DateTime forecastDate, ReportDataSnapshot snapshot = ReportDataSnapshot.MostRecent, CancellationToken cancellationToken = default)
    {
        forecastDate = forecastDate.Date;
        var properties = await db.Properties.AsNoTracking().ToListAsync(cancellationToken);
        var latestAging = await LatestByPropertyAsync(db.PreservedUnitsAgings.AsNoTracking().Where(x => x.PreservedThroughDate <= forecastDate), x => x.PropertyId, x => x.PreservedThroughDate, snapshot, cancellationToken);
        var latestMortgages = await LatestByPropertyAsync(db.MtgAArchives.AsNoTracking().Where(x => x.ExpUsePropertyId.HasValue), x => x.ExpUsePropertyId!.Value, x => x.ImportDateTime, snapshot, cancellationToken);
        var latestContracts = await LatestByPropertyAsync(db.MfAssistanceSec8ContractsArchives.AsNoTracking().Where(x => x.ExpUsePropertyId.HasValue), x => x.ExpUsePropertyId!.Value, x => x.ImportDateTime, snapshot, cancellationToken);
        var latestLihtc = await LatestByPropertyAsync(db.LihtcpubArchives.AsNoTracking().Where(x => x.ExpUsePropertyId.HasValue), x => x.ExpUsePropertyId!.Value, x => x.ImportDateTime, snapshot, cancellationToken);
        var dhcdAwards = await LatestByPropertyAsync(db.DhcdArchives.AsNoTracking().Where(x => x.ExpUsePropertyId.HasValue), x => x.ExpUsePropertyId!.Value, x => x.ImportDateTime, snapshot, cancellationToken);
        var excludedIds = (await db.PropertiesExcludedFromAtRiskReports.AsNoTracking().Select(x => x.PropertyId).ToListAsync(cancellationToken)).ToHashSet();

        var rows = new List<AtRiskReportRow>(properties.Count);
        foreach (var property in properties)
        {
            if (excludedIds.Contains(property.PropertyId)) continue;
            latestAging.TryGetValue(property.PropertyId, out var aging);
            latestMortgages.TryGetValue(property.PropertyId, out var mortgage);
            latestContracts.TryGetValue(property.PropertyId, out var contract);
            latestLihtc.TryGetValue(property.PropertyId, out var lihtc);
            dhcdAwards.TryGetValue(property.PropertyId, out var dhcd);
            var rule = FindPreservationRule(property, aging, mortgage, contract, lihtc, forecastDate);
            var overrideAtRisk = property.OtherAtRiskDate is not null && property.OtherAtRiskDate.Value.Date < forecastDate;
            var isAtRisk = overrideAtRisk || rule is null;
            rows.Add(new AtRiskReportRow
            {
                PropertyId = property.PropertyId,
                PropertyName = property.PropertyName ?? "",
                City = property.City ?? "",
                Street = property.Street ?? "",
                Zip = property.Zip ?? "",
                Agency = property.Agency ?? "",
                TotalUnits = aging?.TotalUnits ?? property.PropertyTotalUnitCount,
                UnitsAtRisk = aging?.UnitsAtRisk ?? property.UnitsAtRiskNum,
                PreservedThroughDate = aging?.PreservedThroughDate,
                Status = isAtRisk ? "At risk" : "Preserved",
                PreservationRule = isAtRisk && overrideAtRisk
                    ? $"At risk date override ({property.OtherAtRiskDate:yyyy-MM-dd})"
                    : isAtRisk ? "No preservation rule matched" : rule!
                , HasDhcdAwards = dhcd is not null
                , DhcdProjectName = dhcd?.ProjectName ?? ""
                , DhcdStateLihtcAwarded = dhcd?.StateLihtcAwarded
                , DhcdNinePctAwarded = dhcd?.NinePctAwarded
                , DhcdAffordableUnits = dhcd?.NumberOfAffordableUnits
                , DhcdImportDate = dhcd?.ImportDateTime
            });
        }

        var result = new AtRiskReportResult { ForecastDate = forecastDate, Rows = rows.OrderBy(x => x.City).ThenBy(x => x.PropertyName).ToArray() };
        logger.LogInformation("Generated preservation report for {ForecastDate}: {Count} properties", forecastDate, result.Rows.Count);
        return result;
    }

    // This function below is reponsable for determining if a property is preserved or not based on a criteria that was made to follow statutory affordablity regulations.
    private static string? FindPreservationRule(Property property, PreservedUnitsAging? aging, MtgAArchive? mortgage, MfAssistanceSec8ContractsArchive? contract, LihtcpubArchive? lihtc, DateTime forecastDate)
    {
        if (property.TitleIiVi?.Equals("VI", StringComparison.OrdinalIgnoreCase) == true && forecastDate <= new DateTime(2043, 1, 1)) return "Title VI override";
        if (property.TitleIiVi?.Equals("II", StringComparison.OrdinalIgnoreCase) == true && property.TitleIiatRiskDate <= forecastDate) return "Title II override";
        if (mortgage?.SectionOfActCode is { } section && PreservingMortgageCodes.Contains(section, StringComparer.OrdinalIgnoreCase)) return $"HUD active mortgage section of act code is {section}";
        if (mortgage?.SectionOfActCode is { } riskSharing && RiskSharingCodes.Contains(riskSharing, StringComparer.OrdinalIgnoreCase) && (mortgage.FinalEndorsementDate ?? DateTime.MinValue).Date >= forecastDate.AddYears(-15)) return $"HUD active mortgage section of act code is {riskSharing} and final endorsement is within 15 years";
        if (mortgage?.SoaCategorySubCategory?.Contains("236(j)(1)", StringComparison.OrdinalIgnoreCase) == true) return "HUD active mortgage SOA category is SOA 236(j)(1)";
        if (mortgage?.SoaCategorySubCategory?.Contains("202 Elderly", StringComparison.OrdinalIgnoreCase) == true) return "HUD active mortgage SOA category is 202 Elderly";
        if (mortgage?.FinalEndorsementDate >= forecastDate.AddYears(-15)) return "HUD active mortgage final endorsement is within 15 years";
        if (contract?.TracsOverallExpirationDate >= forecastDate) return "HUD contract TRACS expiration after forecast date";
        //Finds if "ProgramTypeGroupCode" inside of the  PRAC projects are typically preserved because they are owned by a nonprofit and serve a sensitive population, so 99% of the time they are preserved.
        //Specfically looks if the ProgramTypeGroupCode field in the PRAC
        if (contract?.ProgramTypeGroupCode?.Contains("PRAC", StringComparison.OrdinalIgnoreCase) == true) return "HUD contract program type group code is PRAC";
        // Since 202s must be owned by a nonprofit and deal with a sensitive population, 99% of the time they are preserved.
        //Specifically looks if the ProgramTypeGroupCode field in the MF_Assistance_and_Sec8_Contracts_archive table. It also looks in the MF_Properties_with_Assistance_and_Sec8_Contracts_archive table to find if the property_catagory_type or property_catagory_name is 202/811
        if (contract?.ProgramTypeGroupCode?.Contains("202", StringComparison.OrdinalIgnoreCase) == true) return "HUD contract program type group code is 202";
        // If the LIHTC yr_pis from the LIHTCPUB_archive table is less than 30 years from the forecast date, then the property must be preserved as LIHTC restrictions must last for at least 30 years by law. 
        if (lihtc?.YrPis is int yrPis && yrPis > 0 && yrPis <= 2100 && yrPis + 30 > forecastDate.Year) return $"TC year awarded {yrPis} is within 30 years of forecast date";
        if (aging?.PreservedThroughDate.Date >= forecastDate && (aging.UnitsAtRisk ?? 0) <= 0) return "Preserved-through date is on or after forecast date";
        if (property.OtherAtRiskDate?.Date >= forecastDate) return $"At risk date override: {property.OtherAtRiskDate:yyyy-MM-dd}";
        return null;
    }

    private static async Task<Dictionary<int, T>> LatestByPropertyAsync<T>(IQueryable<T> source, Func<T, int> keySelector, Func<T, DateTime?> dateSelector, ReportDataSnapshot snapshot, CancellationToken cancellationToken)
    {
        var values = await source.ToListAsync(cancellationToken);
        return values.GroupBy(keySelector).ToDictionary(
            group => group.Key,
            group => (snapshot == ReportDataSnapshot.MostRecent
                ? group.OrderByDescending(value => dateSelector(value) ?? DateTime.MinValue)
                : group.OrderBy(value => dateSelector(value) ?? DateTime.MaxValue)).First());
    }
}