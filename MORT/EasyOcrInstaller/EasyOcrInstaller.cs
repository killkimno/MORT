using Microsoft.VisualBasic.Logging;
using MORT.Manager;
using MORT.OcrApi.EasyOcr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Chat;

namespace MORT.EasyOcrInstaller
{
    public partial class EasyOcrInstaller : Form
    {
        private Action _closeCallback;
        private OcrManager _ocrManager;
        private bool _linkedLog;
        public EasyOcrInstaller()
        {
            InitializeComponent();
        }

        public void Show(OcrManager ocrManager, Action closeCallback)
        {
            _closeCallback = closeCallback;
            if (!_linkedLog)
            {
                Python.Deployment.Installer.LogMessage += OnUpdateLog;
                _linkedLog = true;
            }

            tbLog.ScrollToCaret();

            _ocrManager = ocrManager;
            this.Activate();
            this.Show();
        }

        private async Task InstallEasyOcrAsync(bool enableGpu)
        {
            var taskA = Task.Run(async () =>
            {
                await _ocrManager.PrepareEasyOcrAsync("en", enableGpu);
                OnUpdateLog("끝");
            });
                        //await ocrManager (enableGpu);
        }

        private void OnUpdateLog(string log)
        {
            tbLog.AppendText(log);
            //this.BeginInvoke(() => tbLog.AppendText(log));

        }


        private void EasyOcrInstaller_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_linkedLog)
            {
                Python.Deployment.Installer.LogMessage -= OnUpdateLog;
                _linkedLog = false;
            }
            _closeCallback?.Invoke();

        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            _ = InstallEasyOcrAsync(false);
        }
    }
}
