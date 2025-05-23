namespace OCTannerAnniversaryDates.DestinationManager;

/// <inheritdoc />
public class DummyDestinationManager : IDestinationManager
{
    /// <inheritdoc/>
    public void WriteStream(Stream stream, DateTime reportDateTime)
    {
        Console.WriteLine("Uploading!!");
    }
}
