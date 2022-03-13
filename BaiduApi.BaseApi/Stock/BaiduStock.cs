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
    /// http://apistore.baidu.com/apiworks/servicedetail/115.html
    /// </summary>
    public class BaiduStock
    {
        static string urlHs = "http://apis.baidu.com/apistore/stockservice/stock";
        static string urlHk = "http://apis.baidu.com/apistore/stockservice/hkstock";

        /// <summary>
        /// 股票当前时间数据查询
        /// </summary>
        /// <param name="imgStr"></param>
        /// <returns></returns>
        /// 示例  BaiduStock.GetResult("000011");
        public static JObject GetResult(string stockCode)
        {
            string url = stockCode.Length == 6 ? urlHs : urlHk;
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
            


            string param = "stockid=" + stockCode;
            string result =BaiduRequest.Request(url, param);
            //错误返回{"errNum":-1,"errMsg":"\u56fe\u7247\u683c\u5f0f\u975e\u6cd5","querySign":"","retData":[]}
            var jObject = NewtonJsonHelper.ParseObject(result);
            if (jObject.Value<int>("errNum") == -1)
                return null;

            return jObject;
            //["retData"]["stockinfo"]["currentPrice"]
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
    }
}
