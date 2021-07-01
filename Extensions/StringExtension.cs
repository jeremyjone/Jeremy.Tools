using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Jeremy.Tools.Extensions
{
    /// <summary>
    /// 对字符串的扩展操作类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 检查一个字符串是否为正确的 base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static bool CheckBase64(this string base64)
        {
            return base64.Length % 4 == 0;
        }

        /// <summary>
        /// 检查一个字符串是否为正确的 base64。如果不是，修正并返回正确的 base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string CheckAndCorrectBase64(this string base64)
        {
            var mod = base64.Length % 4;
            if (mod > 0) base64 += new string('=', 4 - mod);
            return base64;
        }

        /// <summary>
        /// 字符串转 Unicode 码
        /// </summary>
        /// <param name="value">待转换的字符串</param>
        /// <returns></returns>
        public static string ToUnicode(this string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                // 取两个字符，每个字符都是右对齐。
                stringBuilder.AppendFormat("u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'),
                    bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode 码转字符串
        /// </summary>
        /// <param name="unicode">待转换的 Unicode 字符串</param>
        /// <returns></returns>
        public static string ToUniString(this string unicode)
        {
            unicode = unicode.Replace("%", "\\");

            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled)
                .Replace(unicode, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// 移除富文本中的 HTML 标签。
        /// </summary>
        /// <param name="html">富文本内容</param>
        /// <param name="length">获取长度</param>
        /// <returns>返回指定长度的内容（如果给定长度），或者全部内容。</returns>
        public static string RemoveHtmlTag(this string html, int length = 0)
        {
            // 移除 html 标签
            var text = Regex.Replace(html, "<[^>]+>", "");
            // 移除 html 中的标识符内容
            text = Regex.Replace(text, "&[^;]+;", "");

            return length > 0 && text.Length > length ? text.Substring(0, length) : text;
        }



        /// <summary>
        /// 对字符串进行MD5哈希处理，默认返回全小写文本。如果toUpper参数为true，则返回全大写文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toUpper">当true时，返回全大写文本</param>
        /// <returns></returns>
        public static string ComputeHash(this string text, bool toUpper = false)
        {
            var sb = new StringBuilder();
            foreach (var b in MD5Hash(text))
                sb.Append(b.ToString("X2"));
            return toUpper ? sb.ToString().ToUpper() : sb.ToString().ToLower();
        }

        /// <summary>
        /// SHA256 加密函数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ComputedSha256Encrypt(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = SHA256.Create().ComputeHash(bytes);

            var builder = new StringBuilder();
            foreach (var t in hash)
                builder.Append(t.ToString("x2"));
            return builder.ToString();
        }


        /// <summary>
        /// 计算MD5的处理函数
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static IEnumerable<byte> MD5Hash(string text)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}
