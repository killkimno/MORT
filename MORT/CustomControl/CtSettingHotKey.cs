using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MORT.CustomControl
{
    public partial class CtSettingHotKey : UserControl
    {
        public int index;
        public KeyInputLabel.KeyType keyType = KeyInputLabel.KeyType.None;


        public string FilePath
        {
            get
            {
                return tbFile.Text;
            }
        }
        public CtSettingHotKey()
        {
            InitializeComponent();
        }

        public void Init(string title, string file, string clearKey, string fileSelect, int index, KeyInputLabel.KeyType keyType)
        {
            this.lbTitle.Text = title;
            this.lbFile.Text = file;
            this.index = index;
            this.keyType = keyType;

            btClear.Text = clearKey;
            btSelect.Text = fileSelect;
        }
        
        public void SetEmpty()
        {
            this.lbHotKey.SetEmpty();
            this.tbFile.Text = "";
        }

        public string Apply()
        {
            return this.lbHotKey.GetKeyListToString();
        }

        public void SetKeys(List<Keys> list, string filePath)
        {
            this.lbHotKey.SetKeyList(list);
            this.tbFile.Text = filePath;
        }

        public List<Keys> GetKeys()
        {
            List<Keys> list = this.lbHotKey.keyList;
            return list;
        }


        private void btClear_Click(object sender, EventArgs e)
        {
            lbHotKey.SetEmpty();
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.RestoreDirectory = false;
            openPanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            openPanel.Filter = "Config File (*.conf)|*.conf";
            string file = "";
            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                file = openPanel.FileName;
                string filePath = Application.StartupPath;

                file =  file.Replace(filePath + GlobalDefine.SETTING_PATH.Replace("/", "\\"), "");

                if (file != "")
                {
                    tbFile.Text = file;
                }
            }
        }


    }
}
