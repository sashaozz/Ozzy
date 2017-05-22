using MessagePack;
using System.Text;
using System;

namespace Ozzy
{
    public class ContractlessMessagePackSerializer : ISerializer
    {
        public static ContractlessMessagePackSerializer Instance = new ContractlessMessagePackSerializer();
        public byte[] BinarySerialize<T>(T value)
        {
            //return LZ4MessagePackSerializer.Serialize(value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
            return MessagePackSerializer.Serialize(value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
        public byte[] BinarySerialize(object value, Type type)
        {
            //return LZ4MessagePackSerializer.NonGeneric.Serialize(type, value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
            return MessagePackSerializer.NonGeneric.Serialize(type, value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
        public T BinaryDeSerialize<T>(byte[] value)
        {
            return MessagePackSerializer.Deserialize<T>(value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
            //return LZ4MessagePackSerializer.Deserialize<T>(value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
        public object BinaryDeSerialize(byte[] value, Type type)
        {
            return MessagePackSerializer.NonGeneric.Deserialize(type, value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
            //return LZ4MessagePackSerializer.NonGeneric.Deserialize(type, value, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
        public string TextSerialize<T>(T value)
        {
            return Encoding.UTF8.GetString(BinarySerialize(value));
        }
        public string TextSerialize(object value, Type type)
        {
            return Encoding.UTF8.GetString(BinarySerialize(value, type));
        }
        public T TextDeSerialize<T>(string value)
        {
            return BinaryDeSerialize<T>(Encoding.UTF8.GetBytes(value));
        }
        public object TextDeSerialize(string value, Type type)
        {
            return BinaryDeSerialize(Encoding.UTF8.GetBytes(value), type);
        }
    }
}
