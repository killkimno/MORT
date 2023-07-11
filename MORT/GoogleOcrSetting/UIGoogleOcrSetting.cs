using MORT.Manager;
using System;
using System.Windows.Forms;

namespace MORT.GoogleOcrSetting
{
    public partial class UIGoogleOcrSetting : Form
    {
        public UIGoogleOcrSetting()
        {
            InitializeComponent();
        }

        private void Init()
        {
            lbJson.Text = OcrManager.GoogleJsonPath;
            lbCount.Text = $"{ OcrManager.GoogleCurrentCount} / {AdvencedOptionManager.GoogleOcrLimit}";
            cbUseDefault.Checked = AdvencedOptionManager.UseGoogleOCRPriority;
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.RestoreDirectory = false;
            openPanel.InitialDirectory = System.Environment.CurrentDirectory;
            openPanel.Filter = "Json File (*.json)|*.json";


            string file = "";
            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                file = openPanel.FileName;
                lbJson.Text = file;

                OcrManager.Instace.SetGoogleOcrJsonPath(file);
            }
        }

        private void UIGoogleOcrSetting_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void cbUseDefault_CheckedChanged(object sender, EventArgs e)
        {
            if(cbUseDefault.Checked != AdvencedOptionManager.UseGoogleOCRPriority)
            {
                AdvencedOptionManager.UseGoogleOCRPriority = cbUseDefault.Checked;
                AdvencedOptionManager.Save();
            }
        }

        private void btnReadMe_Click(object sender, EventArgs e)
        {
            try
            {
                Util.OpenURL("https://blog.naver.com/killkimno/222712200800");
            }
            catch { }
        }
    }
}
