using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace SwissPension.WasmPrototype.Frontend.Helpers;

public class WasmLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, WasmLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new(name));

    public void Dispose()
    {
    }
}

public class WasmLogger(string categoryName) : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        Console.WriteLine($"[{logLevel}] {categoryName}: {message}");

        if (exception != null)
            Console.WriteLine(exception);
    }
}