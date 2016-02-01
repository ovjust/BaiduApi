using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BaiduApi.BaseApi.Stock
{
    public class ShdjtStockModelBase
    {
        [DisplayName("代码")]
        public virtual string Code { set; get; }

        //扩展字段
        public virtual decimal 昨收 { set; get; }
        public virtual decimal 市盈率 { set; get; }
        public virtual decimal 市净率 { set; get; }
        //public decimal 换手率 { set; get; }
        public virtual decimal 流通市值 { set; get; }
        public virtual decimal 总市值 { set; get; }
        public virtual decimal 最高 { set; get; }
        public virtual decimal 最低 { set; get; }
        public virtual decimal 振幅 { set; get; }  
    }
}
