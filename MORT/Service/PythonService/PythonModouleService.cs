using Python.Deployment;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.Service.PythonService
{
    public class PythonModouleService
    { 
        private string _embedPythonFile = "python-3.8.10-embed-amd64.zip";
        private string _embedPython = "python-3.8.10-embed-amd64";
        private bool _ininted;

        public async Task InitAsync()
        {
            if(_ininted)
            {
                return;
            }

            try
            {
                Runtime.PythonDLL = Path.GetFullPath(".") + @"\python-3.8.10-embed-amd64\python38.dll";
                Python.Deployment.Installer.Source = new Python.Deployment.Installer.EmbeddedResourceInstallationSource()
                {
                    Assembly = typeof(Program).Assembly,
                    ResourceName = _embedPythonFile,
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

        public void DeletePip()
        {
            string path = Path.GetFullPath(".") + $"\\{_embedPython}";
            if (Path.Exists(path))
            {
                Console.WriteLine("Exist");
                Directory.Delete(path, true);
            }
        }

        public async Task InstallModouleAsync(string modoule, bool force = false)
        {
            if(!_ininted)
            {
                return;
            }

            await Installer.PipInstallModule(modoule, force:force);
        }

        public bool IsPipInstalled()
        {
            string path = Path.GetFullPath(".") + $"\\{_embedPython}";

            if (!Path.Exists(path))
            {
                return false;
            }

            return true;
        }

        public bool IsInstalled(string modoule)
        {
            string path = Path.GetFullPath(".") + $"\\{_embedPython}";

            if (!Path.Exists(path))
            {
                return false;
            }

            if (!Installer.IsModuleInstalled(modoule))
            {
                return false;
            }
            
            return true;
        }

    }
}
