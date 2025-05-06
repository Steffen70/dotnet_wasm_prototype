using SwissPension.WasmPrototype.Common.UiAdapters;
using SwissPension.WasmPrototype.Frontend.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace SwissPension.WasmPrototype.Frontend.UiAdapters;

public class WebGridAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T> : IGridAdapter<T> where T : class
{
    public void AddRow(T item)
    {
        Interop.AddRecordToGrid(item.ToJsObject());
    }
}