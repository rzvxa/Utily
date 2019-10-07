using UnityEngine;

namespace Utils.Unity.Runtime.UI
{
    public static class RectTransform
    {
        public static Rect RectTransformToScreenSpace(UnityEngine.RectTransform transform)
        {
            var size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect((Vector2)transform.position - (size * .5f), size);
        }
    }
}
