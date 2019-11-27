using System;
using System.Collections.Generic;
using System.Text;

namespace oauth2.dingtalk.aspnetcore.Extensions
{
    public static class DictionaryExtension
    {
        public static TV TryGetValue<TK, TV>(this IDictionary<TK, TV> dictionary, TK key)
        {
            if (dictionary == null)
            {
                return default(TV);
            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(TV);
        }
    }
}
