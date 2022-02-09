using System;
using System.Text.Json;

namespace Jeremy.Tools.Json
{


    public static class SerializeHelper
    {
        /// <summary>
        /// 序列化一个对象
        /// </summary>
        /// <param name="value">待序列化的对象</param>
        /// <param name="options">允许自定义参数</param>
        /// <exception cref="JsonSerializeException"></exception>
        /// <returns></returns>
        public static string Serialize<T>(this T value, JsonSerializerOptions options = null) where T : class
        {
            var opt = options ?? new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            try
            {
                return JsonSerializer.Serialize(value, opt);
            }
            catch (Exception e)
            {
                throw new JsonSerializeException($"The value type of [{typeof(T).FullName}] is not support serialize to JSON.", e);
            }
        }

        /// <summary>
        /// 安全的序列化一个对象。如果不能序列化，则返回指定内容（默认为空字符串）。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="res">返回的结果（如果提供）</param>
        /// <param name="options">允许自定义参数</param>
        /// <returns></returns>
        public static string SerializeSafety<T>(this T value, string res = null, JsonSerializerOptions options = null) where T : class
        {
            try
            {
                return Serialize(value, options);
            }
            catch (Exception)
            {
                var s = res;
                return s ?? "";
            }
        }
    }
}
