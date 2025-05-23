namespace OCTannerAnniversaryDates.DestinationManager;

using Microsoft.Extensions.Options;
using Renci.SshNet;
using System.Globalization;

/// <inheritdoc />
public class RenciSFTPDestinationManager : IDestinationManager
{
    private readonly RenciSFTPDestinationManagerConfig config;

    public RenciSFTPDestinationManager(IOptions<RenciSFTPDestinationManagerConfig> options)
    {
        this.config = options.Value;
    }

    /// <inheritdoc/>
    public void WriteStream(Stream stream, DateTime reportDateTime)
    {
        var privateKeyFile = this.config.KeyPassphrase == null
            ? new PrivateKeyFile(this.config.KeyFileLocation)
            : new PrivateKeyFile(this.config.KeyFileLocation, this.config.KeyPassphrase);

        var authenticationMethod = new PrivateKeyAuthenticationMethod(this.config.Username, privateKeyFile);

        string fullMonthName = reportDateTime.ToString("MMMM", CultureInfo.InvariantCulture);
        string remoteFileName = $"Serv_{fullMonthName}Anniversary.txt";
        string remoteDestination = this.config.RemotePath + "/" + remoteFileName; // unix path style

        using var client = new SftpClient(new ConnectionInfo(this.config.Host, this.config.Username, authenticationMethod));
        client.Connect();
        client.UploadFile(stream, remoteDestination);
        client.Disconnect();
    }
}
