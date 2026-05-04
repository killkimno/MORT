using MORT.Model.CustomApi;
using MORT.Service.CustomApi;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly CustomApiPresetService _customApiPresetService;
        private string _presetName;

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
            _presetName = presetName;
        }

        public string GetResult(string original, ref bool isError)
        {
            string trim = original.Replace(" ", "").Replace(Environment.NewLine, "");
            if (string.IsNullOrEmpty(trim))
            {
                return "";
            }
            var preset = _customApiPresetService.FindAdditionalByName(_presetName);

            if(preset == null)
            {
                return GetResultDefault(original, ref isError);
            }

            try
            {
                var client = new RestClient(preset.Url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                // 커스텀 헤더 적용
                if(preset.Headers != null)
                {
                    foreach(var header in preset.Headers)
                    {
                        var parts = header.Split(':', 2);
                        if(parts.Length == 2)
                        {
                            string name = parts[0].Trim();
                            string value = parts[1].Trim();
                            request.AddHeader(name, value);
                        }
                        else
                        {
                            Util.ShowLog($"[CustomAPI] 잘못된 헤더 형식: {header} (':'로 구분된 2개의 요소가 필요합니다)");
                        }
                    }
                }



                // 1) 먼저 템플릿에서 토큰을 치환 (아직 C# 형식)
                string safeOcrText =  System.Web.HttpUtility.JavaScriptStringEncode(original);
                string substitutedTemplate = preset.Request
                    .Replace("{OCR_TEXT}", safeOcrText)
                    .Replace("{SOURCE_CODE}", _transCode)
                    .Replace("{RESULT_CODE}", _resultCode);

                // JSON 형식이 아닌 경우 {}로 감싸기
                substitutedTemplate = substitutedTemplate.Trim();
                if(!substitutedTemplate.StartsWith("{") || !substitutedTemplate.EndsWith("}"))
                {
                    substitutedTemplate = "{" + substitutedTemplate + "}";
                }

                // 2) C# 객체 초기화 문법을 JSON으로 변환
                string finalJson = ConvertTemplateToJson(substitutedTemplate);

                // 3) JSON 유효성 검증
                try
                {
                    using (JsonDocument.Parse(finalJson)) { }
                }
                catch (JsonException ex)
                {
                    isError = true;
                    return $"JSON 변환 실패: {ex.Message}";
                }

                request.AddParameter("application/json", finalJson, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                if (response == null || !response.IsSuccessful)
                {
                    isError = true;
                    return response == null ? "response null" : response.Content;
                }

                string resultToken = "{RESULT_TEXT}";

                // Response 템플릿이 JSON 형식이 아닌 경우 {}로 감싸기
                string responseTemplate = preset.Response.Trim();
                if(!responseTemplate.StartsWith("{") || !responseTemplate.EndsWith("}"))
                {
                    responseTemplate = "{" + responseTemplate + "}";
                }

                string extractedResult = ExtractValue(response.Content, responseTemplate, resultToken);

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

            string content = template.Trim();

            // 이미 유효한 JSON이면 변환 없이 그대로 반환
            try
            {
                using (JsonDocument.Parse(content)) { }
                return content;
            }
            catch (JsonException) { /* JSON이 아닌 경우 아래 변환 로직 수행 */ }

            // 앞뒤 중괄호 제거
            if (content.StartsWith("{") && content.EndsWith("}"))
            {
                content = content.Substring(1, content.Length - 2);
            }

            // 프로퍼티 분할 (C# 스타일: key = value 또는 JSON 스타일: "key": value)
            var properties = SplitPropertiesByComa(content);

            var jsonParts = new List<string>();
            foreach (var prop in properties)
            {
                var (key, value) = ParseProperty(prop.Trim());
                if (!string.IsNullOrEmpty(key))
                {
                    jsonParts.Add($"\"{key}\": {value}");
                }
            }

            return "{" + string.Join(", ", jsonParts) + "}";
        }

        private List<string> SplitPropertiesByComa(string content)
        {
            var parts = new List<string>();
            var current = new StringBuilder();
            bool inString = false;
            bool inEscape = false;

            foreach (char c in content)
            {
                if (inEscape)
                {
                    current.Append(c);
                    inEscape = false;
                    continue;
                }

                if (c == '\\')
                {
                    current.Append(c);
                    inEscape = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                    current.Append(c);
                    continue;
                }

                if (c == ',' && !inString)
                {
                    if (current.Length > 0)
                    {
                        parts.Add(current.ToString());
                        current.Clear();
                    }
                    continue;
                }

                current.Append(c);
            }

            if (current.Length > 0)
            {
                parts.Add(current.ToString());
            }

            return parts;
        }

        private (string key, string value) ParseProperty(string property)
        {
            // JSON 형식 파싱: "key": value
            var jsonMatch = Regex.Match(property, @"^\s*""(?<key>[^""]+)""\s*:\s*(?<value>.+)$");
            if (jsonMatch.Success)
            {
                string jsonKey = jsonMatch.Groups["key"].Value.Trim();
                string jsonValue = jsonMatch.Groups["value"].Value.Trim();
                return (jsonKey, jsonValue);
            }

            // C# 형식 파싱: "key = value" 또는 key = value
            var match = Regex.Match(property, @"^\s*""?(?<key>\w+)""?\s*=\s*(?<value>.+)$");

            if (!match.Success)
            {
                return (null, null);
            }

            string key = match.Groups["key"].Value.Trim();
            string value = match.Groups["value"].Value.Trim();

            // value를 정규화
            // 배열인 경우 ([ 로 시작하고 ] 로 끝남)
            if (value.StartsWith("[") && value.EndsWith("]"))
            {
                // 배열 내부 요소들을 JSON 형식으로 변환
                string arrayContent = value.Substring(1, value.Length - 2).Trim();
                var arrayElements = SplitArrayElements(arrayContent);
                var jsonElements = new List<string>();

                foreach (var element in arrayElements)
                {
                    string trimmed = element.Trim();
                    jsonElements.Add(NormalizeValue(trimmed));
                }

                return (key, "[" + string.Join(", ", jsonElements) + "]");
            }
            else
            {
                return (key, NormalizeValue(value));
            }
        }

        private string NormalizeValue(string value)
        {
            value = value.Trim();

            // 이미 따옴표로 감싸져 있으면 유지
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return value;
            }
            // 불린 값
            else if (value.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                     value.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return value.ToLower();
            }
            // 숫자 값
            else if (int.TryParse(value, out _) || double.TryParse(value, out _))
            {
                return value;
            }
            // null 값
            else if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return "null";
            }
            // 문자열 - 따옴표로 감싸기
            else
            {
                return "\"" + value.Trim('"') + "\"";
            }
        }

        private List<string> SplitArrayElements(string content)
        {
            var elements = new List<string>();
            var current = new StringBuilder();
            bool inString = false;
            bool inEscape = false;
            int bracketDepth = 0;

            foreach (char c in content)
            {
                if (inEscape)
                {
                    current.Append(c);
                    inEscape = false;
                    continue;
                }

                if (c == '\\')
                {
                    current.Append(c);
                    inEscape = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                    current.Append(c);
                    continue;
                }

                if (!inString)
                {
                    if (c == '[' || c == '{')
                    {
                        bracketDepth++;
                    }
                    else if (c == ']' || c == '}')
                    {
                        bracketDepth--;
                    }
                    else if (c == ',' && bracketDepth == 0)
                    {
                        if (current.Length > 0)
                        {
                            elements.Add(current.ToString());
                            current.Clear();
                        }
                        continue;
                    }
                }

                current.Append(c);
            }

            if (current.Length > 0)
            {
                elements.Add(current.ToString());
            }

            return elements;
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



        private string GetResultDefault(string original, ref bool isError)
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
