using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
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
            public string[] text { get; set; }
            public string target_lang { get; set; }
            public string source_lang { get; set; }
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
            var request = new RestRequest("", Method.Post);
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
            request.AddParameter("application/json", JsonSerializer.Serialize(toTrans), ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            using JsonDocument doc = JsonDocument.Parse(response.Content ?? "{}");
            JsonElement root = doc.RootElement;

            if (!response.IsSuccessful)
            {
                string errorResult = "error";
                if(root.TryGetProperty("message", out JsonElement messageElement))
                {
                    string errorMessageObject = messageElement.GetString() ?? string.Empty;
                    if(errorMessageObject != null)
                    {
                        errorResult = errorMessageObject;
                    }
                }

                isError = true;
                return errorResult;
            }

            if(root.TryGetProperty("translations", out JsonElement translationsElement)
                && translationsElement.ValueKind == JsonValueKind.Array
                && translationsElement.GetArrayLength() > 0)
            {
                root = translationsElement[0];
            }

            //parse result
            if(root.TryGetProperty("text", out JsonElement textElement))
            {
                if(textElement.ValueKind == JsonValueKind.Array)
                {
                    foreach(var item in textElement.EnumerateArray())
                    {
                        result += item.GetString() ?? string.Empty;
                    }
                }
                else
                {
                    result = textElement.GetString() ?? string.Empty;
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
