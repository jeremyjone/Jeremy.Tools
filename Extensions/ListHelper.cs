using System.Collections.Generic;
using System.Linq;
using Jeremy.Tools.Helpers;

namespace Jeremy.Tools.Extensions
{
    /// <summary>
    /// 列表对象扩展
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// 移除当前列表中所包含的空项
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static List<T> RemoveEmptyItem<T>(this List<T> l)
        {
            return l.Where(t => t != null && !Utils.IsNullOrEmpty(t.ToString())).ToList();
        }
    }
}
