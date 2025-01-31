using RestSharp;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using static MORT.SettingManager;

namespace MORT.TransAPI
{
    public class DeepLXTranslateAPI
    {
        private DeepLXEndpointType _endpointType;
        private string _url;
        private string _transCode;
        private string _resultCode;
        private string _dl_session;

        private struct ToTrans
        {
            public string text;
            public string target_lang;
            public string source_lang;
        }

        private struct ToTransV2
        {
            public string[] text;
            public string target_lang;
            public string source_lang;
        }

        public void Init(string transCode, string resultCode, DeepLXEndpointType endpointType, string url, string dl_session)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "http://localhost:1188";
            }
            _endpointType = endpointType;
            _dl_session = dl_session;
            string urlEndpoint = "/translate";
            switch (_endpointType)
            {
                case DeepLXEndpointType.Free:
                    urlEndpoint = "/translate";
                    break;
                case DeepLXEndpointType.Paid:
                    urlEndpoint = "/v1/translate";
                    break;
                case DeepLXEndpointType.Official:
                    urlEndpoint = "/v2/translate";
                    break;
            }
            if (url.EndsWith('/'))
            {
                url.Remove(url.Length - 1);
            }
            _url = $"{url}{urlEndpoint}";
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
            if(!string.IsNullOrEmpty(_dl_session))
            {
                request.AddCookie("dl_session", _dl_session);
            }
            
            //Here generate a object of the JSON which  sent to server
            switch (_endpointType)
            {
                case DeepLXEndpointType.Free:
                    ToTrans toTrans = new ToTrans
                    {
                        text = original,
                        target_lang = _resultCode,
                        source_lang = _transCode
                    };
                    request.AddJsonBody(toTrans);
                    break;
                case DeepLXEndpointType.Paid:
                    ToTransV2 toTransPaid = new ToTransV2
                    {
                        text = new string[] { original },
                        target_lang = _resultCode,
                        source_lang = _transCode
                    };
                    request.AddJsonBody(toTransPaid);
                    break;
                case DeepLXEndpointType.Official:
                    ToTransV2 toTransV2 = new ToTransV2
                    {
                        text = new string[] { original },
                        target_lang = _resultCode,
                        source_lang = _transCode
                    };
                    request.AddJsonBody(toTransV2);
                    break;
            }

            


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
