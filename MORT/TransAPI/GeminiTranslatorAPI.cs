using Google.GenAI.Types;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.TransAPI
{
    public class GeminiTranslatorAPI
    {
        private const string ApiEndpointBase = "https://generativelanguage.googleapis.com/v1beta/models/";

        private static readonly HttpClient httpClient = new HttpClient();
        private string _sourceCode;
        private string _resultCode;
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
            _sourceCode = sourceCode;
            _resultCode = resultCode;
            _defaultCommand = $"- Translate to {resultCode}, keep special characters, and output only the translation.";
            //_defaultCommand = $"- {_sourceCode} -> {_resultCode} result only";
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


        private async Task<string> InternalTranslateTextAsync(string requestText, bool saveResult, CancellationToken token)
        {
            string modelName = _useDefaultModel ? _model : _customModel;
            if(string.IsNullOrEmpty(modelName))
            {
                throw new InvalidOperationException("API 모델이 초기화되지 않았습니다. InitializeModel을 먼저 호출하세요.");
            }
            string apiEndpoint = $"{ApiEndpointBase}{modelName}:generateContent";


           
            GenerationConfig generationConfig = null;

            object requestBody;
            if(modelName.Contains("pro", StringComparison.OrdinalIgnoreCase))
            {
                requestBody = new
                {
                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = requestText } } }
                    },

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
                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = requestText } } }
                    },

                    /*
                    generationConfig = new
                    {
                        //추론기능 - 0은 끈 상태
                        thinkingConfig = new
                        {
                            thinkingBudget = 0
                        },
                        temperature = 0.2f // float 값으로 설정 (0.0f ~ 1.0f 사이)
                    }
                    */

                    generationConfig = new
                    {
                        // 추론 기능 활성화: 웹에서 사용하는 모델의 기본 최대 추론 토큰 수 (8192)를 설정합니다.
                        thinkingConfig = new
                        {
                            thinkingBudget = 8192
                        },

                        // 추론에 최적화된 권장 온도(1.0f)를 설정합니다.
                        temperature = 1.0f,

                        // (선택 사항) 웹 대화의 유연하고 자연스러운 응답 생성을 위한 일반적인 기본값을 추가합니다.
                        topP = 0.95f,
                        topK = 40
                    }
                };
            }

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // API 호출
            using(var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(300)))
            using(var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, timeoutCts.Token))
            {
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync($"{apiEndpoint}?key={_apiKey}", content, linkedCts.Token);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                    string translatedText = responseObject?.candidates?[0]?.content?.parts?[0]?.text;

                    return translatedText?.Trim();
                }
                catch(OperationCanceledException)
                {
                    if(timeoutCts.IsCancellationRequested)
                    {
                        return "Timeout: 요청이 시간 초과되었습니다.";
                    }

                    if(token.IsCancellationRequested)
                    {
                        return "";
                    }

                    return "";
                }
                catch(HttpRequestException ex)
                {
                    return $"Error: 요청 중 오류가 발생했습니다. {ex.Message}";
                }
                catch(Exception ex)
                {
                    return $"Error: 예기치 않은 오류가 발생했습니다. {ex.Message}";
                }

            }
        }

        private void InitializeCommand()
        {
            _resultCommand = "";
            bool useCommand = false;
            if(!string.IsNullOrEmpty(_command))
            {
                _resultCommand += $"- {_command}";
                useCommand = true;
            }

            if(!useCommand || (useCommand && !_disableDefaultCommand))
            {
                if(useCommand)
                {
                    _resultCommand += System.Environment.NewLine;
                }
                _resultCommand += $"{_defaultCommand}";
            }


            _inited = true;
        }

        private string CombineText(string text)
        {
            if(!string.IsNullOrEmpty(_command) && _disableDefaultCommand)
            {
                return _command + System.Environment.NewLine + System.Environment.NewLine + text;
            }

            return "**Command:**" + System.Environment.NewLine + System.Environment.NewLine + _resultCommand + System.Environment.NewLine + System.Environment.NewLine + "**Text to Translate**" + System.Environment.NewLine + System.Environment.NewLine + text;
        }

        public async Task<string> TranslateTextAsync(string text, CancellationToken token)
        {
            if(!_inited)
            {
                InitializeCommand();
            }

            text = CombineText(text);
            string result = await InternalTranslateTextAsync(text, false, token);
            return result;
        }
    }
}
