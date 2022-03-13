using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BaiduApi.BaseApi.Btc
{
    public class OkexOrderApi
    {
        /// <summary>
        /// ok usdt永续、 usdt交割、 usd币本位交割 提交订单
        /// </summary>
        /// <param name="open"></param>
        /// <param name="direct"></param>
        /// <param name="amount"></param>
        /// <param name="instrument_id"></param>
        /// <param name="_apiKey"></param>
        /// <param name="_secret"></param>
        /// <param name="_passPhrase"></param>
        /// <param name="isSwap">是否永续合约</param>
        /// <param name="test">使用测试账号</param>
        /// <returns></returns>
        public async Task<string> MakeRealBuy_okex(bool open, bool direct, decimal amount, string instrument_id, string _apiKey, string _secret, string _passPhrase, bool isSwap = true, bool test = false)
        {
            try
            {
                //var timeStamp = TimeHelper.GetTimeStamp_ms();

                var timeStamp = OkexSpotApi.GetOkTimeStamp(DateTime.Now);

                var requestPath = isSwap ? "/api/swap/v3/order" : "/api/futures/v3/order";
                var url = "https://www.okex.com";
                var method = "POST";
                var SecretKey = "D405FAAB41F1273005E978E4BB5C5843";

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);// 
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                /*
                OK - ACCESS - SIGN的请求头是对timestamp + method + requestPath + body字符串(+表示字符串连接)，以及SecretKey，使用HMAC SHA256方法加密，通过Base64编码输出而得到的。

    例如：sign = CryptoJS.enc.Base64.Stringify(CryptoJS.HmacSHA256(timestamp + 'GET' + '/users/self/verify', SecretKey))

    其中，timestamp的值与OK - ACCESS - TIMESTAMP请求头相同，必须是UTC时区Unix时间戳的十进制秒数格式或ISO8601标准的时间格式，精确到毫秒。

    method是请求方法，字母全部大写：GET / POST。

    requestPath是请求接口路径。例如：/ api / spot / v3 / orders ? instrument_id = OKB - USDT & state = 2

    body是指请求主体的字符串，如果请求没有主体(通常为GET请求)则body可省略。例如：{ "product_id":"BTC-USD-0309","order_id":"377454671037440"}

                SecretKey为用户申请APIKey时所生成。例如：22582BD0CFF14C41EDBF1AB98506286D’


                (3) 签名前字符串是否符合标准格式，所有要素的顺序要保持一致，可以复制如下示例跟您的签名前字符串进行比对：

    GET示例： 2020-03-28T12:21:41.274ZGET/api/general/v3/time

    POST示例：2020-03-28T12:21:581ZPOST /api/account/v3/transfer{"currency":"usdt","amount":"1.5","from":"6","to":"3","to_instrument_id":"btc-usdt"}（usdt从资金账户划转到交割btc-usdt合约账户）`<br>


    如果是POST，务必将请求 body 拼接在签名前字符串后面;
    (4) 签名结果（OK-ACCESS-SIGN）的长度应为44位，如：6t56DBHUhMoCylVvGJka4S+Ac6RQO76/P4GyvjsDoXU=，如果长度为88位，是对签名结果进行了16进制编码导致，请确保签名结果为2进制后，再进行签名。
        */

                OrderModel_Okex model = new OrderModel_Okex()
                {
                    client_oid = Guid.NewGuid().ToString().Replace("-", ""),
                    size = Math.Round(amount * 100).ToString(),
                    //type = "2",//开多
                    order_type = "4",//市价
                    instrument_id =  instrument_id,
                    match_price = "0",
                    price = ""
                };
                if (!isSwap)
                {
                    if (instrument_id.Contains("-USD-"))
                        model.size = Math.Round(amount / 0.00255m).ToString();
                }

                if (open)
                {
                    model.type = direct ? "1" : "2";
                }
                else
                {
                    model.type = direct ? "3" : "4";
                }


                var bodyStr = NewtonJsonHelper.Serialize(model);
                //var str = timeStamp + method + "" + requestPath + NewtonJsonHelper.Serialize(model);
                //var sha = Sha1Encrypt.SHA256Encrypt(str, SecretKey);
                //var Base64 = MySecurity.EncodeBase64_UTF8(sha);


                //if (!test)
                //{
                //    this._apiKey = this.editApiKey.Text;
                //    this._secret = this.editApiSecret.Text;
                //    this._passPhrase = this.editApiPassword.Text;
                //}
                using (var client = new HttpClient(new HttpInterceptor(_apiKey, _secret, _passPhrase, bodyStr, timeStamp)))
                //using (var client = new HttpClient(new HttpInterceptor(this.editApiKey.Text, this.editApiSecret.Text, this.editApiPassword.Text, bodyStr, timeStamp)))
                {
                    var res = await client.PostAsync(url + requestPath, new StringContent(bodyStr, Encoding.UTF8, "application/json"));
                    var contentStr = await res.Content.ReadAsStringAsync();
                    var jObject = NewtonJsonHelper.ParseObject(contentStr);
                    //{"result":true,"error_message":"","error_code":"0","client_oid":"8b94e86fe4944fff9d23c74e223403d7","order_id":"6282628678934528"}
                    var result1 = jObject.Value<bool>("result");
                    if (result1 == false)
                        throw new Exception(contentStr);

                    return contentStr;// JObject.Parse(contentStr);
                }
            }
            catch (Exception ex)
            {
                ////ShowError(ex);
                //var msg = "真实下单失败。" + ex.Message;
                //msg += "\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //msg += "\n" + ex.StackTrace;
                ////MessageBox.Show(msg);

                //this.Invoke((EventHandler)delegate
                //{
                //    if (isRealAct)
                //        btnStartActory_Click(null, null);
                //    else
                //        btnStartVirtual_Click(null, null);
                //});
                //UIHelper.ShowMessage(msg);
                throw ex;
                //return null;
            }

        }
    }
}
