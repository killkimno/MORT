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
    public partial class Observer : Form
    {
        public Observer()
        {
            InitializeComponent();
        }
        public void setWindow(int newX, int newY, int newWidth, int newHeight)
        {
            this.Size = new System.Drawing.Size(newWidth, newHeight);
            this.Location = new Point(newX, newY);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT

                return cp;
            }
        }

        public IntPtr getThisIntPtr()
        {
            return this.Handle;     
        }
    }
}
