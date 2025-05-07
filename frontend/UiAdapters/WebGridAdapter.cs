using System.Diagnostics.CodeAnalysis;
using SwissPension.WasmPrototype.Common.UiAdapters;
using SwissPension.WasmPrototype.Frontend.Helpers;

namespace SwissPension.WasmPrototype.Frontend.UiAdapters;

public class WebGridAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(WasmUiThreadDispatcher wasmUiThreadDispatcher) : IGridAdapter<T> where T : class
{
    public void AddRow(T item)
    {
        wasmUiThreadDispatcher.RunOnMainThread(() => { Interop.AddRecordToGrid(item.ToJsObject()); });
    }
}