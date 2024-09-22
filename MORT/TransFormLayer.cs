using MORT.LocalizeManager;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;




namespace MORT
{
    public partial class TransFormLayer : Form, ITransform, ILocalizeForm
    {
        public int TaskIndex { get; private set; }
        public const int MIN_SIZE_X = 200;
        public const int MIN_SIZE_Y = 100;

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

        string resultText = "MORT 1.18dV\n레이어 번역창";
        byte alpha = 150;
        private Point mousePoint;
        StringFormat stringFormat = new StringFormat();
        bool _topMost = true;
        bool isDestroyFormFlag = false;
        bool isStart = false;


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;
        public TranslateStatusType TranslateStatusType { get; private set; }
        public bool UseTopMostOptionWhenTranslate { get; private set; }
        private string _warningMessage;
        private DateTime _dtWarningDisplayTime;
        int sizeX;
        int sizeY;
        private bool _aligenmntRight;
        private StringAlignment _alignemnt => _aligenmntRight ? StringAlignment.Far : StringAlignment.Near;


        public void AddText(string addText)
        {
            resultText = addText + System.Environment.NewLine + resultText;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void ForceUpdateText(string text)
        {
            resultText = text;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void StopTrans()
        {
            TranslateStatusType = TranslateStatusType.Stop;
            ApplyTopMost();
        }

        public void StartTrans()
        {
            TaskIndex++;
            if(TaskIndex > 100000)
            {
                TaskIndex = 0;
            }
            TranslateStatusType = TranslateStatusType.Translate;
            ApplyTopMost();
        }


        //번역창에 번역문 출력
        private delegate void myDelegate(string transText, string ocrText, bool isShowOCRResultFlag);
        private void updateProgress(string transText, string ocrText, bool isShowOCRResultFlag)
        {
            if(transText.CompareTo("not thing") == 0)
            {
                transText = "";
            }

            resultText = transText;
            if(isShowOCRResultFlag == true)
            {
                resultText += "\r\n\r\n" + "OCR : " + ocrText;
            }
        }

        //ocr 및 번역 결과 처리
        public void updateText(string transText, string ocrText, bool isShowOCRResultFlag)
        {
            if(AdvencedOptionManager.UseIgonoreEmptyTranslate && string.IsNullOrEmpty(ocrText))
            {
                return;
            }

            try
            {
                this.BeginInvoke(new myDelegate(updateProgress), new object[] { transText, ocrText, isShowOCRResultFlag });
            }
            catch(InvalidOperationException)
            {
                // Error logging, post processing etc.
                return;
            }
            this.BeginInvoke(new Action(UpdatePaint));
            //  UpdatePaint();
        }


        #region ::::::::::: 레이어 창 생성 ::::::::::

        private void Init()
        {
            _aligenmntRight = AdvencedOptionManager.LayerTextAlignmentRight;
            SetTextAlignmentBottom(AdvencedOptionManager.LayerTextAlignmentBottom);

            if(FormManager.Instace.MyMainForm.MySettingManager.NowSortType == SettingManager.SortType.Normal)
            {
                SortTypeBasicMenu.Checked = true;
                SortTypeCenterMenu.Checked = false;
                stringFormat.Alignment = _alignemnt;
            }
            else
            {
                SortTypeBasicMenu.Checked = false;
                SortTypeCenterMenu.Checked = true;
                stringFormat.Alignment = StringAlignment.Center;
            }

            removeMenu.Checked = FormManager.Instace.MyMainForm.MySettingManager.NowIsRemoveSpace;
            forceTransparencyMenu.Checked = FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency;
            ApplyRTL(AdvencedOptionManager.EnableRTL);
        }

        public void ApplyRTL(bool enableRTL)
        {
            if(enableRTL)
            {
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            else
            {
                stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
            }
        }

        private void SetTextAlignmentBottom(bool isBottom)
        {
            if(isBottom)
            {
                stringFormat.LineAlignment = StringAlignment.Far;
            }
            else
            {
                stringFormat.LineAlignment = StringAlignment.Near;
            }
        }

        private void SetTextAlignmentRight(bool isRight)
        {
            _aligenmntRight = isRight;
            stringFormat.Alignment = _alignemnt;
        }


        public void ApplyTextAlignmentBottom(bool isBottom)
        {
            SetTextAlignmentBottom(isBottom);
            if(InvokeRequired)
            {
                this.BeginInvoke(UpdatePaint);
            }
            else
            {
                UpdatePaint();
            }
        }

        public void ApplyTextAlignmentRight(bool isRight)
        {
            SetTextAlignmentRight(isRight);
            if(InvokeRequired)
            {
                this.BeginInvoke(UpdatePaint);
            }
            else
            {
                UpdatePaint();
            }
        }

        public TransFormLayer()
        {
            InitializeComponent();
            resultText = Properties.Settings.Default.LAYER_TEXT;
            resultText = string.Format(resultText, Properties.Settings.Default.MORT_VERSION);
            resultText = resultText + "\n\n[TIP]" + Util.GetToolTip();
            Init();
            LocalizeForm();
        }

        public void LocalizeForm()
        {
            sortMenu.LocalizeLabel("Context Sort Menu");
            SortTypeBasicMenu.LocalizeLabel("Context SortTypeBasicMenu");
            SortTypeCenterMenu.LocalizeLabel("Context SortTypeCenterMenu");
            removeMenu.LocalizeLabel("Context removeMenu");
            forceTransparencyMenu.LocalizeLabel("Context forceTransparencyMenu");
            CloseToolStripMenuItem.LocalizeLabel("Context CloseToolStripMenuItem");
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

        private void DoUpdatePaint()
        {
            // Get device contexts
            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            string displayMessage = resultText;
            if(!string.IsNullOrEmpty(_warningMessage))
            {
                if(DateTime.Now < _dtWarningDisplayTime)
                {
                    displayMessage = _warningMessage + displayMessage;
                }
                else
                {
                    _warningMessage = "";
                }
            }

            try
            {
                // Get handle to the new bitmap and select it into the current 
                // device context.

                Bitmap bitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using(Graphics gF = Graphics.FromImage(bitmap))
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
                using(GraphicsPath gp = new GraphicsPath())
                using(Pen outline = new Pen(OutlineForeColor, OutlineWidth) { LineJoin = LineJoin.Round })
                using(StringFormat sf = new StringFormat())
                using(Brush foreBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.TextColor))
                {

                    sf.Alignment = stringFormat.Alignment;
                    sf.LineAlignment = stringFormat.LineAlignment;
                    sf.FormatFlags = stringFormat.FormatFlags;
                    Color backgroundColor = Color.FromArgb(alpha, Color.AliceBlue);
                    g.Clear(backgroundColor);

                    Rectangle rectangle = ClientRectangle;
                    rectangle.X = 15;
                    rectangle.Y = 15;
                    rectangle.Width -= 15;
                    rectangle.Height -= 15;

                    if(isActiveGDI)
                    {
                        try
                        {
                            gp.AddString(displayMessage, textFont.FontFamily, (int)textFont.Style, g.DpiY * textFont.Size / 72, rectangle, sf);
                        }
                        catch(Exception ex)
                        {

                            //MessageBox.Show(ex.ToString());
                            TransFormLayer.isActiveGDI = false;
                            CustomLabel.isActiveGDI = false;
                            if(DialogResult.OK == MessageBox.Show("GDI+ 가 작동하지 않습니다. \n레이어 번역창의 일부 기능을 사용할 수 없습니다.\n해결법을 확인해 보겠습니까? ", "GDI+ 에서 일반 오류가 발생했습니다.", MessageBoxButtons.OKCancel))
                            {
                                try
                                {
                                    Util.OpenURL("https://blog.naver.com/killkimno/70185869419");
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

                            CharacterRange[] characterRanges = { new CharacterRange(0, displayMessage.Length) };

                            sf.SetMeasurableCharacterRanges(characterRanges);
                            Region[] stringRegions = g.MeasureCharacterRanges(displayMessage, textFont, rectangle, sf);
                            if(stringRegions.Length > 0)
                            {
                                // Draw rectangle for first measured range.
                                RectangleF measureRect1 = stringRegions[0].GetBounds(g);

                                SolidBrush backColorBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor);
                                g.FillRectangle(backColorBrush, measureRect1.X - 8, measureRect1.Y - 4, measureRect1.Width + 16, measureRect1.Height + 8);
                            }
                        }
                    }
                    else
                    {
                        using(Pen layerOutline = new Pen(Color.FromArgb(40, 134, 249), 3) { LineJoin = LineJoin.Round })
                            g.DrawRectangle(layerOutline, ClientRectangle);
                    }

                    g.SmoothingMode = SmoothingMode.HighQuality;

                    if(isActiveGDI)
                    {
                        using(Pen outline2 = new Pen(FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2, 5) { LineJoin = LineJoin.Round })
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

                if(this == null || this.IsDisposed || this.isDestroyFormFlag)
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
                if(hBitmap != IntPtr.Zero)
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

            //Util.ShowLog("end");
        }

        public void UpdatePaint()
        {
            if(this.InvokeRequired)
            {
                Action action = () => DoUpdatePaint();
                this.BeginInvoke(action);
            }
            else
            {
                DoUpdatePaint();
            }
        }

        enum dragMode { none, left, right, up, down, leftUp, rightUp, leftDown, rightDown };
        dragMode nowDragMode = dragMode.none;

        public void ApplyUseTopMostOptionWhenTranslate(bool useTopMostOptionWhenTranslate)
        {
            UseTopMostOptionWhenTranslate = useTopMostOptionWhenTranslate;
            ApplyTopMost();
        }

        public void SetTopMost(bool topMost, bool useTopMostOptionWhenTranslate)
        {
            UseTopMostOptionWhenTranslate = useTopMostOptionWhenTranslate;
            _topMost = topMost;

            ApplyTopMost();
        }

        public void ApplyTopMost()
        {

            Action callback = delegate
            {
                if(UseTopMostOptionWhenTranslate)
                {
                    if(TranslateStatusType == TranslateStatusType.Translate)
                    {
                        this.TopMost = _topMost;
                    }
                    else
                    {
                        //번역중이 아니면 끈다
                        this.TopMost = false;
                    }
                }
                else
                {
                    this.TopMost = _topMost;
                }

                if(this.TopMost)
                {
                    SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
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
            //더이상 안 씀
            this.Visible = false;
            return;
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
            if((e.X <= 30 && e.X >= 1) && (e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.leftUp;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1) && (e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.rightUp;
            }
            else if((e.X <= 30 && e.X >= 1) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.leftDown;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.rightDown;
            }
            else if((e.X <= 30 && e.X >= 1))
            {
                nowDragMode = dragMode.left;

            }
            else if(this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1)
            {
                nowDragMode = dragMode.right;
            }
            else if((e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.up;
            }
            else if(this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1)
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

            if((e.Button & MouseButtons.Right) == MouseButtons.Right || (e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                nowDragMode = dragMode.none;
            }
            if(nowDragMode == dragMode.none)
            {
                if((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                }
            }
            else
            {
                if(nowDragMode == dragMode.leftUp)
                {
                    int backupTop = this.Top;
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height + backupTop - this.Top);
                }
                else if(nowDragMode == dragMode.leftDown)
                {
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    this.Size = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height - (this.Size.Height - e.Y));
                }
                else if(nowDragMode == dragMode.rightUp)
                {
                    int backupTop = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height + backupTop - this.Top);
                }
                else if(nowDragMode == dragMode.rightDown)
                {
                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height - (this.Size.Height - e.Y));
                }
                else if(nowDragMode == dragMode.up)
                {
                    int backup = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width, this.Size.Height + backup - this.Top);
                }
                else if(nowDragMode == dragMode.down)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height - (this.Size.Height - e.Y));
                }
                else if(nowDragMode == dragMode.left)
                {
                    int backup = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    this.Size = new Size(this.Size.Width + backup - this.Left, this.Size.Height);
                }
                else if(nowDragMode == dragMode.right)
                {

                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height);
                }
            }

            if((e.X <= 30 && e.X >= 0) && (e.Y <= 30 && e.Y >= 0))
            {

                Cursor = Cursors.SizeNWSE;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0) && (e.Y <= 30 && e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if((e.X <= 30 && e.X >= 0) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNWSE;
            }
            else if((e.X <= 30 && e.X >= 0))
            {
                Cursor = Cursors.SizeWE;

            }
            else if(this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0)
            {
                Cursor = Cursors.SizeWE;
            }
            else if((e.Y <= 30 && e.Y >= 0))
            {
                Cursor = Cursors.SizeNS;
            }
            else if(this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0)
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
        public void SetInvisibleBackground()
        {
            isStart = true;
            alpha = 0;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void SetVisibleBackground()
        {
            isStart = false;
            alpha = 190;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void SetOverHitLayer()
        {
            int extendedStyle;
            extendedStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
        public void DisableOverHitLayer()
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
            if(this.Visible) this.Refresh();
            if(this.Size.Height <= MIN_SIZE_Y)
            {
                this.Size = new Size(this.Width, MIN_SIZE_Y);
            }
            if(this.Size.Width < MIN_SIZE_X)
            {
                this.Size = new Size(MIN_SIZE_X, this.Width);
            }
            sizeX = this.Size.Width;
            sizeY = this.Size.Height;

            if(InvokeRequired)
            {

                BeginInvoke(new Action(UpdatePaint));
            }
            else
            {
                UpdatePaint();
            }
            //this.BeginInvoke(new myDelegate2(resizeLayer), new object[] { this.Size.Width, this.Size.Height });
        }

        private void TransFormLayer_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenu.Show(this.PointToScreen(e.Location));
            }
        }

        private void TransFormLayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeApplication();
            if(isDestroyFormFlag == false)
            {
                e.Cancel = true;//종료를 취소하고 
            }
        }

        private void SortTypeBasicMenu_Click(object sender, EventArgs e)
        {
            SortTypeBasicMenu.Checked = true;
            SortTypeCenterMenu.Checked = false;
            FormManager.Instace.MyMainForm.SetTextSort(SettingManager.SortType.Normal);
            stringFormat.Alignment = _alignemnt;
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

        private void forceTransparencyMenu_Click(object sender, EventArgs e)
        {
            ForceTransparency();
        }

        public void ForceTransparency()
        {
            bool isForce = FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency;
            FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency = !isForce;

            forceTransparencyMenu.Checked = !isForce;
        }


        public void DoUpdate(bool isTranslating)
        {
            if(FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency)
            {
                SetOverHitLayer();
                SetInvisibleBackground();
            }
            else
            {
                if(!isTranslating)
                {
                    SetVisibleBackground();
                    DisableOverHitLayer();
                }
            }
        }

        public SettingManager.Skin GetSkinType()
        {
            return SettingManager.Skin.layer;
        }

        public void ApplyWarningMessage(string message, DateTime dtDisplayTime)
        {
            _warningMessage = message;
            _dtWarningDisplayTime = dtDisplayTime;
        }

        public void ClearWarningMessage()
        {
            _warningMessage = "";
        }

       
    }
}
