using System.Collections.Generic;

namespace TryGet.Extensions
{
    public static class ReadOnlyTryGetExtensions
    {
        public static TryGetResult<TValue> TryGet<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            var success = dictionary.TryGetValue(key, out var val);
            return new TryGetResult<TValue>(success, val);
        }
    }
}
