using UnityEditor;
using UnityEngine;
using System.IO;

namespace Utils.Unity.Editor
{
    public static class IO
    {
        public static void CreateAssetPath(string directoryPath)
        {
            directoryPath = directoryPath.Replace('\\', '/');
            var pathSegments = directoryPath.Split('/');
            const string root = "Assets";
            var accumulatedUnityDirectory = root;
            var accumulatedSystemDirectory = Application.dataPath + Path.GetDirectoryName(root);
            foreach (var dir in pathSegments)
            {
                if(!AssetDatabase.IsValidFolder($"{accumulatedUnityDirectory}/{dir}"))
                    AssetDatabase.CreateFolder(accumulatedUnityDirectory, dir);
                // if(!Directory.Exists(accumulatedSystemDirectory + Path.GetDirectoryName($"{accumulatedUnityDirectory}/dir")))
                //     AssetDatabase.CreateFolder(accumulatedUnityDirectory, dir);
                // accumulatedSystemDirectory += $"/{dir}";
                accumulatedUnityDirectory += $"/{dir}";
            }
        }

        private const string AssetIncludedInPathFormat = "Assets/{0}/{1}";

        public static void CopyAsset(string assetPath, string directoryPath, string newAssetName)
        {
            CreateAssetPath(directoryPath);
            AssetDatabase.CopyAsset(assetPath, string.Format(AssetIncludedInPathFormat, directoryPath, newAssetName));
        }

        public static void CreateAsset(Object asset, string directoryPath, string assetName)
        {
            CreateAssetPath(directoryPath);
            AssetDatabase.CreateAsset(asset, string.Format(AssetIncludedInPathFormat, directoryPath, assetName));
        }
    }
}
