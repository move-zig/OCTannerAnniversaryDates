using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OCTannerAnniversaryDates;
using OCTannerAnniversaryDates.Archiver;
using OCTannerAnniversaryDates.Converter;
using OCTannerAnniversaryDates.Reader;
using OCTannerAnniversaryDates.Writer;
using System.IO.Abstractions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IOCTanner, OCTanner>();
builder.Services.AddSingleton<IReader, FileSystemReader>();
builder.Services.AddSingleton<IConverter, GemboxSpreadsheetConverter>();
builder.Services.AddSingleton<IWriter, FTPWriter>();
builder.Services.AddSingleton<IArchiver, FileSystemArchiver>();
builder.Services.AddSingleton<IFileSystem, FileSystem>();

using var host = builder.Build();
var ocTanner = host.Services.GetRequiredService<IOCTanner>();
await ocTanner.SendAnniversaryDatesAsync();