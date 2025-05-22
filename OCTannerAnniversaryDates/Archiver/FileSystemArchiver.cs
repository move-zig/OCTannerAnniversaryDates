using System.IO.Abstractions;

namespace OCTannerAnniversaryDates.Archiver;

public class FileSystemArchiver : IArchiver
{
    private const string location = @"C:\archives";

    private readonly IFileSystem fileSystem;

    public FileSystemArchiver(IFileSystem filesystem)
    {
        this.fileSystem = filesystem;
    }

    public void WriteStream(Stream stream)
    {
        if (!this.fileSystem.Directory.Exists(location))
        {
            this.fileSystem.Directory.CreateDirectory(location);
        }

        using var fileStream = this.fileSystem.FileStream.New(this.fileSystem.Path.Join(location, "archive.csv"), FileMode.CreateNew);

        stream.CopyTo(fileStream);
        fileStream.Flush();
        fileStream.Close();
    }
}
