using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduApi.BaseApi
{
    public class NewtonJsonHelper
    {
        public static JObject ParseObject(string sJson)
        {
            var jObject = JObject.Parse(sJson);
            return jObject;
        }

        public static T ParseJson<T>(string sJson)
        {
            var obj = JsonConvert.DeserializeObject<T>(sJson);//.Trim('\"')
           return obj;
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

//       using Newtonsoft.Json;
//JsonConvert.SerializeObject（obj）
//JsonConvert.DeserializeObject<T>(string)
    }
}
