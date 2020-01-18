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
    public partial class SearchOptionForm : Form
    {
        bool isAccept;
        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }

        private void MakeScreenForm()
        {
            FormManager.Instace.MakeCpatureAreaForm();
        }

        public void acceptCaptureArea()
        {
            isAccept = true;
            FormManager.Instace.MyMainForm.setCaptureArea();
            this.Close();
        }

        public SearchOptionForm()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];
                foundedForm.Activate();
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.Activate();
            }

            MakeScreenForm();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            acceptCaptureArea();

        }

        private void defaultButton_Click(object sender, EventArgs e)
        {
            FormManager.Instace.ResetUseColorGroup();
            FormManager.Instace.DestoryAllOcrAreaForm();
            FormManager.Instace.MyMainForm.setCaptureArea();
        }

        private void DestoryForm(object sender, FormClosingEventArgs e)
        {
            if (isAccept == false)
            {
                FormManager.Instace.ResetCaputreAreaForm();
            }
            FormManager.Instace.SetInvisibleOcrArea();
            FormManager.Instace.DestorySearchOptionForm();

        }


    }
}
