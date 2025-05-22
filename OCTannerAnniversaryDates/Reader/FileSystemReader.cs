namespace OCTannerAnniversaryDates.Reader;

using System.IO.Abstractions;

public class FileSystemReader : IReader
{
    private const string location = @"C:\temp";

    private readonly IFileSystem fileSystem;

    public FileSystemReader(IFileSystem filesystem)
    {
        this.fileSystem = filesystem;
    }

    public Stream GetStream()
    {
        string[] files = this.fileSystem.Directory.GetFiles(location, "XYZ*.csv");

        // we expect exactly one file
        if (files.Length != 1)
        {
            throw new Exception("Unexpected number of files in source directory");
        }

        return this.fileSystem.FileStream.New(files[0], FileMode.Open);
    }
}
