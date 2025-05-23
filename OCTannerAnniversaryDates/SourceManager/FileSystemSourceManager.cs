namespace OCTannerAnniversaryDates.SourceManager;

using System.Globalization;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

/// <inheritdoc />
public partial class FileSystemSourceManager : ISourceManager
{
    private const string location = @"C:\temp";

    private readonly IFileSystem fileSystem;

    [GeneratedRegex(@"^XYZ_(\d{4}-\d{2}-\d{2}-\d{5,6}-(?:AM|PM))\.csv$")]
    private static partial Regex ValidCSVRegex();

    public FileSystemSourceManager(IFileSystem filesystem)
    {
        this.fileSystem = filesystem;
    }

    /// <inheritdoc />
    public (Stream, DateTime) ReadStream()
    {
        var files = this.fileSystem.Directory.GetFiles(location, "*.csv")
            .Where(IsValidCSVFile);

        // we expect exactly one file
        if (files.Count() != 1)
        {
            throw new Exception("Unexpected number of files in source directory");
        }

        var file = files.First();

        var reportDateTime = this.GetDateTimeFromFilename(this.fileSystem.Path.GetFileName(file));

        var stream = this.fileSystem.FileStream.New(file, FileMode.Open);

        return (stream, reportDateTime);

        bool IsValidCSVFile(string fullName)
        {
            return ValidCSVRegex().IsMatch(this.fileSystem.Path.GetFileName(fullName));
        }
    }

    /// <inheritdoc />
    public void ResetSource()
    {
        var directoryInfo = this.fileSystem.DirectoryInfo.New(location);

        foreach (var file in directoryInfo.GetFiles())
        {
            file.Delete();
        }

        foreach (var dir in directoryInfo.GetDirectories())
        {
            dir.Delete(true);
        }

        this.fileSystem.Directory.Delete(location);
    }

    private DateTime GetDateTimeFromFilename(string filename)
    {
        Match m = ValidCSVRegex().Match(filename);
        if (!m.Success)
        {
            throw new Exception("Filename doesn't match pattern");
        }

        string filenameDate = m.Groups[1].Value;

        Console.WriteLine(filenameDate);

        // Example: 2025-06-30-63059-AM or 2025-07-04-101010-PM
        string format = "yyyy-MM-dd-hmmss-tt";

        if (DateTime.TryParseExact(filenameDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }

        throw new Exception("Unable to determine date from filename");
    }
}
