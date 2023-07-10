using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MORT
{
    public partial class DicEditorForm : Form
    {
        private string dicFileName = "";
        private bool isUseJpnFlag = false;
        private string ocrString = "";
        private delegate void myDelegate(string newText);
        private bool isUseClipboard = false;

        private Form1.PDelegateSetSpellCheck setSpellCheck;


        public void SetSpellCheckFunction(Form1.PDelegateSetSpellCheck newFunction)
        {
            setSpellCheck = newFunction;
        }
        private void SetOriginalText(string newText)
        {
            originalTextBox.Text = newText;
        }

        public DicEditorForm(string newText, bool newJpnFlag, string newDicFileName)
        {
            dicFileName = newDicFileName;
            InitializeComponent();
            this.Show();
            ocrString = newText;
            isUseJpnFlag = newJpnFlag;
            this.BeginInvoke(new myDelegate(SetOriginalText), new object[] { newText });
        }

        public DicEditorForm()
        {
            InitializeComponent();

            if (FormManager.Instace.MyMainForm.IsUseClipBoardFlag == true)
            {
                isUseClipboard = true;
                FormManager.Instace.MyMainForm.IsUseClipBoardFlag = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aceeoptButton_Click(object sender, EventArgs e)
        {
            string originalText = originalTextBox.Text.Replace("\r\n", "");

            string resultText = resultTextbox.Text.Replace("\r\n", "");

            if (originalText != "" && resultTextbox.Text != "")
            {
                if (dicFileName.CompareTo("") == 0)
                {
                    dicFileName = ".\\DIC\\dic.txt";

                    if (isUseJpnFlag == true)
                    {
                        dicFileName = ".\\DIC\\dicJpn.txt";
                    }
                }
                else
                {
                    dicFileName = ".\\DIC\\" + dicFileName;
                }


                System.IO.StreamWriter file;


                try
                {
                    using (file = new System.IO.StreamWriter(dicFileName, true, Encoding.UTF8))
                    {

                        file.Write(System.Environment.NewLine);
                        file.WriteLine("/s");
                        file.WriteLine(originalText);
                        file.WriteLine(resultText);
                        file.Write(System.Environment.NewLine);
                    }

                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(dicFileName))
                    {
                        fs.Close();
                        fs.Dispose();
                        file = new System.IO.StreamWriter(dicFileName, true);

                        file.Write(System.Environment.NewLine);
                        file.WriteLine("/s");
                        file.WriteLine(originalText);
                        file.WriteLine(resultText);
                        file.Write(System.Environment.NewLine);
                    }
                }

                file.Close();
                file.Dispose();

                setSpellCheck();
            }


            this.Close();
        }

        private void DicEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isUseClipboard == true)
            {
                FormManager.Instace.MyMainForm.IsUseClipBoardFlag = true;
                isUseClipboard = false;
            }
            FormManager.Instace.DestoryDicEditorForm();
        }



    }
}
