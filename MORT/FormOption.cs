using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//UI 옵션 등을 저장처리
namespace MORT
{
    
    public partial class Form1
    {
        public const string YANDEX_FILE = "yandexAccount.txt";
        //Setting 메니져에 저장된 값을 기본 셋팅에 적용함.
        private void SetValueToUIValue()
        {
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                skinDarkRadioButton.Checked = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                skinLayerRadioButton.Checked = true;
            }
            //TODO : TEMP
            /*
            else if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                skinOverRadioButton.Checked = true;
            }
            */
            showOcrCheckBox.Checked = MySettingManager.NowIsShowOcrResultFlag;
            saveOCRCheckBox.Checked = MySettingManager.NowIsSaveOcrReulstFlag;
            isClipBoardcheckBox1.Checked = MySettingManager.NowIsSaveInClipboardFlag;

            if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract)
            {
                OCR_Type_comboBox.SelectedIndex = 0;
            }
            else if (MySettingManager.OCRType == SettingManager.OcrType.Window)
            {
                OCR_Type_comboBox.SelectedIndex = 1;
            }
            else
            {
                OCR_Type_comboBox.SelectedIndex = 0;
            }


            TransType_Combobox.SelectedIndex = (int)MySettingManager.NowTransType;

            checkStringUpper.Checked = MySettingManager.IsUseStringUpper;
            checkRGB.Checked = MySettingManager.NowIsUseRGBFlag;
            checkHSV.Checked = MySettingManager.NowIsUseHSVFlag;
            checkErode.Checked = MySettingManager.NowIsUseErodeFlag;

            switch (MySettingManager.NowOCRSpeed)
            {
                case 1:
                    speedRadioButton1.Checked = true;
                    break;

                case 2:
                    speedRadioButton2.Checked = true;
                    break;

                case 3:
                    speedRadioButton3.Checked = true;
                    break;

                case 4:
                    speedRadioButton4.Checked = true;
                    break;

                case 5:
                    speedRadioButton5.Checked = true;
                    break;

                default:
                    speedRadioButton3.Checked = true;
                    break;

            }

            dbFileTextBox.Text = MySettingManager.NowDBFile;
            tessDataTextBox.Text = MySettingManager.NowTessData;
            dicFileTextBox.Text = MySettingManager.NowDicFile;

            checkDic.Checked = MySettingManager.NowIsUseDicFileFlag;
            cbPerWordDic.Checked = MySettingManager.isUseMatchWordDic;
            setCheckSpellingToolStripMenuItem.Checked = MySettingManager.NowIsUseDicFileFlag;

            //언어 설정.
            if (MySettingManager.NowIsUseEngFlag)
            {
                languageComboBox.SelectedIndex = 0;
            }
            else if (MySettingManager.NowIsUseJpnFlag)
            {
                languageComboBox.SelectedIndex = 1;
            }
            else if (MySettingManager.NowIsUseOtherLangFlag)
            {
                languageComboBox.SelectedIndex = 2;
            }

            if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract || MySettingManager.OCRType == SettingManager.OcrType.NHocr)
            {
                for (int i = 0; i < TransManager.Instace.transCodeList.Count; i++)
                {
                    if (TransManager.Instace.transCodeList[i].Equals(MySettingManager.TransCode))
                    {
                        yandexTransCodeComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (MySettingManager.OCRType == SettingManager.OcrType.Window)
            {
                SetTransLangugageForWinOCR(MySettingManager.WindowLanguageCode);
            }


            for (int i = 0; i < TransManager.Instace.resultCodeList.Count; i++)
            {
                if (TransManager.Instace.resultCodeList[i].Equals(MySettingManager.ResultCode))
                {
                    yandexResultCodeComboBox.SelectedIndex = i;
                    break;
                }
            }

            //네이버.
            for (int i = 0; i < TransManager.Instace.naverTransCodeList.Count; i++)
            {
                if (TransManager.Instace.naverTransCodeList[i].Equals(MySettingManager.NaverTransCode))
                {
                    naverTransComboBox.SelectedIndex = i;
                    break;
                }
            }

           

            //윈도우 10 관련.
            if (isAvailableWinOCR)
            {
                for (int i = 0; i < languageCodeList.Count; i++)
                {
                    if (languageCodeList[i] == MySettingManager.WindowLanguageCode)
                    {
                        if (WinOCR_Language_comboBox.Items.Count > i)
                        {
                            WinOCR_Language_comboBox.SelectedIndex = i;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            initColorGroup();

            colorGroup = MySettingManager.NowColorGroup;
            if (colorGroup.Count != 0)
            {
                rTextBox.Text = colorGroup[0].getValueR().ToString();
                gTextBox.Text = colorGroup[0].getValueG().ToString();
                bTextBox.Text = colorGroup[0].getValueB().ToString();

                v1TextBox.Text = colorGroup[0].getValueV1().ToString();
                v2TextBox.Text = colorGroup[0].getValueV2().ToString();
                s1TextBox.Text = colorGroup[0].getValueS1().ToString();
                s2TextBox.Text = colorGroup[0].getValueS2().ToString();

                for (int i = 2; i <= MySettingManager.NowColorGroupCount; i++)
                {
                    groupCombo.Items.Add(i);
                }
            }

            locationXList = MySettingManager.NowLocationXList;
            locationYList = MySettingManager.NowLocationYList;
            sizeXList = MySettingManager.NowSizeXList;
            sizeYList = MySettingManager.NowSizeYList;

            textFont = MySettingManager.TextFont;
            textColor = MySettingManager.TextColor;
            outlineColor1 = MySettingManager.OutLineColor1;
            outlineColor2 = MySettingManager.OutLineColor2;
            backgroundColor = MySettingManager.BackgroundColor;

            if (MySettingManager.NowSortType == SettingManager.SortType.Center)
                alignmentCenterCheckBox.Checked = true;
            else
                alignmentCenterCheckBox.Checked = false;
            useBackColorCheckBox.Checked = MySettingManager.NowIsUseBackColor;
            removeSpaceCheckBox.Checked = MySettingManager.NowIsRemoveSpace;

            //엑티브 윈도우
            activeWinodeCheckBox.Checked = MySettingManager.NowIsActiveWindow;

            //업데이트 확인
            checkUpdateCheckBox.Checked = GetCheckUpdate();

            topMostcheckBox.Checked = isTranslateFormTopMostFlag;
            setTranslateTopMostToolStripMenuItem.Checked = isTranslateFormTopMostFlag;

            MySettingManager.TextColor = Color.FromArgb(15, 15, 15);
            fontButton.Text = textFont.FontFamily.Name;
            fontSizeUpDown.Value = (int)textFont.Size;
            SetColorBoxColor(textColorBox, textColor);
            SetColorBoxColor(outlineColor1Box, outlineColor1);
            SetColorBoxColor(outlineColor2Box, outlineColor2);
            SetColorBoxColor(backgroundColorBox, backgroundColor);

            imgZoomsizeUpDown.Value = (decimal)MySettingManager.ImgZoomSize;


            FormManager.Instace.ResetCaputreAreaForm();
        }


        //설정 파일로 저장.
        private void saveSetting(string fileName)
        {
            MySettingManager.NowTransType = transType;

            MySettingManager.NowOCRSpeed = (ocrProcessSpeed / 500) - 1;
            MySettingManager.NowColorGroupCount = groupCombo.Items.Count - 2;
            MySettingManager.NowColorGroup = colorGroup;
            MySettingManager.NowOCRGroupcount = locationXList.Count;
            MySettingManager.NowLocationXList = locationXList;
            MySettingManager.NowLocationYList = locationYList;
            MySettingManager.NowSizeXList = sizeXList;
            MySettingManager.NowSizeYList = sizeYList;

            MySettingManager.saveSetting(fileName);

        }



        //환경 설정 적용
        public void SetUIValueToSetting()
        {
            try
            {
                ChangeSkin();
                MySettingManager.NowIsShowOcrResultFlag = showOcrCheckBox.Checked;
                MySettingManager.NowIsSaveOcrReulstFlag = saveOCRCheckBox.Checked;
                IsUseClipBoardFlag = isClipBoardcheckBox1.Checked;

                transType = (SettingManager.TransType)TransType_Combobox.SelectedIndex;

                MySettingManager.IsUseStringUpper = checkStringUpper.Checked;
                MySettingManager.NowIsUseRGBFlag = checkRGB.Checked;
                MySettingManager.NowIsUseHSVFlag = checkHSV.Checked;
                MySettingManager.NowIsUseErodeFlag = checkErode.Checked;

                if (speedRadioButton1.Checked == true)
                {
                    ocrProcessSpeed = 1000;
                }
                else if (speedRadioButton2.Checked == true)
                {
                    ocrProcessSpeed = 1500;
                }
                else if (speedRadioButton3.Checked == true)
                {
                    ocrProcessSpeed = 2000;
                }
                else if (speedRadioButton4.Checked == true)
                {
                    ocrProcessSpeed = 2500;
                }
                else if (speedRadioButton5.Checked == true)
                {
                    ocrProcessSpeed = 3000;
                }

                MySettingManager.NowDBFile = dbFileTextBox.Text;
                MySettingManager.NowTessData = tessDataTextBox.Text;

                SaveHotKeyFile();


                SetCheckUpdate(checkUpdateCheckBox.Checked);

                //OCR 설정.
                MySettingManager.OCRType = SettingManager.GetOcrType(OCR_Type_comboBox.SelectedItem.ToString());

                //번역 코드 설정.
                string transCode = TransManager.Instace.transCodeList[yandexTransCodeComboBox.SelectedIndex];
                string resultCode = TransManager.Instace.resultCodeList[yandexResultCodeComboBox.SelectedIndex];


                MySettingManager.TransCode = transCode;
                MySettingManager.ResultCode = resultCode;

                MySettingManager.NaverTransCode = TransManager.Instace.naverTransCodeList[naverTransComboBox.SelectedIndex];
                MySettingManager.NaverResultCode = TransManager.Instace.naverResultCodeList[0];

                MySettingManager.GoogleTransCode = TransManager.Instace.googleTransCodeList[googleTransComboBox.SelectedIndex];
                MySettingManager.GoogleResultCode = TransManager.Instace.googleResultCodeList[googleResultCodeComboBox.SelectedIndex];


                NaverTranslateAPI.instance.SetTransCode(MySettingManager.NaverTransCode, MySettingManager.NaverResultCode);
                YandexAPI.instance.SetTransCode(MySettingManager.TransCode, resultCode);
                GoogleBasicTranslateAPI.instance.SetTransCode(MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);

                //윈도우 10 OCR 관련.
                if (isAvailableWinOCR && languageCodeList.Count > WinOCR_Language_comboBox.SelectedIndex)
                {
                    MySettingManager.WindowLanguageCode = languageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    loader.InitOCR(MySettingManager.WindowLanguageCode);
                }
                else
                {
                    MySettingManager.WindowLanguageCode = "";
                }

                //언어 설정.
                MySettingManager.NowIsUseEngFlag = false;
                MySettingManager.NowIsUseJpnFlag = false;
                MySettingManager.NowIsUseOtherLangFlag = false;

                if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract || MySettingManager.OCRType == SettingManager.OcrType.NHocr)
                {
                    if (languageComboBox.SelectedIndex == 0)
                    {
                        //영어.
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if (languageComboBox.SelectedIndex == 1)
                    {
                        //일본어
                        MySettingManager.NowIsUseJpnFlag = true;

                    }
                    else if (languageComboBox.SelectedIndex == 2)
                    {
                        //기타
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }
                else if (MySettingManager.OCRType == SettingManager.OcrType.Window && isAvailableWinOCR)
                {
                    string selectCode = languageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    if (selectCode == "en" || selectCode == "en-US")
                    {
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if (selectCode == "ja")
                    {
                        MySettingManager.NowIsUseJpnFlag = true;
                    }
                    else
                    {
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }

                //폰트 관련
                textFont = new Font(textFont.FontFamily, (int)fontSizeUpDown.Value);
                MySettingManager.TextFont = textFont;
                MySettingManager.TextColor = textColor;
                MySettingManager.OutLineColor1 = outlineColor1;
                MySettingManager.OutLineColor2 = outlineColor2;
                MySettingManager.BackgroundColor = backgroundColor;

                MySettingManager.NowIsUseBackColor = this.useBackColorCheckBox.Checked;
                if (this.alignmentCenterCheckBox.Checked)
                    MySettingManager.NowSortType = SettingManager.SortType.Center;
                else
                    MySettingManager.NowSortType = SettingManager.SortType.Normal;
                MySettingManager.NowIsRemoveSpace = this.removeSpaceCheckBox.Checked;

                MySettingManager.NowIsActiveWindow = activeWinodeCheckBox.Checked;


                //번역창 최상단
                isTranslateFormTopMostFlag = topMostcheckBox.Checked;
                setTranslateTopMostToolStripMenuItem.Checked = topMostcheckBox.Checked;

                //Console.WriteLine("Bing : " + transCode.ToString() + " Naver : " + MySettingManager.NaverTransCode);
                if (MySettingManager.NowSkin == SettingManager.Skin.dark)
                {
                    //더이상 이곳에서 번역키 설정 안 함
                }
                else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                {
                    FormManager.Instace.MyLayerTransForm.UpdateTransform();
                }

                ApplyTransSetting();              

                MySettingManager.ImgZoomSize = (float)imgZoomsizeUpDown.Value;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            //색 리스트
            int[] valueRArray = new int[groupCombo.Items.Count];
            int[] valueGArray = new int[groupCombo.Items.Count];
            int[] valueBArray = new int[groupCombo.Items.Count];

            int[] valueS1Array = new int[groupCombo.Items.Count];
            int[] valueS2Array = new int[groupCombo.Items.Count];
            int[] valueV1Array = new int[groupCombo.Items.Count];
            int[] valueV2Array = new int[groupCombo.Items.Count];

            colorGroup[nowColorGroupIndex].setRGBValuse(Convert.ToInt32(rTextBox.Text), Convert.ToInt32(gTextBox.Text), Convert.ToInt32(bTextBox.Text));
            colorGroup[nowColorGroupIndex].setHSVValuse(Convert.ToInt32(s1TextBox.Text), Convert.ToInt32(s2TextBox.Text), Convert.ToInt32(v1TextBox.Text), Convert.ToInt32(v2TextBox.Text));

            //교정사전.
            MySettingManager.NowIsUseDicFileFlag = checkDic.Checked;
            MySettingManager.NowDicFile = dicFileTextBox.Text;
            MySettingManager.isUseMatchWordDic = cbPerWordDic.Checked;
            for (int i = 0; i < colorGroup.Count; i++)
            {
                colorGroup[i].checkHSVRange();
                valueRArray[i] = colorGroup[i].getValueR();
                valueGArray[i] = colorGroup[i].getValueG();
                valueBArray[i] = colorGroup[i].getValueB();

                valueS1Array[i] = colorGroup[i].getValueS1();
                valueS2Array[i] = colorGroup[i].getValueS2();
                valueV1Array[i] = colorGroup[i].getValueV1();
                valueV2Array[i] = colorGroup[i].getValueV2();
            }


            groupLabel.Text = (groupCombo.Items.Count - 2).ToString();  //색 그룹 개수 표시

            try
            {
                SetIsUseNHocr(false);

                if (MySettingManager.OCRType == SettingManager.OcrType.Tesseract)
                {
                    bool isUseUnicode = false;
                    if (MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseUnicode = true;
                    }

                    if (MySettingManager.NowIsUseOtherLangFlag)
                    {
                        isUseUnicode = true;
                    }

                    setTessdata(MySettingManager.NowTessData, isUseUnicode);
                }
                else if (MySettingManager.OCRType == SettingManager.OcrType.Window)
                {
                    bool isUseJpn = false;

                    if (MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseJpn = true;
                    }

                    SetIsUseJpn(isUseJpn);

                }
                else if (MySettingManager.OCRType == SettingManager.OcrType.NHocr)
                {
                    SetIsUseNHocr(true);
                }


                setFiducialValue(valueRArray, valueGArray, valueBArray, valueS1Array, valueS2Array, valueV1Array, valueV2Array, groupCombo.Items.Count - 2);

                //교정사전 관련
                
                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.isUseMatchWordDic, MySettingManager.NowDicFile);


                bool isUseDBFlag = false;
                if (transType == SettingManager.TransType.db)
                {
                    isUseDBFlag = true;
                }
                SetIsStringUpper(MySettingManager.IsUseStringUpper);
                setUseDB(isUseDBFlag, MySettingManager.NowDBFile);
                setAdvencedImgOption(MySettingManager.NowIsUseRGBFlag, MySettingManager.NowIsUseHSVFlag, MySettingManager.NowIsUseErodeFlag, MySettingManager.ImgZoomSize);
                setCaptureArea();
                SetIsActiveWindow(MySettingManager.NowIsActiveWindow);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ApplyTransSetting()
        {
            yandexAccountTextBox.Text = yandexAccountTextBox.Text.Replace(" ", "");
            yandexKey = yandexAccountTextBox.Text;   

            string naverApiType = MORT.NaverTranslateAPI.API_NMT;
            naverApiType = MORT.NaverTranslateAPI.API_NMT;

            MySettingManager.NaverApiType = naverApiType;

            //공백 제거
            NaverIDKeyTextBox.Text = NaverIDKeyTextBox.Text.Replace(" ", "");
            NaverSecretKeyTextBox.Text = NaverSecretKeyTextBox.Text.Replace(" ", "");
            naverIDKey = NaverIDKeyTextBox.Text;
            naverSecretKey = NaverSecretKeyTextBox.Text;


            //번역기 초기화
            NaverTranslateAPI.instance.Init(naverIDKey, naverSecretKey, naverApiType);
            YandexAPI.instance.Init(yandexKey);

            //구글 토큰 성공 여부.
            SettingManager.isErrorEmptyGoogleToken = false;
            if (transType == SettingManager.TransType.google)
            {
                googleSheet_textBox.Text = googleSheet_textBox.Text.Replace(" ", "");
                textBox_GoogleClientID.Text = textBox_GoogleClientID.Text.Replace(" ", "");
                textBox_GoogleSecretKey.Text = textBox_GoogleSecretKey.Text.Replace(" ", "");
                Logo.SetTopmost(false);
                TransManager.Instace.InitGtrans(googleSheet_textBox.Text, textBox_GoogleClientID.Text, textBox_GoogleSecretKey.Text, MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);
            }

            SaveYandexKeyFile();
            SaveNaverKeyFile();
            SaveGoogleKeyFile();

            TransManager.Instace.ClearFormerDic();
        }
    }


    
}
