using System;
using System.IO;
using System.Net;
using System.Text;

namespace Jeremy.Tools.Net
{
    public class HttpHelper
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
        private static HttpResult Request(string url, string method, string data, string token)
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
                    using var requestStream = webRequest.GetRequestStream();
                    requestStream.Write(d, 0, d.Length);
                    requestStream.Flush();
                }

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
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

            using (var sr = new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException(), EncodingType))
            {
                httpResult.Data = sr.ReadToEnd();
            }

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

            using (var sr = new StreamReader(exResponse.GetResponseStream() ?? throw new InvalidOperationException(), EncodingType))
            {
                httpResult.Data = sr.ReadToEnd();
            }

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
        public static HttpResult Get(string url, string token = null)
        {
            return Request(url, WebRequestMethods.Http.Get, null, token);
        }

        /// <summary>
        /// 通用的Post方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static HttpResult Post(string url, string body = "", string token = null)
        {
            return Request(url, WebRequestMethods.Http.Post, body, token);
        }
    }
}
