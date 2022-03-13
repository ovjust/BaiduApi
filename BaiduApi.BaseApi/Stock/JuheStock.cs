using BaiduApi.BaseApi.Stock;
using DotNet_Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BaiduApi.BaseApi
{
    /// <summary>
    /// 股票当前时间数据查询
    /// http://apistore.baidu.com/astore/serviceinfo/1863.html
    /// </summary>
    public class JuheStock
    {
        static string urlHs = "http://web.juhe.cn:8080/finance/stock/hs?gid={0}&key=3ff96f23a13fe1a33546587a1750114e";
        static string urlHk = "http://web.juhe.cn:8080/finance/stock/hk?num={0}&key=3ff96f23a13fe1a33546587a1750114e";

        /// <summary>
        /// 股票当前时间数据查询
        /// </summary>
        /// <param name="imgStr"></param>
        /// <returns></returns>
        /// 示例  BaiduStock.GetResult("000011");
        public static JObject GetResult(string stockCode)
        {
            string url=stockCode.Length==6?urlHs:urlHk;
           
            if(!PageValidate.IsNumber(stockCode))
                throw new Exception("股票号码必须是全数字");
            if (stockCode.Length != 6)
            { }
            else if (stockCode.StartsWith("6"))
                stockCode = "sh" + stockCode;
            else if (stockCode.StartsWith("0") || stockCode.StartsWith("3"))
                stockCode = "sz" + stockCode;
            else 
                throw new Exception("股票号码不存在");

            url = string.Format(url, stockCode);
            string result = WebRequestHelper.RequestWebClient(url, Encoding.UTF8);
            //错误返回{"errNum":-1,"errMsg":"\u56fe\u7247\u683c\u5f0f\u975e\u6cd5","querySign":"","retData":[]}
            var jObject = NewtonJsonHelper.ParseObject(result);
            if (jObject.Value<string>("resultcode") != "200")
                return null;

            return jObject;
        }
        public static decimal GetPrice (string stockCode)
        {
            var json = GetResult(stockCode);
            //var model = new StockPriceModel();

            //return model;
            var price = json["result"][0]["data"].Value<decimal>("nowPri");
            return price;
        }
        //     JSON返回示例：
        //{
        //  errNum: 0,     //返回错误码
        //  errMsg: "成功", //返回错误信息
        //  retData: {
        //      stockinfo: {
        //      name: "科大讯飞", //股票名称
        //      code: "sz002230", //股票代码，SZ指在深圳交易的股票
        //      date: "2014-09-22", //当前显示股票信息的日期
        //      time: "09:26:11",   //具体时间
        //      OpenningPrice: 27.34, //今日开盘价
        //      closingPrice: 27.34,  //昨日收盘价
        //      currentPrice: 27.34,  //当前价格
        //      hPrice: 27.34,        //今日最高价
        //      lPrice: 27.34,       //今日最低价
        //      competitivePrice: 27.30, //买一报价
        //      auctionPrice: 27.34,   //卖一报价
        //      totalNumber: 47800,    //成交的股票数
        //      turnover: 1306852.00,  //成交额，以元为单位
        //      buyOne: 6100,      //买一 
        //      buyOnePrice: 27.30, //买一价格
        //      buyTwo: 7500,  //买二
        //      buyTwoPrice: 27.29, //买二价格
        //      buyThree: 2000,   //买三
        //      buyThreePrice: 27.27,  //买三价格
        //      buyFour: 100,    //买四
        //      buyFourPrice: 27.25, //买四价格
        //      buyFive: 5700,     //买五
        //      buyFivePrice: 27.22,  //买五价格
        //      sellOne: 10150,       //卖一
        //      sellOnePrice: 27.34,  //卖一价格
        //      sellTwo: 15200,      //卖二
        //      sellTwoPrice: 27.35,  //卖二价格
        //      sellThree: 5914,     //卖三
        //      sellThreePrice: 27.36, //卖三价格
        //      sellFour: 400,        //卖四
        //      sellFourPrice: 27.37,  //卖四价格
        //      sellFive: 3000,       //卖五
        //      sellFivePrice: 27.38   //卖五价格
        //   },
        //   market: {    //大盘指数
        //      shanghai: {
        //          name: "上证指数",
        //          curdot: 2323.554, // 当前价格
        //          curprice: -5.897,  //当前价格涨幅
        //          rate: -0.25,    // 涨幅率
        //          dealnumber: 11586,  //交易量，单位为手（一百股）
        //          turnover: 98322   //成交额，万元为单位
        //      },
        //     shenzhen: {
        //          name: "深证成指",
        //          curdot: 8036.871,
        //          curprice: -10.382,
        //          rate: -0.13,
        //          dealnumber: 1083562,
        //          turnover: 126854
        //      }
        //   },
        //   klinegraph: {  //K线图
        //       minurl: "http://image.sinajs.cn/newchart/min/n/sz002230.gif", //分时K线图
        //       dayurl: "http://image.sinajs.cn/newchart/daily/n/sz002230.gif", //日K线图
        //       weekurl: "http://image.sinajs.cn/newchart/weekly/n/sz002230.gif", //周K线图
        //       monthurl: "http://image.sinajs.cn/newchart/monthly/n/sz002230.gif" //月K线图
        //   }
        //  }
        //}



        #region old
        //public class Stockinfo
        //{
        //    /// <summary>
        //    /// 科大讯飞
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string code { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public DateTime date { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string time { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double OpenningPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double closingPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double currentPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double hPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double lPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double competitivePrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double auctionPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int totalNumber { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int turnover { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int buyOne { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double buyOnePrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int buyTwo { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double buyTwoPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int buyThree { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double buyThreePrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int buyFour { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double buyFourPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int buyFive { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double buyFivePrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int sellOne { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double sellOnePrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int sellTwo { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double sellTwoPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int sellThree { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double sellThreePrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int sellFour { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double sellFourPrice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int sellFive { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double sellFivePrice { get; set; }
        //}

        //public class Shanghai
        //{
        //    /// <summary>
        //    /// 上证指数
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double curdot { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double curprice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double rate { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int dealnumber { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int turnover { get; set; }
        //}

        //public class Shenzhen
        //{
        //    /// <summary>
        //    /// 深证成指
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double curdot { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double curprice { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double rate { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int dealnumber { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int turnover { get; set; }
        //}

        //public class Market
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Shanghai shanghai { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Shenzhen shenzhen { get; set; }
        //}

        //public class Klinegraph
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string minurl { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string dayurl { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string weekurl { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string monthurl { get; set; }
        //}

        //public class RetData
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Stockinfo stockinfo { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Market market { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public Klinegraph klinegraph { get; set; }
        //}

        //public class Root
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public int errNum { get; set; }
        //    /// <summary>
        //    /// 成功
        //    /// </summary>
        //    public string errMsg { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public RetData retData { get; set; }
        //}
        #endregion

        #region new 
        //{"resultcode":"200","reason":"SUCCESSED!","result":[{"data":{"buyFive":"366900","buyFivePri":"12.720","buyFour":"161700","buyFourPri":"12.730","buyOne":"516600","buyOnePri":"12.760","buyThree":"33200","buyThreePri":"12.740","buyTwo":"483500","buyTwoPri":"12.750","competitivePri":"12.760","date":"2020-06-18","gid":"sz000001","increPer":"-0.70","increase":"-0.09","name":"平安银行","nowPri":"12.760","reservePri":"12.770","sellFive":"10000","sellFivePri":"12.810","sellFour":"13300","sellFourPri":"12.800","sellOne":"21200","sellOnePri":"12.770","sellThree":"18100","sellThreePri":"12.790","sellTwo":"11400","sellTwoPri":"12.780","time":"09:25:00","todayMax":"12.760","todayMin":"12.760","todayStartPri":"12.760","traAmount":"24920280.000","traNumber":"19530","yestodEndPri":"12.850"},"dapandata":{"dot":"12.76","name":"平安银行","nowPic":"-0.09","rate":"-0.70","traAmount":"2492","traNumber":"19530"},"gopicture":{"minurl":"http://image.sinajs.cn/newchart/min/n/sz000001.gif","dayurl":"http://image.sinajs.cn/newchart/daily/n/sz000001.gif","weekurl":"http://image.sinajs.cn/newchart/weekly/n/sz000001.gif","monthurl":"http://image.sinajs.cn/newchart/monthly/n/sz000001.gif"}}],"error_code":0}

        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public string buyFive { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyFivePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyFour { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyFourPri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyOne { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyOnePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyThree { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyThreePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyTwo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string buyTwoPri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string competitivePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string increPer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string increase { get; set; }
            /// <summary>
            /// 平安银行
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string nowPri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reservePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellFive { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellFivePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellFour { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellFourPri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellOne { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellOnePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellThree { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellThreePri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellTwo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sellTwoPri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string todayMax { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string todayMin { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string todayStartPri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string traAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string traNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string yestodEndPri { get; set; }
        }

        public class Dapandata
        {
            /// <summary>
            /// 
            /// </summary>
            public string dot { get; set; }
            /// <summary>
            /// 平安银行
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string nowPic { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string traAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string traNumber { get; set; }
        }

        public class Gopicture
        {
            /// <summary>
            /// 
            /// </summary>
            public string minurl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string dayurl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string weekurl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string monthurl { get; set; }
        }

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public Data data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Dapandata dapandata { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Gopicture gopicture { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string resultcode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ResultItem> result { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int error_code { get; set; }
        }
        #endregion
    }
}
