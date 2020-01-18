using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MORT
{
    class YandexAPI
    {
        public static YandexAPI instance;
        private string idKey;
        private string transCode;
        private string resultCode;

        const string url = "https://translate.yandex.net/api/v1.5/tr.json/translate";

        public void Init(string idKey)
        {
            this.idKey = idKey;
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public string Test(string original)
        {
            string result = "";

            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            client.Encoding = Encoding.UTF8;
            string downloadString = client.DownloadString("https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=ko&dt=t&q=" + RestSharp.Extensions.StringExtensions.UrlEncode(original));
            // Result: [[["Under test","テスト中",null,null,3]],null,"ja",...]


            return downloadString;
        }

        public string GetResult(string original, ref bool isError)
        {

            //https://tech.yandex.com/translate/doc/dg/reference/translate-docpage/
            //줄바꿈은 %0A 임
            string trim = original.Replace(" ", "");
            trim = trim.Replace(Environment.NewLine, "");
            Util.ShowLog(original + " / " + trim + "...");
            if (trim == "")
            {
                return "";
            }
            original = original.Replace("\n", "<nl>");
            //byte[] bytes = Encoding.Default.GetBytes("%E3%81%93%0A%E3%81%93");
            //original = Encoding.Unicode.GetString(bytes);
            Console.Write(original);
            string result = "";
            string key = idKey;

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string lang = transCode + "-" + resultCode;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");  //이건 폼형식.
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");
            request.AddParameter("key", key);
            request.AddParameter("text", RestSharp.Extensions.StringExtensions.UrlEncode(original));
            request.AddParameter("lang", lang);

            IRestResponse response = client.Execute(request);

            //RestSharp.Serialization.Json.JsonDeserializer deserial = new RestSharp.Serialization.Json.JsonDeserializer();
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();

            string re = deserial.Deserialize<string>(response);
            Util.ShowLog(re);
            Dictionary<string, object> dic = deserial.Deserialize<Dictionary<string, object>>(response);

            if (dic.ContainsKey("errorMessage"))
            {
                result = (string)dic["errorMessage"];
                if (dic.ContainsKey("errorCode"))
                {
                    result += "\n Error Cdoe : " + (string)dic["errorCode"];
                    isError = true;
                }
            }

            else if (dic.ContainsKey("text"))
            {
                List<object> resultList = (List<object>)dic["text"];

                for (int i = 0; i < resultList.Count; i++)
                {
                    result += (string)resultList[i];
                }
            }
            Util.ShowLog("\n결과" + result);
            return result;
        }






    }
}
