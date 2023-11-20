//using Pstar.Infrastructure.Helpers;
using DotNet_Utilities;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BaiduApi.BaseApi
{
    public static class RestSharpHelper
    {
        public static RestRequest GetRequest(string urlPart, Method method=Method.GET)
        {
            var request = new RestRequest(urlPart,method);
            //request.AddCookie("token",WcfClient.Token);
            request.JsonSerializer = new RestJsonSerializer();
            request.Timeout = 30000;
            return request;
        }
        public static string RequestString(RestRequest request, Method method,  string baseUrl)
        {
            //var url = AppSettings.ApiServerUrl;
            var client = new RestClient(baseUrl);
            IRestResponse content;
            request.Method = method;
            //switch (method)
            //{
            //    case Method.POST:
            //        content = client.Post(request);
            //        break;
            //    case Method.DELETE:
            //        content = client.Delete(request);
            //        break;
            //    case Method.PUT:
            //        content = client.Put(request);
            //        break;
            //    default:
            //        content = client.Get(request);
            //        break;
            //}
            content = client.Execute(request);
            if (content.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(content.StatusCode.ToString()+" "+content.Content);
            
     
          
            //JsonDeserializer json = new JsonDeserializer();
          //var str=  json.Deserialize<string>(content);
            return content.Content;
            //return content.Content.Trim('\"');
        }

        public static IRestResponse RequestResponse(RestRequest request, Method method, string baseUrl)
        {
            //var url = AppSettings.ApiServerUrl;
            var client = new RestClient(baseUrl);
            IRestResponse content;
            request.Method = method;
            //switch (method)
            //{
            //    case Method.POST:
            //        content = client.Post(request);
            //        break;
            //    case Method.DELETE:
            //        content = client.Delete(request);
            //        break;
            //    case Method.PUT:
            //        content = client.Put(request);
            //        break;
            //    default:
            //        content = client.Get(request);
            //        break;
            //}
            content = client.Execute(request);
            
            return content;
            //return content.Content.Trim('\"');
        }

        public static T Request<T>(this RestRequest request, Method method,string baseUrl) where T : new()
        {
            //var url = AppSettings.ApiServerUrl;
            var client = new RestClient(baseUrl);
            request.Method = method;
            client.Timeout = 600 * 1000;
            IRestResponse<T> content;
            content = client.Execute<T>(request);
            if (content.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(content.StatusCode.ToString() + " " + content.Content + " " + content.ErrorMessage);
            return content.Data;
            //return content.Content.Trim('\"');
        }

        public static void AddParam(RestRequest request,object source, ParameterType paramType, string[] ignorFields=null)
        {
            Type type = source.GetType();
            PropertyInfo[] pi = type.GetProperties();//BindingFlags.Public|BindingFlags.NonPublic| BindingFlags.GetField|
            foreach (PropertyInfo item in pi)
            {
                if (ignorFields==null||!ignorFields.Contains(item.Name) )
                {
                    object objSource = item.GetValue(source, null);
                    request.AddParameter(item.Name, objSource, paramType);
                }
            }
        }
        public static void AddUrlParam(RestRequest request, object source, ParameterType paramType, string[] ignorFields = null)
        {
            Type type = source.GetType();
            PropertyInfo[] pi = type.GetProperties();//BindingFlags.Public|BindingFlags.NonPublic| BindingFlags.GetField|
            foreach (PropertyInfo item in pi)
            {
                if (ignorFields == null || !ignorFields.Contains(item.Name))
                {
                    object objSource = item.GetValue(source, null);
                    var str = objSource is DateTime ? TimeHelper.FormatDate((DateTime)objSource, "1") : objSource.ToString();
                    request.AddQueryParameter(item.Name, str);
                }
            }
        }

    }
}
