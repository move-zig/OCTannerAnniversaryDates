using System.ComponentModel.DataAnnotations;

namespace OCTannerAnniversaryDates.DestinationManager;

public sealed class RenciSFTPDestinationManagerConfig
{
    public const string ConfigurationSectionName = "RenciSFTPWriter";

    [Required]
    required public string Host { get; set; }

    [Required]
    required public string RemotePath { get; set; }

    [Required]
    required public string Username { get; set; }

    [Required]
    required public string KeyFileLocation { get; set; }

    public string? KeyPassphrase { get; set; }
}
