using UnityEngine;
namespace Utils.Unity.Runtime
{
    public static class GameObjectExt
    {
        public static T GetComponentInMeOrParent<T>(this GameObject go, bool includeInactive = false)
            where T : Component
        {
            return go.GetComponent<T>() ?? go.GetComponentInParent<T>(includeInactive);
        }

        public static T GetComponentInMeOrChildren<T>(this GameObject go, bool includeInactive = false)
            where T : Component
        {
            return go.GetComponent<T>() ?? go.GetComponentInChildren<T>(includeInactive);
        }

        public static T GetComponentInParent<T>(this GameObject go, bool includeInactive)
            where T : Component
        {
            if (!includeInactive)
                return go.GetComponentInParent<T>();
            var t = go.transform;
            while (!(t is null))
            {
                var c = t.gameObject.GetComponent<T>();
                if (!(c is null))
                    return c;
                t = t.parent;
            }
            return null;
        }

        public static string GetPathInScene(this GameObject obj)
        {
            string p = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                p = "/" + obj.name + p;
            }
            return p;
        }
    }
}
