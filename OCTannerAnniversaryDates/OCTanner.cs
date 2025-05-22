using OCTannerAnniversaryDates.Archiver;
using OCTannerAnniversaryDates.Converter;
using OCTannerAnniversaryDates.Reader;
using OCTannerAnniversaryDates.Writer;

namespace OCTannerAnniversaryDates;

public class OCTanner : IOCTanner
{
    private readonly IReader reader;
    private readonly IConverter converter;
    private readonly IWriter writer;
    private readonly IArchiver archiver;

    public OCTanner(IReader reader, IConverter converter, IWriter writer, IArchiver archiver)
    {
        this.reader = reader;
        this.converter = converter;
        this.writer = writer;
        this.archiver = archiver;
    }

    public async Task SendAnniversaryDatesAsync()
    {
        var inputStream = this.reader.GetStream();
        using var outputStream = this.converter.Convert(inputStream);
        await this.writer.WriteStream(outputStream);
        this.archiver.WriteStream(outputStream);
    }
}
