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
        public static NaverTranslateAPI instance;
        private string idKey;
        private string secretKey;

        private string transCode;
        private string resultCode;

        public void Init(string idKey, string secretKey)
        {
            this.idKey = idKey;
            this.secretKey = secretKey;
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public string GetResult(string original)
        {
            string result = "";
            var client = new RestClient(" https://openapi.naver.com/v1/language/translate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");


            request.AddHeader("X-Naver-Client-Id", idKey);
            request.AddHeader("X-Naver-Client-Secret", secretKey);
            request.AddParameter("application/x-www-form-urlencoded", "source=" + transCode + "&target=" + resultCode + "&text=" + original , ParameterType.RequestBody);


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
                    }
                    
                    //result = (string)resultdic["translatedText"];
                }
                
            }


            /*
            var JSONObj = deserial.Deserialize<Dictionary<string, Dictionary<string, object>>>(response);
            try
            {
                Dictionary<string, object> resultDic = (Dictionary<string, object>)JSONObj["message"]["result"];

                result = resultDic["translatedText"].ToString();
            }
            catch
            {
                result = "error";
            }
            
            */
            //result = deserial.Deserialize <string>( response);
            return result;
        }

        

        
        
    }


}
