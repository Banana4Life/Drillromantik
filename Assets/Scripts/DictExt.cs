using System.Collections.Generic;

public static class DictExt
{
    public static TV GetOrElse<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV def = default)
    {
        if (dict.TryGetValue(key, out var value))
        {
            return value;
        }

        return def;
    }
}