using DotNet_Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace BaiduApi.BaseApi.Stock
{
    /// <summary>
    /// 新浪股票api及页面
    /// http://market.finance.sina.com.cn/pricehis.php?symbol=sh600900&startdate=2011-08-17&enddate=2011-08-19
    /// </summary>
    public class SinaStockWB
    {
        //readonly string urlByDate = "http://vip.stock.finance.sina.com.cn/quotes_service/view/vMS_tradehistory.php?symbol={0}&date={1}";//symbol=sh600000&date=2015-08-10
        readonly string urlDateRange = "http://market.finance.sina.com.cn/pricehis.php?symbol={0}&startdate={1}&enddate={2}";//symbol=sh600900&startdate=2011-08-17&enddate=2011-08-19

        Func<object, object> DocumentCompletedCallBack;
        WebBrowser webBrowser = new WebBrowser();

        /// <summary>
        /// 股票当前时间数据查询
        /// </summary>
        /// <param name="imgStr"></param>
        /// <returns></returns>
        /// 示例  BaiduStock.GetResult("000011");
        public void GetDatesPriceSlice(string stockCode,DateTime date1,DateTime date2,Func<object,object> CallBack)
        {
            this.DocumentCompletedCallBack = CallBack;

            if (!PageValidate.IsNumber(stockCode))
                throw new Exception("股票号码必须是全数字");
            if (stockCode.Length != 6)
            { }
            else if (stockCode.StartsWith("6"))
                stockCode = "sh" + stockCode;
            else if (stockCode.StartsWith("0") || stockCode.StartsWith("3"))
                stockCode = "sz" + stockCode;
            else
                throw new Exception("股票号码不存在");

            var url1 = string.Format(urlDateRange, stockCode, GetDateString(date1), GetDateString(date2));

            
            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            webBrowser.Url = new Uri(url1);
          
            //string result = WebRequestHelper.RequestWebClient(url1, Encoding.Default);
            ////StringReader reader = new StringReader(result);
            ////HtmlDocument htmlDocument=HtmlDocument.
            //XmlDocument xml = new XmlDocument();
            ////var xmlReadSetting=new XmlReaderSettings();
            ////xmlReadSetting.ProhibitDtd=false;
            ////XmlReader xmlRead = XmlReader.Create(url1, xmlReadSetting);
            //xml.InnerText = result;
            //xml.LoadXml(result);
        
        }



        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var xml = webBrowser.Document;
            var table = xml.GetElementById("datalist");
            var trs = table.Children["tbody"].GetElementsByTagName("tr");
            decimal[] returns=null;
            if (trs.Count > 0)
            {
                var maxPrice = trs[0].FirstChild.InnerText;
                var minPrice = trs[trs.Count - 1].FirstChild.InnerText;
                returns= new decimal[] { decimal.Parse(maxPrice), decimal.Parse(minPrice) };
            }
            DocumentCompletedCallBack.Invoke(returns);
        }


       static  string GetDateString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
