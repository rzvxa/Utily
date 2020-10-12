// I found this on unity forums,
// https://forum.unity.com/threads/editor-want-to-check-all-prefabs-in-a-project-for-an-attached-monobehaviour.253149/
// It's not lincensed but i like to give creadit to original author
// I've changed deprecated APIs so it can work nicely with newer versions of Unity3D
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace Utils.Unity.Editor
{
    public class SearchForComponentsWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<SearchForComponentsWindow>();
            window.Show();
            window.position = new Rect(20, 80, 550, 500);
        }

        private string[] _modes = new string[] { "Search for component usage", "Search for missing components" };
        private string[] _checkType = new string[] { "Check single component", "Check all components" };

        private List<string> _listResult;
        private List<ComponentNames> _prefabComponents;
        private List<ComponentNames> _notUsedComponents;
        private List<ComponentNames> _addedComponents;
        private List<ComponentNames> _existingComponents;
        private List<ComponentNames> _sceneComponents;
        private int _editorMode;
        private int _selectedCheckType;
        private bool _recursionVal;
        private MonoScript _targetComponent;
        private string _componentName = "";

        private bool _showPrefabs;
        private bool _showAdded;
        private bool _showScene;
        private bool _showUnused = true;
        private Vector2 _scroll;
        private Vector2 _scroll1;
        private Vector2 _scroll2;
        private Vector2 _scroll3;
        private Vector2 _scroll4;

        private class ComponentNames
        {
            public string ComponentName;
            public string NamespaceName;
            public string AssetPath;
            public List<string> UsageSource;
            public ComponentNames(string comp, string space, string path)
            {
                ComponentName = comp;
                NamespaceName = space;
                AssetPath = path;
                UsageSource = new List<string>();
            }
            public override bool Equals(object obj)
            {
                return ((ComponentNames)obj).ComponentName == ComponentName && ((ComponentNames)obj).NamespaceName == NamespaceName;
            }
            public override int GetHashCode()
            {
                return ComponentName.GetHashCode() + NamespaceName.GetHashCode();
            }
        }

        private void OnGUI()
        {
            GUILayout.Label(position + "");
            GUILayout.Space(3);
            int oldValue = GUI.skin.window.padding.bottom;
            GUI.skin.window.padding.bottom = -20;
            Rect windowRect = GUILayoutUtility.GetRect(1, 17);
            windowRect.x += 4;
            windowRect.width -= 7;
            _editorMode = GUI.SelectionGrid(windowRect, _editorMode, _modes, 2, "Window");
            GUI.skin.window.padding.bottom = oldValue;

            switch (_editorMode)
            {
                case 0:
                    _selectedCheckType = GUILayout.SelectionGrid(_selectedCheckType, _checkType, 2, "Toggle");
                    _recursionVal = GUILayout.Toggle(_recursionVal, "Search all dependencies");
                    GUI.enabled = _selectedCheckType == 0;
                    _targetComponent = (MonoScript)EditorGUILayout.ObjectField(_targetComponent, typeof(MonoScript), false);
                    GUI.enabled = true;

                    if (GUILayout.Button("Check component usage"))
                    {
                        AssetDatabase.SaveAssets();
                        switch (_selectedCheckType)
                        {
                            case 0:
                                _componentName = _targetComponent.name;
                                string targetPath = AssetDatabase.GetAssetPath(_targetComponent);
                                string[] allPrefabs = GetAllPrefabs();
                                _listResult = new List<string>();
                                foreach (string prefab in allPrefabs)
                                {
                                    string[] single = new string[] { prefab };
                                    string[] dependencies = AssetDatabase.GetDependencies(single, _recursionVal);
                                    foreach (string dependedAsset in dependencies)
                                    {
                                        if (dependedAsset == targetPath)
                                        {
                                            _listResult.Add(prefab);
                                        }
                                    }
                                }
                                break;
                            case 1:
                                List<string> scenesToLoad = new List<string>();
                                _existingComponents = new List<ComponentNames>();
                                _prefabComponents = new List<ComponentNames>();
                                _notUsedComponents = new List<ComponentNames>();
                                _addedComponents = new List<ComponentNames>();
                                _sceneComponents = new List<ComponentNames>();

                                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                {
                                    string projectPath = Application.dataPath;
                                    projectPath = projectPath.Substring(0, projectPath.IndexOf("Assets"));

                                    string[] allAssets = AssetDatabase.GetAllAssetPaths();

                                    foreach (string asset in allAssets)
                                    {
                                        int indexCS = asset.IndexOf(".cs");
                                        int indexJS = asset.IndexOf(".js");
                                        if (indexCS != -1 || indexJS != -1)
                                        {
                                            ComponentNames newComponent = new ComponentNames(NameFromPath(asset), "", asset);
                                            try
                                            {
                                                System.IO.FileStream FS = new System.IO.FileStream(projectPath + asset, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                                                System.IO.StreamReader SR = new System.IO.StreamReader(FS);
                                                string line;
                                                while (!SR.EndOfStream)
                                                {
                                                    line = SR.ReadLine();
                                                    int index1 = line.IndexOf("namespace");
                                                    int index2 = line.IndexOf("{");
                                                    if (index1 != -1 && index2 != -1)
                                                    {
                                                        line = line.Substring(index1 + 9);
                                                        index2 = line.IndexOf("{");
                                                        line = line.Substring(0, index2);
                                                        line = line.Replace(" ", "");
                                                        newComponent.NamespaceName = line;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }

                                            _existingComponents.Add(newComponent);

                                            try
                                            {
                                                System.IO.FileStream FS = new System.IO.FileStream(projectPath + asset, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                                                System.IO.StreamReader SR = new System.IO.StreamReader(FS);

                                                string line;
                                                int lineNum = 0;
                                                while (!SR.EndOfStream)
                                                {
                                                    lineNum++;
                                                    line = SR.ReadLine();
                                                    int index = line.IndexOf("AddComponent");
                                                    if (index != -1)
                                                    {
                                                        line = line.Substring(index + 12);
                                                        if (line[0] == '(')
                                                        {
                                                            line = line.Substring(1, line.IndexOf(')') - 1);
                                                        }
                                                        else if (line[0] == '<')
                                                        {
                                                            line = line.Substring(1, line.IndexOf('>') - 1);
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                        line = line.Replace(" ", "");
                                                        line = line.Replace("\"", "");
                                                        index = line.LastIndexOf('.');
                                                        ComponentNames newComp;
                                                        if (index == -1)
                                                        {
                                                            newComp = new ComponentNames(line, "", "");
                                                        }
                                                        else
                                                        {
                                                            newComp = new ComponentNames(line.Substring(index + 1, line.Length - (index + 1)), line.Substring(0, index), "");
                                                        }
                                                        string pName = asset + ", Line " + lineNum;
                                                        newComp.UsageSource.Add(pName);
                                                        index = _addedComponents.IndexOf(newComp);
                                                        if (index == -1)
                                                        {
                                                            _addedComponents.Add(newComp);
                                                        }
                                                        else
                                                        {
                                                            if (!_addedComponents[index].UsageSource.Contains(pName)) _addedComponents[index].UsageSource.Add(pName);
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        int indexPrefab = asset.IndexOf(".prefab");

                                        if (indexPrefab != -1)
                                        {
                                            string[] single = new string[] { asset };
                                            string[] dependencies = AssetDatabase.GetDependencies(single, _recursionVal);
                                            foreach (string dependedAsset in dependencies)
                                            {
                                                if (dependedAsset.IndexOf(".cs") != -1 || dependedAsset.IndexOf(".js") != -1)
                                                {
                                                    ComponentNames newComponent = new ComponentNames(NameFromPath(dependedAsset), GetNamespaceFromPath(dependedAsset), dependedAsset);
                                                    int index = _prefabComponents.IndexOf(newComponent);
                                                    if (index == -1)
                                                    {
                                                        newComponent.UsageSource.Add(asset);
                                                        _prefabComponents.Add(newComponent);
                                                    }
                                                    else
                                                    {
                                                        if (!_prefabComponents[index].UsageSource.Contains(asset)) _prefabComponents[index].UsageSource.Add(asset);
                                                    }
                                                }
                                            }
                                        }
                                        int indexUnity = asset.IndexOf(".unity");
                                        if (indexUnity != -1)
                                        {
                                            scenesToLoad.Add(asset);
                                        }
                                    }

                                    for (int i = _addedComponents.Count - 1; i > -1; i--)
                                    {
                                        _addedComponents[i].AssetPath = GetPathFromNames(_addedComponents[i].NamespaceName, _addedComponents[i].ComponentName);
                                        if (_addedComponents[i].AssetPath == "") _addedComponents.RemoveAt(i);

                                    }

                                    foreach (string scene in scenesToLoad)
                                    {
                                        EditorSceneManager.OpenScene(scene);
                                        GameObject[] sceneGOs = GetAllObjectsInScene();
                                        foreach (GameObject g in sceneGOs)
                                        {
                                            Component[] comps = g.GetComponentsInChildren<Component>(true);
                                            foreach (Component c in comps)
                                            {

                                                if (c != null && c.GetType() != null && c.GetType().BaseType != null && c.GetType().BaseType == typeof(MonoBehaviour))
                                                {
                                                    SerializedObject so = new SerializedObject(c);
                                                    SerializedProperty p = so.FindProperty("m_Script");
                                                    string path = AssetDatabase.GetAssetPath(p.objectReferenceValue);
                                                    ComponentNames newComp = new ComponentNames(NameFromPath(path), GetNamespaceFromPath(path), path);
                                                    newComp.UsageSource.Add(scene);
                                                    int index = _sceneComponents.IndexOf(newComp);
                                                    if (index == -1)
                                                    {
                                                        _sceneComponents.Add(newComp);
                                                    }
                                                    else
                                                    {
                                                        if (!_sceneComponents[index].UsageSource.Contains(scene)) _sceneComponents[index].UsageSource.Add(scene);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    foreach (ComponentNames c in _existingComponents)
                                    {
                                        if (_addedComponents.Contains(c)) continue;
                                        if (_prefabComponents.Contains(c)) continue;
                                        if (_sceneComponents.Contains(c)) continue;
                                        _notUsedComponents.Add(c);
                                    }

                                    _addedComponents.Sort(SortAlphabetically);
                                    _prefabComponents.Sort(SortAlphabetically);
                                    _sceneComponents.Sort(SortAlphabetically);
                                    _notUsedComponents.Sort(SortAlphabetically);
                                }
                                break;
                        }
                    }
                    break;
                case 1:
                    if (GUILayout.Button("Search!"))
                    {
                        string[] allPrefabs = GetAllPrefabs();
                        _listResult = new List<string>();
                        foreach (string prefab in allPrefabs)
                        {
                            UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
                            GameObject go;
                            try
                            {
                                go = (GameObject)o;
                                Component[] components = go.GetComponentsInChildren<Component>(true);
                                foreach (Component c in components)
                                {
                                    if (c == null)
                                    {
                                        _listResult.Add(prefab);
                                    }
                                }
                            }
                            catch
                            {
                                Debug.Log("For some reason, prefab " + prefab + " won't cast to GameObject");
                            }
                        }
                    }
                    break;
            }
            if (_editorMode == 1 || _selectedCheckType == 0)
            {
                if (_listResult != null)
                {
                    if (_listResult.Count == 0)
                    {
                        GUILayout.Label(_editorMode == 0 ? (_componentName == "" ? "Choose a component" : "No prefabs use component " + _componentName) : ("No prefabs have missing components!\nClick Search to check again"));
                    }
                    else
                    {
                        GUILayout.Label(_editorMode == 0 ? ("The following " + _listResult.Count + " prefabs use component " + _componentName + ":") : ("The following prefabs have missing components:"));
                        _scroll = GUILayout.BeginScrollView(_scroll);
                        foreach (string s in _listResult)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(s, GUILayout.Width(position.width / 2));
                            if (GUILayout.Button("Select", GUILayout.Width(position.width / 2 - 10)))
                            {
                                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(s);
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndScrollView();
                    }
                }
            }
            else
            {
                _showPrefabs = GUILayout.Toggle(_showPrefabs, "Show prefab components");
                if (_showPrefabs)
                {
                    GUILayout.Label("The following components are attatched to prefabs:");
                    DisplayResults(ref _scroll1, ref _prefabComponents);
                }
                _showAdded = GUILayout.Toggle(_showAdded, "Show AddComponent arguments");
                if (_showAdded)
                {
                    GUILayout.Label("The following components are AddComponent arguments:");
                    DisplayResults(ref _scroll2, ref _addedComponents);
                }
                _showScene = GUILayout.Toggle(_showScene, "Show Scene-used components");
                if (_showScene)
                {
                    GUILayout.Label("The following components are used by scene objects:");
                    DisplayResults(ref _scroll3, ref _sceneComponents);
                }
                _showUnused = GUILayout.Toggle(_showUnused, "Show Unused Components");
                if (_showUnused)
                {
                    GUILayout.Label("The following components are not used by prefabs, by AddComponent, OR in any scene:");
                    DisplayResults(ref _scroll4, ref _notUsedComponents);
                }
            }
        }

        int SortAlphabetically(ComponentNames a, ComponentNames b)
        {
            return a.AssetPath.CompareTo(b.AssetPath);
        }

        GameObject[] GetAllObjectsInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            GameObject[] allGOs = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject go in allGOs)
            {
                //if ( go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave )
                //    continue;

                string assetPath = AssetDatabase.GetAssetPath(go.transform.root.gameObject);
                if (!string.IsNullOrEmpty(assetPath))
                    continue;

                objectsInScene.Add(go);
            }

            return objectsInScene.ToArray();
        }

        void DisplayResults(ref Vector2 scroller, ref List<ComponentNames> list)
        {
            if (list == null) return;
            scroller = GUILayout.BeginScrollView(scroller);
            foreach (ComponentNames c in list)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(c.AssetPath, GUILayout.Width(position.width / 5 * 4));
                if (GUILayout.Button("Select", GUILayout.Width(position.width / 5 - 30)))
                {
                    Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(c.AssetPath);
                }
                GUILayout.EndHorizontal();
                if (c.UsageSource.Count == 1)
                {
                    GUILayout.Label("   In 1 Place: " + c.UsageSource[0]);
                }
                if (c.UsageSource.Count > 1)
                {
                    GUILayout.Label("   In " + c.UsageSource.Count + " Places: " + c.UsageSource[0] + ", " + c.UsageSource[1] + (c.UsageSource.Count > 2 ? ", ..." : ""));
                }
            }
            GUILayout.EndScrollView();

        }

        string NameFromPath(string s)
        {
            s = s.Substring(s.LastIndexOf('/') + 1);
            return s.Substring(0, s.Length - 3);
        }

        string GetNamespaceFromPath(string path)
        {
            foreach (ComponentNames c in _existingComponents)
            {
                if (c.AssetPath == path)
                {
                    return c.NamespaceName;
                }
            }
            return "";
        }

        string GetPathFromNames(string space, string name)
        {
            ComponentNames test = new ComponentNames(name, space, "");
            int index = _existingComponents.IndexOf(test);
            if (index != -1)
            {
                return _existingComponents[index].AssetPath;
            }
            return "";
        }

        public static string[] GetAllPrefabs()
        {
            string[] temp = AssetDatabase.GetAllAssetPaths();
            List<string> result = new List<string>();
            foreach (string s in temp)
            {
                if (s.Contains(".prefab")) result.Add(s);
            }
            return result.ToArray();
        }
    }

}
