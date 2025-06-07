using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MORT.TransAPI
{
    public class GeminiTranslatorAPI
    {
        private const string ApiEndpointBase = "https://generativelanguage.googleapis.com/v1beta/models/";

        // HttpClient 인스턴스를 static으로 선언하여 재사용
        private static readonly HttpClient httpClient = new HttpClient();
        private string _sourceCode;
        private string _resultCode;
        private string _model;
        private string _command;
        private string _apiKey;
        private string _customModel;

        private bool _useDefaultModel = true;

        public void Initialize(string sourceCode, string resultCode)
        {
            _sourceCode = sourceCode;
            _resultCode = resultCode;
        }

        public void InitializeModel(string model, string apiKey, bool useDefaultModel)
        {
            _useDefaultModel = useDefaultModel;
            _model = model;
            _apiKey = apiKey;
        }

        public void InitializeCustom(string customModel, string command)
        {
            _customModel = customModel;
            _command = command;

            _command += System.Environment.NewLine + $" 번역만 출력한다 {_sourceCode} to {_resultCode}:";

        }

        public async Task<string> TranslateTextAsync(string text)
        {
            string modelName =  _useDefaultModel ? _model : _customModel;
            string apiEndpoint = $"{ApiEndpointBase}{modelName}:generateContent";

            // HttpClient 초기화는 생성자나 정적 생성자에서 한 번만 수행
            // httpClient.DefaultRequestHeaders.Add("x-goog-api-key", ApiKey); // 이 부분은 한 번만 설정
            // API 키는 각 요청에 쿼리 파라미터로 추가하는 것이 더 유연할 수 있습니다.
            // 또는 HttpClientFactory 사용을 고려합니다.

            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new object[]
                    {
                        new { text = $"{_command}: {text}" }
                    }
                }
            }
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // API 키를 쿼리 파라미터로 추가하는 방식
            HttpResponseMessage response = await httpClient.PostAsync($"{apiEndpoint}?key={_apiKey}", content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
            string translatedText = responseObject?.candidates?[0]?.content?.parts?[0]?.text;
            return translatedText?.Trim();
        }
    }
}
