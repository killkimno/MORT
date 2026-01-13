using Google.GenAI.Types;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.TransAPI
{
    public enum GeminiErrorType
    {
        None,
        NormalError,
        ProhibitedContent
    }
    
    public class GeminiTranslatorAPI
    {
        private const string ApiEndpointBase = "https://generativelanguage.googleapis.com/v1beta/models/";

        private static readonly HttpClient httpClient = new HttpClient();
        private string _model;
        private string _command;
        private string _resultCommand;
        private string _apiKey;
        private string _customModel;

        private bool _disableDefaultCommand;
        private bool _useDefaultModel = true;
        private string _defaultCommand = string.Empty;
        private bool _inited = false;

        public void Initialize(string sourceCode, string resultCode)
        {
            _defaultCommand = $"- Task: Translate to {resultCode}.\n" +
                              $"- Constraint 1: Output ONLY the translation result. No explanation.\n" +
                              $"- Constraint 2: Keep ALL special characters, symbols, and punctuation exactly as they are in the original text.\n" +
                              $"- Constraint 3: Do not continue or add any text.";

            //_defaultCommand = $"- Translate to {resultCode}, Output ONLY the translation result. No explanation. Do not continue the story, Preserve all special characters and formatting.";
            _defaultCommand = $"Translate to {resultCode} (ONLY output result), strictly preserving all symbols, special characters.";
        }

        public void InitializeModel(string model, string apiKey, bool useDefaultModel)
        {
            _useDefaultModel = useDefaultModel;
            _model = model;
            _apiKey = apiKey;
        }

        public void InitializeCustom(string customModel, string command, bool disableDefaultCommand)
        {
            _customModel = customModel;
            _command = command;
            _disableDefaultCommand = disableDefaultCommand;
            _inited = false;
        }


        private async Task<(string Result, GeminiErrorType Error)> InternalTranslateTextAsync(string command, string requestText, string ocrText, bool saveResult, CancellationToken token)
        {
            string modelName = _useDefaultModel ? _model : _customModel;
            if (string.IsNullOrEmpty(modelName))
            {
                throw new InvalidOperationException("API 모델이 초기화되지 않았습니다. InitializeModel을 먼저 호출하세요.");
            }

            string apiEndpoint = $"{ApiEndpointBase}{modelName}:generateContent";

            var systemInstruction = new
            {
                parts = new[] { new { text = command } }
            };

            var safetySettingsParms = new[]
            {
                new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_CIVIC_INTEGRITY", threshold = "BLOCK_NONE" } // 선거/정치 관련 제한 완화
            };

            GenerationConfig generationConfig = null;

            object requestBody;
            if (modelName.Contains("pro", StringComparison.OrdinalIgnoreCase))
            {
                requestBody = new
                {
                    system_instruction = systemInstruction,

                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = requestText } } }
                    },

                    safetySettings = safetySettingsParms,

                    generationConfig = new
                    {
                        // pro 모델에서는 thinkingConfig를 제외
                        temperature = 0.2f // float 값으로 설정 (0.0f ~ 1.0f 사이)
                    }
                };
            }
            else
            {
                requestBody = new
                {
                    system_instruction = systemInstruction,
                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = requestText } } }
                    },

                    safetySettings = safetySettingsParms,

                    generationConfig = new
                    {
                        thinkingConfig = new { thinkingBudget = 0 },
                        temperature = 0.2f
                    }
                };
            }

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            string log = "";

            // API 호출
            using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(300)))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, timeoutCts.Token))
            {
                try
                {
                    log += "----------------------------" + System.Environment.NewLine;
                    log += $"Start Gemini - {ocrText}";
                    Logger.Logger.IncrementTrans();
                    HttpResponseMessage response = await httpClient.PostAsync($"{apiEndpoint}?key={_apiKey}", content, linkedCts.Token);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = doc.RootElement;
        
                        // promptFeedback 속성이 있는지 확인
                        if (root.TryGetProperty("promptFeedback", out JsonElement feedback))
                        {
                            // blockReason 속성이 있는지 확인
                            if (feedback.TryGetProperty("blockReason", out JsonElement reason))
                            {
                                string reasonValue = reason.GetString();
                                // 대소문자 구분 없이 "PROHIBITED_CONTENT"인지 확인
                                if (string.Equals(reasonValue, "PROHIBITED_CONTENT", StringComparison.OrdinalIgnoreCase))
                                {
                                    // 차단된 콘텐츠일 때의 처리 로직
                                    return(string.Empty, GeminiErrorType.ProhibitedContent);
                                }
                            }
                        }
                    }
                    
                    
                    dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                    string translatedText = responseObject?.candidates?[0]?.content?.parts?[0]?.text;
                    log += "----------------------------" + System.Environment.NewLine;

                    log += $"result - {translatedText}" + System.Environment.NewLine;

                    log += "=============================" + System.Environment.NewLine;

                    Logger.Logger.AddLog(log);
                    //TODO : 블락된 사유 처리가 필요함
                    return (translatedText?.Trim(), GeminiErrorType.None);
                }
                catch (OperationCanceledException)
                {
                    log += " / canceled";
                    Logger.Logger.AddLog(log);
                    if (timeoutCts.IsCancellationRequested)
                    {
                        return ("Timeout: 요청이 시간 초과되었습니다.", GeminiErrorType.NormalError);
                    }

                    if (token.IsCancellationRequested)
                    {
                        return ("", GeminiErrorType.NormalError);
                    }

                    return ("", GeminiErrorType.NormalError);
                }
                catch (HttpRequestException ex)
                {
                    log += " / canceled";
                    Logger.Logger.AddLog(log);
                    return ($"Error: 요청 중 오류가 발생했습니다. {ex.Message}", GeminiErrorType.NormalError);
                }
                catch (Exception ex)
                {
                    log += " / canceled";
                    Logger.Logger.AddLog(log);
                    return ($"Error: 예기치 않은 오류가 발생했습니다. {ex.Message}", GeminiErrorType.NormalError);
                }
            }
        }

        private void InitializeCommand()
        {
            _resultCommand = "";

            // 1. 커스텀 명령이 있다면 먼저 추가 (예: "- 무조건 경어로 번역해줘")
            if (!string.IsNullOrEmpty(_command))
            {
                _resultCommand += $"- {_command}";
            }

            // 2. 기본 명령을 추가해야 하는 경우
            // 조건: 커스텀 명령이 없거나 (useCommand = false)
            //       커스텀 명령이 있고 기본 명령을 비활성화하지 않은 경우
            bool useDefault = string.IsNullOrEmpty(_command) || (!_disableDefaultCommand);

            if (useDefault)
            {
                // 3. 커스텀 명령이 이미 있을 경우에만 공백(' ') 하나를 삽입하여 토큰 낭비 최소화
                if (!string.IsNullOrEmpty(_resultCommand))
                {
                    _resultCommand += " ";
                }

                // 4. 기본 명령 추가
                _resultCommand += $"\n{_defaultCommand}";
            }

            _inited = true;
        }

        private string CombineTextOptimized(string text)
        {
            string commandPrefix = !string.IsNullOrEmpty(_command) && _disableDefaultCommand ? _command : _resultCommand;
            return $"{commandPrefix}\nInput: {text}";
        }


        public async Task<(string Result, GeminiErrorType Error)> TranslateTextAsync(string text, CancellationToken token)
        {
            if (!_inited)
            {
                InitializeCommand();
            }

            string command = CombineTextOptimized(text);
            var result = await InternalTranslateTextAsync(_resultCommand, text, text, false, token);
            return result;
        }
    }
}