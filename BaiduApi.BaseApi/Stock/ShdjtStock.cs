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
    /// 散户大家庭api及页面 显示所有股票
    /// http://www.shdjt.com/sh.asp 与 http://www.shdjt.com/sz.asp  http://www.shdjt.com/cy.htm
    /// </summary>
    public class ShdjtStock
    {
        static readonly string url = "http://www.shdjt.com/{0}.htm";//
        public static readonly string[] markets = new string[] { "sh", "sz", "cy" };


        public static List<ShdjtStockModel> GetStocksAll()
        {
            List<ShdjtStockModel> list = new List<ShdjtStockModel>();
            foreach (var s in markets)
            {
                GetStocks(s, list);
            }
            return list;
        }

        /// <summary>
        /// 股票当前时间数据查询
        /// </summary>
        /// <param name="imgStr"></param>
        /// <returns></returns>
        /// 示例  BaiduStock.GetResult("000011");
        public static void GetStocks(string market, List<ShdjtStockModel> list)
        {
            var url1 = string.Format(url, market);

            string result = WebRequestHelper.RequestWebClient(url1, Encoding.Default);

            CQ cq = result;
            var tbody = cq["#senfe tbody"];
            var trs = cq["#senfe tbody tr"];
            try
            {
                for (var i = 0; i < trs.Length; i++)
                {

                    CQ tr = tbody.Find(string.Format("tr:eq({0})", i));
                    var model = new ShdjtStockModel();
                    model.Code = tr.Find("td:eq(1)").Text();
                    model.Name = tr.Find("td:eq(2) a:first").Text();
                    model.Industry = tr.Find("td:eq(3)").Text();
                    model.Price = decimal.Parse(tr.Find("td:eq(4)").Text());
                    model.Increa = decimal.Parse(tr.Find("td:eq(5)").Text().TrimEnd('%'));
                    model.Change = decimal.Parse(tr.Find("td:eq(25)").Text());
                    model.量比 = decimal.Parse(tr.Find("td:eq(26)").Text());

                    model.净额万元 = decimal.Parse(tr.Find("td:eq(6)").Text());
                    model.ddx大单占通 = decimal.Parse(tr.Find("td:eq(7)").Text());
                    model.ddy买入占比 = decimal.Parse(tr.Find("td:eq(8)").Text());
                    model.ddz大买强度 = decimal.Parse(tr.Find("td:eq(9)").Text());
                    model.主动率 = decimal.Parse(tr.Find("td:eq(10)").Text());
                    model.通吃率 = decimal.Parse(tr.Find("td:eq(11)").Text());
                    model.ddx红次 = decimal.Parse(tr.Find("td:eq(12)").Text());
                    model.ddx红连 = decimal.Parse(tr.Find("td:eq(13)").Text());
                    model.特大差 = decimal.Parse(tr.Find("td:eq(14)").Text());
                    model.大单差 = decimal.Parse(tr.Find("td:eq(15)").Text());
                    model.中单差 = decimal.Parse(tr.Find("td:eq(16)").Text());
                    model.小单差 = decimal.Parse(tr.Find("td:eq(17)").Text());
                    model.特大买入 = decimal.Parse(tr.Find("td:eq(18)").Text());
                    model.特大卖出 = decimal.Parse(tr.Find("td:eq(18)").Text());
                    model.大单买入 = decimal.Parse(tr.Find("td:eq(19)").Text());
                    model.大单卖出 = decimal.Parse(tr.Find("td:eq(20)").Text());
                    model.中单买入 = decimal.Parse(tr.Find("td:eq(21)").Text());
                    model.中单卖出 = decimal.Parse(tr.Find("td:eq(22)").Text());
                    model.小单买入 = decimal.Parse(tr.Find("td:eq(23)").Text());
                    model.小单卖出 = decimal.Parse(tr.Find("td:eq(24)").Text());
                    list.Add(model);

                }
            }
            catch (Exception ex)
            {
            }
        }




    }
}
