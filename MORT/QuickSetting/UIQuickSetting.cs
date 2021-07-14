using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
 

    public partial class UIQuickSetting : Form
    {            

        private enum StepType
        {
            None, SetFont, SetOCR, OcrComplete, Final,
        }
       

        private StepType currentStepType = StepType.None;

        private QuickSettingData settingData = null;


        public UIQuickSetting()
        {
            InitializeComponent();
        }

        public void Show(QuickSettingData.LanguageType language)
        {
            settingData = new QuickSettingData();

            FormManager.Instace.SetTopMostOcrArea(false);
            DisableAll();
            this.Show();
            settingData.languageType = language;

            ShowSetFont();
        }

        /// <summary>
        /// 폰트 색 설정하기
        /// </summary>
        private void SetFontColor()
        {
            if (rbBlack.Checked)
            {
                settingData.fontColorType = QuickSettingData.FontColorType.Black;
            }
            else if (rbWhite.Checked)
            {
                settingData.fontColorType = QuickSettingData.FontColorType.White;
            }
            else
            {
                settingData.fontColorType = QuickSettingData.FontColorType.None;
            }


        }

        private void SetTranslatorType()
        {
            SettingManager.TransType transType = SettingManager.TransType.google_url;

            if(settingData.languageType == QuickSettingData.LanguageType.English)
            {
                //구글 기본 번역기
                transType = SettingManager.TransType.google_url;
            }
            else if(settingData.languageType == QuickSettingData.LanguageType.Japen)
            {
                //1. 이지트랜스 사용여부 체크 -> 되면 이지 트랜스
                //2. 안 되면 구글 기본 번역기
                bool isEzTrans = TransManager.Instace.InitEzTrans();

                if(isEzTrans)
                {
                    transType = SettingManager.TransType.ezTrans;
                }
                else
                {
                    transType = SettingManager.TransType.google_url;
                }
            }

            settingData.transType = transType;
        }

        private void SetOcrType()
        {
            settingData.ocrType = SettingManager.OcrType.Tesseract;

            if (FormManager.Instace.MyMainForm.isAvailableWinOCR)
            {
                List<string> codeList = FormManager.Instace.MyMainForm.WinLanguageCodeList;
                string code = settingData.LanguageCode;

                foreach(var obj in codeList)
                {
                    if(Util.GetIsEqualWinCode(code, obj))
                    {
                        settingData.ocrType = SettingManager.OcrType.Window;
                    }
                }
            }
        }

        private void SetSetting()
        {
            if (FormManager.Instace.MySearchOptionForm != null)
            {
                FormManager.Instace.MySearchOptionForm.acceptCaptureArea();
            }

            SetOcrType();
            SetTranslatorType();

            bool isUseJpn = false;

            if(settingData.languageType == QuickSettingData.LanguageType.English)
            {
                isUseJpn = true;
            }

            FormManager.Instace.SetInvisibleOcrArea();
            FormManager.Instace.MyMainForm.ApplyFromQuickSetting(settingData);
            settingData = null;
        }



        private void DisableAll()
        {
            this.pnFinal.Visible = false;
            this.pnOcrComplete.Visible = false;
            this.pnSetFont.Visible = false;
            this.pnSetOcr.Visible = false;         
        
        }

        private void ShowSetFont()
        {
            DisableAll();

            currentStepType = StepType.SetFont;
            this.pnSetFont.Visible = true;
          
            rbUnknown.Checked = true;            
            lbTitle.Text = "번역할 게임의 폰트색은 어떤건가요?";
            btNext.Text = "다음";
        }

     

        private void ShowSetOCR()
        {
            DisableAll();

            currentStepType = StepType.SetOCR;
            this.pnSetOcr.Visible = true;

            lbTitle.Text = "게임 대사 영역 설정 - OCR 영역 설정";
            btNext.Text = "OCR 영역 설정";
        }

        private void DoOcrArea()
        {
            FormManager.Instace.ResetUseColorGroup();
            FormManager.Instace.DestoryAllOcrAreaForm(true);
            FormManager.Instace.MyMainForm.SetCaptureArea();
            FormManager.Instace.MakeCpatureAreaForm(ShowOcrComplete);
        }

        private void ShowOcrComplete()
        {
        
            FormManager.Instace.MakeSearchOptionForm();
            FormManager.Instace.SetTopMostOcrArea(false);
            DisableAll();

            currentStepType = StepType.OcrComplete;
            this.pnOcrComplete.Visible = true;

            lbTitle.Text = "OCR 영역 설정 완료";
            btNext.Text = "다음";

            this.Focus();
        }

        private void ShowFinal()
        {
            DisableAll();

            currentStepType = StepType.Final;
            this.pnFinal.Visible = true;

            lbTitle.Text = "설정이 끝났습니다";
            btNext.Text = "닫기";
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            switch(currentStepType)
            {
                case StepType.SetFont:
                    SetFontColor();
                    ShowSetOCR();
                    break;

                case StepType.SetOCR:
                    DoOcrArea();
                    break;

                case StepType.OcrComplete:
                    SetSetting();
                    ShowFinal();
                    break;
                case StepType.Final:

                    this.Close();
                

                    break;
            }
        }

        private void btShowTrnaslate_Click(object sender, EventArgs e)
        {
            try
            {

                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221760617100");
            }
            catch { }
      
        }

        private void btShowBasic_Click(object sender, EventArgs e)
        {
            try
            {

                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221904769542");
            }
            catch { }
        }

        private void UIQuickSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormManager.GetIsRemain())
            {
                FormManager.Instace.SetTopMostOcrArea(true);
                FormManager.Instace.DestoryQuickSetting();
            }
        }
    }
}
