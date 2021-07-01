using System.Net;

namespace Jeremy.Tools.Net
{
    public class HttpResult
    {
        /// <summary>
        /// 原始的响应信息对象
        /// </summary>
        public HttpWebResponse HttpWebResponse { get; set; }

        /// <summary>
        /// 状态信息，使用 Http.Status 的枚举中选择
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// 网络状态码，如果遇到非网络问题，置为0
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// 响应信息的数据内容，错误时接收错误信息
        /// </summary>
        public string Data { get; set; }
    }
}