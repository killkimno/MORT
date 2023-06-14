using System;
using System.Threading;
using System.Windows.Forms;

namespace MORT
{
    public partial class Logo : Form
    {
        static Logo instance;
        bool isCloseApplication = false;
        Thread thread = null;
        public Logo()
        {
            InitializeComponent();
            instance = this;

            lbVersion.Text = "Build : " + Properties.Settings.Default.MORT_VERSION + " - " + Properties.Settings.Default.MORT_RELEASE;
        }
        private delegate void myDelegate();
        private void closeForm()
        {
            this.Close();
        }
        private void disableLogoThread(float time)
        {
            DateTime Tthen = DateTime.Now;
            do
            {
                Application.DoEvents();

            } while (Tthen.AddSeconds(time) > DateTime.Now && isCloseApplication == false);
            this.BeginInvoke(new myDelegate(closeForm));
            //this.Close();
        }
        public void disableLogo(float newTime)
        {
            thread = thread = new Thread(delegate ()
           {
               disableLogoThread(newTime);
           });
            thread.Start();

        }

        public void closeApplication()
        {
            isCloseApplication = true;

            if (thread != null)
            {
                thread.Join();
            }
            instance = null;
        }

        public static void SetTopmost(bool isTopMost)
        {
            if (instance != null)
                instance.TopMost = isTopMost;
        }
    }

}
