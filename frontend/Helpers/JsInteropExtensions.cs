using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;

namespace SwissPension.WasmPrototype.Frontend.Helpers;

public static class JsInteropExtensions
{
    public static JSObject ToJsObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
        this T data)
    {
        var jsObject = Interop.CreatePlainJsObject();

        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(data);

            switch (value)
            {
                case string s:
                    jsObject.SetProperty(property.Name, s);
                    break;
                case int i:
                    jsObject.SetProperty(property.Name, i);
                    break;
                case bool b:
                    jsObject.SetProperty(property.Name, b);
                    break;
                case double d:
                    jsObject.SetProperty(property.Name, d);
                    break;
                case float f:
                    jsObject.SetProperty(property.Name, f);
                    break;
                case null:
                    jsObject.SetProperty(property.Name, (string)null!);
                    break;
                default:
                    Program.Logger.LogWarning($"Unsupported type for property {property.Name}");
                    break;
            }
        }

        return jsObject;
    }
}