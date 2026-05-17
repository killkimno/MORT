using System;
using System.Drawing;
using System.Windows.Forms;

namespace MORT
{
    public sealed class BorderlessResizeForwarder : NativeWindow
    {
        private readonly Form _ownerForm;
        private readonly Func<Point, int> _hitTest;

        public BorderlessResizeForwarder(Form ownerForm, Control child, Func<Point, int> hitTest)
        {
            _ownerForm = ownerForm;
            _hitTest = hitTest;
            if(child.IsHandleCreated == false)
            {
                _ = child.Handle;
            }
            AssignHandle(child.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = -1;

            if(m.Msg == WM_NCHITTEST)
            {
                int x = unchecked((short)(long)m.LParam);
                int y = unchecked((short)((long)m.LParam >> 16));
                Point formClient = _ownerForm.PointToClient(new Point(x, y));
                if(_hitTest(formClient) != 0)
                {
                    m.Result = (IntPtr)HTTRANSPARENT;
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
