using MORT.Model.CustomApi;
using MORT.Service.CustomApi;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        private CustomApiModel _preset;
        private readonly CustomApiPresetService _customApiPresetService;

        public CustomAPI(CustomApiPresetService customApiPresetService)
        {
            _url = "";
            _transCode = "";
            _resultCode = "";
            _customApiPresetService = customApiPresetService;
        }

        public void Init(string url, string transCode, string resultCode, string presetName)
        {
            _url = url; //example http://127.0.0.1:16888/translater
            _transCode = transCode;
            _resultCode = resultCode;

            _preset = _customApiPresetService.FindAdditionalByName(presetName) ?? _customApiPresetService.BuiltInList.First();
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
                    prompt = $"You are a professional {_transCode} ({_resultCode}) to {_resultCode} ({_resultCode}) translator. Your goal is to accurately convey the meaning and nuances of the original {_transCode} text while adhering to {{_resultCode}} grammar, vocabulary, and cultural sensitivities.\r\nProduce only the {_resultCode} translation, without any additional explanations or commentary. Please translate the following {_transCode} text into {_resultCode}:\r\n\r\n{original}",
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
            //return GetResultTest(original, ref isError);
            string trim = original.Replace(" ", "").Replace(Environment.NewLine, "");
            if (string.IsNullOrEmpty(trim))
            {
                return "";
            }

            try
            {
                var client = new RestClient("http://localhost:11434/api/generate");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                var preset = _preset;

                string safeOcrText = System.Web.HttpUtility.JavaScriptStringEncode(original);
                string finalJson = preset.Request
                    .Replace("{OCR_TEXT}", safeOcrText)
                    .Replace("{SOURCE_CODE}", _transCode)
                    .Replace("{RESULT_CODE}", _resultCode);

                // finalJson이 올바른 JSON인지 검사하고, 아니면 간단히 변환 시도
                try
                {
                    using (JsonDocument.Parse(finalJson)) { /* valid JSON */ }
                }
                catch (JsonException)
                {
                    // C# 스타일 템플릿(예: model = "x")을 JSON으로 바꿉니다.
                    finalJson = ConvertTemplateToJson(finalJson);

                    // 변환 후에도 JSON 파싱 검증
                    try
                    {
                        using (JsonDocument.Parse(finalJson)) { /* valid JSON now */ }
                    }
                    catch (JsonException ex)
                    {
                        isError = true;
                        return $"템플릿이 유효한 JSON으로 변환되지 않았습니다: {ex.Message}";
                    }
                }

                // RestSharp에 raw JSON 문자열로 본문 추가 (직렬화 중복 방지)
                // RestSharp 버전에 따라 AddStringBody 또는 AddParameter 사용
                request.AddParameter("application/json", finalJson, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                if (response == null || !response.IsSuccessful)
                {
                    isError = true;
                    return "Ollama 연결 실패";
                }

                string resultToken = "{RESULT_TEXT}";
                string extractedResult = ExtractValue(response.Content, preset.Response, resultToken);

                if (extractedResult != null && !extractedResult.StartsWith("Error:"))
                {
                    return extractedResult.Trim();
                }
                else
                {
                    isError = true;
                    return extractedResult ?? "결과를 찾을 수 없습니다.";
                }
            }
            catch (Exception ex)
            {
                isError = true;
                return ex.Message;
            }
        }

        private string ConvertTemplateToJson(string template)
        {
            if (string.IsNullOrWhiteSpace(template)) return template;

            // 1) 프로퍼티 이름 패턴 "name =" -> "\"name\":"
            string step1 = Regex.Replace(template, @"(\b\w+\b)\s*=", "\"$1\":");

            // 2) 가능하면 약간의 공백 정리 (선택)
            string step2 = Regex.Replace(step1, @"\s+,\s+", ", ");

            // 3) 앵커: 중괄호 내부가 이미 쌍따옴표로 감싸져 있는지 확인 후 반환
            return step2;
        }

        public string ExtractValue(string realJson, string templateJson, string resultToken)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(templateJson))
                    return "Error: 템플릿이 비어있습니다.";

                // 1) 템플릿 정리: 줄바꿈 제거 및 트림
                string normalized = templateJson.Replace("\r", "").Replace("\n", "").Trim();

                // 2) C# 스타일 "key = value" 또는 unquoted key 를 JSON 스타일로 변환
                //    패턴: "key" = ...   또는   key = ...
                normalized = Regex.Replace(
                    normalized,
                    @"(?:""(?<k>[^""]+)""|(?<k>\b[A-Za-z0-9_]+\b))\s*=\s*",
                    "\"${k}\":",
                    RegexOptions.Compiled);

                // 3) 토큰 직전의 키 찾기 (비인용 토큰 / 인용된 토큰 둘 다 검사)
                string patternUnquoted = $"\"([^\"]+)\"\\s*:\\s*{Regex.Escape(resultToken)}";
                var match = Regex.Match(normalized, patternUnquoted);

                if(!match.Success)
                {
                    string patternQuoted = $"\"([^\"]+)\"\\s*:\\s*\"{Regex.Escape(resultToken)}\"";
                    match = Regex.Match(normalized, patternQuoted);
                }

                if(!match.Success)
                {
                    return "Error: 템플릿에서 토큰의 키를 찾을 수 없습니다.";
                }

                string keyName = match.Groups[1].Value;

                // 4) 실제 JSON에서 해당 키의 값을 재귀적으로 탐색하여 반환
                using(JsonDocument doc = JsonDocument.Parse(realJson))
                {
                    return FindValueByKey(doc.RootElement, keyName);
                }
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
