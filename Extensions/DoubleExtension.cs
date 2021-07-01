namespace Jeremy.Tools.Extensions
{
    /// <summary>
    /// 浮点数扩展
    /// </summary>
    public static class DoubleExtension
    {
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
