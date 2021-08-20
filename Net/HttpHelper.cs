using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jeremy.Tools.Net
{
    public static class HttpHelper
    {
        private static readonly Encoding EncodingType = Encoding.UTF8;

        /// <summary>
        ///  HTTP请求(包含文本的body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(普通文本或者JSON文本)</param>
        /// <param name="method">请求的方法。请使用 WebRequestMethods.Http 的枚举值</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task<HttpResult> RequestAsync(string url, string method, string data, string token)
        {
            var httpResult = new HttpResult();
            HttpWebRequest webRequest = null;

            try
            {
                // 必须包含下面字段，如果没有，无法建立请求
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                webRequest = WebRequest.Create(url) as HttpWebRequest ?? throw new NullReferenceException(nameof(webRequest));
                webRequest.Method = method;
                webRequest.ContentType = "application/json;application/x-www-form-urlencoded;charset=UTF-8";
                if (token != null) webRequest.Headers.Add("Authorization", $"Bearer {token}");

                if (data != null)
                {
                    webRequest.AllowWriteStreamBuffering = true;
                    var d = EncodingType.GetBytes(data);
                    await using var requestStream = await webRequest.GetRequestStreamAsync();
                    await requestStream.WriteAsync(d.AsMemory(0, d.Length));
                    await requestStream.FlushAsync();
                }

                var webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
                GetResponse(ref httpResult, webResponse);
                webResponse.Close();
            }
            catch (WebException webException)
            {
                GetWebExceptionResponse(ref httpResult, webException);
            }
            catch (Exception ex)
            {
                GetExceptionResponse(ref httpResult, ex);
            }
            finally
            {
                webRequest?.Abort();
            }

            return httpResult;
        }

        /// <summary>
        /// 获取HTTP访问网络的响应结果
        /// </summary>
        /// <param name="httpResult">即将被HTTP请求封装函数返回的HttpResult变量</param>
        /// <param name="webResponse">响应对象</param>
        private static void GetResponse(ref HttpResult httpResult, HttpWebResponse webResponse)
        {
            httpResult.HttpWebResponse = webResponse;
            httpResult.Status = Status.Success;
            httpResult.HttpStatusCode = (int)webResponse.StatusCode;

            using var sr = new StreamReader(webResponse.GetResponseStream(), EncodingType);
            httpResult.Data = sr.ReadToEnd();

            webResponse.Close();
        }

        /// <summary>
        /// 获取HTTP访问网络期间发生错误时引发的异常响应信息
        /// </summary>
        /// <param name="httpResult">即将被HTTP请求封装函数返回的HttpResult变量</param>
        /// <param name="webException">访问网络期间发生错误时引发的异常对象</param>
        private static void GetWebExceptionResponse(ref HttpResult httpResult, WebException webException)
        {
            if (!(webException.Response is HttpWebResponse exResponse)) return;

            httpResult.HttpWebResponse = exResponse;
            httpResult.Status = Status.Fail;
            httpResult.HttpStatusCode = (int)exResponse.StatusCode;

            using var sr = new StreamReader(exResponse.GetResponseStream(), EncodingType);
            httpResult.Data = sr.ReadToEnd();

            exResponse.Close();
        }

        /// <summary>
        /// 获取请求阶段的非网络错误引发的异常响应信息
        /// </summary>
        /// <param name="httpResult">即将被HTTP请求封装函数返回的HttpResult变量</param>
        /// <param name="ex">发生错误时引发的异常对象</param>
        private static void GetExceptionResponse(ref HttpResult httpResult, Exception ex)
        {
            httpResult.HttpWebResponse = null;
            httpResult.Status = Status.Error;
            httpResult.HttpStatusCode = 0;
            httpResult.Data = ex.Message;
        }

        /// <summary>
        /// 通用的Get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<HttpResult> GetAsync(string url, string token = null)
        {
            return await RequestAsync(url, WebRequestMethods.Http.Get, null, token);
        }

        /// <summary>
        /// 通用的Get方法
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<HttpResult> GetAsync<TParam>(string url, TParam param, string token = null) where TParam : class
        {
            return await RequestAsync(url + param.ToUrlParams(), WebRequestMethods.Http.Get, null, token);
        }

        /// <summary>
        /// 通用的Post方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<HttpResult> PostAsync(string url, string body = "", string token = null)
        {
            return await RequestAsync(url, WebRequestMethods.Http.Post, body, token);
        }

        public static async Task<HttpResult> PostAsync<TParam>(string url, TParam param, string body = "", string token = null) where TParam : class
        {
            return await RequestAsync(url + param.ToUrlParams(), WebRequestMethods.Http.Post, body, token);
        }

        /// <summary>
        /// 转换一个参数对象为对应的字符串，以连接 URL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string ToUrlParams<T>(this T param) where T : class
        {
            var properties = param.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var sb = new StringBuilder("?");
            foreach (var p in properties)
            {
                var v = p.GetValue(param, null) ?? "";

                // 字符串需要转译
                sb.Append($"{p.Name}={Uri.EscapeDataString(v.ToString())}&");
            }

            // 删除最后一个 '&'
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}
