using MORT.Model.CustomApi;
using MORT.Service.CustomApi;
using RestSharp;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Windows.Media.Ocr;

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


        public string GetResultTest2(string original, ref bool isError)
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


                var preset = new CustomApiModel("","","","","");
                var presetValue = preset;

                string safeOcrText = System.Web.HttpUtility.JavaScriptStringEncode(original);
                string finalJson = presetValue.Request.Replace("{OCR_TEXT}", safeOcrText).Replace("{SOURCE_CODE}", _transCode).Replace("{RESULT_CODE}", _resultCode);

                // 3. Ollama 전용 JSON 바디 생성
                // TranslateGemma 모델 지시어를 포함한 프롬프트 구성
                var requestBody = new
                {
                    model = "translategemma",
                    prompt = $"You are a professional {_transCode} ({_resultCode}) to {_resultCode} ({_resultCode}) translator. Your goal is to accurately convey the meaning and nuances of the original {_transCode} text while adhering to {{TARGET_LANG}} grammar, vocabulary, and cultural sensitivities.\r\nProduce only the {_resultCode} translation, without any additional explanations or commentary. Please translate the following {_transCode} text into {_resultCode}:\r\n\r\n{original}",
                    stream = false // 결과를 한 번에 받기 위해 false 설정
                };

                request.AddJsonBody(finalJson);

                // 4. 요청 실행
                IRestResponse response = client.Execute(request);

                if(response == null || !response.IsSuccessful)
                {
                    isError = true;
                    return "Ollama 연결 실패";
                }

                string resultToken = "{RESULT_TEXT}";

                // ExtractValue를 호출하여 템플릿 구조 내 토큰 위치의 실제 값을 가져옵니다.
                string extractedResult = ExtractValue(response.Content, presetValue.Response, resultToken);

                if(extractedResult != null && !extractedResult.StartsWith("Error:"))
                {
                    return extractedResult.Trim();
                }
                else
                {
                    isError = true;
                    return extractedResult ?? "결과를 찾을 수 없습니다.";
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


        public string ExtractValue(string realJson, string templateJson, string resultToken)
        {
            try
            {
                // 1. 템플릿에서 토큰 바로 앞에 있는 키 값을 찾습니다.
                // 예: "ResultText" : {RESULT_TEXT} -> "ResultText" 추출
                string pattern = $"\"([^\"]+)\"\\s*:\\s*{Regex.Escape(resultToken)}";
                var match = Regex.Match(templateJson, pattern);

                if(!match.Success)
                {
                    // 쌍따옴표가 있는 경우도 한번 더 체크 ("ResultText" : "{RESULT_TEXT}")
                    pattern = $"\"([^\"]+)\"\\s*:\\s*\"{Regex.Escape(resultToken)}\"";
                    match = Regex.Match(templateJson, pattern);
                }

                if(match.Success)
                {
                    string keyName = match.Groups[1].Value;

                    // 2. 실제 JSON에서 해당 키의 값을 추출합니다.
                    using(JsonDocument doc = JsonDocument.Parse(realJson))
                    {
                        // 최하단 필드부터 재귀적으로 탐색하여 해당 키의 값을 반환
                        return FindValueByKey(doc.RootElement, keyName);
                    }
                }

                return "Error: 템플릿에서 토큰의 키를 찾을 수 없습니다.";
            }
            catch(Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // 실제 JSON 내부를 돌며 특정 키 이름을 가진 값을 찾는 함수
        private string FindValueByKey(JsonElement element, string keyName)
        {
            if(element.ValueKind == JsonValueKind.Object)
            {
                foreach(var property in element.EnumerateObject())
                {
                    if(property.Name == keyName)
                    {
                        return property.Value.ToString();
                    }

                    string found = FindValueByKey(property.Value, keyName);
                    if(found != null) return found;
                }
            }
            else if(element.ValueKind == JsonValueKind.Array)
            {
                foreach(var item in element.EnumerateArray())
                {
                    string found = FindValueByKey(item, keyName);
                    if(found != null) return found;
                }
            }
            return null;
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
