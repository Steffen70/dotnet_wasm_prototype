using System.ComponentModel;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common.UiAdapters;

namespace SwissPension.WasmPrototype.Forms.UiAdapters;

public class FormsGridAdapter<T>(ILoggerFactory loggerFactory) : IGridAdapter<T> where T : class
{
    private readonly ILogger<FormsGridAdapter<T>> _logger = loggerFactory.CreateLogger<FormsGridAdapter<T>>();
    public BindingList<T> BindingList { get; } = new();

    public void AddRow(T item) => BindingList.Add(item);

    public void StreamEnd() => _logger.LogInformation($"Stream ended, total records: {BindingList.Count}");
}