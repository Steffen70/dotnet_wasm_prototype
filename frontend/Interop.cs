using System.Runtime.InteropServices.JavaScript;

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

    [JSImport("globalThis.addRecordToGrid")]
    internal static partial void AddRecordToGrid(JSObject record);
    
    [JSExport]
    internal static void SetText(string id, string text) => Program.SetText(id, text);
    
    // fire-and-forget
    [JSExport]
    internal static void HelloWorld() => Program.HelloWorldAsync(); 
    
    [JSExport]
    internal static void FetchUsers() => Program.FetchUsersAsync(); 
}