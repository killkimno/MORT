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
        private DateTime _dtTimeOut;
        private IDeeplAPIContract _contract;

        private string _urlFormat = "https://www.deepl.com/translator#{0}/{1}/{2}";

        private Random _rand = new Random();

        public string LastResult => _lastResult;
        
        public bool Complete { get;  set; }
        private bool _initComplete;

        public DeeplWebView()
        {
            InitializeComponent();
        }

        public void PrepareTranslate()
        {
            _dtTimeOut = DateTime.Now.AddSeconds(3);
            Complete = false;
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
            _webView.NavigationCompleted += _webView_NavigationCompleted;
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

            double random = _rand.NextDouble();

            //랜덤 딜레이를 준다
            await Task.Delay((int)(random * 300));
            Util.ShowLog($"delay {(int)(random * 300)}");
            string requestText = RestSharp.Extensions.StringExtensions.UrlEncode(text);
            _webView.Source = new Uri( string.Format(_urlFormat, transCode, resultCode, requestText));
            Console.WriteLine("ocr : " + string.Format(_urlFormat, transCode, resultCode, requestText));
            await Task.Delay(100);

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
                    Console.WriteLine("WAIT : " + result);
                    if (_dtTimeOut < DateTime.Now)
                    {
                        Complete = true;
                        _lastResult = "TimeOut";
                        return;
                        break;
                    }
                }
                catch (Exception e) { Complete = true; _lastResult = e.Message; Console.WriteLine(e); }

            }
            while (result == _lastResult || result == _defaultKey);

            random = _rand.NextDouble();

            //랜덤 딜레이를 준다
            await Task.Delay((int)(random * 350));

            _lastResult = result;
            Complete = true;
            Console.WriteLine("Done : " + result);
        }

        private void _webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if(!_start)
            {
                _contract.UpdateCondition($"DeepL_Ready");
                _start = true;
            }            
        }
    }
}
