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

        }


    }
}
