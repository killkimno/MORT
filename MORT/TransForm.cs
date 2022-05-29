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
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MORT
{
    public partial class TransForm : Form, ITransform
    {
        public int TaskIndex { get; private set; }
        public Thread thread;  //빙 번역기 처리 쓰레드
        public TranslateStatusType TranslateStatusType { get; private set; }
        public bool UseTopMostOptionWhenTranslate { get; private set; }

        private Point mousePoint;

        private Font _defaultFont;

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

        public void ApplyFont(Font font)
        {
            this.Invoke(new Action(delegate ()
            {
                this.transTextBox.Font = font;
            }));
        }

        public void SetDefaultFont()
        {
            this.Invoke(new Action(delegate ()
            {
                this.transTextBox.Font = _defaultFont;
            }));
        }

        public void AddText(string addText)
        {
            transTextBox.Text = addText + System.Environment.NewLine + transTextBox.Text;
        }

        //ocr 및 번역 결과 처리
        public void updateText(string transText, string ocrText, SettingManager.TransType transType, bool isShowOCRResultFlag, bool isSaveOCRFlag)
        {

            if (AdvencedOptionManager.UseIgonoreEmptyTranslate && string.IsNullOrEmpty(ocrText))
            {
                return;
            }

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
            TranslateStatusType = TranslateStatusType.Stop;


            ApplyTopMost();
        }

        public void StartTrans()
        {
            TaskIndex++;
            if (TaskIndex > 100000)
            {
                TaskIndex = 0;
            }

            this.StopStateLabel.Visible = false;
            TranslateStatusType = TranslateStatusType.Translate;

            ApplyTopMost();
        }

        public TransForm()
        {
            InitializeComponent();
            _defaultFont = this.transTextBox.Font;
            string basicText = Properties.Settings.Default.BASIC_TEXT;
            basicText = string.Format(basicText, Properties.Settings.Default.MORT_VERSION);

            transTextBox.Text = basicText + System.Environment.NewLine + System.Environment.NewLine + "[TIP]" + Util.GetToolTip(); ;
            TranslateStatusType = TranslateStatusType.None;
        }

        public void ApplyUseTopMostOptionWhenTranslate(bool useTopMostOptionWhenTranslate)
        {
            UseTopMostOptionWhenTranslate = useTopMostOptionWhenTranslate;
            ApplyTopMost();
        }

        public void SetTopMost(bool topMost, bool useTopMostOptionWhenTranslate)
        {
            UseTopMostOptionWhenTranslate = useTopMostOptionWhenTranslate;
            isTopMostFlag = topMost;

            ApplyTopMost();
        }

        public void ApplyTopMost()
        {
            Action callback = delegate
            {
                if (UseTopMostOptionWhenTranslate)
                {
                    if (TranslateStatusType == TranslateStatusType.Translate)
                    {
                        this.TopMost = isTopMostFlag;
                    }
                    else
                    {
                        //번역중이 아니면 끈다
                        this.TopMost = false;
                    }
                }
                else
                {
                    this.TopMost = isTopMostFlag;
                }
            };


            if(InvokeRequired)
            {
                this.BeginInvoke(callback);
            }
            else
            {
                callback();
            }
        }

        private void closeApplication()
        {
            //더이상 안 씀.
            this.Visible = false;
            return;
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


        #region ::::::::: 인터페이스 관련 :::::::::::


        public void ForceTransparency()
        {

        }

        public void DoUpdate(bool isTranslating)
        {

        }

        public SettingManager.Skin GetSkinType()
        {
            return SettingManager.Skin.dark;
        }

        public void ForceUpdateText(string text)
        {
            transTextBox.Text = text;
        }

        #endregion


    }
}
