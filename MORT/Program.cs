using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MORT
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
                string strCurrentProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper();


                System.Diagnostics.Process[] myProc = System.Diagnostics.Process.GetProcessesByName(strCurrentProcess);
                // 여기서 Mulpumi는 프로젝트 속성의 프로젝트 이름

                if (myProc.Length < 2)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("이미 실행중입니다.");
                    Application.Exit();
                }
        }

  

        
    }
}
