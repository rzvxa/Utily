using UnityEditor;
using System.Collections.Generic;

namespace Utils.Unity.Editor
{
    public static class PlayerSettingsHelper
    {
        public static string[] GetCurrentBuildTargetScriptingDefineSymbols() =>
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings
                                                             .selectedBuildTargetGroup)
            .Split(';');

        public static bool ScriptingDefineSymbolExists(string define)
        {
            var defs = GetCurrentBuildTargetScriptingDefineSymbols();
            foreach (var def in defs)
                if (def == define)
                    return true;
            return false;
        }

        public static void AddScriptingDefineSymbol(string define)
        {
            var defines = new HashSet<string>();
            var defs = GetCurrentBuildTargetScriptingDefineSymbols();
            foreach (var def in defs)
                defines.Add(def);
            defines.Add(define);
            var resultDefines = string.Empty;
            foreach (var def in defines)
                resultDefines += $"{def};";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings
                                                             .selectedBuildTargetGroup,
                                                             resultDefines);
        }

        public static void RemoveScriptingDefineSymbol(string define)
        {
            var defines = string.Empty;
            var defs = GetCurrentBuildTargetScriptingDefineSymbols();
            foreach(var def in defs)
                if (def != define)
                    defines += $"{def};";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings
                                                             .selectedBuildTargetGroup,
                                                             defines);
        }
    }
}
