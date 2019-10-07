using UnityEngine;

namespace Utils.Unity.Runtime
{
    public static class Texture2DUtility
    {
        public static Vector2Int PointToPixel(Vector2 point, Rect textureRenderRect, int textureHeight, int textureWidth)
        {
            var xs = Mathf.InverseLerp(textureRenderRect.xMin, textureRenderRect.xMax, point.x);
            var ys = Mathf.InverseLerp(textureRenderRect.yMin, textureRenderRect.yMax, point.y);
            return new Vector2Int(
                    (int)Mathf.Lerp(0, textureWidth, xs),
                    (int)Mathf.Lerp(0, textureHeight, ys)
                    );
        }

        public static Vector2 PixelToPoint(Vector2Int pixel, Rect textureRenderRect, int textureHeight, int textureWidth)
        {
            var xs = Mathf.InverseLerp(0, textureWidth, pixel.x);
            var ys = Mathf.InverseLerp(0, textureHeight, pixel.y);
            return new Vector2(
                    Mathf.Lerp(textureRenderRect.xMin, textureRenderRect.xMax, xs),
                    Mathf.Lerp(textureRenderRect.yMin, textureRenderRect.yMax, ys)
                    );
        }
    }
}
