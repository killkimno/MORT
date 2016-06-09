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
    public partial class CounterForm : Form
    {
        public CounterForm()
        {
            InitializeComponent();
            webBrowser1.Navigate("http://goo.gl/WZYvOI");
        }
    }
}
