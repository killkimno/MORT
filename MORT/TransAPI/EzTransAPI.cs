using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MORT.TransAPI
{


    public class EzTransAPI
    {


        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(String fileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi)]
        private extern static IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool J2K_InitializeEx(IntPtr ptr, IntPtr key);


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool J2K_FreeMem(IntPtr krStr);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate IntPtr J2K_TranslateMMNT(int data0, byte[] krStr); //char *형식 code page 932 요구
 
      



        public IntPtr[] ezt_addr = new IntPtr[20];
        private J2K_TranslateMMNT DoTransFunc;
        private J2K_FreeMem DoFreeMemFunc;
        private bool isInit = false;

        public bool IsInit
        {
            get
            {
                return isInit;
            }
        }
        public void LoadDll()
        {
            var dllFile = new FileInfo(@".\ExternDll\J2KEngine.dll");
            IntPtr libHandle = LoadLibrary(dllFile.FullName);
            if (libHandle == IntPtr.Zero)
            {
                Console.WriteLine("파일 불러오기 실패!");
            }                
            else
            {
                Console.WriteLine("불러온 파일 핸들: 0x{0:X8}", libHandle.ToInt32());

                /*
                ezt_addr[0] = GetProcAddress(libHandle, "J2K_FreeMem");
                ezt_addr[1] = GetProcAddress(libHandle, "J2K_GetPriorDict");
                ezt_addr[2] = GetProcAddress(libHandle, "J2K_GetProperty");
                ezt_addr[3] = GetProcAddress(libHandle, "J2K_Initialize");
                ezt_addr[4] = GetProcAddress(libHandle, "J2K_InitializeEx");
                ezt_addr[5] = GetProcAddress(libHandle, "J2K_ReloadUserDict");
                ezt_addr[6] = GetProcAddress(libHandle, "J2K_SetDelJPN");
                ezt_addr[7] = GetProcAddress(libHandle, "J2K_SetField");
                ezt_addr[8] = GetProcAddress(libHandle, "J2K_SetHnj2han");
                ezt_addr[9] = GetProcAddress(libHandle, "J2K_SetJWin");
                ezt_addr[10] = GetProcAddress(libHandle, "J2K_SetPriorDict");
                ezt_addr[11] = GetProcAddress(libHandle, "J2K_SetProperty");
                ezt_addr[12] = GetProcAddress(libHandle, "J2K_StopTranslation");
                ezt_addr[13] = GetProcAddress(libHandle, "J2K_Terminate");
                ezt_addr[14] = GetProcAddress(libHandle, "J2K_TranslateChat");
                ezt_addr[15] = GetProcAddress(libHandle, "J2K_TranslateFM");
                ezt_addr[16] = GetProcAddress(libHandle, "J2K_TranslateMM");
                ezt_addr[17] = GetProcAddress(libHandle, "J2K_TranslateMMEx");
                ezt_addr[18] = GetProcAddress(libHandle, "J2K_TranslateMMNT");
                ezt_addr[19] = GetProcAddress(libHandle, "J2K_TranslateMMNTW");
                */
                var dat = new FileInfo(@".\ExternDLL\Dat");
                unsafe
                {
                    var func = (J2K_InitializeEx)Marshal.GetDelegateForFunctionPointer(GetProcAddress(libHandle, "J2K_InitializeEx"), typeof(J2K_InitializeEx));
                    DoTransFunc = (J2K_TranslateMMNT)Marshal.GetDelegateForFunctionPointer(GetProcAddress(libHandle, "J2K_TranslateMMNT"), typeof(J2K_TranslateMMNT));
                    DoFreeMemFunc = (J2K_FreeMem)Marshal.GetDelegateForFunctionPointer(GetProcAddress(libHandle, "J2K_FreeMem"), typeof(J2K_FreeMem));

                    isInit = func(Marshal.StringToHGlobalAnsi("CSUSER123455"), Marshal.StringToHGlobalAnsi(dat.FullName ));

                    if(isInit)
                    {
                        Console.WriteLine("성공");
                    }
                    else
                    {

                        Console.WriteLine("망");
                    }
                }

                isInit = true;
            }
        }


        public string DoTrsans(string original)
        {
            string result = "";

            StringBuilder sb = new StringBuilder(original);


            //932 형식으로 인코딩 후 byte[] 형식으로 데이터 보냄
            Encoding enc = Encoding.GetEncoding(932);
            var data = enc.GetBytes(original);

            IntPtr ptr = DoTransFunc(0, data);

            result = Marshal.PtrToStringAnsi(ptr);
            DoFreeMemFunc(ptr);;

            return result;
        }


        

    }
}
