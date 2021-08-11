using System;
using System.Collections.Generic;
using System.Linq;
using Jeremy.Tools.Common;
using Jeremy.Tools.Extensions;

namespace Jeremy.Tools.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// 将一个枚举转换为可制定的列表项
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static List<TResult> EnumToList<TEnum, TResult>(Func<TEnum, TResult> selector) where TEnum : struct, IComparable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(selector).ToList();
        }

        /// <summary>
        /// 将一个枚举转换为 List&lt;<see cref="EnumItem"/>&gt; 的列表项
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <returns></returns>
        public static List<EnumItem> EnumToList<TEnum>() where TEnum : struct, IComparable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(x => new EnumItem { Key = x.ToInt16().ToString(), Value = x.ToString() })
                .ToList();
        }
    }
}
