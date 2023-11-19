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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Chat;

namespace MORT.EasyOcrInstaller
{
    public partial class EasyOcrInstaller : Form
    {
        private List<string> _cudaCommandLineLinst = new List<string>()
        {
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu118",
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu121"
        };

        private Action _closeCallback;
        private OcrManager _ocrManager;
        private bool _linkedLog;

        private string _commandLine;
        private bool _useCustomCommandLine;

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

            pnMain.Visible = true;
            pnEnableGPU.Visible = false;
            pnLog.Visible = false;
        }

        private async Task InstallEasyOcrAsync(bool enableGpu, string commandLine = "")
        {

            pnMain.Visible = false;
            pnEnableGPU.Visible = false;
            pnLog.Visible = true;

            var taskA = Task.Run(async () =>
            {
                await _ocrManager.PrepareEasyOcrAsync("en", enableGpu, commandLine);
                OnUpdateLog("끝");
            });
            //await ocrManager (enableGpu);
        }

        private void ShowGpuSetting()
        {

            pnMain.Visible = false;
            pnEnableGPU.Visible = true;
            pnLog.Visible = false;
            ShowCudaVersion();

            cbCuda.SelectedIndex = 0;
            SetGpuCommand();
        }

        private void SetGpuCommand()
        {
            if (_useCustomCommandLine)
            {
                _commandLine = tbGpuLine.Text;
            }
            else
            {
                _commandLine = _cudaCommandLineLinst[cbCuda.SelectedIndex];
            }

            _commandLine = _commandLine.Replace("pip3 install ", "");

            if(_commandLine.IndexOf("pip3") == 0)
            {
                var regex = new Regex("pip3");
                _commandLine = regex.Replace(_commandLine, "pip", 1);
            }

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

        private void ShowCudaVersion()
        {
            _useCustomCommandLine = false;
            cbCuda.Enabled = true;
            tbGpuLine.Enabled = false;
        }

        private void ShowGpuCommandLineVerison()
        {
            _useCustomCommandLine = true;
            cbCuda.Enabled = false;
            tbGpuLine.Enabled = true;
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            _ = InstallEasyOcrAsync(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShowCudaVersion();
        }

        private void rbGpuCommandLine_CheckedChanged(object sender, EventArgs e)
        {
            ShowGpuCommandLineVerison();
        }

        private void btnEnableGpuInstallStep_Click(object sender, EventArgs e)
        {
            ShowGpuSetting();
        }

        private void btnEnableGpuInstall_Click(object sender, EventArgs e)
        {
            SetGpuCommand();
            _ = InstallEasyOcrAsync(true, _commandLine);
        }

    }
}
