using DotNet_Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BaiduApi.BaseApi.Stock
{
    public class StockCommon
    {
        public static string FormateStockCodeCommon(string stockCode)
        {
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
            return stockCode;
        }

        public static string FormateStockCode_SinaShort(string stockCode)
        {
            return "s_" + StockCommon.FormateStockCodeCommon(stockCode);
        }

        /// <summary>
        /// 查询多股的当前价格、报价信息,sina、tencent使用
        /// </summary>
        /// <param name="stockCodes"></param>
        /// <returns></returns>
        public static List<string[]> GetStocks(char resultSplit, bool resultShort, string[] stockCodes, string urlList, string paramJoin, string pattern)
        {
            string[] arr;
            if (resultShort)
                arr = stockCodes.Select(p => StockCommon.FormateStockCode_SinaShort(p)).ToArray();
            else
                arr = stockCodes.Select(p => StockCommon.FormateStockCodeCommon(p)).ToArray();
            var code = string.Join(paramJoin, arr);
            var url1 = string.Format(urlList, code);

            string result = WebRequestHelper.RequestWebClient(url1, Encoding.Default);
            //Match match = Regex.Match(result, @"=""([^""]*)""");//([^<]*)  a-z0-9A-Z_~./\-:|
            MatchCollection mc = Regex.Matches(result, pattern);
            //if (match.Success)
            //{
            //    var info = match.Groups[1].Value;
            //    return info.Split(',');
            //}
            return GetStocksMatchResult(mc, resultSplit);
        }

        private static List<string[]> GetStocksMatchResult(MatchCollection mc, char split)
        {
            List<string[]> list = new List<string[]>();
            foreach (Match match in mc)
            {
                var info = match.Groups[1].Value;
                list.Add(info.Split(split));
            }
            return list;
        }
    }
}
