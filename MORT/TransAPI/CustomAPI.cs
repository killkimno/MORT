using RestSharp;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;

namespace MORT.TransAPI
{
    public class CustomAPI
    {
        private string _url;
        private string _transCode;
        private string _resultCode;

        private struct ToTrans
        {
            public string name;
            public string text;
            public string target;
            public string source;
        }

        public void Init(string url, string transCode, string resultCode)
        {
            _url = url; //example http://127.0.0.1:16888/translater
            _transCode = transCode;
            _resultCode = resultCode;
        }

        public string GetResultTest(string original, ref bool isError)
        {
            // 1. 공백 및 줄바꿈 체크
            string trim = original.Replace(" ", "").Replace(Environment.NewLine, "");
            if(string.IsNullOrEmpty(trim))
            {
                return "";
            }

            try
            {
                // 2. RestClient 설정 (Ollama 로컬 주소)
                // _url은 http://localhost:11434/api/generate 여야 합니다.
                var client = new RestClient("http://localhost:11434/api/generate");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                // 3. Ollama 전용 JSON 바디 생성
                // TranslateGemma 모델 지시어를 포함한 프롬프트 구성
                var requestBody = new
                {
                    model = "translategemma",
                    prompt = $"You are a professional {_transCode} ({_resultCode}) to {_resultCode} ({_resultCode}) translator. Your goal is to accurately convey the meaning and nuances of the original {_transCode} text while adhering to {{TARGET_LANG}} grammar, vocabulary, and cultural sensitivities.\r\nProduce only the {_resultCode} translation, without any additional explanations or commentary. Please translate the following {_transCode} text into {_resultCode}:\r\n\r\n{original}",
                    stream = false // 결과를 한 번에 받기 위해 false 설정
                };

                request.AddJsonBody(requestBody);

                // 4. 요청 실행
                IRestResponse response = client.Execute(request);

                if(response == null || !response.IsSuccessful)
                {
                    isError = true;
                    return "Ollama 연결 실패";
                }

                // 5. 결과 파싱 (Ollama는 결과가 'response' 키에 담겨 옴)
                IDictionary<string, object> dic = (IDictionary<string, object>)SimpleJson.DeserializeObject(response.Content);

                if(dic.ContainsKey("response"))
                {
                    string translatedText = dic["response"].ToString();
                    // 번역 결과만 리턴
                    return translatedText.Trim();
                }
                else if(dic.ContainsKey("error"))
                {
                    isError = true;
                    return dic["error"].ToString();
                }

                return "결과를 찾을 수 없습니다.";
            }
            catch(Exception ex)
            {
                isError = true;
                return ex.Message;
            }
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
                name = _transCode + _resultCode,
                text = original,
                target = _resultCode,
                source = _transCode
            };


            request.AddJsonBody(toTrans);


            IRestResponse response = client.Execute(request);

            if(response == null || !response.IsSuccessful)
            {
                isError = true;
                return "error";
            }

            IDictionary<string, object> dic = (IDictionary<string, object>)SimpleJson.DeserializeObject(response.Content);

            //result example

            /*
             the return can be like this JSON:
            {
            "text": "これは翻訳インタフェースです",//the source ext
            "from": "ja", //the source text type
            "to": "en", //the destination text type
            "errorMessage": "",
            "errorCode" : 0,
            "result": ["This is a translation interface"] 
            //the result is an array of string because the source text may contain many paragraphs, 
            the proccess will be slow, so the source text should be cutoff by nextline("/n"), every paragraph can be a member of the array.
            } 
            */

            //parse error
            string errorCode = "0";
            if (dic.ContainsKey("errorCode"))
            {
                string errorCodeObject = (string)dic["errorCode"];
                if (errorCodeObject != null)
                {
                    errorCode = errorCodeObject;
                }
            }


            if (!string.IsNullOrEmpty(errorCode) && !errorCode.Equals("0"))
            {
                string errorResult = "error";
                if (dic.ContainsKey("errorMessage"))
                {
                    string errorMessageObject = (string)dic["errorMessage"];
                    if (errorMessageObject != null)
                    {
                        errorResult = errorMessageObject;
                    }
                }

                isError = true;
                return errorResult;
            }

            //parse result
            if (dic.ContainsKey("result"))
            {
                var resultObject = dic["result"];
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
