using System;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common;
using SwissPension.WasmPrototype.Common.Services;
using SwissPension.WasmPrototype.Frontend.Helpers;
using SwissPension.WasmPrototype.Frontend.UiAdapters;

namespace SwissPension.WasmPrototype.Frontend;

public class Program
{
    internal static UserService UserService { get; private set; }

    internal static ILogger<Program> Logger { get; private set; }

    internal static void SetText(string id, string text)
    {
        try
        {
            var el = Interop.GetElementById(id);
            if (el != null)
            {
                el.SetProperty("innerText", text);
                Logger.LogInformation($"Updated element '{id}' with text: {text}");
            }
            else
            {
                Logger.LogInformation($"Element with id '{id}' not found.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"Error in SetText: {ex.Message}");
        }
    }

    public static void Main()
    {
        try
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddProvider(new WasmLoggerProvider()); });
            Logger = loggerFactory.CreateLogger<Program>();

            Logger.LogInformation("WASM loaded successfully.");

            var wasmUiThreadDispatcher = new WasmUiThreadDispatcher();

            Logger.LogInformation($"Creating gRPC channel for address: {BuildConstants.ApiUrl}");

            var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

            var channel = GrpcChannel.ForAddress(BuildConstants.ApiUrl, new()
            {
                HttpHandler = grpcWebHandler,
                LoggerFactory = loggerFactory
            });

            var adminClient = new Admin.AdminClient(channel);

            UserService = new(loggerFactory, adminClient, new WebGridAdapter<User>(wasmUiThreadDispatcher));
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error in Main: {ex.Message} stack trace: {ex.StackTrace}";

            if (Logger == null)
                Console.WriteLine(errorMessage);
            else
                Logger.LogError(ex, "An error occurred during the initialization process.");
        }
        finally
        {
            Interop.HandleDotnetReady();
        }
    }
}