using UnityEditor;

namespace Utils.Unity.Editor
{
    public static class UtilyMenu
    {
        private const string MainMenu = "Utily/";

        [MenuItem(MainMenu + "Search For Components")]
        public static void OpenSearchForComponentsWindow() =>
            SearchForComponentsWindow.ShowWindow();

        [MenuItem(MainMenu + "Settings")]
        public static void OpenSettingsWindow() =>
            UtilySettingsWindow.ShowWindow();
    }
}
