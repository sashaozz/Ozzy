using System;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ozzy
{
    public class DefaultJasonNetSerializer : ISerializer
    {
        private static readonly JsonSerializerSettings _settings;
        static DefaultJasonNetSerializer()
        {
            _settings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };
            _settings.Converters.Add(new StringEnumConverter());
        }

        //todo: consider change binary serialization to BSON
        public byte[] BinarySerialize<T>(T value)
        {
            return Encoding.UTF8.GetBytes(TextSerialize<T>(value));
        }

        public byte[] BinarySerialize(object value, Type type)
        {
            return Encoding.UTF8.GetBytes(TextSerialize(value, type));
        }

        public T BinaryDeSerialize<T>(byte[] value)
        {
            return TextDeSerialize<T>(Encoding.UTF8.GetString(value));
        }

        public object BinaryDeSerialize(byte[] value, Type type)
        {
            return TextDeSerialize(Encoding.UTF8.GetString(value), type);
        }

        public string TextSerialize<T>(T value)
        {
            return TextSerialize(value, value.GetType());
        }
        public string TextSerialize(object value, Type type)
        {
            return (type == null) ?
                JsonConvert.SerializeObject(value, _settings)
                : JsonConvert.SerializeObject(value, type, _settings);
        }
        public T TextDeSerialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _settings);
        }
        public object TextDeSerialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, _settings);
        }
    }
}
