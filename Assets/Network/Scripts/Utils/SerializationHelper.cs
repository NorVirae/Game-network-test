using Newtonsoft.Json;

namespace Network
{
    public static class SerializationHelper
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
