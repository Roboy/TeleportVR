using System.Collections.Generic;
using System;

public static class DictionaryExtensions 
{
    public static TValue GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
    {
        return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
    }

    public static TValue GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
    {
        return dictionary.TryGetValue(key, out TValue value) ? value : defaultValueProvider();
    }
}
