using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT
{
    public partial class Form1
    {
     

        private void OnClick_GitHub(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/killkimno/MORT");
            }
            catch { }

        }
    }
}
