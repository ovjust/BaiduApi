
namespace BaiduApi.BaseApi.Btc
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


      public  class HttpInterceptor : DelegatingHandler
        {
            private string _apiKey;
            private string _passPhrase;
            private string _secret;
            private string _bodyStr;
        private string _timeStamp;
        public HttpInterceptor(string apiKey, string secret, string passPhrase, string bodyStr,string timeStamp=null)
            {
                this._apiKey = apiKey;
                this._passPhrase = passPhrase;
                this._secret = secret;
                this._bodyStr = bodyStr;
            this._timeStamp = timeStamp;
            InnerHandler = new HttpClientHandler();
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var method = request.Method.Method;
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("OK-ACCESS-KEY", this._apiKey);

                
                var now = DateTime.Now;
                var timeStamp = TimeZoneInfo.ConvertTimeToUtc(now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            if (_timeStamp != null)
                timeStamp = _timeStamp;

            var requestUrl = request.RequestUri.PathAndQuery;
                string sign = "";
                if (!String.IsNullOrEmpty(this._bodyStr))
                {
                    sign = Encryptor.HmacSHA256($"{timeStamp}{method}{requestUrl}{this._bodyStr}", this._secret);
                }
                else
                {
                    sign = Encryptor.HmacSHA256($"{timeStamp}{method}{requestUrl}", this._secret);
                }

                request.Headers.Add("OK-ACCESS-SIGN", sign);
                request.Headers.Add("OK-ACCESS-TIMESTAMP", timeStamp.ToString());
                request.Headers.Add("OK-ACCESS-PASSPHRASE", this._passPhrase);

                return base.SendAsync(request, cancellationToken);
            }
        }
    

}
