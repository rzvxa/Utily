using UnityEngine;
using UnityEditor;
using Utils.Unity.Editor;

namespace Utils.Unity.Editor
{
    public class UtilySettingsWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<UtilySettingsWindow>();
            window.titleContent = new GUIContent("Utily Settings");
            window.minSize = window.maxSize = new Vector2(320, 220);
            window._addressableUtilities = PlayerSettingsHelper
                .ScriptingDefineSymbolExists(AddressableDefine);
            window.Show();
        }

        private bool _addressableUtilities;
        private const string UtilyConfigurationDefine = "UTILY";
        private const string AddressableDefine = UtilyConfigurationDefine + "_ADDRESSABLE_ASSETS";

        private void Configure()
        {
            if (_addressableUtilities)
                PlayerSettingsHelper.AddScriptingDefineSymbol(AddressableDefine);
            else
                PlayerSettingsHelper.RemoveScriptingDefineSymbol(AddressableDefine);

            PlayerSettingsHelper.AddScriptingDefineSymbol(UtilyConfigurationDefine);
            EditorUtility.DisplayDialog("Utily Configured", "Utily Configured successfuly!", "Ok");
        }
        private void Cleanup()
        {
            PlayerSettingsHelper.RemoveScriptingDefineSymbol(AddressableDefine);
            PlayerSettingsHelper.RemoveScriptingDefineSymbol(UtilyConfigurationDefine);
            EditorUtility.DisplayDialog("Utily Remove", "Cleaned up Utily configurations,\r\nNow you can safely remove it!", "Ok");
        }

        private void OnGUI()
        {
            var labelStyle = new GUIStyle("Label");
            labelStyle.alignment = TextAnchor.UpperCenter;
            labelStyle.fontSize += 10;
            labelStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Utily: Unity Utilities", labelStyle);
            labelStyle.fontSize -= 5;
            labelStyle.fontStyle = FontStyle.Italic;
            GUILayout.Label("Settings", labelStyle);

            EditorGUILayoutHelper.DrawLine();

            var isConfigured = PlayerSettingsHelper.ScriptingDefineSymbolExists(UtilyConfigurationDefine);
            EditorGUILayout.BeginHorizontal();
            Color confColor;
            string confStatus = "Configured!";
            if (isConfigured)
            {
                confColor = Color.green;
            }
            else
            {
                confColor = Color.red;
                confStatus = "Not " + confStatus;
            }
            labelStyle.fontSize -= 5;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("Utily Library is ");
            EditorGUILayoutHelper.BeginColorZone(confColor);
            EditorGUILayout.LabelField(confStatus, labelStyle);
            EditorGUILayoutHelper.EndColorZone();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Configuration options", labelStyle);
            EditorGUI.indentLevel++;
            _addressableUtilities = EditorGUILayout
                .ToggleLeft("Addressable Utilities", _addressableUtilities);
            EditorGUI.indentLevel--;
            EditorGUILayoutHelper.Button("Configure", Configure);
            EditorGUILayoutHelper.BeginColorZone(Color.red);
            EditorGUILayoutHelper.Button("Clean", Cleanup);
            EditorGUILayoutHelper.EndColorZone();

            EditorGUILayoutHelper.DrawLine();

            EditorGUILayoutHelper.LinkField("Open in GitHub", "https://github.com/rzvxa/Utily");
            EditorGUILayout.LabelField("Utily is Licensed Under MIT");
        }
    }
}
