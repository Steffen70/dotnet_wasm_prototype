using System;
using System.Runtime.InteropServices.JavaScript;

namespace SwissPension.WasmPrototype.Frontend;

public partial class Program
{
    [JSExport]
    public static void SetText(string id, string text)
    {
        try
        {
            var el = GetElementById(id);
            if (el != null)
            {
                el.SetProperty("innerText", text);
                Console.WriteLine($"Updated element '{id}' with text: {text}");
            }
            else
            {
                Console.WriteLine($"Element with id '{id}' not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SetText: {ex.Message}");
        }
    }

    [JSImport("globalThis.document.getElementById")]
    public static partial JSObject GetElementById(string id);

    [JSImport("globalThis.handleDotnetReady")]
    public static partial void HandleDotnetReady();

    public static void Main()
    {
        Console.WriteLine("WASM loaded successfully.");

        SetText("output", "Hello from WASM-CSharp!");
        HandleDotnetReady();
    }
}