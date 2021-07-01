using Jeremy.Tools.Common;
using Jeremy.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeremy.Tools
{
    public static class Utils
    {
        /// <summary>
        /// 对数据进行判空<br />
        /// 【注意】仅适用于判定基本类型，不要使用该方法判定复杂类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要判断的对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(T value)
        {
            if (typeof(T) == typeof(string))
                return string.IsNullOrWhiteSpace(value as string);

            return value == null || value.Equals(default(T));
        }

        /// <summary>
        /// 将一个枚举转换为可制定的列表项
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static List<TResult> EnumToList<TEnum, TResult>(Func<TEnum, TResult> selector) where TEnum : struct, IComparable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(selector).ToList();
        }

        /// <summary>
        /// 将一个枚举转换为 List&lt;<see cref="EnumItem"/>&gt; 的列表项
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<EnumItem> EnumToList<TEnum>() where TEnum : struct, IComparable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(x => new EnumItem { Key = x.ToInt16().ToString(), Value = x.ToString() })
                .ToList();
        }
    }
}
