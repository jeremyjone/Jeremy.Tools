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
        /// <param name="options">允许自定义参数</param>
        /// <exception cref="JsonDeserializeException"></exception>
        /// <returns></returns>
        public static T Deserialize<T>(this string value, JsonSerializerOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(value)) value = new { }.ToString();
            var opt = options ?? new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            try
            {
                return JsonSerializer.Deserialize<T>(value, opt);
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
        /// <param name="value">json 格式的字符数组</param>
        /// <param name="options">允许自定义参数</param>
        /// <returns></returns>
        public static T Deserialize<T>(this byte[] value, JsonSerializerOptions options = null)
        {
            if (value.Length == 0) value = new { }.ToBytes();
            var opt = options ?? new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var readOnlySpan = new ReadOnlySpan<byte>(value);

            try
            {
                return JsonSerializer.Deserialize<T>(readOnlySpan, opt);
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
        /// <param name="options">允许自定义参数</param>
        /// <returns></returns>
        public static T DeserializeSafety<T>(this string value, T res = null, JsonSerializerOptions options = null) where T : class, new()
        {
            try
            {
                return Deserialize<T>(value, options);
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
        /// <param name="options">允许自定义参数</param>
        /// <returns></returns>
        public static T DeserializeSafety<T>(this byte[] value, T res = null, JsonSerializerOptions options = null) where T : class, new()
        {
            try
            {
                return Deserialize<T>(value, options);
            }
            catch (Exception)
            {
                return res ?? new T();
            }
        }
    }
}