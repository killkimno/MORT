using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Init(int index, KeyInputLabel.KeyType keyType)
        {
            this.lbTitle.Text = "설정 불러오기 " + (index+1).ToString() + " : ";
            this.lbFile.Text = "설정 파일명 " + (index + 1).ToString() + " : ";
            this.index = index;
            this.keyType = keyType;
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

                file =  file.Replace(filePath +"\\" + GlobalDefine.SETTING_PATH.Replace("/", "\\"), "");

                if (file != "")
                {
                    tbFile.Text = file;
                }
            }
        }


    }
}
