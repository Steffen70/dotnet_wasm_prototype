using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace SwissPension.WasmPrototype.Frontend;

internal static partial class Interop
{
    [JSImport("globalThis.document.getElementById")]
    internal static partial JSObject GetElementById(string id);

    [JSImport("globalThis.document.createElement")]
    internal static partial JSObject CreateElement(string tag);

    [JSImport("globalThis.document.createTextNode")]
    internal static partial JSObject CreateTextNode(string text);

    [JSImport("globalThis.Node.prototype.appendChild.call")]
    internal static partial void AppendChild(JSObject parent, JSObject child);

    [JSImport("globalThis.HTMLElement.prototype.remove.call")]
    internal static partial void RemoveElement(JSObject element);

    [JSImport("globalThis.handleDotnetReady")]
    internal static partial void HandleDotnetReady();

    [JSImport("globalThis.createPlainJsObject")]
    internal static partial JSObject CreatePlainJsObject();
    
    [JSImport("globalThis.createPlainJsArray")]
    internal static partial JSObject CreatePlainJsArray();
    
    [JSImport("Array.prototype.push.call")]
    internal static partial int PushToArray(JSObject array, JSObject item);
    
    [JSImport("globalThis.handleUsersReady")]
    internal static partial void HandleUsersReady();

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