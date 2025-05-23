using System.Reflection;

namespace SwissPension.WasmPrototype.Backend.Extensions;

public static class ApplicationBuilderExtensions
{
    private const string ServiceNamespace = ".GrpcServices";

    public static IApplicationBuilder MapGrpcServices(this IApplicationBuilder app, string serviceNamespace = ServiceNamespace)
    {
        // Get the base namespace of the assembly
        var baseNamespace = Assembly.GetExecutingAssembly().GetName().Name;

        // Combine the base namespace with the service namespace
        var serviceTypeNamespace = $"{baseNamespace}{ServiceNamespace}";

        // Get all service types in the assembly
        var serviceTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.Namespace?.StartsWith(serviceTypeNamespace) == true);

        // Filter out nested types - only top-level service types (gRPC services)
        serviceTypes = serviceTypes.Where(x => !x.IsNested);

        // Map each service type
        foreach (var serviceTypeToMap in serviceTypes)
        {
            var method = typeof(GrpcEndpointRouteBuilderExtensions)
                .GetMethod(nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService))
                !.MakeGenericMethod(serviceTypeToMap);

            _ = method.Invoke(null, [app]);
        }

        return app;
    }
}