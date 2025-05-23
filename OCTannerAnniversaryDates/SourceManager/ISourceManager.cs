namespace OCTannerAnniversaryDates.SourceManager;

public interface ISourceManager
{
    public (Stream stream, DateTime reportDateTime) ReadStream();

    public void ResetSource();
}
