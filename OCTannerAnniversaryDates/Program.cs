using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OCTannerAnniversaryDates;
using OCTannerAnniversaryDates.Archiver;
using OCTannerAnniversaryDates.Converter;
using OCTannerAnniversaryDates.SourceManager;
using OCTannerAnniversaryDates.DestinationManager;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IOCTanner, OCTanner>();
builder.Services.AddOptions<OCTannerConfig>()
    .Bind(builder.Configuration.GetRequiredSection(OCTannerConfig.ConfigurationSectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<ISourceManager, FileSystemSourceManager>();
builder.Services.AddSingleton<IConverter, GemboxSpreadsheetConverter>();
builder.Services.AddSingleton<IDestinationManager, DummyDestinationManager>();
//builder.Services.AddSingleton<IDestinationManager, RenciSFTPDestinationManager>();
//builder.Services.AddOptions<RenciSFTPDestinationManagerConfig>()
//    .Bind(builder.Configuration.GetRequiredSection(RenciSFTPDestinationManagerConfig.ConfigurationSectionName))
//    .ValidateDataAnnotations()
//    .ValidateOnStart();
builder.Services.AddSingleton<IArchiver, FileSystemArchiver>();
builder.Services.AddSingleton<IFileSystem, FileSystem>();

using var host = builder.Build();
var ocTanner = host.Services.GetRequiredService<IOCTanner>();
await ocTanner.SendAnniversaryDatesAsync();