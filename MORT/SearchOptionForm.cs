using MORT.LocalizeManager;
using System;
using System.Windows.Forms;


namespace MORT
{
    public partial class SearchOptionForm : Form, ILocalizeForm
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
            FormManager.Instace.MyMainForm.SetCaptureArea();
            this.Close();
        }

        public SearchOptionForm()
        {
            InitializeComponent();
            LocalizeForm();
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

        private void AddExceptButton_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < FormManager.Instace.exceptionAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.exceptionAreaFormList[i];
                foundedForm.Activate();
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.Activate();
            }

            FormManager.Instace.MakeExceptionAreaForm();
        }


        private void acceptButton_Click(object sender, EventArgs e)
        {
            acceptCaptureArea();

        }

        private void defaultButton_Click(object sender, EventArgs e)
        {
            FormManager.Instace.ResetUseColorGroup();
            FormManager.Instace.DestoryAllOcrAreaForm(true);
            FormManager.Instace.MyMainForm.SetCaptureArea();
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

        public void LocalizeForm()
        {
            this.LocalizeLabel("OCR Area Manage Form");
            addButton.LocalizeLabel("OCR Area Add Area");
            AddExceptButton.LocalizeLabel("OCR Area Manage Except Area");
            defaultButton.LocalizeLabel("Common Default");
            acceptButton.LocalizeLabel("OCR Area Apply");
        }
    }
}
