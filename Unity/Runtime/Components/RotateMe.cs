using UnityEngine;

namespace Utils.Unity.Runtime.Components
{
    public class RotateMe : MonoBehaviour
    {
        [SerializeField] private float RotationSpeed;
        [SerializeField] private Vector3 RotationDirection;

        private void Update()
        {
            transform.Rotate(RotationDirection.normalized * (RotationSpeed * Time.deltaTime));
        }
    }
}
