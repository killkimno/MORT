using Python.Deployment;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Service.PythonService
{
    public class PythonModouleService
    { 
        private string _embedPython = "python-3.8.10-embed-amd64.zip";
        private bool _ininted;

        public async Task InitAsync()
        {
            if(_ininted)
            {
                return;
            }

            try
            {
                Runtime.PythonDLL = @"D:\project\MORT\64bit\MORT\MORT\bin\x64\Release\net7.0-windows10.0.22621.0\python-3.8.10-embed-amd64\python38.dll";
                Python.Deployment.Installer.Source = new Python.Deployment.Installer.EmbeddedResourceInstallationSource()
                {
                    Assembly = typeof(Program).Assembly,
                    ResourceName = _embedPython,
                };

                Python.Deployment.Installer.InstallPath = Path.GetFullPath(".");

                //Console.WriteLine(PythonEngine.PythonPath);
                
                
                Installer.InstallPath = Path.GetFullPath(".");
                Python.Deployment.Installer.LogMessage += Console.WriteLine;
                await Python.Deployment.Installer.SetupPython();
                await Installer.TryInstallPip();
                 PythonEngine.Initialize();
                var _threadState = PythonEngine.BeginAllowThreads();
                _ininted = true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                Python.Deployment.Installer.LogMessage -= Console.WriteLine;
            }         
           
        }

        public async Task InstallModouleAsync(string modoule)
        {
            if(!_ininted)
            {
                return;
            }

            await Installer.PipInstallModule(modoule);
        }

    }
}
