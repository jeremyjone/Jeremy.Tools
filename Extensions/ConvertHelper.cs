using Jeremy.Tools.Json;
using System;
using System.Text;

namespace Jeremy.Tools.Extensions
{
    /// <summary>
    /// 类型转换工具
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        /// 尝试将一个对象转换为布尔值
        /// </summary>
        /// <param name="value">待转换的对象</param>
        /// <returns></returns>
        public static bool ToBool(this object value)
        {
            var res = false;
            if (value != null && value != DBNull.Value && bool.TryParse(value.ToString(), out res))
            {
                return res;
            }

            if (value is string)
                res = !string.IsNullOrWhiteSpace(value.ToString());

            return res;
        }

        /// <summary>
        /// 尝试将一个对象转换为一个16位的整数。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static short ToInt16(this object value, short target = default)
        {
            var res = target;
            if (value != null && value != DBNull.Value && short.TryParse(value.ToString(), out res))
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// 尝试将一个对象转换为一个32位的整数。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int ToInt32(this object value, int target = default)
        {
            var res = target;
            if (value != null && value != DBNull.Value && int.TryParse(value.ToString(), out res))
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// 尝试将一个对象转换为一个单精度浮点数。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float ToFloat(this object value, float target = default)
        {
            var res = target;
            if (value != null && value != DBNull.Value && float.TryParse(value.ToString(), out res))
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// 尝试将一个对象转换为一个双精度浮点数。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double ToDouble(this object value, double target = default)
        {
            var res = target;
            if (value != null && value != DBNull.Value && double.TryParse(value.ToString(), out res))
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// 尝试将一个对象转换为一个高精度浮点数。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value, decimal target = default)
        {
            var res = target;
            if (value != null && value != DBNull.Value && decimal.TryParse(value.ToString(), out res))
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// 尝试将一个对象转换为一个日期对象。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object value, DateTime target = default)
        {
            var res = target;
            if (value != null && value != DBNull.Value && DateTime.TryParse(value.ToString(), out res))
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// 将一个对象的整体内容转为一个字节数组
        /// </summary>
        /// <param name="value">待转换的对象</param>
        /// <returns></returns>
        public static byte[] ToBytes(this object value)
        {
            var json = value.Serialize();
            return Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// 将一个字符串转为字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
