using System;
using System.Text.Json;
using Jeremy.Tools.Extensions;

namespace Jeremy.Tools.Json
{
    public static class DeserializeHelper
    {
        /// <summary>
        /// 反序列化一个 json 到指定类型的对象
        /// </summary>
        /// <typeparam name="T">反序列的对象类型</typeparam>
        /// <param name="value">json 字符串</param>
        /// <exception cref="JsonDeserializeException"></exception>
        /// <returns></returns>
        public static T Deserialize<T>(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) value = new { }.ToString();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            try
            {
                return JsonSerializer.Deserialize<T>(value, options);
            }
            catch (Exception e)
            {
                throw new JsonDeserializeException($"The value is not support deserialize to [{typeof(T).FullName}].", e);
            }
        }

        /// <summary>
        /// 反序列化一个 json 到指定类型的对象
        /// </summary>
        /// <typeparam name="T">反序列的对象类型</typeparam>
        /// <param name="value">json 格式内容</param>
        /// <returns></returns>
        public static T Deserialize<T>(this byte[] value)
        {
            if (value.Length == 0) value = new { }.ToBytes();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var readOnlySpan = new ReadOnlySpan<byte>(value);

            try
            {
                return JsonSerializer.Deserialize<T>(readOnlySpan, options);
            }
            catch (Exception e)
            {
                throw new JsonDeserializeException($"The value is not support deserialize to [{typeof(T).FullName}].", e);
            }
        }

        /// <summary>
        /// 安全的反序列化一个 json 到指定类型的对象。如果不能反序列化，则返回指定的对象（默认为一个新的空对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static T DeserializeSafety<T>(this string value, T res = null) where T : class, new()
        {
            try
            {
                return Deserialize<T>(value);
            }
            catch (Exception)
            {
                return res ?? new T();
            }
        }

        /// <summary>
        /// 安全的反序列化一个 json 到指定类型的对象。如果不能反序列化，则返回指定的对象（默认为一个新的空对象）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static T DeserializeSafety<T>(this byte[] value, T res = null) where T : class, new()
        {
            try
            {
                return Deserialize<T>(value);
            }
            catch (Exception)
            {
                return res ?? new T();
            }
        }
    }
}