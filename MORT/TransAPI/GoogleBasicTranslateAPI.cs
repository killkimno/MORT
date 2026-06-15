using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MORT
{
    public interface IGoogleBasicTranslateAPIContract
    {
        void UpdateCondition(string condition);
    }

    class GoogleBasicTranslateAPI
    {
        public static GoogleBasicTranslateAPI instance;

        private string _transCode;
        private string _resultCode;

        private bool _isAllowExecutive;

        private DateTime _dtNextCheck = DateTime.MinValue;
        private bool _lowQuailtyMode;
        private IGoogleBasicTranslateAPIContract _contract;

        public void InitContract(IGoogleBasicTranslateAPIContract contract)
        {
            _contract = contract;
        }

        public void UpdateCondition()
        {
            _contract.UpdateCondition(_lowQuailtyMode ? "Basic_LowQuality": "Basic_HighQuality");
        }

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

        private string GetResult(string original, ref bool isError, string transCode , string resultCode )
        {
            if ( string.IsNullOrWhiteSpace(original))
            {
                Util.ShowLog("Empty");
                return "";
            }

            string encodedOriginal = Uri.EscapeDataString(original);
            Util.ShowLog("Original : " + original+ System.Environment.NewLine + "Result : " + encodedOriginal);
            string result = "";

            //dict-chrome-ex , gtx , webapp
            //var client = new RestClient("https://translate.googleapis.com/translate_a/single?client=dict-chrome-ex&sl=" + transCode + "&tl=" + resultCode + "&dt=t&q=" + Uri.EscapeDataString(original));

            string clientType = _lowQuailtyMode ? "dict-chrome-ex" : "gtx";

            //string requestLog = $"https://translate.googleapis.com/translate_a/single?client={clientType}&sl={transCode}&tl={resultCode}&dt=t&q={encodedOriginal}";

            //Util.ShowLog($"Google Request : {requestLog}");

            var client = new RestClient($"https://translate.googleapis.com/translate_a/single?client={clientType}&sl={transCode}&tl={resultCode}&dt=t&q={encodedOriginal}");

            var request = new RestRequest("", Method.Get);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");  //이건 폼형식.
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("charset", "UTF-8");

            request.Timeout = TimeSpan.FromMilliseconds(2000);
            try
            {               
                RestResponse response = client.Execute(request);
               
                if(response != null)
                {
                    Util.ShowLog($"Result Status : Success = {response.IsSuccessful} StatusCode : {response.StatusCode}");
                    if((int)response.StatusCode == 429) 
                    {
                        isError = true;
                        if (_lowQuailtyMode)
                        {
                            return "시간당 사용할 수 있는 쿼리 모두 소모 - 다른 번역 방법을 선택하거나, 최대 한 시간 뒤에 다시 사용해 주세요";
                        }
                        else
                        {
                            //저품질로 다시 한 번 해본다
                            isError = false;
                            _dtNextCheck = DateTime.Now.AddHours(1);
                            _lowQuailtyMode = true;
                            UpdateCondition();
                            return GetResult(original, ref isError, transCode, resultCode);
                        }
                    }
                    else
                    {
                        string re = response.Content ?? string.Empty;

                        Util.ShowLog(re);
                        using JsonDocument doc = JsonDocument.Parse(re);
                        JsonElement translations = doc.RootElement[0];

                        Util.ShowLog(translations.GetArrayLength() + " @@@@");
                        foreach(var item in translations.EnumerateArray())
                        {
                            if(item.ValueKind == JsonValueKind.Array
                                && item.GetArrayLength() > 0
                                && item[0].ValueKind == JsonValueKind.String)
                            {
                                result += (item[0].GetString() ?? string.Empty) + " ";
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

            //저품질 모드인지 체크한다.
            if(_lowQuailtyMode && DateTime.Now > _dtNextCheck)
            {
                _lowQuailtyMode = false;
                UpdateCondition();
            }

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

            if(_lowQuailtyMode)
            {
                result = "[저품질]" + result;
            }

            return result;
        }
    }
}
