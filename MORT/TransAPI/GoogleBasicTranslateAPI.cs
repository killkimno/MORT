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

            
            string re = deserial.Deserialize<string>(response);
            var obj = deserial.Deserialize<List<List<List<string>>>>(response);
         
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
