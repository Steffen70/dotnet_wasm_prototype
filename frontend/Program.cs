using System;

namespace SwissPension.WasmPrototype.Frontend;

public static partial class Program
{
    internal static void SetText(string id, string text)
    {
        try
        {
            var el = Interop.GetElementById(id);
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
    
    public static void Main()
    {
        Console.WriteLine("WASM loaded successfully.");

        SetText("output", "Hello from WASM-CSharp!");
        Interop.HandleDotnetReady();
    }
}