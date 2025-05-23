namespace OCTannerAnniversaryDates.SourceManager;

using System.Globalization;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

/// <inheritdoc />
public partial class FileSystemSourceManager : ISourceManager
{
    private const string location = @"C:\temp";

    private readonly IFileSystem fileSystem;

    private string? fileFullname = null;

    [GeneratedRegex(@"^XYZ_(\d{4}-\d{2}-\d{2})-(\d{5,6}-(?:AM|PM))\.csv$")]
    private static partial Regex ValidCSVRegex();

    public FileSystemSourceManager(IFileSystem filesystem)
    {
        this.fileSystem = filesystem;
    }

    /// <inheritdoc/>
    public bool Locate()
    {
        var files = this.fileSystem.Directory.GetFiles(location, "*.csv")
            .Where(IsValidCSVFile);

        var fileCount = files.Count();

        if (fileCount == 0)
        {
            return false;
        }

        // we expect at most one file
        if (files.Count() > 1)
        {
            throw new Exception("Unexpected number of files in source directory");
        }

        this.fileFullname = files.First();

        return true;

        bool IsValidCSVFile(string fullName)
        {
            return ValidCSVRegex().IsMatch(this.fileSystem.Path.GetFileName(fullName));
        }
    }

    /// <inheritdoc />
    public Stream ReadStream()
    {
        if (this.fileFullname == null)
        {
            throw new Exception("No file located");
        }

        return this.fileSystem.FileStream.New(this.fileFullname, FileMode.Open);
    }

    /// <inheritdoc />
    public DateTime ReportDateTime()
    {
        if (this.fileFullname == null)
        {
            throw new Exception("No file located");
        }

        Match m = ValidCSVRegex().Match(this.fileSystem.Path.GetFileName(this.fileFullname));
        if (!m.Success)
        {
            throw new Exception("Filename doesn't match pattern");
        }

        string filenameDate = m.Groups[1].Value;
        string filenameTime = m.Groups[2].Value;

        if (filenameTime.Length == 8)
        {
            filenameTime = "0" + filenameTime;
        }

        Console.WriteLine(filenameDate);
        Console.WriteLine(filenameTime);

        // Example: 2025-06-30 63059-AM or 2025-07-04 101010-PM
        string format = "yyyy-MM-dd hhmmss-tt";

        if (DateTime.TryParseExact(filenameDate + " " + filenameTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }

        throw new Exception("Unable to determine date from filename");
    }

    /// <inheritdoc />
    public void ResetSource()
    {
        if (this.fileFullname == null)
        {
            throw new Exception("No file located");
        }

        this.fileSystem.File.Delete(this.fileFullname);
    }
}
