namespace Jeremy.Tools.Helpers
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
    }
}
