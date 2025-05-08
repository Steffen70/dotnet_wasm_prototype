using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace SwissPension.WasmPrototype.Frontend;

internal static partial class Interop
{
    [JSImport("globalThis.document.getElementById")]
    internal static partial JSObject GetElementById(string id);

    [JSImport("globalThis.createPlainJsObject")]
    internal static partial JSObject CreatePlainJsObject();
    
    [JSImport("globalThis.createPlainJsArray")]
    internal static partial JSObject CreatePlainJsArray();
    
    [JSImport("globalThis.Array.prototype.push.call")]
    internal static partial int PushToArray(JSObject array, JSObject item);
    
    [JSImport("globalThis.handleDotnetReady")]
    internal static partial void HandleDotnetReady();
    
    [JSImport("globalThis.handleFirstPageReady")]
    internal static partial void HandleFirstPageReady(string gridId);
    
    [JSImport("globalThis.handleStreamEnd")]
    internal static partial void HandleStreamEnd(string gridId, int totalCount);

    [JSExport]
    internal static void SetText(string id, string text) => Program.SetText(id, text);

    [JSExport]
    internal static JSObject FetchGridData(int skip, int take) => Program.UserGridAdapter.FetchGridData(skip, take);

    // fire-and-forget
    [JSExport]
    internal static void HelloWorld() => Task.Run(Program.UserService.HelloWorldAsync);

    [JSExport]
    internal static void FetchUsers() => Task.Run(Program.UserService.FetchUsersAsync);
}