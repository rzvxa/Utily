using UnityEngine;

namespace Utils.Unity.Runtime.Components
{
    public sealed class SafeArea : MonoBehaviour
    {
        private RectTransform _panel;
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            var safeArea = GetSafeArea();

            if(safeArea != _lastSafeArea)
                ApplySafeArea(safeArea);
        }

        private Rect GetSafeArea() => Screen.safeArea;

        private void ApplySafeArea(Rect rect)
        {
            _lastSafeArea = rect;

            var anchorMin = rect.position;
            var anchorMax = rect.position + rect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
        }
    }
}
