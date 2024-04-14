using System;
using System.Drawing;
using System.Windows.Forms;

namespace MORT
{
    public partial class RTT : Form
    {
        private Point mousePoint;
        private Form1 m_InstanceRef = null;
        public Form1 InstanceRef
        {
            get
            {
                return m_InstanceRef;
            }
            set
            {
                m_InstanceRef = value;
            }
        }


        public RTT()
        {
            InitializeComponent();
        }

        public void ToggleStartButton(bool isStart)
        {
            if(InvokeRequired)
            {

                BeginInvoke(new Action(() =>
                {
                    startTransButton.Visible = !isStart;
                    stopButton.Visible = isStart;
                }));
            }
            else
            {
                startTransButton.Visible = !isStart;
                stopButton.Visible = isStart;
            }

        }


        private void mouseDragClick(MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void mouseDragMove(MouseEventArgs e)
        {
            if((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            if(m_InstanceRef != null)
            {
                m_InstanceRef.Visible = true;
                m_InstanceRef.Activate();
            }
        }

        private void closeApplication()
        {
            this.Visible = false;
            return;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            return;
        }

        private void RTT_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDragClick(e);
        }

        private void RTT_MouseMove(object sender, MouseEventArgs e)
        {
            mouseDragMove(e);
        }


        private void setCaptureAreaButton_MouseDown(object sender, MouseEventArgs e)
        {
            //todo remote
            this.setCaptureAreaButton.Image = global::MORT.Properties.Resources.Remote_Search_Click;
        }

        private void setCaptureAreaButton_MouseLeave(object sender, EventArgs e)
        {
            //this.setCaptureAreaButton.Image = ChangeOpacity(this.setCaptureAreaButton.Image, 0.9f);
        }

        private void setCaptureAreaButton_MouseUp(object sender, MouseEventArgs e)
        {
            //todo remote
            this.setCaptureAreaButton.Image = global::MORT.Properties.Resources.Remote_Search;
        }

        private void setSnapShotButton_MouseUp(object sender, MouseEventArgs e)
        {
            //todo remote
            this.snapButton.Image = global::MORT.Properties.Resources.Remote_Snap_Shot;
        }

        private void setSnapShotButton_MouseDown(object sender, MouseEventArgs e)
        {
            //todo remote
            this.snapButton.Image = global::MORT.Properties.Resources.Remote_Snap_Shot_Click;
        }

        private void startTransButton_MouseDown(object sender, MouseEventArgs e)
        {
            //todo remote
            this.startTransButton.Image = global::MORT.Properties.Resources.Remote_Translate_Click;
        }

        private void startTransButton_MouseUp(object sender, MouseEventArgs e)
        {
            //todo remote
            this.startTransButton.Image = global::MORT.Properties.Resources.Remote_Translate;
        }

        private void stopButton_MouseDown(object sender, MouseEventArgs e)
        {
            //todo remote
            this.stopButton.Image = global::MORT.Properties.Resources.Remote_Stop_Click;
        }

        private void stopButton_MouseUp(object sender, MouseEventArgs e)
        {
            //todo remote
            this.stopButton.Image = global::MORT.Properties.Resources.Remote_Stop;
        }

        private void setCaptureAreaButton_Click(object sender, EventArgs e)
        {
            if(m_InstanceRef != null)
            {
                m_InstanceRef.MakeCaptureArea();
            }
        }

        private void startTransButton_Click(object sender, EventArgs e)
        {

            if(m_InstanceRef != null)
            {
                this.BeginInvoke((Action)m_InstanceRef.BeforeStartRealTimeTrans);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if(m_InstanceRef != null)
            {
                m_InstanceRef.StopTrans();
            }
        }

        private void RTT_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeApplication();
            e.Cancel = true;//종료를 취소하고 
        }

        private void snapButton_Click(object sender, EventArgs e)
        {
            if(m_InstanceRef != null)
            {
                m_InstanceRef.MakeAndStartSnapShop();
            }
        }

        private void closeButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.closeButton.Image = global::MORT.Properties.Resources.Remote_Exit_Click;
        }

        private void closeButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.closeButton.Image = global::MORT.Properties.Resources.Remote_Exit;
        }

        private void settingButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.settingButton.Image = global::MORT.Properties.Resources.Remote_Option1_Click;
        }

        private void settingButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.settingButton.Image = global::MORT.Properties.Resources.Remote_Option1;
        }
    }
}
