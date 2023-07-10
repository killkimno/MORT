using System;
using System.Windows.Forms;



namespace MORT
{



    public partial class About : Form
    {

        public About()
        {
            InitializeComponent();

            lbVersion.Text = "버전 : " + Properties.Settings.Default.MORT_VERSION + " - " + Properties.Settings.Default.MORT_RELEASE;

            int dicVersion = 0;
            string dicEngVersion = "";
            string dicJpnVersion = "";
            string result = Util.ParseStringFromFile(GlobalDefine.DATA_VERSION_FILE, "@MORT_DIC_ENG", '[', ']');

            if (!string.IsNullOrEmpty(result))
            {
                dicEngVersion = result;
            }

            result = Util.ParseStringFromFile(GlobalDefine.DATA_VERSION_FILE, "@MORT_DIC_JPN", '[', ']');

            if (!string.IsNullOrEmpty(result))
            {
                dicJpnVersion = result;
            }

            lbDicversion.Text = "교정사전 버전 : 영문 - " + dicEngVersion + " / 일본어 - " + dicJpnVersion;

          
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            {
                Util.OpenURL("https://blog.naver.com/killkimno");
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            {
                Util.OpenURL("http://blog.naver.com/sabon2000");
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Logo")
                {
                    return;
                }
            }

            Logo logo = new Logo();
            logo.StartPosition = FormStartPosition.CenterScreen;

            logo.Show();
            logo.disableLogo(2.5f);


        }
    }
}
