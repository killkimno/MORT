using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Services.Client;
using System.Net;
using System.IO;
using System.Threading;
namespace MORT
{



    public partial class TransForm : Form
    {
        public Thread thread;  //빙 번역기 처리 쓰레드
        static TranslatorContainer tc;
        static string bingAccountKey;
        string formerOcrString;
        private Point mousePoint;
        private string transCode = "en";
        private string resultCode = "ko";
        

        bool isTopMostFlag = true;
        bool isDestroyFormFlag = false;

        #region:::::::::::::::::::::::::::::::::::::::::::계정키 클래스:::::::::::::::::::::::::::::::::::::::::::

        public void setBingAccountKey(string newKey)
        {
            bingAccountKey = newKey;
            tc = InitializeTranslatorContainer();
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this.transCode = transCode;
            this.resultCode = resultCode;
        }

     
        public partial class Translation
        {
            private String _Text;
            public String Text
            {
                get
                {
                    return this._Text;
                }
                set
                {
                    this._Text = value;
                }
            }
        }


        public partial class TranslatorContainer : System.Data.Services.Client.DataServiceContext
        {

            public TranslatorContainer(Uri serviceRoot) :
                base(serviceRoot)
            {
            }

            /// <summary>
            /// </summary>
            /// <param name="Text">the text to translate Sample Values : hello</param>
            /// <param name="To">the language code to translate the text into Sample Values : nl</param>
            /// <param name="From">the language code of the translation text Sample Values : en</param>
            public DataServiceQuery<Translation> Translate(String Text, String To, String From)
            {
                if ((Text == null))
                {
                    throw new System.ArgumentNullException("Text", "Text value cannot be null");
                }
                if ((To == null))
                {
                    throw new System.ArgumentNullException("To", "To value cannot be null");
                }
                DataServiceQuery<Translation> query;
                query = base.CreateQuery<Translation>("Translate");
                if ((Text != null))
                {
                    query = query.AddQueryOption("Text", string.Concat("\'", System.Uri.EscapeDataString(Text), "\'"));
                }
                if ((To != null))
                {
                    query = query.AddQueryOption("To", string.Concat("\'", System.Uri.EscapeDataString(To), "\'"));
                }
                if ((From != null))
                {
                    query = query.AddQueryOption("From", string.Concat("\'", System.Uri.EscapeDataString(From), "\'"));
                }
                return query;
            }

        }
        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::번역 관련 메소드:::::::::::::::::::::::::::::::::::::::::::
        private static TranslatorContainer InitializeTranslatorContainer()
        {
            // this is the service root uri for the Microsoft Translator service 
            System.Uri serviceRootUri = new Uri("https://api.datamarket.azure.com/Bing/MicrosoftTranslator/");

            // this is the Account Key I generated for this app
            string accountKey = bingAccountKey;

            // throw new Exception("Invalid Account Key");

            TranslatorContainer newTc = new TranslatorContainer(serviceRootUri);
            newTc.Credentials = new NetworkCredential(accountKey, accountKey);
            return newTc;
        }
        //bing 번역기로부터 번역문 얻기
        private static Translation TranslateString(TranslatorContainer tc, string inputString, string transCode, string resultCode)
        {
            System.Data.Services.Client.DataServiceQuery<MORT.TransForm.Translation> translationQuery = tc.Translate(inputString, resultCode, transCode);

            // Call the query and get the results as a List
            System.Collections.Generic.List<MORT.TransForm.Translation> translationResults = translationQuery.Execute().ToList();

            // Verify there was a result
            if (translationResults.Count() <= 0)
            {
                return null;
            }

            // In case there were multiple results, pick the first one
            Translation translationResult = translationResults.First();

            return translationResult;
        }

        #endregion


        //bing 번역기를 이용한 번역
        private void useBingTrans(string transText, string ocrText, bool isShowOCRResultFlag, bool isSaveOCRFlag)
        {
            //ocr 추출결과가 없으면 끝냄
            if (ocrText.CompareTo(formerOcrString) == 0)
            {
                return;
            }
            else
            {
                formerOcrString = ocrText;
            }


            try
            {
                string replaceOcrText = formerOcrString.Replace("\r\n", " ");
                Translation translationResult = TranslateString(tc, formerOcrString, transCode, resultCode);

                // Handle the error condition
                if (translationResult == null || translationResult.Text == "")
                {
                    transTextBox.Text = "";
                    return;
                }
                try
                {
                    string translationString = translationResult.Text.Replace("\n", "\r\n");
                    this.BeginInvoke(new myDelegate(updateProgress), new object[] { translationString, ocrText, isShowOCRResultFlag, isSaveOCRFlag });
                }
                catch (InvalidOperationException)
                {
                    // Error logging, post processing etc.
                    return;
                }
            }
            catch (InvalidOperationException)
            {
                this.BeginInvoke(new myDelegate(updateProgress), new object[] { "빙 번역기 사용 불가 - 잘못된 키 또는 남은 문자수 0 - 새로운 계정키를 넣으시기 바랍니다.", ocrText, isShowOCRResultFlag, isSaveOCRFlag }); ;
            }
        }
        //번역창에 번역문 출력
        private delegate void myDelegate(string transText, string ocrText, bool isShowOCRResultFlag, bool isSaveOCRFlag);
        private void updateProgress(string transText, string ocrText, bool isShowOCRResultFlag, bool isSaveOCRFlag)
        {           

            transTextBox.Text = transText;
            
            if (isShowOCRResultFlag == true)
            {
                transTextBox.Text += "\r\n" + "OCR : " + ocrText;
            }
            //만약 ocr 결과를 저장하기로 했으면
            if (isSaveOCRFlag == true)
            {
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

        //ocr 및 번역 결과 처리
        public void updateText(string transText, string ocrText, SettingManager.TransType transType, bool isShowOCRResultFlag, bool isSaveOCRFlag)
        {
            try
            {
                if (transType == SettingManager.TransType.bing)          //만약 빙 번역기를 사용한다면
                {
                    if (thread == null)             //현재 수행중인 번역이 없다면
                    {
                        thread = new Thread(delegate()  //쓰레드로 수행
                        {
                            useBingTrans(transText, ocrText, isShowOCRResultFlag, isSaveOCRFlag);
                        });

                        thread.Start();
                    }
                    else
                    {
                        if (thread.IsAlive == false)    //이미 빙 번역기 쓰레드가 수행중이라면 -> 조인하고 다시 수행
                        {
                            thread.Join();
                            thread = new Thread(delegate()
                            {
                                useBingTrans(transText, ocrText, isShowOCRResultFlag, isSaveOCRFlag);
                            });

                            thread.Start();
                        }
                    }
                }
                else
                {      //db를 이용한 번역       
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
            tc = InitializeTranslatorContainer();
        }

        public void setTopMostFlag(bool newTopMostFlag)
        {
            isTopMostFlag = newTopMostFlag;
            this.TopMost = isTopMostFlag;
        }
        private void closeApplication()
        {
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
