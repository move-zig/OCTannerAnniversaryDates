namespace OCTannerAnniversaryDates.DestinationManager;

public interface IDestinationManager
{
    public void WriteStream(Stream stream, DateTime reportDateTime);
}
