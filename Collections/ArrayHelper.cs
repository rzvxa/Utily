using System;
namespace Utils.Collections
{
    public static class ArrayHelper
    {
        public static T[] Slice<T>(this T[] @this, int index, int length)
        {
            T[] slice = new T[length];
            Array.Copy(@this, index, slice, 0, length);
            return slice;
        }
    }
}
