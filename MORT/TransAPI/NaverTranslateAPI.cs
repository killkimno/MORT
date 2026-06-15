using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;


namespace MORT
{

    class NaverTranslateAPI
    {
        public const string API_NMT = "NMT";
        public const string API_SMT = "SMT";    //2019 12 03 더이상 안 쓰임
        public static NaverTranslateAPI instance;
        private string idKey;
        private string secretKey;
        private bool isPaid = false;

        private string transCode;
        private string resultCode;

        private string apiType;
        private string url;

        public void Init(string idKey, string secretKey, string apiType, bool isPaid = true)
        {
            this.apiType = apiType;
            this.idKey = idKey;
            this.secretKey = secretKey;
            this.isPaid = isPaid;

            url = "https://papago.apigw.ntruss.com/nmt/v1/translation";
        }

        private void Init(TransManager.NaverKeyData data)
        {
            Init(data.id, data.secret, MORT.NaverTranslateAPI.API_NMT.ToString(), data.isPaid);
        }

        /// <summary>
        /// 같은 id Key일때만 변경한다.
        /// </summary>
        /// <param name="idKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="apiType"></param>
        /// <param name="isPaid"></param>
        public void ChangeValue(string idKey, string secretKey, bool isPaid = false)
        {
            if(this.idKey == idKey)
            {
                this.isPaid = isPaid;
                this.secretKey = secretKey;

                url = "https://naveropenapi.apigw.ntruss.com/nmt/v1/translation";
            }
        }

        public string GetAPIType()
        {
            return apiType;
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

        public string GetResult(string original, ref bool isError)
        {
            /*
            //TEST
            if(!isPaid)
            {
                TransManager.Instace.SetState(TransManager.NaverKeyData.eState.Error);
                if (TransManager.Instace.naverKeyList.Count > 1)
                {
                    TransManager.NaverKeyData data = TransManager.Instace.GetNextNaverKey();
                    Init(data);
                    return "\n[" + (TransManager.Instace.currentNaverIndex + 1).ToString() + "]번째 키를 활성화 합니다. ";
                }
            }
            */
            //줄바꿈은 %0A 임
            string trim = original.Replace(" ", "");
            trim = trim.Replace(Environment.NewLine, "");
            if (trim == "")
            {
                return "";
            }

            string result = "";
            var client = new RestClient(url);
            var request = new RestRequest("", Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded"); //폼 형식
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");

            request.AddHeader("X-NCP-APIGW-API-KEY-ID", idKey);
            request.AddHeader("X-NCP-APIGW-API-KEY", secretKey);

            request.AddParameter("application/x-www-form-urlencoded", "source=" + transCode + "&target=" + resultCode + "&text=" + Uri.EscapeDataString(original), ParameterType.RequestBody);


            RestResponse response = client.Execute(request);
            using JsonDocument doc = JsonDocument.Parse(response.Content ?? "{}");
            JsonElement root = doc.RootElement;

            //Util.ShowLog(re);

            /*
            if(!dic.ContainsKey("errorMessage"))
            dic.Add("errorMessage", "ddd");
            if (!dic.ContainsKey("errorCode"))
                dic.Add("errorCode", " ssss");
                */

            //무료 API 에러
            if (root.TryGetProperty("errorMessage", out JsonElement errorMessageElement))
            {
                isError = true;
                result = GetJsonElementText(errorMessageElement);

                if (root.TryGetProperty("errorCode", out JsonElement errorCodeElement))
                {
                    string error = GetJsonElementText(errorCodeElement);
                    result += "\n Error Cdoe : " + error;

                    if (error == "010")
                    {
                        //초과
                        TransManager.Instace.SetState(TransManager.NaverKeyData.eState.Limit);
                    }
                    else if (error == "024")
                    {
                        //인증실패 사용할 수 없음
                        TransManager.Instace.SetState(TransManager.NaverKeyData.eState.Error);
                    }

                    if (TransManager.Instace.naverKeyList.Count > 1)
                    {
                        TransManager.NaverKeyData data = TransManager.Instace.GetNextNaverKey();
                        Init(data);
                        result += "\n[" + (TransManager.Instace.currentNaverIndex + 1).ToString() + "]번째 키를 활성화 합니다. ";
                    }
                }
                //result = "1";
            }
            //유료 API 에러
            else if(root.TryGetProperty("error", out JsonElement errorElement))
            {
                isError = true;
                result = errorElement.TryGetProperty("message", out JsonElement paidErrorMessageElement) ? GetJsonElementText(paidErrorMessageElement) : "error";

                if (errorElement.TryGetProperty("errorCode", out JsonElement paidErrorCodeElement))
                {
                    string error = GetJsonElementText(paidErrorCodeElement);
                    result += "\n Error Cdoe : " + error;

                    if (error == "010")
                    {
                        //초과
                        TransManager.Instace.SetState(TransManager.NaverKeyData.eState.Limit);
                    }
                    else if (error == "200")
                    {
                        //인증실패 사용할 수 없음
                        TransManager.Instace.SetState(TransManager.NaverKeyData.eState.Error);
                    }

                    if (TransManager.Instace.naverKeyList.Count > 1)
                    {
                        TransManager.NaverKeyData data = TransManager.Instace.GetNextNaverKey();
                        Init(data);
                        result += "\n[" + (TransManager.Instace.currentNaverIndex + 1).ToString() + "]번째 키를 활성화 합니다. ";
                    }
                }
            }

            else if (root.TryGetProperty("message", out JsonElement messageElement))
            {

                result = "1";
                if (messageElement.TryGetProperty("result", out JsonElement resultElement))
                {
                    if (resultElement.TryGetProperty("translatedText", out JsonElement translatedTextElement))
                    {
                        result = GetJsonElementText(translatedTextElement);
                    }

                    //result = (string)resultdic["translatedText"];
                }

            }
            //result += "\n" + TransManager.Instace.currentNaverIndex;
            return result;
        }

        private static string GetJsonElementText(JsonElement element)
        {
            return element.ValueKind == JsonValueKind.String ? element.GetString() ?? string.Empty : element.ToString();
        }
    }


}
