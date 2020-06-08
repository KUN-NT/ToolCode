using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RequestImg
{
    public class RequestHelp
    {
        public static string BaseUri = ConfigurationManager.AppSettings["ApiUri"];
        /// <summary>
        /// GET请求--异步方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">方法</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static async Task<T> TryGetAsync<T>(string action, Dictionary<string, string> param)
        {
            using (HttpClient client = new HttpClient())
            {
                //基地址/域名
                client.BaseAddress = new Uri(BaseUri);
                //设定传输格式为json
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Accept-Charset", "GB2312,utf-8;q=0.7,*;q=0.7");

                StringBuilder strUri = new StringBuilder();
                //方法
                strUri.AppendFormat("{0}?", action);
                //参数
                if (param.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in param)
                    {
                        strUri.AppendFormat("{0}={1}&", pair.Key, pair.Value);
                    }
                    strUri.Remove(strUri.Length - 1, 1); //去掉'&&'
                }
                else
                {
                    strUri.Remove(strUri.Length - 1, 1); //去掉'？'
                }
                HttpResponseMessage response = await client.GetAsync(strUri.ToString());
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                return default(T);
            }
        }
    }
}
