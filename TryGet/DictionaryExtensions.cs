using System;
using System.Collections.Generic;
using System.Text;

namespace TryGet
{
    public static class DictionaryExtensions
    {
        public static TryGetResult<TValue> TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var success = dictionary.TryGetValue(key, out var val);
            return new TryGetResult<TValue>(success, val);
        }
    }
}
