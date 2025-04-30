using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common;
using SwissPension.WasmPrototype.Frontend.Helpers;

namespace SwissPension.WasmPrototype.Frontend;

public partial class Program
{
    internal static Admin.AdminClient AdminClient { get; private set; }

    internal static ILogger Logger { get; private set; }

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
    
    internal static async Task CallAndRespondAsync()
    {
        var response = await AdminClient.HelloWorldAsync(new());
        var message = response.Message;
        Logger.LogInformation($"Received response from server: {message}");
    
        // Set JS DOM text or call JS function with result
        SetText("output", message);
    }

    public static void Main()
    {
        try
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new WasmLoggerProvider());
            });

            Logger = loggerFactory.CreateLogger(typeof(Program).Namespace!);

            Logger.LogInformation("WASM loaded successfully.");

            Logger.LogInformation($"Creating gRPC channel for address: {BuildConstants.ApiUrl}");

            var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            var httpClient = new HttpClient(grpcWebHandler);

            var channel = GrpcChannel.ForAddress(BuildConstants.ApiUrl, new()
            {
                HttpClient = httpClient,
                LoggerFactory = loggerFactory
            });

            AdminClient = new(channel);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error in Main: {ex.Message}";
#if DEBUG            
            errorMessage += " stack trace: {ex.StackTrace}";
#endif
            if (Logger == null)
            {
                Console.WriteLine(errorMessage);
            }
            else
            {
                Logger.LogError(ex, "An error occurred during the initialization process.");
            }
        }
        finally
        {
            Interop.HandleDotnetReady();
        }
    }
}