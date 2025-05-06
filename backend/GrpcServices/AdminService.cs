using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SwissPension.WasmPrototype.Common;

namespace SwissPension.WasmPrototype.Backend.GrpcServices;

public class AdminService : Admin.AdminBase
{
    public override Task<HelloWorldResponse> HelloWorld(Empty request, ServerCallContext context) => Task.FromResult(new HelloWorldResponse { Message = "Hello over gRPC!" });

    public async IAsyncEnumerable<User> GenerateUsers()
    {
        for (var i = 0; i < 2000; i++)
        {
            yield return new()
            {
                Name = $"User {i}",
                Department = $"Dept {i % 5}",
                Email = $"user{i}@swisspension.net"
            };
            await Task.Yield();
        }
    }

    public override async Task FetchUsers(Empty request, IServerStreamWriter<User> responseStream, ServerCallContext context)
    {
        await foreach (var user in GenerateUsers()) await responseStream.WriteAsync(user);
    }
}