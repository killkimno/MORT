using MORT.CustomControl;
using MORT.LocalizeManager;
using MORT.SettingData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace MORT
{
    public partial class UIAdvencedOption : Form, ILocalizeForm
    {
        private string _fontData;
        private Dictionary<int, CtSettingHotKey> settingHotKeyDic = new Dictionary<int, CtSettingHotKey>();
        private Dictionary<KeyInputLabel.KeyType, CustomControl.CtHotKey> hotKeyDic = new System.Collections.Generic.Dictionary<KeyInputLabel.KeyType, CustomControl.CtHotKey>();
        private bool isInit = false;
        public UIAdvencedOption()
        {
            InitializeComponent();
        }

        private string LocalizeString(string key)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key, "");
        }

        private void AddHotKey(string title, string information, KeyInputLabel.KeyType type, CustomControl.CtHotKey hotKey)
        {
            hotKey.Init(title, information, LocalizeString("Common HotKey Clear"), type);
            hotKeyDic.Add(type, hotKey);
        }

        private void Init()
        {
            settingHotKeyDic.Add(0, ctSettingHotKey1);
            settingHotKeyDic.Add(1, ctSettingHotKey2);
            settingHotKeyDic.Add(2, ctSettingHotKey3);
            settingHotKeyDic.Add(3, ctSettingHotKey4);

            string title = LocalizeManager.LocalizeManager.GetLocalizeString("Adv HotKey Setting Load Input", "");
            string file = LocalizeManager.LocalizeManager.GetLocalizeString("Adv HotKey Setting File Input", "");
            string clear = LocalizeManager.LocalizeManager.GetLocalizeString("Common HotKey Clear", "");
            string fileSelect = LocalizeManager.LocalizeManager.GetLocalizeString("Common File Select", "");

            foreach(var obj in settingHotKeyDic)
            {
                obj.Value.Init(string.Format(title, obj.Key + 1), string.Format(file, obj.Key + 1), clear, fileSelect, obj.Key, KeyInputLabel.KeyType.OpenSetting);
            }

            AddHotKey(LocalizeString("Adv HotKey Force Invisible"), LocalizeString("Adv HotKey Force Invisible Info"), KeyInputLabel.KeyType.LayerTransparency, ctLayerTransparencyHotKey);
            AddHotKey(LocalizeString("Adv HotKey Db"), LocalizeString("Adv HotKey Db Info"), KeyInputLabel.KeyType.DBTranslate, ctDb);
            AddHotKey(LocalizeString("Adv HotKey PaPago"), LocalizeString("Adv HotKey PaPago Info"), KeyInputLabel.KeyType.NaverTranslate, ctNaverTrans);
            AddHotKey(LocalizeString("Adv HotKey BasicTrans"), LocalizeString("Adv HotKey BasicTrans Info"), KeyInputLabel.KeyType.GoogleTranslate, ctGoogleTrans);
            AddHotKey(LocalizeString("Adv HotKey Google Sheet"), LocalizeString("Adv HotKey Google Sheet Info"), KeyInputLabel.KeyType.GoogleSheetTranslate, ctGoogleSheet);
            AddHotKey(LocalizeString("Adv HotKey EzTrans"), LocalizeString("Adv HotKey EzTrans Info"), KeyInputLabel.KeyType.EzTrans, ctEzTrans);
            AddHotKey(LocalizeString("Adv HotKey DeepL"), LocalizeString("Adv HotKey DeepL Info"), KeyInputLabel.KeyType.DeepL, ctDeepL);
            AddHotKey(LocalizeString("Adv HotKey PapagoWeb"), LocalizeString("Adv HotKey PapagoWeb Info"), KeyInputLabel.KeyType.PapagoWeb, ctPapagoWeb);

            LocalizeForm();
            InitData();
        }

        private void InitData()
        {

            isInit = false;
            //단축키 데이터를 가져온다.
            var list = AdvencedOptionManager.GetHotKeyList();

            foreach(var obj in settingHotKeyDic)
            {
                obj.Value.SetEmpty();
            }

            foreach(var obj in hotKeyDic)
            {
                obj.Value.SetEmpty();
            }

            foreach(var obj in list)
            {
                if(obj.keyType == KeyInputLabel.KeyType.OpenSetting)
                {
                    if(settingHotKeyDic.ContainsKey(obj.index))
                    {
                        settingHotKeyDic[obj.index].SetKeys(obj.keyList, obj.extraData);
                    }
                }
                else if(hotKeyDic.TryGetValue(obj.keyType, out var hotKey))
                {
                    hotKey.SetKeys(obj.keyList);
                }
            }
            //앱 설정
            cbEnableSystemTray.Checked = AdvencedOptionManager.EnableSystemTrayMode;
            cbEnableBorder.Checked = AdvencedOptionManager.EnableYellowBorder;
            cbEnableRTL.Checked = AdvencedOptionManager.EnableRTL;

            ocrAreaBackgroundColor.BackColor = AdvencedOptionManager.SelectOcrAreaBackgroundColor;
            ocrAreaSelectedColor.BackColor = AdvencedOptionManager.SelectOcrAreaColor;

            //번역창 설정
            cbOverlayAutoSize.Checked = AdvencedOptionManager.IsAutoFontSize;
            cbOverlayAutoMerge.Checked = AdvencedOptionManager.UseAutoMerge;
            _fontData = AdvencedOptionManager.BasicFontData;
            cbLayerAlignmentBottom.Checked = AdvencedOptionManager.LayerTextAlignmentBottom;
            cbLayerAlignmentRight.Checked = AdvencedOptionManager.LayerTextAlignmentRight;
            cbAdvencedHideTransform.Checked = AdvencedOptionManager.EnableAdvencedHideTransform;

            //결과 유지하기
            cbEnableMemory.Checked = AdvencedOptionManager.EnableTranslateMemory;
            SetUpDownValue(udMemoryTime, AdvencedOptionManager.TranslateMemoryLimit);
            SetUpDownValue(udMemoryTime, AdvencedOptionManager.TranslateMemoryRemainTime);

            //TOP MOST 설정
            cbTopMost.Checked = AdvencedOptionManager.UseTopMostOptionWhenTranslate;

            //비어있는 번역 무시
            cbIgonreEmpty.Checked = AdvencedOptionManager.UseIgonoreEmptyTranslate;

            SetUpDownValue(udMinFontSize, AdvencedOptionManager.MinAutoFontSize);
            SetUpDownValue(udMaxSFontize, AdvencedOptionManager.MaxAutoFontSize);
            SetUpDownValue(udSnapShotRemainTime, AdvencedOptionManager.SnapShopRemainTime);

            //번역기 설정
            cbJpnExecutive.Checked = AdvencedOptionManager.IsExecutive;
            cbDeeplAltOption.Checked = AdvencedOptionManager.UseDeeplAltOption;

            //커스텀 api
            tbCustomURL.Text = AdvencedOptionManager.CustomApiUrl;
            tbCustomApiSource.Text = AdvencedOptionManager.CustomApiLanguageSource;
            tbCustomApiTarget.Text = AdvencedOptionManager.CustomApiLanguageTarget;

            cbCustomApiLanguageCode.Checked = AdvencedOptionManager.UseGoogleLanguageCode;


            //구글 ocr 설정
            cbGoogleOcrPriority.Checked = AdvencedOptionManager.UseGoogleOCRPriority;
            SetUpDownValue(udGoogleOcrLimit, AdvencedOptionManager.GoogleOcrLimit);

            //교정사전 설정
            SetUpDownValue(udReProcessDicCount, AdvencedOptionManager.DicReProcessCount);

            //클립보드 설정
            cbIsUseClipboardTrans.Checked = AdvencedOptionManager.IsUseClipboardTrans;
            cbIsShowClipboardOriginal.Checked = AdvencedOptionManager.IsShowClipboardOriginal;
            cbShowProcessClipboard.Checked = AdvencedOptionManager.IsShowClipboardProcessing;

            //클립보드 저장
            cbClipboardSaveType.SelectedIndex = AdvencedOptionManager.ClipboardSaveType;

            //번역집 설정
            InitTranslationFile();
            InitAppLanguage();

            RenderCustomApiLanguageCode();
            isInit = true;

        }

        private void SetUpDownValue(NumericUpDown componet, int value)
        {
            if(value < componet.Minimum)
            {
                value = (int)componet.Minimum;
            }
            else if(value > componet.Maximum)
            {
                value = (int)componet.Maximum;
            }

            componet.Value = value;
        }

        #region :::::::::: 앱 설정 ::::::::::

        public void SetAppSetting()
        {
            AdvencedOptionManager.EnableSystemTrayMode = cbEnableSystemTray.Checked;
            AdvencedOptionManager.EnableYellowBorder = cbEnableBorder.Checked;

            AdvencedOptionManager.SelectOcrAreaBackgroundColor = ocrAreaBackgroundColor.BackColor;
            AdvencedOptionManager.SelectOcrAreaColor = ocrAreaSelectedColor.BackColor;

            AdvencedOptionManager.EnableRTL = cbEnableRTL.Checked;
        }

        #endregion


        #region :::::::::::: 고급 단축키 :::::::::::


        private void SetHotKey()
        {
            //설정
            List<HotKeyData> keyList = new List<HotKeyData>();

            foreach(var obj in settingHotKeyDic)
            {
                string result = obj.Value.Apply();
                HotKeyData data = new HotKeyData(obj.Key, obj.Value.keyType, obj.Value.GetKeys(), result, obj.Value.FilePath);
                keyList.Add(data);
            }

            foreach(var obj in hotKeyDic)
            {
                string keyResult = obj.Value.Apply();
                HotKeyData keyData = new HotKeyData(obj.Value, keyResult);
                keyList.Add(keyData);
            }

            AdvencedOptionManager.SetHotKey(keyList);

        }


        #endregion

        #region ::::::::: 번역 관련 :::::::::
        public void SetTranslatorSetting()
        {
            AdvencedOptionManager.SetExecutive(cbJpnExecutive.Checked);
            AdvencedOptionManager.SetDeeplOption(cbDeeplAltOption.Checked);

            AdvencedOptionManager.SetCustomApiOption(cbCustomApiLanguageCode.Checked, tbCustomApiSource.Text, tbCustomApiTarget.Text, tbCustomURL.Text);
        }

        #endregion

        private void SetOcrSetting()
        {
            AdvencedOptionManager.UseGoogleOCRPriority = cbGoogleOcrPriority.Checked;
            AdvencedOptionManager.GoogleOcrLimit = (int)udGoogleOcrLimit.Value;
        }


        #region ::::::::::: 번역창 관련 ::::::::::::

        public void SetOverlaySetting()
        {
            AdvencedOptionManager.SetOverLay(cbOverlayAutoSize.Checked, (int)udMinFontSize.Value, (int)udMaxSFontize.Value, (int)udSnapShotRemainTime.Value, cbOverlayAutoMerge.Checked);
        }

        public void SetLayerSetting()
        {
            AdvencedOptionManager.SetLayer(cbLayerAlignmentBottom.Checked, cbLayerAlignmentRight.Checked);
        }

        public void SetTranslationFormSetting()
        {
            AdvencedOptionManager.SetTranslationFormSetting(cbTopMost.Checked, cbIgonreEmpty.Checked, _fontData, cbAdvencedHideTransform.Checked);
            AdvencedOptionManager.SetTranslateMemory(cbEnableMemory.Checked, (int)udMemoryCount.Value, (int)udMemoryTime.Value);
        }

        #endregion

        #region:::::::::: 교정사전 관련 ::::::::::

        public void SetDicSetting()
        {
            AdvencedOptionManager.SetDicReProcessCount((int)udReProcessDicCount.Value);
        }

        #endregion

        #region ::::::::: 클립보드 관련 ::::::::::

        public void SetClipboardSetting()
        {
            AdvencedOptionManager.SetClipboardTrans(cbIsUseClipboardTrans.Checked, cbIsShowClipboardOriginal.Checked, cbShowProcessClipboard.Checked);
        }

        public void SetClipboardSaveSetting()
        {
            AdvencedOptionManager.SetClipboardSave(cbClipboardSaveType.SelectedIndex);
        }

        #endregion


        #region ::::::::::: 번역집 관련 :::::::::::

        private void InitTranslationFile()
        {
            cblTransration.Items.Clear();

            string[] fileEntries = Directory.GetFiles(GlobalDefine.ADVENCED_TRANSRATION_PATH, "*.txt");
            List<string> fileList = AdvencedOptionManager.TranslationFileList;
            foreach(var obj in fileEntries)
            {
                bool isCheck = false;
                string fileName = Path.GetFileNameWithoutExtension(obj);

                if(fileList.Contains(fileName))
                {
                    isCheck = true;
                }

                cblTransration.Items.Add(fileName, isCheck);
            }

            cbUseDbStyle.Checked = AdvencedOptionManager.IsTranslationDbStyle;
            cbCheckStringUpper.Checked = AdvencedOptionManager.IsTranslationStringUpper;

            gbDbOption.Enabled = cbUseDbStyle.Checked;
        }

        public void SetTranslationFile()
        {
            List<string> list = new List<string>();
            foreach(var obj in cblTransration.CheckedItems)
            {
                list.Add(obj.ToString());
            }

            AdvencedOptionManager.TranslationFileList = list;
            AdvencedOptionManager.IsTranslationDbStyle = cbUseDbStyle.Checked;
            AdvencedOptionManager.IsTranslationStringUpper = cbCheckStringUpper.Checked;
        }

        private void GetTranslationInfo(string file)
        {
            string information = Util.ParseStringFromFile(file, "@INFO");
            information = information.Replace("\r\n", "\n");
            information = information.Replace("\n", System.Environment.NewLine);

            tbInformation.Text = information;
        }

        private void cblTransration_SelectedIndexChanged(object sender, EventArgs e)
        {
            string file = cblTransration.SelectedItem.ToString();
            Console.WriteLine(file);
            GetTranslationInfo(GlobalDefine.ADVENCED_TRANSRATION_PATH + file + ".txt");
        }

        private void OnClickTransrtionAllOn(object sender, EventArgs e)
        {
            for(int i = 0; i < cblTransration.Items.Count; i++)
            {
                cblTransration.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void OnClickTransrtionAllOff(object sender, EventArgs e)
        {
            for(int i = 0; i < cblTransration.Items.Count; i++)
            {
                cblTransration.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void cbUseDbStyle_CheckedChanged(object sender, EventArgs e)
        {
            gbDbOption.Enabled = cbUseDbStyle.Checked;
        }


        #endregion

        #region :::::::::: 앱 언어 관련 ::::::::::

        private void SetAppLanguage()
        {
            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');
            string saveValue = "";
            var before = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);
            var current = LocalizeManager.AppLanguage.Korea;

            if(rbEnglish.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "en");
                current = LocalizeManager.AppLanguage.English;
                saveValue = "en";
            }
            else if(rbKorea.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "ko");
                current = LocalizeManager.AppLanguage.Korea;
                saveValue = "ko";
            }
            else if(rbJpn.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "ja");
                current = LocalizeManager.AppLanguage.Japanese;
                saveValue = "ja";
            }

            else if(rbzh.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "zh");
                current = LocalizeManager.AppLanguage.SimplifiedChinese;
                saveValue = "zh";
            }
            else if(rbId.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "id");
                current = LocalizeManager.AppLanguage.Indonesian;
                saveValue = "id";
            }
            else if(rbRu.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "ru");
                current = LocalizeManager.AppLanguage.Russian;
                saveValue = "ru";
            }
            else if(rbPt.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "pt");
                current = LocalizeManager.AppLanguage.Portuguese;
                saveValue = "pt";
            }
            else if(rbUk.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "uk");
                current = LocalizeManager.AppLanguage.Ukrainian;
                saveValue = "pt";
            }

            if(before != current)
            {
                string message = LocalizeManager.LocalizeManager.GetLocalizeString("LanguageNotice", "언어는 앱을 재시작해야 적용됩니다.", current);
                FormManager.ShowPopupMessage("", message);
            }

            // Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@SET_BASIC_DEFAULT_PAGE ", saveValue);
        }

        private void InitAppLanguage()
        {
            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');
            var language = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);

            switch(language)
            {
                case LocalizeManager.AppLanguage.Korea:
                    rbKorea.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.English:
                    rbEnglish.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Japanese:
                    rbJpn.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.SimplifiedChinese:
                    rbzh.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Indonesian:
                    rbId.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Russian:
                    rbRu.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Portuguese:
                    rbPt.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Ukrainian:
                    rbUk.Checked = true;
                    break;

                default:
                    rbKorea.Checked = true;
                    break;
            }
        }



        #endregion

        private void UIAdvencedOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FormManager.GetIsRemain())
            {
                FormManager.Instace.DestoryAdvencedOption();
            }
        }

        private void OnClickApply(object sender, EventArgs e)
        {
            SetAppSetting();
            SetHotKey();
            SetOverlaySetting();
            SetLayerSetting();
            SetTranslationFormSetting();
            SetDicSetting();
            SetClipboardSetting();
            SetClipboardSaveSetting();
            SetTranslatorSetting();
            SetOcrSetting();
            SetTranslationFile();
            SetAppLanguage();

            AdvencedOptionManager.Save();

            FormManager.Instace.MyMainForm.ApplyAdvencedOption();
            FormManager.ShowPopupMessage("MORT", LocalizeString("Apply Complete"));
        }


        private void OnClickReset(object sender, EventArgs e)
        {
            if(DialogResult.OK == MessageBox.Show("설정을 초기화 하시겠습니까?", "고급 설정 초기화", MessageBoxButtons.OKCancel))
            {
                AdvencedOptionManager.Reset();
                AdvencedOptionManager.Save();

                InitData();

                FormManager.Instace.MyMainForm.ApplyAdvencedOption();
            }
        }

        private void UIAdvencedOption_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void udMinFontSize_ValueChanged(object sender, EventArgs e)
        {
            if(isInit)
            {
                if(udMinFontSize.Value > udMaxSFontize.Value)
                {
                    udMaxSFontize.Value = udMinFontSize.Value;
                }

            }
        }

        private void udMaxSFontize_ValueChanged(object sender, EventArgs e)
        {
            if(isInit)
            {
                if(udMinFontSize.Value > udMaxSFontize.Value)
                {
                    udMinFontSize.Value = udMaxSFontize.Value;
                }
            }
        }

        private void udReProcessDicCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(_fontData))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(SerializableFont));
                    using(TextReader tr = new StringReader(_fontData))
                    {
                        SerializableFont font = (SerializableFont)deserializer.Deserialize(tr);
                        fontDialog.Font = font.ToFont();
                    }
                }
            }
            catch
            {

            }

            try
            {
                DialogResult dr = this.fontDialog.ShowDialog();
                //확인버튼 누르면 변경
                if(dr == DialogResult.OK)
                {
                    this.Invoke(new Action(delegate ()
                    {
                        SerializableFont serializableFont = new SerializableFont(this.fontDialog.Font);
                        XmlSerializer serializer = new XmlSerializer(serializableFont.GetType());
                        using(StringWriter sw = new StringWriter())
                        {
                            serializer.Serialize(sw, serializableFont);
                            _fontData = sw.ToString();
                            Console.WriteLine(_fontData);
                        }
                    }));
                }
            }
            catch(System.ArgumentException ex)
            {
                MessageBox.Show("사용할 수 없는 폰트입니다");
            }

        }

        public void LocalizeForm()
        {
            this.LocalizeLabel("Advenced Config");
            //버튼
            btnApply.LocalizeLabel("Common Apply");
            btReset.LocalizeLabel("Common Default");


            //탭
            AppConfigTab.LocalizeLabel("TabAppConfig");
            HotKeyTab.LocalizeLabel("TabHotKey");
            TransFormTab.LocalizeLabel("TabTransForm");
            TransZipTab.LocalizeLabel("TabTransBook");
            TransTab.LocalizeLabel("TabTrans");
            OcrTab.LocalizeLabel("TabOCR");
            DicTab.LocalizeLabel("TabDic");
            //앱 설정
            gbGeneral.LocalizeLabel("AdvencedGbGeneral");
            cbEnableSystemTray.LocalizeLabel("AdvencedCbSystemTray");
            cbEnableRTL.LocalizeLabel("AdvencedCbEnableRTL");

            gbAttachWindow.LocalizeLabel("Advenced Attach Window");
            cbEnableBorder.LocalizeLabel("Advenced Enable Yellow Border");

            gbOcrAreaColor.LocalizeLabel("Advenced OCR Area Background Color");
            lbOcrAreaBackgroundColor.LocalizeLabel("Adv OCR Area Background");
            lbOcrAreaSelectedColor.LocalizeLabel("Adv OCR Area Select");
            btnColorPreview.LocalizeLabel("Adv OCR Area Preview");
            btnSetDefaultColor.LocalizeLabel("Adv OCR Area Set Default");


            //고급 단축키
            gbHotKeySetting.LocalizeLabel("Adv HotKey Setting");
            gbHotKeyTransform.LocalizeLabel("Adv HotKey Transform");
            gbHotKeyTrans.LocalizeLabel("Adv HotKey Trans");


            //번역창
            gbOverlay.LocalizeLabel("Adv Transform Overlay");
            gbDark.LocalizeLabel("Adv Transform Dark");
            gbTransformLayer.LocalizeLabel("Adv Transform Layer");
            gbTransformGeneral.LocalizeLabel("Adv Transform General");

            cbOverlayAutoSize.LocalizeLabel("Adv Overlay Auto Size");
            cbOverlayAutoMerge.LocalizeLabel("Adv Overlay Auto Merge");
            lbOverlayFontMinSize.LocalizeLabel("Adv Overlay Min Font Size");
            lbOverlayFontMaxSize.LocalizeLabel("Adv Overlay Max Font Size");
            lbOverlaySnapShotRemainTime.LocalizeLabel("Adv Overlay Snap Shot Remain Time");
            btnFont.LocalizeLabel("Adv Font Setting");
            cbLayerAlignmentBottom.LocalizeLabel("Adv Layer Alignment Bottom");
            cbLayerAlignmentRight.LocalizeLabel("Adv Layer Alignment Right");
            cbIgonreEmpty.LocalizeLabel("Adv Ignore Empty");
            cbTopMost.LocalizeLabel("Adv Topmost");
            cbAdvencedHideTransform.LocalizeLabel("Adv Enable Advenced Hide Transform");

            udMaxSFontize.Anchor(lbOverlayFontMaxSize, 10, 50);
            udMinFontSize.Anchor(lbOverlayFontMinSize, 10, 50);
            udSnapShotRemainTime.Anchor(lbOverlaySnapShotRemainTime, 10, 50);

            gbMemory.LocalizeLabel("Adv Memory Title");
            cbEnableMemory.LocalizeLabel("Adv Enable Memory");
            lbMemoryCount.LocalizeLabel("Adv Memory Count");
            lbMemoryTime.LocalizeLabel("Adv Memory Time");
            lbMemoryInformation.LocalizeLabel("Adv Memory Information");

            udMemoryCount.Anchor(lbMemoryCount, 10, 50);
            udMemoryTime.Anchor(lbMemoryTime, 10, 50);

            //번역집
            gbTranslationZip.LocalizeLabel("Adv Translation Zip");
            lbUseTranslationZip.LocalizeLabel("Adv Use Translation Zip");
            lbInfoTranslationZip.LocalizeLabel("Adv Info Translation Zip");
            cbUseDbStyle.LocalizeLabel("Adv DbStyle Translation Zip");
            btAllOff.LocalizeLabel("Common Clear All");
            btAllOn.LocalizeLabel("Common Selact All");
            gbDbOption.LocalizeLabel("Adb DbStyleOption Translation Zip");
            cbCheckStringUpper.LocalizeLabel("Adv DbStyleOption String Upper Translation Zip");

            //번역 설정
            gbGoogleTrans.LocalizeLabel("Adv Google translation");
            gbDeepL.LocalizeLabel("Adv DeepL Option");
            gbClipboard.LocalizeLabel("Adv Clipboard");
            cbJpnExecutive.LocalizeLabel("Adv Jpn Executive");
            cbDeeplAltOption.LocalizeLabel("Adv Deepl Use Alt Option");
            cbIsUseClipboardTrans.LocalizeLabel("Adv Use Clipboard");
            cbIsShowClipboardOriginal.LocalizeLabel("Adv Clipboard Display Original");
            cbShowProcessClipboard.LocalizeLabel("Adv Clipboard Display Progress");

            gbCustomApi.LocalizeLabel("Adv Custom Api");
            gbCustomApiCode.LocalizeLabel("Adv Custom Api Language Code");
            cbCustomApiLanguageCode.LocalizeLabel("Adv Custom Api Use Google Language Code");


            //OCR 설정
            gbGoogleOcr.LocalizeLabel("Adv Google Ocr");
            cbGoogleOcrPriority.LocalizeLabel("Adv Priority Google Ocr");
            lbGoogleOcrLimit.LocalizeLabel("Adv Google Ocr Limit");
            lbLimitInfo1.LocalizeLabel("Adv Google Ocr Limit Info1");
            lbLimitInfo2.LocalizeLabel("Adv Google Ocr Limit Info2");
            lbLimitInfo3.LocalizeLabel("Adv Google Ocr Limit Info3");

            udGoogleOcrLimit.Anchor(lbGoogleOcrLimit, 10);

            //클립보드 저장 설정
            gbClipboardSave.LocalizeLabel("Adv Clipboard Save");
            lbClipboardSaveType.LocalizeLabel("Adv Clipboard Save Type");
            cbClipboardSaveType.LocalizeItems();
            cbClipboardSaveType.Anchor(lbClipboardSaveType, 10);

            //교정 사전
            gbDic.LocalizeLabel("Adv Dic");
            lbReProcessDic.LocalizeLabel("Adv RePorcess Dic");
            lbDicInfo.LocalizeLabel("Adv Dic Info");
            udReProcessDicCount.Anchor(lbReProcessDic, 10);
        }

        private void RenderCustomApiLanguageCode()
        {
            gbCustomApiCode.Enabled = !cbCustomApiLanguageCode.Checked;
        }

        private void cbCustomApiLanguageCode_CheckedChanged(object sender, EventArgs e) => RenderCustomApiLanguageCode();

        private void ocrAreaBackgroundColor_Click(object sender, EventArgs e)
        {
            Opulos.Core.UI.AlphaColorDialog acd = new Opulos.Core.UI.AlphaColorDialog();

            acd.ColorChanged += delegate
            {
                System.Diagnostics.Debug.WriteLine("Color changed: " + acd.Color);
            };

            acd.SetColor(ocrAreaBackgroundColor.BackColor);
            DialogResult dr2 = acd.ShowDialog();


            if(dr2 == DialogResult.OK)
            {
                Util.ShowLog("Result = " + acd.Color.A.ToString());
                var resultColor = acd.Color;
                ocrAreaBackgroundColor.BackColor = resultColor;
            }
            acd.Dispose();
        }

        private void ocrAreaSelectedColor_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = AdvencedOptionManager.SelectOcrAreaColor;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if(dr == DialogResult.OK)
            {
                ocrAreaSelectedColor.BackColor = this.colorDialog1.Color;
            }
        }

        private void btnSetDefaultColor_Click(object sender, EventArgs e)
        {
            ocrAreaBackgroundColor.BackColor = Color.White;
            ocrAreaSelectedColor.BackColor = Color.Black;
        }

        private void btnColorPreview_Click(object sender, EventArgs e)
        {
            screenForm.MakePreview(ocrAreaSelectedColor.BackColor, ocrAreaBackgroundColor.BackColor);
        }
    }
}
