using System;
using System.Reflection;
using System.Linq;

namespace Utils.Reflection
{
    public static class TypeExt
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        public static FieldInfo GetFieldRecursive(this Type givenType, string fieldName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            FieldInfo fi = null;

            while (givenType != null)
            {
                fi = givenType.GetField(fieldName, bindingFlags);

                if (fi != null) break;

                givenType = givenType.BaseType;
            }

            if (fi == null)
            {
                throw new Exception($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi;
        }

        public static PropertyInfo GetPropertyRecursive(this Type givenType, string propertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            PropertyInfo pi = null;
            while (givenType != null)
            {
                pi = givenType.GetProperty(propertyName, bindingFlags);

                if (pi != null) break;

                givenType = givenType.BaseType;
            }

            if (pi is null)
            {
                throw new Exception($"Property '{propertyName}' not found in type hierarchy.");
            }

            return pi;
        }

        public static Type GetGenericTypeRecursive(this Type givenType, Func<Type, bool> predicate)
        {
            if(TryGetGenericTypeRecursive(givenType, predicate, out var targetType)) return targetType;
            throw new Exception("Given type do not have a base type with given predicate");
        }

        public static Type GetGenericTypeRecursive(this Type givenType, Type typeDefinition = null)
        {
            if(TryGetGenericTypeRecursive(givenType, out var targetType, typeDefinition)) return targetType;
            throw new Exception("Given type do not have a base type with given predicate");
        }

        public static bool TryGetGenericTypeRecursive(this Type givenType, Func<Type, bool> predicate, out Type targetType)
        {
            return TryGetTypeRecursive(givenType, t => t.IsGenericType && predicate.Invoke(t), out targetType);
        }

        public static bool TryGetGenericTypeRecursive(this Type givenType, out Type targetType, Type typeDefinition = null)
        {
            return TryGetGenericTypeRecursive(givenType, t => typeDefinition is null || t.GetGenericTypeDefinition() == typeDefinition, out targetType);
        }

        public static Type GetTypeRecursive(this Type givenType, Type typeDifinition = null)
        {
            return GetTypeRecursive(givenType, t => typeDifinition is null || typeDifinition == t);
        }

        public static Type GetTypeRecursive(this Type givenType, Func<Type, bool> predicate)
        {
            if (TryGetTypeRecursive(givenType, predicate, out var targetType)) return targetType;
            throw new Exception("Given type do not have a base type with given predicate");
        }

        public static bool TryGetTypeRecursive(this Type givenType, out Type targetType, Type typeDefinition = null)
        {
            return TryGetTypeRecursive(givenType, t => typeDefinition is null || typeDefinition == t, out targetType);
        }

        public static bool TryGetTypeRecursive(this Type givenType, Func<Type, bool> predicate, out Type targetType)
        {
            while (givenType != null)
            {
                if (predicate.Invoke(givenType))
                {
                    targetType = givenType;
                    return true;
                }

                givenType = givenType.BaseType;
            }

            targetType = null;
            return false;
        }

        public static Type[] GetDerivedTypes(this Type givenType, Assembly[] assemblies)
        {
            return (from asm in assemblies
                    from type in asm.GetTypes()
                    where givenType.IsAssignableFrom(type)
                    select type).ToArray();
        }
    }
}
