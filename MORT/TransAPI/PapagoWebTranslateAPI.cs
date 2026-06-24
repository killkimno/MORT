using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace MORT.TransAPI
{
    public class PapagoWebTranslateAPI
    {
        private static readonly string UrlBase = "https://papago.naver.com";
        private static readonly string UrlN2MT = "/api/text/translation";

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

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
            httpClient.DefaultRequestHeaders.Referrer = new Uri(UrlBase + "/");
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en");

            var url = UrlBase + UrlN2MT;

            var paramDic = new Dictionary<string, string>
            {
                { "dict", "false" },
                { "honorific", "false" },
                { "useGlossary", "false" },
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

        public void Init(string transCode, string resultCode)
        {
            _transCode = transCode;
            _resultCode = resultCode;
        }
    }
}
