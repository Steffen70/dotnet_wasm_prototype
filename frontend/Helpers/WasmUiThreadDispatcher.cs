using System;
using System.Threading;
using System.Threading.Tasks;

namespace SwissPension.WasmPrototype.Frontend.Helpers;

public class WasmUiThreadDispatcher
{
    private readonly SynchronizationContext _mainSyncContext = SynchronizationContext.Current;

    public void RunOnMainThread(Action action)
    {
        var context = _mainSyncContext;
        if (SynchronizationContext.Current == context)
            action();
        else
            context.Post(_ => action(), null);
    }

    public Task RunOnMainThreadAsync(Action action)
    {
        var tcs = new TaskCompletionSource<object>();

        RunOnMainThread(() =>
        {
            try
            {
                action();
                tcs.SetResult(new());
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }
}
