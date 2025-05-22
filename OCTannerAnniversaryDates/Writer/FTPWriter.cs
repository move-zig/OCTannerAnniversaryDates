namespace OCTannerAnniversaryDates.Writer;

public class FTPWriter : IWriter
{
    public async Task WriteStream(Stream stream)
    {
        Console.WriteLine("Uploading!!");
    }
}
