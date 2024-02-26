using MORT.LocalizeManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MORT
{
    public partial class ColorGroupForm : Form, ILocalizeForm
    {
        private OcrAreaForm ocrAreaForm;

        public ColorGroupForm()
        {
            InitializeComponent();
            LocalizeForm();
        }

        public void LocalizeForm()
        {
            this.LocalizeLabel("ColorGroupForm Title");
            cbReset.LocalizeLabel("ColorGroupForm Reset Button");
            acceptButton.LocalizeLabel("Common Apply");
            btnCancel.LocalizeLabel("Common Cancel");
            lbInformation.LocalizeLabel("ColorGroupForm Information");
        }

        private void ColorGroupForm_Load(object sender, EventArgs e)
        {


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void ShowGroupForm(OcrAreaForm ocrAreaForm)
        {
            this.ocrAreaForm = ocrAreaForm;
            this.checkedListBox.Items.Clear();
            UpdateList(ocrAreaForm.screenType);
        }

        public void UpdateList(screenForm.ScreenType screenType)
        {
            if(ocrAreaForm != null && !ocrAreaForm.IsDisposed)
            {
                int index = ocrAreaForm.Index;
                List<int> useColorList = null;
                if(screenType == screenForm.ScreenType.Normal)
                {
                    useColorList = FormManager.Instace.MyMainForm.MySettingManager.UseColorGroup[index - 1];
                }
                else if(screenType == screenForm.ScreenType.Quick)
                {
                    useColorList = FormManager.Instace.MyMainForm.MySettingManager.QuickOcrUsecolorGroup;
                }

                List<ColorGroup> list = FormManager.Instace.MyMainForm.MySettingManager.NowColorGroup;

                for(int i = 0; i < list.Count; i++)
                {
                    string value = (i + 1).ToString() + " : R=" + (list[i].getValueR().ToString()) + ",G=" + list[i].getValueG().ToString() + ",B=" + list[i].getValueB().ToString();
                    value += " / S=" + list[i].getValueS1().ToString() + "~" + list[i].getValueS2().ToString() + "V=" + list[i].getValueV1().ToString() + "~" + list[i].getValueV2().ToString();
                    this.checkedListBox.Items.Add(value);


                    bool isChecked = true;
                    if(useColorList[i] == 0)
                        isChecked = false;
                    checkedListBox.SetItemChecked(i, isChecked);
                }
            }

        }

        private void Accept()
        {
            if(ocrAreaForm != null && !ocrAreaForm.IsDisposed)
            {
                int index = ocrAreaForm.Index;
                List<int> useColorList = null;
                if(ocrAreaForm.screenType == screenForm.ScreenType.Quick)
                {
                    useColorList = FormManager.Instace.MyMainForm.MySettingManager.QuickOcrUsecolorGroup;
                }
                else
                {
                    useColorList = FormManager.Instace.MyMainForm.MySettingManager.UseColorGroup[index - 1];
                }


                for(int i = 0; i < useColorList.Count && i < checkedListBox.Items.Count; i++)
                {
                    bool isChecked = this.checkedListBox.GetItemChecked(i);
                    if(isChecked)
                        useColorList[i] = 1;
                    else
                        useColorList[i] = 0;
                }

                this.Close();
            }

        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            Accept();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, true);
            }
        }
    }
}
