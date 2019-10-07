using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Utils.Unity.Editor
{
    public static class EditorGUILayoutHelper
    {
        public static void PopGenericMenuAsDropDown
            (GUIContent label,
             GUIContent btnContent,
             GenericMenu genericMenu,
             Color? labelColor = null,
             Color? contentColor = null)
        {
            var defaultColor = GUI.contentColor;

            if (labelColor is null) labelColor = defaultColor;
            if (contentColor is null) contentColor = defaultColor;

            GUI.contentColor = labelColor.Value;
            EditorGUILayout.LabelField(label);
            var theRect = GUILayoutUtility.GetRect(btnContent, EditorStyles.popup);
            GUI.contentColor = contentColor.Value;
            if (GUI.Button(theRect, btnContent, EditorStyles.popup))
                genericMenu.DropDown(theRect);
            GUI.contentColor = defaultColor;
        }

        public static void DrawLine(Color? color = null, int thickness = 2, int padding = 10)
        {
            if (!color.HasValue) color = Color.gray;
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            rect.height = thickness;
            rect.y += padding / 2;
            rect.x -= 2;
            rect.width += 6;
            EditorGUI.DrawRect(rect, color.Value);
        }

        public static bool LastControlRectClicked(int mouseButton)
        {
            var rect = GUILayoutUtility.GetLastRect();
            return RectClicked(mouseButton, rect);
        }

        public static bool RectClicked(int mouseButton, Rect rect)
        {
            return (Event.current.type == EventType.MouseDown
                    && Event.current.button == mouseButton && rect.Contains(Event.current.mousePosition));
        }

        private static readonly Stack<Color> _editorColorStack = new Stack<Color>();

        public static void BeginColorZone(Color color)
        {
            _editorColorStack.Push(GUI.color);
            GUI.color = color;
        }

        public static void EndColorZone()
        {
            var color = _editorColorStack.Pop();
            GUI.color = color;
        }


        public static string Browse(string path, string name)
        {
            string res;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(name);
            res = EditorGUILayout.TextField(path);
            if(GUILayout.Button("..."))
                res = EditorUtility.OpenFolderPanel
                    ("Source of Resources",
                     EditorApplication.applicationPath,
                     "");
            EditorGUILayout.EndHorizontal();
            return res;
        }
    }
}
