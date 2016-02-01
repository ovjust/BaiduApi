using CsQuery;
using DotNet_Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace BaiduApi.BaseApi.Stock
{
    /// <summary>
    /// 新浪股票api及页面
    /// http://market.finance.sina.com.cn/pricehis.php?symbol=sh600900&startdate=2011-08-17&enddate=2011-08-19
    /// </summary>
    public class SinaStock
    {
        //readonly string urlByDate = "http://vip.stock.finance.sina.com.cn/quotes_service/view/vMS_tradehistory.php?symbol={0}&date={1}";//symbol=sh600000&date=2015-08-10
        static readonly string urlDateRange = "http://market.finance.sina.com.cn/pricehis.php?symbol={0}&startdate={1}&enddate={2}";//symbol=sh600900&startdate=2011-08-17&enddate=2011-08-19
        static readonly string urlList = "http://hq.sinajs.cn/list={0}";
        static readonly char resultSplit=',';
        static readonly string paramJoin=",";
        static readonly string pattern = @"=""([^""]*)""";
        /*http://hq.sinajs.cn/list=s_sh000001 或list=sh000001上证指数
         * http://hq.sinajs.cn/list=s_sh601996,sh000001 返回简单的 var hq_str_s_sh601996="丰林集团,10.27,0.13,1.28,536585,56098"; 
         * 数据含义分别为：指数名称，当前点数，当前价格，涨跌率，成交量（手），成交额（万元）；
         * 
         * */

        /// <summary>
        /// 查询多股的当前价格、涨幅基本数据 
        /// </summary>
        /// <param name="stockCodes"></param>
        /// <returns></returns>
        public static List<string[]> GetStocksShort(params string[] stockCodes)
        {
            return StockCommon.GetStocks(resultSplit, true, stockCodes,urlList,paramJoin,pattern);
        }

        public static List<string[]> GetStocksLong(params string[] stockCodes)
        {
            return StockCommon.GetStocks(resultSplit, false, stockCodes, urlList, paramJoin, pattern);
        }
     
        /// <summary>
        /// 股票当前时间数据查询
        /// </summary>
        /// <param name="imgStr"></param>
        /// <returns></returns>
        /// 示例  BaiduStock.GetResult("000011");
        public static decimal[] GetDatesPriceSlice(string stockCode, DateTime date1, DateTime date2)
        {
            stockCode = StockCommon.FormateStockCodeCommon(stockCode);

            var url1 = string.Format(urlDateRange, stockCode, GetDateString(date1), GetDateString(date2));

            string result = WebRequestHelper.RequestWebClient(url1, Encoding.Default);
            decimal[] returns = null;
            #region  放弃使用xml htmlDocument等方法，因为加载出错且很慢
            // XPathDocument xmldContext = new XPathDocument(new StringReader(result.Replace("\r\n", string.Empty)));
            // XPathNavigator xnav = xmldContext.CreateNavigator(); XPathNodeIterator xpNodeIter = xnav.Select("datalist"); 
            //var iCount = xpNodeIter.Count; 

            ////StringReader reader = new StringReader(result);
            ////HtmlDocument htmlDocument=HtmlDocument.
            //XmlDocument xml = new XmlDocument();
            ////var xmlReadSetting=new XmlReaderSettings();
            ////xmlReadSetting.ProhibitDtd=false;
            ////XmlReader xmlRead = XmlReader.Create(url1, xmlReadSetting);
            //xml.InnerText = result;
            //xml.LoadXml(result);

            //var xml = webBrowser.Document;
            //var table = xml.GetElementById("datalist");
            //var trs = table.Children["tbody"].GetElementsByTagName("tr");

            //if (trs.Count > 0)
            //{
            //    var maxPrice = trs[0].FirstChild.InnerText;
            //    var minPrice = trs[trs.Count - 1].FirstChild.InnerText;
            //    returns = new decimal[] { decimal.Parse(maxPrice), decimal.Parse(minPrice) };
            #endregion

            CQ cq = result;
            var trs = cq[".list tbody tr"];
            if (trs.Length != 0)
            {
                var maxPrice = trs.First().Find("td:first").Text(); //cq[".list tbody tr"].First().Children().First().Text();
                var minPrice = trs.Last().Find("td:first").Text(); ;
                returns = new decimal[] { decimal.Parse(maxPrice), decimal.Parse(minPrice) };
            }
            return returns;

        }

       

      




        static string GetDateString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }


        /*
         * 0：”大秦铁路”，股票名字；
1：”27.55″，今日开盘价；
2：”27.25″，昨日收盘价；
3：”26.91″，当前价格；
4：”27.55″，今日最高价；
5：”26.20″，今日最低价；
6：”26.91″，竞买价，即“买一”报价；
7：”26.92″，竞卖价，即“卖一”报价；
8：”22114263″，成交的股票数，由于股票交易以一百股为基本单位，所以在使用时，通常把该值除以一百；
9：”589824680″，成交金额，单位为“元”，为了一目了然，通常以“万元”为成交金额的单位，所以通常把该值除以一万；
10：”4695″，“买一”申请4695股，即47手；
11：”26.91″，“买一”报价；
12：”57590″，“买二”
13：”26.90″，“买二”
14：”14700″，“买三”
15：”26.89″，“买三”
16：”14300″，“买四”
17：”26.88″，“买四”
18：”15100″，“买五”
19：”26.87″，“买五”
20：”3100″，“卖一”申报3100股，即31手；
21：”26.92″，“卖一”报价
(22, 23), (24, 25), (26,27), (28, 29)分别为“卖二”至“卖四的情况”
30：”2008-01-11″，日期；
31：”15:05:32″，时间；
         * 
         * */
    }
}
