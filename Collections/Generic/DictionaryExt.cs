using System;
using System.Collections.Generic;

namespace Utils.Collections.Generic
{
    public static class DictionaryExt
    {
        public static void UnionWith<T,TY>(this Dictionary<T, TY> @this, Dictionary<T, TY> other)
        {
            if(other is null)
                throw new ArgumentNullException("You cant union with empty dictionary.");

            foreach (var item in other)
            {
                if(!@this.ContainsKey(item.Key))
                    @this.Add(item.Key, item.Value);
            }
        }
    }
}
