using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public static class Util
    {
        // https://stackoverflow.com/a/16193323/419956 by @AdamHouldsWorth
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            return dict.GetOrCreate(key, new TValue());
        }

        // https://stackoverflow.com/a/16193323/419956 by @AdamHouldsWorth
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultIfNull)
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = defaultIfNull;
                dict.Add(key, val);
            }

            return val;
        }
    }
}
