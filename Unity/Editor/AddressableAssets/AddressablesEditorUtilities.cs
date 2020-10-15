using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;

using Object = UnityEngine.Object;

namespace Utils.Unity.Editor.AddressableAssets
{
    public static class AddressablesEditorUtilities
    {
        public static bool SetObject(SerializedProperty property,
                                     Object target,
                                     AssetReference assetRef,
                                     out string guid)
        {
            guid = null;
            try
            {
                if (assetRef == null)
                    return false;
                Undo.RecordObject(property.serializedObject.targetObject,
                                  "Assign Asset Reference");
                if (target == null)
                {
                    assetRef.SetEditorAsset(null);
                    return true;
                }

                Object subObject = null;
                // Handle sprite atlas
                if (target.GetType() == typeof(Sprite))
                {
                    var atlasEntries = new List<AddressableAssetEntry>();
                    AddressableAssetSettingsDefaultObject.Settings
                        .GetAllAssets(atlasEntries, false, null, e => AssetDatabase
                                      .GetMainAssetTypeAtPath(e.AssetPath) == typeof(SpriteAtlas));
                    var spriteName = target.name;
                    if (spriteName.EndsWith("(Clone)"))
                        spriteName = spriteName.Replace("(Clone)", "");

                    foreach (var a in atlasEntries)
                    {
                        var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(a.AssetPath);
                        if (atlas == null)
                            continue;
                        var s = atlas.GetSprite(spriteName);
                        if (s == null)
                            continue;
                        subObject = target;
                        target = atlas;
                        break;
                    }
                }
                var success = assetRef.SetEditorAsset(target);
                if (success)
                {
                    if (subObject != null)
                        assetRef.SetEditorSubObject(subObject);
                    guid = assetRef.AssetGUID;
                    EditorUtility.SetDirty(property.serializedObject.targetObject);

                    var comp = property.serializedObject.targetObject as Component;
                    if (comp != null && comp.gameObject != null && comp.gameObject.activeInHierarchy)
                        EditorSceneManager.MarkSceneDirty(comp.gameObject.scene);
                }
                return success;
            }
            catch (Exception e)
            {
                Debug.unityLogger.LogException(e);
            }
            return false;
        }
    }
}
