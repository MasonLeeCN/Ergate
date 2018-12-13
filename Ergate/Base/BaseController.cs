using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Ergate
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 返回Result数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="dataCount"></param>
        /// <returns></returns>
        public T ToResult<T>(T t, HttpStatusCode code = HttpStatusCode.OK, string message = "", int dataCount = 0)
        {
            if (code == HttpStatusCode.OK)
            {
                if (dataCount > 0)
                {
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(dataCount));
                }
                return t;
            }
            else
            {
                var responseMessage = new HttpResponseMessage();
                responseMessage.Content = new StringContent(message, System.Text.Encoding.UTF8);
                responseMessage.StatusCode = code;
                throw new System.Web.Http.HttpResponseException(responseMessage);
            }
        }

        /// <summary>
        /// 抛出验证错误
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="message"></param>
        protected void ValidateError(string fieldname, string message)
        {
            throw new ValidationException(fieldname, message);
        }

        /// <summary>
        /// 解密token信息
        /// </summary>
        /// <returns></returns>

        protected TokenInfo GetTokenInfo(string header = "")
        {
            var tokenInfo = new TokenInfo();
            if (string.IsNullOrEmpty(header))
            {
                header = Request.Headers.ToList().Where(x => x.Key == "Authorization").FirstOrDefault().Value;
                if (string.IsNullOrEmpty(header))
                {
                    throw new MsgException("登录信息失效");
                }
            }

            var arr = header.Split('.');
            if (arr.Length >= 2)
            {

                try
                {
                    var imgData = arr[1];
                    string dummyData = imgData.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                    if (dummyData.Length % 4 > 0)
                    {
                        dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
                    }
                    var loginInfo = dummyData;
                    var jsonResult = loginInfo.Base64Decode();
                    tokenInfo = jsonResult.Deserialize<TokenInfo>();
                }
                catch
                {

                    throw new MsgException("解密token信息错误");
                }
            }
            else
            {
                throw new MsgException("登录信息错误");
            }

            return tokenInfo;
        }


        /// <summary>
        /// Gets the parameter to md5
        /// </summary>
        /// <param name="paras">The paras.</param>
        /// <returns></returns>
        /// <exception cref="Exception">paras can't be null</exception>
        protected string GetParamKey(params object[] paras)
        {
            return paras.GetMD5();
        }
    }
}
