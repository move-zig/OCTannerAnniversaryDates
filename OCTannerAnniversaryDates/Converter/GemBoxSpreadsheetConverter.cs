﻿namespace OCTannerAnniversaryDates.Converter;

using GemBox.Spreadsheet;

/// <inheritdoc />
public class GemboxSpreadsheetConverter : IConverter
{
    static GemboxSpreadsheetConverter()
    {
        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
    }

    /// <inheritdoc />
    public Stream Convert(Stream stream)
    {
        var excelFile = ExcelFile.Load(stream, new CsvLoadOptions(','));
        excelFile.Worksheets[0].Rows.Remove(0); // remove header row

        var convertedStream = new MemoryStream();
        excelFile.Save(convertedStream, new CsvSaveOptions('\t'));

        return convertedStream;
    }
}
