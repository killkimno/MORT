using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RestSharp;

namespace MORT
{
    class GoogleBasicTranslateAPI
    {
        public static GoogleBasicTranslateAPI instance;

        private string transCode;
        private string resultCode;

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public string GetResult(string original)
        {

            string result = "";


            var client = new RestClient("https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + transCode + "&tl=" + resultCode + "&dt=t&q=" + RestSharp.Extensions.MonoHttp.HttpUtility.UrlEncode(original));
            var request = new RestRequest(Method.GET);


            request.AddHeader("content-type", "application/x-www-form-urlencoded");  //이건 폼형식.
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");


            IRestResponse response = client.Execute(request);

            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            /*

            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            client.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            client.Encoding = Encoding.UTF8;
            string downloadString = client.DownloadString("https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + transCode  + "&tl=" + resultCode + "&dt=t&q=" + RestSharp.Extensions.MonoHttp.HttpUtility.UrlEncode(original));
            // Result: [[["Under test","テスト中",null,null,3]],null,"ja",...]
            */

            
            string re = deserial.Deserialize<string>(response);
            var obj = deserial.Deserialize<List<List<List<string>>>>(response);
            /*
              Dictionary<string, List<object>> dic = deserial.Deserialize<Dictionary<string, List<object>>>(response);

            foreach(var obj in dic)
            {
                Console.WriteLine(obj.Key);
            }

          
            */
            Console.WriteLine(re);
            Console.WriteLine(obj.Count + " @@@@");
            if(obj.Count >= 1)
            {
                for (int i = 0; i < obj[0].Count; i++)
                {

                    if (obj[0][i] != null)
                        result += obj[0][i][0] + System.Environment.NewLine;
                }
            }
       

            return result;
        }
    }
}
