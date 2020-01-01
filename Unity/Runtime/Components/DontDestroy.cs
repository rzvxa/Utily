using UnityEngine;

namespace Utils.Unity.Runtime.Components
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}
