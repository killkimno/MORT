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
    public interface IDeeplAPIContract
    {
        void UpdateCondition(string condition);
    }
    public class DeepLTranslateAPI : IDisposable
    {
        private DeeplWebView _view;
        private string _url = "https://www.deepl.com/translator#en/ko/tank%20divsion";
        private bool _start = false;
        private DateTime _dtTimeOut;
        private string _lastResult = "\"\\r\\n\"";
        private string _defaultKey = "\"\\r\\n\"";

        private string _transCode = "en";
        private string _resultCode = "kr";
        private IDeeplAPIContract _contract;

        public const float NormalTimeoutSecond = 4;
        public const float AltTimeoutSecond = 2;

        public void Dispose()
        {
            if (_view != null && !_view.IsDisposed) { _view.Close(); }
        }

        public void InitContract(IDeeplAPIContract contract)
        {
            _contract = contract;
        }

        public void Init(string transCode, string resultCode)
        {
            _transCode = transCode;
            _resultCode = resultCode;

            CheckAndInitWebview(); 
        }

        private void CheckAndInitWebview()
        {
            if (_view == null || _view.IsDisposed)
            {
                _view = new DeeplWebView();
                _view.Activate();
                _view.Show();
                _view.Hide();
                _view.Init(_contract);
                _contract?.UpdateCondition($"DeepL_Init");
            }
        }

        public void ShowWebview()
        {
            if (_view == null || _view.IsDisposed)
            {
                _view = new DeeplWebView();
                _view.Visible = false;
                _view.Activate();
                _view.Show();
                //_view.Hide();
                _view.Init(_contract);
                _contract?.UpdateCondition($"DeepL_Init");
            }
            else
            {
                _view.Visible = true;
                _view.Show();
            }
        }

        public string DoTrans(string original, ref bool isError)
        {
            if(AdvencedOptionManager.UseDeeplAltOption)
            {
                _dtTimeOut = DateTime.Now.AddSeconds(AltTimeoutSecond);
            }
            else
            {
                _dtTimeOut = DateTime.Now.AddSeconds(NormalTimeoutSecond);
            }
     
            _view.PrepareTranslate(_dtTimeOut);
            if (_view.InvokeRequired)
            {
                _view.BeginInvoke(new Action(() => _view.DoTrans(original, _transCode, _resultCode)));
            }
            else
            {
                _view.DoTrans(original, _transCode, _resultCode);
            }
            var task = WaitResultAsync();
            string result = task.Result;
            if(result.Length >=4)
            {
                result= result.Replace("\"", "");
            }
            isError = _view.IsError;
            return result;
        }

        private async Task<string> WaitResultAsync()
        {
            while(!_view.Complete)
            {
                if (_dtTimeOut < DateTime.Now)
                {
                    Console.WriteLine($"api {_dtTimeOut} / now {DateTime.Now}");
                    return "TimeOut";
                }
                await Task.Delay(100);
            }

            return _view.LastResult;
        }
    }
}
