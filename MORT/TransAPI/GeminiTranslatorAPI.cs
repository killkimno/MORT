using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using static System.Net.Mime.MediaTypeNames;

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


        private async Task<string> InternalTranslateTextAsync(string requestText, bool saveResult)
        {
            string modelName = _useDefaultModel ? _model : _customModel;
            if(string.IsNullOrEmpty(modelName))
            {
                throw new InvalidOperationException("API 모델이 초기화되지 않았습니다. InitializeModel을 먼저 호출하세요.");
            }
            string apiEndpoint = $"{ApiEndpointBase}{modelName}:generateContent";

            var requestBody = new
            {
                contents = new[]
                {
                    new { role = "user", parts = new[] { new { text = requestText } } }
                },
                
                // generationConfig 객체를 추가하여 모델 생성 설정을 합니다.
                generationConfig = new
                {
                    temperature = 0.2f // float 값으로 설정 (0.0f ~ 1.0f 사이)
                                       // 번역에는 보통 0.0f (가장 일관적) 또는 0.1f, 0.2f 와 같이 낮은 값을 권장
                }
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // API 호출
            using(var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync($"{apiEndpoint}?key={_apiKey}", content, cts.Token);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                    string translatedText = responseObject?.candidates?[0]?.content?.parts?[0]?.text;

                    return translatedText?.Trim();
                }
                catch(OperationCanceledException)
                {
                    return "Timeout: 요청이 시간 초과되었습니다.";
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

        public async Task<string> TranslateTextAsync(string text)
        {         
            if(!_inited)
            {
                InitializeCommand();
            }

            text = CombineText(text);
            string result = await InternalTranslateTextAsync(text, false);
            return result;
        }
    }
}
