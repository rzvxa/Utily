using System;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Utils.Unity.Editor
{
    public static class SerializedPropertyExt
    {
        public static T GetActualValue<T>(this SerializedProperty @this,
                                                                FieldInfo field,
                                                                out string label)
        {
            label = "";
            try
            {
                if (@this == null || field == null)
                    return default;
                var serializedObject = @this.serializedObject;
                if (serializedObject == null)
                    return default;

                var targetObject = serializedObject.targetObject;

                if (@this.depth > 0)
                {
                    var slicedName = @this.propertyPath.Split('.').ToList();
                    var arrayCounts = new List<int>();
                    for (int index = 0; index < slicedName.Count; index++)
                    {
                        arrayCounts.Add(-1);
                        var currName = slicedName[index];
                        if (currName.EndsWith("]"))
                        {
                            var arraySlice = currName.Split('[', ']');
                            if (arraySlice.Length >= 2)
                            {
                                arrayCounts[index - 2] = Convert.ToInt32(arraySlice[1]);
                                slicedName[index] = string.Empty;
                                slicedName[index - 1] = string.Empty;
                            }
                        }
                    }

                    while (string.IsNullOrEmpty(slicedName.Last()))
                    {
                        int i = slicedName.Count - 1;
                        slicedName.RemoveAt(i);
                        arrayCounts.RemoveAt(i);
                    }

                    if (@this.propertyPath.EndsWith("]"))
                    {
                        var slice = @this.propertyPath.Split('[', ']');
                        if (slice.Length >= 2)
                            label = $"Element {slice[slice.Length - 2]}";
                    }
                    else
                    {
                        label = slicedName.Last();
                    }

                    return DescendHierarchy<T>(targetObject, slicedName, arrayCounts, 0);
                }

                var obj = field.GetValue(targetObject);
                return (T)obj;
            }
            catch
            {
                return default;
            }
        }

        private static T DescendHierarchy<T>(object targetObject,
                                             List<string> splitName,
                                             List<int> splitCounts,
                                             int depth)
        {
            if (depth >= splitName.Count)
                return default;

            var currName = splitName[depth];

            if (string.IsNullOrEmpty(currName))
                return DescendHierarchy<T>(targetObject, splitName, splitCounts, depth + 1);

            int arrayIndex = splitCounts[depth];

            var newField = targetObject.GetType() .GetField(currName,
                                                            BindingFlags.Public |
                                                            BindingFlags.NonPublic |
                                                            BindingFlags.Instance);

            if (newField == null)
            {
                var baseType = targetObject.GetType().BaseType;
                while (baseType != null && newField == null)
                {
                    newField = baseType.GetField(currName,
                                                 BindingFlags.Public |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Instance);
                    baseType = baseType.BaseType;
                }
            }

            var newObj = newField.GetValue(targetObject);
            if (depth == splitName.Count - 1)
            {
                T actualObject = default(T);
                if (arrayIndex >= 0)
                {
                    if (newObj.GetType().IsArray && ((Array)newObj).Length > arrayIndex)
                        actualObject = (T)((Array)newObj).GetValue(arrayIndex);

                    var newObjList = newObj as IList;
                    if (newObjList != null && newObjList.Count > arrayIndex)
                    actualObject = (T)newObjList[arrayIndex];

                }
                else
                {
                    actualObject = (T)newObj;
                }
                return actualObject;
            }
            else if (arrayIndex >= 0)
            {
                if (newObj is IList)
                {
                    var list = (IList)newObj;
                    newObj = list[arrayIndex];
                }
                else if (newObj is Array)
                {
                    var a = (Array)newObj;
                    newObj = a.GetValue(arrayIndex);
                }
            }

            return DescendHierarchy<T>(newObj, splitName, splitCounts, depth + 1);
        }
    }
}
