using WebPresDB.Models;

namespace WebPresDB.Services;

public interface IAtRiskReportService
{
    Task<AtRiskReportResult> GenerateAsync(DateTime forecastDate, CancellationToken cancellationToken = default);
}