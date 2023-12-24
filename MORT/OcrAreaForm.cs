using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MORT
{
    public partial class OcrAreaForm : Form
    {
        public screenForm.ScreenType screenType = screenForm.ScreenType.Normal;
        public int Index = 0;
        public List<bool> useColorGroupList = new List<bool>();
        enum dragMode { none, left, right, up, down, leftUp, rightUp, leftDown, rightDown };
        dragMode nowDragMode = dragMode.none;
        private Point mousePoint;

        public static int ocrAreaIndex = 0;
        public static int exceptionAreaIndex = 0;

        private Color borderColor1, borderColor2;

        public void SetVisible(bool isVisible)
        {
            if(isVisible)
            {
                if(this.screenType == screenForm.ScreenType.Exception)
                {
                    this.Opacity = 0.7;
                }
                else
                {
                    this.Opacity = 1;
                }             
             
            }
            else
            {
                this.Opacity = 0;
            }

            Refresh();
        }

        public OcrAreaForm()
        {
            screenType = screenForm.ScreenType.Normal;
            this.Activate();
            InitializeComponent();
            Index = ++ocrAreaIndex;
            setTitleLabel();
            Init();
            Refresh();
        }

        public OcrAreaForm(screenForm.ScreenType screenType)
        {
            //Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(155)))), ((int)(((byte)(191)))))
            this.screenType = screenType;
            if (screenType == screenForm.ScreenType.Normal)
            {
                this.Activate();
                InitializeComponent();
                Index = ++ocrAreaIndex;
                setTitleLabel();
            }
            else if (screenType == screenForm.ScreenType.Exception)
            {
                this.Activate();
                InitializeComponent();
                Index = ++exceptionAreaIndex;
                setTitleLabel();
            }
            else
            {
                this.Activate();
                InitializeComponent();
                Index = -1;
                setTitleLabel();
            }
            Init();
            SetVisible(true);
            Refresh();
          
        }

        private void Init()
        {
            if(screenType == screenForm.ScreenType.Exception)
            {
                borderColor1 = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(2)))), ((int)(((byte)(40)))));
                borderColor2 = Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(28)))), ((int)(((byte)(36)))));

                color_group_button.Visible = false;
                color_picker_button.Visible = false;
                titleLabel.BackColor = Color.DarkGray;      
                
            }
            else
            {
                borderColor1 = Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(155)))), ((int)(((byte)(191)))));
                borderColor2 = Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(125)))), ((int)(((byte)(153)))));
            }
           
        }


        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }

        public void setTitleLabel()     //창 이름 다시 정함
        {
            if (screenType == screenForm.ScreenType.Quick)
            {
                titleLabel.Text = Properties.Settings.Default.UI_OCR_QUICK_AREA + " | 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }
            else if (screenType == screenForm.ScreenType.Snap)
            {
                titleLabel.Text = "왜 보임? 영역, 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }
            else if (screenType == screenForm.ScreenType.Normal)
            {
                titleLabel.Text = Properties.Settings.Default.UI_OCR_AREA_TITLE + Index + " | 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }
            else if (screenType == screenForm.ScreenType.Exception)
            {
                titleLabel.Text = Properties.Settings.Default.UI_OCR_EXCEPTION_AREA_TITLE + Index + " | 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }


            //Graphics formGraphics = this.CreateGraphics();
            //titleLabel.Text  = formGraphics.DpiX.ToString() + " / " + formGraphics.DpiX.ToString();

        }
        public void reSetTitleLabel(int closeIndex)     //다른 창이 없어졌을 때
        {
            if (Index > closeIndex)
            {
                Index--;
                setTitleLabel();
            }
        }


        private void panealBorder_Paint(object sender, PaintEventArgs e)        //패널에 경계선 칠하기 함수
        {
           
            int borderSize = Util.ocrFormBorder;
            int secondBorderSize = Util.ocrformSecondBorder;
            Panel myPanel = (Panel)sender;
            Pen myPen = new Pen(borderColor1, borderSize);

            myPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);


            e.Graphics.DrawRectangle(myPen,
            myPanel.ClientRectangle.Left + borderSize / 2,
            myPanel.ClientRectangle.Top + borderSize / 2,
            myPanel.ClientRectangle.Width - borderSize,
            myPanel.ClientRectangle.Height - borderSize - Util.ocrFormTitleBar);

            myPen = new Pen(borderColor2, secondBorderSize);
            e.Graphics.DrawRectangle(myPen,
            myPanel.ClientRectangle.Left + borderSize / 2,
            myPanel.ClientRectangle.Top + borderSize / 2,
            myPanel.ClientRectangle.Width - borderSize,
            myPanel.ClientRectangle.Height - borderSize - Util.ocrFormTitleBar);

            base.OnPaint(e);

            
        }

        private void titleLabel_Paint(object sender, PaintEventArgs e)
        {
            Label myLabel = (Label)sender;
            myLabel.Size = new Size(this.ClientSize.Width, Util.ocrFormTitleBar);

        }


        #region:::::::::::::::::::::::::::::::::::::::::::레이어 창 이동 관련:::::::::::::::::::::::::::::::::::::::::::


        private void panealBorder_MouseDown(object sender, MouseEventArgs e)
        {
            int max = Util.ocrFormMAX;
            Util.ShowLog( "max : " + max + " / " +e.ToString() + " x : " + e.X + " / y : " +e.Y + System.Environment.NewLine + " size : " + this.Size.ToString());
            if ((e.X <= max && e.X >= 1) && (e.Y <= max && e.Y >= 1))
            {
                nowDragMode = dragMode.leftUp;
            }
            else if ((this.Size.Width - e.X <= max && this.Size.Width - e.X >= 1) && (e.Y <= max && e.Y >= 1))
            {
                nowDragMode = dragMode.rightUp;
            }
            else if ((e.X <= max && e.X >= 1) && (this.Size.Height - e.Y <= max && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.leftDown;
            }
            else if ((this.Size.Width - e.X <= max && this.Size.Width - e.X >= 1) && (this.Size.Height - e.Y <= max && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.rightDown;
            }
            else if ((e.X <= max && e.X >= 1))
            {
                nowDragMode = dragMode.left;

            }
            else if (this.Size.Width - e.X <= max && this.Size.Width - e.X >= 1)
            {
                nowDragMode = dragMode.right;
            }
            else if ((e.Y <= max && e.Y >= 1))
            {
                nowDragMode = dragMode.up;
            }
            else if (this.Size.Height - e.Y <= max && this.Size.Height - e.Y >= 1)
            {
                nowDragMode = dragMode.down;
            }
            else
            {
                nowDragMode = dragMode.none;
            }

            mousePoint = new Point(e.X, e.Y);
        }

        private void panealBorder_MouseMove(object sender, MouseEventArgs e)
        {
            int max = Util.ocrFormMAX;

            if ((e.Button & MouseButtons.Right) == MouseButtons.Right || (e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                nowDragMode = dragMode.none;
            }
            if (nowDragMode != dragMode.none)
            {
                Size newSize = new Size(0, 0);
                if (nowDragMode == dragMode.leftUp)
                {
                    int backupTop = this.Top;
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                    newSize = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height + backupTop - this.Top);
                }
                else if (nowDragMode == dragMode.leftDown)
                {
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    newSize = new Size(this.Size.Width + backupLeft - this.Left, e.Y + 20);
                }
                else if (nowDragMode == dragMode.rightUp)
                {
                    int backupTop = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    newSize = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height + backupTop - this.Top);
                }
                else if (nowDragMode == dragMode.rightDown)
                {
                    newSize = new Size(this.Size.Width - (this.Size.Width - e.X), e.Y + 20);
                }
                else if (nowDragMode == dragMode.up)
                {
                    int backup = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    newSize = new Size(this.Size.Width, this.Size.Height + backup - this.Top);
                }
                else if (nowDragMode == dragMode.down)
                {
                    newSize = new Size(this.Size.Width, e.Y + 20);
                }
                else if (nowDragMode == dragMode.left)
                {
                    int backup = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    newSize = new Size(this.Size.Width + backup - this.Left, this.Size.Height);
                }
                else if (nowDragMode == dragMode.right)
                {

                    newSize = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height);
                }



                if (newSize.Height <= 50)
                {
                    newSize.Height = 50;
                }
                if (newSize.Width < 50)
                {
                    newSize.Width = 50;
                }

                this.Size = newSize;


            }

            if ((e.X <= max && e.X >= 0) && (e.Y <= max && e.Y >= 0))
            {

                Cursor = Cursors.SizeNWSE;
            }
            else if ((this.Size.Width - e.X <= max && this.Size.Width - e.X >= 0) && (e.Y <= max && e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if ((e.X <= max && e.X >= 0) && (this.Size.Height - e.Y <= max && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if ((this.Size.Width - e.X <= max && this.Size.Width - e.X >= 0) && (this.Size.Height - e.Y <= max && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNWSE;
            }
            else if ((e.X <= max && e.X >= 0))
            {
                Cursor = Cursors.SizeWE;

            }
            else if (this.Size.Width - e.X <= max && this.Size.Width - e.X >= 0)
            {
                Cursor = Cursors.SizeWE;
            }
            else if ((e.Y <= max && e.Y >= 0))
            {
                Cursor = Cursors.SizeNS;
            }
            else if (this.Size.Height - e.Y <= max && this.Size.Height - e.Y >= 0)
            {
                Cursor = Cursors.SizeNS;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
        private void panealBorder_MouseUp(object sender, MouseEventArgs e)
        {
            nowDragMode = dragMode.none;
            CheckFormLocation();            
        }


        public void CheckFormLocation()
        {
            int screenTop = 0;
            int screenLeft = 0;
            foreach(Screen s in Screen.AllScreens)
            {
                //r = Rectangle.Union(r, s.Bounds);
                if(s.Bounds.Top < screenTop)
                {
                    screenTop = s.Bounds.Top;
                }

                if(s.Bounds.Left < screenLeft) 
                { 
                    screenLeft = s.Bounds.Left; 
                }
            }

            Form f = this;

            bool needReposition = false;
            if(f.Location.Y < screenTop)
            {
                needReposition = true;
                Location = new Point(this.Location.X, screenTop);
            }

            if(f.Left < screenLeft) 
            {
                Location = new Point(screenLeft, Location.Y);
            }
            
        }

        #endregion

        private void OcrAreaForm_Resize(object sender, EventArgs e)
        {
            
            setTitleLabel();
            exit_button.Location = new Point(this.ClientSize.Width - 20 , 0);
            color_picker_button.Location = new Point(this.ClientSize.Width - 45, 0);
            color_group_button.Location = new Point(this.ClientSize.Width - 70, 0);

            if (this.Visible) this.Refresh();
        }

        private void titleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;

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
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void titleLabel_MouseUp(object sender, MouseEventArgs e)
        {
            CheckFormLocation();
        }

        private void OcrAreaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (screenType == screenForm.ScreenType.Normal)
            {
                ocrAreaIndex--;
                FormManager.Instace.DestoryOcrAreaForm(Index);
            }
            else if (screenType == screenForm.ScreenType.Quick)
            {
                FormManager.Instace.quickOcrAreaForm = null;
            }
            else if (screenType == screenForm.ScreenType.Snap)
            {
                FormManager.Instace.snapOcrAreaForm = null;
            }
            else if(screenType == screenForm.ScreenType.Exception)
            {
                exceptionAreaIndex--;
                FormManager.Instace.DestoryExceptionArea(Index);

            }
        }


        public void ShowColorPicker(bool isShowResult, bool isUseRgb, bool isUseHSv, bool isUseThreshold, ColorGroup colorGroup, int threshold)
        {
            int borderSize = Util.ocrformSecondBorder;

            Size uScreenSize = new Size(Math.Max(2, this.ClientRectangle.Width - borderSize * 2), Math.Max(2, this.ClientRectangle.Height - borderSize * 2 - Util.ocrFormTitleBar));
            Bitmap bitmap = new Bitmap(uScreenSize.Width, uScreenSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(borderSize + this.Location.X, borderSize + this.Location.Y + Util.ocrFormTitleBar, 0, 0, uScreenSize);


            if (ColorPickerForm.IsAlreadyMadeFlag == false)
            {
                ColorPickerForm.Instance.Show();
            }

            ColorPickerForm.Instance.Activate();
            ColorPickerForm.Instance.ScreenCapture(bitmap);

            if(isShowResult)
            {
                ColorPickerForm.Instance.ShowBinary(isUseRgb, isUseHSv, isUseThreshold, colorGroup, threshold);
            }
        }

        private void OcrAreaForm_Move(object sender, EventArgs e)
        {
            setTitleLabel();
        }

        private void color_picker_button_Click(object sender, EventArgs e)
        {
            ShowColorPicker(false, false, false, false, null, 0);


        }

        private void color_group_button_Click(object sender, EventArgs e)
        {
            FormManager.Instace.ShowColorGroupForm(this);
        }

    }


}
