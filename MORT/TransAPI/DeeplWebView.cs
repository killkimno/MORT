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

        public string LastResult => _lastResult;
        
        public bool Complete { get;  set; }

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

        public void Init()
        {
            if (_webView != null)
            {
                return;
            }

            _webView = new WebView2();

            _webView.Source = new Uri("https://www.deepl.com/translator#en/ko/tank%20divsion");
            _webView.NavigationCompleted += _webView_NavigationCompleted;
        }

        public void DoTrans(string original)
        {
            Complete = false;
            SourceAsync(original);
            //return _lastResult;
        }

        private async Task SourceAsync(string text)
        {
            while (!_start)
            {
                Console.WriteLine("wait");
                await Task.Delay(10);
            }
            string requestText = RestSharp.Extensions.StringExtensions.UrlEncode(text);
            _webView.Source = new Uri($"https://www.deepl.com/translator#en/ko/{requestText}");
            await Task.Delay(10);

            var test = "document.getElementsByClassName(\"lmt__textarea lmt__textarea_dummydiv\")[1].innerHTML";
            string result = "";
            do
            {
                try
                {
                    await Task.Delay(10);
                    var html = await _webView.CoreWebView2.ExecuteScriptAsync(test);
                    if (html == null)
                    {
                        Console.WriteLine("null");
                        await Task.Delay(10);
                        continue;
                    }

                    result = html;

                    if(_dtTimeOut < DateTime.Now)
                    {
                        Complete = true;
                        _lastResult = "TimeOut";
                        return;
                        break;
                    }
                }
                catch (Exception e) { Complete = true; Console.WriteLine(e); }

            }
            while (result == _lastResult || result == _defaultKey);

           
            _lastResult = result;
            Complete = true;
            Console.WriteLine("Done : " + result);
        }

        private void _webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            _start = true;
        }
    }
}
