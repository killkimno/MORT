using R3;
using System;
using MORT.Service.Overlay;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public partial class TransFormOver : Form, ITransform
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        public static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint affinity);
        const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;
        const uint WDA_NONE = 0x00;

        // 키보드 후킹을 위한 변수
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        #region Win32 Keyboard Hook Setup

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using(var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using(var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(13, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID); // 프로그램 종료 시 후킹 해제
            base.OnFormClosing(e);
        }
        #endregion



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
        bool _isStart = false;
        private bool _attchedCapture;

        private int adjustX = 0;
        private int adjustY = 0;
        private bool _enableRTL;


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TranslateStatusType TranslateStatusType { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseTopMostOptionWhenTranslate { get; private set; }

        int sizeX;
        int sizeY;

        private List<OCRDataManager.ResultData> _dataList = null;
        private int clientPositionX = 0;
        private int clientPositionY = 0;
        private Rectangle _lastDisplayRect = Rectangle.Empty;
        private readonly Dictionary<int, OverlayDataCache> _overlayDataCache = new Dictionary<int, OverlayDataCache>();
        Bitmap bitmap = null;

        private readonly IDisposable _disposable;

        private class OverlayDataCache
        {
            public readonly Rectangle AreaRect;
            public readonly int ClientPositionX;
            public readonly int ClientPositionY;
            public readonly string OcrText;
            public readonly string TransText;
            public readonly OCRDataManager.ResultData Data;

            public OverlayDataCache(Rectangle areaRect, int clientPositionX, int clientPositionY, string ocrText, string transText, OCRDataManager.ResultData data)
            {
                AreaRect = areaRect;
                ClientPositionX = clientPositionX;
                ClientPositionY = clientPositionY;
                OcrText = ocrText;
                TransText = transText;
                Data = data;
            }
        }


        //번역창에 번역문 출력
        private delegate void myDelegate(string transText, string ocrText, bool isShowOCRResultFlag);
        private void updateProgress(string transText, string ocrText, bool isShowOCRResultFlag)
        {

            if(transText.CompareTo("not thing") == 0)
            {
                transText = "";
            }

            Util.ShowLog(transText + " / " + ocrText);
            resultText = transText;
            if(isShowOCRResultFlag == true)
            {
                resultText += "\r\n" + "OCR : " + ocrText;
            }
        }

        public void UpdateText(List<OCRDataManager.ResultData> dataList, bool isShowOCRResultFlag, int positionX, int positionY)
        {
            this.clientPositionX = positionX;
            this.clientPositionY = positionY;

            if(dataList != null)
            {
                _dataList = GetStableOverlayDataList(dataList);
            }
            else
            {
                _dataList = null;
                _overlayDataCache.Clear();
            }

            Util.CheckTimeSpan(true);
            try
            {
                string transText = "";
                string ocrText = "";

                if(_dataList != null)
                {
                    for(int i = 0; i < _dataList.Count; i++)
                    {
                        ocrText += _dataList[i].GetOCR();
                        transText += _dataList[i].GetTrans();
                    }
                }
                this.BeginInvoke(new myDelegate(updateProgress), new object[] { transText, ocrText, isShowOCRResultFlag });
            }
            catch(InvalidOperationException)
            {
                // Error logging, post processing etc.
                return;
            }
            this.BeginInvoke(new Action(UpdatePaint));

            Util.CheckTimeSpan(false);

            //  UpdatePaint();
        }

        private List<OCRDataManager.ResultData> GetStableOverlayDataList(List<OCRDataManager.ResultData> dataList)
        {
            List<OCRDataManager.ResultData> stableList = new List<OCRDataManager.ResultData>();
            HashSet<int> currentIndexSet = new HashSet<int>();

            foreach(var data in dataList)
            {
                currentIndexSet.Add(data.Index);

                Rectangle areaRect = GetAreaRect(data);
                string ocrText = data.GetOCR();
                string transText = data.TransString;

                if(_overlayDataCache.TryGetValue(data.Index, out var cache)
                    && cache.AreaRect == areaRect
                    && cache.ClientPositionX == clientPositionX
                    && cache.ClientPositionY == clientPositionY
                    && cache.OcrText == ocrText
                    && cache.TransText == transText)
                {
                    cache.Data.ReplaceAutoColor(data);
                    stableList.Add(cache.Data);
                    continue;
                }

                _overlayDataCache[data.Index] = new OverlayDataCache(areaRect, clientPositionX, clientPositionY, ocrText, transText, data);
                stableList.Add(data);
            }

            foreach(var key in _overlayDataCache.Keys.ToList())
            {
                if(!currentIndexSet.Contains(key))
                {
                    _overlayDataCache.Remove(key);
                }
            }

            return stableList;
        }

        private Rectangle GetAreaRect(OCRDataManager.ResultData data)
        {
            if(data.SnapShot)
            {
                return FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect;
            }

            return FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(data.Index);
        }


        #region ::::::::::: 레이어 창 생성 ::::::::::

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // 이 창(오버레이)을 모든 스크린 캡처 도구에서 보이지 않게 설정합니다.
            // 이제 캡처 시 창을 Hide/Show 하지 않아도 됩니다.
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);

            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }


        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0 && (wParam == (IntPtr)0x0100 || wParam == (IntPtr)0x0104)) // WM_KEYDOWN 또는 WM_SYSKEYDOWN
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                // 1. PrintScreen 키 (단일 키)
                bool isPrtSc = (key == Keys.PrintScreen);

                // 2. Win + Shift + S 감지 (조합 키)
                // S 키가 눌렸을 때, Win 키와 Shift 키가 현재 눌려있는지 상태 확인
                bool isWinShiftS = (key == Keys.S &&
                                   (GetAsyncKeyState((int)Keys.LWin) < 0 || GetAsyncKeyState((int)Keys.RWin) < 0) &&
                                   (GetAsyncKeyState((int)Keys.ShiftKey) < 0));

                if(!_attchedCapture && !_forceLock && (isPrtSc || isWinShiftS))
                {
                    _forceLock = true;
                    // 캡처에 포함되도록 즉시 변경
                    SetWindowDisplayAffinity(this.Handle, WDA_NONE);

                    // 1초 후 자동 복구
                    var timer = new System.Windows.Forms.Timer { Interval = 5000 };
                    timer.Tick += (s, e) =>
                    {
                        _forceLock = false;
                        if(!_attchedCapture)
                        {
                            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
                        }
                       
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void Init()
        {

            if(FormManager.Instace.MyMainForm.MySettingManager.NowSortType == SettingManager.SortType.Normal)
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

            _disposable = SettingManager.AttachedCapture.Subscribe(ApplyAttachedCapture);
        }

        protected override void OnClosed(EventArgs e)
        {
            _disposable.Dispose();
            base.OnClosed(e);
        }

        private void ApplyAttachedCapture(bool attached)
        {
            _attchedCapture = attached;
            if(_attchedCapture)
            {
                SetWindowDisplayAffinity(this.Handle, WDA_NONE);
            }
            else
            {
                SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
            }
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
            if(FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect != Rectangle.Empty)
            {
                rect = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect;
            }
            else
            {
                rect = FormManager.Instace.MyMainForm.GetOcrAreaProcessFullRect();
            }


            rect.Width = (int)(rect.Width * 1.3);
            rect.Height = (int)(rect.Height * 1.3);

            if(_isStart)
            {
                if(_lastDisplayRect != Rectangle.Empty && _lastDisplayRect.Contains(rect))
                {
                    rect = _lastDisplayRect;
                }
                else if(_lastDisplayRect != Rectangle.Empty)
                {
                    rect = Rectangle.Union(_lastDisplayRect, rect);
                }
            }

            if(_lastDisplayRect != rect)
            {
                _lastDisplayRect = rect;
            }

            if(this.Size != rect.Size)
            {
                this.Size = rect.Size;
            }

            if(this.Location != rect.Location)
            {
                this.Location = rect.Location;
            }
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

        private sealed class OverlayRenderBlock
        {
            public OCRDataManager.ResultData TargetData;
            public OCRDataManager.TransData TransData;
            public int ColorIndex;
            public Rectangle CaptureRect;
            public Rectangle SourceRect;
            public Rectangle ViewRect;
            public Rectangle ContentRect;
            public bool VerticalMode;
        }

        private void AddText(GraphicsPath gp, Graphics g, Font textFont, Rectangle rectangleOriginal, StringFormat sf)
        {
            if(!isActiveGDI || _dataList == null)
            {
                return;
            }

            Color outlineColor1 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor1;
            Color outlineColor2 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2;
            const int outlineWidth1 = 2;
            const int outlineWidth2 = 5;
            FontStyle overlayFontStyle = AdvencedOptionManager.OverlayUseFontOutline
                ? textFont.Style
                : textFont.Style & ~FontStyle.Bold;
            using Font overlayBaseFont = new Font(textFont.FontFamily, textFont.SizeInPoints, overlayFontStyle, GraphicsUnit.Point);

            List<OverlayRenderBlock> blocks = BuildRenderBlocks();
            ResolveBlockCollisions(blocks);

            foreach(var block in blocks)
            {
                using StringFormat blockFormat = (StringFormat)sf.Clone();
                ConfigureStringFormat(blockFormat, block.VerticalMode);

                Rectangle contentRect = Rectangle.Inflate(block.ViewRect, -4, -4);
                if(contentRect.Width <= 0 || contentRect.Height <= 0)
                {
                    block.TransData.ViewRect = block.ViewRect;
                    block.TransData.ContentRect = Rectangle.Empty;
                    Util.ShowLog($"Overlay block clipped: {block.TransData.trans}");
                    continue;
                }

                float minimumSize = AdvencedOptionManager.IsAutoFontSize
                    ? Math.Max(1, AdvencedOptionManager.MinAutoFontSize)
                    : overlayBaseFont.SizeInPoints;

                if(AdvencedOptionManager.IsAutoFontSize)
                {
                    TryExpandForMinimumFont(g, block, blocks, overlayBaseFont, blockFormat, minimumSize);
                    contentRect = Rectangle.Inflate(block.ViewRect, -4, -4);
                }

                float fontSize = AdvencedOptionManager.IsAutoFontSize
                    ? FindBestFontSize(g, block, overlayBaseFont, contentRect, blockFormat)
                    : overlayBaseFont.SizeInPoints;

                using Font renderFont = new Font(overlayBaseFont.FontFamily, fontSize, overlayBaseFont.Style, GraphicsUnit.Point);
                block.ContentRect = contentRect;
                block.TransData.ViewRect = block.ViewRect;
                block.TransData.ContentRect = contentRect;
                if(Form1.IsDebugShowWordArea)
                {
                    Util.ShowLog(
                        $"Overlay layout: source={block.SourceRect}, view={block.ViewRect}, content={contentRect}, " +
                        $"font={fontSize:0.00}, sourceFont={GetSourceFontPointSize(g, block):0.00}, text={block.TransData.trans}");
                }

                if(_isStart && Form1.IsDebugShowWordArea)
                {
                    using var debugBrush = new SolidBrush(Color.FromArgb(90, 0, 0, 0));
                    g.FillRectangle(debugBrush, block.SourceRect);
                }
                else if(_isStart && FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor)
                {
                    Color background = FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor;
                    if(block.TargetData.UseAutoColor && AdvencedOptionManager.OverlayAutoBackgroundColor)
                    {
                        Color sampled = block.TargetData.GetAutoColor(block.ColorIndex).BackGround;
                        background = Color.FromArgb(background.A, sampled);
                    }

                    using var backgroundBrush = new SolidBrush(background);
                    g.FillRectangle(backgroundBrush, block.ViewRect);
                }

                DrawWrappedText(g, block, renderFont, blockFormat, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);

                if(!DoesTextFit(g, block.TransData.trans, renderFont, contentRect, blockFormat, block.VerticalMode))
                {
                    Util.ShowLog($"Overlay block clipped at minimum font: {block.TransData.trans}");
                }
            }
        }

        private List<OverlayRenderBlock> BuildRenderBlocks()
        {
            var blocks = new List<OverlayRenderBlock>();
            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);

            foreach(var targetData in _dataList)
            {
                Rectangle areaRect;
                try
                {
                    areaRect = targetData.SnapShot
                        ? FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect
                        : FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(targetData.Index);
                }
                catch(Exception ex)
                {
                    Util.ShowLog(ex.Message);
                    continue;
                }

                if(areaRect == Rectangle.Empty)
                {
                    continue;
                }

                int originX = areaRect.X - FormManager.BorderWidth / 2;
                int originY = areaRect.Y - FormManager.BorderHeight / 2;
                if(_attchedCapture)
                {
                    originX = Math.Max(originX, clientPositionX);
                    originY = Math.Max(originY, clientPositionY);
                }
                Rectangle captureRect = new Rectangle(
                    originX - Location.X,
                    originY - Location.Y,
                    areaRect.Width,
                    areaRect.Height);
                captureRect = Rectangle.Intersect(ClientRectangle, captureRect);

                for(int colorIndex = 0; colorIndex < targetData.TransDataList.Count; colorIndex++)
                {
                    var transData = targetData.TransDataList[colorIndex];
                    Rectangle source = transData.SourceRect == Rectangle.Empty ? transData.lineRect : transData.SourceRect;
                    Rectangle screenRect = Rectangle.FromLTRB(
                        originX + (int)Math.Floor(source.Left / zoom) - Location.X,
                        originY + (int)Math.Floor(source.Top / zoom) - Location.Y,
                        originX + (int)Math.Ceiling(source.Right / zoom) - Location.X,
                        originY + (int)Math.Ceiling(source.Bottom / zoom) - Location.Y);
                    screenRect = Rectangle.Intersect(captureRect, screenRect);
                    if(screenRect.Width <= 0 || screenRect.Height <= 0)
                    {
                        continue;
                    }

                    blocks.Add(new OverlayRenderBlock
                    {
                        TargetData = targetData,
                        TransData = transData,
                        ColorIndex = colorIndex,
                        CaptureRect = captureRect,
                        SourceRect = screenRect,
                        ViewRect = screenRect,
                        ContentRect = Rectangle.Inflate(screenRect, -4, -4),
                        VerticalMode = AdvencedOptionManager.OverlayKeepSourceDirection
                            && transData.angleType == OCRDataManager.WordAngleType.Vertical,
                    });
                }
            }

            return blocks;
        }

        private static void ResolveBlockCollisions(List<OverlayRenderBlock> blocks)
        {
            int maximumIterations = Math.Max(1, blocks.Count * blocks.Count * 4);
            for(int iteration = 0; iteration < maximumIterations; iteration++)
            {
                int firstIndex = -1;
                int secondIndex = -1;
                long largestArea = 0;

                for(int i = 0; i < blocks.Count; i++)
                {
                    for(int j = i + 1; j < blocks.Count; j++)
                    {
                        Rectangle overlap = Rectangle.Intersect(blocks[i].ViewRect, blocks[j].ViewRect);
                        long area = (long)overlap.Width * overlap.Height;
                        if(area > largestArea)
                        {
                            largestArea = area;
                            firstIndex = i;
                            secondIndex = j;
                        }
                    }
                }

                if(firstIndex < 0)
                {
                    return;
                }

                OverlayRenderBlock first = blocks[firstIndex];
                OverlayRenderBlock second = blocks[secondIndex];
                bool preserveFirst = first.TransData.TitleData && !second.TransData.TitleData;
                bool preserveSecond = second.TransData.TitleData && !first.TransData.TitleData;

                var vertical = SplitVertically(first.ViewRect, second.ViewRect, preserveFirst, preserveSecond);
                var horizontal = SplitHorizontally(first.ViewRect, second.ViewRect, preserveFirst, preserveSecond);
                if(vertical.Loss <= horizontal.Loss)
                {
                    first.ViewRect = vertical.First;
                    second.ViewRect = vertical.Second;
                }
                else
                {
                    first.ViewRect = horizontal.First;
                    second.ViewRect = horizontal.Second;
                }
            }
        }

        private static (Rectangle First, Rectangle Second, long Loss) SplitVertically(Rectangle first, Rectangle second, bool preserveFirst, bool preserveSecond)
        {
            Rectangle originalFirst = first;
            Rectangle originalSecond = second;
            Rectangle overlap = Rectangle.Intersect(first, second);
            bool firstIsLeft = first.Left + first.Width / 2.0 <= second.Left + second.Width / 2.0;
            int boundary;
            if(firstIsLeft)
            {
                boundary = preserveFirst ? first.Right
                    : preserveSecond ? second.Left
                    : GetWeightedBoundary(overlap.Left, overlap.Right, first, second);
                boundary = Math.Clamp(boundary, overlap.Left, overlap.Right);
                first = Rectangle.FromLTRB(first.Left, first.Top, boundary, first.Bottom);
                second = Rectangle.FromLTRB(boundary, second.Top, second.Right, second.Bottom);
            }
            else
            {
                boundary = preserveFirst ? first.Left
                    : preserveSecond ? second.Right
                    : GetWeightedBoundary(overlap.Left, overlap.Right, second, first);
                boundary = Math.Clamp(boundary, overlap.Left, overlap.Right);
                second = Rectangle.FromLTRB(second.Left, second.Top, boundary, second.Bottom);
                first = Rectangle.FromLTRB(boundary, first.Top, first.Right, first.Bottom);
            }

            return (first, second, GetAreaLoss(originalFirst, originalSecond, first, second));
        }

        private static (Rectangle First, Rectangle Second, long Loss) SplitHorizontally(Rectangle first, Rectangle second, bool preserveFirst, bool preserveSecond)
        {
            Rectangle originalFirst = first;
            Rectangle originalSecond = second;
            Rectangle overlap = Rectangle.Intersect(first, second);
            bool firstIsTop = first.Top + first.Height / 2.0 <= second.Top + second.Height / 2.0;
            int boundary;
            if(firstIsTop)
            {
                boundary = preserveFirst ? first.Bottom
                    : preserveSecond ? second.Top
                    : GetWeightedBoundary(overlap.Top, overlap.Bottom, first, second);
                boundary = Math.Clamp(boundary, overlap.Top, overlap.Bottom);
                first = Rectangle.FromLTRB(first.Left, first.Top, first.Right, boundary);
                second = Rectangle.FromLTRB(second.Left, boundary, second.Right, second.Bottom);
            }
            else
            {
                boundary = preserveFirst ? first.Top
                    : preserveSecond ? second.Bottom
                    : GetWeightedBoundary(overlap.Top, overlap.Bottom, second, first);
                boundary = Math.Clamp(boundary, overlap.Top, overlap.Bottom);
                second = Rectangle.FromLTRB(second.Left, second.Top, second.Right, boundary);
                first = Rectangle.FromLTRB(first.Left, boundary, first.Right, first.Bottom);
            }

            return (first, second, GetAreaLoss(originalFirst, originalSecond, first, second));
        }

        private static int GetWeightedBoundary(int start, int end, Rectangle leading, Rectangle trailing)
        {
            long leadingArea = Math.Max(1L, (long)leading.Width * leading.Height);
            long trailingArea = Math.Max(1L, (long)trailing.Width * trailing.Height);
            double leadingShare = leadingArea / (double)(leadingArea + trailingArea);
            return start + (int)Math.Round((end - start) * leadingShare);
        }

        private static long GetAreaLoss(Rectangle originalFirst, Rectangle originalSecond, Rectangle first, Rectangle second)
        {
            long originalArea = (long)originalFirst.Width * originalFirst.Height
                + (long)originalSecond.Width * originalSecond.Height;
            long remainingArea = (long)first.Width * first.Height
                + (long)second.Width * second.Height;
            return Math.Max(0, originalArea - remainingArea);
        }

        private void TryExpandForMinimumFont(Graphics g, OverlayRenderBlock block, List<OverlayRenderBlock> blocks, Font baseFont, StringFormat format, float minimumSize)
        {
            Rectangle currentContent = Rectangle.Inflate(block.ViewRect, -4, -4);
            using Font minimumFont = new Font(baseFont.FontFamily, minimumSize, baseFont.Style, GraphicsUnit.Point);
            if(DoesTextFit(g, block.TransData.trans, minimumFont, currentContent, format, block.VerticalMode))
            {
                return;
            }

            if(block.VerticalMode)
            {
                int minimumLeft = block.CaptureRect.Left;
                foreach(var other in blocks)
                {
                    if(ReferenceEquals(other, block) || GetAxisOverlap(block.ViewRect.Top, block.ViewRect.Bottom, other.ViewRect.Top, other.ViewRect.Bottom) <= 0)
                    {
                        continue;
                    }
                    if(other.ViewRect.Right <= block.ViewRect.Left)
                    {
                        minimumLeft = Math.Max(minimumLeft, other.ViewRect.Right);
                    }
                }

                Rectangle maximum = Rectangle.FromLTRB(minimumLeft, block.ViewRect.Top, block.ViewRect.Right, block.ViewRect.Bottom);
                if(DoesTextFit(g, block.TransData.trans, minimumFont, Rectangle.Inflate(maximum, -4, -4), format, true))
                {
                    int low = minimumLeft;
                    int high = block.ViewRect.Left;
                    while(low < high)
                    {
                        int middle = (low + high + 1) / 2;
                        Rectangle candidate = Rectangle.FromLTRB(middle, block.ViewRect.Top, block.ViewRect.Right, block.ViewRect.Bottom);
                        if(DoesTextFit(g, block.TransData.trans, minimumFont, Rectangle.Inflate(candidate, -4, -4), format, true))
                        {
                            low = middle;
                        }
                        else
                        {
                            high = middle - 1;
                        }
                    }
                    block.ViewRect = Rectangle.FromLTRB(low, block.ViewRect.Top, block.ViewRect.Right, block.ViewRect.Bottom);
                }
            }
            else
            {
                int maximumBottom = block.CaptureRect.Bottom;
                foreach(var other in blocks)
                {
                    if(ReferenceEquals(other, block) || GetAxisOverlap(block.ViewRect.Left, block.ViewRect.Right, other.ViewRect.Left, other.ViewRect.Right) <= 0)
                    {
                        continue;
                    }
                    if(other.ViewRect.Top >= block.ViewRect.Bottom)
                    {
                        maximumBottom = Math.Min(maximumBottom, other.ViewRect.Top);
                    }
                }

                Rectangle maximum = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, block.ViewRect.Right, maximumBottom);
                if(DoesTextFit(g, block.TransData.trans, minimumFont, Rectangle.Inflate(maximum, -4, -4), format, false))
                {
                    int low = block.ViewRect.Bottom;
                    int high = maximumBottom;
                    while(low < high)
                    {
                        int middle = (low + high) / 2;
                        Rectangle candidate = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, block.ViewRect.Right, middle);
                        if(DoesTextFit(g, block.TransData.trans, minimumFont, Rectangle.Inflate(candidate, -4, -4), format, false))
                        {
                            high = middle;
                        }
                        else
                        {
                            low = middle + 1;
                        }
                    }
                    block.ViewRect = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, block.ViewRect.Right, low);
                }
            }
        }

        private float FindBestFontSize(Graphics g, OverlayRenderBlock block, Font baseFont, Rectangle contentRect, StringFormat format)
        {
            float minimum = Math.Max(1, AdvencedOptionManager.MinAutoFontSize);
            float maximum = Math.Max(minimum, AdvencedOptionManager.MaxAutoFontSize);
            if(block.TransData.lineDataList.Count > 0)
            {
                float sourcePointSize = GetSourceFontPointSize(g, block);
                maximum = Math.Max(minimum, Math.Min(maximum, sourcePointSize));
            }

            float low = minimum;
            float high = maximum;
            for(int iteration = 0; iteration < 9; iteration++)
            {
                float middle = (low + high) / 2f;
                using Font candidate = new Font(baseFont.FontFamily, middle, baseFont.Style, GraphicsUnit.Point);
                if(DoesTextFit(g, block.TransData.trans, candidate, contentRect, format, block.VerticalMode))
                {
                    low = middle;
                }
                else
                {
                    high = middle;
                }
            }
            return low;
        }

        private static float GetSourceFontPointSize(Graphics g, OverlayRenderBlock block)
        {
            if(block.TransData.lineDataList.Count == 0)
            {
                return 0;
            }

            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
            var sourceSizes = block.TransData.lineDataList
                .Select(OCRDataManager.GetFontSize)
                .OrderBy(size => size)
                .ToList();
            int middle = sourceSizes.Count / 2;
            float sourceMedian = sourceSizes.Count % 2 == 1
                ? sourceSizes[middle]
                : (sourceSizes[middle - 1] + sourceSizes[middle]) / 2f;
            return (float)(sourceMedian / zoom * 72.0 / g.DpiY);
        }

        private bool DoesTextFit(Graphics g, string text, Font font, Rectangle rect, StringFormat format, bool vertical)
        {
            if(string.IsNullOrEmpty(text) || rect.Width <= 0 || rect.Height <= 0)
            {
                return string.IsNullOrEmpty(text);
            }

            List<string> lines = GetWrappedLinesByAddString(g, text, font, rect.Width, rect.Height, format, vertical);
            if(lines.Count == 0)
            {
                return true;
            }

            float lineAdvance = font.GetHeight(g) * 1.2f;
            int outlinePadding = AdvencedOptionManager.OverlayUseFontOutline ? 5 : 0;
            float occupied = lines.Count * lineAdvance + outlinePadding;
            if(vertical ? occupied > rect.Width : occupied > rect.Height)
            {
                return false;
            }

            float emSize = g.DpiY * font.SizeInPoints / 72f;
            foreach(string line in lines)
            {
                using var path = new GraphicsPath();
                path.AddString(line, font.FontFamily, (int)font.Style, emSize, Point.Empty, format);
                RectangleF bounds = path.GetBounds();
                if(vertical ? bounds.Height + outlinePadding > rect.Height : bounds.Width + outlinePadding > rect.Width)
                {
                    return false;
                }
            }
            return true;
        }

        private void DrawWrappedText(Graphics g, OverlayRenderBlock block, Font font, StringFormat format, Color outlineColor1, int outlineWidth1, Color outlineColor2, int outlineWidth2)
        {
            List<string> lines = GetWrappedLinesByAddString(g, block.TransData.trans, font, block.ContentRect.Width, block.ContentRect.Height, format, block.VerticalMode);
            float advance = font.GetHeight(g) * 1.2f;
            for(int index = 0; index < lines.Count; index++)
            {
                Rectangle lineRect = block.VerticalMode
                    ? new Rectangle(block.ContentRect.Right - (int)Math.Ceiling((index + 1) * advance), block.ContentRect.Top, (int)Math.Ceiling(advance), block.ContentRect.Height)
                    : new Rectangle(block.ContentRect.Left, block.ContentRect.Top + (int)Math.Floor(index * advance), block.ContentRect.Width, (int)Math.Ceiling(advance));
                lineRect = Rectangle.Intersect(lineRect, block.ContentRect);
                DrawStringWithOutline2(g, lines[index], font, lineRect, format, block.TargetData, block.ColorIndex, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);
            }
        }

        private void ConfigureStringFormat(StringFormat format, bool vertical)
        {
            if(vertical)
            {
                format.FormatFlags |= StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
                return;
            }

            format.FormatFlags &= ~StringFormatFlags.DirectionVertical;
            if(_enableRTL)
            {
                format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            else
            {
                format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
            }
        }

        private static int GetAxisOverlap(int firstStart, int firstEnd, int secondStart, int secondEnd)
        {
            return Math.Max(0, Math.Min(firstEnd, secondEnd) - Math.Max(firstStart, secondStart));
        }

        private void AddTextLegacy(GraphicsPath gp, Graphics g, Font textFont, Rectangle rectangleOriginal, StringFormat sf)
        {
            if(!isActiveGDI)
                return;

            Rectangle rectangle = rectangleOriginal;
            SolidBrush backColorBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor);
            SolidBrush defualtColorBrush = new SolidBrush(Color.FromArgb(90, 0, 0, 0));
            Color outlineColor1 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor1;
            Color outlineColor2 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2;
            int outlineWidth1 = 2;
            int outlineWidth2 = 5;

            if(_dataList != null)
            {
                for(int i = 0; i < _dataList.Count; i++)
                {
                    int x = 0, y = 0;

                    try
                    {
                        if(_dataList[i].SnapShot)
                        {
                            x = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect.X;
                            y = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect.Y;
                        }
                        else
                        {
                            Rectangle ocrAreaRect = FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(_dataList[i].Index);
                            if (ocrAreaRect == Rectangle.Empty)
                            {
                                continue;
                            }

                            x = ocrAreaRect.X;
                            y = ocrAreaRect.Y;
                        }
                    }
                    catch(Exception ex)
                    {
                        Util.ShowLog(ex.Message);
                        continue;
                    }

                    y = y - FormManager.BorderHeight / 2;
                    x = x - FormManager.BorderWidth / 2;

                    Util.ShowLog($"{x} / {y}");

                    if(_attchedCapture)
                    {
                        if(x < clientPositionX)
                            x = clientPositionX;
                        if(y < clientPositionY)
                            y = clientPositionY;
                    }
         

                    var targetData = _dataList[i];

                    for(int j = 0; j < targetData.TransDataList.Count; j++)
                    {
                        var transData = targetData.TransDataList[j];
                        transData.ViewRect = transData.lineRect;
                        if(targetData.UseAutoColor && AdvencedOptionManager.OverlayAutoBackgroundColor)
                        {
                            var autoColor = targetData.GetAutoColor(j);
                            byte alpha = FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor.A;
                            Color backColor = Color.FromArgb(alpha, autoColor.BackGround);
                            backColorBrush = new SolidBrush(backColor);
                        }

                        rectangle.X = x + (int)(transData.lineRect.X / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.X;
                        rectangle.Y = y + (int)(transData.lineRect.Y / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.Y;
                        rectangle.Height = (int)(transData.lineRect.Height / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
                        rectangle.Width = (int)(transData.lineRect.Width / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);

                        try
                        {
                            Rectangle textRect = Screen.PrimaryScreen.Bounds;

                            if(transData.angleType == OCRDataManager.WordAngleType.Vertical)
                            {
                                sf.FormatFlags |= StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
                                textRect.Height = rectangle.Height;
                            }
                            else
                            {
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

                            if(AdvencedOptionManager.IsAutoFontSize)
                            {
                                float fontSize = OCRDataManager.GetFontSize(transData.lineDataList[0]) / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize / 2;
                                Logger.Logger.AddLog($"Font size : {fontSize}");
                                fontSize *= 1.25f;
                                fontSize = AdvencedOptionManager.GetResultAutoFontSize(fontSize);

                                textFont = new Font(textFont.FontFamily, fontSize);
                            }

                            float emSizeValue = g.DpiY / 72f;
                            float emSize = emSizeValue * textFont.SizeInPoints;

                            CharacterRange[] characterRanges = { new CharacterRange(0, transData.trans.Length) };
                            sf.SetMeasurableCharacterRanges(characterRanges);
                            Region[] stringRegions = g.MeasureCharacterRanges(transData.trans, textFont, textRect, sf);
                            if(stringRegions.Length > 0)
                            {
                                RectangleF measureRect1 = stringRegions[0].GetBounds(g);

                                if(transData.angleType == OCRDataManager.WordAngleType.Vertical)
                                {
                                    if(rectangle.Width < measureRect1.Width)
                                    {
                                        rectangle.Width = (int)measureRect1.Width;
                                    }
                                }
                                else
                                {
                                    if(rectangle.Height < measureRect1.Height)
                                    {
                                        rectangle.Width = rectangle.Width + (int)(rectangle.Width * 0.15f);
                                        stringRegions = g.MeasureCharacterRanges(transData.trans, textFont, rectangle, sf);
                                        if(stringRegions.Length > 0)
                                        {
                                            measureRect1 = stringRegions[0].GetBounds(g);
                                            rectangle.Height = (int)measureRect1.Height;
                                        }
                                    }
                                }
                            }

                            if(transData.lineDataList.Count == 1)
                            {
                                var size = g.MeasureString(transData.trans, textFont, int.MaxValue, sf);
                                if(rectangle.Width < (int)size.Width && transData.angleType == OCRDataManager.WordAngleType.Horizontal)
                                {
                                    rectangle.Width = (int)(size.Width + textFont.Size);
                                }

                                if(rectangle.Height < (int)size.Height && transData.angleType == OCRDataManager.WordAngleType.Vertical)
                                {
                                    rectangle.Height = (int)(size.Height + textFont.Size);
                                }
                            }

                            bool verticalMode = transData.angleType == OCRDataManager.WordAngleType.Vertical;

                            if(_isStart)
                            {
                                if(Form1.IsDebugShowWordArea)
                                {
                                    for(int z = 0; z < transData.lineDataList.Count; z++)
                                    {
                                        Rectangle ocrRect = transData.lineDataList[z].lineRect;
                                        ocrRect.X = x + (int)(ocrRect.X / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.X;
                                        ocrRect.Y = y + (int)(ocrRect.Y / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.Y;
                                        ocrRect.Height = (int)(ocrRect.Height / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) + 5;
                                        ocrRect.Width = (int)(ocrRect.Width / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) + 5;

                                        g.FillRectangle(defualtColorBrush, ocrRect.X, ocrRect.Y, ocrRect.Width, ocrRect.Height);
                                    }
                                }
                                else if(FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor)
                                {
                                    rectangle.Height += 10;
                                    rectangle.Width += 10;
                                    RectangleF measureRect1 = rectangle;
                                    g.FillRectangle(backColorBrush, measureRect1.X + 0, measureRect1.Y + 0, measureRect1.Width, measureRect1.Height);
                                }
                            }

                            if(transData.lineDataList.Count <= 1 && transData.TitleData || verticalMode)
                            {
                                // 한 줄이거나 타이틀, 버티컬 모드
                                DrawStringWithOutline2(g, transData.trans, textFont, rectangle, sf, targetData, j, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);
                            }
                            else
                            {
                                float lineSpacing = 1.2f;
                                List<string> wrappedLines = GetWrappedLinesByAddString(g, transData.trans, textFont, rectangle.Width, rectangle.Height, sf, verticalMode);
                                float fontHeight = textFont.GetHeight(g);

                                for(int lineIdx = 0; lineIdx < wrappedLines.Count; lineIdx++)
                                {
                                    Rectangle lineRect;
                                    if(verticalMode)
                                    {
                                        lineRect = new Rectangle(
                                            rectangle.X + (int)(lineIdx * fontHeight * lineSpacing),
                                            rectangle.Y,
                                            (int)fontHeight,
                                            rectangle.Height
                                        );
                                    }
                                    else
                                    {
                                        lineRect = new Rectangle(
                                            rectangle.X,
                                            rectangle.Y + (int)(lineIdx * fontHeight * lineSpacing),
                                            rectangle.Width,
                                            (int)fontHeight
                                        );
                                    }
                                    DrawStringWithOutline2(g, wrappedLines[lineIdx], textFont, lineRect, sf, targetData, j, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);
                                }
                            }


                        }
                        catch(Exception ex)
                        {
                            Util.ShowLog(ex.Message);
                        }
                    }
                }
            }
        }

        // 폰트색 기준으로 outline1(조금 밝게), outline2(조금 어둡게) 자동 계산
        private void GetAutoOutlineColors(Color fontColor, out Color outline1, out Color outline2)
        {
            Util.RGB2HSV(fontColor, out double h, out double s, out double v);
            //RgbToHsv(fontColor, out double h, out double s, out double v);

            s /= 255;
            v /= 255;
            // V(명도) 0~1, S(채도) 0~1
            if(v >= 0.5)
            {
                // 밝은 폰트: outline1은 S 10% 감소, V 30% 증가(최대 1), outline2는 V 10%
                double s1 = Math.Max(0, s - 0.05);
                double v1 = Math.Min(1, v - 0.1);
                double v2 = 0.1;
                outline1 = Util.HsvToRgb(h, s1, v1);
                outline2 = Util.HsvToRgb(h, 0, 0);
            }
            else
            {
                // 어두운 폰트: 동일하게 처리(혹은 필요시 다르게 조정)
                double s1 = Math.Max(0, s + 0.05);
                double v1 = Math.Max(0, v + 0.1);
                double v2 = 0.1;
                outline1 = Util.HsvToRgb(h, s1, v1);
                outline2 = Util.HsvToRgb(h, 0, 1);
            }
        }

        // 2중 아웃라인 + 본문 텍스트를 DrawString으로 그리는 함수
        private void DrawStringWithOutline2(Graphics g, string text, Font font, Rectangle rect, StringFormat sf, OCRDataManager.ResultData targetData, int colorIdx, Color outlineColor1, int outlineWidth1, Color outlineColor2, int outlineWidth2)
        {
            // 텍스트 색상
            Color fontColor = FormManager.Instace.MyMainForm.MySettingManager.TextColor;
            if(targetData.UseAutoColor && AdvencedOptionManager.OverlayAutoFontColor)
            {
                fontColor = targetData.GetAutoColor(colorIdx).Font;

                if(AdvencedOptionManager.OverlayUseFontOutline)
                {
                    GetAutoOutlineColors(fontColor, out outlineColor1, out outlineColor2);
                }
            }


            // GraphicsPath로 텍스트 경로 생성
            using(GraphicsPath path = new GraphicsPath())
            {
                float emSize = g.DpiY * font.SizeInPoints / 72f;
                path.AddString(
                    text,
                    font.FontFamily,
                    (int)font.Style,
                    emSize,
                    rect,
                    sf
                );

                if(AdvencedOptionManager.OverlayUseFontOutline)
                {
                    // 1. 바깥쪽 아웃라인(더 두껍게)
                    using(Pen outline2 = new Pen(outlineColor2, outlineWidth2) { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(outline2, path);
                    }
                    // 2. 안쪽 아웃라인(얇게)
                    using(Pen outline1 = new Pen(outlineColor1, outlineWidth1) { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(outline1, path);
                    }
                }
                // 본문 텍스트
                using(Brush fontBrush = new SolidBrush(fontColor))
                {
                    g.FillPath(fontBrush, path);
                }
            }
        }

        private List<string> GetWrappedLinesByAddString(Graphics g, string text, Font font, int maxWidth, int maxHeight, StringFormat sf, bool isVertical)
        {
            List<string> lines = new List<string>();
            if(string.IsNullOrEmpty(text))
                return lines;

            float emSize = g.DpiY * font.SizeInPoints / 72f;
            float fudge = font.Size * 1.2f; // 픽셀 여유

            string[] originalLines = text.Replace("\r\n", "\n").Split('\n');
            foreach(var originalLine in originalLines)
            {
                string remaining = originalLine;
                while(!string.IsNullOrEmpty(remaining))
                {
                    int lastFit = 0;
                    for(int i = 1; i <= remaining.Length; i++)
                    {
                        string sub = remaining.Substring(0, i);
                        float width;
                        if(isVertical)
                        {
                            // 세로 모드: GraphicsPath 기준
                            using(GraphicsPath path = new GraphicsPath())
                            {
                                path.AddString(sub, font.FontFamily, (int)font.Style, emSize, new Point(0, 0), sf);
                                RectangleF bounds = path.GetBounds();
                                width = bounds.Height; // 세로 모드는 높이로 판단
                            }
                            if(width > maxHeight - fudge)
                                break;
                        }
                        else
                        {
                            // 가로 모드: MeasureString과 GraphicsPath 중 더 큰 값 사용
                            using(GraphicsPath path = new GraphicsPath())
                            {
                                path.AddString(sub, font.FontFamily, (int)font.Style, emSize, new Point(0, 0), sf);
                                RectangleF bounds = path.GetBounds();
                                SizeF ms = g.MeasureString(sub, font);
                                width = Math.Max(bounds.Width, ms.Width);
                            }
                            if(width > maxWidth - fudge)
                                break;
                        }

                        lastFit = i;
                    }
                    if(lastFit == 0) lastFit = 1;
                    lines.Add(remaining.Substring(0, lastFit));
                    if(lastFit >= remaining.Length)
                        break;
                    remaining = remaining.Substring(lastFit).TrimStart();
                }
            }
            return lines;
        }

        private bool isLockPaint = false;
        private bool _forceLock;

        public void UpdatePaint()
        {
            if(_forceLock)
            {
                return;
            }

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
                {
                    using Brush foreBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.TextColor);
                    sf.Alignment = stringFormat.Alignment;
                    sf.FormatFlags = stringFormat.FormatFlags;
                    Color backgroundColor = Color.FromArgb(alpha, Color.Red);
                    g.Clear(backgroundColor);

                    Rectangle rectangle = ClientRectangle;
                    rectangle.X = this.Location.X;
                    rectangle.Y = this.Location.Y;

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;


                    AddText(gp, g, textFont, rectangle, sf);

                    if(!_isStart)
                    {
                        using(Pen layerOutline = new Pen(Color.FromArgb(40, 134, 249), 3) { LineJoin = LineJoin.Round })
                            g.DrawRectangle(layerOutline, ClientRectangle);

                    }
                    /*
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
                    */

                }

                if(!_isStart)
                {
                    g.Clear(Color.FromArgb(0));
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
            if(enableRTL)
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

        public void Prepare()
        {
            if(InvokeRequired)
            {
                Invoke((Action)Prepare);
                return;
            }

            _dataList = new List<OCRDataManager.ResultData>();
            _overlayDataCache.Clear();
            resultText = "";
            _lastDisplayRect = Rectangle.Empty;
            isLockPaint = false;
            _forceLock = false;
            _isStart = true;
            alpha = 0;
            UpdatePaint();
            OverlayRenderSynchronizationService.Flush();
        }

        public void StartTrans()
        {
            TaskIndex++;
            if(TaskIndex > 100000)
            {
                TaskIndex = 0;
            }

            TranslateStatusType = TranslateStatusType.Translate;
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
        public void setInvisibleBackground()
        {
            isLockPaint = false;
            _isStart = true;
            _forceLock = false;
            alpha = 0;     //0이어야 함
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void setVisibleBackground()
        {
            isLockPaint = false;
            _isStart = false;
            _forceLock = false;
            alpha = 190;
            _lastDisplayRect = Rectangle.Empty;
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
