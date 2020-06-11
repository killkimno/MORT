using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


namespace MORT
{

    class NaverTranslateAPI
    {
        public const string API_NMT = "NMT";
        public const string API_SMT = "SMT";    //2019 12 03 더이상 안 쓰임
        public static NaverTranslateAPI instance;
        private string idKey;
        private string secretKey;

        private string transCode;
        private string resultCode;

        private string apiType;
        private string url;

        public void Init(string idKey, string secretKey, string apiType)
        {
            this.apiType = apiType;
            this.idKey = idKey;
            this.secretKey = secretKey;

            if (apiType == API_NMT)
            {
                url = "https://openapi.naver.com/v1/papago/n2mt";
            }
            else
            {
                url = "https://openapi.naver.com/v1/papago/n2mt";
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
            //줄바꿈은 %0A 임
            string trim = original.Replace(" ", "");
            trim = trim.Replace(Environment.NewLine, "");
            Util.ShowLog(original + " / " + trim + "...");
            if (trim == "")
            {
                return "";
            }




            string result = "";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded"); //폼 형식
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");


            request.AddHeader("X-Naver-Client-Id", idKey);
            request.AddHeader("X-Naver-Client-Secret", secretKey);
            request.AddParameter("application/x-www-form-urlencoded", "source=" + transCode + "&target=" + resultCode + "&text=" + RestSharp.Extensions.StringExtensions.UrlEncode(original), ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            //RestSharp.Serialization.Json.JsonDeserializer deserial = new RestSharp.Serialization.Json.JsonDeserializer();
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();

            Dictionary<string, object> dic = deserial.Deserialize<Dictionary<string, object>>(response);
            /*
            if(!dic.ContainsKey("errorMessage"))
            dic.Add("errorMessage", "ddd");
            if (!dic.ContainsKey("errorCode"))
                dic.Add("errorCode", " ssss");
                */

            if (dic.ContainsKey("errorMessage"))
            {
                isError = true;
                result = (string)dic["errorMessage"];

                if (dic.ContainsKey("errorCode"))
                {
                    string error = (string)dic["errorCode"];
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
                        idKey = data.id;
                        secretKey = data.secret;
                        result += "\n[" + (TransManager.Instace.currentNaverIndex + 1).ToString() + "]번째 키를 활성화 합니다. ";
                    }
                }
                //result = "1";
            }

            else if (dic.ContainsKey("message"))
            {

                Dictionary<string, object> resultdic = (Dictionary<string, object>)dic["message"];
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
            //result += "\n" + TransManager.Instace.currentNaverIndex;
            return result;
        }
    }


}
