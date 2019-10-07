using UnityEngine;

namespace Utils.Unity.Serialization
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class SerializePropertyAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }

        public SerializePropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
