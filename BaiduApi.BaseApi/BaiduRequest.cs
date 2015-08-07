using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BaiduApi.BaseApi
{
    public class BaiduRequest
    {
        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public static string Request(string url, string param)
        {
            string strURL = url + '?' + param;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            // 添加header
            request.Headers.Add("apikey", BaiduAccount.ApiKey);
            request.ContentType = "application/x-www-form-urlencoded";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }
    }
}
