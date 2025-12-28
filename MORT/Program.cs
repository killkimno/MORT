using ABI.System;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Exception = System.Exception;

namespace MORT
{
    static class Program
    {
        private const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        // kernel32 APIs
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetDefaultDllDirectories(uint directoryFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr AddDllDirectory([MarshalAs(UnmanagedType.LPWStr)] string newDirectory);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool RemoveDllDirectory(IntPtr cookie);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);

        // store registration state so we can unregister on exit
        private static IntPtr _addDllDirCookie = IntPtr.Zero;
        private static bool _setDllDirectoryUsed = false;
        private static bool _dllPathRegistered = false;

        public static bool IS_FORCE_QUITE = false;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            EnsureDllPathRegistered();

            // unregister on exit
            Application.ApplicationExit += (s, e) => UnregisterDllPath();

            ApplicationConfiguration.Initialize();
            RunMort();
            return;
        }

        static void EnsureDllPathRegistered()
        {
            return;
            if(_dllPathRegistered) return;

            try
            {
                string dllPath = Path.Combine(AppContext.BaseDirectory, "DLL");
                Directory.CreateDirectory(dllPath);

                bool setDefaultSucceeded = false;
                try
                {
                    setDefaultSucceeded = SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS);
                    Debug.WriteLine($"SetDefaultDllDirectories returned: {setDefaultSucceeded}");
                }
                catch(EntryPointNotFoundException)
                {
                    Debug.WriteLine("SetDefaultDllDirectories not available on this OS.");
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"SetDefaultDllDirectories exception: {ex.Message}");
                }

                // Try AddDllDirectory (preferred)
                try
                {
                    IntPtr cookie = AddDllDirectory(dllPath);
                    if(cookie != IntPtr.Zero)
                    {
                        _addDllDirCookie = cookie;
                        _dllPathRegistered = true;
                        Debug.WriteLine($"AddDllDirectory succeeded: {dllPath}");
                        return;
                    }
                    else
                    {
                        int err = Marshal.GetLastWin32Error();
                        Debug.WriteLine($"AddDllDirectory returned NULL. Win32Error={err}");
                    }
                }
                catch(EntryPointNotFoundException)
                {
                    Debug.WriteLine("AddDllDirectory not available on this OS.");
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"AddDllDirectory exception: {ex.Message}");
                }

                // Fallback: SetDllDirectory (less secure) — only if AddDllDirectory not available/failed
                try
                {
                    if(SetDllDirectory(dllPath))
                    {
                        _setDllDirectoryUsed = true;
                        _dllPathRegistered = true;
                        Debug.WriteLine($"SetDllDirectory succeeded (fallback): {dllPath}");
                    }
                    else
                    {
                        int err = Marshal.GetLastWin32Error();
                        Debug.WriteLine($"SetDllDirectory failed: {err}");
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"SetDllDirectory exception: {ex.Message}");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"EnsureDllPathRegistered overall exception: {ex.Message}");
            }
        }

        static void UnregisterDllPath()
        {
            return;
            try
            {
                if(_addDllDirCookie != IntPtr.Zero)
                {
                    if(RemoveDllDirectory(_addDllDirCookie))
                    {
                        Debug.WriteLine("RemoveDllDirectory succeeded.");
                    }
                    else
                    {
                        Debug.WriteLine($"RemoveDllDirectory failed: {Marshal.GetLastWin32Error()}");
                    }
                    _addDllDirCookie = IntPtr.Zero;
                }
                else if(_setDllDirectoryUsed)
                {
                    // reset to default
                    SetDllDirectory(null);
                    _setDllDirectoryUsed = false;
                    Debug.WriteLine("SetDllDirectory reset to null.");
                }
                _dllPathRegistered = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"UnregisterDllPath exception: {ex.Message}");
            }
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
                bool enableMultipleRun = File.Exists("EnableMultipleRun.txt");

                if(File.Exists("MORT_2.exe"))
                {
                    foreach(var proc in myProc)
                    {
                        if(proc.Id != current.Id)
                        {
                            proc.WaitForExit();
                        }
                    }

                    File.Delete("MORT_2.exe");
                    isForceKill = true;
                }

                if(File.Exists("MORT_2.dll.config"))
                {
                    File.Delete("MORT_2.dll.config");
                }

                if(myProc.Length < 2 || isForceKill || enableMultipleRun)
                {
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("이미 실행중입니다." + System.Environment.NewLine + "(Already running)");
                    Application.Exit();
                }
            }
            catch(System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}