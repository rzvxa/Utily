using UnityEngine;

namespace Utils.Unity.Runtime.IO
{
    public static class PlayerPrefsHelper
    {
        public static void WriteTextureToPlayerPrefs(string key, Texture2D texture)
        {
            var textureBytes = texture.EncodeToPNG();
            var base64Texture = System.Convert.ToBase64String(textureBytes);
            PlayerPrefs.SetString(key, base64Texture);
            PlayerPrefs.Save();
        }

        public static Texture2D ReadTextureFromPlayerPrefs(string key)
        {
            var base64Texture = PlayerPrefs.GetString(key, null);

            if (!string.IsNullOrEmpty(base64Texture))
            {
                var bytes = System.Convert.FromBase64String(base64Texture);
                var texture = new Texture2D(2, 2);

                if (texture.LoadImage(bytes))
                    return texture;
            }
            return null;
        }
    }
}
