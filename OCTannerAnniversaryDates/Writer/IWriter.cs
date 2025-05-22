namespace OCTannerAnniversaryDates.Writer;

public interface IWriter
{
    public Task WriteStream(Stream stream);
}
