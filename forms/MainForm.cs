using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grpc.Core;
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

        sfdgUsers.DataSource = UserCollection;
    }

    private Admin.AdminClient AdminClient { get; }

    private ILogger Logger { get; }

    private ObservableCollection<User> UserCollection { get; } = new();

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

    private void sfbFetchUsers_Click(object sender, EventArgs e) => _ = sfbFetchUsers_ClickAsync();

    private async Task sfbFetchUsers_ClickAsync()
    {
        Logger.LogInformation("Fetching users...");

        try
        {
            using var call = AdminClient.FetchUsers(new());

            await foreach (var user in call.ResponseStream.ReadAllAsync())
                UserCollection.Add(user);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "FetchUsers failed.");
        }
    }
}