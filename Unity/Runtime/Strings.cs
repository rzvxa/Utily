using UnityEngine;

namespace Utils.Unity.Runtime.Strings
{
    public static class StringsExt
    {
        public static void CopyToClipboard(this string s)
        {
            TextEditor te = new TextEditor();
            te.text = s;
            te.SelectAll();
            te.Copy();
        }
    }
}
