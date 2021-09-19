using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MORT
{
    class GoogleBasicTranslateAPI
    {
        public static GoogleBasicTranslateAPI instance;

        private string _transCode;
        private string _resultCode;

        private bool _isAllowExecutive;

        public void SetTransCode(string transCode, string resultCode)
        {
            this._transCode = transCode;
            this._resultCode = resultCode;

            if (transCode != "ja")
            {
                _isAllowExecutive = true;
            }
            else
            {
                _isAllowExecutive = false;
            }
        }

        public string GetResult(string original, ref bool isError, string transCode , string resultCode )
        {
            RestSharp.Serialization.Json.JsonDeserializer deserial = new RestSharp.Serialization.Json.JsonDeserializer();
            //RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            if ( string.IsNullOrWhiteSpace(original))
            {
                Util.ShowLog("Empty");
                return "";
            }

            Util.ShowLog("Original : " + original+ System.Environment.NewLine + "Result : " + (RestSharp.Extensions.StringExtensions.UrlEncode(original)));
            string result = "";


            var client = new RestClient("https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + transCode + "&tl=" + resultCode + "&dt=t&q=" + RestSharp.Extensions.StringExtensions.UrlEncode(original));
            var request = new RestRequest(Method.GET);


            request.AddHeader("content-type", "application/x-www-form-urlencoded");  //이건 폼형식.
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");


            try
            {
               
                IRestResponse response = client.Execute(request);

                // RestSharp.Serialization.Json.JsonDeserializer deserial = new RestSharp.Serialization.Json.JsonDeserializer();
               
                if(response != null)
                {
                    if((int)response.StatusCode == 429)
                    {
                        isError = true;
                        return "시간당 사용할 수 있는 쿼리 모두 소모 - 다른 번역 방법을 선택하거나, 최대 한 시간 뒤에 다시 사용해 주세요";
                    }
                    else
                    {
                        string re = deserial.Deserialize<string>(response);
                        var obj = deserial.Deserialize<List<List<List<string>>>>(response);

                        Util.ShowLog(re);
                        Util.ShowLog(obj.Count + " @@@@");
                        if (obj.Count >= 1)
                        {
                            for (int i = 0; i < obj[0].Count; i++)
                            {

                                if (obj[0][i] != null)
                                    result += obj[0][i][0] + " ";
                            }
                        }
                    }
                }

        
            }
            catch (Exception e)
            {
                isError = true;
                return "처리하는 도중 오류가 발생했습니다" + System.Environment.NewLine + e.Message;
            }

            return result;
        }

        public string DoTrans(string original, ref bool isError)
        {
            string result = "";

            if(_isAllowExecutive && AdvencedOptionManager.IsExecutive)
            {
                original = GetResult(original, ref isError, _transCode, "ja");
                result = original;

                if(!isError)
                {
                    result = GetResult(original, ref isError, "ja", _resultCode);
                }
            }
            else
            {
                result = GetResult(original, ref isError, _transCode, _resultCode);
            }


            return result;
        }
    }
}
