using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SwissPension.WasmPrototype.Common.UiAdapters;
using SwissPension.WasmPrototype.Frontend.Helpers;

namespace SwissPension.WasmPrototype.Frontend.UiAdapters;

public class WebGridAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(WasmUiThreadDispatcher dispatcher, ILoggerFactory loggerFactory) : IGridAdapter<T> where T : class
{
    private readonly ILogger<WebGridAdapter<T>> _logger = loggerFactory.CreateLogger<WebGridAdapter<T>>();
    private readonly ConcurrentQueue<T> _pendingRows = new();
    private int _flushScheduled;

    public void AddRow(T item)
    {
        _pendingRows.Enqueue(item);

        ScheduleFlush();
    }

    private void ScheduleFlush()
    {
        // Only schedule once per batch
        if (Interlocked.CompareExchange(ref _flushScheduled, 1, 0) == 1)
            return;

        // Introduce a delay to allow batching
        _ = Task.Run(async () =>
        {
            // Adjust delay to balance latency vs batching
            await Task.Delay(500); 
            await dispatcher.RunOnMainThreadAsync(() =>
            {
                FlushPending();
                _flushScheduled = 0;
            });
        });
    }

    private void FlushPending()
    {
        var batch = new List<JSObject>();

        while (_pendingRows.TryDequeue(out var row)) batch.Add(row.ToJsObject());

        if (batch.Count <= 0) return;

        _logger.LogInformation($"Flushing {batch.Count} rows...");

        // still interop per row, but now batched
        foreach (var row in batch) Interop.AddRecordToGrid(row);
    }
}