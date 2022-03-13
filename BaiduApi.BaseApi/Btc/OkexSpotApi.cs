//using DotNet_Utilities;
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
    public class OkexSpotApi
    {
        /// <summary>
        /// 币币价格url
        /// </summary>
        static string urlBase = "https://www.okex.com/api/index/v3/{0}/constituents";//{0}-{1}

        /// <summary>
        /// 币币价格
        /// </summary>
        public static decimal? GetPrice(string currency1, string currency2)
        {
            var pair = string.Format("{0}-{1}", currency1, currency2);
            return GetPrice(pair);
        }
        public static decimal? GetPrice(string pairCode)
        {
            try
            {
                //ServicePointManager.Expect100Continue = true;
                ////System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3; //加上这一句

                //ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);// SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                //                在Framework 4.0上,如果要允许TLS 1.0,1.1和1.2,只需替换：

                //SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
                //通过：

                //(SecurityProtocolType)(0xc0 | 0x300 | 0xc00)
                var url = string.Format(urlBase,pairCode);
                string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
                var jObject = DotNet_Utilities.NewtonJsonHelper.ParseObject(result);
                var price = jObject["data"].Value<decimal>("last");
                return price;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }

            //https://www.okex.me/docs/zh/#index-restful-content
        }
        /// <summary>
        /// 永续合约价格url
        /// </summary>
        static string urlBaseContract = "https://www.okex.me/api/swap/v3/instruments/BTC-USDT-SWAP/index";

        /// <summary>
        /// 永续合约价格  BTC-USD-SWAP
        /// </summary>
        public static decimal? GetPriceContract()
        {
            try
            {
                //ServicePointManager.Expect100Continue = true;
                ////System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3; //加上这一句

                //ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);// SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                //                在Framework 4.0上,如果要允许TLS 1.0,1.1和1.2,只需替换：

                //SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
                //通过：

                //(SecurityProtocolType)(0xc0 | 0x300 | 0xc00)
                //var url = string.Format(urlBaseContract, currency1);
                var url = urlBaseContract;
                string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
                var jObject = NewtonJsonHelper.ParseObject(result);
                var price = jObject.Value<decimal>("index");
                return price;
            }
            catch (Exception ex)
            {
                return null;
            }

            //https://www.okex.me/docs/zh/#index-restful-content
        }

        /// <summary>
        /// 永续合约价格url
        /// </summary>
        //static string urlBaseCandle = "http://www.okex.com/api/swap/v3/instruments/BTC-USDT-SWAP/candles?start={0}&end={1}&granularity={2}";
        static string urlBaseCandle = "http://www.okex.com/api/swap/v3/instruments/{3}-USDT-SWAP/candles?start={0}&end={1}&granularity={2}";
        static string urlBaseCandle_pair = "http://www.okex.com/api/swap/v3/instruments/{3}-SWAP/candles?start={0}&end={1}&granularity={2}";

        static string urlInstrumentCandle = "http://www.okex.com/api/futures/v3/instruments/{3}/candles?start={0}&end={1}&granularity={2}";
        //103.151.217.238:8091

        public static string timeFormat = "yyyy-MM-ddTHH:mm:ss.000Z";
        public static int timeZone = 8;

        public static string GetOkTimeStamp(DateTime start)
        {
            start = start.AddHours(-timeZone);
            var time1 = start.ToString(timeFormat);
            return time1;
        }
        /// <summary>
        /// 永续合约k线  BTC-USD-SWAP
        /// </summary>
        /// <param name="start">开始时间，北京时间</param>
        /// <param name="end"></param>
        /// <param name="duration">间隔，秒。 开始、结束时间 应不大于200个间隔</param>
        /// <param name="isSwap">是否永续</param>
        /// <returns></returns>
        public static string GetCandles(DateTime start, DateTime end, int duration, string coinName = "BTC",  string instrument_id = "", bool isSwap = true)
        {
            if (isSwap)
                instrument_id = coinName + "-USDT-SWAP";
            return GetCandles_pair(start, end, duration, instrument_id, isSwap);
        }


        /// <summary>
        /// 永续合约k线  BTC-USD-SWAP
        /// </summary>
        /// <param name="start">开始时间，北京时间</param>
        /// <param name="end"></param>
        /// <param name="duration">间隔，秒。 开始、结束时间 应不大于200个间隔</param>
        /// <param name="isSwap">是否永续</param>
        /// <returns></returns>
        public static string GetCandles_pair(DateTime start, DateTime end, int duration, string instrument_id = "", bool isSwap = true)
        {
            string url=null;
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);// SecurityProtocolType.Tls12;
            //                在Framework 4.0上,如果要允许TLS 1.0,1.1和1.2,只需替换：

            //SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
            //通过：

            //(SecurityProtocolType)(0xc0 | 0x300 | 0xc00)

            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
            //WebProxy proxyObject = new WebProxy("103.151.217.156", 1080);//str为IP地址 port为端口号 代理类  

            start = start.AddHours(-timeZone);
            var time1 = start.ToString(timeFormat);
            end = end.AddHours(-timeZone);
            var time2 = end.ToString(timeFormat);
            
            if (isSwap)
                url = string.Format(urlBaseCandle_pair, time1, time2, duration, instrument_id);
            else
                url = string.Format(urlInstrumentCandle, time1, time2, duration, instrument_id);
            //var url = urlBaseCandle;
            //string result = WebRequestHelper.RequestHttpClientProxy(url, Encoding.UTF8);
            string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);

            return result;
            }
             catch (Exception ex)
            {
                return url;
            }

            //https://www.okex.me/docs/zh/#index-restful-content
        }
    }
}
