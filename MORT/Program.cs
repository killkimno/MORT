using ABI.System;
using System;
using System.IO;
using System.Windows.Forms;

namespace MORT
{
    static class Program
    {
        public static bool IS_FORCE_QUITE = false;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            RunMort();
            return;                   
        }

        public static void RunMort()
        {
            try
            {
                Console.WriteLine("Main");
                bool isForceKill = false;

                var current = System.Diagnostics.Process.GetCurrentProcess();
                string strCurrentProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper();
                System.Diagnostics.Process[] myProc = System.Diagnostics.Process.GetProcessesByName(strCurrentProcess);
                // 여기서 Mulpumi는 프로젝트 속성의 프로젝트 이름

                bool enableMultipleRun = File.Exists("EnableMultipleRun.txt");

                if (File.Exists("MORT_2.exe"))
                {
                    foreach (var proc in myProc)
                    {
                        if (proc.Id != current.Id)
                        {
                            proc.WaitForExit();
                        }
                    }

                    File.Delete("MORT_2.exe");
                    isForceKill = true;
                }

                if (File.Exists("MORT_2.dll.config"))
                {
                    File.Delete("MORT_2.dll.config");
                }

                if (myProc.Length < 2 || isForceKill || enableMultipleRun)
                {
                    //  ApplicationConfiguration.Initialize(); 에서 모두 처리한다
                    //SetProcessDPIAware();
                    //Application.EnableVisualStyles();
                    //Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("이미 실행중입니다." + System.Environment.NewLine + "(Already running)");
                    Application.Exit();
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
