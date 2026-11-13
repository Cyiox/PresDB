using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using WebPresDB.Models;

namespace WebPresDB.Services;

public sealed class NoticeStorageService
{
    private const long MaximumPdfSize = 100 * 1024 * 1024;
    private readonly PreservationTestContext db;
    private readonly string rootPath;
    private readonly ILogger<NoticeStorageService> logger;

    public NoticeStorageService(
        PreservationTestContext db,
        IConfiguration configuration,
        IWebHostEnvironment environment,
        ILogger<NoticeStorageService> logger)
    {
        this.db = db;
        this.logger = logger;
        var configuredPath = configuration["40TNoticeStorage:RootPath"] ?? "40T Notice Storage";
        rootPath = Path.GetFullPath(Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(environment.ContentRootPath, configuredPath));
    }

    public async Task<IReadOnlyList<NoticeFolder>> GetFoldersAsync(
        string? searchTerm = null,
        bool unmatchedOnly = false,
        bool matchedOnly = false,
        int? maxResults = null,
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"The 40T notice storage folder was not found: {rootPath}");
        }

        var directories = Directory.EnumerateDirectories(rootPath)
            .Select(path => new DirectoryInfo(path))
            .Where(directory => string.IsNullOrWhiteSpace(searchTerm)
                || directory.Name.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase))
            .OrderBy(directory => directory.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var propertyIds = directories
            .Select(directory => ParsePropertyId(directory.Name))
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToArray();

        var properties = await db.Properties
            .AsNoTracking()
            .Where(property => propertyIds.Contains(property.PropertyId))
            .ToDictionaryAsync(property => property.PropertyId, cancellationToken);

        var folders = directories
            .Select(directory =>
            {
                var propertyId = ParsePropertyId(directory.Name);
                properties.TryGetValue(propertyId ?? 0, out var property);
                var latestDate = GetFolderFiles(directory.Name).Files
                    .Where(file => file.NoticeDate.HasValue)
                    .Select(file => file.NoticeDate)
                    .Max();
                return new NoticeFolder(directory.Name, propertyId, property, latestDate);
            })
            .Where(folder => !unmatchedOnly || folder.Property is null)
            .Where(folder => !matchedOnly || folder.Property is not null)
            .ToList();

        return maxResults.HasValue ? folders.Take(Math.Max(0, maxResults.Value)).ToList() : folders;
    }

    public NoticeFileResult GetFolderFiles(string folderName)
    {
        var folderPath = GetFolderPath(folderName);
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Notice folder '{folderName}' was not found.");
        }

        var files = Directory.EnumerateFiles(folderPath, "*.pdf", SearchOption.TopDirectoryOnly)
            .Select(path =>
            {
                var name = Path.GetFileName(path);
                var noticeDate = ParseNoticeDate(name);
                return new NoticeFile(name, new FileInfo(path).Length, noticeDate, noticeDate);
            })
            .OrderBy(file => file.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new NoticeFileResult(folderName, files);
    }

    public async Task<NoticeStorageStatistics> GetStatisticsAsync(
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var allFolders = await GetFoldersAsync(cancellationToken: cancellationToken);
        var matchingFolders = await GetFoldersAsync(
            searchTerm,
            matchedOnly: true,
            cancellationToken: cancellationToken);
        var allFiles = allFolders
            .SelectMany(folder => GetFolderFiles(folder.FolderName).Files)
            .ToList();

        return new NoticeStorageStatistics(
            allFiles.Count,
            allFolders.Count,
            allFiles
                .Where(file => file.UploadedDate.HasValue)
                .Select(file => file.UploadedDate)
                .Max(),
            matchingFolders.Count);
    }

    public string GetPdfPath(string folderName, string fileName)
    {
        var folderPath = GetFolderPath(folderName);
        var safeFileName = Path.GetFileName(fileName);
        if (!string.Equals(safeFileName, fileName, StringComparison.Ordinal)
            || !string.Equals(Path.GetExtension(safeFileName), ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new FileNotFoundException("Only PDF files in a notice folder can be opened.");
        }

        var filePath = Path.GetFullPath(Path.Combine(folderPath, safeFileName));
        EnsureWithinRoot(filePath);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The requested notice PDF was not found.");
        }

        return filePath;
    }

    public async Task SavePdfAsync(string folderName, string fileName, Stream content, long length, CancellationToken cancellationToken = default)
    {
        if (length <= 0 || length > MaximumPdfSize)
        {
            throw new InvalidOperationException("PDF files must be between 1 byte and 100 MB.");
        }

        var folderPath = GetFolderPath(folderName);
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Notice folder '{folderName}' was not found.");
        }

        var safeFileName = Path.GetFileName(fileName);
        if (!string.Equals(safeFileName, fileName, StringComparison.Ordinal)
            || !string.Equals(Path.GetExtension(safeFileName), ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only PDF files can be uploaded.");
        }

        var filePath = Path.GetFullPath(Path.Combine(folderPath, safeFileName));
        EnsureWithinRoot(filePath);
        await using var output = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(output, cancellationToken);
        logger.LogInformation("Stored 40T notice PDF {FileName} in folder {FolderName}.", safeFileName, folderName);
    }

    public void DeletePdf(string folderName, string fileName)
    {
        var filePath = GetPdfPath(folderName, fileName);
        File.Delete(filePath);
        logger.LogInformation("Deleted 40T notice PDF {FileName} from folder {FolderName}.", fileName, folderName);
    }

    private string GetFolderPath(string folderName)
    {
        var safeFolderName = Path.GetFileName(folderName);
        if (string.IsNullOrWhiteSpace(safeFolderName)
            || !string.Equals(safeFolderName, folderName, StringComparison.Ordinal))
        {
            throw new DirectoryNotFoundException("The requested notice folder is invalid.");
        }

        var folderPath = Path.GetFullPath(Path.Combine(rootPath, safeFolderName));
        EnsureWithinRoot(folderPath);
        return folderPath;
    }

    private void EnsureWithinRoot(string path)
    {
        var rootWithSeparator = rootPath.EndsWith(Path.DirectorySeparatorChar)
            ? rootPath
            : rootPath + Path.DirectorySeparatorChar;
        if (!path.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(path, rootPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The requested notice path is outside the configured storage folder.");
        }
    }

    private static int? ParsePropertyId(string folderName)
    {
        var separator = folderName.IndexOf('-');
        return separator > 0 && int.TryParse(folderName[..separator], out var propertyId)
            ? propertyId
            : null;
    }

    private static DateTime? ParseNoticeDate(string fileName)
    {
        var match = Regex.Match(
            Path.GetFileNameWithoutExtension(fileName),
            @"(?<!\d)(\d{4}[.\-_]\d{1,2}[.\-_]\d{1,2}|\d{1,2}[.\-_]\d{1,2}[.\-_]\d{2,4})(?!\d)");
        if (!match.Success)
        {
            return null;
        }

        var value = match.Groups[1].Value.Replace('-', '.').Replace('_', '.');
        var formats = value.Length >= 8 && value[..4].All(char.IsDigit)
            ? new[] { "yyyy.M.d", "yyyy.MM.dd" }
            : new[] { "M.d.yy", "M.d.yyyy" };

        return DateTime.TryParseExact(
            value,
            formats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var date)
            ? date.Date
            : null;
    }
}

public sealed record NoticeFolder(string FolderName, int? PropertyId, Property? Property, DateTime? LastUpdated)
{
    public bool IsMatched => Property is not null;
}

public sealed record NoticeFile(string Name, long Size, DateTime? NoticeDate, DateTime? UploadedDate);

public sealed record NoticeFileResult(string FolderName, IReadOnlyList<NoticeFile> Files);

public sealed record NoticeStorageStatistics(
    int TotalNoticeCount,
    int TotalProjectCount,
    DateTime? MostRecentUpload,
    int MatchingProjectCount);
