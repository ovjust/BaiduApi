//using Binance.API.Csharp.Client;

using DevExpress.Utils.OAuth.Provider;
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
    public class BiAnSwapApi
    {
        public static OrderResult Order( bool direct, decimal amount, string coinType)
        {
            return Order(true, direct,  amount,  coinType);
        }

        public static OrderResult Order(bool isOpen, bool direct, decimal amount, string coinType, string apiKey = null, string SecretKey = null, bool doubleHold = true)
        {
            coinType += "USDT";
          return   OrderPair( isOpen,  direct,  amount,  coinType,  apiKey = null,  SecretKey,  doubleHold);
        }
        public static OrderResult OrderPair(bool isOpen, bool direct, decimal amount, string coinType,string apiKey=null,string SecretKey=null,bool doubleHold=true)
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
            var requestPath = "/fapi/v1/order";
            var url = "https://fapi.binance.com";
            var method = "POST";
            //var apiKey = "lhUJnQLL2k1dRRftHzoveeib8HMGFEEk3fitsqN5Au38PPsQTciXSQmkHlj08CRY ";
            //var SecretKey = "2b5eb11e18796d12d88f13dc27dbbd02c2cc51ff7059765ed9821957d82bb4d9";//测试示例
            //var SecretKey = "d9Jt8WlxE0Je2A1uE5akrgyHVeNuZnnYydvmK2m9ULckWXDUzLsPG9CiiNJKkiUz";

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
            var str = "symbol={0}&side={1}&type=MARKET&quantity={2}&recvWindow=100000&timestamp={3}";
            str = string.Format(str, coinType, (isOpen==direct) ? "BUY" : "SELL", amount, timeStamp);
            if(doubleHold)
            {
                str += "&positionSide=" + (direct? "LONG": "SHORT");
            }
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
            //var content = client.Execute<OrderResult>(req, Method.POST);
            //var content = client.Execute<OrderResult>(req, Method.POST);
            req.Method = Method.POST;
            var content = client.Execute<OrderResult>(req);
            if (content.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // var res = NewtonJsonHelper.Serialize(content.Data);
                return content.Data;
            }
            else
            {
                var res = content.StatusCode.ToString() + "\n" + content.Content + "\n" + content.ToString() + "\n" + content.ErrorMessage;
                throw new Exception(res);
            }
        }

        /// <summary>
        /// 币本位
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="direct"></param>
        /// <param name="amount"></param>
        /// <param name="symbol"></param>
        /// <param name="apiKey"></param>
        /// <param name="SecretKey"></param>
        /// <returns></returns>
        public static OrderResult Order_Usd(bool isOpen, bool direct, decimal amount, string coinType, string symbol, string apiKey = null, string SecretKey = null)
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
            var requestPath = "/dapi/v1/order";
            var url = "https://dapi.binance.com";
            var method = "POST";
            //var apiKey = "lhUJnQLL2k1dRRftHzoveeib8HMGFEEk3fitsqN5Au38PPsQTciXSQmkHlj08CRY ";
            //var SecretKey = "2b5eb11e18796d12d88f13dc27dbbd02c2cc51ff7059765ed9821957d82bb4d9";//测试示例
            //var SecretKey = "d9Jt8WlxE0Je2A1uE5akrgyHVeNuZnnYydvmK2m9ULckWXDUzLsPG9CiiNJKkiUz";

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
            var str = "symbol={0}&side={1}&type=MARKET&quantity={2}&recvWindow=100000&timestamp={3}";
            str = string.Format(str, symbol, (isOpen && direct) ? "BUY" : "SELL", amount, timeStamp,coinType);
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
            //var content = client.Execute<OrderResult>(req, Method.POST);
            //var content = client.Execute<OrderResult>(req, Method.POST);
            req.Method = Method.POST;
            var content = client.Execute<OrderResult>(req);
            if (content.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // var res = NewtonJsonHelper.Serialize(content.Data);
                return content.Data;
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
            //var content = client.Execute<OrderResult>(req, Method.POST);
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
        /// 币本位合约价格  BTC-USD-SWAP
        /// 交易对
        /// </summary>
        public static decimal? GetPriceContract_Usd(string coinType, string symbol,string sslType=null)
        {
            //try
            //{
                ServicePointManager.Expect100Continue = true;

                if (!string.IsNullOrEmpty( sslType ))
                    System.Net.ServicePointManager.SecurityProtocol = EnumHelper.Parse<SecurityProtocolType>(sslType); //加上这一句
                else
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                var url = "https://dapi.binance.com/dapi/v1/ticker/price?symbol={0}";//BTC
                //  "symbol": "BTCUSD_200925",
                //"pair": "BTCUSD",
                url = string.Format(url,symbol, coinType);

                var client = new RestClient(url);
                var req = new RestRequest("" );
                req.Method = Method.GET;
            //查找我们导入的证书
            //X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            //certStore.Open(OpenFlags.ReadOnly);
            //var aa = certStore.Certificates;
            //X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindBySubjectName, "*.binance.com", false);
            //client.ClientCertificates.Add(certCollection[0]);

            var cer = new X509Certificate2("cer\\1.cer");
            client.ClientCertificates = new X509CertificateCollection();
            client.ClientCertificates.Add(cer);


            var content = client.Execute(req);
                if (content.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // var res = NewtonJsonHelper.Serialize(content.Data);
                    string result = content.Content;
                    var jObject = NewtonJsonHelper.ParseObject(result);
                    var price = jObject[0].Value<decimal>("price");
                    return price;
                }
                else
                {
                    var res = content.StatusCode.ToString() + "\n" + content.Content + "\n" + content.ToString() + "\n" + content.ErrorMessage;
                    throw new Exception(res);
                }

             
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //https://www.okex.me/docs/zh/#index-restful-content
        }
        public static decimal? GetPriceContract_Usd_b(string coinType, string symbol, string sslType = null)
        {
            try
            {

                var url = "https://dapi.binance.com/dapi/v1/ticker/price?symbol={0}";//BTC
                //  "symbol": "BTCUSD_200925",
                //"pair": "BTCUSD",
                url = string.Format(url, symbol);

                var result = WebRequestHelper.HttpPost(url, null, "*.binance.com", "GET");
            
                    var jObject = NewtonJsonHelper.ParseObject(result);
                    var price = jObject[0].Value<decimal>("price");
                    return price;



        }
            catch (Exception ex)
            {
                throw ;
            }

    //https://www.okex.me/docs/zh/#index-restful-content
}

        /// <summary>
        /// 币本位合约价格  BTC-USD-SWAP
        /// 交易对
        /// </summary>
        public static decimal? GetPriceContract_Usd2(string coinType, string symbol)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                var url = "http://dapi.binance.com/dapi/v1/ticker/price?symbol={0}";//BTC
                //  "symbol": "http://dapi.binance.com/dapi/v1/ticker/price?symbol=BTCUSD_200925",
                //"pair": "BTCUSD",
                url = string.Format(url, symbol, coinType);
                string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
                var jObject = NewtonJsonHelper.ParseObject(result);
                var price = jObject[0].Value<decimal>("price");
                return price;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //https://www.okex.me/docs/zh/#index-restful-content
        }

        public  static decimal GetPriceContract_Usd3(string coinType, string symbol,string url)
        {
            //// Build out a client, provide a logger, and more configuration options, or even your own APIProcessor implementation
            //var client = new BinanceClient(new ClientConfiguration()
            //{
            //    ApiKey = "YOUR_API_KEY",
            //    SecretKey = "YOUR_SECRET_KEY",
            //});

            ////You an also specify symbols like this.
            //var desiredSymbol = TradingPairSymbols.BNBPairs.BNB_BTC;
            //client.GetPrice()

            //IReponse Response = await client.GetCompressedAggregateTrades(new GetCompressedAggregateTradesRequest()
            //{
            //    Symbol = "BNBBTC",
            //    StartTime = DateTime.UtcNow().AddDays(-1),
            //    EndTime = Date.UtcNow(),
            //});


            //ApiClient apiClient = new ApiClient("@YourApiKey", "@YourApiSecret",url);
            // BinanceClient binanceClient = new BinanceClient(apiClient, false);
            //     var singleTickerInfo = binanceClient.GetPriceChange24H(coinType+"USDT").Result;//ETHBTC

            //     //var allTickersInfo = binanceClient.GetPriceChange24H().Result;
            //     return singleTickerInfo.First().LastPrice;

            return 0;
        }





        public static decimal? GetPriceContract(string coinType)
        {
            var pair = coinType + "USDT";
            return GetPriceContractPair(pair);
        }
        /// <summary>
        /// 永续合约价格  BTC-USD-SWAP
        /// </summary>
        public static decimal? GetPriceContractPair(string pair)
        {
            string url=null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                url = "https://fapi.binance.com/fapi/v1/ticker/price?symbol={0}";//BTC
                                                                                 
                //url = "http://103.151.217.238:8091/fapi/v1/ticker/price?symbol={0}";//BTC


                url = string.Format(url, pair);
                string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
                var jObject = NewtonJsonHelper.ParseObject(result);
                var price = jObject.Value<decimal>("price");
                return price;
            }
            catch (Exception ex)
            {
                throw new Exception(url) ;
                //return url;
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
