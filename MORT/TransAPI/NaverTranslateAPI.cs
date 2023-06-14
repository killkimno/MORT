using RestSharp;
using System;
using System.Collections.Generic;


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

        public void Init(string idKey, string secretKey, string apiType, bool isPaid = false)
        {
            this.apiType = apiType;
            this.idKey = idKey;
            this.secretKey = secretKey;
            this.isPaid = isPaid;
            if (isPaid)
            {
                url = "https://naveropenapi.apigw.ntruss.com/nmt/v1/translation";
               
            }
            else
            {
                url = "https://openapi.naver.com/v1/papago/n2mt";
            }
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
                if (isPaid)
                {
                    url = "https://naveropenapi.apigw.ntruss.com/nmt/v1/translation";

                }
                else
                {
                    url = "https://openapi.naver.com/v1/papago/n2mt";
                }
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
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded"); //폼 형식
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");

            if (!isPaid)
            {
                request.AddHeader("X-Naver-Client-Id", idKey);
                request.AddHeader("X-Naver-Client-Secret", secretKey);
            }
            else
            {
                request.AddHeader("X-NCP-APIGW-API-KEY-ID", idKey);
                request.AddHeader("X-NCP-APIGW-API-KEY", secretKey);
            }
       
            request.AddParameter("application/x-www-form-urlencoded", "source=" + transCode + "&target=" + resultCode + "&text=" + RestSharp.Extensions.StringExtensions.UrlEncode(original), ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            RestSharp.Serialization.Json.JsonDeserializer deserial = new RestSharp.Serialization.Json.JsonDeserializer();
            //RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();

            Dictionary<string, object> dic = deserial.Deserialize<Dictionary<string, object>>(response);


            string re = deserial.Deserialize<string>(response);

            Util.ShowLog(re);

            /*
            if(!dic.ContainsKey("errorMessage"))
            dic.Add("errorMessage", "ddd");
            if (!dic.ContainsKey("errorCode"))
                dic.Add("errorCode", " ssss");
                */

            //무료 API 에러
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
                        Init(data);
                        result += "\n[" + (TransManager.Instace.currentNaverIndex + 1).ToString() + "]번째 키를 활성화 합니다. ";
                    }
                }
                //result = "1";
            }
            //유료 API 에러
            else if(dic.ContainsKey("error"))
            {
                Dictionary<string, object> errorDic = (Dictionary<string, object>)dic["error"];
                isError = true;
                result = (string)errorDic["message"];

                if (errorDic.ContainsKey("errorCode"))
                {
                    string error = (string)errorDic["errorCode"];
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
