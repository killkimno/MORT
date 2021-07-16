using MORT.CustomControl;
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
    public partial class UIAdvencedOption : Form
    {
        private Dictionary<int, CtSettingHotKey> hotKeyDic = new Dictionary<int, CtSettingHotKey>();
        public UIAdvencedOption()
        {
            InitializeComponent();
            hotKeyDic.Add(0, ctSettingHotKey1);
            hotKeyDic.Add(1, ctSettingHotKey2);
            hotKeyDic.Add(2, ctSettingHotKey3);


            foreach(var obj in hotKeyDic)
            {
                obj.Value.Init(obj.Key, KeyInputLabel.KeyType.OpenSetting);
            }

        }

        private void Init()
        {
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

            AdvencedOptionManager.Save();

            FormManager.Instace.MyMainForm.ApplyAdvencedOption();
        }

        private void UIAdvencedOption_Load(object sender, EventArgs e)
        {
            Init();
        }
    }
}
