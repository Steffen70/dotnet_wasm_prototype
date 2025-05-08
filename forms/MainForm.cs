using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common;
using SwissPension.WasmPrototype.Common.Services;
using SwissPension.WasmPrototype.Forms.UiAdapters;

namespace SwissPension.WasmPrototype.Forms;

public partial class MainForm : Form
{
    private readonly UserService _userService;

    public MainForm()
    {
        InitializeComponent();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        var logger = loggerFactory.CreateLogger<MainForm>();

        logger.LogInformation($"Creating gRPC channel for address: {BuildConstants.ApiUrl}");

        var baseAddress = new Uri(BuildConstants.ApiUrl);

        var channel = GrpcChannel.ForAddress(baseAddress, new()
        {
            LoggerFactory = loggerFactory
        });

        var adminClient = new Admin.AdminClient(channel);

        var userGridAdapter = new FormsGridAdapter<User>(loggerFactory);
        sfdgUsers.DataSource = userGridAdapter.BindingList;

        _userService = new(loggerFactory, adminClient, userGridAdapter);
    }

    private void sfbHelloWorld_Click(object sender, EventArgs e) => Task.Run(_userService.HelloWorldAsync);

    private void sfbFetchUsers_Click(object sender, EventArgs e) => Task.Run(_userService.FetchUsersAsync);
}