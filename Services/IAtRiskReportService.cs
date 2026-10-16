using WebPresDB.Models;

namespace WebPresDB.Services;

public interface IAtRiskReportService
{
    Task<AtRiskReportResult> GenerateAsync(DateTime forecastDate, ReportDataSnapshot snapshot = ReportDataSnapshot.MostRecent, CancellationToken cancellationToken = default);
}