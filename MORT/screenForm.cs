using System;
using System.Drawing;
using System.Windows.Forms;


namespace MORT
{


    public partial class screenForm : Form
    {
        private Action callback;
        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::

        public enum ScreenType
        {
            Normal, Quick, Snap, Exception, Preview
        }

        public static screenForm instance;
        public ScreenType screenType = ScreenType.Normal;

        public bool LeftButtonDown = false;
        public bool RectangleDrawn = false;
        public bool ReadyToDrag = false;


        public Point ClickPoint = new Point();
        public Point CurrentTopLeft = new Point();
        public Point CurrentBottomRight = new Point();
        public Point DragClickRelative = new Point();

        public int RectangleHeight = new int();
        public int RectangleWidth = new int();

        Pen MyPen = new Pen(Color.Black, 1);
        SolidBrush TransparentBrush = new SolidBrush(Color.White);
        Pen EraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);
        SolidBrush eraserBrush = new SolidBrush(Color.FromArgb(255, Color.Black));


        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::Mouse Event Handlers & Drawing Initialization:::::::::::::::::::::::::::::::::::::::::::
      
        public screenForm(ScreenType screenType, Color selectAreaColor, Color backColor)
        {

            instance = this;
            InitializeComponent();
            this.MouseDown += new MouseEventHandler(mouse_Click);

            this.MouseMove += new MouseEventHandler(mouse_Move);

            this.Location = SystemInformation.VirtualScreen.Location;
            this.Size = SystemInformation.VirtualScreen.Size;

            this.screenType = screenType;

            eraserBrush = new SolidBrush(selectAreaColor);
            BackColor = Color.FromArgb(backColor.R, backColor.G, backColor.B);

            //투명도 15% -> 255, 최소값 15%
            double opacity =  Math.Max((double)backColor.A , 75) / 255 * 0.15;
            Opacity = opacity;

            
            TransparentBrush = new SolidBrush(AdvencedOptionManager.SelectOcrAreaBackgroundColor);


        }
        #endregion


        public static void MakeScreenForm(ScreenType screenType, Action callback = null)
        {
            if (screenForm.instance == null)
            {
                var selectAreaColor = AdvencedOptionManager.SelectOcrAreaColor;
                var backColor = AdvencedOptionManager.SelectOcrAreaBackgroundColor;
                screenForm form = new screenForm(screenType, selectAreaColor, backColor);
                form.Show();
                form.callback = callback;
            }
        }

        public static void MakePreview(Color selectAreaColor, Color backColor)
        {
            if (screenForm.instance == null)
            {
                screenForm form = new screenForm(ScreenType.Preview, selectAreaColor, backColor);
                form.Show();
                form.callback = null;
            }
        }


        static public void MakeAreaForm(ScreenType scrrenType,int newX, int newY, int newX2, int newY2, bool isShowFlag)
        {
            int top = Util.GetScreenTopPosition() + 20;
            if (newY < top)
            {
                newY = top;
            }


            OcrAreaForm searchOptionForm = new OcrAreaForm(scrrenType);

            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;

            searchOptionForm.Show();
            searchOptionForm.SetVisible(false);
            searchOptionForm.StartPosition = FormStartPosition.Manual;
            searchOptionForm.Location = new Point(newX - BorderWidth, newY - TitlebarHeight);
            searchOptionForm.Size = new Size(newX2 + BorderWidth * 2, newY2 + TitlebarHeight + BorderWidth);
            searchOptionForm.CheckFormLocation();

            if (scrrenType == ScreenType.Normal)
            {
                FormManager.Instace.AddOcrAreaForm(searchOptionForm);
            }
            else if(scrrenType == ScreenType.Exception)
            {
                FormManager.Instace.AddExceptionAreaForm(searchOptionForm);
            }

            searchOptionForm.SetVisible(isShowFlag);
        }

        static public void MakeQuickOcrAreaForm(int newX, int newY, int newX2, int newY2, bool isShow)
        {

            if (newY < 20)
            {
                newY = 20;
            }

            OcrAreaForm searchOptionForm = null;
            if (FormManager.Instace.quickOcrAreaForm == null)
            {
                searchOptionForm = new OcrAreaForm(ScreenType.Quick);
            }
            else
            {
                searchOptionForm = FormManager.Instace.quickOcrAreaForm;
            }

            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;

            //TitlebarHeight = 27;
            Util.ShowLog("TitlebarHeight " + TitlebarHeight);

            searchOptionForm.StartPosition = FormStartPosition.Manual;

            searchOptionForm.Show();
            searchOptionForm.SetVisible(false);
            searchOptionForm.Location = new Point(newX - BorderWidth, newY - TitlebarHeight);
            searchOptionForm.Size = new Size(newX2 + BorderWidth * 2, newY2 + TitlebarHeight + BorderWidth);

            FormManager.Instace.MakeQuickOcrAreaForm(searchOptionForm);

          
            FormManager.Instace.MyMainForm.SetCaptureArea();

            searchOptionForm.SetVisible(isShow);
        }

        static public void MakeSnapOcrAreaForm(int newX, int newY, int newX2, int newY2)
        {
            if (newY < 20)
            {
                newY = 20;
            }

            OcrAreaForm searchOptionForm = null;
            if (FormManager.Instace.snapOcrAreaForm == null)
            {
                searchOptionForm = new OcrAreaForm(ScreenType.Snap);
            }
            else
            {
                searchOptionForm = FormManager.Instace.snapOcrAreaForm;
            }

            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;


            searchOptionForm.StartPosition = FormStartPosition.Manual;
            searchOptionForm.Show();
            searchOptionForm.SetVisible(false);
            searchOptionForm.Location = new Point(newX - BorderWidth, newY - TitlebarHeight);
            searchOptionForm.Size = new Size(newX2 + BorderWidth * 2, newY2 + TitlebarHeight + BorderWidth);

            FormManager.Instace.MakeSnapShotOcrAreaForm(searchOptionForm);

        }


        #region:::::::::::::::::::::::::::::::::::::::::::Mouse Buttons:::::::::::::::::::::::::::::::::::::::::::



        private void mouse_Up(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RectangleDrawn = true;
                LeftButtonDown = false;
                Point curPos = new Point(Cursor.Position.X, Cursor.Position.Y);

                if (ClickPoint.X > curPos.X)
                {
                    int temp = ClickPoint.X;
                    ClickPoint.X = curPos.X;
                    curPos.X = temp;
                }
                if (ClickPoint.Y > curPos.Y)
                {
                    int temp = ClickPoint.Y;
                    ClickPoint.Y = curPos.Y;
                    curPos.Y = temp;
                }
                if (curPos.X == ClickPoint.X)
                {
                    curPos.X++;
                }
                if (curPos.Y == ClickPoint.Y)
                {
                    curPos.Y++;
                }

                int width = curPos.X - ClickPoint.X;
                int height = curPos.Y - ClickPoint.Y;

                //최소 사이즈 미적용시 무효처리
                if(width <= 4  || height <= 4)
                {
                    if (callback != null)
                    {
                        callback();
                    }

                    callback = null;
                    screenForm.instance = null;
                    this.Close();

                    return;
                }

                if (screenType == ScreenType.Normal || screenType == ScreenType.Exception)
                {
                    MakeAreaForm(screenType, ClickPoint.X, ClickPoint.Y, width, height, true);
                }
                else if (screenType == ScreenType.Quick)
                {
                    MakeQuickOcrAreaForm(ClickPoint.X, ClickPoint.Y, width, height, FormManager.Instace.GetIsShowOcrAreaFlag());
                }
                else if (screenType == ScreenType.Snap)
                {
                    MakeSnapOcrAreaForm(ClickPoint.X, ClickPoint.Y, width, height);
                }
            }

            if (callback != null)
            {
                callback();
            }

            callback = null;
            screenForm.instance = null;
            this.Close();
        }
        private void mouse_Click(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                LeftButtonDown = true;
                ClickPoint = new Point(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);

                if (RectangleDrawn)
                {

                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    DragClickRelative.X = Cursor.Position.X - CurrentTopLeft.X;
                    DragClickRelative.Y = Cursor.Position.Y - CurrentTopLeft.Y;

                }
            }
        }


        #endregion



        private void mouse_Move(object sender, MouseEventArgs e)
        {
            if (LeftButtonDown && !RectangleDrawn)
            {
                DrawSelection();
            }
        }

        private void DrawSelection()
        {
            this.Cursor = Cursors.Arrow;
         
            //Erase the previous rectangle
            //g.DrawRectangle(EraserPen, CurrentTopLeft.X - this.Location.X, CurrentTopLeft.Y - this.Location.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

            //Calculate X Coordinates
            if (Cursor.Position.X < ClickPoint.X)
            {

                CurrentTopLeft.X = Cursor.Position.X;
                CurrentBottomRight.X = ClickPoint.X;

            }
            else
            {

                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = Cursor.Position.X;

            }

            //Calculate Y Coordinates
            if (Cursor.Position.Y < ClickPoint.Y)
            {

                CurrentTopLeft.Y = Cursor.Position.Y;
                CurrentBottomRight.Y = ClickPoint.Y;

            }
            else
            {
                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = Cursor.Position.Y;
            }

            BufferedGraphicsContext currentContext;
            BufferedGraphics myBuffer;
            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;
            // Creates a BufferedGraphics instance associated with Form1, and with
            // dimensions the same size as the drawing surface of Form1.
            myBuffer = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);


            Pen drwaPen = new Pen(Color.DarkGreen, 2);
            Rectangle rect = new Rectangle(CurrentTopLeft.X - this.Location.X, CurrentTopLeft.Y - this.Location.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

            var g = myBuffer.Graphics;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.Clear(Color.FromArgb(255, this.BackColor));
            //g.Clear(Color.FromArgb(255, TransparentBrush.Color));

            g.FillRectangle(eraserBrush, rect);
            g.DrawRectangle(drwaPen, rect);

            myBuffer.Render();
            // Renders the contents of the buffer to the specified drawing surface.
            //myBuffer.Render(this.CreateGraphics());
            myBuffer.Dispose();
        }


        private void screenForm_Load(object sender, EventArgs e)
        {

        }

        private void screenForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

    }
}