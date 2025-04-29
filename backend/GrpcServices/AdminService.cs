
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SwissPension.WasmPrototype.Common;

namespace SwissPension.WasmPrototype.Backend.GrpcServices;

public class AdminService : Admin.AdminBase
{
    public override Task<HelloWorldResponse> HelloWorld(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new HelloWorldResponse { Message = "Hello World!" });
    }
}
