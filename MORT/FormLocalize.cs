using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MORT.LocalizeManager;

namespace MORT
{
    public partial class Form1 : IGoogleBasicTranslateAPIContract
    {
        private void InitLocalize()
        {
            AppLanguage language = AppLanguage.Korea; // 어디선가 가져와야 한다
            CultureInfo ci = CultureInfo.CurrentUICulture;

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');

            if(string.IsNullOrEmpty(result))
            {
                result = ci.TwoLetterISOLanguageName;
            }
            language = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);
            LocalizeManager.LocalizeManager.Init(Properties.Resources.localize, language);

            //TODO : 딴곳에 둬야한다
            GoogleBasicTranslateAPI.instance.InitContract(this);
            GoogleBasicTranslateAPI.instance.UpdateCondition();
            LocalizeBasicForm();
            //this.lbTransType.Text = "fuck";
        }

        private void LocalizeBasicForm()
        {
            tpBasic.LocalizeLabel("Tab Basic");
            tpText.LocalizeLabel("Tab Text");
            tpExtra.LocalizeLabel("Tab Extra");
            tpTranslation.LocalizeLabel("Tab Translation");
            tpETC.LocalizeLabel("Tab ETC");
            tpQuickSetting.LocalizeLabel("Tab Quick Setting");
            tpDebuging.LocalizeLabel("Tab Debuging");

            //기본설정 - 패널
            lbTransTypeTitle.LocalizeLabel("Panel Translate");
            lbAdjustImg.LocalizeLabel("Panel Adjust Img");

            //기본설정 - OCR         
            showOcrCheckBox.LocalizeLabel("Show OCR Result");
            saveOCRCheckBox.LocalizeLabel("Save OCR Result");
            isClipBoardcheckBox1.LocalizeLabel("OCR Clipboard");

            OCR_Type_comboBox.LocalizeItems();

            //win ocr
            lbWinOCRLanguage.LocalizeLabel("Common Language");


            //구글 OCR
            lbGoogleOCRLanguage.LocalizeLabel("Common Language");
            btnSettingGoogleOCR.LocalizeLabel("Google OCR API Setting");
            lbGoogleOcrStatus.LocalizeLabel("Google OCR Information");

            //NH OCR
            lbNHOcrInfo.LocalizeLabel("NHOCR Info");

            //테저렉 OCR
            lbWinOCRLanguage.LocalizeLabel("Common Language");
            cbFastTess.LocalizeLabel("Fast Tesseract");
            lbTesseractLanguage.LocalizeLabel("Tesseract Language");

            //번역
            lbTransType.LocalizeLabel("Translation Type");
            TransType_Combobox.LocalizeItems();
            lbBasicInfo.LocalizeLabel("Basic Translate Info");
            lbPapagoID.LocalizeLabel("Papago ID");
            lbPapagoSecret.LocalizeLabel("Papago Secret");
            Button_NaverTransKeyList.LocalizeLabel("Key Manage");
            lbGoogleSheetAddress.LocalizeLabel("Sheet Address");
            lbSheetID.LocalizeLabel("Sheet ID");
            lbSheetSecret.LocalizeLabel("Sheet Secret");
            button_RemoveAllGoogleToekn.LocalizeLabel("Clear Sheet Toekn");
            lbEzTransInfo.LocalizeLabel("EzTrans Info");

            lbDbFile.LocalizeLabel("Common File Name");
            checkStringUpper.LocalizeLabel("DB Check String Upper");
            cbDBMultiGet.LocalizeLabel("DB Multi Line");

            checkDic.LocalizeLabel("Use Dic");
            lbDicFile.LocalizeLabel("Common File Name");
            cbPerWordDic.LocalizeLabel("Use Per Word Dic");




        }

        void IGoogleBasicTranslateAPIContract.UpdateCondition(string key)
        {
            lbBasicStatus.LocalizeLabel(key);
        }
    }

    public static class ComponenetLoclize
    {
        public static void LocalizeLabel(this System.Windows.Forms.Control control, string key)
        {
            if(control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text);
            }
        }

        public static void LocalizeItems(this System.Windows.Forms.ComboBox comboBox)
        {
            List<string> keys = new List<string>();
            System.Object[] results = new System.Object[comboBox.Items.Count];
            int index = 0;
            foreach (var obj in comboBox.Items)
            {
                string key = obj.ToString();
                string result = LocalizeManager.LocalizeManager.GetLocalizeString(key, key);
                results[index++] = result;
            }
            Action action = delegate ()
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(results);
            };

            if (comboBox.InvokeRequired)
            {
                comboBox.BeginInvoke(action);
            }
            else
            {
                action();
            }



        }

        public static void LocalizeLabel(this System.Windows.Forms.Control control)
        {
            if (control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(control.Name, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(control.Name, control.Text);
            }
        }

        public static void LocalizeLabel(this System.Windows.Forms.Control control, System.Windows.Forms.Control target, string key, int left, int min = 0)
        {
            if (control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text);
            }
            control.Anchor(target, left, min);
        }

        public static void Anchor(this System.Windows.Forms.Control control, System.Windows.Forms.Control target, int left, int min = 0)
        {
            int result = target.Location.X + target.Size.Width + left;

            if(result < min)
            {
                result = min;
            }

            control.Location = new System.Drawing.Point(result, control.Location.Y);
        }
    }
}
