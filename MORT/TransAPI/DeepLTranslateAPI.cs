using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.TransAPI
{
    public interface IDeeplAPIContract
    {
        void UpdateCondition(string condition);
    }
    public class DeepLTranslateAPI : IDisposable
    {

        private DeeplWebView _view;
        private string _url = "https://www.deepl.com/en/translator#en/ko/tank%20divsion";
        private bool _start = false;
        private DateTime _dtTimeOut;
        private string _lastResult = "\"\\r\\n\"";
        private string _defaultKey = "\"\\r\\n\"";

        private string _transCode = "en";
        private string _resultCode = "kr";
        private IDeeplAPIContract _contract;
        private bool _unavailableWebview;

        public const float NormalTimeoutSecond = 5f;
        public const float AltTimeoutSecond = 3f;

        private string _frontUrl;
        private string _urlFormat;
        private string _elementTarget;

        private bool _initialized;
        private bool _isFirstTranslate;

        public void Dispose()
        {
            if (_view != null && !_view.IsDisposed) { _view.Close(); }
        }

        public void InitContract(IDeeplAPIContract contract)
        {
            _contract = contract;
        }

        public void Init(string transCode, string resultCode, string frontUrl, string urlFormat, string elementTarget)
        {
            //2025 07 07 - deepl 코드 정책이 바뀌었다
            if(transCode is "zh-CN" or "zh-TW" or "ZH-HANS" or "ZH-HANT")
            {
                transCode = "zh";
            }

            if(resultCode is "zh-CN" or "zh-TW" or "ZH-HANS" or "ZH-HANT")
            {
                //resultCode = "zh-hant";
                resultCode = "zh";
            }
            _transCode = transCode;
            _resultCode = resultCode;

            //https://www.deepl.com/translator?share=generic#en/zh-hant/blablabla
            //frontUrl = "https://www.deepl.com/en/translator?share=generic#";
            //urlFormat = "https://www.deepl.com/translator?share=generic#{0}/{1}/{2}";

            _frontUrl = frontUrl;
            _urlFormat = urlFormat;
            _elementTarget = elementTarget;
            _initialized = true;
            CheckAndInitWebview(); 
        }

        private void CheckAndInitWebview()
        {
            try
            {
                if (_view == null || _view.IsDisposed)
                {
                    _view = new DeeplWebView();
                    _isFirstTranslate = true;
                    _view.Activate();
                    _view.Show();
                    _view.Hide();
                    _view.Init(_contract, _frontUrl, _urlFormat, _elementTarget);
                    _contract?.UpdateCondition($"DeepL_Init");
                }
            }
            catch
            {
                _unavailableWebview = true;
                _contract?.UpdateCondition($"DeepL_Error");
            }          
        }

        public void ShowWebview()
        {
            if(_unavailableWebview)
            {
                return;
            }

            if (!_initialized)
            {
                FormManager.ShowPopupMessage("", LocalizeManager.LocalizeManager.GetLocalizeString("DeepL_RequireApply_Message", "적용을 먼저 해주세요"));
                return;
            }

            if (_view == null || _view.IsDisposed)
            {
                _view = new DeeplWebView();
                _view.Visible = false;
                _view.Activate();
                _view.Show();
                _view.ShowInTaskbar = false;
                _view.Init(_contract, _frontUrl, _urlFormat, _elementTarget);
                _contract?.UpdateCondition($"DeepL_Init");
            }
            else
            {
                _view.ShowInTaskbar = true;
                _view.Visible = true;
                _view.Show();
            }
        }

        public string DoTrans(string original, ref bool isError, CancellationToken ct)
        {
            if(_unavailableWebview)
            {
                return "";
            }

            if(AdvencedOptionManager.UseDeeplAltOption)
            {
                _dtTimeOut = DateTime.Now.AddSeconds(AltTimeoutSecond);
            }
            else
            {
                _dtTimeOut = DateTime.Now.AddSeconds(NormalTimeoutSecond);
            }

            if(_isFirstTranslate)
            {
                // 첫 시도는 오래걸린다
                _isFirstTranslate = false;
                _dtTimeOut = _dtTimeOut.AddSeconds(5);
            }

            _view.PrepareTranslate(_dtTimeOut.AddSeconds(-0.5f));
            if (_view.InvokeRequired)
            {
                _view.BeginInvoke(new Action(() => _view.DoTrans(original, _transCode, _resultCode)));
            }
            else
            {
                _view.DoTrans(original, _transCode, _resultCode);
            }
            var task = WaitResultAsync(ct);
            string result = task.Result.Item1;
            if(result.Length >=4)
            {
                string token = "\"";
                var regex = new Regex(Regex.Escape(token));
                result = regex.Replace(result, "", 1);

                int target = result.LastIndexOf(token, StringComparison.InvariantCulture);
                          

                if (target != -1)
                {
                    try
                    {
                        result = result.Remove(target, token.Length);
                    }
                    catch
                    {

                    }
              
                }

                result = result.Replace(@"\r\n", "\r\n");
                result = result.Replace(@"\n", "\r\n");
                result = result.Replace(@"\r", "");
                result = result.Replace(@"\", "");

            }
            isError = _view.IsError || task.Result.Item2;
            return result;
        }

        private async Task<(string, bool)> WaitResultAsync(CancellationToken ct)
        {
            while(!_view.Complete)
            {
                if (_dtTimeOut < DateTime.Now && false)
                {
                    Console.WriteLine($"api {_dtTimeOut} / now {DateTime.Now}");
                    
                    return ("TimeOut", true);
                }
                await Task.Delay(80, cancellationToken:ct);
            }

            return (_view.LastResult, false);
        }
    }
}
