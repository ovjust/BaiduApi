using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduApi.BaseApi.Btc
{
    public class OrderModel_Okex
    {
        /// <summary>
        /// 
        /// </summary>
        public string client_oid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string order_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string match_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string instrument_id { get; set; }
    }
    //如果好用，请收藏地址，帮忙分享。
    public class OrderResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string error_message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string error_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string client_oid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string order_id { get; set; }
    }
}
