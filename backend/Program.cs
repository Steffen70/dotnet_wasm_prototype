using System.Security.Cryptography.X509Certificates;
using SwissPension.WasmPrototype.Backend.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace SwissPension.WasmPrototype.Backend;

public class Program
{
    private const string ApiPortEnvironmentVariable = "API_PORT";

    private const string CorsPolicyName = "ClientPolicy";
    
    internal const string HttpClientName = "CustomHttpClient";

    public static void Main(string[] args)
    {
        // Working directory is not always the same as the executable directory - base directory is more reliable
        var basePath = Path.Combine(AppContext.BaseDirectory, "cert");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
            loggingBuilder.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
        });

#if RELEASE
        // Get the API port from the environment variable
        var apiPort = int.Parse(Environment.GetEnvironmentVariable(ApiPortEnvironmentVariable) ?? throw new InvalidOperationException($"{ApiPortEnvironmentVariable} environment variable not set"));
#elif DEBUG
        // Don't require the API_PORT environment variable in DEBUG mode
        const int apiPort = 8444;
#endif

        // Configure the Kestrel server with the certificate and the API port
        builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(apiPort, listenOptions =>
        {
            var pemPath = Path.Combine(basePath,"localhost.pem");
            var logger = listenOptions.ApplicationServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Using certificate from {pemPath}");

            listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(pemPath));
            // Enable HTTP/2 and HTTP/1.1 for gRPC-Web compatibility
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        }));

        // Allow all origins
        builder.Services.AddCors(o => o.AddPolicy(CorsPolicyName, policyBuilder =>
        {
            policyBuilder
                // Allow all ports on localhost
                .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                // Allow all methods and headers
                .AllowAnyMethod()
                .AllowAnyHeader()
                // Expose the gRPC-Web headers
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
        }));

        builder.Services.AddGrpc();

        // Add custom HttpClient with the certificate handler to talk to other gRPC services
        builder.Services.AddHttpClient(HttpClientName).ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            var publicKeyPath = Path.Combine(basePath,"localhost.crt");
            logger.LogInformation($"Using public key from {publicKeyPath}");

            // Load the certificate from the environment variable
            var certificate = X509Certificate2.CreateFromPemFile(publicKeyPath);

            // Expected thumbprint and issuer of the certificate for validation
            var expectedThumbprint = certificate.Thumbprint;
            var expectedIssuer = certificate.Issuer;

            logger.LogInformation($"Creating custom HttpClient with certificate handler for {expectedIssuer}");

            // Create the gRPC channels and clients with the custom certificate handler
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);

            handler.ServerCertificateCustomValidationCallback = (_, cert, _, _) =>
                cert?.Issuer == expectedIssuer && cert.Thumbprint == expectedThumbprint;

            return handler;
        });

        // Add the custom HttpClient to the service provider
        builder.Services.AddTransient(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Creating custom HttpClient with certificate handler");

            return httpClientFactory.CreateClient(HttpClientName);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        // Enable the HTTPS redirection - only use HTTPS
        app.UseHttpsRedirection();

        // Enable CORS - allow all origins and add gRPC-Web headers
        app.UseCors(CorsPolicyName);

        // Enable gRPC-Web for all services
        app.UseGrpcWeb(new() { DefaultEnabled = true });

        // Add all services in the GrpcServices namespace
        app.MapGrpcServices();

        app.Run();
    }
}