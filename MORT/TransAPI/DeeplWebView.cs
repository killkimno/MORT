using Microsoft.Web.WebView2.WinForms;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.TransAPI
{
    public partial class DeeplWebView : Form
    {
        public WebView2 _webView;
        private bool _start = false;

        private const string _suffix = "^^^^";
        private string _lastResult = "\"\\r\\n\"";
        private string _defaultKey = "\"\\r\\n\"";
        private DateTime _dtTimeout;
        private DateTime _dtRetryTimeOut;
        private IDeeplAPIContract _contract;

        private string _urlFormat = "https://www.deepl.com/translator#{0}/{1}/{2}";

        private Random _rand = new Random();

        public string LastResult => _lastResult;

        public bool Complete { get; set; }
        public bool IsError { get; private set; }
        private bool _initComplete;
        /// <summary>
        /// 마지막으로 입력된 값
        /// </summary>
        private string _lastUrl;
        private DateTime _dtNextAvailableTime = DateTime.MinValue;
        private const int RetryTimeoutSeoncd = 1;

        private string _frontUrl;
        private string _elementTarget;

        public DeeplWebView()
        {
            InitializeComponent();
        }

        public void PrepareTranslate(DateTime dateTimeout)
        {
            _dtTimeout = dateTimeout;
            _dtRetryTimeOut = DateTime.Now.AddSeconds(RetryTimeoutSeoncd);
            Complete = false;
            IsError = false;
            _lastResult = _defaultKey;
        }

        public void Init(IDeeplAPIContract contract, string frontUrl, string urlFormat, string elementTarget)
        {
            if (_webView != null)
            {
                return;
            }
            _contract = contract;
            _frontUrl = frontUrl;
            _urlFormat = urlFormat;
            _elementTarget = elementTarget;

            _webView = new WebView2();

            _webView.Source = new Uri(_frontUrl);
            _webView.NavigationCompleted += WebView_NavigationCompleted;


            ((System.ComponentModel.ISupportInitialize)(_webView)).BeginInit();
            this.SuspendLayout();
            // 
            // webView
            // 
            this._webView.AllowExternalDrop = true;
            this._webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this._webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._webView.Location = new System.Drawing.Point(0, 0);
            this._webView.Name = "webView";
            _webView.Dock = DockStyle.Fill;
            _webView.Size = this.Size;



            this.Controls.Add(_webView);

            ((System.ComponentModel.ISupportInitialize)(_webView)).EndInit();
            this.ResumeLayout(false);

            _webView.Refresh();
        }

        public void DoTrans(string original, string transCode, string resultCode)
        {
            Complete = false;
            SourceAsync(original, transCode, resultCode);
            //return _lastResult;
        }

        private string ConvertHtmlToResult(string html)
        {
            if (html.Length > 2)
            {
                if (html[0] == '"' && html[html.Length - 1] == '"')
                {
                    html = html.Remove(0, 1);
                    html = html.Remove(html.Length - 1, 1);
                }

                html = html.Replace("\\n\\n", System.Environment.NewLine);
                html = html.Replace("\\n", "");
                int removePoint = html.LastIndexOf(_suffix);
                html = html.Remove(removePoint);
                html = html.TrimEnd();
                //html = html.Replace(GlobalDefine.SPLITE_TOEKN_DEEPL + "\\n\\n" ,GlobalDefine.SPLITE_TOEKN_DEEPL + System.Environment.NewLine);

                if (html.IndexOf(GlobalDefine.SPLITE_TOEKN_DEEPL) == 0)
                {
                 //   html = html.Insert(GlobalDefine.SPLITE_TOEKN_DEEPL.Length, System.Environment.NewLine);
                }


            }

            return html;
        }

        private async Task SourceAsync(string text, string transCode, string resultCode)
        {
            while (!_start)
            {
                await Task.Delay(100);
            }

            while (DateTime.Now < _dtNextAvailableTime)
            {
                await Task.Delay(50);
                Console.WriteLine("Wait : " + _dtNextAvailableTime.ToString());
            }

            double random = _rand.NextDouble();

            text = text.Replace(@"/", @"\/");
            text = text.TrimEnd();
            text += System.Environment.NewLine + _suffix;
            //랜덤 딜레이를 준다
            await Task.Delay((int)(random * 140));
            string requestText = RestSharp.Extensions.StringExtensions.UrlEncode(text);

            if (requestText != _lastUrl)
            {
                _webView.Source = new Uri(string.Format(_urlFormat, transCode, resultCode, requestText));
                Console.WriteLine("ocr : " + string.Format(_urlFormat, transCode, resultCode, requestText));
                await Task.Delay(50);
            }
            else
            {
                //1초 대기로 변경
                _dtTimeout = _dtRetryTimeOut;
            }

            //elementTarget 기본값 백업
            //elementTarget = "document.getElementsByClassName(\"lmt__textarea lmt__textarea_dummydiv\")[1].innerHTML";
            //_elementTarget = "document.getElementsByClassName(\"relative flex flex-1 flex-col\")[{0}].innerText";
            string result = "";
            do
            {
                try
                {
                    await Task.Delay(50);
                    var html = await _webView.CoreWebView2.ExecuteScriptAsync(_elementTarget);
                    string origianl = html;
                    if (html == null || html == "null" || html == "" || html == "\"\\n\"" || html == "\"\"")
                    {
                        if (_dtTimeout < DateTime.Now)
                        {
                            Complete = true;
                            IsError = true;
                            Console.WriteLine($"WebView {_dtTimeout} / now {DateTime.Now}");
                            _lastResult = "";
                            return;
                        }

                        result = _defaultKey;
                        Console.WriteLine("null");
                        await Task.Delay(100);
                        continue;
                    }

                    html = ConvertHtmlToResult(html);


                    result = html;

                    if (_dtTimeout < DateTime.Now && false)
                    {
                        Complete = true;
                        IsError = true;
                        Console.WriteLine($"WebView {_dtTimeout} / now {DateTime.Now}");
                        _lastResult = "";
                        return;
                    }
                }
                catch (Exception e) { IsError = true; Complete = true; _lastResult = e.Message; Console.WriteLine(e); }

            }
            while (result == _lastResult || result == _defaultKey);

            //랜덤 딜레이를 준다
            random = _rand.NextDouble();
            _dtNextAvailableTime = DateTime.Now.AddMilliseconds(random * 650);

            _lastUrl = requestText;
            _lastResult = result;
            Complete = true;

        }

        private void WebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!_start)
            {
                _contract.UpdateCondition($"DeepL_Ready");
                _start = true;
            }
        }

        private void DeeplWebView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;//종료를 취소하고 
            }
            else
            {
                Close();
            }
        }
    }
}
