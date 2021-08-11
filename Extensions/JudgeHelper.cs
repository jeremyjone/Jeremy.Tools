using System.Linq;

namespace Jeremy.Tools.Extensions
{
    /// <summary>
    /// 判断扩展
    /// </summary>
    public static class JudgeHelper
    {
        /// <summary>
        /// 判定一个类对象是否为空，以及它的所有属性是否均为假值。<br />
        /// </summary>
        /// <remarks>
        /// 通常一个没有初值的初始化的对象为 true 值。
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

        /// <summary>
        /// 判断两个double类型数据是否相等。<br />
        /// 【默认误差小于0.00001则为真】
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="range">误差范围</param>
        /// <returns></returns>
        public static bool IsEqual(this double a, double b, double range = 0.00001)
        {
            return (a - b) > -range && (a - b) < range;
        }
    }
}
