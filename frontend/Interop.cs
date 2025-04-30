using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;

namespace SwissPension.WasmPrototype.Frontend;

internal static partial class Interop
{
    [JSImport("globalThis.document.getElementById")]
    internal static partial JSObject GetElementById(string id);

    [JSImport("globalThis.handleDotnetReady")]
    internal static partial void HandleDotnetReady();
    
    [JSExport]
    internal static void SetText(string id, string text) => Program.SetText(id, text);
    
    // fire-and-forget
    [JSExport]
    internal static void SayHelloWorld() => Program.CallAndRespondAsync(); 
}