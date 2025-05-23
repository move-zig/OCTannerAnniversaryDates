namespace OCTannerAnniversaryDates.Archiver;

using System.IO.Abstractions;

/// <inheritdoc />
public class FileSystemArchiver : IArchiver
{
    private const string location = @"C:\archives";

    private readonly IFileSystem fileSystem;

    public FileSystemArchiver(IFileSystem filesystem)
    {
        this.fileSystem = filesystem;
    }

    /// <inheritdoc />
    public void WriteStream(Stream stream, DateTime reportDateTime)
    {
        if (!this.fileSystem.Directory.Exists(location))
        {
            this.fileSystem.Directory.CreateDirectory(location);
        }

        using var fileStream = this.fileSystem.FileStream.New(this.fileSystem.Path.Join(location, this.ArchiveFilename(reportDateTime)), FileMode.CreateNew);

        stream.CopyTo(fileStream);
        fileStream.Flush();
        fileStream.Close();
    }

    private string ArchiveFilename(DateTime reportDateTime)
    {
        return "dslfkjdslfkjd.txt";
    }
}
