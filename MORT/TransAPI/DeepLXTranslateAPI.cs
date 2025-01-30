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

            if(response == null || !response.IsSuccessful)
            {
                isError = true;
                return "error";
            }

            IDictionary<string, object> dic = (IDictionary<string, object>)SimpleJson.DeserializeObject(response.Content);

            //parse error
            //string errorCode = "0";
            //if (dic.ContainsKey("code"))
            //{
            //    string errorCodeObject = (string)dic["code"];
            //    if (errorCodeObject != null)
            //    {
            //        errorCode = errorCodeObject;
            //    }
            //}
            //
            //
            //if (!string.IsNullOrEmpty(errorCode) && !errorCode.Equals("0"))
            //{
            //    string errorResult = "error";
            //    if (dic.ContainsKey("errorMessage"))
            //    {
            //        string errorMessageObject = (string)dic["errorMessage"];
            //        if (errorMessageObject != null)
            //        {
            //            errorResult = errorMessageObject;
            //        }
            //    }
            //
            //    isError = true;
            //    return errorResult;
            //}

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
