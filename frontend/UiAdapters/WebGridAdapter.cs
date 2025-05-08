using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common.UiAdapters;
using SwissPension.WasmPrototype.Frontend.Helpers;

namespace SwissPension.WasmPrototype.Frontend.UiAdapters;

public class WebGridAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(WasmUiThreadDispatcher dispatcher, ILoggerFactory loggerFactory, string gridId) : IGridAdapter<T> where T : class
{
    private readonly ILogger<WebGridAdapter<T>> _logger = loggerFactory.CreateLogger<WebGridAdapter<T>>();
    private readonly ConcurrentQueue<T> _records = new();

    public void AddRow(T item)
    {
        var firstPageLoaded = _records.Count == 10;

        _records.Enqueue(item);

        if (!firstPageLoaded) return;
        
        _logger.LogInformation("First page loaded, signalling users ready...");
        dispatcher.RunOnMainThread(() => Interop.HandleFirstPageReady(gridId));
    }

    public void StreamEnd()
    {
        _logger.LogInformation($"Stream ended, total records: { _records.Count }");
        dispatcher.RunOnMainThread(() => Interop.HandleStreamEnd(gridId, _records.Count));
    }

    private class GridPage
    {
        public JSObject Data { get; init; }
        public int Count { get; init; }
    }

    public JSObject FetchGridData(int skip, int take)
    {
        try
        {
            var count = _records.Count;

            var jsArray = Interop.CreatePlainJsArray();
            var itemsFetched = 0;
            foreach (var jsItem in _records.Skip(skip).Take(take).Select(item => item.ToJsObject()))
            {
                itemsFetched++;
                Interop.PushToArray(jsArray, jsItem);
            }
            var result = new GridPage{  Data = jsArray, Count = count };

            _logger.LogInformation($"Fetched {itemsFetched} items (skip={skip}, take={take}, total={count})");

            var jsResult = result.ToJsObject();
            return jsResult;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in FetchGridData: {ex.Message}");
        }

        return Interop.CreatePlainJsObject();
    }
}