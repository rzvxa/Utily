using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using Utils.SystemTypes;

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

        private static readonly Stack<bool> _editorActivityStack = new
            Stack<bool>();

        public static void BeginActivityGroup(bool isActive)
        {
            _editorActivityStack.Push(GUI.enabled);
            GUI.enabled = isActive;
        }

        public static void EndActivityGroup()
        {
            var enabled = _editorActivityStack.Pop();
            GUI.enabled = enabled;
        }


        public static string Browse(string path, string name)
        {
            string res;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(name);
            res = EditorGUILayout.TextField(path);
            if (GUILayout.Button("..."))
                res = EditorUtility.OpenFolderPanel
                    ("Source of Resources",
                     EditorApplication.applicationPath,
                     "");
            EditorGUILayout.EndHorizontal();
            return res;
        }

        public static void Button(string text, Action onClick)
        {
            if (GUILayout.Button(text))
                onClick?.Invoke();
        }

        public static void LinkField(string link) => LinkField(link, link);



        public static void LinkField(string label, string link) =>
            LinkField(label, () => Application.OpenURL(link));


        public static void LinkField(string label, Action onClick)
        {
            var linkStyle = new GUIStyle(EditorStyles.label);
            linkStyle.wordWrap = false;
            linkStyle.normal.textColor = new Color(0x00 / 255f,
                                                   0x78 / 255f,
                                                   0xDA / 255f,
                                                   1f);
            linkStyle.stretchWidth = false;
            var position = GUILayoutUtility.GetRect(new GUIContent(label), linkStyle);

            Handles.BeginGUI();
            Handles.color = linkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax),
                             new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            if (GUI.Button(position, label, linkStyle))
                onClick?.Invoke();
        }

        public static T SelectionGrid<T>(T selectedOption, int xCount, GUIStyle style = null)
            where T : Enum, IConvertible
        {
            if (style is null)
                style = GUI.skin.GetStyle("Button");
            // NOTE: boxing upboxing is needed for converting int to T and vise versa
            selectedOption = (T)(object) GUILayout.SelectionGrid((int)(object) selectedOption,
                                                         Enums.GetNames<T>(),
                                                         xCount,
                                                         style);
            return selectedOption;
        }
    }
}
