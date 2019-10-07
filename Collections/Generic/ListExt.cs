using System.Collections.Generic;

namespace Utils.Collections.Generic
{
    public static class ListExt
    {
        public static void Push<T> (this List<T> @this, T element)
        {
            @this.Add(element);
        }

        public static T Pop<T> (this List<T> @this)
        {
            var x = @this.Count - 1;
            var element = @this[x];
            @this.RemoveAt(x);
            return element;
        }
    }
}
