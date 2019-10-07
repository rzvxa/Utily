using UnityEngine;

namespace Utils.Unity.Runtime
{
    public static class Texture2DExt
    {
        public static Texture2D CopyToNew(this Texture2D @this, bool mipChain, bool linear)
        {
            var texture = new Texture2D(@this.width, @this.height, @this.format, mipChain, linear);
            Graphics.CopyTexture(@this, texture);
            texture.Apply();
            return texture;
        }
    }
}
