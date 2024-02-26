using MORT.LocalizeManager;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace MORT
{
    public partial class ColorPickerForm : Form, ILocalizeForm
    {
        public class ScreenCaptureClass
        {
            /// <summary>
            /// Gets Image object containing screen shot
            /// </summary>
            /// <param name="handle">The handle to the window. 
            /// <returns></returns>
            public Image CaptureWindow(IntPtr handle)
            {
                var hdcSrc = User32.GetWindowDC(handle);
                var windowRect = new User32.Rect();
                User32.GetWindowRect(handle, ref windowRect);
                var width = windowRect.right - windowRect.left;
                var height = windowRect.bottom - windowRect.top;
                var hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
                var hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);
                var hOld = Gdi32.SelectObject(hdcDest, hBitmap);
                Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, Gdi32.Srccopy);
                Gdi32.SelectObject(hdcDest, hOld);
                Gdi32.DeleteDC(hdcDest);
                User32.ReleaseDC(handle, hdcSrc);
                Image img = Image.FromHbitmap(hBitmap);
                Gdi32.DeleteObject(hBitmap);
                return img;
            }

            /// <summary>
            /// Gdi32 API functions
            /// </summary>
            private class Gdi32
            {
                public const int Srccopy = 0x00CC0020; // BitBlt dwRop parameter

                [DllImport("gdi32.dll")]
                public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                    int nWidth, int nHeight, IntPtr hObjectSource,
                    int nXSrc, int nYSrc, int dwRop);

                [DllImport("gdi32.dll")]
                public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

                [DllImport("gdi32.dll")]
                public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int nWidth,
                    int nHeight);

                [DllImport("gdi32.dll")]
                public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);

                [DllImport("gdi32.dll")]
                public static extern bool DeleteDC(IntPtr hDc);

                [DllImport("gdi32.dll")]
                public static extern bool DeleteObject(IntPtr hObject);

            }

            /// <summary>
            /// User32 API functions
            /// </summary>
            private class User32
            {
                [StructLayout(LayoutKind.Sequential)]
                public struct Rect
                {
                    public readonly int top;
                    public readonly int left;
                    public readonly int bottom;
                    public readonly int right;
                }
                [DllImport("user32.dll")]
                public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

                [DllImport("user32.dll")]
                public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

                [DllImport("user32.dll")]
                public static extern IntPtr GetWindowDC(IntPtr hWnd);
            }
        }


        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);


        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        int zoom = 1;
        int lastPositionX = 0;
        int lastPositionY = 0;
        BinaryColorPickerForm binaryForm = null;

        private static ColorPickerForm instance;
        private static bool isAlreadyMadeFlag = false;
        public static bool IsAlreadyMadeFlag
        {
            get
            {
                return isAlreadyMadeFlag;
            }
            set
            {
            }
        }
        public static ColorPickerForm Instance
        {
            get
            {
                bool newIsAlreadyFlag = false;
                if(instance == null)
                {
                    newIsAlreadyFlag = false;
                    instance = new ColorPickerForm();
                }
                else
                {
                    newIsAlreadyFlag = true;

                }
                isAlreadyMadeFlag = newIsAlreadyFlag;
                return instance;
            }
            private set
            {
            }
        }

        public ColorPickerForm()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(MouseWheelEvent);
            LocalizeForm();
            //ScreenCapture();
        }

        private string LocalizeString(string key)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key, "");
        }

        public void LocalizeForm()
        {
            this.LocalizeLabel("ColorPicker Title");
            processButton.LocalizeLabel("ColorPicker processButton");
        }

        private void MouseWheelEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {


            // 휠을 아래로 내리면
            // 축소(Reduce)가 아닌 확대(Expand) 이므로 false 대입
            if(e.Delta < 0)
            {
                if(zoom > 1)
                {
                    zoom--;
                    zoomComboBox.Text = (zoom * 100).ToString() + "%";
                }
            }
            else
            {
                if(zoom < 4)
                {
                    zoom++;
                    zoomComboBox.Text = (zoom * 100).ToString() + "%";
                }
            }
            ApplyZoom();



            /* 셀의 크기가 셀의 한계 범위 안에 든다면
            * ->  MinCellSize < 셀 사이즈 < MaxCellSize
            * ResizeCellSize함수는 셀의 크기를 1씩 증가 혹은 감소 하고
            * true를 리턴한다.
            * 하지만, 셀 사이즈가 한계 크기와 같다면
            * 셀 크기에는 변동이 없고 false를 리턴
            * 리턴값이 true라면 화면을 다시 그린다.
            */

        }

        private void Init()
        {
            hLabel.Text = "";
            sLabel.Text = "";
            vLabel.Text = "";

            rLabel.Text = "";
            gLabel.Text = "";
            bLabel.Text = "";

            zoom = 1;
            zoomComboBox.Text = "100%";
            zoomComboBox.SelectedItem = 0;

            if(binaryForm != null)
            {
                binaryForm.Close();
            }
            binaryForm = null;
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Size.Width / 2) - (this.Size.Width / 2), (Screen.PrimaryScreen.Bounds.Size.Height / 2) - (this.Size.Height / 2));

        }

        private void ApplyZoom()
        {
            screenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            binaryScreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Size screenSize = screenPictureBox.Image.Size;
            screenSize.Height = screenSize.Height * zoom;
            screenSize.Width = screenSize.Width * zoom;
            screenPictureBox.Size = screenSize;
            binaryScreenPictureBox.Size = screenSize;
        }
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);

        public Bitmap CaptureWindow(Control ctl)
        {
            //Bitmap bmp = new Bitmap(ctl.Width, ctl.Height);  // includes borders
            Bitmap bmp = new Bitmap(ctl.ClientRectangle.Width, ctl.ClientRectangle.Height);  // content only
            using(Graphics graphics = Graphics.FromImage(bmp))
            {
                IntPtr hDC = graphics.GetHdc();
                try { PrintWindow(ctl.Handle, hDC, (uint)0); }
                finally { graphics.ReleaseHdc(hDC); }
            }
            return bmp;
        }

        public static IntPtr GetForegroundWindowIntPtr()
        {
            IntPtr hdcBitmap = IntPtr.Zero;
            var foregroundWindowsHandle = GetForegroundWindow();
            if(foregroundWindowsHandle == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            var rect = new Rect();
            GetWindowRect(foregroundWindowsHandle, ref rect);
            var result = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
            using(var g = Graphics.FromImage(result))
            {
                hdcBitmap = g.GetHdc();
                bool succeeded = PrintWindow(foregroundWindowsHandle, hdcBitmap, 0x3);
            }
            return hdcBitmap;
        }

        public void ScreenCapture(Bitmap bitmap)
        {

            int sizeX = bitmap.Width;
            int sizeY = bitmap.Height;
            FormManager.Instace.SetTopMostOcrArea(false);
            this.Focus();

            #region ::::::::::: 원본 코드임 ::::::::::::
            //백업

            Size uScreenSize = new Size(sizeX, sizeY);

            screenPictureBox.Image = bitmap;
            screenPictureBox.Size = uScreenSize;

            binaryScreenPictureBox.Image = new Bitmap(bitmap);
            binaryScreenPictureBox.Size = uScreenSize;

            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;

            imgPanel.Size = new Size(sizeX, sizeY);
            if(sizeY > informationPanel.Size.Height + 30)
            {
                this.Size = new Size(sizeX + BorderWidth * 2, sizeY + BorderWidth + TitlebarHeight);
            }
            else
            {
                this.Size = new Size(sizeX + BorderWidth * 2 + informationPanel.Size.Width, BorderWidth + TitlebarHeight + informationPanel.Size.Height + 60);
            }

            #endregion


            Init();
        }

        public void ShowBinary(bool isRgb = false, bool isHsv = false, bool isThreshold = false, ColorGroup color = null, int threshold = 127)
        {
            screenPictureBox.Visible = false;
            binaryScreenPictureBox.Visible = true;

            if(this.OwnedForms.Length == 0)
            {
                binaryForm = new BinaryColorPickerForm();
            }
            binaryForm.Owner = this;

            binaryForm.Setup(screenPictureBox, binaryScreenPictureBox);
            binaryForm.Show();

            binaryForm.SetValue(isHsv, isRgb, isThreshold, color, threshold);
        }

        private void ColorPickerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormManager.Instace.SetTopMostOcrArea(true);
            isAlreadyMadeFlag = false;
            instance = null;

            binaryForm = null;
        }

        public static void RGB2HSV(int r, int g, int b, out double h, out double s, out double v)
        {
            double min, max, delta;

            max = Math.Max(r, Math.Max(g, b));
            min = Math.Min(r, Math.Min(g, b));

            v = max;                // v, 0..255

            delta = max - min;                      // 0..255, < v

            if(max != 0)
                s = (int)(delta) * 255 / max;        // s, 0..255
            else
            {
                s = 0;
                h = 0;
                return;
            }

            if(delta == 0)
            {
                h = 0;
                return;
            }

            if(r == max)
                h = (g - b) * 60 / delta;        // between yellow & magenta
            else if(g == max)
                h = 120 + (b - r) * 60 / delta;    // between cyan & yellow
            else
                h = 240 + (r - g) * 60 / delta;    // between magenta & cyan

            if(h < 0)
                h += 360;
        }

        public static Bitmap ConvertBlackAndWhite(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });
            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            double max = Math.Max(color.R, Math.Max(color.G, color.B));
            double min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        private void screenPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            int positionX = mouseEvent.X / zoom;
            int positionY = mouseEvent.Y / zoom;

            if(e.Button == MouseButtons.Left && (positionX != lastPositionX || positionY != lastPositionY))
            {

                if((0 <= positionX && positionX < screenPictureBox.Image.Size.Width) &&
                    0 <= positionY && positionY < screenPictureBox.Image.Size.Height)
                {
                    Bitmap screenBitmap = (Bitmap)screenPictureBox.Image;

                    Color rgbValue = screenBitmap.GetPixel(positionX, positionY);
                    rLabel.Text = rgbValue.R.ToString();
                    gLabel.Text = rgbValue.G.ToString();
                    bLabel.Text = rgbValue.B.ToString();

                    double hue;
                    double saturation;
                    double value;
                    RGB2HSV(rgbValue.R, rgbValue.G, rgbValue.B, out hue, out saturation, out value);


                    saturation = saturation * 100 / 255;
                    value = value * 100 / 255;

                    hLabel.Text = hue.ToString("0");
                    sLabel.Text = ((saturation)).ToString("0");
                    vLabel.Text = ((value)).ToString("0");
                    //hLabel.Text = positionX.ToString();
                    colorBox.BackColor = rgbValue;

                    lastPositionX = positionX;
                    lastPositionY = positionY;
                }

            }

        }

        private void ColorPickerForm_Resize(object sender, EventArgs e)
        {
            imgPanel.Size = new Size(this.Size.Width - informationPanel.Size.Width - SystemInformation.VerticalScrollBarWidth, this.ClientRectangle.Height);
            informationPanel.Size = new Size(informationPanel.Size.Width, this.ClientRectangle.Height);
        }

        private void zoomComboBox_TextUpdate(object sender, EventArgs e)
        {
            zoomComboBox.Text = (zoom * 100).ToString() + "%";
        }

        private void zoomComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            zoom = zoomComboBox.SelectedIndex + 1;
            ApplyZoom();
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            ShowBinary();
        }
    }
}
