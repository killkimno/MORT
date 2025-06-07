using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        private bool _includeDefaultCommand;
        private bool _useDefaultModel = true;
        private string _defaultCommand = string.Empty;
        private bool _inited = false;
        private List<object> conversationHistory = new List<object>();

        public void Initialize(string sourceCode, string resultCode)
        {
            _sourceCode = sourceCode;
            _resultCode = resultCode;
            _defaultCommand = System.Environment.NewLine + $" Translate to {_resultCode} and output only the result, Please keep all special characters from the original text ";
            _inited = false;
            conversationHistory.Clear();
            //_defaultCommand = $"- {_sourceCode} -> {_resultCode} result only";
        }

        public void InitializeModel(string model, string apiKey, bool useDefaultModel)
        {
            _useDefaultModel = useDefaultModel;
            _model = model;
            _apiKey = apiKey;
        }

        public void InitializeCustom(string customModel, string command, bool includeDefaultCommand)
        {
            _customModel = customModel;
            _command = command;
            _includeDefaultCommand = includeDefaultCommand;
            _inited = false;
            conversationHistory.Clear();

        }


        private async Task<string> InternalTranslateTextAsync(string text, bool saveResult)
        {
            string modelName = _useDefaultModel ? _model : _customModel;
            if(string.IsNullOrEmpty(modelName))
            {
                throw new InvalidOperationException("API 모델이 초기화되지 않았습니다. InitializeModel을 먼저 호출하세요.");
            }
            string apiEndpoint = $"{ApiEndpointBase}{modelName}:generateContent";

            var history = conversationHistory.ToList();
            // 현재 번역할 텍스트를 conversationHistory에 추가 (새로운 사용자 입력)
            history.Add(new { role = "user", parts = new[] { new { text = text } } });

            if(saveResult)
            {
                conversationHistory.Add(new { role = "user", parts = new[] { new { text = text } } });
            }

            // API 요청 본문 생성: 모든 conversationHistory를 포함
            var requestBody = new
            {
                contents = history
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // API 호출
            HttpResponseMessage response = await httpClient.PostAsync($"{apiEndpoint}?key={_apiKey}", content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
            string translatedText = responseObject?.candidates?[0]?.content?.parts?[0]?.text;

            // 모델의 번역 결과 응답을 conversationHistory에 추가
            if(saveResult && !string.IsNullOrEmpty(translatedText))
            {
                conversationHistory.Add(new { role = "model", parts = new[] { new { text = translatedText } } });
            }

            return translatedText?.Trim();
        }

        private async Task InitializeCommandAsync()
        {
            bool useCommand = false;
            if(!string.IsNullOrEmpty(_command))
            {
                await InternalTranslateTextAsync($"- {_command}", true);
                useCommand = true;
            }

            if(useCommand && !_includeDefaultCommand)
            {
                await InternalTranslateTextAsync($"- {_defaultCommand}", true);
                await InternalTranslateTextAsync($"start work", true);
            }
           
         
            _inited = true;
        }

        public async Task<string> TranslateTextAsync(string text)
        {         
            if(!_inited)
            {
                await InitializeCommandAsync();
                _inited = true;
            }

            string result = await InternalTranslateTextAsync(text, false);
            return result;
        }
    }
}
