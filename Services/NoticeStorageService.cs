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
    private readonly string extractedDataPath;
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
        extractedDataPath = Path.Combine(environment.ContentRootPath, "40t_extracted_notices.csv");
    }

    public async Task<IReadOnlyList<NoticeFolder>> GetFoldersAsync(
        string? searchTerm = null,
        bool unmatchedOnly = false,
        bool matchedOnly = false,
        string? noticeType = null,
        int? maxResults = null,
        CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"The 40T notice storage folder was not found: {rootPath}");
        }

        var directories = Directory.EnumerateDirectories(rootPath)
            .Select(path => new DirectoryInfo(path))
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

        var extractedTypes = LoadExtractedNoticeTypes();
        var dbNoticeTypes = await db.Tbl40Tdocuments
            .AsNoTracking()
            .Where(document => document.PropertyId.HasValue && document.NoticeType != null)
            .Select(document => new { document.PropertyId, document.NoticeType })
            .ToListAsync(cancellationToken);

        var folders = directories
            .Select(directory =>
            {
                var propertyId = ParsePropertyId(directory.Name);
                properties.TryGetValue(propertyId ?? 0, out var property);
                var folderFiles = GetFolderFiles(directory.Name).Files;
                var latestDate = folderFiles
                    .Where(file => file.NoticeDate.HasValue)
                    .Select(file => file.NoticeDate)
                    .Max();
                var csvTypes = extractedTypes
                    .Where(entry => string.Equals(entry.FolderName, directory.Name, StringComparison.Ordinal))
                    .SelectMany(entry => entry.NoticeTypes)
                    .Concat(dbNoticeTypes
                        .Where(document => document.PropertyId == propertyId)
                        .Select(document => NormalizeNoticeType(document.NoticeType)))
                    .Where(type => !string.IsNullOrWhiteSpace(type))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(type => type, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
                return new NoticeFolder(directory.Name, propertyId, property, latestDate, csvTypes);
            })
            .Where(folder => string.IsNullOrWhiteSpace(searchTerm)
                || folder.FolderName.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
                || folder.PropertyId?.ToString(CultureInfo.InvariantCulture).Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) == true
                || (folder.Property?.PropertyName?.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) ?? false)
                || (folder.Property?.City?.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) ?? false))
            .Where(folder => !unmatchedOnly || folder.Property is null)
            .Where(folder => !matchedOnly || folder.Property is not null)
            .Where(folder => string.IsNullOrWhiteSpace(noticeType)
                || folder.NoticeTypes.Contains(noticeType, StringComparer.OrdinalIgnoreCase))
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
                var noticeType = LoadExtractedNoticeTypes()
                    .Where(entry => string.Equals(entry.FolderName, folderName, StringComparison.Ordinal)
                        && string.Equals(entry.FileName, name, StringComparison.OrdinalIgnoreCase))
                    .Select(entry => string.Join(" | ", entry.NoticeTypes))
                    .FirstOrDefault();
                return new NoticeFile(name, new FileInfo(path).Length, noticeDate, noticeDate, noticeType);
            })
            .OrderBy(file => file.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new NoticeFileResult(folderName, files);
    }

    public async Task<NoticeStorageStatistics> GetStatisticsAsync(
        string? searchTerm = null,
        string? noticeType = null,
        CancellationToken cancellationToken = default)
    {
        var allFolders = await GetFoldersAsync(cancellationToken: cancellationToken);
        var matchingFolders = await GetFoldersAsync(
            searchTerm,
            matchedOnly: true,
            noticeType: noticeType,
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

    public async Task<IReadOnlyList<string>> GetNoticeTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var csvTypes = LoadExtractedNoticeTypes()
            .SelectMany(entry => entry.NoticeTypes);
        var databaseTypes = await db.Tbl40Tdocuments
            .AsNoTracking()
            .Where(document => document.NoticeType != null)
            .Select(document => document.NoticeType!)
            .ToListAsync(cancellationToken);

        return csvTypes
            .Concat(databaseTypes.Select(NormalizeNoticeType))
            .Where(type => !string.IsNullOrWhiteSpace(type))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(type => type, StringComparer.OrdinalIgnoreCase)
            .ToArray();
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

    private IReadOnlyList<ExtractedNoticeEntry> LoadExtractedNoticeTypes()
    {
        if (!File.Exists(extractedDataPath))
        {
            return Array.Empty<ExtractedNoticeEntry>();
        }

        using var reader = new StreamReader(extractedDataPath);
        var header = ParseCsvLine(reader.ReadLine());
        var columnIndexes = header
            .Select((name, index) => new { name = name.Trim(), index })
            .ToDictionary(item => item.name, item => item.index, StringComparer.OrdinalIgnoreCase);

        if (!columnIndexes.TryGetValue("folder_name", out var folderIndex)
            || !columnIndexes.TryGetValue("file_name", out var fileIndex)
            || !columnIndexes.TryGetValue("checked_notifications", out var noticeIndex))
        {
            return Array.Empty<ExtractedNoticeEntry>();
        }

        var entries = new List<ExtractedNoticeEntry>();
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            var values = ParseCsvLine(line);
            if (folderIndex >= values.Count || fileIndex >= values.Count || noticeIndex >= values.Count)
            {
                continue;
            }

            var noticeTypes = values[noticeIndex]
                .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(NormalizeNoticeType)
                .Where(type => !string.IsNullOrWhiteSpace(type))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (noticeTypes.Length > 0)
            {
                entries.Add(new ExtractedNoticeEntry(values[folderIndex], values[fileIndex], noticeTypes));
            }
        }

        return entries;
    }

    private static IReadOnlyList<string> ParseCsvLine(string? line)
    {
        var values = new List<string>();
        if (line is null)
        {
            return values;
        }

        var current = new System.Text.StringBuilder();
        var inQuotes = false;
        for (var index = 0; index < line.Length; index++)
        {
            var character = line[index];
            if (character == '"')
            {
                if (inQuotes && index + 1 < line.Length && line[index + 1] == '"')
                {
                    current.Append('"');
                    index++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (character == ',' && !inQuotes)
            {
                values.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(character);
            }
        }
        values.Add(current.ToString());
        return values;
    }

    private static string NormalizeNoticeType(string? noticeType)
    {
        if (string.IsNullOrWhiteSpace(noticeType))
        {
            return "";
        }

        var type = noticeType.Trim();
        if (type.StartsWith("1 Year Notice", StringComparison.OrdinalIgnoreCase))
        {
            return "1 Year Notice";
        }
        if (type.StartsWith("2 Year Notice", StringComparison.OrdinalIgnoreCase))
        {
            return "2 Year Notice";
        }
        if (type.Equals("Notice of Intent To Sell", StringComparison.OrdinalIgnoreCase)
            || type.Equals("Notice of Intent to Sell to a Preservation Buyer", StringComparison.OrdinalIgnoreCase))
        {
            return "Notice of Intent to Sell";
        }
        return type;
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

public sealed record NoticeFile(string Name, long Size, DateTime? NoticeDate, DateTime? UploadedDate, string? NoticeType);

public sealed record NoticeFileResult(string FolderName, IReadOnlyList<NoticeFile> Files);

public sealed record NoticeStorageStatistics(
    int TotalNoticeCount,
    int TotalProjectCount,
    DateTime? MostRecentUpload,
    int MatchingProjectCount);

public sealed record NoticeFolder(
    string FolderName,
    int? PropertyId,
    Property? Property,
    DateTime? LastUpdated,
    IReadOnlyList<string> NoticeTypes)
{
    public bool IsMatched => Property is not null;
}

internal sealed record ExtractedNoticeEntry(
    string FolderName,
    string FileName,
    IReadOnlyList<string> NoticeTypes);
