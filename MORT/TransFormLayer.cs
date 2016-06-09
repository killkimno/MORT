using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Data.Services.Client;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;




namespace MORT
{
    public partial class TransFormLayer : Form
    {

        #region Native Methods and Structures

        const Int32 WS_EX_LAYERED = 0x80000;
        const Int32 HTCAPTION = 0x02;
        const Int32 WM_NCHITTEST = 0x84;
        const Int32 ULW_ALPHA = 0x02;
        const byte AC_SRC_OVER = 0x00;
        const byte AC_SRC_ALPHA = 0x01;

        [StructLayout(LayoutKind.Sequential)]
        struct FramePoint
        {
            public Int32 x;
            public Int32 y;

            public FramePoint(Int32 x, Int32 y)
            { this.x = x; this.y = y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FrameSize
        {
            public Int32 cx;
            public Int32 cy;

            public FrameSize(Int32 cx, Int32 cy)
            { this.cx = cx; this.cy = cy; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
            ref FramePoint pptDst, ref FrameSize psize, IntPtr hdcSrc, ref FramePoint pprSrc,
            Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        #endregion
    
        public static bool isActiveGDI = true;
        public Thread thread;  //빙 번역기 처리 쓰레드
        static TranslatorContainer tc;
        static string bingAccountKey;
        private string transCode = "en";
        private string resultCode = "ko";
        string formerOcrString;
        SettingManager.TransType formerTransType = SettingManager.TransType.db;
        string resultText = "MORT 1.15dV\n레이어 번역창";
        byte alpha = 150;
        private Point mousePoint;
        StringFormat stringFormat = new StringFormat();
        bool isTopMostFlag = true;
        bool isDestroyFormFlag = false;
        bool isStart = false;


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        int sizeX;
        int sizeY;

    
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
            
            System.Data.Services.Client.DataServiceQuery<MORT.TransFormLayer.Translation> translationQuery = tc.Translate(inputString, resultCode, transCode);

            // Call the query and get the results as a List
            System.Collections.Generic.List<MORT.TransFormLayer.Translation> translationResults = translationQuery.Execute().ToList();

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
                    resultText = "";
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

        //Naver 번역기를 이용한 번역
        private void UseNaverTrans(string transText, string ocrText, bool isShowOCRResultFlag, bool isSaveOCRFlag)
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
                string result = NaverTranslateAPI.instance.GetResult(ocrText);

                // Handle the error condition
                if (result == "")
                {
                    resultText = "";
                    return;
                }
                try
                {
                    string translationString = result.Replace("\n", "\r\n");
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

            if (transText.CompareTo("not thing") == 0)
            {
                transText = "";
            }

            resultText = transText;
            if (isShowOCRResultFlag == true)
            {
                resultText += "\r\n" + "OCR : " + ocrText;
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
            if(formerTransType != transType)
            {
                formerOcrString = "";
            }
            formerTransType = transType;

            if (transType == SettingManager.TransType.bing || transType == SettingManager.TransType.naver)          //만약 빙 번역기를 사용한다면
            {
                if (thread == null)             //현재 수행중인 번역이 없다면
                {
                    thread = new Thread(delegate()  //쓰레드로 수행
                    {
                        if (transType == SettingManager.TransType.naver)
                            UseNaverTrans(transText, ocrText, isShowOCRResultFlag, isSaveOCRFlag);
                        if(transType == SettingManager.TransType.bing )
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
                            if (transType == SettingManager.TransType.naver)
                                UseNaverTrans(transText, ocrText, isShowOCRResultFlag, isSaveOCRFlag);
                            if (transType == SettingManager.TransType.bing)
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

            this.BeginInvoke(new Action(UpdatePaint));
          //  UpdatePaint();
        }


        #region ::::::::::: 레이어 창 생성 ::::::::::

        private void Init()
        {

            if (FormManager.Instace.MyMainForm.MySettingManager.NowSortType == SettingManager.SortType.Normal)
            {
                SortTypeBasicMenu.Checked = true;
                SortTypeCenterMenu.Checked = false;
                stringFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                SortTypeBasicMenu.Checked = false;
                SortTypeCenterMenu.Checked = true;
                stringFormat.Alignment = StringAlignment.Center;
            }
            
            removeMenu.Checked = FormManager.Instace.MyMainForm.MySettingManager.NowIsRemoveSpace;
        }

        public TransFormLayer()
        {
            InitializeComponent();
            Init();
            tc = InitializeTranslatorContainer();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Add the layered extended style (WS_EX_LAYERED) to this window.
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_LAYERED;
                return createParams;
            }
        }


        #endregion

        public void UpdateTransform()
        {
            Init();
            UpdatePaint();
        }

        private void UpdatePaint()
        {
            // Get device contexts
            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                // Get handle to the new bitmap and select it into the current 
                // device context.

                Bitmap bitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                
                using (Graphics gF = Graphics.FromImage(bitmap))
                {
                    SolidBrush brush = new SolidBrush(Color.FromArgb(0, 240, 248, 255));
                    gF.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                }

                Font textFont = FormManager.Instace.MyMainForm.MySettingManager.TextFont;

                // Set parameters for layered window update.
                FrameSize newSize = new FrameSize(bitmap.Width, bitmap.Height);
                FramePoint sourceLocation = new FramePoint(0, 0);
                FramePoint newLocation = new FramePoint(this.Left, this.Top);
                BLENDFUNCTION blend = new BLENDFUNCTION();
                blend.BlendOp = AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = AC_SRC_ALPHA;

                Graphics g = Graphics.FromImage(bitmap);
                Color OutlineForeColor = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor1;
                float OutlineWidth = 2;
                using (GraphicsPath gp = new GraphicsPath())
                using (Pen outline = new Pen(OutlineForeColor, OutlineWidth) { LineJoin = LineJoin.Round })
                using (StringFormat sf = new StringFormat())
                using (Brush foreBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.TextColor))
                {
                   
                    sf.Alignment = stringFormat.Alignment;
                    Color backgroundColor = Color.FromArgb(alpha, Color.AliceBlue);
                    g.Clear(backgroundColor);

                    Rectangle rectangle = ClientRectangle;
                    rectangle.X = 15;
                    rectangle.Y = 15;
                    rectangle.Width -= 15;
                    rectangle.Height -= 15;

                    if (isActiveGDI)
                    {
                        try
                        {
                            gp.AddString(resultText, textFont.FontFamily, (int)textFont.Style, g.DpiY * textFont.Size / 72, rectangle, sf);
                        }
                        catch (Exception ex)
                        {
                            
                            //MessageBox.Show(ex.ToString());
                            TransFormLayer.isActiveGDI = false;
                            CustomLabel.isActiveGDI = false;
                            if (DialogResult.OK == MessageBox.Show("GDI+ 가 작동하지 않습니다. \n레이어 번역창의 일부 기능을 사용할 수 없습니다.\n해결법을 확인해 보겠습니까? ", "GDI+ 에서 일반 오류가 발생했습니다.", MessageBoxButtons.OKCancel))
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start("http://killkimno.blog.me/70185869419");
                                }
                                catch { }
                            }
                        }
                       
                    }
                     
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;                   


                    if(isStart)
                    {
                        if(FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor)
                        {

                            CharacterRange[] characterRanges = { new CharacterRange(0, resultText.Length) };

                            sf.SetMeasurableCharacterRanges(characterRanges);
                            Region[] stringRegions = g.MeasureCharacterRanges(resultText, textFont, rectangle, sf);
                            if (stringRegions.Length > 0)
                            {
                                // Draw rectangle for first measured range.
                                RectangleF measureRect1 = stringRegions[0].GetBounds(g);

                                SolidBrush backColorBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor);
                                g.FillRectangle(backColorBrush, measureRect1.X, measureRect1.Y, measureRect1.Width, measureRect1.Height);
                            }

                        }

                    }
                    else
                    {
                        using (Pen layerOutline = new Pen(Color.FromArgb(40,134,249), 3) { LineJoin = LineJoin.Round })
                            g.DrawRectangle(layerOutline, ClientRectangle);
                    }

                    g.SmoothingMode = SmoothingMode.HighQuality;

                    if(isActiveGDI)
                    {
                        using (Pen outline2 = new Pen(FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2, 5) { LineJoin = LineJoin.Round })
                            g.DrawPath(outline2, gp);
                        g.DrawPath(outline, gp);
                        g.FillPath(foreBrush, gp);
                    }
                    else
                    {
                        g.DrawString(resultText, textFont, foreBrush, rectangle);
                    }

                }


                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  //Set the fact that background is transparent
                hOldBitmap = SelectObject(memDc, hBitmap);


                // Update the window.
                
                UpdateLayeredWindow(
                    this.Handle,     // Handle to the layered window
                    screenDc,        // Handle to the screen DC
                    ref newLocation, // New screen position of the layered window
                    ref newSize,     // New size of the layered window
                    memDc,           // Handle to the layered window surface DC
                    ref sourceLocation, // Location of the layer in the DC
                    0,               // Color key of the layered window
                    ref blend,       // Transparency of the layered window
                    ULW_ALPHA        // Use blend as the blend function
                    );
                  
            }
            finally
            {
                // Release device context.
                ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, hOldBitmap);
                    DeleteObject(hBitmap);
                }
                DeleteDC(memDc);
                GC.Collect();

                /*
            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;
            */
            }
        }

        
        enum dragMode { none, left, right, up, down, leftUp, rightUp, leftDown, rightDown };
        dragMode nowDragMode = dragMode.none;
          

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
            FormManager.Instace.MyLayerTransForm = null;
            this.Close();
        }




        #region:::::::::::::::::::::::::::::::::::::::::::레이어 창 이동 관련:::::::::::::::::::::::::::::::::::::::::::


        private void TransForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.X <= 30 && e.X >= 1) && (e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.leftUp;
            }
            else if ((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1) && (e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.rightUp;
            }
            else if ((e.X <= 30 && e.X >= 1) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.leftDown;
            }
            else if ((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.rightDown;
            }
            else if ((e.X <= 30 && e.X >= 1))
            {
                nowDragMode = dragMode.left;

            }
            else if (this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1)
            {
                nowDragMode = dragMode.right;
            }
            else if ((e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.up;
            }
            else if (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1)
            {
                nowDragMode = dragMode.down;
            }
            else
            {
                nowDragMode = dragMode.none;
            }

            mousePoint = new Point(e.X, e.Y);
        }

        private void TransForm_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.Button & MouseButtons.Right) == MouseButtons.Right || (e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                nowDragMode = dragMode.none;
            }
            if (nowDragMode == dragMode.none)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                }
            }
            else
            {
                if (nowDragMode == dragMode.leftUp)
                {
                    int backupTop = this.Top;
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height + backupTop - this.Top);
                }
                else if (nowDragMode == dragMode.leftDown)
                {
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    this.Size = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height - (this.Size.Height - e.Y));
                }
                else if (nowDragMode == dragMode.rightUp)
                {
                    int backupTop = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height + backupTop - this.Top);
                }
                else if (nowDragMode == dragMode.rightDown)
                {
                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height - (this.Size.Height - e.Y));
                }
                else if (nowDragMode == dragMode.up)
                {
                    int backup = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width, this.Size.Height + backup - this.Top);
                }
                else if (nowDragMode == dragMode.down)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height - (this.Size.Height - e.Y));
                }
                else if (nowDragMode == dragMode.left)
                {
                    int backup = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    this.Size = new Size(this.Size.Width + backup - this.Left, this.Size.Height);
                }
                else if (nowDragMode == dragMode.right)
                {

                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height);
                }
            }

            if ((e.X <= 30 && e.X >= 0) && (e.Y <= 30 && e.Y >= 0))
            {

                Cursor = Cursors.SizeNWSE;
            }
            else if ((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0) && (e.Y <= 30 && e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if ((e.X <= 30 && e.X >= 0) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if ((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNWSE;
            }
            else if ((e.X <= 30 && e.X >= 0))
            {
                Cursor = Cursors.SizeWE;

            }
            else if (this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0)
            {
                Cursor = Cursors.SizeWE;
            }
            else if ((e.Y <= 30 && e.Y >= 0))
            {
                Cursor = Cursors.SizeNS;
            }
            else if (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0)
            {
                Cursor = Cursors.SizeNS;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
        private void TransForm_MouseUp(object sender, MouseEventArgs e)
        {
            nowDragMode = dragMode.none;
        }
        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::레이어 색및 클릭 관련:::::::::::::::::::::::::::::::::::::::::::
        public void setInvisibleBackground()
        {
            isStart = true;
            alpha = 0;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void setVisibleBackground()
        {
            isStart = false;
            alpha = 190;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void setOverHitLayer()
        {
            int extendedStyle;
            extendedStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
        public void disableOverHitLayer()
        {
            int extendedStyle;
            extendedStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
        }

        #endregion
        
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            closeApplication();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void TransFormLayer_Resize(object sender, EventArgs e)
        {
            if (this.Visible) this.Refresh();
            if (this.Size.Height <= 50)
            {
                this.Size = new Size(this.Width, 50);
            }
            if (this.Size.Width < 150)
            {
                this.Size = new Size(150, this.Width);
            }
            sizeX = this.Size.Width;
            sizeY = this.Size.Height;
            this.BeginInvoke(new Action(UpdatePaint));
            //this.BeginInvoke(new myDelegate2(resizeLayer), new object[] { this.Size.Width, this.Size.Height });
        }

        private void TransFormLayer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenu.Show(this.PointToScreen(e.Location));
            }
        }

        private void TransFormLayer_FormClosing(object sender, FormClosingEventArgs e)
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

        private void SortTypeBasicMenu_Click(object sender, EventArgs e)
        {
            SortTypeBasicMenu.Checked = true;
            SortTypeCenterMenu.Checked = false;
            FormManager.Instace.MyMainForm.SetTextSort(SettingManager.SortType.Normal);
            stringFormat.Alignment = StringAlignment.Near;
            this.BeginInvoke(new Action(UpdatePaint));
            //transTextLayer.TextAlign = ContentAlignment.TopLeft;
        }

        private void SortTypeCenterMenu_Click(object sender, EventArgs e)
        {
            SortTypeBasicMenu.Checked = false;
            SortTypeCenterMenu.Checked = true;
            FormManager.Instace.MyMainForm.SetTextSort(SettingManager.SortType.Center);
            stringFormat.Alignment = StringAlignment.Center;
            this.BeginInvoke(new Action(UpdatePaint));
            //transTextLayer.TextAlign = ContentAlignment.TopCenter;
        }

        private void removeMenu_Click(object sender, EventArgs e)
        {
            removeMenu.Checked = !removeMenu.Checked;
            FormManager.Instace.MyMainForm.SetIsRemoveSpace(removeMenu.Checked);

        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
