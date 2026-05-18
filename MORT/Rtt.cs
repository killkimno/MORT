using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MORT
{
    public partial class RTT : Form
    {
        private const int ResizeGripSize = 18;
        private const int EdgeSize = 6;
        private const int CornerSize = 14;

        private Point mousePoint;
        private Form1 m_InstanceRef = null;
        private Size _baseClientSize;
        private readonly Dictionary<Control, Rectangle> _baseBounds = new Dictionary<Control, Rectangle>();
        private readonly List<BorderlessResizeForwarder> _edgeForwarders = new List<BorderlessResizeForwarder>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CaptureBaseLayout();
            AttachEdgeForwarders();
        }

        public void ApplyTopMost(bool topMost)
        {
            this.TopMost = topMost;
        }

        private void AttachEdgeForwarders()
        {
            if(_edgeForwarders.Count > 0)
            {
                return;
            }

            foreach(Control c in Controls)
            {
                _edgeForwarders.Add(new BorderlessResizeForwarder(this, c, HitTestResizeZone));
            }
        }

        private int HitTestResizeZone(Point clientPoint)
        {
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if(clientPoint.X >= ClientSize.Width - ResizeGripSize &&
                clientPoint.Y >= ClientSize.Height - ResizeGripSize)
            {
                return HTBOTTOMRIGHT;
            }

            bool leftCorner = clientPoint.X < CornerSize;
            bool rightCorner = clientPoint.X >= ClientSize.Width - CornerSize;
            bool topCorner = clientPoint.Y < CornerSize;
            bool bottomCorner = clientPoint.Y >= ClientSize.Height - CornerSize;

            if(leftCorner && topCorner)
            {
                return HTTOPLEFT;
            }
            if(rightCorner && topCorner)
            {
                return HTTOPRIGHT;
            }
            if(leftCorner && bottomCorner)
            {
                return HTBOTTOMLEFT;
            }

            bool leftEdge = clientPoint.X < EdgeSize;
            bool rightEdge = clientPoint.X >= ClientSize.Width - EdgeSize;
            bool topEdge = clientPoint.Y < EdgeSize;
            bool bottomEdge = clientPoint.Y >= ClientSize.Height - EdgeSize;

            if(leftEdge)
            {
                return HTLEFT;
            }
            if(rightEdge)
            {
                return HTRIGHT;
            }
            if(topEdge)
            {
                return HTTOP;
            }
            if(bottomEdge)
            {
                return HTBOTTOM;
            }
            return 0;
        }

        private void CaptureBaseLayout()
        {
            if(_baseClientSize.Width != 0 && _baseClientSize.Height != 0)
            {
                return;
            }

            _baseClientSize = ClientSize;
            _baseBounds.Clear();
            foreach(Control c in Controls)
            {
                _baseBounds[c] = c.Bounds;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyScaleToChildren();
        }

        private void ApplyScaleToChildren()
        {
            if(_baseClientSize.Width == 0 || _baseClientSize.Height == 0)
            {
                return;
            }

            float sx = (float)ClientSize.Width / _baseClientSize.Width;
            float sy = (float)ClientSize.Height / _baseClientSize.Height;
            float scale = Math.Min(sx, sy);

            int scaledContentWidth = (int)Math.Round(_baseClientSize.Width * scale);
            int scaledContentHeight = (int)Math.Round(_baseClientSize.Height * scale);
            int offsetX = (ClientSize.Width - scaledContentWidth) / 2;
            int offsetY = (ClientSize.Height - scaledContentHeight) / 2;

            SuspendLayout();
            foreach(var pair in _baseBounds)
            {
                if(pair.Key == closeButton)
                {
                    continue;
                }

                Rectangle b = pair.Value;
                pair.Key.Bounds = new Rectangle(
                    offsetX + (int)Math.Round(b.X * scale),
                    offsetY + (int)Math.Round(b.Y * scale),
                    (int)Math.Round(b.Width * scale),
                    (int)Math.Round(b.Height * scale));
            }

            if(_baseBounds.TryGetValue(closeButton, out Rectangle closeBase))
            {
                int rightMargin = _baseClientSize.Width - (closeBase.X + closeBase.Width);
                closeButton.Bounds = new Rectangle(
                    ClientSize.Width - closeBase.Width - rightMargin,
                    closeBase.Y,
                    closeBase.Width,
                    closeBase.Height);
            }
            ResumeLayout();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int WM_SIZING = 0x0214;
            const int WMSZ_LEFT = 1;
            const int WMSZ_RIGHT = 2;
            const int WMSZ_TOP = 3;
            const int WMSZ_TOPLEFT = 4;
            const int WMSZ_TOPRIGHT = 5;
            const int WMSZ_BOTTOM = 6;
            const int WMSZ_BOTTOMLEFT = 7;
            const int WMSZ_BOTTOMRIGHT = 8;

            if(m.Msg == WM_NCHITTEST)
            {
                int x = unchecked((short)(long)m.LParam);
                int y = unchecked((short)((long)m.LParam >> 16));
                Point clientPoint = PointToClient(new Point(x, y));
                int hit = HitTestResizeZone(clientPoint);
                if(hit != 0)
                {
                    m.Result = (IntPtr)hit;
                    return;
                }
            }
            else if(m.Msg == WM_SIZING && _baseClientSize.Width > 0 && _baseClientSize.Height > 0)
            {
                RECT r = Marshal.PtrToStructure<RECT>(m.LParam);
                int edge = m.WParam.ToInt32();
                int width = r.Right - r.Left;
                int height = r.Bottom - r.Top;
                float baseAspect = (float)_baseClientSize.Width / _baseClientSize.Height;

                if(edge == WMSZ_LEFT || edge == WMSZ_RIGHT)
                {
                    int newHeight = (int)Math.Round(width / baseAspect);
                    r.Bottom = r.Top + newHeight;
                }
                else if(edge == WMSZ_TOP || edge == WMSZ_BOTTOM)
                {
                    int newWidth = (int)Math.Round(height * baseAspect);
                    r.Right = r.Left + newWidth;
                }
                else
                {
                    float dx = (float)width / _baseClientSize.Width;
                    float dy = (float)height / _baseClientSize.Height;
                    float scale = Math.Max(dx, dy);
                    int newWidth = (int)Math.Round(_baseClientSize.Width * scale);
                    int newHeight = (int)Math.Round(_baseClientSize.Height * scale);

                    if(edge == WMSZ_TOPLEFT)
                    {
                        r.Left = r.Right - newWidth;
                        r.Top = r.Bottom - newHeight;
                    }
                    else if(edge == WMSZ_TOPRIGHT)
                    {
                        r.Right = r.Left + newWidth;
                        r.Top = r.Bottom - newHeight;
                    }
                    else if(edge == WMSZ_BOTTOMLEFT)
                    {
                        r.Left = r.Right - newWidth;
                        r.Bottom = r.Top + newHeight;
                    }
                    else if(edge == WMSZ_BOTTOMRIGHT)
                    {
                        r.Right = r.Left + newWidth;
                        r.Bottom = r.Top + newHeight;
                    }
                }

                Marshal.StructureToPtr(r, m.LParam, true);
                m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
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
