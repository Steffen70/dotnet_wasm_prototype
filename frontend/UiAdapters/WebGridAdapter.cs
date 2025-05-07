using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common.UiAdapters;
using SwissPension.WasmPrototype.Frontend.Helpers;

namespace SwissPension.WasmPrototype.Frontend.UiAdapters;

public class WebGridAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(WasmUiThreadDispatcher dispatcher, ILoggerFactory loggerFactory) : IGridAdapter<T> where T : class
{
    private readonly ILogger<WebGridAdapter<T>> _logger = loggerFactory.CreateLogger<WebGridAdapter<T>>();
    private readonly ConcurrentQueue<T> _records = new();

    public void AddRow(T item)
    {
        var wasEmpty = _records.IsEmpty;
        
        _records.Enqueue(item);
        
        if(wasEmpty)
            dispatcher.RunOnMainThread(Interop.HandleUsersReady);
    }

    public JSObject FetchGridData(int skip, int take)
    {
        try
        {
            var count = _records.Count;
            _logger.LogInformation($"Current row count is {count}");
            
            var data = _records.Skip(skip).Take(take).Select(record => record.ToJsObject()).ToList();
            
            _logger.LogInformation($"Fetched {data.Count} items (skip={skip}, take={take}, total={count})");
            
            var jsArray = Interop.CreatePlainJsArray();
            foreach (var jsItem in data) 
                Interop.PushToArray(jsArray, jsItem);
            
            var result = Interop.CreatePlainJsObject();
            result.SetProperty("data", jsArray);
            result.SetProperty("count", count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in FetchGridData: {ex.Message}");
        }
        
        return Interop.CreatePlainJsObject();
    }
}