namespace WebPresDB.Services;

public sealed class PublishedReportStorageService
{
    private readonly string rootPath;

    public PublishedReportStorageService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var configuredPath = configuration["PastPublishedReports:RootPath"] ?? "Past-Published-Reports";
        rootPath = Path.GetFullPath(Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(environment.ContentRootPath, configuredPath));
    }

    public IReadOnlyList<PublishedReportFile> GetReports()
    {
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"The published reports folder was not found: {rootPath}");
        }

        return Directory.EnumerateFiles(rootPath, "*.pdf", SearchOption.TopDirectoryOnly)
            .Select(path => new FileInfo(path))
            .OrderByDescending(file => file.LastWriteTimeUtc)
            .ThenBy(file => file.Name, StringComparer.OrdinalIgnoreCase)
            .Select(file => new PublishedReportFile(file.Name, file.Length, file.LastWriteTime))
            .ToList();
    }

    public string GetPdfPath(string fileName)
    {
        var safeFileName = Path.GetFileName(fileName);
        if (!string.Equals(safeFileName, fileName, StringComparison.Ordinal)
            || !string.Equals(Path.GetExtension(safeFileName), ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new FileNotFoundException("Only PDF files in the published reports folder can be opened.");
        }

        var filePath = Path.GetFullPath(Path.Combine(rootPath, safeFileName));
        EnsureWithinRoot(filePath);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The requested published report was not found.");
        }

        return filePath;
    }

    private void EnsureWithinRoot(string path)
    {
        var rootWithSeparator = rootPath.EndsWith(Path.DirectorySeparatorChar)
            ? rootPath
            : rootPath + Path.DirectorySeparatorChar;
        if (!path.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(path, rootPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The requested report path is outside the configured storage folder.");
        }
    }
}

public sealed record PublishedReportFile(string Name, long Size, DateTime LastUpdated);
