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
    public partial class DonatePage : Form
    {
        public DonatePage()
        {
            InitializeComponent();
        }

        private void DonatePage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormManager.GetIsRemain())
            {
                FormManager.Instace.DestoryDonatePage();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenDonatePage();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenDonatePage();
        }

        private void OpenDonatePage()
        {
            try
            {
                System.Diagnostics.Process.Start("https://toon.at/donate/mort");
            }
            catch { }
        }
    }
}
