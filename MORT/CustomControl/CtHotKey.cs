using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MORT.CustomControl
{
    public partial class CtHotKey : UserControl
    {
        public KeyInputLabel.KeyType keyType = KeyInputLabel.KeyType.None;
        public CtHotKey()
        {
            InitializeComponent();
        }

        public void Init(string title, string info, string clear, KeyInputLabel.KeyType keyType)
        {
            btClear.Text = clear;
            this.lbTitle.Text = title;
            this.keyType = keyType;
            this.lbInformation.Text = info;

        }


        public void SetEmpty()
        {
            this.lbHotKey.SetEmpty();
        }

        public string Apply()
        {
            return this.lbHotKey.GetKeyListToString();
        }

        public void SetKeys(List<Keys> list)
        {
            this.lbHotKey.SetKeyList(list);
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
    }
}
