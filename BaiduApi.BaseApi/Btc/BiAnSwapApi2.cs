using Binance.API.Csharp.Client;
using Binance.API.Csharp.Client.Models.Enums;
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
    public class BiAnSwapApi2
    {
        //public decimal GetPrice
        static void useSSL()
        {
            //if (checkBox1.Checked)
            {
                ServicePointManager.Expect100Continue = true;

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3; //加上这一句

                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
            }
        }
        //static string apiUrl = "https://api.binance.com";
        static string apiUrl = "https://fapi.binance.com";
        
        public static decimal GetPricePair(string  pair)
        {
           
                    useSSL();
                    ApiClient apiClient = new ApiClient("@YourApiKey", "@YourApiSecret", apiUrl);
                    BinanceClient binanceClient = new BinanceClient(apiClient, false);
                    var singleTickerInfo = binanceClient.GetPriceChange24H(pair, "").Result;
                    return singleTickerInfo.First().LastPrice;
            
        }
        public static decimal GetPrice(string coinType)
        {
            var pair = coinType + "USDT";
            return GetPricePair(pair);

        }
        /// <summary>
        /// 币币交易
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="isBuy"></param>
        /// <param name="amount"></param>
        /// <param name="apiKey"></param>
        /// <param name="SecretKey"></param>
        public static void Order(string coinType, bool isBuy, decimal amount, string apiKey, string SecretKey)
        {
            coinType += "USDT";
             OrderPair(coinType,  isBuy,  amount,  apiKey,  SecretKey);
        }
        public static void OrderPair(string coinType, bool isBuy,decimal amount, string apiKey, string SecretKey)
        {
                    useSSL();
                    ApiClient apiClient = new ApiClient(apiKey, SecretKey, apiUrl);
                    BinanceClient binanceClient = new BinanceClient(apiClient, false);
                    var orderResult = binanceClient.PostNewOrder(coinType , amount, 0,isBuy? OrderSide.BUY:OrderSide.SELL, Binance.API.Csharp.Client.Models.Enums.OrderType.MARKET).Result;
            //return orderResult;
        }
    }
}
