using Newtonsoft.Json;

namespace Utils.Unity.Runtime
{
    public static class ObjectExtension
    {
        public static T CloneByJson<T>(this T obj) where T : class
        {
            if (obj == null) return null;
            var serializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All};
            string json = JsonConvert.SerializeObject(obj, serializerSettings);
            return JsonConvert.DeserializeObject<T>(json, serializerSettings);
        }
    }
}