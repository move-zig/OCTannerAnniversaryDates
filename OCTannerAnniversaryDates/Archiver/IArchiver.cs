namespace OCTannerAnniversaryDates.Archiver;

public interface IArchiver
{
    public void WriteStream(Stream stream, DateTime reportDateTime);
}
