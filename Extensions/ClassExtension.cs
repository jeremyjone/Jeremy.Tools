using System.Collections.Generic;
using System.Linq;

namespace Jeremy.Tools.Extensions
{
    /// <summary>
    /// 类对象扩展
    /// </summary>
    public static class ClassExtension
    {
        /// <summary>
        /// 将一个对象的属性值转化为键值对字典
        /// </summary>
        /// <param name="value">待转换的对象</param>
        /// <param name="ignoreEmpty">忽略空值</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary<T>(this T value, bool ignoreEmpty = true) where T : class
        {
            return value.GetType().GetProperties().OrderBy(x => x.Name)
                .Where(x => !ignoreEmpty || !Utils.IsNullOrEmpty(x.GetValue(value)))
                .ToDictionary(x => x.Name, x => x.GetValue(value));
        }

        /// <summary>
        /// 判定一个类对象是否为空，以及它的所有属性是否均为空。只要有一个属性不为空则返回 true，否则返回 false。<br />
        /// </summary>
        /// <remarks>
        /// 通常一个初始化的对象为 true 值。
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this T value) where T : class, new()
        {
            if (value == null) return true;

            return value.GetType().GetProperties().Select(prop => prop.GetValue(value))
                .All(p => p == null || !p.ToBool());
        }
    }
}
