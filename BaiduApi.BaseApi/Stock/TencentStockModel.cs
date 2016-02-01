using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BaiduApi.BaseApi.Stock
{
    public class TencentStockModel
    {
        [DisplayName("代码")]
        public string Code { set; get; }
        [DisplayName("名称")]
        public string Name { set; get; }
        //[DisplayName("行业")]
        //public string Industry { set; get; }
        [DisplayName("最新价")]
        public decimal Price { set; get; }
        [DisplayName("涨幅")]
        public decimal Increa { set; get; }
        [DisplayName("换手率")]
        public decimal Change { set; get; }
        public decimal 市盈率 { set; get; }
        public decimal 市净率 { set; get; }
        public decimal 昨收 { set; get; }
        public decimal 最高 { set; get; }
        public decimal 最低 { set; get; }
        public decimal 涨停价 { set; get; }
        public decimal 跌停价 { set; get; }
        public decimal 流通市值 { set; get; }
        public decimal 成交量手 { set; get; }
       
        //public decimal 量比 { set; get; }
        //public decimal 净额万元 { set; get; }
        //public decimal ddx大单占通 { set; get; }
        //public decimal ddy买入占比 { set; get; }
        //public decimal ddz大买强度 { set; get; }
        //public decimal 主动率 { set; get; }
        //public decimal 通吃率 { set; get; }
        //public decimal ddx红次 { set; get; }
        //public decimal ddx红连 { set; get; }
        //public decimal 特大差 { set; get; }
        //public decimal 大单差 { set; get; }
        //public decimal 中单差 { set; get; }
        //public decimal 小单差 { set; get; }
        //public decimal 单数比 { set; get; }
        //public decimal 特大买入 { set; get; }
        //public decimal 特大卖出 { set; get; }
        //public decimal 大单买入 { set; get; }
        //public decimal 大单卖出 { set; get; }
        //public decimal 中单买入 { set; get; }
        //public decimal 中单卖出 { set; get; }
        //public decimal 小单买入 { set; get; }
        //public decimal 小单卖出 { set; get; }
    }
}
