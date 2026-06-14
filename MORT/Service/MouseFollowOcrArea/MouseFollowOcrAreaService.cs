using System;
using System.Drawing;
using System.Windows.Forms;

namespace MORT.Service.MouseFollowOcrArea
{
    public class MouseFollowOcrAreaService : IDisposable
    {
        private readonly Timer _timer = new Timer();
        private Form1 _mainForm;
        private bool _isEnabled;
        private DateTime _lastCaptureUpdateTime = DateTime.MinValue;

        public bool IsEnabled => _isEnabled;

        public bool IsUseOnlyOcrAreaFollowMouse =>
            _isEnabled
            && AdvencedOptionManager.UseOnlyOcrAreaFollowMouse
            && !AdvencedOptionManager.UseLegacyOcrAreaFollowMouse
            && FormManager.Instace.ocrAreaFollowMouseForm != null
            && !FormManager.Instace.ocrAreaFollowMouseForm.IsDisposed;

        public void Initialize(Form1 mainForm)
        {
            _mainForm = mainForm;
            _timer.Interval = 30;
            _timer.Tick += Timer_Tick;
        }

        public void Toggle()
        {
            if (_isEnabled)
            {
                SetEnabled(false);
                return;
            }

            if (AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                if (GetTarget() == null)
                {
                    return;
                }

                SetEnabled(true);
                return;
            }

            if (FormManager.Instace.ocrAreaFollowMouseForm == null || FormManager.Instace.ocrAreaFollowMouseForm.IsDisposed)
            {
                FormManager.Instace.MakeOcrAreaFollowMouseForm(() =>
                {
                    if (FormManager.Instace.ocrAreaFollowMouseForm != null && !FormManager.Instace.ocrAreaFollowMouseForm.IsDisposed)
                    {
                        SetEnabled(true);
                    }
                });
                return;
            }

            SetEnabled(true);
        }

        public void SetEnabled(bool isEnable)
        {
            if (isEnable && GetTarget() == null)
            {
                return;
            }

            _isEnabled = isEnable;
            _timer.Enabled = isEnable;

            if (_mainForm != null && _mainForm.Initialized)
            {
                _mainForm.SetTempCaptureArea();
            }

            Util.ShowLog("OCR area follow mouse : " + isEnable);
        }

        public void ClearForm()
        {
            SetEnabled(false);
            FormManager.Instace.DestroyOcrAreaFollowMouseForm();
            _mainForm?.SetCaptureArea();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OcrAreaForm target = GetTarget();
            if (target == null || target.IsDisposed)
            {
                SetEnabled(false);
                return;
            }

            int borderWidth = Util.ocrFormBorder;
            int titlebarHeight = Util.ocrFormTitleBar;
            int ocrWidth = Math.Max(target.Size.Width - borderWidth * 2, 1);
            int ocrHeight = Math.Max(target.Size.Height - titlebarHeight - borderWidth, 1);
            Point mouse = Cursor.Position;
            Point newLocation = new Point(
                mouse.X - borderWidth - ocrWidth / 2,
                mouse.Y - titlebarHeight - ocrHeight / 2);

            if (target.Location != newLocation)
            {
                target.Location = newLocation;
                if (!AdvencedOptionManager.UseLegacyOcrAreaFollowMouse && _lastCaptureUpdateTime.AddMilliseconds(100) < DateTime.Now)
                {
                    _lastCaptureUpdateTime = DateTime.Now;
                    _mainForm?.SetTempCaptureArea();
                }
            }
        }

        private OcrAreaForm GetTarget()
        {
            if (!AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                if (FormManager.Instace.ocrAreaFollowMouseForm != null && !FormManager.Instace.ocrAreaFollowMouseForm.IsDisposed)
                {
                    return FormManager.Instace.ocrAreaFollowMouseForm;
                }

                return null;
            }

            if (FormManager.Instace.quickOcrAreaForm != null && !FormManager.Instace.quickOcrAreaForm.IsDisposed)
            {
                return FormManager.Instace.quickOcrAreaForm;
            }

            OcrAreaForm firstArea = FormManager.Instace.GetOCRArea(1);
            if (firstArea != null && !firstArea.IsDisposed)
            {
                return firstArea;
            }

            if (FormManager.Instace.OcrAreaFormList.Count > 0)
            {
                firstArea = FormManager.Instace.OcrAreaFormList[0];
                if (firstArea != null && !firstArea.IsDisposed)
                {
                    return firstArea;
                }
            }

            return null;
        }

        public void Dispose()
        {
            SetEnabled(false);
            _timer.Dispose();
        }
    }
}
