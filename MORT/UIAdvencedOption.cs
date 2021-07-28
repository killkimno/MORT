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
        private Dictionary<int, CtSettingHotKey> hotKeyDic = new Dictionary<int, CtSettingHotKey>();
        private bool isInit = false;
        public UIAdvencedOption()
        {
            InitializeComponent();
            hotKeyDic.Add(0, ctSettingHotKey1);
            hotKeyDic.Add(1, ctSettingHotKey2);
            hotKeyDic.Add(2, ctSettingHotKey3);
            hotKeyDic.Add(3, ctSettingHotKey4);

            foreach (var obj in hotKeyDic)
            {
                obj.Value.Init(obj.Key, KeyInputLabel.KeyType.OpenSetting);
            }

        }

        private void Init()
        {
            isInit = false;
            //단축키 데이터를 가져온다.
            var list = AdvencedOptionManager.GetHotKeyList();

            foreach (var obj in hotKeyDic)
            {
                obj.Value.SetEmpty();
            }

            foreach(var obj in list)
            {
                if(hotKeyDic.ContainsKey(obj.index))
                {
                    hotKeyDic[obj.index].SetKeys(obj.keyList, obj.extraData);
                }
            }


            //번역창 설정
            cbOverlayAutoSize.Checked = AdvencedOptionManager.IsAutoFontSize;

            SetUpDownValue(udMinFontSize, AdvencedOptionManager.MinAutoFontSize);
            SetUpDownValue(udMaxSFontize, AdvencedOptionManager.MaxAutoFontSize);


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


        #region :::::::::::: 고급 단축키 - 설정 불러오기 :::::::::::

        private  void SetHotKey()
        {
            List<HotKeyData> keyList = new List<HotKeyData>();

            foreach(var obj in hotKeyDic)
            {
                string result = obj.Value.Apply();
                HotKeyData data = new HotKeyData(obj.Key, obj.Value.keyType, obj.Value.GetKeys(), result, obj.Value.FilePath);
                keyList.Add(data);
            }

            AdvencedOptionManager.SetHotKey(keyList);
        

        }


        #endregion

        #region ::::::::::: 번역창 관련 ::::::::::::
        
        public void SetOverlaySetting()
        {
            AdvencedOptionManager.SetOverLay(cbOverlayAutoSize.Checked, (int)udMinFontSize.Value, (int)udMaxSFontize.Value);
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
            SetHotKey();
            SetOverlaySetting();
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

       
    }
}
