using System.Collections.Generic;

namespace Ozzy.Core
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetValueOrDefault(dictionary, key, default(TValue));
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue @default)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : @default;
        }
    }
}
