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


        private void mouseDragClick(MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void mouseDragMove(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            if (m_InstanceRef != null)
            {
                m_InstanceRef.Visible = true;
                m_InstanceRef.Activate();
            }
        }

        private void closeApplication()
        {
            Boolean isFindFormFlag = false;
            Form1 mainForm = null;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form1")
                {
                    mainForm = (Form1)frm;

                    if (mainForm.Visible == false)
                    {
                        isFindFormFlag = false;
                    }
                    else
                    {
                        isFindFormFlag = true;
                    }

                    break;
                }
            }
            if (isFindFormFlag == false)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "TransForm")
                    {
                        if (frm.Visible == false)
                        {
                            isFindFormFlag = false;
                        }
                        else
                        {
                            isFindFormFlag = true;
                        }

                        break;
                    }
                }
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "TransFormLayer")
                    {
                        if (frm.Visible == false)
                        {
                            isFindFormFlag = false;
                        }
                        else
                        {
                            isFindFormFlag = true;
                        }

                        break;
                    }
                }

            }

            if (isFindFormFlag == false && mainForm != null && this.Visible == true)
            {
                if (MessageBox.Show("종료하시겠습니까?", "종료하시겠습니까?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    mainForm.exitApplication();
                }

            }
            else
            {
                this.Visible = false;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            closeApplication();
            
        }

        private void RTT_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDragClick(e);
        }

        private void RTT_MouseMove(object sender, MouseEventArgs e)
        {
            mouseDragMove(e);
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDragClick(e);
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            mouseDragMove(e);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDragClick(e);
        }

        private void titleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            mouseDragMove(e);
        }

        private void setCaptureAreaButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.setCaptureAreaButton.Image = global::MORT.Properties.Resources.box_setting_button_click;
            searchLabel.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
        }

        private void setCaptureAreaButton_MouseUp(object sender, MouseEventArgs e)
        {

            this.setCaptureAreaButton.Image = global::MORT.Properties.Resources.box_setting_button;
            searchLabel.BackColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void startTransButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.startTransButton.Image = global::MORT.Properties.Resources.translate_start_button_click;
                translateLabel.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(179)))));
        }

        private void startTransButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.startTransButton.Image = global::MORT.Properties.Resources.translate_start_button;
            translateLabel.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(151)))), ((int)(((byte)(255)))));
        }

        private void stopButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.stopButton.Image = global::MORT.Properties.Resources.pause_button_click;
            stopLabel.BackColor = Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
        }

        private void stopButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.stopButton.Image = global::MORT.Properties.Resources.pause_button;
            stopLabel.BackColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void setCaptureAreaButton_Click(object sender, EventArgs e)
        {
            if (m_InstanceRef != null)
            {
                m_InstanceRef.clickCaptureAreaButton();
            }
        }

        private void startTransButton_Click(object sender, EventArgs e)
        {

            if (m_InstanceRef != null)
            {
                m_InstanceRef.StartTrnas();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (m_InstanceRef != null)
            {
                m_InstanceRef.StopTrans();
            }
        }

        private void RTT_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeApplication();
            e.Cancel = true;//종료를 취소하고 
        }



    }
}
