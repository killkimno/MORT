using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        static int areaIndex = 1;
        public OcrAreaForm()
        {
            screenType = screenForm.ScreenType.Normal;
            this.Activate();
            InitializeComponent();
            Index = areaIndex++;
            setTitleLabel();
        }

        public OcrAreaForm(screenForm.ScreenType screenType)
        {
            this.screenType = screenType;
            if(screenType == screenForm.ScreenType.Normal)
            {
                this.Activate();
                InitializeComponent();
                Index = areaIndex++;
                setTitleLabel();
            }
            else
            {
                this.Activate();
                InitializeComponent();
                Index = -1;
                setTitleLabel();
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
                titleLabel.Text = "빠른 영역, 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }
            else if (screenType == screenForm.ScreenType.Snap)
            {
                titleLabel.Text = "왜 보임? 영역, 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }
            else if(screenType == screenForm.ScreenType.Normal)
            {
                titleLabel.Text = "영역" + Index + " 사이즈 : " + this.Size.Width + "x" + this.Size.Height + " / 위치 : X " + this.Location.X + " Y " + this.Location.Y;
            }
            
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
            int borderSize = 8;
            int secondBorderSize = 3;
            Panel myPanel = (Panel)sender;
            Pen myPen = new Pen(Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(155)))), ((int)(((byte)(191))))), borderSize);

            myPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);  
            e.Graphics.DrawRectangle(myPen,
            myPanel.ClientRectangle.Left + borderSize / 2,
            myPanel.ClientRectangle.Top + borderSize / 2,
            myPanel.ClientRectangle.Width - borderSize,
            myPanel.ClientRectangle.Height - borderSize -20);

            myPen = new Pen(Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(125)))), ((int)(((byte)(153))))), secondBorderSize); 
            e.Graphics.DrawRectangle(myPen,
            myPanel.ClientRectangle.Left + borderSize / 2 ,
            myPanel.ClientRectangle.Top + borderSize / 2,
            myPanel.ClientRectangle.Width - borderSize,
            myPanel.ClientRectangle.Height - borderSize - 20);
             
            base.OnPaint(e);
             
        }

        private void titleLabel_Paint(object sender, PaintEventArgs e)
        {
            Label myLabel = (Label)sender;
            myLabel.Size = new Size(this.ClientSize.Width, 20);

        }


        #region:::::::::::::::::::::::::::::::::::::::::::레이어 창 이동 관련:::::::::::::::::::::::::::::::::::::::::::


        private void panealBorder_MouseDown(object sender, MouseEventArgs e)
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

        private void panealBorder_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.Button & MouseButtons.Right) == MouseButtons.Right || (e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                nowDragMode = dragMode.none;
            }
            if (nowDragMode != dragMode.none)
            {
                Size newSize = new Size(0,0);
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
        private void panealBorder_MouseUp(object sender, MouseEventArgs e)
        {
            nowDragMode = dragMode.none;
            if (Location.Y < 0)
            {
                Location = new Point(this.Location.X, 0);
            }
        }

        #endregion

        private void OcrAreaForm_Resize(object sender, EventArgs e)
        {
            setTitleLabel();
            exit_button.Location = new Point(this.ClientSize.Width - 20, 0);
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
            if (Location.Y < 0)
            {
                Location = new Point(this.Location.X, 0);
            }
        }

        private void OcrAreaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(screenType == screenForm.ScreenType.Normal)
            {
                areaIndex--;
                FormManager.Instace.DestoryOcrAreaForm(Index);
            }
            else if(screenType == screenForm.ScreenType.Quick)
            {
                FormManager.Instace.quickOcrAreaForm = null;
            }
            else if(screenType == screenForm.ScreenType.Snap)
            {
                FormManager.Instace.snapOcrAreaForm = null;
            }
            
        }

        private void OcrAreaForm_Move(object sender, EventArgs e)
        {
            setTitleLabel();
        }

        private void color_picker_button_Click(object sender, EventArgs e)
        {
            int borderSize = 8;

            if (ColorPickerForm.IsAlreadyMadeFlag == false)
            {
                ColorPickerForm.Instance.Show();
            }
                
            ColorPickerForm.Instance.Activate();
            ColorPickerForm.Instance.ScreenCapture(borderSize + this.Location.X,
            borderSize + this.Location.Y + 20,
            this.ClientRectangle.Width - borderSize * 2,
            this.ClientRectangle.Height - borderSize * 2 - 20);            

        }

        private void color_group_button_Click(object sender, EventArgs e)
        {
            FormManager.Instace.ShowColorGroupForm(this);
        }

    }


}
