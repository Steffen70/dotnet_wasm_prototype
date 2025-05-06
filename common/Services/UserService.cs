using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common.UiAdapters;

namespace SwissPension.WasmPrototype.Common.Services;

public class UserService(ILoggerFactory loggerFactory, Admin.AdminClient adminClient, IGridAdapter<User> gridAdapter)
{
    private readonly ILogger<UserService> _logger = loggerFactory.CreateLogger<UserService>();

    public async Task HelloWorldAsync()
    {
        try
        {
            var response = await adminClient.HelloWorldAsync(new());
            var message = response.Message;
            _logger.LogInformation($"Received response from server: {message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HelloWorld failed.");
        }
    }

    public async Task FetchUsersAsync()
    {
        _logger.LogInformation("Fetching users...");

        try
        {
            using var call = adminClient.FetchUsers(new());

            await foreach (var user in call.ResponseStream.ReadAllAsync())
            {
                gridAdapter.AddRow(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FetchUsers failed.");
        }
    }
}