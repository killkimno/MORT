using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace MORT.TransAPI
{
    public class PapagoWebTranslateAPI
    {
        private static readonly string UrlBase = "https://papago.naver.com";
        private static readonly string UrlN2MT = "/apis/n2mt/translate"; // Neural Machine Translation
        private static readonly string FormUrlEncodedTemplate = "deviceId={0}&locale=en&dict=false&honorific=false&instant=true&source={1}&target={2}&text={3}";

        private static readonly Guid UUID = Guid.NewGuid();

        private static readonly Regex patternSource = new Regex(@"/vendors~main[^""]+", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex patternVersion = new Regex(@"v\d\.\d\.\d_[^""]+", RegexOptions.Compiled | RegexOptions.Singleline);

        private double _nextUpdate = 0;
        private string _version; // HMAC 키

        private string _transCode;
        private string _resultCode;

        private DateTime _dtNextAvailableTime = DateTime.MinValue;
        private Random _rand = new Random();

        public class TestPA
        {
            public string TranslatedText { get; set; }
        }
               
        public async Task<(string Result, bool IsError)> TranslateAsync(string text)
        {
            while (DateTime.Now < _dtNextAvailableTime)
            {
                await Task.Delay(50);
                Console.WriteLine("Wait : " + _dtNextAvailableTime.ToString());
            }

            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

            // 현재 유닉스 밀리 초
            var timestamp = Math.Truncate(DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds);

            // 버전이 없거나 마지막으로 버전 확인하고 10분 지났다면 업데이트하기
            if (_nextUpdate == 0 || _nextUpdate <= timestamp)
            {
                _nextUpdate = timestamp + (60 * 10 * 1000);
                await updateVersion(httpClient);
            }

            var url = UrlBase + UrlN2MT;
            var param = string.Format(FormUrlEncodedTemplate, UUID, _transCode, _resultCode, text);

            // 버전을 키로 사용한 HmacMD5 으로 인증 토큰 만들기
            var key = Encoding.UTF8.GetBytes(_version);
            var data = Encoding.UTF8.GetBytes($"{UUID}\n{url}\n{timestamp}");
            var token = Convert.ToBase64String(new HMACMD5(key).ComputeHash(data));

            // 최소 헤더 보내기
            httpClient.DefaultRequestHeaders.Add("Authorization", $"PPG {UUID}:{token}");
            httpClient.DefaultRequestHeaders.Add("Timestamp", timestamp.ToString());

            var paramDic = new Dictionary<string, string>
            {
                {"Content-Type", "application/x-www-form-urlencoded; charset=UTF-8" },
                { "deviceId", UUID.ToString() },
                { "locale", "en" },
                { "dict", "false" },
                { "honorific", "false" },
                { "instant", "true" },
                { "source", _transCode },
                { "target", _resultCode },
                { "text", text }
            };

            var encodedContent = new FormUrlEncodedContent(paramDic);

            var response = await httpClient.PostAsync(url, encodedContent);

            //랜덤 딜레이를 준다
            double random = _rand.NextDouble();
            _dtNextAvailableTime = DateTime.Now.AddMilliseconds(random * 650);

            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK) 
            {
                string errorMessage = "Error - null";
                if (response != null)
                {
                    errorMessage = $"Error - {response.StatusCode}";
                }

                return (errorMessage, true);
            }

            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            var responseText = streamReader.ReadToEnd();
            string result = "error";
            try
            {
                var pa = JsonConvert.DeserializeObject<TestPA> (responseText);

                result = pa.TranslatedText;
            }
            catch (JsonReaderException)
            {
                return ("Invalid JSON", true);
            }
            catch (Exception ex)
            {
                return (ex.Message, true);
            }


            return (result, false);
        }

        private async Task updateVersion(System.Net.Http.HttpClient wc)
        {
            // 메인 페이지에서 버전 불러올 수 있는 자바스크립트 주소 찾아오기
            var main = await wc.GetStringAsync(UrlBase);
            var mainMatch = patternSource.Match(main);

            // 자바스크립트 속에서 현재 파파고 버전 확인하기
            var script = await wc.GetStringAsync(UrlBase + mainMatch.Value);
            var scriptMatch = patternVersion.Match(script);

            _version = scriptMatch.Value;
        }

        public void Init(string transCode, string resultCode)
        {
            _transCode = transCode;
            _resultCode = resultCode;
        }
    }
}
