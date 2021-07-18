using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Services.Client;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace MORT
{



    public partial class TransForm : Form
    {
        public Thread thread;  //빙 번역기 처리 쓰레드

        private Point mousePoint;


        bool isTopMostFlag = true;
        bool isDestroyFormFlag = false;

        //번역창에 번역문 출력
        private delegate void myDelegate(string transText, string ocrText, bool isShowOCRResultFlag, bool isSaveOCRFlag);
        private void updateProgress(string transText, string ocrText, bool isShowOCRResultFlag, bool isSaveOCRFlag)
        {

            transTextBox.Text = transText;

            if (isShowOCRResultFlag == true)
            {
                transTextBox.Text += "\r\n\r\n" + "OCR : " + ocrText;
            }
            //만약 ocr 결과를 저장하기로 했으면
            if (isSaveOCRFlag == true)
            {
                ocrText = ocrText.Replace("\r\n", "\n");
                System.IO.StreamWriter file;
                try
                {
                    using (file = new System.IO.StreamWriter(@"ocrResult.txt", true))
                    {
                        file.WriteLine("/s");
                        file.WriteLine(ocrText);
                        file.WriteLine("/t");
                        file.WriteLine(transText);
                        file.WriteLine("/e");
                        file.WriteLine(System.Environment.NewLine);
                    }

                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(@"ocrResult.txt"))
                    {
                        fs.Close();
                        fs.Dispose();
                        file = new System.IO.StreamWriter(@"ocrResult.txt", true);
                        file.WriteLine("/s");
                        file.WriteLine(ocrText);
                        file.WriteLine("/t");
                        file.WriteLine(transText);
                        file.WriteLine("/e");
                        file.WriteLine(System.Environment.NewLine);
                    }
                }

                file.Close();
                file.Dispose();
            }
        }

        public void AddText(string addText)
        {
            transTextBox.Text = addText + System.Environment.NewLine + transTextBox.Text;
        }

        //ocr 및 번역 결과 처리
        public void updateText(string transText, string ocrText, SettingManager.TransType transType, bool isShowOCRResultFlag, bool isSaveOCRFlag)
        {
            try
            {
                if (thread != null)
                {
                    thread.Join();
                }
                try
                {
                    this.BeginInvoke(new myDelegate(updateProgress), new object[] { transText, ocrText, isShowOCRResultFlag, isSaveOCRFlag });
                }
                catch (InvalidOperationException)
                {
                    // Error logging, post processing etc.
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void StopTrans()
        {
            this.StopStateLabel.Visible = true;
        }

        public void StartTrans()
        {
            this.StopStateLabel.Visible = false;
        }

        public TransForm()
        {
            InitializeComponent();

            string basicText = Properties.Settings.Default.BASIC_TEXT;
            basicText = string.Format(basicText, Properties.Settings.Default.MORT_VERSION);

            transTextBox.Text = basicText + System.Environment.NewLine + System.Environment.NewLine + "[TIP]" + Util.GetToolTip(); ;
        }

        public void setTopMostFlag(bool newTopMostFlag)
        {
            isTopMostFlag = newTopMostFlag;
            this.TopMost = isTopMostFlag;
        }
        private void closeApplication()
        {
            //더이상 안 씀.
            this.Visible = false;
            return;
            Boolean isFindFormFlag = false;
            Form1 mainForm = null;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form1")
                {
                    mainForm = (Form1)frm;

                    if (mainForm.Visible == false)
                    {
                        isFindFormFlag = false;
                    }
                    else
                    {
                        isFindFormFlag = true;
                    }

                    break;
                }
            }
            if (isFindFormFlag == false)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "RTT")
                    {
                        if (frm.Visible == false)
                        {
                            isFindFormFlag = false;
                        }
                        else
                        {
                            isFindFormFlag = true;
                        }

                        break;
                    }
                }
            }

            if (isFindFormFlag == false && mainForm != null && this.Visible == true)
            {
                this.TopMost = false;
                if (MessageBox.Show("종료하시겠습니까?", "종료하시겠습니까?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    mainForm.exitApplication();

                }
                this.TopMost = isTopMostFlag;
            }
            else
            {
                this.Visible = false;
            }
        }

        public void destroyForm()
        {
            isDestroyFormFlag = true;
            FormManager.Instace.MyBasicTransForm = null;
            this.Close();
        }

        private void TransForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Join();
            }
            closeApplication();
            if (isDestroyFormFlag == false)
            {
                e.Cancel = true;//종료를 취소하고 
            }

        }

        private void TransForm_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void TransForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            closeApplication();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


    }
}
