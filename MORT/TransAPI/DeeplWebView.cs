using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace MORT.TransAPI
{
    public partial class DeeplWebView : Form
    {
        public  WebView2 _webView;
        private string _url = "https://www.deepl.com/translator#en/ko/tank%20divsion";
        private bool _start = false;

        private string _lastResult = "\"\\r\\n\"";
        private string _defaultKey = "\"\\r\\n\"";
        private DateTime _dtTimeout;
        private DateTime _dtRetryTimeOut;
        private IDeeplAPIContract _contract;

        private string _urlFormat = "https://www.deepl.com/translator#{0}/{1}/{2}";

        private Random _rand = new Random();

        public string LastResult => _lastResult;
        
        public bool Complete { get;  set; }
        public bool IsError { get; private set; }
        private bool _initComplete;
        /// <summary>
        /// 마지막으로 입력된 값
        /// </summary>
        private string _lastUrl;
        private DateTime _dtNextAvailableTime = DateTime.MinValue;
        private const int RetryTimeoutSeoncd = 1;

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

        public void Init(IDeeplAPIContract contract)
        {        
            if (_webView != null)
            {
                return;
            }
            _contract = contract;

            _webView = new WebView2();

            _webView.Source = new Uri("https://www.deepl.com/translator#en/ko/tank%20divsion");
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

        private async Task SourceAsync(string text, string transCode, string resultCode )
        {
            while (!_start)
            {               
                await Task.Delay(100);
            }

            while(DateTime.Now < _dtNextAvailableTime)
            {
                await Task.Delay(50);
                Console.WriteLine("Wait : " + _dtNextAvailableTime.ToString());
            }

            double random = _rand.NextDouble();

            //랜덤 딜레이를 준다
            await Task.Delay((int)(random * 200));
            Util.ShowLog($"delay {(int)(random * 200)}");
            string requestText = RestSharp.Extensions.StringExtensions.UrlEncode(text);

            if(requestText != _lastUrl)
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

            var test = "document.getElementsByClassName(\"lmt__textarea lmt__textarea_dummydiv\")[1].innerHTML";
            string result = "";
            do
            {
                try
                {
                    await Task.Delay(50);
                    var html = await _webView.CoreWebView2.ExecuteScriptAsync(test);
                    if (html == null)
                    {
                        Console.WriteLine("null");
                        await Task.Delay(100);
                        continue;
                    }

                    result = html;
                    if (_dtTimeout < DateTime.Now)
                    {
                        Complete = true;
                        IsError = true;
                        Console.WriteLine($"WebView {_dtTimeout} / now { DateTime.Now}");
                        _lastResult = "";
                        return;
                    }
                }
                catch (Exception e) { IsError = true; Complete = true; _lastResult = e.Message; Console.WriteLine(e); }

            }
            while (result == _lastResult || result == _defaultKey);

            //랜덤 딜레이를 준다
            random = _rand.NextDouble();
            _dtNextAvailableTime = DateTime.Now.AddMilliseconds(random * 1000);
                    
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
