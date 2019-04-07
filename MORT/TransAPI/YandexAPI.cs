using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RestSharp;
using System.Net;

namespace MORT
{
    class YandexAPI
    {
        public static YandexAPI instance;
        private string idKey;
        private string transCode;
        private string resultCode;

        private string url = "https://translate.yandex.net/api/v1.5/tr.json/translate";

        public void Init(string idKey)
        {
            this.idKey = idKey;  
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public string GetResult(string original)
        {
            //https://tech.yandex.com/translate/doc/dg/reference/translate-docpage/
            //줄바꿈은 %0A 임
            string trim = original.Replace(" ", "");
            trim = trim.Replace(Environment.NewLine, "");
            Console.WriteLine(original + " / " + trim + "...");
            if (trim == "")
            {
                return "";
            }
            original = original.Replace("\n", "<nl>");
            //byte[] bytes = Encoding.Default.GetBytes("%E3%81%93%0A%E3%81%93");
            //original = Encoding.Unicode.GetString(bytes);
            Console.Write(original);
            string result = "";
            string key = "trnsl.1.1.20190224T103602Z.5adf358a3f9542f4.0ceb7102ec696d4d5bbab3a14b97a96b82d91c98";

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string lang = transCode + "-" + resultCode;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");  //이건 폼형식.
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");
            request.AddParameter("key", key);
            request.AddParameter("text", original);
            request.AddParameter("lang", lang);
      
            IRestResponse response = client.Execute(request);

            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();

            string re = deserial.Deserialize<string>(response);
            Console.WriteLine(re);
            Dictionary<string, object> dic = deserial.Deserialize<Dictionary<string, object>>(response);

            if (dic.ContainsKey("errorMessage"))
            {
                result = (string)dic["errorMessage"];
                if (dic.ContainsKey("errorCode"))
                {
                    result += "\n Error Cdoe : " + (string)dic["errorCode"];
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
            Console.WriteLine("\n결과" + result);
            return result;
        }




        

    }
}
