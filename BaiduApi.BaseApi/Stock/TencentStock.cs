using DotNet_Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BaiduApi.BaseApi.Stock
{
      /// <summary>
    /// 腾讯股票当前时间数据查询，数据较全 有市净率、换手率等，可以多股查询，也可以只查简单数据
    /// http://blog.csdn.net/ustbhacker/article/details/8365756
    /// </summary>
    public class TencentStock
    {
        static string urlList = "http://qt.gtimg.cn/q={0}";
        static readonly char resultSplit = '~';
        static readonly string paramJoin = ",";
        static readonly string pattern = @"=""([^""]*)""";

        public static List<string[]> GetStocksShort(params string[] stockCodes)
        {
            return StockCommon.GetStocks(resultSplit, true, stockCodes, urlList, paramJoin, pattern);
        }

        public static List<string[]> GetStocksLong(params string[] stockCodes)
        {
            return StockCommon.GetStocks(resultSplit, false, stockCodes, urlList, paramJoin, pattern);
        }

//返回数据：
     //   var hq_str_s_sh601996 = "丰林集团,10.27,0.13,1.28,536585,56098";
        // 价格、价格增长量、涨幅
//[html] view plaincopy
//v_sz000858="51~五 粮 液~000858~27.78~27.60~27.70~417909~190109~227800~27.78~492~27.77~332~27.76~202~27.75~334~27.74~291~27.79~305~27.80~570~27.81~269~27.82~448~27.83~127~15:00:13/27.78/4365/S/12124331/24602|14:56:55/27.80/14/S/38932/24395|14:56:52/27.81/116/B/322585/24392|14:56:49/27.80/131/S/364220/24385|14:56:46/27.81/5/B/13905/24381|14:56:43/27.80/31/B/86199/24375~20121221150355~0.18~0.65~28.11~27.55~27.80/413544/1151265041~417909~116339~1.10~10.14~~28.11~27.55~2.03~1054.39~1054.52~3.64~30.36~24.84~";  
//以 ~ 分割字符串中内容，下标从0开始，依次为
//[html] view plaincopy
// 0: 未知  
// 1: 名字  
// 2: 代码  
// 3: 当前价格  
// 4: 昨收  
// 5: 今开  
// 6: 成交量（手）  
// 7: 外盘  
// 8: 内盘  
// 9: 买一  
//10: 买一量（手）  
//11-18: 买二 买五  
//19: 卖一  
//20: 卖一量  
//21-28: 卖二 卖五  
//29: 最近逐笔成交  
//30: 时间  
//31: 涨跌  
//32: 涨跌%  
//33: 最高  
//34: 最低  
//35: 价格/成交量（手）/成交额  
//36: 成交量（手）  
//37: 成交额（万）  
//38: 换手率  
//39: 市盈率  
//40:   
//41: 最高  
//42: 最低  
//43: 振幅  
//44: 流通市值  
//45: 总市值  
//46: 市净率  
//47: 涨停价  
//48: 跌停价  
    }
}
