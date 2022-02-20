using MORT.CustomControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public partial class UIAdvencedOption : Form
    {
        private Dictionary<int, CtSettingHotKey> settingHotKeyDic = new Dictionary<int, CtSettingHotKey>();
        private bool isInit = false;
        public UIAdvencedOption()
        {
            InitializeComponent();
            settingHotKeyDic.Add(0, ctSettingHotKey1);
            settingHotKeyDic.Add(1, ctSettingHotKey2);
            settingHotKeyDic.Add(2, ctSettingHotKey3);
            settingHotKeyDic.Add(3, ctSettingHotKey4);

            foreach (var obj in settingHotKeyDic)
            {
                obj.Value.Init(obj.Key, KeyInputLabel.KeyType.OpenSetting);
            }

            ctLayerTransparencyHotKey.Init("강제 투명화 : ", "스냅샷, 한 번만 번역하기를 하더라도 투명화를 유지합니다", KeyInputLabel.KeyType.LayerTransparency);

        }

        private void Init()
        {
            isInit = false;
            //단축키 데이터를 가져온다.
            var list = AdvencedOptionManager.GetHotKeyList();

            foreach (var obj in settingHotKeyDic)
            {
                obj.Value.SetEmpty();
            }
            ctLayerTransparencyHotKey.SetEmpty();

            foreach (var obj in list)
            {
                if(obj.keyType == KeyInputLabel.KeyType.OpenSetting)
                {
                    if (settingHotKeyDic.ContainsKey(obj.index))
                    {
                        settingHotKeyDic[obj.index].SetKeys(obj.keyList, obj.extraData);
                    }
                }
                else if(obj.keyType == KeyInputLabel.KeyType.LayerTransparency)
                {
                    ctLayerTransparencyHotKey.SetKeys(obj.keyList);
                }
            }
            //앱 설정
            cbEnableSystemTray.Checked = AdvencedOptionManager.EnableSystemTrayMode;

            //번역창 설정
            cbOverlayAutoSize.Checked = AdvencedOptionManager.IsAutoFontSize;

            SetUpDownValue(udMinFontSize, AdvencedOptionManager.MinAutoFontSize);
            SetUpDownValue(udMaxSFontize, AdvencedOptionManager.MaxAutoFontSize);

            //번역기 설정
            cbJpnExecutive.Checked = AdvencedOptionManager.IsExecutive;

            //교정사전 설정
            SetUpDownValue(udReProcessDicCount, AdvencedOptionManager.DicReProcessCount);

            //클립보드 설정
            cbIsUseClipboardTrans.Checked = AdvencedOptionManager.IsUseClipboardTrans;
            cbIsShowClipboardOriginal.Checked = AdvencedOptionManager.IsShowClipboardOriginal;
            cbShowProcessClipboard.Checked = AdvencedOptionManager.IsShowClipboardProcessing;

            //번역집 설정
            InitTranslationFile();
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
        }

        #endregion


        #region :::::::::::: 고급 단축키 - 설정 불러오기 :::::::::::


        private void SetHotKey()
        {
            List<HotKeyData> keyList = new List<HotKeyData>();

            foreach(var obj in settingHotKeyDic)
            {
                string result = obj.Value.Apply();
                HotKeyData data = new HotKeyData(obj.Key, obj.Value.keyType, obj.Value.GetKeys(), result, obj.Value.FilePath);
                keyList.Add(data);
            }

            string keyResult = ctLayerTransparencyHotKey.Apply();
            HotKeyData keyData = new HotKeyData(ctLayerTransparencyHotKey, keyResult);
            keyList.Add(keyData);

            AdvencedOptionManager.SetHotKey(keyList);        

        }


        #endregion

        #region ::::::::: 번역 관련 :::::::::
        public void SetTranslatorSetting()
        {
            AdvencedOptionManager.SetExecutive(cbJpnExecutive.Checked);
        }

        #endregion

        #region ::::::::::: 번역창 관련 ::::::::::::

        public void SetOverlaySetting()
        {
            AdvencedOptionManager.SetOverLay(cbOverlayAutoSize.Checked, (int)udMinFontSize.Value, (int)udMaxSFontize.Value);
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

        #endregion


        #region ::::::::::: 번역집 관련 :::::::::::

        private void InitTranslationFile()
        {
            cblTransration.Items.Clear();

            string[] fileEntries = Directory.GetFiles(GlobalDefine.ADVENCED_TRANSRATION_PATH, "*.txt");
            List<string> fileList = AdvencedOptionManager.TranslationFileList;
            foreach (var obj in fileEntries)
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
            GetTranslationInfo(GlobalDefine.ADVENCED_TRANSRATION_PATH +  file + ".txt");
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
            for (int i = 0; i < cblTransration.Items.Count; i++)
            {
                cblTransration.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void cbUseDbStyle_CheckedChanged(object sender, EventArgs e)
        {
            gbDbOption.Enabled = cbUseDbStyle.Checked;
        }


        #endregion

        private void UIAdvencedOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormManager.GetIsRemain())
            {
                FormManager.Instace.DestoryAdvencedOption();
            }
        }

        private void OnClickApply(object sender, EventArgs e)
        {
            SetAppSetting();
            SetHotKey();
            SetOverlaySetting();
            SetDicSetting();
            SetClipboardSetting();
            SetTranslatorSetting();
            SetTranslationFile();
            AdvencedOptionManager.Save();

            FormManager.Instace.MyMainForm.ApplyAdvencedOption();
            FormManager.ShowPopupMessage("MORT", "적용 완료");
        }


        private void OnClickReset(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("설정을 초기화 하시겠습니까?", "고급 설정 초기화", MessageBoxButtons.OKCancel))
            {
                AdvencedOptionManager.Reset();
                AdvencedOptionManager.Save();              

                Init();

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
                if(udMinFontSize.Value > udMaxSFontize.Value )
                {
                    udMaxSFontize.Value = udMinFontSize.Value;
                }

            }
        }

        private void udMaxSFontize_ValueChanged(object sender, EventArgs e)
        {
            if (isInit)
            {
                if (udMinFontSize.Value > udMaxSFontize.Value)
                {
                    udMinFontSize.Value = udMaxSFontize.Value;
                }

            }
        }

        private void udReProcessDicCount_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
