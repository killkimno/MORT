using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public partial class TransFormOver : Form, ITransform
    {
        public int TaskIndex { get; private set; }
        public int makeIndex = 0;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);



        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

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


        string resultText = "MORT 1.161V\n레이어 번역창";
        byte alpha = 0;
        private Point mousePoint;
        StringFormat stringFormat = new StringFormat();
        bool isTopMostFlag = true;
        bool isDestroyFormFlag = false;
        bool isStart = false;

        private int adjustX = 0;
        private int adjustY = 0;
        private bool _enableRTL;


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        public TranslateStatusType TranslateStatusType { get; private set; }
        public bool UseTopMostOptionWhenTranslate { get; private set; }

        int sizeX;
        int sizeY;

        private List<OCRDataManager.ResultData> dataList = null;
        private int clientPositionX = 0;
        private int clientPositionY = 0;
        Bitmap bitmap = null;


        //번역창에 번역문 출력
        private delegate void myDelegate(string transText, string ocrText, bool isShowOCRResultFlag);
        private void updateProgress(string transText, string ocrText, bool isShowOCRResultFlag)
        {

            if (transText.CompareTo("not thing") == 0)
            {
                transText = "";
            }

            Util.ShowLog(transText + " / " + ocrText);
            resultText = transText;
            if (isShowOCRResultFlag == true)
            {
                resultText += "\r\n" + "OCR : " + ocrText;
            }
        }

        public void UpdateText(List<OCRDataManager.ResultData> dataList, bool isShowOCRResultFlag, int positionX , int positionY)
        {
            this.clientPositionX = positionX;
            this.clientPositionY = positionY;
            this.dataList = dataList;
            Util.CheckTimeSpan(true);
            try
            {
                string transText = "";
                string ocrText = "";

                if(dataList != null)
                {
                    for(int i = 0; i < dataList.Count; i++)
                    {
                        ocrText += dataList[i].GetOCR();
                        transText += dataList[i].GetTrans();
                    }
                }
                this.BeginInvoke(new myDelegate(updateProgress), new object[] { transText, ocrText, isShowOCRResultFlag});
            }
            catch (InvalidOperationException)
            {
                // Error logging, post processing etc.
                return;
            }
            this.BeginInvoke(new Action(UpdatePaint));

            Util.CheckTimeSpan(false);

            //  UpdatePaint();
        }


        #region ::::::::::: 레이어 창 생성 ::::::::::

        private void Init()
        {

            if (FormManager.Instace.MyMainForm.MySettingManager.NowSortType == SettingManager.SortType.Normal)
            {
                stringFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                stringFormat.Alignment = StringAlignment.Center;
            }

            ApplyRTL(AdvencedOptionManager.EnableRTL);
        }

        public TransFormOver()
        {
            InitializeComponent();

            Init();

        }

        public void SetAdjustPosition(int x, int y)
        {
            this.adjustX = x;
            this.adjustY = y;
        }

        public void CheckSizeAndLocation()
        {
            //스크린 캡쳐 아래아로 해야 함.
            Rectangle rect = Rectangle.Empty;
            if (FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect != Rectangle.Empty)
            {
                rect = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect;
            }
            else
            {
                rect = FormManager.Instace.MyMainForm.MySettingManager.GetCaptureFullArea();
            }
      

            rect.Width = (int)(rect.Width * 1.3);
            rect.Height = (int)(rect.Height * 1.3);

            this.Size = rect.Size;
            this.Location = rect.Location;
        }

        public void HideTaksBar()
        {

            ShowWindow((int)this.Handle, 0);
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

        private void AddText(GraphicsPath gp, Graphics g, Font textFont, Rectangle rectangleOriginal, StringFormat sf)
        {
            if(!isActiveGDI)
            {
                return;
            }

            Rectangle rectangle = rectangleOriginal;
            SolidBrush backColorBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor);
            SolidBrush defualtColorBrush = new SolidBrush(Color.FromArgb(90, 0, 0, 0) );

            //ocr 영역 가져옴.
            //TODO : 현재 그냥 임시 땜빵임.

            if (dataList != null)
            {
                for(int i = 0; i < dataList.Count; i++)
                {
                    int x = 0;
                    int y = 0;

                    if(dataList[i].SnapShot)
                    {
                        x = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect.X;
                        y = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect.Y;
                    }
                    else
                    {
                        x = FormManager.Instace.MyMainForm.MySettingManager.GetLocationX(dataList[i].index);
                        y = FormManager.Instace.MyMainForm.MySettingManager.GetLocationY(dataList[i].index);
                    }


                    y = y - FormManager.BorderHeight / 2;
                    x = x - FormManager.BorderWidth / 2;

                    Util.ShowLog($"{x} / {y}");

                    if (x < clientPositionX)
                    {
                        x = clientPositionX;
                    }

                    if (y < clientPositionY)
                    {
                        y = clientPositionY;
                    }

                    var targetData = dataList[i];
                    for (int j = 0; j < targetData.transDataList.Count; j++)
                    {
                        var transData = targetData.transDataList[j];
                     
                        //todo : 바꿔야 함
                        rectangle.X = x + (int)(transData.lineRect.X / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize)  - this.Location.X;
                        rectangle.Y = y + (int)(transData.lineRect.Y / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) -this.Location.Y ;
                        rectangle.Height = (int)(transData.lineRect.Height / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
                        rectangle.Width = (int)(transData.lineRect.Width / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);

                        //Util.ShowLog(rectangle.X + " /" + rectangle.Y + " / " + this.Location.X + " / " + this.Location.Y + " / adjust : " + adjustX + " / " + adjustY);
                        try
                        {
                            Rectangle textRect = Screen.PrimaryScreen.Bounds;

                            if (transData.angleType == OCRDataManager.WordAngleType.Vertical)
                            {
                                //sf.LineAlignment = StringAlignment.Far;
                                sf.FormatFlags |= StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
                                textRect.Height = rectangle.Height;
                            }
                            else
                            {
                                //sf.FormatFlags = new StringFormatFlags();
                                sf.FormatFlags &= ~(StringFormatFlags.DirectionVertical);

                                if(_enableRTL)
                                {
                                    sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                                }
                                else
                                {
                                    sf.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
                                }

                                textRect.Width = rectangle.Width;
                            }


                            if (AdvencedOptionManager.IsAutoFontSize)
                            {
                                float fontSize = OCRDataManager.GetFontSize(transData.lineDataList[0]) / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize / 2;
                                fontSize++;
                                fontSize = AdvencedOptionManager.GetResultAutoFontSize(fontSize);

                                textFont = new Font(textFont.FontFamily, fontSize);
                            }

                            //-------------------------


                            CharacterRange[] characterRanges = { new CharacterRange(0, transData.trans.Length) };
                            sf.SetMeasurableCharacterRanges(characterRanges);
                            Region[] stringRegions = g.MeasureCharacterRanges(transData.trans, textFont, textRect, sf);
                            if (stringRegions.Length > 0)
                            {
                                // Draw rectangle for first measured range.
                                RectangleF measureRect1 = stringRegions[0].GetBounds(g);

                                if (transData.angleType == OCRDataManager.WordAngleType.Vertical)
                                {
                                    if (rectangle.Width < measureRect1.Width)
                                    {
                                        rectangle.Width = (int)measureRect1.Width;
                                    }
                                }
                                else
                                {
                                    if (rectangle.Height < measureRect1.Height)
                                    {
                                        rectangle.Width = rectangle.Width + (int)(rectangle.Width * 0.15);
                                        stringRegions = g.MeasureCharacterRanges(transData.trans, textFont, rectangle, sf);
                                        if (stringRegions.Length > 0)
                                        {
                                            measureRect1 = stringRegions[0].GetBounds(g);
                                            rectangle.Height = (int)measureRect1.Height;
                                        }

                                    }
                                }

                            }

                            //--------

                            //결과가 한 줄일때만 처리한다
                            if(transData.lineDataList.Count == 1)
                            {
                                var size = g.MeasureString(transData.trans, textFont);
                                if (rectangle.Width < (int)size.Width && transData.angleType == OCRDataManager.WordAngleType.Horizontal)
                                {
                                    rectangle.Width = (int)size.Width + 1;
                                }

                                if (rectangle.Height < (int)size.Height && transData.angleType == OCRDataManager.WordAngleType.Vertical)
                                {
                                    rectangle.Height = (int)size.Height + 1;
                                }
                            }

                            gp.AddString(transData.trans, textFont.FontFamily, (int)textFont.Style, g.DpiY * textFont.Size / 72, rectangle, sf);


                            if (isStart)
                            {

                                if (Form1.IsDebugShowWordArea)
                                {
                                    for (int z = 0; z < transData.lineDataList.Count; z++)
                                    {
                                        Rectangle ocrRect = transData.lineDataList[z].lineRect;
                                        ocrRect.X = x + (int)(ocrRect.X / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.X;
                                        ocrRect.Y = y + (int)(ocrRect.Y / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.Y;
                                        ocrRect.Height = (int)(ocrRect.Height / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) + 5;
                                        ocrRect.Width = (int)(ocrRect.Width / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) + 5;

                                        g.FillRectangle(defualtColorBrush, ocrRect.X, ocrRect.Y, ocrRect.Width, ocrRect.Height);
                                    }
                                }
                                else if (FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor)
                                {
                                    //rectangle = Rectangle.Union(rectangle, dataList[i].transDataList[j].lineRect);
                                    rectangle.Height += 10;
                                    rectangle.Width += 10;
                                    //원문                                
                                    RectangleF measureRect1 = rectangle;
                                    g.FillRectangle(backColorBrush, measureRect1.X + 0, measureRect1.Y + 0, measureRect1.Width, measureRect1.Height);

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Util.ShowLog(ex.Message);
                        }
                    }
                 
                }
            }
        }

        private bool isLockPaint = false;

        public void UpdatePaint()
        {
            if (this.InvokeRequired)
            {
                Action action = () => DoUpdatePaint();
                this.BeginInvoke(action);
            }
            else
            {
                DoUpdatePaint();
            }
        }

        private void DoUpdatePaint()
        {
            if(isLockPaint)
            {
                Util.ShowLog("Lock Paint!!!!");
                return;
            }

            isLockPaint = true;
            CheckSizeAndLocation();
            Util.ShowLog("Update paint + " + makeIndex);

            // Get device contexts
            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

          
            try
            {
              
                if(bitmap == null || bitmap.Width != this.Width || bitmap.Height != Height)
                {
                    bitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }

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
                    sf.FormatFlags = stringFormat.FormatFlags;
                    Color backgroundColor = Color.FromArgb(alpha, Color.Red);
                    g.Clear(backgroundColor);

                    Rectangle rectangle = ClientRectangle;
                    rectangle.X = this.Location.X;
                    rectangle.Y = this.Location.Y;


                    AddText(gp, g, textFont, rectangle, sf);


                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    if (!isStart)
                    {
                        using (Pen layerOutline = new Pen(Color.FromArgb(40, 134, 249), 3) { LineJoin = LineJoin.Round })
                            g.DrawRectangle(layerOutline, ClientRectangle);

                    }

                    g.SmoothingMode = SmoothingMode.HighQuality;

                    if (isActiveGDI)
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

                if (!isStart)
                {
                    g.Clear(Color.FromArgb(0));
                }

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  //Set the fact that background is transparent
                hOldBitmap = SelectObject(memDc, hBitmap);


                // Update the window.

                if (this == null || this.IsDisposed || this.isDestroyFormFlag)
                {
                    return;
                }

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
                //SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

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
            }

            isLockPaint = false;
        }


        enum dragMode { none, left, right, up, down, leftUp, rightUp, leftDown, rightDown };
        dragMode nowDragMode = dragMode.none;

        public void ApplyUseTopMostOptionWhenTranslate(bool useTopMostOptionWhenTranslate)
        {
            UseTopMostOptionWhenTranslate = useTopMostOptionWhenTranslate;
            //CheckTopMostOption();
        }

        public void ApplyRTL(bool enableRTL)
        {
            _enableRTL = enableRTL;
            if (enableRTL)
            {
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            else
            {
                stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
            }
        }

        public void SetTopMost(bool topMost, bool useTopMostOptionWhenTranslate)
        {
            //무조건 탑 모스트다
            isTopMostFlag = true;
            this.TopMost = isTopMostFlag;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        public void StartTrans()
        {
            TaskIndex++;
            if (TaskIndex > 100000)
            {
                TaskIndex = 0;
            }

            TranslateStatusType = TranslateStatusType.Translate;
            dataList = new List<OCRDataManager.ResultData>();
        }

        public void StopTrans()
        {
            TranslateStatusType = TranslateStatusType.Stop;
        }

        public void ApplyTopMost()
        {
            this.TopMost = true;
        }

        public void destroyForm()
        {
            isDestroyFormFlag = true;
            FormManager.Instace.MyOverTransForm = null;
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
            isLockPaint = false;
            isStart = true;
            alpha = 0;     //0이어야 함
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void setVisibleBackground()
        {
            isLockPaint = false;
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

        public async void VisibleOverlayTransAsync(int waitTime)
        {
            int taskIndex = TaskIndex;
            await Task.Delay(waitTime * 1000);

            if(!this.IsDisposed && taskIndex == TaskIndex)
            {
                setVisibleBackground();
                disableOverHitLayer();
            }    

        }

        #endregion





        #region ::::::::: 인터페이스 관련 :::::::::::
        public void ForceTransparency()
        {
           
        }


        public void DoUpdate(bool isTranslating)
        {

        }

        public SettingManager.Skin GetSkinType()
        {
            return SettingManager.Skin.over;
        }

        public void ForceUpdateText(string text)
        {

        }

        public void ApplyWarningMessage(string message, DateTime dtRemainTime)
        {
            //따로 처리 안 한다
        }


        public void ClearWarningMessage()
        {
            //따로 처리 안 한다
        }

        #endregion




    }
}
