using DotNet_Utilities;
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
    /// 百度文字识别类
    /// http://apistore.baidu.com/apiworks/servicedetail/146.html
    /// </summary>
    public class BaiduOcr
    {
        static string url = "http://apis.baidu.com/apistore/idlocr/ocr";
       

        /// <summary>
        /// 识别文字
        /// </summary>
        /// <param name="imgStr"></param>
        /// <returns></returns>
        /// 示例  var img = ImageIO.ReadFile("test1.jpg");
        ///    var imgStr=ImageToString.ToBase64String(img,ImageFormat.Jpeg);
        ///    imgStr = HttpUtility.UrlEncode(imgStr);
        ///       imgStr = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDABMNDxEPDBMREBEWFRMXHTAfHRsbHTsqLSMwRj5KSUU+RENNV29eTVJpU0NEYYRiaXN3fX59S12Jkoh5kW96fXj/2wBDARUWFh0ZHTkfHzl4UERQeHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHj/wAARCAAfACEDAREAAhEBAxEB/8QAGAABAQEBAQAAAAAAAAAAAAAAAAQDBQb/xAAjEAACAgICAgEFAAAAAAAAAAABAgADBBESIRMxBSIyQXGB/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/APawEBAQEBAgy8i8ZTVV3UY6V1eU2XoWDDZB19S646Gz39w9fkKsW1r8Wm2yo1PYis1be0JG9H9QNYCAgc35Cl3yuVuJZl0cB41rZQa32dt2y6OuOiOxo61vsLcVblxaVyXD3hFFjL6La7I/sDWAgICAgICB/9k=";
        public static string GetResult(string imgStr)
        {
            string param = "fromdevice=pc&clientip=10.10.10.0&detecttype=LocateRecognize&languagetype=CHN_ENG&imagetype=1&image=" + imgStr;
            string result =BaiduRequest.Request(url, param);
            //错误返回{"errNum":-1,"errMsg":"\u56fe\u7247\u683c\u5f0f\u975e\u6cd5","querySign":"","retData":[]}
            // 成功返回{
            //  "errNum": "0",
            //  "errMsg": "success",
            //  "querySign": "4249122576,294700750",
            //  "retData": [
            //    {
            //      "rect": {
            //        "left": "14",
            //        "top": "21",
            //        "width": "47",
            //        "height": "20"
            //      },
            //      "word": "  MgC"
            //    }
            //  ]
            //}
            var jObject = NewtonJson.ParseJson(result);
            if (jObject.Value<int>("errNum") == -1)
                return null;

            var first = jObject["retData"].FirstOrDefault();
            if (first == null)
                return null;

            return first["word"].ToString().Trim();
        }

     
    }
}
