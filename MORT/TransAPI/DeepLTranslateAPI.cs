using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Windows.UI.Xaml.Controls;

namespace MORT.TransAPI
{
    public class DeepLTranslateAPI : IDisposable
    {
        private DeeplWebView _view;
        private string _url = "https://www.deepl.com/translator#en/ko/tank%20divsion";
        private bool _start = false;
        private DateTime _dtTimeOut;
        private string _lastResult = "\"\\r\\n\"";
        private string _defaultKey = "\"\\r\\n\"";

        public void Dispose()
        {
            if (_view != null && !_view.IsDisposed) { _view.Close(); }
        }

        public void Init()
        {
            if(_view != null )
            {
                return;
            }

            _view = new DeeplWebView();
            _view.Activate();
            _view.Show();
            _view.Hide();
            _view.Init();
        }

        private void _webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            _start = true;
        }

        public string DoTrans(string original, ref bool isError)
        {
            _dtTimeOut = DateTime.Now.AddSeconds(3);
            _view.PrepareTranslate();
            if (_view.InvokeRequired)
            {

                _view.BeginInvoke(new Action(() => _view.DoTrans(original)));
            }
            else
            {
                _view.DoTrans(original);
            }
            var task = SourceAsync();
            string result = task.Result;
            return result;
        }

        private async Task<string> SourceAsync()
        {
            while(!_view.Complete)
            {
                if (_dtTimeOut < DateTime.Now)
                {
                    return "TimeOut";
                }
                await Task.Delay(10);
            }

            return _view.LastResult;
        }
    }
}
