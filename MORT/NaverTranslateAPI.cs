using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RestSharp;
using System.Net;

namespace MORT
{

    class NaverTranslateAPI
    {
        public const string API_NMT = "NMT";
        public const string API_SMT = "SMT";
        public static NaverTranslateAPI instance;
        private string idKey;
        private string secretKey;

        private string transCode;
        private string resultCode;

        private string url;

        public void Init(string idKey, string secretKey, string apiType)
        {
            this.idKey = idKey;
            this.secretKey = secretKey;

            if(apiType == API_NMT)
            {
                url = "https://openapi.naver.com/v1/papago/n2mt";
            }
            else
            {
                url = "https://openapi.naver.com/v1/language/translate";
            }
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public string GetResult(string original)
        {
            try
            {

            }
            catch
            {

            }
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
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");


            request.AddHeader("X-Naver-Client-Id", idKey);
            request.AddHeader("X-Naver-Client-Secret", secretKey);
            request.AddParameter("application/x-www-form-urlencoded", "source=" + transCode + "&target=" + resultCode + "&text=" + original, ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            
            Dictionary<string, object> dic = deserial.Deserialize<Dictionary<string, object>>(response);

            if(dic.ContainsKey("errorMessage"))
            {
                result = (string)dic["errorMessage"];

                if(dic.ContainsKey("errorCode"))
                {
                    result += "\n Error Cdoe : " + (string)dic["errorCode"];
                }
                //result = "1";
            }
            
            else if(dic.ContainsKey("message"))
            {
                
                Dictionary<string, object> resultdic = (Dictionary < string, object>)dic["message"];
                result = "1";
                if (resultdic.ContainsKey("result"))
                {
                    Dictionary<string, object> transDic = (Dictionary<string, object>)resultdic["result"];
                    
                    if (transDic.ContainsKey("translatedText"))
                    {
                        //Dictionary<string, object> transDic2 = (Dictionary<string, object>)transDic["translatedText"];
                        result = (string)transDic["translatedText"];
                        result = result.Replace("<nl>", "\n");
                    }
                    
                    //result = (string)resultdic["translatedText"];
                }
                
            }

            return result;
        }

        

        
        
    }


}
