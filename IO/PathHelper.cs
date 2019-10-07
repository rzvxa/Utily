using System;

namespace Utils.IO
{
    public static class PathHelper
    {
        public static string AbsoluteToRelativePath(string path, string relativeTo)
        {
            if(!path.StartsWith(relativeTo))
                throw new InvalidOperationException("Unable to make relative path");

            return path.Substring(relativeTo.Length + 1);
        }
    }
}
