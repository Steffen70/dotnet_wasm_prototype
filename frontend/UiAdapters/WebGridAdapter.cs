using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        _logger.LogInformation($"Stream ended, total records: {_records.Count}");
        dispatcher.RunOnMainThread(() => Interop.HandleStreamEnd(gridId, _records.Count));
    }

    public JSObject FetchPaginatedData(int skip, int take)
    {
        _logger.LogInformation($"Fetching paginated data with skip: {skip}, take: {take}");
        return GetJsResult(_records.ToList(), skip, take);
    }

    public JSObject FetchSortedData(JSObject sortCriteria, int skip, int take)
    {
        var sortedData = new SortCriteria(sortCriteria, loggerFactory).FetchSortedData(_records);

        return GetJsResult(sortedData.ToList(), skip, take);
    }

    public JSObject FetchFilteredData(JSObject filterCriteria, int skip, int take)
    {
        var filteredData = FilterCriteria.FetchFilteredData(FilterCriteria.GetFilterCriteria(filterCriteria, loggerFactory), _records);
        return GetJsResult(filteredData.ToList(), skip, take);
    }

    public JSObject FetchFilteredAndSortedData(JSObject filterCriteria, JSObject sortCriteria, int skip, int take)
    {
        var filteredData = FilterCriteria.FetchFilteredData(FilterCriteria.GetFilterCriteria(filterCriteria, loggerFactory), _records);
        var sortedData = new SortCriteria(sortCriteria, loggerFactory).FetchSortedData(filteredData);

        return GetJsResult(sortedData.ToList(), skip, take);
    }

    private JSObject GetJsResult(IList<T> records, int skip, int take)
    {
        var items = records.Skip(skip).Take(take).Select(item => item.ToJsObject()).ToList();

        var count = records.Count;

        var jsArray = Interop.CreatePlainJsArray();
        var itemsFetched = 0;
        foreach (var jsItem in items)
        {
            itemsFetched++;
            Interop.PushToArray(jsArray, jsItem);
        }

        var result = new GridPage { Data = jsArray, Count = count };

        _logger.LogInformation($"Fetched {itemsFetched} items, selected records: {count}, total records: {_records.Count}");

        var jsResult = result.ToJsObject();
        return jsResult;
    }

    private class GridPage
    {
        public JSObject Data { get; init; }
        public int Count { get; init; }
    }

    private class SortCriteria(JSObject sortCriteria, ILoggerFactory loggerFactory)
    {
        private readonly ILogger<SortCriteria> _logger = loggerFactory.CreateLogger<SortCriteria>();
        private string PropertyName { get; } = sortCriteria.GetPropertyAsString("fieldName")!;
        private string SortDirection { get; } = sortCriteria.GetPropertyAsString("direction")!;

        public IEnumerable<T> FetchSortedData(IEnumerable<T> records)
        {
            _logger.LogInformation($"Fetching sorted data with propertyName: {PropertyName}, direction: {SortDirection}");

            var keySelector = (T item) => item.GetType().GetProperty(PropertyName)!.GetValue(item);
            return SortDirection == "ascending" ? records.OrderBy(keySelector) : records.OrderByDescending(keySelector);
        }
    }

    private class FilterCriteria
    {
        public static IEnumerable<FilterCriteria> GetFilterCriteria(JSObject filterCriteria, ILoggerFactory loggerFactory)
        {
            var arrayLengthDiscriptor = Interop.GetArrayLengthDescriptor(filterCriteria, "length");
            var arrayLength = arrayLengthDiscriptor.GetPropertyAsInt32("value");
            var logger = loggerFactory.CreateLogger<FilterCriteria>();
            logger.LogInformation($"Processing {arrayLength} filter criteria");
            
            // yield return new(Interop.GetArrayItem(filterCriteria, 0), logger);
            
            for (var i = 0; i < arrayLength; i++)
            {
                var filterItem = Interop.GetArrayItem(filterCriteria, i);
                yield return new(filterItem, logger);
            }
        }

        public static IEnumerable<T> FetchFilteredData(IEnumerable<FilterCriteria> filterCriteriaEnumerable, IEnumerable<T> records)
            => filterCriteriaEnumerable.Aggregate(records, (currentRecords, filterCriteria) => filterCriteria.FetchFilteredData(currentRecords));

        private readonly ILogger<FilterCriteria> _logger;

        private readonly Dictionary<string, IFilterStrategy> _strategies = new()
        {
            { "startswith", new StartsWithStrategy() },
            { "endswith", new EndsWithStrategy() },
            { "contains", new ContainsStrategy() },
            { "equals", new EqualsStrategy() },
            { "doesnotstartwith", new DoesNotStartWithStrategy() },
            { "doesnotendwith", new DoesNotEndWithStrategy() },
            { "doesnotcontain", new DoesNotContainStrategy() },
            { "isempty", new IsEmptyStrategy() },
            { "isnotempty", new IsNotEmptyStrategy() }
        };

        private FilterCriteria(JSObject filterCriteria, ILogger<FilterCriteria> logger)
        {
            _logger = logger;
            PropertyName = filterCriteria.GetPropertyAsString("field")!;
            Operator = filterCriteria.GetPropertyAsString("operator")!;
            Value = filterCriteria.GetPropertyAsString("value") ?? "";
        }

        private string PropertyName { get; }
        private string Operator { get; }
        private string Value { get; }

        private IEnumerable<T> FetchFilteredData(IEnumerable<T> records)
        {
            _logger.LogInformation($"Fetching filtered data with propertyName: {PropertyName}, operator: {Operator}, value: {Value}");
            return _strategies[Operator].FetchFilteredData(records, item => item.GetType().GetProperty(PropertyName)!.GetValue(item), Value);
        }

        private interface IFilterStrategy
        {
            IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value);
        }

        private class StartsWithStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => keySelector(record).ToString()!.StartsWith(value));
        }

        private class EndsWithStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => keySelector(record).ToString()!.EndsWith(value));
        }

        private class ContainsStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => keySelector(record).ToString()!.Contains(value));
        }

        private class EqualsStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => keySelector(record).ToString() == value);
        }

        private class DoesNotStartWithStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => !keySelector(record).ToString()!.StartsWith(value));
        }

        private class DoesNotEndWithStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => !keySelector(record).ToString()!.EndsWith(value));
        }

        private class IsNotEmptyStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string _) => records.Where(record => !string.IsNullOrEmpty(keySelector(record).ToString()));
        }

        private class IsEmptyStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string _) => records.Where(record => string.IsNullOrEmpty(keySelector(record).ToString()));
        }

        private class DoesNotContainStrategy : IFilterStrategy
        {
            public IEnumerable<T> FetchFilteredData(IEnumerable<T> records, Func<T, object> keySelector, string value) => records.Where(record => !keySelector(record).ToString()!.Contains(value));
        }
    }
}