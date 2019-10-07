using UnityEngine;

namespace Utils.Unity.Runtime
{
    public static class TransformExt
    {
        public static Transform Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            return transform;
        }
    }
}
