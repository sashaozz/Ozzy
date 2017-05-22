using System;

namespace Ozzy
{
    public interface ISerializer
    {
        byte[] BinarySerialize<T>(T value);
        byte[] BinarySerialize(object value, Type type);
        T BinaryDeSerialize<T>(byte[] value);
        object BinaryDeSerialize(byte[] value, Type type);

        string TextSerialize<T>(T value);
        string TextSerialize(object value, Type type);
        T TextDeSerialize<T>(string value);
        object TextDeSerialize(string value, Type type);
    }
}
