using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduApi.BaseApi.Btc
{
    public class BtcPriceApi
    {
        public static decimal? GetPrice(string currency1, string currency2)
        {

            return OkexSpotApi.GetPrice(currency1, currency2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency1"></param>
        /// <param name="currency2"></param>
        /// <param name="method">渠道</param>
        /// <returns></returns>
        public static decimal? GetPrice(string currency1, string currency2,string method)
        {

            return OkexSpotApi.GetPrice(currency1, currency2);
        }


        public static string Order()
        {
            return null;
        }
    }
}
