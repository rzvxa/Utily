namespace Utils.SystemTypes
{
    public static class StringExt
    {
        public static string Format(this string @this, params object[] args) =>
            string.Format(@this, args);

        public static bool NullOrEmpty(this string @this) =>
            string.IsNullOrEmpty(@this);

        public static bool NullOrWhiteSpace(this string @this) =>
            string.IsNullOrWhiteSpace(@this);
    }
}
