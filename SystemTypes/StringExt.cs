namespace Utils.SystemTypes
{
    public static class StringExt
    {
        public static string Format(this string @this, params object[] args) =>
            string.Format(@this, args);
    }
}
