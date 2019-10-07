using System.IO;
using System.Collections.Generic;

namespace Utils.IO
{
    public static class DirectoryHelper
    {
        public static IEnumerable<string> DirectorySearch(string sDir)
        {
            var files = new List<string>();
            foreach (var d in Directory.GetDirectories(sDir))
            {
                foreach (var f in Directory.GetFiles(d))
                    files.Add(f);
                files.AddRange(DirectorySearch(d));
            }
            return files;
        }
    }
}
