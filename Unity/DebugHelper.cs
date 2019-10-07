using UnityEngine;

namespace Utils.Unity
{
    public static class DebugHelper
    {
        public static void LogColored(object message, Color messageColor)
        {
            var hexColor = ColorUtility.ToHtmlStringRGB(messageColor);
            Debug.unityLogger.LogFormat(LogType.Log, "<color=#{0}>{1}</color>", hexColor, message);
        }
    }
}
