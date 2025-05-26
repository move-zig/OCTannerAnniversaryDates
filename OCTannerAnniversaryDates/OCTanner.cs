namespace OCTannerAnniversaryDates;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OCTannerAnniversaryDates.Archiver;
using OCTannerAnniversaryDates.Converter;
using OCTannerAnniversaryDates.SourceManager;
using OCTannerAnniversaryDates.DestinationManager;

public class OCTanner : IOCTanner
{
    private readonly string emailAddress;
    private readonly ISourceManager sourceManager;
    private readonly IConverter converter;
    private readonly IDestinationManager destinationManager;
    private readonly IArchiver archiver;
    private readonly ILogger<OCTanner> logger;

    public OCTanner(
        IOptions<OCTannerConfig> options,
        ISourceManager sourceManager,
        IConverter converter,
        IDestinationManager destinationManager,
        IArchiver archiver,
        ILogger<OCTanner> logger)
    {
        this.emailAddress = options.Value.EmailAddress;
        this.sourceManager = sourceManager;
        this.converter = converter;
        this.destinationManager = destinationManager;
        this.archiver = archiver;
        this.logger = logger;
    }

    public async Task SendAnniversaryDatesAsync()
    {
        try
        {
            if (!this.sourceManager.Locate())
            {
                Console.WriteLine("no file found");
                return;
            }

            var reportDateTime = this.sourceManager.ReportDateTime();
            using (var inputStream = this.sourceManager.ReadStream())
            {
                using var outputStream = this.converter.Convert(inputStream);
                this.destinationManager.WriteStream(outputStream, reportDateTime);
                this.archiver.WriteStream(outputStream, reportDateTime);
            }

            this.sourceManager.ResetSource();
        }
        catch (Exception ex)
        {
            this.logger.LogError("Uncaught exception: {message}", ex.Message);
        }
    }
}
