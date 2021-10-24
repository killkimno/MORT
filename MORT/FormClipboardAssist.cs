using MORT.ClipboardAssist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT
{
    /// <summary>
    /// 클립보드 처리 
    /// </summary>
    public partial class Form1 : IClipboard
    {
        public bool _isDoingClipboard;
        private string _lastText = "";
        private ClipboardMonitor monitor;

        /// <summary>
        /// 클립보드 감지기 초기화
        /// </summary>
        public void InitClipboardMonitor(bool isUse)
        {
            if (monitor == null)
            {
                monitor = new ClipboardMonitor(this);              
            }

            monitor.SetActive(isUse);

        }

        void IClipboard.TextChange(string text)
        {
            if(_isDoingClipboard)
            {
                return;
            }

            TextChangeAsync(text);
        }

        public async void TextChangeAsync(string text)
        {
            try
            {
                if ((thread == null || isEndFlag) && eCurrentState == eCurrentStateType.None)
                {
                    if (MySettingManager.NowSkin != SettingManager.Skin.over && _lastText.CompareTo(text) != 0)
                    {
                        Util.ShowLog("Start clipboard : " + text);
                        _isDoingClipboard = true;


                        if (AdvencedOptionManager.IsShowClipboardProcessing)
                        {
                            FormManager.Instace.AddText("클립보드 감지 - 번역중");
                        }
                   

                        System.Threading.Tasks.Task<string> transTasks = TransManager.Instace.StartTrans(text, MySettingManager.NowTransType);
                        await transTasks;

                        string result = "";
                     

                        result = transTasks.Result;

                        if (AdvencedOptionManager.IsShowClipboardOriginal)
                        {
                            result += $"{System.Environment.NewLine}{System.Environment.NewLine}클립보드 : {text}";
                        }

                        FormManager.Instace.ForceUpdateText(result);
                        DoTextToSpeach(transTasks.Result);

                        _isDoingClipboard = false;
                    }
                }
            }
            catch
            {

            }

        }


    }
}
