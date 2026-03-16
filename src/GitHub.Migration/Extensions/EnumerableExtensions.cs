// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

public static class EnumerableExtensions
{
    extension(IEnumerable<KeyValuePair<string, string>> enumerable)
    {
        public IReadOnlyList<string> ToKeyValueList() =>
            enumerable.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList();
    }
}
