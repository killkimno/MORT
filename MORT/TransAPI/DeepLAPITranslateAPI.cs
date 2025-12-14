using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static MORT.SettingManager;

namespace MORT.TransAPI
{
    public class DeepLAPITranslateAPI
    {
        private DeepLAPIEndpointType _endpointType;
        private string _url;
        private string _transCode;
        private string _resultCode;
        private string _apiKey;

        private struct ToTrans
        {
            public string[] text;
            public string target_lang;
            public string source_lang;
        }

        public void InitApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void Init(string transCode, string resultCode, DeepLAPIEndpointType endpointType)
        {
            if(transCode is "zh-CN" or "zh-TW" or "ZH-HANS" or "ZH-HANT")
            {
                transCode = "zh";
            }

            _endpointType = endpointType;
            switch (_endpointType)
            {
                case DeepLAPIEndpointType.Free:
                    _url = "https://api-free.deepl.com/v2/translate";
                    break;
                case DeepLAPIEndpointType.Paid:
                    _url = "https://api.deepl.com/v2/translate";
                    break;
            }
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
            request.AddHeader("Authorization", $"DeepL-Auth-Key {_apiKey}");
            request.AddHeader("content-type", "application/json"); //폼 형식
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");

            ToTrans toTrans = new ToTrans
            {
                text = [original],
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

            if (!response.IsSuccessful)
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

            if (dic.ContainsKey("translations"))
            {
                dic = (IDictionary<string, object>)((JsonArray)dic["translations"])[0];
            }

            //parse result
            if (dic.ContainsKey("text"))
            {
                var resultObject = dic["text"];
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
