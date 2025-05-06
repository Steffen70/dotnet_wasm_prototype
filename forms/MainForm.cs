using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common;

namespace SwissPension.WasmPrototype.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        Logger = loggerFactory.CreateLogger(GetType().Namespace!);

        Logger.LogInformation($"Creating gRPC channel for address: {BuildConstants.ApiUrl}");

        var baseAddress = new Uri(BuildConstants.ApiUrl);

        var channel = GrpcChannel.ForAddress(baseAddress, new()
        {
            LoggerFactory = loggerFactory
        });

        AdminClient = new(channel);
    }

    private Admin.AdminClient AdminClient { get; }

    private ILogger Logger { get; }

    private void sfbHelloWorld_Click(object sender, EventArgs e) => _ = sfbHelloWorld_ClickAsync();

    private async Task sfbHelloWorld_ClickAsync()
    {
        try
        {
            var response = await AdminClient.HelloWorldAsync(new());
            var message = response.Message;
            Logger.LogInformation($"Received response from server: {message}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "HelloWorld failed.");
        }
    }
}