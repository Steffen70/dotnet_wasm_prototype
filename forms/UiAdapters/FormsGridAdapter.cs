using SwissPension.WasmPrototype.Common.UiAdapters;
using System.ComponentModel;

namespace SwissPension.WasmPrototype.Forms.UiAdapters;

public class FormsGridAdapter<T> : IGridAdapter<T> where T : class
{
    public BindingList<T> BindingList { get; } = new();

    public void AddRow(T item) => BindingList.Add(item);
}
