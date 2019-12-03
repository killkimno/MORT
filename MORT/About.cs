using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace MORT
{



    public partial class About : Form
    {

        public About()
        {
            InitializeComponent();

            lbVersion.Text = "버전 : " + Properties.Settings.Default.MORT_VERSION + " - " + Properties.Settings.Default.MORT_RELEASE;
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
                try
                {
                    System.Diagnostics.Process.Start("http://killkimno.blog.me");
                }
                catch { }
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            {
                try
                {
                    System.Diagnostics.Process.Start("http://blog.naver.com/kluge_");
                }
                catch { }
            }
            
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            {
                try
                {
                    System.Diagnostics.Process.Start("http://blog.naver.com/sabon2000");
                }
                catch { }
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
