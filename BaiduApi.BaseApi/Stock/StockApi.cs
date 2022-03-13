using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduApi.BaseApi.Stock
{
   public  class StockApi
    {
        public static decimal? GetPrice(string stockCode)
        {
            try
            {
                return JuheStock.GetPrice(stockCode);
            }
            catch (Exception ex)
            {
                return null;
            }
          
        }
    }
}
