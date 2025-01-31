using RestSharp;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;

namespace MORT.TransAPI
{
    public class DeepLXTranslateAPI
    {
        private string _url;
        private string _transCode;
        private string _resultCode;

        private struct ToTrans
        {
            public string text;
            public string target_lang;
            public string source_lang;
        }

        public void Init(string transCode, string resultCode)
        {
            _url = "http://localhost:1188/translate";
            _transCode = transCode;
            _resultCode = resultCode;
        }

        public string GetResult(string original, ref bool isError)
        {
            //줄바꿈은 %0A 임
            string trim = original.Replace(" ", "");
            trim = trim.Replace(Environment.NewLine, "");
            if (trim == "")
            {
                return "";
            }

            string result = "";
            var client = new RestClient(_url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json"); //폼 형식
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");

            //Here generate a object of the JSON which  sent to server
            ToTrans toTrans = new ToTrans
            {
                text = original,
                target_lang = _resultCode,
                source_lang = _transCode
            };

            request.AddJsonBody(toTrans);


            IRestResponse response = client.Execute(request);

            IDictionary<string, object> dic = (IDictionary<string, object>)SimpleJson.DeserializeObject(response.Content);

            //parse error
            long errorCode = 200;
            if (dic.ContainsKey("code"))
            {
                long errorCodeObject = (long)dic["code"];
                if (errorCodeObject != 200)
                {
                    errorCode = errorCodeObject;
                }
            }
            
            if (errorCode != 200)
            {
                string errorResult = "error";
                if (dic.ContainsKey("message"))
                {
                    string errorMessageObject = (string)dic["message"];
                    if (errorMessageObject != null)
                    {
                        errorResult = errorMessageObject;
                    }
                }
            
                isError = true;
                return errorResult;
            }

            //parse result
            if (dic.ContainsKey("data"))
            {
                var resultObject = dic["data"];
                if (resultObject is JsonArray)
                {
                    JsonArray resultarray = (JsonArray)resultObject;
                    for (int i = 0; i < resultarray.Count; i++)
                    {
                        result += (string)resultarray[i];
                    }
                }
                else
                {
                    result = (string)resultObject;
                }
               
            }
            else
            {
                result = "Empty result";
            }

            return result;
        }
    }
}
