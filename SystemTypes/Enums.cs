using System;

namespace Utils.SystemTypes
{
    public static class Enums
    {
        public static string[] GetNames<T>() where T : Enum =>
            Enum.GetNames(typeof(T));
    }
}
