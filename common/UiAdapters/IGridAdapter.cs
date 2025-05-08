

namespace SwissPension.WasmPrototype.Common.UiAdapters;

public interface IGridAdapter<in T> where T : class
{
    void AddRow(T item);
    void StreamEnd();
}