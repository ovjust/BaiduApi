using MihaZupan;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BaiduApi.BaseApi
{
    public class WebRequestHelper
    {
        public static  string RequestHttpClientProxy(string url, Encoding encode = null, IWebProxy proxy1 = null)
        {
            //比如这个是sock5代理的IP和端口，我随便写的，你们换成自己的就行（针对的是sock5不加密的情况下写法）
            var socks5Hostname = "103.151.217.156";
            int socks5Port = 1080;
            //Console.WriteLine("====================================================");
            //Console.WriteLine("sock5代理IP:" + socks5Hostname + ":" + socks5Port + "");
            //Console.WriteLine("====================================================");
            HttpToSocks5Proxy proxy = new HttpToSocks5Proxy(socks5Hostname, socks5Port);
            HttpClientHandler handler = new HttpClientHandler() { Proxy = proxy };
            HttpClient httpClient = new HttpClient(handler, true);
            //httpClient.gets
               var result = AwaitByTaskCompleteSource(async ()=> {  return await httpClient.GetStringAsync(url); });
            return result;
        }

        private static string AwaitByTaskCompleteSource(Func<Task<string>> func)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();
            var task1 = taskCompletionSource.Task;
            Task.Run(async () =>
            {
                var result = await func.Invoke();
                taskCompletionSource.SetResult(result);
            });
            var task1Result = task1.Result;
            //Debug.WriteLine($"3. AwaitByTaskCompleteSource end:{task1Result}");
            return task1Result;
        }

        public static string RequestWebClient(string url, Encoding encode=null, IWebProxy proxy= null)
        {
          
            using (WebClient client = new WebClient())
            {
             
                if (proxy != null)
                    client.Proxy = proxy;
                if (encode != null)
                {
                    client.Encoding =encode;
                }
                var ret= client.DownloadString(url);
                return ret;
            }
        }

        public static string PostWebClient(string url,string method,NameValueCollection values, Encoding encode=null)
        {
            using (WebClient client = new WebClient())
            {
                //client.Headers["Accept"] = "application/json, text/javascript, */*; q=0.01";
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                if (encode == null)
                {
                    encode = Encoding.UTF8;
                }
                client.Encoding = encode;
                var retBytes =client.UploadValues(url, method, values);
                string str = encode.GetString(retBytes);
                return str;
            }
        }



        public static void SendPostByWebClientTonce(string url, string base64, string tonce, string postData)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["Content-type"] = "application/json-rpc";
                client.Headers["Authorization"] = "Basic " + base64;
                client.Headers["Json-Rpc-Tonce"] = tonce;
                try
                {
                    byte[] response = client.UploadData(
                        url, "POST", Encoding.Default.GetBytes(postData));
                    Console.WriteLine("\nResponse: {0}", Encoding.UTF8.GetString(response));
                }
                catch (System.Net.WebException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static string SendPostByWebRequestTonce(string url, string base64,
                                                string tonce, string postData)
        {
            //WebRequest webRequest = WebRequest.Create(url);
            WebRequest webRequest = HttpWebRequest.Create(url);
            if (webRequest == null)
            {
                Console.WriteLine("Failed to create web request for url: " + url);
                return null;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(postData);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json-rpc";
            webRequest.ContentLength = bytes.Length;
            webRequest.Headers["Authorization"] = "Basic " + base64;
            webRequest.Headers["Json-Rpc-Tonce"] = tonce;
            try
            {
                // Send the json authentication post request
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(bytes, 0, bytes.Length);
                    dataStream.Close();
                }
                // Get authentication response
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            string s = reader.ReadToEnd();
                            return s;
                            //MessageBox.Show(s);
                            //Console.WriteLine("Response: " + reader.ReadToEnd());
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static string HttpPost(string url, string param = null,string cert= "www.xxx.com", string methodType= "POST")
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;   //协议按需选择，
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;

            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Method =methodType;
            request.ContentType = "application/json";
            request.Accept = "*/*";
            request.Timeout = 10*1000;
            request.AllowAutoRedirect = false;
            ////查找我们导入的证书
            //X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            //certStore.Open(OpenFlags.ReadOnly);
            //var aa = certStore.Certificates;
            //X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindBySubjectName, cert, false);
            //request.ClientCertificates.Add(certCollection[0]);

            var cer = new X509Certificate2("cer\\1.cer");
            //client.ClientCertificates = new X509CertificateCollection();
            request.ClientCertificates.Add(cer);

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }
    }
}
