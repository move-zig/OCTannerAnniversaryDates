namespace OCTannerAnniversaryDates.SourceManager;

public interface ISourceManager
{

    public bool Locate();

    public Stream ReadStream();

    public DateTime ReportDateTime();

    public void ResetSource();
}
