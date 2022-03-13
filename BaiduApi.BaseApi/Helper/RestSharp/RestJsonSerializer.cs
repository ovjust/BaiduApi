using Newtonsoft.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduApi.BaseApi
{
    public class RestJsonSerializer : ISerializer
    {
        public RestJsonSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public T DeSerialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public string ContentType { get; set; }

    }
}
