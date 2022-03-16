using Binance.API.Csharp.Client;
using Binance.API.Csharp.Client.Models.Enums;
using DotNet_Utilities;
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
    public class BiAnFApi_candle
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
        static string apiUrl = "https://fapi.binance.com/fapi/v1/klines?symbol={0}&interval={1}&startTime={2}&endTime={3}";
        //https://fapi.binance.com/fapi/v1/klines?symbol=BTCUSDT&interval=15m&startTime=&endTime=
        public static int timeZone = 8;

        static Dictionary<int, string> durations;
        /*
         *     MIN1 = "1m"
    MIN3 = "3m"
    MIN5 = "5m"
    MIN15 = "15m"
    MIN30 = "30m"
    HOUR1 = "1h"
    HOUR2 = "2h"
    HOUR4 = "4h"
    HOUR6 = "6h"
    HOUR8 = "8h"
    HOUR12 = "12h"
    DAY1 = "1d"
    DAY3 = "3d"
    WEEK1 = "1w"
    MON1 = "1m"
    INVALID = None
         * 
         * */
        public static Dictionary<int, string> Durations
        {
            get
            {
                if (durations == null)
                {
                    durations = new Dictionary<int, string>();
                    durations.Add(60,"1m");
                    durations.Add(180, "3m");
                    durations.Add(300, "5m");
                    durations.Add(900, "15m");
                    durations.Add(1800, "30m");
                    durations.Add(3600, "1h");
                    durations.Add(7200, "2h");
                    durations.Add(14400, "4h");
                    durations.Add(86400, "1d");
                }
                return durations;
            }
        }

        public static string GetCandles_pair(string symbol, int interval,
                      DateTime start, DateTime end)
        {

            useSSL();
            //start = start.AddHours(-timeZone);
            var time1 = TimeHelper.GetTimeStamp_ms(start);
            //end = end.AddHours(-timeZone);
            var time2 = TimeHelper.GetTimeStamp_ms(end);

            var duration = Durations[interval];
              var  url = string.Format(apiUrl, symbol, duration,time1, time2);

            //var url = urlBaseCandle;
            //string result = WebRequestHelper.RequestHttpClientProxy(url, Encoding.UTF8);
            string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);

            return result;

        }
        public static string GetCandles(DateTime start, DateTime end, int duration, string coinName = "BTC", string instrument_id = "", bool isSwap = true)
        {
            if (isSwap)
                instrument_id = coinName + "USDT";
            return GetCandles_pair(instrument_id,duration,start, end);
        }


        public static string GetAllCoinsPrice()
        {
            var url= "https://fapi.binance.com/fapi/v1/ticker/24hr";
            useSSL();
            string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
            return result;

            /*
             * {
 1 "symbol": "BTCUSDT",
  "priceChange": "-94.99999800",    //24小时价格变动
 1 "priceChangePercent": "-95.960",  //24小时价格变动百分比
  "weightedAvgPrice": "0.29628482", //加权平均价
 1 "lastPrice": "4.00000200",        //最近一次成交价
  "lastQty": "200.00000000",        //最近一次成交额
  "openPrice": "99.00000000",       //24小时内第一次成交的价格
  "highPrice": "100.00000000",      //24小时最高价
  "lowPrice": "0.10000000",         //24小时最低价
  "volume": "8913.30000000",        //24小时成交量
  "quoteVolume": "15.30000000",     //24小时成交额
  "openTime": 1499783499040,        //24小时内，第一笔交易的发生时间
  "closeTime": 1499869899040,       //24小时内，最后一笔交易的发生时间
  "firstId": 28385,   // 首笔成交id
  "lastId": 28460,    // 末笔成交id
  "count": 76         // 成交笔数
}
             * */
        }
    }
}
