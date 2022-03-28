using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Basket.Management.Basket.Helpers
{
    public static class JsonSerializer<TType> where TType : class
    {
        public static string Serialize(TType instance)
        {
            var serializer = CreateSerializer();
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        private static DataContractJsonSerializer CreateSerializer()
        {
            var serializerSettings = new DataContractJsonSerializerSettings();
            serializerSettings.DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssZ");
            return new DataContractJsonSerializer(typeof(TType), serializerSettings);
        }

        public static TType Deserialize(string json)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                var serializer = CreateSerializer();
                return serializer.ReadObject(stream) as TType;
            }
        }
    }
}
