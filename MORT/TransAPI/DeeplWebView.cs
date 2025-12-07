using Microsoft.Web.WebView2.WinForms;
using System;
using System.ComponentModel;
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Complete { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        private string _lastLanguageCode;

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
            if(_webView != null)
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
            if(html.Length > 2)
            {
                if(html[0] == '"' && html[html.Length - 1] == '"')
                {
                    html = html.Remove(0, 1);
                    html = html.Remove(html.Length - 1, 1);
                }

                html = html.Replace("\\n\\n", System.Environment.NewLine);
                html = html.Replace("\\n", "");
                int removePoint = html.LastIndexOf(_suffix);
                if(removePoint >= 0)
                {
                    html = html.Remove(removePoint);
                    html = html.TrimEnd();
                    //html = html.Replace(GlobalDefine.SPLITE_TOEKN_DEEPL + "\\n\\n" ,GlobalDefine.SPLITE_TOEKN_DEEPL + System.Environment.NewLine);

                    if(html.IndexOf(GlobalDefine.SPLITE_TOEKN_DEEPL) == 0)
                    {
                        //   html = html.Insert(GlobalDefine.SPLITE_TOEKN_DEEPL.Length, System.Environment.NewLine);
                    }
                }
            }

            return html;
        }

        private async Task SetEmptyResultAsync()
        {
            int remainCount = 4;
            try
            {

                string scriptToClearText = @"
            (function() {
                // 'data-testid=""translator-target-input""'를 가진 요소의 하위에 있는 'div[contenteditable=""true""]'를 찾습니다.
                var editableDiv = document.querySelector('[data-testid=""translator-target-input""] div[contenteditable=""true""]');
                
                if (editableDiv) {
                    // 1. 요소의 내부 HTML을 빈 문자열로 설정하여 텍스트를 비웁니다.
                    editableDiv.innerHTML = '';
                    
                    // 2. 입력 포커스를 다시 설정하여 사용자가 바로 입력할 수 있도록 합니다.
                    editableDiv.focus(); 

                    // 3. (추가된 핵심 부분) 'input' 이벤트를 수동으로 생성하고 발생시킵니다.
                    // 이 이벤트는 웹 애플리케이션에게 내용이 사용자 입력처럼 '변경'되었음을 알립니다.
                    var event = new Event('input', { bubbles: true });
                    editableDiv.dispatchEvent(event);

                    // 4. C#으로 성공 메시지 전송
                    if (window.chrome && window.chrome.webview) {
                        window.chrome.webview.postMessage('TEXTBOX_CLEARED_SUCCESS');
                    }
                    
                    return 'true';
                } else {
                    return '지정된 data-testid와 contenteditable 속성을 모두 가진 요소를 찾을 수 없습니다.';
                }
            })();
        ";

                while(remainCount >= 0)
                {
                    var test = await _webView.ExecuteScriptAsync(scriptToClearText);

                    if(test == "\"true\"")
                    {
                        break;
                    }

                    await Task.Delay(50);
                    remainCount--;
                }



            }
            catch
            {

            }
        }

        private async Task RefreshTextAsync()
        {
            try
            {


                string scriptToSetText = $@"
            (function() {{
                // 소스 텍스트 박스('data-testid=""translator-source-input""')의 하위에 있는 'div[contenteditable=""true""]'를 찾습니다.
                var editableDiv = document.querySelector('[data-testid=""translator-source-input""] div[contenteditable=""true""]');
                
                if (editableDiv) {{
                    // 2. 입력 포커스를 다시 설정합니다.
                    editableDiv.focus(); 

                    // 3. (핵심 부분) 'input' 이벤트를 수동으로 생성하고 발생시킵니다.
                    var event = new Event('input', {{ bubbles: true }});
                    editableDiv.dispatchEvent(event);

                    // 4. C#으로 성공 메시지 전송
                    if (window.chrome && window.chrome.webview) {{
                        window.chrome.webview.postMessage('SOURCE_TEXTBOX_SET_SUCCESS');
                    }}
                    
                    return '소스 텍스트 박스 내용이 성공적으로 설정되었으며, 변경 이벤트도 발생시켰습니다.';
                }} else {{
                    return '지정된 data-testid=""translator-source-input"" 요소를 찾을 수 없습니다.';
                }}
            }})();
        ";

                await _webView.ExecuteScriptAsync(scriptToSetText);

            }
            catch
            {

            }
        }


        private async Task SourceAsync(string text, string transCode, string resultCode)
        {
            while(!_start)
            {
                await Task.Delay(100);
            }

            while(DateTime.Now < _dtNextAvailableTime)
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
            string languageCode = transCode + resultCode;


            if(requestText != _lastUrl)
            {

                if(true)
                {
                    await SetEmptyResultAsync();
                    _lastLanguageCode = languageCode;
                    //더이상 사용하지 않는다 추후에 문제가 생기면 사용한다
                    _webView.Source = new Uri(string.Format(_urlFormat, transCode, resultCode, requestText));
                    Console.WriteLine("ocr : " + string.Format(_urlFormat, transCode, resultCode, requestText));
                    await Task.Delay(50);
                }
                else
                {


                }

            }
            else
            {
                //1초 대기로 변경
                _dtTimeout = _dtRetryTimeOut;
            }

            //elementTarget 기본값 백업
            //elementTarget = "document.getElementsByClassName(\"lmt__textarea lmt__textarea_dummydiv\")[1].innerHTML";
            //_elementTarget = "document.getElementsByClassName(\"relative flex flex-1 flex-col\")[{0}].innerText";
            bool first = true;
            string result = "";
            do
            {
                try
                {

                    var html = await _webView.CoreWebView2.ExecuteScriptAsync(_elementTarget);
                    string origianl = html;
                    if(html == null || html == "null" || html == "" || html == "\"\\n\"" || html == "\"\"")
                    {
                        if(_dtTimeout < DateTime.Now)
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

                    if(_dtTimeout < DateTime.Now && false)
                    {
                        Complete = true;
                        IsError = true;
                        Console.WriteLine($"WebView {_dtTimeout} / now {DateTime.Now}");
                        _lastResult = "";
                        return;
                    }
                }
                catch(Exception e)
                {
                    IsError = true;
                    Complete = false;
                    result = _defaultKey;
                    Console.WriteLine(e);
                }

            }
            while(result == _lastResult || result == _defaultKey);

            //랜덤 딜레이를 준다
            random = _rand.NextDouble();
            _dtNextAvailableTime = DateTime.Now.AddMilliseconds(random * 650);

            _lastUrl = requestText;
            _lastResult = result;
            Complete = true;

        }

        private void WebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if(!_start)
            {
                _contract.UpdateCondition($"DeepL_Ready");
                _start = true;
            }
        }

        private void DeeplWebView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
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
