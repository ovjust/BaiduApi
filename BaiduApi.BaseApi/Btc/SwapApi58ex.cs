using DotNet_Utilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BaiduApi.BaseApi.Btc
{
    public class SwapApi58ex
    {
        static Dictionary<string, int> contractIds;

        public static Dictionary<string, int> ContractIds
        {
            get
            {
                if(contractIds==null)
                {
                    contractIds = new Dictionary<string, int>();
                    contractIds.Add("BTC", 1001);
                    contractIds.Add("ETH", 1003);
                }
                return contractIds;
            }

        
        }
        static Dictionary<int, int> durations;

        //0:1分钟 1:3分钟 2:5分钟 3:15分钟
        //序号错的，减1 5:30分钟 6:1小时 7:2小时 8:4小时 9:6小时 10:12小时 11:1天 86400  12:1周
        public static Dictionary<int, int> Durations
        {
            get
            {
                if (durations == null)
                {
                    durations = new Dictionary<int, int>();
                    durations.Add(60, 0);
                    durations.Add(180, 1);
                    durations.Add(300, 2);
                    durations.Add(900, 3);
                    durations.Add(1800, 4);
                    durations.Add(3600, 5);
                    durations.Add(7200, 6);
                    durations.Add(14400, 7);
                    durations.Add(86400,10);
                }
                return durations;
            }

        }

      

        public static string Order(bool open, bool direct, decimal amount, string coinType)
        {
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12| SecurityProtocolType.Ssl3; //加上这一句

            ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);// SecurityProtocolType.Tls12;
            //                                                                                               //                在Framework 4.0上,如果要允许TLS 1.0,1.1和1.2,只需替换：

            //SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
            //通过：

            //(SecurityProtocolType)(0xc0 | 0x300 | 0xc00)


            var timeStamp = TimeHelper.GetTimeStamp_ms();
            var requestPath = "/v1/regular/order/place";
            var url = "https://openapi.58ex.com";
            var method = "POST";
            var apiKey = "ee4153c7-e2ce-4189-9564-4fdd4decea7a ";
            //var SecretKey = "CE5DB7F718510CF47D73AF75D3CDF3B6";//测试示例

            var SecretKey = "6FA31EF443CAE9041F0A9570AD5821EB";

            /*
            OK - ACCESS - SIGN的请求头是对timestamp + method + requestPath + body字符串(+表示字符串连接)，以及SecretKey，使用HMAC SHA256方法加密，通过Base64编码输出而得到的。

例如：sign = CryptoJS.enc.Base64.Stringify(CryptoJS.HmacSHA256(timestamp + 'GET' + '/users/self/verify', SecretKey))

其中，timestamp的值与OK - ACCESS - TIMESTAMP请求头相同，必须是UTC时区Unix时间戳的十进制秒数格式或ISO8601标准的时间格式，精确到毫秒。

method是请求方法，字母全部大写：GET / POST。

requestPath是请求接口路径。例如：/ api / spot / v3 / orders ? instrument_id = OKB - USDT & state = 2

body是指请求主体的字符串，如果请求没有主体(通常为GET请求)则body可省略。例如：{ "product_id":"BTC-USD-0309","order_id":"377454671037440"}

            SecretKey为用户申请APIKey时所生成。例如：22582BD0CFF14C41EDBF1AB98506286D’

    */
            //var str = "symbol=BTCUSDT&side=BUY&type=LIMIT&quantity=1&price=9000&timeInForce=GTC&recvWindow=5000&timestamp=1591702613943";// + timeStamp;
            //var str1 = "AccessKeyId=089cf604-7b87-4b13-b806-eaadb67c8b70&SignatureMethod=HmacSHA256&SignatureVersion=2&Timestamp=1551084749915&";
            //var str2 = "contractId=2001&side=1&size=1&type=2";
            if (!ContractIds.ContainsKey(coinType))
                throw new Exception("不支持的币种");

            var str1 = "AccessKeyId={0}&SignatureMethod=HmacSHA256&SignatureVersion=2&Timestamp={1}&";
            var str2 = "contractId={0}&close={3}&leverage=20&side={1}&size={2}&type=2";//type=2 市价
            str1 = string.Format(str1, apiKey, timeStamp);
            str2 = string.Format(str2, ContractIds[coinType], direct ? 1 : 2, amount,open?0:1);
            var str = str1+str2 ;
    
            var sha = Sha1Encrypt.SHA256Encrypt(str, SecretKey);
            var client = new RestClient(url);
            var req = new RestRequest(requestPath + "?" + str2);
            //"X-58COIN-APIKEY:089cf604-7b87-4b13-b806-eaadb67c8b70" -H "Timestamp:1551084749915" -H "Signature:617cfbed0db129bc27feca8adedb829e5294f0c81ac3408067479fc4ecfd276b" 
            req.AddHeader("X-58COIN-APIKEY", apiKey);
            req.AddHeader("Timestamp", timeStamp);
            req.AddHeader("Signature", sha);

            //OrderModel_BiAn model = new OrderModel_BiAn()
            //{
            //    symbol = Guid.NewGuid().ToString().Replace("-", ""),
            //    side = "0.001",
            //    type = "2",//开多
            //    order_type = "4",//市价
            //    instrument_id = "BTC-USDT-SWAP"
            //};
            //req.AddJsonBody(model);

            //var content = client.Execute<OrderResult>(req, Method.POST);
            req.Method = Method.POST;
            var content = client.Execute<OrderResult>(req);
            if (content.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // var res = NewtonJsonHelper.Serialize(content.Data);
                return content.Content;
            }
            else
            {
                var res = content.StatusCode.ToString() + "\n" + content.Content + "\n" + content.ToString() + "\n" + content.ErrorMessage;
                throw new Exception(res);
            }
        }
        public static string OrderTest1(bool direct, decimal amount, string coinType)
        {
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

            ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
            //System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);// SecurityProtocolType.Tls12;
            //                                                                                               //                在Framework 4.0上,如果要允许TLS 1.0,1.1和1.2,只需替换：

            //SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
            //通过：

            //(SecurityProtocolType)(0xc0 | 0x300 | 0xc00)


            var timeStamp = TimeHelper.GetTimeStamp_ms();
            var requestPath = "/fapi/v1/order";
            var url = "https://fapi.binance.com";
            var method = "POST";
            var apiKey = "lhUJnQLL2k1dRRftHzoveeib8HMGFEEk3fitsqN5Au38PPsQTciXSQmkHlj08CRY ";
            //var SecretKey = "2b5eb11e18796d12d88f13dc27dbbd02c2cc51ff7059765ed9821957d82bb4d9";//测试示例
            var SecretKey = "d9Jt8WlxE0Je2A1uE5akrgyHVeNuZnnYydvmK2m9ULckWXDUzLsPG9CiiNJKkiUz";

            /*
            OK - ACCESS - SIGN的请求头是对timestamp + method + requestPath + body字符串(+表示字符串连接)，以及SecretKey，使用HMAC SHA256方法加密，通过Base64编码输出而得到的。

例如：sign = CryptoJS.enc.Base64.Stringify(CryptoJS.HmacSHA256(timestamp + 'GET' + '/users/self/verify', SecretKey))

其中，timestamp的值与OK - ACCESS - TIMESTAMP请求头相同，必须是UTC时区Unix时间戳的十进制秒数格式或ISO8601标准的时间格式，精确到毫秒。

method是请求方法，字母全部大写：GET / POST。

requestPath是请求接口路径。例如：/ api / spot / v3 / orders ? instrument_id = OKB - USDT & state = 2

body是指请求主体的字符串，如果请求没有主体(通常为GET请求)则body可省略。例如：{ "product_id":"BTC-USD-0309","order_id":"377454671037440"}

            SecretKey为用户申请APIKey时所生成。例如：22582BD0CFF14C41EDBF1AB98506286D’

    */
            var str = "symbol=BTCUSDT&side=BUY&type=LIMIT&quantity=0.1&price=9000&timeInForce=GTC&recvWindow=5000&timestamp="+ timeStamp;
            //var str = "symbol={0}USDT&side={1}&type=MARKET&quantity={2}&timeInForce=GTC&recvWindow=5000&timestamp={3}";
            str = string.Format(str, coinType, direct ? "BUY" : "SELL", amount, timeStamp);
            var sha = Sha1Encrypt.SHA256Encrypt(str, SecretKey);
            //var Base64 = MySecurity.EncodeBase64_UTF8(sha);
            str += "&signature=" + sha;

            var client = new RestClient(url);
            var req = new RestRequest(requestPath + "?" + str);
            req.AddHeader("X-MBX-APIKEY", apiKey);


            //OrderModel_BiAn model = new OrderModel_BiAn()
            //{
            //    symbol = Guid.NewGuid().ToString().Replace("-", ""),
            //    side = "0.001",
            //    type = "2",//开多
            //    order_type = "4",//市价
            //    instrument_id = "BTC-USDT-SWAP"
            //};
            //req.AddJsonBody(model);
            //var content = client.Execute(req, Method.POST);
            req.Method = Method.POST;
            var content = client.Execute(req);
            if (content.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // var res = NewtonJsonHelper.Serialize(content.Data);
                return content.Content;
            }
            else
            {
                var res = content.StatusCode.ToString() + "\n" + content.Content + "\n" + content.ToString() + "\n" + content.ErrorMessage;
                throw new Exception(res);
            }
        }

        /// <summary>
        /// 永续合约价格  BTC-USD-SWAP
        /// </summary>
        public static decimal? GetPriceContract(string coinType)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
               
                var url = "https://openapi.58ex.com/v1/usdt/market/ticker?contractId={0}";//BTC
                if (!ContractIds.ContainsKey(coinType))
                    throw new Exception("不支持的币种");
                url = string.Format(url, ContractIds[ coinType]);
                string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
                var jObject = NewtonJsonHelper.ParseObject(result);
                var price = jObject["data"].Value<decimal>("last");
                return price;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //https://www.okex.me/docs/zh/#index-restful-content
        }


   
        public static string GetCandles(DateTime start, DateTime end, int duration,  string coinType)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                /*
                 * contractId	long	true	合约ID		
type	int	true	K线类型		0:1分钟 1:3分钟 2:5分钟 3:15分钟 5:30分钟 6:1小时 7:2小时 8:4小时 9:6小时 10:12小时 11:1天 12:1周
since	long	false	开始时间，时间戳，最多3000条		
*/
                var url = "https://openapi.58ex.com/v1/usdt/market/kline?contractId={0}&type={1}&since={2}";//BTC
                if (!ContractIds.ContainsKey(coinType))
                    throw new Exception("不支持的币种");
                if (!Durations.ContainsKey(duration))
                    throw new Exception("不支持的K线时间类型");
                var timeStamp = TimeHelper.GetTimeStamp_ms(start);
                url = string.Format(url, ContractIds[coinType], Durations[duration], timeStamp);
                string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
                //var jObject = NewtonJsonHelper.ParseObject(result);
                //var price = jObject["data"].Value<decimal>("last");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //https://www.okex.me/docs/zh/#index-restful-content
        }
        public class OrderModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string symbol { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string side { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string timeInForce { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public decimal quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long recvWindow { get; set; }

            public long timestamp { get; set; }
        }
        //如果好用，请收藏地址，帮忙分享。
        public class OrderResult
        {

            /// <summary>
            /// 
            /// </summary>
            public string clientOrderId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cumQty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cumQuote { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string executedQty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long orderId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string avgPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string origQty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string price { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool reduceOnly { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string side { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string positionSide { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public bool closePosition { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string symbol { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string timeInForce { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string origType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string activatePrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string priceRate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long updateTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string workingType { get; set; }


        }
    }
}
