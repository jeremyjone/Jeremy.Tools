namespace Jeremy.Tools.Common
{
    /// <summary>
    /// Enum 项转换为该对象
    /// </summary>
    /// <remarks>
    /// 【例】<br />
    /// Enum {A, B = 3}<br />
    /// 则：EnumItem{Key = "0", Value = "A"}, EnumItem{Key = "3", Value = "B"}
    /// </remarks>
    public class EnumItem
    {
        /// <summary>
        /// 对应 Enum 每项的键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 对应 Enum 每项的值
        /// </summary>
        public string Value { get; set; }
    }
}
