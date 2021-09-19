//만든이 : 몽키해드
//블로그 주소 : https://blog.naver.com/killkimno

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace MORT
{    

    public enum eCurrentStateType
    {
        None, Init, LoadFile, SaveFile, Accept, SetDefault,
    }

    public partial class Form1 : Form
    {
        public static bool IsLockHotKey = false;

        //개발용 버전인가?
        public readonly bool IsDevVersion = false;

        public class ImgData
        {
            public int channels;
            public byte[] data;

            public int x;
            public int y;

            public int index;

            public void ClearList(List<byte> lista)
            {
                if (lista == null)
                {
                    return;
                }
                //todo
                //https://jacking75.github.io/NET_Span_5_Reasons_to_Use/ span을 이용 하던가
                //byte 를 intptr 로 보낸 후 마샬링 하던가 해야 함
                //IntPtr nativeMem = Marshal.AllocHGlobal(1000000);
                lista.Clear();
                int identificador = GC.GetGeneration(lista);
                GC.Collect(identificador, GCCollectionMode.Forced);

                lista.TrimExcess();
                lista = null;
            }

            public void Clear()
            {
                Array.Clear(data, 0, data.Length);

                int identificador = GC.GetGeneration(data);
                GC.Collect(identificador, GCCollectionMode.Forced);
                data = null;

                //ClearList(rList);
                //ClearList(gList);
                //ClearList(bList);
            }


        }


        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::

        private eCurrentStateType eCurrentState = eCurrentStateType.None;
        public delegate void PDelegateSetSpellCheck();

        string nowOcrString = "";                   //현재 ocr 문장

        //현재 버전
        int nowVersion = 1171;

        //IntPtr observerHwnd;
        //번역 쓰레드
        Thread thread;
        volatile bool isEndFlag = false;            //번역 끝내는 플레그
        bool isTranslateFormTopMostFlag = true;     //번역창이 최상위냐 아니냐

        //enum Skin {dark, layer };                  //스킨 열거형
        //Skin nowSkin = Skin.dark;                   //현재 스킨 - 다크
        private Point mousePoint;                   //창 이동 관련
        int ocrProcessSpeed = 2000;                 //ocr 처리 딜레이 시간
        private bool isBeforeSnapShot = false;       //마지막 스냅샷 했는가? 

        //폰트 관련
        Font textFont;
        Color textColor;
        Color outlineColor1;
        Color outlineColor2;
        Color backgroundColor;

        public bool IsUseClipBoardFlag
        {
            set
            {
                MySettingManager.NowIsSaveInClipboardFlag = value;
            }

            get
            {
                return MySettingManager.NowIsSaveInClipboardFlag;
            }
        }

        int nowColorGroupIndex = 0;                 //색 그룹 수

        List<int> locationXList = new List<int>();
        List<int> locationYList = new List<int>();
        List<int> sizeXList = new List<int>();
        List<int> sizeYList = new List<int>();

        //제외 영역 관련.
        List<int> exceptionLocationXList = new List<int>();
        List<int> exceptionLocationYList = new List<int>();
        List<int> exceptionSizeXList = new List<int>();
        List<int> exceptionSizeYList = new List<int>();

        List<ColorGroup> colorGroup = new List<ColorGroup>();   //색 그룹 리스트

        bool isProcessTransFlag = false;

        SettingManager.TransType transType;

        string yandexKey = "";
        string naverIDKey = "";
        string naverSecretKey = "";

        private List<string> winLanguageCodeList = new List<string>();
        public List<string> WinLanguageCodeList
        {
            get { return winLanguageCodeList; }
        }


        bool isProgramStart = false;                //모든게 다 로딩이 되었나
        public bool isAvailableWinOCR = true;           //윈도우 10 OCR 사용 가능한지 확인.
        private string winOcrErrorCode = "";
        public bool isShowWinOCRWarning = false;
        public SettingManager MySettingManager = new SettingManager(); //설정 관리자
        GlobalKeyboardHook gHook;
        List<int> nowKeyPressList = new List<int>();


        private bool isDebugUnlockOCRSpeed = false;
        public static bool isDebugShowFormerResultLog = false;
        public static bool isDebugTransOneLine = false;
        public static bool isDebugShowWordArea = false;


        private List<KeyInputLabel> inputKeyUIList = new List<KeyInputLabel>();

        #region ::::::::::::::::::::::::::DLL:::::::::::::::::::::::::::::::::::::::::::::::::

        //MORT_CORE 침식함수
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setErode();

        //MORT_CORE 내부 동작 함수
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void processOcr(StringBuilder test, StringBuilder test1);



        //MORT_CORE 내부 동작 함수 - 데이터도 같이 보냄
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void processOcrWithData(StringBuilder ocrBuilder, StringBuilder resultBuilder,
            int width, int height, int positionX, int positionY, [In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] data);



        //MORT_CORE 스펠링 체크
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetSpellingCheck(StringBuilder ocrResult, bool isUseJpn);

        //MORT_CORE DB만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetDBText(StringBuilder original, StringBuilder result);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr processGetImgData(int index, ref int x, ref int y, ref int channels, ref int locationX, ref int locationY);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr processGetImgDataFromByte(int index, int width, int height, int positionX, int positionY, [In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] data, ref int x, ref int y, ref int channels);

        //MORT_CORE 이미지 영역 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCutPoint(int[] newX, int[] newY, int[] newX2, int[] newY2, int size);

        //MORT_CORE 제외 영역 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetExceptPoint(int[] newX, int[] newY, int[] newX2, int[] newY2, int size);
        //

        //MORT_CORE 초기화
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initOcr();

        //MORT_CORE 폰트 교육자료 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTessdata(string tessData, bool isUseJpnFlag);

        //MORT_CORE RGB, HSV 값 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setFiducialValue(int[] newValueR, int[] newValueG, int[] newValueB, int[] newValueS1, int[] newValueS2, int[] newValueV1, int[] newValueV2, int size);


        //디버그 모드 활성화.
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsDebugMode(bool isDebug, bool isShowReplace, bool isSaveCapture, bool isSaveCaptureResult);

        //MORT_CORE 빙 / DB 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setUseDB(bool newIsUseDBFlag, bool IsUsePartialDB, string newDbFileText);

        //MORT_CORE 교정 사전 사용
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setUseCheckSpelling(bool newIsUseCheckSpellingFlag, bool isMatchingWord, string newDicFileText);

        //MORT_CORE 이미지 보정 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setAdvencedImgOption(bool newIsUseRGBFlag, bool newIsUseHSVFlag, bool newIsUseErodeFlag, float imgZoomSize , bool isUseThreshold, int thresholdValue);

        //MORT_CORE NHocr 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsUseNHocr(bool isUseNHocr);

        //MORT_CORE isUseJPN 강제 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsUseJpn(bool _isUseJpn);

        //MORT_CORE 대소문자 구분 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsStringUpper(bool isUpper);

        //MORT_CORE 공백제거
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetRemoveSpace(bool isRemove);

        //MORT_CORE OCR 영역 인덱스 표시
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetShowOCRIndex(bool isShow);

        //MORT_CORE 활성화 윈도우 캡쳐 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsActiveWindow(bool isActiveWindow);

        //MORT_CORE 사용할 색그룹 추가
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddOcrColorSet(int[] colorList, int size);

        //MORT_CORE 사용할 색그룹 초기화
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearOcrColorSet();

        class Loader : MarshalByRefObject
        {
            public override object InitializeLifetimeService()
            {
                return null;
            }

            public void LoadAssembly(string path)
            {
                _assembly = Assembly.Load(AssemblyName.GetAssemblyName(path));
            }

            public void InitFunc()
            {
                Module[] modules = _assembly.GetModules();

                for (int i = 0; i < modules.Length; i++)
                {
                   // Console.WriteLine("sdfsdfdsfsf" + modules[i].ToString());
                }

                //loader.Initialize(1, "test2.Class1", "Test");
                //Type type = Type.GetType("namespace.qualified.TypeName, MORT_WIN10OCR.Class1");
                //Type type = modules[0].GetType("MORT_WIN10OCR.Class1");

                Type type = _assembly.GetType("MORT_WIN10OCR.Class1");
                MethodInfo method = type.GetMethod("StartMakeBitmap", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method2 = type.GetMethod("ProcessOCR", BindingFlags.Static | BindingFlags.Public);

                MethodInfo method3 = type.GetMethod("GetIsAvailable", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method4 = type.GetMethod("InitOcr", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method5 = type.GetMethod("GetIsAvailableDLL", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method6 = type.GetMethod("GetText", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method7 = type.GetMethod("GetAvailableLanguageList", BindingFlags.Static | BindingFlags.Public);

                MethodInfo method8 = type.GetMethod("TestMar", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method9 = type.GetMethod("TextToSpeach", BindingFlags.Static | BindingFlags.Public);


                MethodInfo method11 = type.GetMethod("SetBitMap", BindingFlags.Static | BindingFlags.Public);






                makeBitmap = (Action)Delegate.CreateDelegate(typeof(Action), method);
                processOCRFunc = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method2);
                getTextFunc = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method6);
                getDLLAvailableFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method5);
                getOCRAvailableFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method3);
                initOCRFunc = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), method4);
                getLanguageListFunc = (Func<List<string>>)Delegate.CreateDelegate(typeof(Func<List<string>>), method7);

                textToSpeachFunc = (Action<string, int>)Delegate.CreateDelegate(typeof(Action<string, int>), method9);
                testFunc = (Func<IntPtr>)Delegate.CreateDelegate(typeof(Func<IntPtr>), method8);

                SetBitmapFunc = (Action<byte[], int, int, int>)Delegate.CreateDelegate(typeof(Action<byte[], int, int, int>), method11);

            }


            public List<string> GetLanguageList()
            {
                return getLanguageListFunc();
            }

            public string GetText()
            {
                return getTextFunc();
            }

            public IntPtr GetMar()
            {
                return testFunc();
            }

            public void InitOCR(string code)
            {
                initOCRFunc(code);
            }

            public bool GetIsAvailableOCR()
            {
                return getOCRAvailableFunc();
            }

            public void TextToSpeach(string text, int type)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    textToSpeachFunc(text, type);
                }

            }

            public void MakeBitMap()
            {
                makeBitmap();

            }



            public void SetImg(byte[] data, int channels, int x, int y)
            {
                SetBitmapFunc(data, channels, x, y);
            }

            public string ProcessOcrFunc()
            {
                string result = "yes";
                result = processOCRFunc();


                return result;
            }

            private Assembly _assembly;
            public Action makeBitmap;

            public Action<byte[], int, int, int> SetBitmapFunc;
            public Func<string> processOCRFunc;       //OCR 처리하기.
            public Func<string> getTextFunc;       //OCR 처리하기.
            public Func<bool> getDLLAvailableFunc;          //DLL 사용 가능한지 확인.
            public Func<bool> getOCRAvailableFunc;          //OCR 사용 가능한지 확인.
            public Action<string> initOCRFunc;          //OCR 사용 가능한지 확인.
            public Func<List<string>> getLanguageListFunc;  //사요 가능한 언어 가져오기.

            public Action<string, int> textToSpeachFunc;           //tts

            public Func<IntPtr> testFunc;   //마샬링 테스트.


        }
        private static Loader loader;
        private static AppDomain Domain;

        private const string m_kDomainName = "myProgram";
        private const string m_kTargetFolder = "DLL";
        private const string m_kFilePath = ".\\";
        private const string m_kFileName = "MORT_WIN10OCR.dll";

        public void LoadDll()
        {
            if (Domain != null)
                AppDomain.Unload(Domain);

            string dest = Path.Combine(m_kFilePath, m_kTargetFolder, m_kFileName);

            Domain = AppDomain.CreateDomain(m_kDomainName);
            loader = (Loader)Domain.CreateInstanceAndUnwrap(typeof(Loader).Assembly.FullName, typeof(Loader).FullName);
            loader.LoadAssembly(dest);
            loader.InitFunc();       
        }


        #endregion


        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::Skin Change Function:::::::::::::::::::::::::::::::::::::::::::
        public void ChangeSkin()
        {

            bool isChange = false;
            if (MySettingManager.NowSkin == SettingManager.Skin.dark && !skinDarkRadioButton.Checked)
            {
                isChange = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer && !skinLayerRadioButton.Checked)
            {
                isChange = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.over && !skinOverRadioButton.Checked)
            {
                isChange = true;
            }
            else if (eCurrentState == eCurrentStateType.SetDefault || eCurrentState == eCurrentStateType.LoadFile)
            {
                isChange = true;
            }



            if (isChange)
            {
                FormManager.Instace.DestoryTransForm();

                if (skinDarkRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.dark;
                }
                else if (skinLayerRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.layer;
                }
                else if (skinOverRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.over;
                }
                MakeTransForm();
            }


            //번역창 위치 설정.    처음 킬 때 , 설정 파일을 부를 때만 번역창 위치를 설정한다.
            SetTransFormLocation();

        }
        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::폼 생성 관련 함수:::::::::::::::::::::::::::::::::::::::::::

        private void MakeSearchOptionForm()
        {
            FormManager.Instace.MakeSearchOptionForm();
        }

        //리모컨 생성 함수
        private void makeRTT()
        {
            FormManager.Instace.MakeRTT();
        }

        //교정사전 편집창 생성
        private void MakeDicEditorForm()
        {
            FormManager.Instace.MakeDicEditorForm(nowOcrString, MySettingManager.NowIsUseJpnFlag, MySettingManager.NowDicFile);

        }

        private void MakeQuickOcrForm()
        {

        }

        //번역창 생성 함수
        private void MakeLogo()
        {
            Logo logo = new Logo();
            logo.Name = "Logo";
            logo.StartPosition = FormStartPosition.CenterScreen;

            logo.Show();

            CheckVersion();

            if (!Program.IS_FORCE_QUITE)
            {
                CheckDefaultSetting();

                DateTime Tthen = DateTime.Now;
                do
                {
                    Application.DoEvents();

                } while (Tthen.AddSeconds(0.7f) > DateTime.Now);
                logo.disableLogo(2.0f);
            }
            else
            {
                logo.closeApplication();
                logo.Close();
            }

          

            //  Assembly assembly = Assembly.LoadFile(@"G:\Project\visualStudio Projects\MORT\MORT\bin\Release\test2.dll");
        }


        private void MakeTransForm()
        {
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                FormManager.Instace.MakeBasicTransForm(isTranslateFormTopMostFlag);
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                FormManager.Instace.MakeLayerTransForm(isTranslateFormTopMostFlag, isProcessTransFlag);
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                FormManager.Instace.MakeOverTransForm(isTranslateFormTopMostFlag, isProcessTransFlag);

            }

        }

        /// <summary>
        /// 번역창 위치 / 크기 설정
        /// </summary>
        private void SetTransFormLocation()
        {
            // 처음 킬 때 , 설정 파일을 부를 때만 번역창 위치를 설정한다.
            switch (eCurrentState)
            {
                case eCurrentStateType.Init:
                case eCurrentStateType.LoadFile:

                    if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                    {
                        if (FormManager.Instace.MyLayerTransForm != null)
                        {
                            TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;

                            int x = MySettingManager.transFormLocationX;
                            int y = MySettingManager.transFormLocationY;

                            int sizeX = MySettingManager.transFormSizeX;
                            int sizeY = MySettingManager.transFormSizeY;

                            if (sizeX < TransFormLayer.MIN_SIZE_X)
                            {
                                sizeX = TransFormLayer.MIN_SIZE_X;
                            }

                            if (sizeY < TransFormLayer.MIN_SIZE_Y)
                            {
                                sizeY = TransFormLayer.MIN_SIZE_Y;
                            }


                            //모두 -1이면 기본값이다.
                            if (x != -1 && y != -1 && sizeX != -1 && sizeY != -1)
                            {
                                //실제 모니터와 크기와 위치 보정하기
                                //Screen.PrimaryScreen.Bounds.Height
                                Rectangle rect = new Rectangle(x, y, sizeX, sizeY);
                                Screen screen = Screen.FromRectangle(rect);

                                bool isContain = false;
                                if (screen == null)
                                {
                                    Util.ShowLog("1Screen = null");
                                    isContain = false;

                                }
                                else
                                {
                                    Util.ShowLog("1Screen = not null " + screen.ToString());


                                    Rectangle Inter = Rectangle.Intersect(screen.Bounds, rect);
                                    if (Inter != null && Inter.Width > 0 && Inter.Height > 0)
                                    {

                                        isContain = true;
                                        //x축 검사.
                                        if (rect.X < screen.Bounds.X)
                                        {
                                            rect.X = screen.Bounds.X;
                                        }
                                        else if (screen.Bounds.X + screen.Bounds.Width < rect.X + rect.Width)
                                        {
                                            rect.X = (screen.Bounds.X + screen.Bounds.Width) - rect.Width;
                                        }

                                        //축 검사.
                                        if (rect.Y < screen.Bounds.Y)
                                        {
                                            rect.Y = screen.Bounds.Y;
                                        }
                                        else if (screen.Bounds.Y + screen.Bounds.Height < rect.Y + rect.Height)
                                        {
                                            rect.Y = (screen.Bounds.Y + screen.Bounds.Height) - rect.Height;
                                        }

                                    }
                                    else
                                    {
                                        Util.ShowLog("! not contain");
                                        isContain = false;


                                    }
                                }

                                if (!isContain)
                                {
                                    rect = new Rectangle(20, Screen.PrimaryScreen.Bounds.Height - 300, 973, 192);
                                }

                                transForm.Location = new Point(rect.X, rect.Y);
                                transForm.Size = new Size(rect.Width, rect.Height);


                            }
                        }
                    }

                    break;

                case eCurrentStateType.SetDefault:

                    if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                    {
                        if (FormManager.Instace.MyLayerTransForm != null)
                        {
                            TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;
                            transForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                            transForm.Size = new Size(973, 192);
                        }
                    }
                    break;

                case eCurrentStateType.Accept:

                    if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                    {
                        if (FormManager.Instace.MyLayerTransForm != null)
                        {
                            TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;

                            Screen screen = Screen.FromControl(transForm);

                            if (screen == null)
                            {
                                Util.ShowLog("Screen = null");
                            }
                            else
                            {
                                Util.ShowLog("Screen = not null " + screen.ToString());
                            }

                        }
                    }

                    break;

            }
        }

        #endregion

        #region :::::::::::::::::::::::::::::::::::::::::::버전 확인 관련 :::::::::::::::::::::::::::::::::::::::::

        private bool GetCheckUpdate()
        {
            bool isCheckUpdate = true;


            //구버전 파일은 없앤다.
            if(File.Exists(GlobalDefine.CHECK_UPDATE_FILE))
            {
                File.Delete(GlobalDefine.CHECK_UPDATE_FILE);
            }

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_UPDATE ", '[', ']');


            if(!Boolean.TryParse(result, out isCheckUpdate))
            {
                isCheckUpdate = true;
            }


            return isCheckUpdate;
        }

        private void SetCheckUpdate(bool isUse)
        {            

            Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_UPDATE ", isUse.ToString());

        }

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::초기화:::::::::::::::::::::::::::::::::::::::::::


        private void CheckGDI()
        {

            TransFormLayer.isActiveGDI = true;
            CustomLabel.isActiveGDI = true;

            try
            {
                using (GraphicsPath gp = new GraphicsPath())
                using (StringFormat sf = new StringFormat())
                {

                    Font textFont = FormManager.Instace.MyMainForm.MySettingManager.TextFont;
                    gp.AddString("테스트, どうした 1234!", textFont.FontFamily, (int)textFont.Style, 10, ClientRectangle, sf);
                    //  throw new System.InvalidOperationException("Logfile cannot be read-only");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                TransFormLayer.isActiveGDI = false;
                CustomLabel.isActiveGDI = false;
                if (DialogResult.OK == MessageBox.Show("GDI+ 가 작동하지 않습니다. \n레이어 번역창의 일부 기능을 사용할 수 없습니다.\n해결법을 확인해 보겠습니까? ", "GDI+ 에서 일반 오류가 발생했습니다.", MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70185869419");
                    }
                    catch { }
                }
            }


        }


        //파일로 부터 세팅 불러옴
        void LoadSettingfile(string fileName)
        {
            MySettingManager.LoadSettingfile(fileName);
            SetValueToUIValue();
        }

        /// <summary>
        /// 파일 불러오기 -> 파일값 ui에 적용 -> ui 값을 실제 설정에 적용
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenSettingFile(string fileName)
        {
            eCurrentState = eCurrentStateType.LoadFile;
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                LoadSettingfile(fileName);
                ApplyUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                LoadSettingfile(fileName);
                ApplyUIValueToSetting();
            }

            SaveSetting(GlobalDefine.USER_SETTING_FILE);
            eCurrentState = eCurrentStateType.None;
        }

        //색 그룹 초기화
        void initColorGroup()
        {
            colorGroup.Clear();
            colorGroup.Add(new ColorGroup());
            groupCombo.Items.Clear();
            groupCombo.Items.Add("추가");
            groupCombo.Items.Add("삭제");
            groupCombo.Items.Add("1");
            groupCombo.SelectedIndex = 2;
        }

        //단축키를 위한 키 후킹 기능 초기화
        void initKeyHooker()
        {
            gHook = new GlobalKeyboardHook(); // Create a new GlobalKeyboardHook
            // Declare a KeyDown Event
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            gHook.KeyUp += new KeyEventHandler(gHook_KeyUp);
            // Add the keys you want to hook to the HookedKeys list
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                gHook.HookedKeys.Add(key);
            }

            gHook.hook();
        }

        /// <summary>
        /// MORT 버전 확인
        /// </summary>
        /// <param name="content"></param>
        private void CheckMortVersion(String content)
        {
            try
            {
                if (content != null)
                {
                    string versionKey = "@MORT_VERSION ";
                    string minorKey = "@MORT_MINOR_VERSION ";
                    string newVersionString = "";
                    string minorVersionString = "";                    
                    string downloadPage = "";

                    if(IsDevVersion)
                    {
                        versionKey = "@DEV_VERSION ";
                        minorKey = "@DEV_MINOR_VERSION ";
                    }

                    newVersionString = Util.ParseString(content, versionKey, '[', ']');
                    downloadPage = Util.ParseString(content, versionKey, '{', '}');
                    minorVersionString = Util.ParseString(content, minorKey, '[', ']');



                    UpdateType updateType = Util.GetUpdateType(nowVersion, newVersionString, minorVersionString);



                    Util.ShowLog("------------------" + System.Environment.NewLine +  "Now : " + nowVersion + " / New : " + newVersionString + " / Minor : "  + minorVersionString + " / Result : " + updateType.ToString());

                    //1. 버전 비교를 한다.
                    if (updateType == UpdateType.Major)
                    {
                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        string checkMessageSubtitle = "(주요 버전 업데이트 " + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show("새로운 버전을 확인했습니다.\r\n업데이트 하시겠습니까?\r\n\r\n다운로드 페이지로 이동합니다", checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            Logo.SetTopmost(false);
                            try
                            {
                                Logo.SetTopmost(true);
                                isTranslateFormTopMostFlag = false;
                                setTranslateTopMostToolStripMenuItem.Checked = false;
                                System.Diagnostics.Process.Start(downloadPage);
                            }
                            catch { }
                        }
                        else
                        {

                        }
                    }
                    else if(updateType == UpdateType.Minor)
                    {

                        string fileUrl = Util.ParseString(content, minorVersionString, '{', '}');


                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        string checkMessageSubtitle = "(마이너 버전 업데이트 " + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show("마이너 버전을 확인했습니다.\r\n업데이트 하시겠습니까?\r\n\r\n업데이터를 실행합니다", checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            var updater = new  MORT.Updater.Updater();
                            updater.Show();
                            updater.DoDownload(newVersionString, fileUrl, downloadPage);



                            Program.IS_FORCE_QUITE = true;

                            return;
                        }
                        else
                        {

                        }

                    }
                    else if(updateType == UpdateType.None)
                    {
                        if(IsDevVersion)
                        {
                            int finishedVersion = Util.ParseInt(content, "@MORT_DEV_FINISHED_VERSION ");
                            
                            if (finishedVersion >= nowVersion)
                            {
                                string checkMessageSubtitle = "정식버전이 나왔습니다";
                                if (DialogResult.OK == MessageBox.Show("정식버전이 나왔습니다.\r\n정식 버전을 받겠습니까?\r\n\r\n다운로드 페이지로 이동합니다", checkMessageSubtitle, MessageBoxButtons.OKCancel))
                                {
                                    Logo.SetTopmost(false);
                                    try
                                    {
                                        Logo.SetTopmost(true);
                                        isTranslateFormTopMostFlag = false;
                                        setTranslateTopMostToolStripMenuItem.Checked = false;
                                        downloadPage = Util.ParseString(content, "@MORT_VERSION ", '{', '}');

                                        System.Diagnostics.Process.Start(downloadPage);
                                    }
                                    catch { }
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

        }

        private void UpdateDic(string fileName, String data)
        {
            //Util.ShowLog("Dic data : " + data);
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    using (StreamWriter file = new StreamWriter(fileName, false, utf8WithoutBom))
                    {
                        file.WriteLine(data);
                        file.Write(System.Environment.NewLine);

                        Util.ShowLog("Dic update complete");
                    }

                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter file = new StreamWriter(fileName, true, utf8WithoutBom))
                        {
                            file.WriteLine(data);
                            file.Write(System.Environment.NewLine);
                        }
                    }
                }
            }
        }

        private void CheckDicVersion(String content, string checkType, string fileName)
        {
            try
            {
                if (content != null)
                {
                    int dicVersion = 0;
                    string result = Util.ParseStringFromFile(GlobalDefine.DATA_VERSION_FILE, checkType, '[', ']');

                    if (!string.IsNullOrEmpty(result))
                    {
                        dicVersion = Convert.ToInt32(result);
                    }


                    string newVersionString = "";
                    string downloadPage = "";

                    newVersionString = Util.ParseString(content, checkType, '[', ']');
                    downloadPage = Util.ParseString(content, checkType, '{', '}');
                    //var path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                    Util.ShowLog(checkType + " : " + dicVersion + " / " + newVersionString + " / download : " + downloadPage);

                    if (dicVersion < Convert.ToInt32(newVersionString))
                    {
                        using (WebClient client = new WebClient())
                        {
                            Stream stream = client.OpenRead(downloadPage);
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                String dicData = reader.ReadToEnd();
                                dicData = dicData.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                                UpdateDic(fileName, dicData);
                                Util.ChangeFileData(GlobalDefine.DATA_VERSION_FILE, checkType, newVersionString, '[', ']');

                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }


        //버전 확인.
        void CheckVersion()
        {
            if (GetCheckUpdate())
            {
                try
                {
                    nowVersion = Properties.Settings.Default.MORT_VERSION_VALUE;
                    //http://killkimno.github.io/MORT_VERSION/version.txt
                    using (WebClient client = new WebClient())
                    {
                        Stream stream = client.OpenRead("http://killkimno.github.io/MORT_VERSION/version.txt");
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            String content = reader.ReadToEnd();
                            content = content.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                            Util.ShowLog("--- Version : " + content);

                            CheckMortVersion(content);

                            if (!Program.IS_FORCE_QUITE)
                            {
                                CheckDicVersion(content, "@MORT_DIC_ENG", @".\\DIC\\dic.txt");
                                CheckDicVersion(content, "@MORT_DIC_JPN", @".\\DIC\\dicJpn.txt");
                            }
                       

                        }

                    }



                }
                catch (Exception e)
                {
                    Util.ShowLog(e.ToString());
                }
            }

        }

        private void CheckDefaultSetting()
        {
            try
            {
                //http://killkimno.github.io/MORT_VERSION/default_setting.txt
                using (WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead("http://killkimno.github.io/MORT_VERSION/default_setting.txt");
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        String content = reader.ReadToEnd();
                        content = content.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                        Util.ShowLog("--- default setting : " + content);

                        string naver = Util.ParseString(content, "@SPLITE_NAVER_TOKEN", '{', '}');
                        string google = Util.ParseString(content, "@SPLITE_GOOGLE_TOEKN ", '{', '}');
                        string isUseAdvence = Util.ParseString(content, "@ENABLE_ADVENCED_TOKEN", '{', '}');
                        if (naver != "" && google != "")
                        {
                            Util.SetSpliteToken(naver, google, isUseAdvence);
                        }
                    }

                }



            }
            catch (Exception e)
            {
                Util.ShowLog(e.ToString());
            }
        }

        private void InitTransCode()
        {
            naverTransComboBox.SelectedIndex = 0;

            googleTransComboBox.SelectedIndex = 0;
            googleResultCodeComboBox.SelectedIndex = 0;

            TransManager.Instace.InitTransCode(naverTransComboBox, cbNaverResultCode, googleTransComboBox, googleResultCodeComboBox);
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();
        //폼 생성
        public Form1()
        {
            try
            {
                eCurrentState = eCurrentStateType.Init;

                //SetProcessDPIAware();               


                InitializeComponent();

                this.Text = $"Monkeyhead's OCR RealTime Translator - {Properties.Settings.Default.MORT_VERSION}";

                plDebugOff.Visible = true;
                plDebugOn.Visible = false;

                FormManager.Instace.MyMainForm = this;
                notifyIcon1.Visible = false;

                NaverTranslateAPI.instance = new NaverTranslateAPI();
                YandexAPI.instance = new YandexAPI();
                GoogleBasicTranslateAPI.instance = new GoogleBasicTranslateAPI();

                isAvailableWinOCR = true;
                try
                {
                    //윈도우 10 ocr 설정.
                    LoadDll();
                    List<string> codeList = loader.GetLanguageList();

                    //test
                    //codeList.Clear();
                    //codeList.Add("ko,한국어");
                    //


                    WinOCR_Language_comboBox.Items.Clear();
                    for (int i = 0; i < codeList.Count; i++)
                    {
                        string[] key = codeList[i].Split(',');
                        if (key.Length >= 2)
                        {
                            winLanguageCodeList.Add(key[0]);
                            WinOCR_Language_comboBox.Items.Add(key[1]);
                        }
                    }

                    if (winLanguageCodeList.Count > 0)
                    {
                        WinOCR_Language_comboBox.SelectedIndex = 0;
                        loader.InitOCR(winLanguageCodeList[0]);
                    }
                    else
                    {
                        loader.InitOCR("");
                    }

                }
                catch (Exception e)
                {
                    isAvailableWinOCR = false;
                    winOcrErrorCode = e.Message;
                }


                OpenNaverKeyFile();
                OpenGoogleKeyFile();


                TransManager.Instace.InitFormerDic();
                InitHotKey();
                InitTransCode();


                LoadSettingfile(GlobalDefine.USER_SETTING_FILE);
                initOcr();
                //GDI+ 동작 여부 검사.
                CheckGDI();
                MakeLogo();

           

                if (!Program.IS_FORCE_QUITE)
                {
                    AdvencedOptionManager.Init();
                    MakeTransForm();
                    ApplyUIValueToSetting();


                    makeRTT();
                    initKeyHooker();

                    notifyIcon1.Visible = true;
                    isProgramStart = true;

                    
                    ApplyAdvencedOption();
                }
                else
                {
                    this.Opacity = 0;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                try
                {
                    System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70185869419");
                }
                catch { }
                this.Close();
            }

            eCurrentState = eCurrentStateType.None;
        }

        //폼이 불러온 후 처리함.
        private void Form1_Load(object sender, EventArgs e)
        {
            if(Program.IS_FORCE_QUITE)
            {
                this.Opacity = 0;
                this.Hide();
                return;
            }

            using (Graphics graphics = this.CreateGraphics())
            {
                Util.SetDPI(graphics.DpiX, graphics.DpiY);

                int width = tabControl1.ItemSize.Width;
                int height = tabControl1.ItemSize.Height;

                if (Util.dpiMulti > 1)
                {
                    width = width + (int)(Util.dpiMulti / 2 * width);
                    height = (int)(tabControl1.ItemSize.Height * Util.dpiMulti);

                    tabControl1.ItemSize = new Size(width, height);
                    pictureBox1.Size = new Size(height, pictureBox1.Height);
                }

            }

            //비활성화 -> 빠른 설정 탭으로
            if(!cbSetBasicDefaultPage.Checked)
            {
                tabControl1.SelectedIndex = 5;
            }
          

            //툴팁 초기화.
            toolTip_OCR.SetToolTip(showOcrCheckBox, Properties.Settings.Default.TOOLTIP_SHOW_OCR_RESULT);
            toolTip_OCR.SetToolTip(saveOCRCheckBox, Properties.Settings.Default.TOOLTIP_OCRSAVE);
            toolTip_OCR.SetToolTip(isClipBoardcheckBox1, Properties.Settings.Default.TOOLTIP_CLIPBOARD);
            toolTip_OCR.SetToolTip(checkDic, Properties.Settings.Default.TOOLTIP_DIC);
            toolTip_OCR.SetToolTip(cbPerWordDic, Properties.Settings.Default.TOOLTIP_WORDDIC);

            toolTip_OCR.SetToolTip(checkRGB, Properties.Settings.Default.TOOLTIP_RGB);
            toolTip_OCR.SetToolTip(checkHSV, Properties.Settings.Default.TOOLTIP_HSV);

            toolTip_OCR.SetToolTip(cbDBMultiGet, Properties.Settings.Default.TOOLTIP_MULTI_DB);
            //OCR 영역 다시 초기화 함.

            FormManager.Instace.RefreshOCRAreaForm();

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            GetIsHasError();
        }

        public bool GetIsHasError()
        {
            bool isError = false;

            if (SettingManager.isErrorEmptyGoogleToken)
            {
                Logo.SetTopmost(false);
                if (MessageBox.Show("인증 토큰을 발견하지 못했습니다.\n재발급 받으시겠습니까?\n발급 후 다시 적용하기를 눌러주셔야 합니다!", "구글 번역기", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TransManager.Instace.InitGTransToken();
                }

                isError = true;
            }

            return isError;
        }
        #endregion


        #region::::::::::::::::::::::::::::::::::::::::::키 후킹::::::::::::::::::::::::::::::::::::::::::::::::::

        private void SaveHotKeyFile()
        {
            string result = "";
            foreach(var obj in inputKeyUIList)
            {
                string key = obj.GetKeyListToString();
                result += obj.keyType.ToString() + System.Environment.NewLine;
                result += key + System.Environment.NewLine + System.Environment.NewLine; ;              

            }

            Util.SaveFile(GlobalDefine.HOTKEY_FILE, result);

            return;
        }

        private void InitHotKey()
        {
            transKeyInputLabel.keyType = KeyInputLabel.KeyType.Translate;
            dicKeyInputLabel.keyType = KeyInputLabel.KeyType.OpenDic;
            quickKeyInputLabel.keyType = KeyInputLabel.KeyType.QuickOCR;
            snapShotInputLabel.keyType = KeyInputLabel.KeyType.SnapShot;
            lbOneTrans.keyType = KeyInputLabel.KeyType.TranslateOnce;
            lbHideTranslate.keyType = KeyInputLabel.KeyType.Hide;

            inputKeyUIList.Add(transKeyInputLabel);
            inputKeyUIList.Add(dicKeyInputLabel);
            inputKeyUIList.Add(quickKeyInputLabel);
            inputKeyUIList.Add(snapShotInputLabel);
            inputKeyUIList.Add(lbOneTrans);
            inputKeyUIList.Add(lbHideTranslate);


            foreach(var obj in inputKeyUIList)
            {
                obj.SetDefaultKey();
            }


            if(File.Exists(GlobalDefine.HOTKEY_FILE))
            {
                OpenHotKeyFile(inputKeyUIList);
            }
            else if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD) || File.Exists(GlobalDefine.HOTKEY_FILE_OLD_V2))
            {
                OpenOldHotKeyFile();               
            }

            if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD))
            {
                File.Delete(GlobalDefine.HOTKEY_FILE_OLD);
            }

            if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD_V2))
            {
                File.Delete(GlobalDefine.HOTKEY_FILE_OLD_V2);
            }
        }


        private void OpenHotKeyFile(List<KeyInputLabel> keyList)
        {
            try
            {
                var reader = Util.OpenFile(GlobalDefine.HOTKEY_FILE);

                if(reader != null)
                {
                    string data = reader.ReadToEnd();
                    reader.Close();

                    foreach(var obj in keyList)
                    {
                        string key = Util.GetNextLine(data, obj.keyType.ToString());
                        obj.SetKeyList(key);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 구버전 단축키를 불러온다
        /// </summary>
        private void OpenOldHotKeyFile()
        {
            try 
            {
                bool isOldFile = false;
                string filePath = GlobalDefine.HOTKEY_FILE_OLD_V2;
                if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD))
                {
                    filePath = GlobalDefine.HOTKEY_FILE_OLD;
                    isOldFile = true;
                }


                StreamReader r = new StreamReader(filePath);

                string line = r.ReadLine();

                if (line == null || line == "")
                {
                    line = "";
                    InitTansKey();
                }
                else
                {
                    transKeyInputLabel.SetKeyList(line);
                }

                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitDicKey();

                }
                else
                {
                    dicKeyInputLabel.SetKeyList(line);
                }

                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitQuickKey();
                }
                else
                {
                    quickKeyInputLabel.SetKeyList(line);
                }

                //스냅샷.
                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitSnapShotKey();
                }
                else
                {
                    snapShotInputLabel.SetKeyList(line);
                }

                //한 번만 번역하기.
                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitOneTranslateKey();
                }
                else
                {
                    lbOneTrans.SetKeyList(line);
                }

                //번역창 숨기기번역하기.
                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitHideTransKey();
                }
                else
                {
                    lbHideTranslate.SetKeyList(line);
                }

                r.Close();
                r.Dispose();           


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.HOTKEY_FILE_OLD_V2))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }

        }

        List<Keys> inputKeyList = new List<Keys>();

        public void gHook_KeyUp(object sender, KeyEventArgs e)
        {
            inputKeyList.Clear();

        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            if(IsLockHotKey)
            {
                return;
            }


            Keys code = e.KeyCode;

            //테스트용
            if (e.KeyCode == Keys.V)
            {
                //loader.TextToSpeach("test");
                //Util.ShowLog("v");
            }



            //----

            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
            {
                code = Keys.ShiftKey;
            }
            else if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                code = Keys.ControlKey;
            }
            else if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
            {
                code = Keys.Menu;
            }
            //quickKeyInputLabel.SetText(e.KeyCode.ToString() + " " + code + " " + e.SuppressKeyPress.ToString());
            if (transKeyInputLabel.isFocus || quickKeyInputLabel.isFocus || dicKeyInputLabel.isFocus || snapShotInputLabel.isFocus || lbOneTrans.isFocus)
            {
                return;
            }

            bool isHas = false;

            for (int i = 0; i < inputKeyList.Count; i++)
            {
                if (inputKeyList[i] == code)
                {
                    isHas = true;
                }
            }

            if (!isHas)
            {
                inputKeyList.Add(code);
            }
            else
            {
                return;
            }

            //번역 시작.
            if (transKeyInputLabel.GetIsCorrect(inputKeyList))
            {
                if (thread == null)
                {
                    CheckStartRealTimeTrans();
                }
                else if (thread != null && thread.IsAlive == true)
                {
                    StopTrans();
                }
            }
            else if (lbOneTrans.GetIsCorrect(inputKeyList))
            {
                if (thread == null)
                {
                    StartTrnas(true);
                }
                else if (thread != null && thread.IsAlive == true)
                {
                    isEndFlag = true;
                    thread.Join();

                    isEndFlag = false;

                    thread = new Thread(() => ProcessTrans(true));
                    thread.Start();
                }

            }

            else if (quickKeyInputLabel.GetIsCorrect(inputKeyList))
            {
                //빠른 ocr 영역.
                FormManager.Instace.MakeQuickCaptureAreaForm();
            }
            else if (dicKeyInputLabel.GetIsCorrect(inputKeyList))
            {
                //교정사전 열기
                MakeDicEditorForm();
            }
            else if (snapShotInputLabel.GetIsCorrect(inputKeyList))
            {
                //스냅샷 열기
                MakeAndStartSnapShop();
            }
            else if (lbHideTranslate.GetIsCorrect(inputKeyList))
            {
                //스냅샷 열기
                FormManager.Instace.HideTransFrom();
            }
            else
            {
                HotKeyData data = AdvencedOptionManager.GetHotKeyResult(inputKeyList);
                //고급 단축키
                if (data != null)
                {
                    switch(data.keyType)
                    {
                        case KeyInputLabel.KeyType.OpenSetting:

                            Util.ShowLog("Open setting : " + data.extraData);
                            if(data.extraData != "")
                            {
                                string path = GlobalDefine.SETTING_PATH + data.extraData;

                                if(File.Exists(path))
                                {
                                    OpenSettingFile(path);
                                }
                                else
                                {
                                    string error = Properties.Settings.Default.FAIL_HOTKEY_OPEN_SETTING_FILE;
                                    error = string.Format(error, data.extraData);
                                    FormManager.Instace.AddText(error);
                                    Util.ShowLog("None");
                                }
                            }
                            break;

                        case KeyInputLabel.KeyType.LayerTransparency:
                            FormManager.Instace.SetForceTransparency(isProcessTransFlag);

                            break;
                    }
                }
            }
        }

        #endregion


        #region:::::::::::::::::::::::::::::::::::::::::::내부 동작 함수:::::::::::::::::::::::::::::::::::::::::::

        #region:::::::::::::::::::::::::::::::::::::::::::텍스트 설정 :::::::::::::::::::::::::::::::::::::::::::


        private void useBackColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void removeSpaceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void alignmentCenterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void cbShowOCRIndex_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }


        private void ShowResultFont()
        {
            string first = FormManager.CUSTOM_LABEL_TEXT;
            string second = FormManager.CUSTOM_LABEL_TEXT2;


            if (this.removeSpaceCheckBox.Checked)
            {
                fontResultLabel.Text = first.Replace(" ", "");
                fontResultLabel.Text += second.Replace(" ", "");
            }
            else
            {
                fontResultLabel.Text = first;
                fontResultLabel.Text += second;
            }

            if (this.cbShowOCRIndex.Checked)
            {
                fontResultLabel.Text = string.Format(fontResultLabel.Text, "1. ", "2. ");
            }
            else
            {
                fontResultLabel.Text = string.Format(fontResultLabel.Text, "- ", "- ");
            }




            fontResultLabel.TextFont = this.textFont;
            fontResultLabel.TextColor = this.textColorBox.BackColor;
            fontResultLabel.OutlineForeColor = this.outlineColor1Box.BackColor;
            fontResultLabel.OutlineForecolor2 = this.outlineColor2Box.BackColor;
            fontResultLabel.BackColor = this.backgroundColorBox.BackColor;
            fontResultLabel.IsFillBackColor = useBackColorCheckBox.Checked;
            fontResultLabel.IsAlignmentCenter = alignmentCenterCheckBox.Checked;
            fontResultLabel.Refresh();
        }

        private void fontButton_Click(object sender, EventArgs e)
        {

            this.fontDialog.Font = textFont;
            try
            {
                DialogResult dr = this.fontDialog.ShowDialog();
                //확인버튼 누르면 변경
                if (dr == DialogResult.OK)
                {
                    textFont = this.fontDialog.Font;
                    int fontSize = (int)this.fontDialog.Font.Size;

                    if (fontSize > fontSizeUpDown.Maximum)
                        fontSize = (int)fontSizeUpDown.Maximum;
                    else if (fontSize < fontSizeUpDown.Minimum)
                        fontSize = (int)fontSizeUpDown.Minimum;

                    fontButton.Text = this.fontDialog.Font.FontFamily.Name;
                    fontSizeUpDown.Value = fontSize;

                    ShowResultFont();
                }
            }
            catch (System.ArgumentException ex)
            {
                MessageBox.Show("사용할 수 없는 폰트입니다");
            }

        }

        private void fontSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            textFont = new Font(textFont.FontFamily, (int)fontSizeUpDown.Value);
            ShowResultFont();
        }

        private void defaultColorButton_Click(object sender, EventArgs e)
        {
            textColor = new Color();
            outlineColor1 = new Color();
            outlineColor2 = new Color();
            backgroundColor = new Color();

            textColor = Color.FromArgb(255, 255, 255);
            outlineColor1 = Color.FromArgb(192, 192, 192);      //old : 100 / 149 / 237
            outlineColor2 = Color.FromArgb(0, 0, 0);       //old : 65 / 105 / 225
            backgroundColor = Color.FromArgb(145, 0, 0, 0);      // 0,0,0

            SetColorBoxColor(textColorBox, textColor);
            SetColorBoxColor(outlineColor1Box, outlineColor1);
            SetColorBoxColor(outlineColor2Box, outlineColor2);
            SetColorBoxColor(backgroundColorBox, backgroundColor);

            ShowResultFont();
        }

        private void SetColorBoxColor(PictureBox obj, Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;

            if (r == 0)
                r = 1;
            if (g == 0)
                g = 1;
            if (b == 0)
                b = 1;

            Color picturBoxColor = new Color();
            picturBoxColor = Color.FromArgb(r, g, b);

            obj.BackColor = picturBoxColor;

            ShowResultFont();
        }

        private void textColorBox_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = textColor;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                textColor = this.colorDialog1.Color;
                SetColorBoxColor(textColorBox, this.colorDialog1.Color);
            }
        }

        private void outlineColor1Box_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = outlineColor1;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                outlineColor1 = this.colorDialog1.Color;
                SetColorBoxColor(outlineColor1Box, this.colorDialog1.Color);
            }
        }

        private void outlineColor2Box_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = outlineColor2;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                outlineColor2 = this.colorDialog1.Color;
                SetColorBoxColor(outlineColor2Box, this.colorDialog1.Color);
            }
        }

        private void backgroundColorBox_Click(object sender, EventArgs e)
        {


            /*
            this.colorDialog1.Color = backgroundColor;
            DialogResult dr = this.colorDialog1.ShowDialog();


            if (dr == DialogResult.OK)
            {
                backgroundColor = this.colorDialog1.Color;
                SetColorBoxColor(backgroundColorBox, this.colorDialog1.Color);
            }

            */



            Opulos.Core.UI.AlphaColorDialog acd = new Opulos.Core.UI.AlphaColorDialog();

            acd.ColorChanged += delegate {
                System.Diagnostics.Debug.WriteLine("Color changed: " + acd.Color);
            };

            acd.SetColor(backgroundColor);
            DialogResult dr2 = acd.ShowDialog();


            if (dr2 == DialogResult.OK)
            {
                Util.ShowLog("Result = " + acd.Color.A.ToString());
                backgroundColor = acd.Color;
                SetColorBoxColor(backgroundColorBox, acd.Color);
            }
            acd.Dispose();

        }


        #endregion

        //프로그램 닫기
        private void CloseApplication()
        {
            FormManager.Instace.SetTopMostTransform(false);
            if (MessageBox.Show(new Form { TopMost = true }, "종료하시겠습니까?", "종료하시겠습니까?", MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question) == DialogResult.Yes)
            {
                exitApplication();
            }
            FormManager.Instace.SetTopMostTransform(true);
            return;
        }

        //클립보드에 ocr 문장 저장
        private void setClipboard(string transText)
        {
            if (transText != null)
            {
                try
                {
                    isClipeBoardReady = false;
                    string replaceOcrText = transText.Replace(" ", "");
                    replaceOcrText = transText.Replace("not thing", " ");
                    if (replaceOcrText.CompareTo("") != 0)
                    {
                        Clipboard.SetText(replaceOcrText);               //인시로 둠
                    }

                    isClipeBoardReady = true;



                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    isClipeBoardReady = true;
                    return;
                }
            }

            isClipeBoardReady = true;


        }

        private void setColorValueText(ColorGroup nowColorGroup)
        {
            rTextBox.Text = nowColorGroup.getValueR().ToString();
            gTextBox.Text = nowColorGroup.getValueG().ToString();
            bTextBox.Text = nowColorGroup.getValueB().ToString();

            v1TextBox.Text = nowColorGroup.getValueV1().ToString();
            v2TextBox.Text = nowColorGroup.getValueV2().ToString();
            s1TextBox.Text = nowColorGroup.getValueS1().ToString();
            s2TextBox.Text = nowColorGroup.getValueS2().ToString();
        }

        #region :::::::::: 번역 계정키 관련 ::::::::::



        private void SaveNaverKeyFile()
        {
            bool isPaid = false;
            var data = TransManager.Instace.GetNaverKey();

            isPaid = data.isPaid;

            TransManager.Instace.SaveNaverKeyFile(NaverIDKeyTextBox.Text, NaverSecretKeyTextBox.Text, isPaid);

        }

        private void OpenNaverKeyFile()
        {
            try
            {
                TransManager.Instace.OpenNaverKeyFile();
                TransManager.Instace.SortNaverKeyList();

                TransManager.NaverKeyData data = TransManager.Instace.GetNaverKey();
                naverIDKey = data.id;
                NaverIDKeyTextBox.Text = data.id;
                naverSecretKey = data.secret;
                NaverSecretKeyTextBox.Text = data.secret;


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.NAVER_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }
        }



        private void SaveGoogleKeyFile()
        {
            try
            {
                using (StreamWriter newTask = new StreamWriter(GlobalDefine.GOOGLE_ACCOUNT_FILE, false))
                {
                    newTask.WriteLine(googleSheet_textBox.Text);
                    newTask.WriteLine(textBox_GoogleClientID.Text);
                    newTask.WriteLine(textBox_GoogleSecretKey.Text);
                    newTask.Close();
                }


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.GOOGLE_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(GlobalDefine.GOOGLE_ACCOUNT_FILE, false))
                    {
                        newTask.WriteLine(googleSheet_textBox.Text);
                        newTask.WriteLine(textBox_GoogleClientID.Text);
                        newTask.WriteLine(textBox_GoogleSecretKey.Text);
                        newTask.Close();
                    }
                }
            }

        }

        private void OpenGoogleKeyFile()
        {
            try
            {
                StreamReader r = new StreamReader(GlobalDefine.GOOGLE_ACCOUNT_FILE);
                string line = r.ReadLine();
                TransManager.Instace.googleKey = line;
                googleSheet_textBox.Text = line;

                line = r.ReadLine();
                textBox_GoogleClientID.Text = line;
                line = r.ReadLine();
                textBox_GoogleSecretKey.Text = line;
                r.Close();
                r.Dispose();

            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.GOOGLE_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }
        }

        #endregion

        bool isClipeBoardReady = false;

        public void DoTextToSpeach(string text)
        {
            if (isAvailableWinOCR && MySettingManager.IsUseTTS)
            {
                int type = 0;
                if (MySettingManager.IsWaitTTSEnd)
                {
                    type = 1;
                }

                loader.TextToSpeach(text, type);
            }

        }

        /// <summary>
        /// WIN OCR을 위한 이미지 데이터를 가져온다.
        /// </summary>
        /// <param name="ocrAreaCount"></param>
        /// <param name="imgDataList"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        private void GetImgDataForWInOCR(int ocrAreaCount, List<ImgData> imgDataList, ref int positionX, ref int positionY)
        {
            for (int j = 0; j < ocrAreaCount; j++)
            {
                int x = 15;
                int y = 0;
                int channels = 4;
                IntPtr data = IntPtr.Zero;
                data = processGetImgData(j, ref x, ref y, ref channels, ref positionX, ref positionY);

                if (data != IntPtr.Zero)
                {
                    var arr = new byte[x * y * channels];
                    Marshal.Copy(data, arr, 0, x * y * channels);
                    Marshal.FreeHGlobal(data);


                    List<byte> rList = null;
                    List<byte> gList = null;
                    List<byte> bList = null;

                    // Util.ShowLog(channels.ToString());
                    //bgra.
                    /*
                    if (channels == 1)
                    {
                        int size = arr.Length;
                        rList = new List<byte>(size);
                        gList = new List<byte>(size);
                        bList = new List<byte>(size);
                        for (int i = 0; i < arr.Length; i++)
                        {
                            bList.Add(arr[i]);
                            gList.Add(arr[i]);
                            rList.Add(arr[i]);
                        }
                    }
                    else
                    {
                        int size = arr.Length / channels;
                        rList = new List<byte>(size);
                        gList = new List<byte>(size);
                        bList = new List<byte>(size);

                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (i % channels == 0)
                            {
                                bList.Add(arr[i]);
                            }
                            else if (i % channels == 1)
                            {
                                gList.Add(arr[i]);
                            }
                            else if (i % channels == 2)
                            {
                                rList.Add(arr[i]);
                            }
                        }
                    }
                    */
                    ImgData imgData = new ImgData();
                    imgData.channels = channels;
                    imgData.data = arr;

                    //imgData.rList = rList;
                    //imgData.gList = gList;
                    //imgData.bList = bList;
                    imgData.x = x;
                    imgData.y = y;
                    imgData.index = j;
                    imgDataList.Add(imgData);

                }
            }
        }

        /// <summary>
        /// 캡쳐를 이용해 WIN OCR을 위한 이미지 데이터를 가져온다.
        /// </summary>
        /// <param name="ocrAreaCount"></param>
        /// <param name="imgDataList"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        private void GetImgDataFromCaptureForWinOCR(int ocrAreaCount, List<ImgData> imgDataList, ref int positionX, ref int positionY)
        {
            byte[] byteData = default(byte[]);
            int width = 0;
            int height = 0;


            GetImgDataFromCapture(ref byteData, ref width, ref height, ref positionX, ref positionY);

            for (int j = 0; j < ocrAreaCount; j++)
            {
                int x = 15;
                int y = 0;
                int channels = 4;
                IntPtr data = IntPtr.Zero;

                data = processGetImgDataFromByte(j, width, height, positionX, positionY, byteData, ref x, ref y, ref channels);

                if (data != IntPtr.Zero)
                {
                    var arr = new byte[x * y * channels];
                    Marshal.Copy(data, arr, 0, x * y * channels);
                    Marshal.FreeHGlobal(data);




                    // Util.ShowLog(channels.ToString());
                    //bgra.
                    /*
                    List<byte> rList = null;
                    List<byte> gList = null;
                    List<byte> bList = null;
                    if (channels == 1)
                    {
                        int size = arr.Length;
                        rList = new List<byte>(size);
                        gList = new List<byte>(size);
                        bList = new List<byte>(size);
                        for (int i = 0; i < arr.Length; i++)
                        {
                            bList.Add(arr[i]);
                            gList.Add(arr[i]);
                            rList.Add(arr[i]);
                        }
                    }
                    else
                    {
                        int size = arr.Length / channels;
                        rList = new List<byte>(size);
                        gList = new List<byte>(size);
                        bList = new List<byte>(size);

                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (i % channels == 0)
                            {
                                bList.Add(arr[i]);
                            }
                            else if (i % channels == 1)
                            {
                                gList.Add(arr[i]);
                            }
                            else if (i % channels == 2)
                            {
                                rList.Add(arr[i]);
                            }
                        }
                    }
                    */

                    ImgData imgData = new ImgData();
                    imgData.channels = channels;
                    imgData.data = arr;

                    imgData.x = x;
                    imgData.y = y;
                    imgData.index = j;
                    imgDataList.Add(imgData);
                }
            }

        }


        /// <summary>
        /// 캡쳐로 부터 이미지 데이터를 가져온다.
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        private void GetImgDataFromCapture(ref byte[] byteData, ref int width, ref int height, ref int positionX, ref int positionY)
        {

            if (FormManager.Instace.screenCaptureUI != null)
            {
                FormManager.Instace.screenCaptureUI.DoPrepare();
            }

            while (true)
            {
                if (FormManager.Instace.screenCaptureUI != null)
                {
                    FormManager.Instace.screenCaptureUI.DoCapture();
                    bool isSuccess = FormManager.Instace.screenCaptureUI.GetData(ref byteData, ref width, ref height, ref positionX, ref positionY);

                    if (isEndFlag)
                    {
                        return;
                    }

                    if (isSuccess)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(2);
                    }
                }
                else
                {
                    isEndFlag = true;
                    return;
                }
            }
        }

        private string AdjustText(string text)
        {
            string result = text;

            if (MySettingManager.NowIsRemoveSpace == true)
            {
                result = result.Replace(" ", "");
            }

            //교정 사전 사용 여부 체크.
            if (MySettingManager.NowIsUseDicFileFlag)
            {
                StringBuilder sb = new StringBuilder(result, 8192);
                ProcessGetSpellingCheck(sb, MySettingManager.isUseMatchWordDic);
                result = sb.ToString();       //ocr 결과
                sb.Clear();
            }


            //------------------OCR 줄바꿈 없애기 처리---------------------

            //over는 줄바꿈 처리 안 한다.

            bool isRequireReplace = true;

            if(isDebugTransOneLine)
            {
                isRequireReplace = false;
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.db || MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                isRequireReplace = false;
            }

            if(isRequireReplace)
            {
                if (MySettingManager.NowIsRemoveSpace)
                {
                    result = result.Replace("\r\n", "");
                }
                else
                {
                    result = result.Replace("\r\n", " ");
                }
            }

            //---------------------------------------------------------

            return result;
        }

        public void ProcessTrans(bool isSnap = false)              //번역 시작 쓰레드
        {
            //캡쳐할 클라이언트 위치.
            int clientPositionX = 0;
            int clientPositionY = 0;

            //isEndFlag = false;
            string formerOcrString = "";    //바로 이전에 가져온 문장
            isClipeBoardReady = true;
            int lastTick = 0;
            try
            {
                while (isEndFlag == false)
                {
                    int diff = Math.Abs(System.Environment.TickCount - lastTick);

                    //TODO :빠른 속도를 원하면 저 주석 해제하면 됨
                    if (diff >= ocrProcessSpeed/* / 10*/ || isDebugUnlockOCRSpeed)
                    {
                        lastTick = System.Environment.TickCount;


                        if (FormManager.Instace.MyBasicTransForm != null || FormManager.Instace.MyLayerTransForm != null || FormManager.Instace.MyOverTransForm != null)
                        {
                            string argv3 = "";

                            #region :::::::::: 윈도우 OCR 처리 :::::::::::

                            //win ocr 처리.
                            if (MySettingManager.OCRType == SettingManager.OcrType.Window)
                            {
                                if (loader.GetIsAvailableOCR())
                                {
                                    unsafe
                                    {
                                        Util.CheckTimeSpan(true);
                                        int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                        List<ImgData> imgDataList = new List<ImgData>();

                                        if (MySettingManager.isUseAttachedCapture)
                                        {
                                            GetImgDataFromCaptureForWinOCR(ocrAreaCount, imgDataList, ref clientPositionX, ref clientPositionY);
                                        }
                                        else
                                        {
                                            GetImgDataForWInOCR(ocrAreaCount, imgDataList, ref clientPositionX, ref clientPositionY);
                                        }

                                        if (isEndFlag)
                                        {
                                            break;
                                        }


                                        string ocrResult = "";
                                        string transResult = "";
                                        argv3 = "";

                                        OCRDataManager.Instace.ClearData();

                                        for (int j = 0; j < imgDataList.Count; j++)
                                        {
                                            //잠시 막음 - 원래 이게 성장임
                                            loader.SetImg(imgDataList[j].data, imgDataList[j].channels, imgDataList[j].x, imgDataList[j].y);

                                            Util.CheckTimeSpan(false);
                                            imgDataList[j].Clear();
                                            loader.MakeBitMap();
                                            loader.ProcessOcrFunc();

                                            while (!isEndFlag && !loader.GetIsAvailableOCR())
                                            {
                                                //Thread.SpinWait(1);
                                                Thread.Sleep(2);

                                            }

                                            string result = loader.GetText();


                                            IntPtr ptr = loader.GetMar();
                                            WinOCRResultData point = (WinOCRResultData)Marshal.PtrToStructure(ptr, typeof(WinOCRResultData));
                                            OCRDataManager.ResultData winOcrResultData = OCRDataManager.Instace.AddData(point, j);

                                            Marshal.FreeCoTaskMem(ptr);


                                            List<string> ocrList = null;
                                            if (MySettingManager.NowSkin == SettingManager.Skin.over)
                                            {
                                                if (winOcrResultData != null)
                                                {
                                                    ocrList = winOcrResultData.GetOcrText();
                                                    result = "";

                                                    for (int i = 0; i < ocrList.Count; i++)
                                                    {
                                                        ocrList[i] = AdjustText(ocrList[i]);

                                                        result += System.Environment.NewLine + Util.GetSpliteToken(transType) + ocrList[i];
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                result = AdjustText(result);
                                            }

                                            System.Threading.Tasks.Task<string> transTask = null;

                                            transTask = TransManager.Instace.StartTrans(result, MySettingManager.NowTransType, ocrList);
                                            transResult = transTask.Result;

                                            if (winOcrResultData != null)
                                            {
                                                winOcrResultData.InitTransResult(transResult, transType);
                                            }



                                            if (imgDataList.Count > 1)
                                            {
                                                if (MySettingManager.IsShowOCRIndex)
                                                {
                                                    if (!string.IsNullOrEmpty(result))
                                                    {
                                                        if (transResult != "not thing")
                                                        {
                                                            argv3 += (imgDataList[j].index + 1).ToString() + " : " + transResult + System.Environment.NewLine;

                                                        }
                                                    }

                                                    ocrResult += (imgDataList[j].index + 1).ToString() + " : " + result + System.Environment.NewLine;
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(result))
                                                    {
                                                        if (transResult != "not thing")
                                                        {
                                                            argv3 += "- " + transResult;

                                                            if (j + 1 < imgDataList.Count)
                                                            {
                                                                argv3 += System.Environment.NewLine;

                                                            }
                                                        }

                                                        ocrResult += "- " + result;

                                                        if (j + 1 < imgDataList.Count)
                                                        {
                                                            ocrResult += System.Environment.NewLine;
                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                argv3 = transResult;
                                                ocrResult = result;
                                            }
                                        }


                                        nowOcrString = ocrResult;
                                        imgDataList.Clear();
                                        imgDataList = null;
                                    }

                                }
                                else
                                {
                                    //준비되지 않았으면 이전과 같게 처리.
                                    nowOcrString = formerOcrString;
                                }
                            }

                            #endregion
                            else
                            {
                                //Tessreact OCR / NHOcr
                                StringBuilder sb = new StringBuilder(8192);
                                StringBuilder sb2 = new StringBuilder(8192);
                                IntPtr hdc = IntPtr.Zero;

                                if (MySettingManager.isUseAttachedCapture)
                                {
                                    byte[] byteData = default(byte[]);
                                    int width = 0;
                                    int height = 0;

                                    int positionX = 0;
                                    int positionY = 0;

                                    GetImgDataFromCapture(ref byteData, ref width, ref height, ref positionX, ref positionY);

                                    if (isEndFlag)
                                    {
                                        break;
                                    }

                                    processOcrWithData(sb, sb2, width, height, positionX, positionY, byteData);

                                }
                                else
                                {
                                    processOcr(sb, sb2);
                                }


                                nowOcrString = sb.ToString();       //ocr 결과

                                //------------------OCR 줄바꿈 없애기 처리---------------------
                                nowOcrString = nowOcrString.Replace("\r\n", "\n");


                                if (!isDebugTransOneLine)    //디버그 - 한 줄씩 번역이 켜져 있으면 -> 줄바꿈 없애기를 안 한다
                                {
                                    if (MySettingManager.NowIsRemoveSpace)
                                    {
                                        nowOcrString = nowOcrString.Replace("\n", "");
                                    }
                                    else
                                    {
                                        nowOcrString = nowOcrString.Replace("\n", " ");
                                    }
                                }

                                //---------------------------------------
                                nowOcrString = nowOcrString.Replace("\t", System.Environment.NewLine);

                                argv3 = sb2.ToString();      //번역 결과.
                                sb.Clear();
                                sb2.Clear();



                                if (MySettingManager.NowTransType != SettingManager.TransType.db && formerOcrString.CompareTo(nowOcrString) != 0)
                                {
                                    System.Threading.Tasks.Task<string> test = TransManager.Instace.StartTrans(nowOcrString, MySettingManager.NowTransType);
                                    argv3 = test.Result;

                                }
                            }

                            //OCR, 번역 끝 화면에 뿌리기
                            //새로 데이터 갱신해야 함.
                            if (formerOcrString.CompareTo(nowOcrString) != 0 || nowOcrString == "")
                            {
                                formerOcrString = nowOcrString;

                                if (IsUseClipBoardFlag == true && isClipeBoardReady)
                                {
                                    this.BeginInvoke(new myDelegate(setClipboard), new object[] { nowOcrString });

                                }
                                if (MySettingManager.NowSkin == SettingManager.Skin.dark && FormManager.Instace.MyBasicTransForm != null)
                                {
                                    FormManager.Instace.MyBasicTransForm.updateText(argv3, nowOcrString, transType, MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag);
                                }
                                else if (MySettingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyLayerTransForm != null)
                                {
                                    Action action = delegate
                                    {
                                        if (FormManager.Instace.MyLayerTransForm != null)
                                        {
                                            FormManager.Instace.MyLayerTransForm.updateText(argv3, nowOcrString, transType, MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag);

                                        }


                                    };
                                    BeginInvoke(action);
                                }
                                else if (MySettingManager.NowSkin == SettingManager.Skin.over && FormManager.Instace.MyOverTransForm != null)
                                {
                                    Action action = delegate
                                    {
                                        if (FormManager.Instace.MyOverTransForm != null)
                                        {
                                            List<OCRDataManager.ResultData> dataList = OCRDataManager.Instace.GetData();
                                            //argv3, nowOcrString
                                            FormManager.Instace.MyOverTransForm.UpdateText(dataList, MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag, clientPositionX, clientPositionY);

                                        }

                                    };

                                    BeginInvoke(action);


                                }

                                if (MySettingManager.NowSkin == SettingManager.Skin.over)
                                {
                                    string transResult = argv3.Replace(Util.GetSpliteToken(transType), "");
                                    DoTextToSpeach(transResult);
                                }
                                else
                                {
                                    DoTextToSpeach(argv3);
                                }




                                if (isSnap)
                                {
                                    Action callback = delegate
                                    {
                                        StopTrans(true);
                                    };
                                    isEndFlag = true;
                                    BeginInvoke(callback);
                                }
                            }
                            else
                            {
                                //이전과 같아서 그래픽만 갱신함.
                                if (MySettingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyLayerTransForm != null)
                                {
                                    FormManager.Instace.MyLayerTransForm.UpdatePaint();
                                    //BeginInvoke(new Action(FormManager.Instace.MyLayerTransForm.UpdatePaint));
                                }

                                if (MySettingManager.NowSkin == SettingManager.Skin.over && FormManager.Instace.MyOverTransForm != null)
                                {
                                    FormManager.Instace.MyOverTransForm.UpdatePaint();
                                    //BeginInvoke(new Action(FormManager.Instace.MyOverTransForm.UpdatePaint));
                                }

                                if (isSnap)
                                {
                                    Action callback = delegate
                                    {

                                        StopTrans(true);
                                    };
                                    isEndFlag = true;
                                    BeginInvoke(callback);
                                }
                            }
                        }
                    }
                }

                TransManager.Instace.SaveFormerResultFile(MySettingManager.NowTransType);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::외부 조작 함수:::::::::::::::::::::::::::::::::::::::::::

        public void SetUseColorGroup()
        {
            ClearOcrColorSet();
            for (int i = 0; i < MySettingManager.NowOCRGroupcount; i++)
            {
                AddOcrColorSet(MySettingManager.UseColorGroup[i].ToArray(), MySettingManager.UseColorGroup[i].Count);
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                AddOcrColorSet(MySettingManager.QuickOcrUsecolorGroup.ToArray(), MySettingManager.QuickOcrUsecolorGroup.Count);
            }

            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                AddOcrColorSet(MySettingManager.QuickOcrUsecolorGroup.ToArray(), MySettingManager.QuickOcrUsecolorGroup.Count);
            }
        }

        public void setObserverHwnd(IntPtr newHwnd)
        {
            //observerHwnd = newHwnd;

        }

        public void SetIsRemoveSpace(bool isRemoveSpace)
        {
            removeSpaceCheckBox.Checked = isRemoveSpace;
            MySettingManager.NowIsRemoveSpace = isRemoveSpace;
        }

        public void SetTextSort(SettingManager.SortType sortType)
        {
            if (sortType == SettingManager.SortType.Normal)
                alignmentCenterCheckBox.Checked = false;
            else
                alignmentCenterCheckBox.Checked = true;

            MySettingManager.NowSortType = sortType;

        }

        //스냅샷 위치 -> 바로 번역.
        public void MakeAndStartSnapShop()
        {
            if (!MySettingManager.isUseAttachedCapture && MySettingManager.NowIsActiveWindow)
            {
                MessageBox.Show("현재 스냅샷을 사용할 수 없습니다" + System.Environment.NewLine + "부가설정 > 이미지 캡쳐 > 활성화 된 윈도우에서 추출하기를 꺼주세요");
                return;
            }

            Action callback = delegate
            {
                Action callback2 = delegate
                {

                    this.BeginInvoke(new myDelegate(updateText), new object[] { "번역 시작" });

                    SetCaptureArea();

                    if (thread != null && thread.IsAlive == true)
                    {
                        isEndFlag = true;
                        thread.Join();

                        isEndFlag = false;

                        thread = new Thread(() => ProcessTrans(true));
                        thread.Start();
                        MakeTransForm();
                    }
                    else
                    {
                        //setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.NowDicFile);
                        StartTrnas(true);
                    }
                };


                BeginInvoke(callback2);

            };

            FormManager.Instace.MakeSnapShotAreaForm(callback);
        }

        //델리게이트 이용
        public void setSpellCheck()
        {
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.isUseMatchWordDic, MySettingManager.NowDicFile);
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.isUseMatchWordDic, MySettingManager.NowDicFile);
            }
        }


        public void exitApplication()
        {
            StopTrans();
            if (thread != null)  //만약 쓰레드가 생성 되었다면
            {
                //thread.Suspend();
                thread.Abort();
                thread.Join();
            }

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Logo")
                {
                    Logo foundedForm = (Logo)frm;
                    foundedForm.closeApplication();
                    break;
                }
            }


            this.Dispose();
            Application.Exit();

        }

        public void CheckStartRealTimeTrans()
        {
            //스냅샷을 했을경우 ocr영역이 바뀌기 때문에 다시 설정해 줘야함.
            if (isBeforeSnapShot)
            {
                SetCaptureArea();
            }

            if (FormManager.Instace.GetOcrAreaCount() == 0)
            {
                if (MessageBox.Show("OCR 영역이 없기 때문에 번역할 수 없습니다." + Environment.NewLine + "사용법을 확인해 보시겠습니까?", "OCR영역이 없습니다", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    try
                    {

                        System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221904784013");
                    }
                    catch { }
                }
                return;
            }

            if (MySettingManager.NowTransType == SettingManager.TransType.google_url)
            {
                Action checkCallback = delegate
                {
                    if (MessageBox.Show("기본 번역기 상태에서 일반 번역은 권장하지 않습니다." + Environment.NewLine + "계속 하시겠습니까?", "경고 - 스냅샷을 이용해 주세요", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        StartTrnas();
                    }
                };

                this.BeginInvoke(checkCallback);

            }
            else
            {
                StartTrnas();
            }

        }

        public void StartTrnas(bool isOnlyOne = false)
        {
            if (MySettingManager.OCRType == SettingManager.OcrType.Window && !isAvailableWinOCR)
            {
                MessageBox.Show("윈도우 10 OCR을 사용할 수 없는 상태입니다.\n에러명 :" + winOcrErrorCode);
                return;
            }

            //오버레이 번역창 가능여부 체크.
            if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                bool isError = false;
                string errorMsg = "";

                if (MySettingManager.OCRType != SettingManager.OcrType.Window)
                {
                    isError = true;
                    errorMsg = "오버레이 번역창은 윈도우 10 OCR에서만 사용할 수 있습니다.";
                }
                else if (!MySettingManager.isUseAttachedCapture && !MySettingManager.NowIsActiveWindow)
                {
                    isError = true;
                    errorMsg = "화면을 가져올 윈도우 지정을 해야 합니다." + System.Environment.NewLine + System.Environment.NewLine +
                        "부가설정 -> 이미지 캡쳐 -> 활썽화된 윈도우에서 이미지 캡쳐 또는 화면을 가져올 윈도우 지정하기를 사용해 주세요";
                }


                if (isError)
                {
                    errorMsg += System.Environment.NewLine + "오버레이 번역창 사용법을 보시겠습니까?";


                    if (MessageBox.Show(errorMsg, "오버레이 번역창 오류", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/222233614879");
                    }

                    return;
                }
            }

            if (FormManager.Instace.MySearchOptionForm != null)
            {
                FormManager.Instace.MySearchOptionForm.acceptCaptureArea();
            }

            isProcessTransFlag = true;
            FormManager.Instace.MyRemoteController.ToggleStartButton(true);

            if (thread == null)
            {
                isEndFlag = false;
                thread = new Thread(() => ProcessTrans(isOnlyOne));
                thread.Start();
            }

            if (isOnlyOne && !FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency)
            {
                isProcessTransFlag = false;
            }

            MakeTransForm();
        }

        public void StopTrans(bool isOnceTrans = false)
        {
            isProcessTransFlag = false;

            FormManager.Instace.MyRemoteController.ToggleStartButton(false);
            if (thread != null)
            {
                isEndFlag = true;
                thread.Join();
                thread = null;
                isEndFlag = false;
            }

            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.StopTrans();
                }
            }

            else
            {
                //한번만 번역 & 강제 투명화 -> 번역이 끝나도 투명상태 유지.
                if (isOnceTrans)
                {
                    if (!FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency)
                    {
                        FormManager.Instace.SetVisibleTrans();
                    }
                }
                else
                {
                    FormManager.Instace.SetVisibleTrans();
                }
            }

            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                FormManager.Instace.snapOcrAreaForm.Close();
            }
        }

        //ocr 영역 적용

        public void SetCaptureArea()
        {
            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;

            FormManager.BorderWidth = Util.GetBorderWidth();
            FormManager.BorderHeight = +SystemInformation.FrameBorderSize.Height;
            FormManager.TitlebarHeight = Util.GetTitlebarHeight();
            locationXList = new List<int>();
            locationYList = new List<int>();
            sizeXList = new List<int>();
            sizeYList = new List<int>();

            exceptionLocationXList.Clear();
            exceptionLocationYList.Clear();
            exceptionSizeXList.Clear();
            exceptionSizeYList.Clear();

            List<int> tempXList = new List<int>();
            List<int> tempYList = new List<int>();
            List<int> tempSizeXList = new List<int>();
            List<int> tempSizeYList = new List<int>();

            //2019 01 01
            //스냅샷이 있으면 모든걸 없애버린다.
            bool isSnapShot = false;
            isBeforeSnapShot = true;

            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                isSnapShot = true;
                isBeforeSnapShot = true;
            }


            if (isSnapShot)
            {
                //퀵 사이즈 전용.
                int quickX = 0;
                int quickY = 0;
                int quickSizeX = 0;
                int quickSizeY = 0;

                quickX = FormManager.Instace.snapOcrAreaForm.Location.X + BorderWidth;
                quickY = FormManager.Instace.snapOcrAreaForm.Location.Y + TitlebarHeight;
                quickSizeX = FormManager.Instace.snapOcrAreaForm.Size.Width - BorderWidth * 2;
                quickSizeY = FormManager.Instace.snapOcrAreaForm.Size.Height - TitlebarHeight - BorderWidth;

                tempXList.Add(quickX);
                tempYList.Add(quickY);
                tempSizeXList.Add(quickSizeX);
                tempSizeYList.Add(quickSizeY);
            }


            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = foundedForm.Size.Width - BorderWidth * 2;
                int sizeY = foundedForm.Size.Height - TitlebarHeight - BorderWidth;
                Util.ShowLog("!!!!! " + locationY + " size y : " + sizeY);
                locationXList.Add(locationX);
                locationYList.Add(locationY);
                sizeXList.Add(sizeX);
                sizeYList.Add(sizeY);

                if (!isSnapShot)
                {
                    tempXList.Add(locationX);
                    tempYList.Add(locationY);
                    tempSizeXList.Add(sizeX);
                    tempSizeYList.Add(sizeY);

                }
            }


            //OCR 설정 저장용
            MySettingManager.NowOCRGroupcount = locationYList.Count;
            MySettingManager.NowLocationXList = locationXList;
            MySettingManager.NowLocationYList = locationYList;
            MySettingManager.NowSizeXList = sizeXList;
            MySettingManager.NowSizeYList = sizeYList;


            //제외 영역 설정
            for (int i = 0; i < FormManager.Instace.exceptionAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.exceptionAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = foundedForm.Size.Width - BorderWidth * 2;
                int sizeY = foundedForm.Size.Height - TitlebarHeight - BorderWidth;
                Util.ShowLog("Exception  " + locationY + " size y : " + sizeY);

                exceptionLocationXList.Add(locationX);
                exceptionLocationYList.Add(locationY);
                exceptionSizeXList.Add(sizeX);
                exceptionSizeYList.Add(sizeY);

            }


            if (FormManager.Instace.quickOcrAreaForm != null && !isSnapShot)
            {
                //퀵 사이즈 전용.
                int quickX = 0;
                int quickY = 0;
                int quickSizeX = 0;
                int quickSizeY = 0;

                OcrAreaForm quickForm = FormManager.Instace.quickOcrAreaForm;

                quickX = quickForm.Location.X + BorderWidth;
                quickY = quickForm.Location.Y + TitlebarHeight;
                quickSizeX = quickForm.Size.Width - BorderWidth * 2;
                quickSizeY = quickForm.Size.Height - TitlebarHeight - BorderWidth;

                tempXList.Add(quickX);
                tempYList.Add(quickY);
                tempSizeXList.Add(quickSizeX);
                tempSizeYList.Add(quickSizeY);

            }

            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                setCutPoint(tempXList.ToArray(), tempYList.ToArray(), tempSizeXList.ToArray(), tempSizeYList.ToArray(), tempXList.Count);
                SetExceptPoint(exceptionLocationXList.ToArray(), exceptionLocationYList.ToArray(), exceptionSizeXList.ToArray(), exceptionSizeYList.ToArray(), exceptionLocationXList.Count);
                SetUseColorGroup();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                setCutPoint(tempXList.ToArray(), tempYList.ToArray(), tempSizeXList.ToArray(), tempSizeYList.ToArray(), tempXList.Count);
                SetExceptPoint(exceptionLocationXList.ToArray(), exceptionLocationYList.ToArray(), exceptionSizeXList.ToArray(), exceptionSizeYList.ToArray(), exceptionLocationXList.Count);
                SetUseColorGroup();
            }

        }


        public void MakeCaptureArea()            //영역 검색 버튼 클릭
        {
            int searchAreaQuantity = 0;

            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];
                searchAreaQuantity++;
                foundedForm.SetVisible(true);
            }

            for (int i = 0; i < FormManager.Instace.exceptionAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.exceptionAreaFormList[i];
                foundedForm.SetVisible(true);
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.SetVisible(true);
            }

            MakeSearchOptionForm();
            if (searchAreaQuantity < 1)
            {
                FormManager.Instace.MakeCpatureAreaForm();
            }

        }

        #endregion


        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if(!Program.IS_FORCE_QUITE)
            {
                CloseApplication();
                e.Cancel = true;//종료를 취소하고 
            }
        
        }

        #region:::::::::::::::::::::::::::::::::::::::::::체크박스 및 라디오 클릭:::::::::::::::::::::::::::::::::::::::::::

        private bool isLockImgCheckBox = false;
        private void SetImgCheckBox(bool isUseHsv, bool isUseRgb, bool isUseThreshold)
        {

            if(!isLockImgCheckBox && eCurrentState == eCurrentStateType.None)
            {
                isLockImgCheckBox = true;

                checkHSV.Checked = false;
                checkRGB.Checked = false;
                cbThreshold.Checked = false;

                MySettingManager.NowIsUseHSVFlag = false;
                MySettingManager.NowIsUseRGBFlag = false;
                MySettingManager.isUseThreshold = false;

                if (isUseHsv)
                {
                    checkHSV.Checked = true;
                    MySettingManager.NowIsUseHSVFlag = true;
                }
                else if(isUseRgb)
                {
                    checkRGB.Checked = true;
                    MySettingManager.NowIsUseRGBFlag = true;
                }
                else if(isUseThreshold)
                {
                    cbThreshold.Checked = true;
                    MySettingManager.isUseThreshold = true;
                }

                isLockImgCheckBox = false;
            }
        }

        private void cbThreshold_CheckedChanged(object sender, EventArgs e)
        {
            SetImgCheckBox(false, false, cbThreshold.Checked);
        }

    

        private void checkRGB_MouseDown(object sender, MouseEventArgs e)
        {
            SetImgCheckBox(false, checkRGB.Checked, false);
        }
        private void checkHSV_MouseDown(object sender, MouseEventArgs e)
        {
            SetImgCheckBox(checkHSV.Checked, false, false);
        }

        private void tesseractLanguageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // 0 = 영어
            // 1 = 일본어
            // 2 = 기타
            ChangeTesseractLanguage(tesseractLanguageComboBox.SelectedIndex);
        }

        private void ChangeTesseractLanguage(int index)
        {
            if (index == (int)GlobalDefine.TesseractLanguageType.English)
            {
                tessDataTextBox.Text = "eng";
                naverTransComboBox.SelectedIndex = 0;
                googleTransComboBox.SelectedIndex = 0;
                removeSpaceCheckBox.Checked = false;
                cbPerWordDic.Checked = true;
            }
            else if (index == (int)GlobalDefine.TesseractLanguageType.Japen)
            {
                tessDataTextBox.Text = "jpn";
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
                removeSpaceCheckBox.Checked = true;
                cbPerWordDic.Checked = false;
            }

            if(index != tesseractLanguageComboBox.SelectedIndex)
            {
                tesseractLanguageComboBox.SelectedIndex = index;
            }
        }

        private void groupCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupCombo.SelectedIndex == 0)  //아이템 추가
            {
                colorGroup.Add(new ColorGroup());

                groupCombo.Items.Add(groupCombo.Items.Count - 1);       //카운트는 1부터 시작
                nowColorGroupIndex = groupCombo.Items.Count - 3;        //나우 인덱스는 0부터 시작 (실질적인 숫자는 2부터 시작) -> 번호 2 = 카운트 4 / 나우는 1 이어야 함
                groupCombo.SelectedIndex = groupCombo.Items.Count - 1;  //현재 선택 -> 가장 위

                for (int i = 0; i < MySettingManager.UseColorGroup.Count; i++)
                {
                    MySettingManager.UseColorGroup[i].Add(1);
                }

                MySettingManager.QuickOcrUsecolorGroup.Add(1);

            }
            else if (groupCombo.SelectedIndex == 1) //아이템 삭제
            {
                if (groupCombo.Items.Count > 3)
                {
                    int removePoint = 0;
                    colorGroup.RemoveAt(nowColorGroupIndex);
                    groupCombo.Items.RemoveAt(nowColorGroupIndex + 2);      //나우 + 2 = 실질적인 콤보박스 번호
                    if (nowColorGroupIndex == 0)
                    {
                        groupCombo.SelectedIndex = 2;
                        removePoint = 2;
                    }
                    else
                    {
                        groupCombo.SelectedIndex = nowColorGroupIndex + 1;      //나우 + 1 = 지우기 전 이전
                        removePoint = 3;
                    }


                    for (int i = nowColorGroupIndex + removePoint; i < groupCombo.Items.Count; i++)
                    {
                        int newText = Convert.ToInt32(groupCombo.Items[i].ToString());
                        newText--;
                        groupCombo.Items[i] = newText.ToString();
                    }

                    for (int i = 0; i < MySettingManager.UseColorGroup.Count; i++)
                    {
                        MySettingManager.UseColorGroup[i].RemoveAt(nowColorGroupIndex);
                    }
                    MySettingManager.QuickOcrUsecolorGroup.RemoveAt(nowColorGroupIndex);
                }
                else
                {
                    groupCombo.SelectedIndex = nowColorGroupIndex + 2;
                }

            }
            else
            {
                nowColorGroupIndex = groupCombo.SelectedIndex - 2;      //나우 + 2 = 그룹의 숫자 인덱스
                colorGroup[nowColorGroupIndex].checkHSVRange();
                setColorValueText(colorGroup[nowColorGroupIndex]);
            }

            groupLabel.Text = (groupCombo.Items.Count - 2).ToString();
        }
        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::키값 입력:::::::::::::::::::::::::::::::::::::::::::

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }


        private void rgbTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 255)
                {
                    value = 255;
                }
                thisTextBox.Text = value.ToString();

            }

            colorGroup[nowColorGroupIndex].setRGBValuse(Convert.ToInt32(rTextBox.Text), Convert.ToInt32(gTextBox.Text), Convert.ToInt32(bTextBox.Text));

        }

        private void hsvTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 100)
                {
                    value = 100;
                }
                thisTextBox.Text = value.ToString();
            }
            colorGroup[nowColorGroupIndex].setHSVValuse(Convert.ToInt32(s1TextBox.Text), Convert.ToInt32(s2TextBox.Text), Convert.ToInt32(v1TextBox.Text), Convert.ToInt32(v2TextBox.Text));
        }

        private void thresholdTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 255)
                {
                    value = 255;
                }
                thisTextBox.Text = value.ToString();
            }
            
        }

        #endregion


        private void acceptButton_Click(object sender, EventArgs e)
        {
            inputKeyList.Clear();

            eCurrentState = eCurrentStateType.Accept;
            acceptButton.Focus();
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;
                ApplyUIValueToSetting();
                //MakeTransForm();

                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                ApplyUIValueToSetting();
            }

            TransForm foundedForm = null;
            TransFormLayer foundedLayerForm = null;
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.TopMost = false;
                }
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                if (FormManager.Instace.MyLayerTransForm != null)
                {
                    FormManager.Instace.MyLayerTransForm.TopMost = false;
                }
            }
            //설정 저장
            SaveSetting(GlobalDefine.USER_SETTING_FILE);

            bool isError = GetIsHasError();

            if (!isError)
            {
                FormManager.ShowPopupMessage("MORT", "적용 완료");
            }



            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.TopMost = isTranslateFormTopMostFlag;
                }
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                if (FormManager.Instace.MyLayerTransForm != null)
                {
                    FormManager.Instace.MyLayerTransForm.TopMost = isTranslateFormTopMostFlag;
                }
            }

            eCurrentState = eCurrentStateType.None;
        }

        


        #region:::::::::::::::::::::::::::::::::::::::::::트레이 아이콘 함수:::::::::::::::::::::::::::::::::::::::::::
        //트레이 아이콘을 더블클릭 했을시 호출
        void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true; // 폼의 표시
            MakeTransForm();
            makeRTT();
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal; // 최소화를 멈춘다 
            this.Activate(); // 폼을 활성화 시킨다
        }

        void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransForm foundedForm = null;
            TransFormLayer foundedLayerForm = null;
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "TransForm")
                    {
                        foundedForm = (TransForm)frm;
                        foundedForm.TopMost = false;
                        break;
                    }
                }
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "TransFormLayer")
                    {
                        foundedLayerForm = (TransFormLayer)frm;
                        foundedLayerForm.TopMost = false;
                        break;
                    }
                }
            }


            if (MessageBox.Show("종료하시겠습니까?", "종료하시겠습니까?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {

                exitApplication();
            }

            if (foundedForm != null && foundedLayerForm == null)
            {
                foundedForm.TopMost = isTranslateFormTopMostFlag;
            }
            else if (foundedForm == null && foundedLayerForm != null)
            {
                foundedLayerForm.TopMost = isTranslateFormTopMostFlag;
            }
        }

        private void ContextTranslate_Click(object sender, EventArgs e)
        {
            if (thread == null)
            {
                CheckStartRealTimeTrans();
            }
            else if (thread != null && thread.IsAlive == true)
            {
                StopTrans();
            }
        }

        private void ContextOption_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form1")
                {
                    frm.Activate();
                    this.Show();
                    return;
                }
            }
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(20, 20);
            this.Show();
        }


        private void setTranslateTopMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isTranslateFormTopMostFlag = !isTranslateFormTopMostFlag;
            setTranslateTopMostToolStripMenuItem.Checked = !setTranslateTopMostToolStripMenuItem.Checked;
            topMostcheckBox.Checked = isTranslateFormTopMostFlag;

            FormManager.Instace.SetSubMenuTopMost(isTranslateFormTopMostFlag);
        }


        private void setCutPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeCaptureArea();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && isProgramStart == true)
            {
                ContextOption.Show();
                if (thread == null)
                {
                    this.BeginInvoke(new myDelegate(updateText), new object[] { "번역 시작" });

                }
                else if (thread != null)
                {
                    this.BeginInvoke(new myDelegate(updateText), new object[] { "번역 중지" });
                }
            }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "About")
                {

                    frm.Activate();
                    frm.Show();
                    return;
                }
            }

            About aboutForm = new About();
            aboutForm.Show();
        }

        private void showTransToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeTransForm();
        }


        private void setCheckSpellingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySettingManager.NowDicFile = dicFileTextBox.Text;
            MySettingManager.NowIsUseDicFileFlag = setCheckSpellingToolStripMenuItem.Checked;
            checkDic.Checked = MySettingManager.NowIsUseDicFileFlag;
            setSpellCheck();


        }

        private void rTTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            makeRTT();
        }


        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70179867557");
            }
            catch { }

        }
        #endregion

        private delegate void myDelegate(string transText);
        private void updateText(string transText)
        {
            transToolStripMenuItem.Text = transText;
        }


        #region:::::::::::::::::::::::::::::::::::::::::::폼 이동 관련 함수:::::::::::::::::::::::::::::::::::::::::::
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
        private void fromUpImg_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void fromUpImg_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        #endregion


        private void pictureBox1_Click(object sender, EventArgs e)      //닫기 버튼
        {
            CloseApplication();
        }

        private void panealBorder_Paint(object sender, PaintEventArgs e)        //패널에 경계선 칠하기 함수
        {
            Panel myPanel = (Panel)sender;

            Pen myPen = new Pen(Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70))))), 1);
            e.Graphics.DrawRectangle(myPen,
            myPanel.ClientRectangle.Left,
            myPanel.ClientRectangle.Top,
            myPanel.ClientRectangle.Width - 1,
            myPanel.ClientRectangle.Height - 1);
            base.OnPaint(e);
        }

        /// <summary>
        /// 툴팁 / 버튼 둘 다 / 설정 저장하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingSaveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            eCurrentState = eCurrentStateType.SaveFile;
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                ApplyUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                ApplyUIValueToSetting();
            }
            SaveFileDialog savePanel = new SaveFileDialog();
            savePanel.RestoreDirectory = false;
            savePanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            savePanel.Filter = "Config File (*.conf)|*.conf";
            if (savePanel.ShowDialog() == DialogResult.OK)
            {
                SaveSetting(savePanel.FileName);
            }

            eCurrentState = eCurrentStateType.None;
        }

        private void settingLoadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.RestoreDirectory = false;
            openPanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            openPanel.Filter = "Config File (*.conf)|*.conf";


            string file = "";
            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                file = openPanel.FileName;

                if (file != "")
                {
                    OpenSettingFile(file);
                }
            }


        }

        private void settingDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eCurrentState = eCurrentStateType.SetDefault;
            isTranslateFormTopMostFlag = true;


            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.Close();
                FormManager.Instace.quickOcrAreaForm = null;
            }

            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                MySettingManager.SetDefault();
                SetValueToUIValue();
                ApplyUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                MySettingManager.SetDefault();
                SetValueToUIValue();
                ApplyUIValueToSetting();
            }

            if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                if (FormManager.Instace.MyLayerTransForm != null)
                {
                    FormManager.Instace.MyLayerTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                }
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                }
            }
            SaveSetting(GlobalDefine.USER_SETTING_FILE);
            eCurrentState = eCurrentStateType.None;
        }



        private void checkDic_CheckedChanged(object sender, EventArgs e)
        {
            setCheckSpellingToolStripMenuItem.Checked = checkDic.Checked;
        }



        private void defaultButton_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("설정을 초기화 하시겠습니까?", "설정 초기화", MessageBoxButtons.OKCancel))
            {
                settingDefaultToolStripMenuItem_Click(sender, e);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                // MessageBox.Show(".");
            }
        }

        private void SetDefaultZoomSizeButton_Click(object sender, EventArgs e)
        {
            MySettingManager.ImgZoomSize = 2;
            imgZoomsizeUpDown.Value = (decimal)2;
        }

        private void help_Button_Click(object sender, EventArgs e)
        {
            try
            {

                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221904784013");
            }
            catch { }
        }

        private void error_Information_Button_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70185869419");
            }
            catch { }
        }

        private void about_Button_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "About")
                {

                    frm.Activate();
                    frm.Show();
                    return;
                }
            }

            About aboutForm = new About();
            aboutForm.Show();
        }

        //마지막으로 선택한 OCR 타입
        private SettingManager.OcrType beforeOcrPanelType = SettingManager.OcrType.Tesseract;
        //OCR 방식 변경
        private void OCR_Type_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tesseract_panel.Visible = false;
            WinOCR_panel.Visible = false;
            pnNHocr.Visible = false;
            //string selectItem = OCR_Type_comboBox.SelectedItem.ToString();
            SettingManager.OcrType ocrType = SettingManager.GetOcrType(OCR_Type_comboBox.SelectedIndex);
            if (ocrType == SettingManager.OcrType.Tesseract)
            {
                Tesseract_panel.Visible = true;
            }
            else if (ocrType == SettingManager.OcrType.Window)
            {
                WinOCR_panel.Visible = true;

                if (isProgramStart && isAvailableWinOCR && !isShowWinOCRWarning && winLanguageCodeList.Count == 1)
                {
                    if (winLanguageCodeList[0] == "ko")
                    {
                        if (DialogResult.OK == MessageBox.Show("한국어 윈도우 OCR 언어팩만 존재합니다\n정상적인 OCR 추출을 위해선 추가 다운로드가 필요합니다\n\n다운로드 방법을 알아보시겠습니까? ", ".", MessageBoxButtons.OKCancel))
                        {
                            try
                            {
                                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/220865537274");
                            }
                            catch { }
                        }

                        isShowWinOCRWarning = true;
                    }
                }
            }
            else if (ocrType == SettingManager.OcrType.NHocr)
            {
                pnNHocr.Visible = true;
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
                removeSpaceCheckBox.Checked = true;
                cbPerWordDic.Checked = false;
            }

            //유저가 변경한 상태다.
            if(eCurrentState == eCurrentStateType.None)
            {
                bool isRequireChange = false;
                int languageType = 0;   //0 = 영어, 1 = 일본어 , //2 = 기타

                if(beforeOcrPanelType == SettingManager.OcrType.Tesseract)
                {
                    switch(tesseractLanguageComboBox.SelectedIndex)
                    {
                        case (int)GlobalDefine.TesseractLanguageType.English:
                            isRequireChange = true;
                            languageType = 0;
                            break;

                        case (int)GlobalDefine.TesseractLanguageType.Japen:
                            isRequireChange = true;
                            languageType = 1;
                            break;
                    }
                }
                else if(beforeOcrPanelType == SettingManager.OcrType.NHocr)
                {
                    isRequireChange = true;
                    languageType = 1;
                }
                else if(beforeOcrPanelType == SettingManager.OcrType.Window)
                {
                    string selectCode = winLanguageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    if (selectCode == "en" || selectCode == "en-US")
                    {
                        isRequireChange = true;
                        languageType = 0;
                    }
                    else if (selectCode == "ja")
                    {
                        isRequireChange = true;
                        languageType = 1;
                    }
                }

                if(isRequireChange)
                {
                    if(ocrType == SettingManager.OcrType.Tesseract)
                    {
                        if (languageType == 0)
                        {
                            ChangeTesseractLanguage((int)GlobalDefine.TesseractLanguageType.English);
                        }
                        else if (languageType == 1)
                        {
                            ChangeTesseractLanguage((int)GlobalDefine.TesseractLanguageType.Japen);
                        }

                    }
                    else if(ocrType == SettingManager.OcrType.Window)
                    {
                        
                        if (isAvailableWinOCR)
                        {
                            string code = "";
                            
                            if (languageType == 0)
                            {
                                code = "en";
                            }
                            else if (languageType == 1)
                            {
                                code = "ja";
                            }

                            //OCR을 찾았나 못 찾았나.
                            //영어는 en 또는 en-us일 수 있다
                            bool isFound = false;
                            for (int i = 0; i < winLanguageCodeList.Count; i++)
                            {
                                if (Util.GetIsEqualWinCode(winLanguageCodeList[i], code))
                                {
                                    if (WinOCR_Language_comboBox.Items.Count > i)
                                    {
                                        isFound = true;
                                        ChangeWinOcrLanguage(i);

                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            beforeOcrPanelType = ocrType;
        }

        //번역 방식 변경.
        private void TransType_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB_Panel.Visible = false;
            Yandex_Panel.Visible = false;
            Naver_Panel.Visible = false;
            Google_Panel.Visible = false;
            pnGoogleBasic.Visible = false;
            pnEzTrans.Visible = false;

            if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.db)
            {
                DB_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.naver)
            {
                Naver_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.google)
            {
                Google_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.google_url)
            {
                pnGoogleBasic.Visible = true;
            }
            else if(TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.ezTrans)
            {
                pnEzTrans.Visible = true;
            }
        }


        //단축키 - 번역 초기값.
        private void SetEmptyTansKey()
        {
            transKeyInputLabel.SetEmpty();
        }

        //단축키 - 교정 사전 초기값.
        private void SetEmptyDicKey()
        {
            this.dicKeyInputLabel.SetEmpty();
        }

        //단축키 - 빠른 영역 초기값.
        private void SetEmptyQuickKey()
        {
            this.quickKeyInputLabel.SetEmpty();
        }

        //단축키 - 스냅샷 초기값.
        private void SetEmptySnapShotKey()
        {
            this.snapShotInputLabel.SetEmpty();
        }

        //단축키 - 한 번만 번역하기
        private void SetEmptyOneTranslate()
        {
            this.lbOneTrans.SetEmpty();
        }

        private void SetEmptyHideTranslate()
        {
            this.lbHideTranslate.SetEmpty();
        }


        //단축키 - 번역 초기값.
        private void InitTansKey()
        {
            transKeyInputLabel.SetDefaultKey();
        }

        //단축키 - 교정 사전 초기값.
        private void InitDicKey()
        {
            this.dicKeyInputLabel.SetDefaultKey();
        }

        //단축키 - 빠른 영역 초기값.
        private void InitQuickKey()
        {
            this.quickKeyInputLabel.SetDefaultKey();
        }

        //단축키 - 스냅샷 초기값.
        private void InitSnapShotKey()
        {
            this.snapShotInputLabel.SetDefaultKey();
        }

        //한 번만 번역하기 초기값
        private void InitOneTranslateKey()
        {
            this.lbOneTrans.SetDefaultKey();
        }


        //단축키 - 창 숨김 초기값.
        private void InitHideTransKey()
        {
            this.lbHideTranslate.SetDefaultKey();
        }




        private void SetTransLangugage(string resultCode)
        {
            Util.ShowLog("OCR Code : " + resultCode);
            TransManager.TransCodeData codeData = TransManager.Instace.GetTransCodeData(resultCode);

            if(codeData != null)
            {
                if(codeData.naverCode != "")
                {
                    foreach (var obj in naverTransComboBox.Items)
                    {
                        TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                        if (codeData.naverCode == data.naverCode)
                        {
                            naverTransComboBox.SelectedItem = obj;
                            break;
                        }
                    }
                }

                if(codeData.googleCode != "")
                {
                    foreach (var obj in googleTransComboBox.Items)
                    {
                        TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                        if (codeData.googleCode == data.googleCode)
                        {
                            googleTransComboBox.SelectedItem = obj;
                            break;
                        }
                    }
                }
            }

            return;

            if (resultCode == "ko")
            {
            }
            else if (resultCode == "ja")
            {
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
            }
            else if (resultCode == "en" || resultCode =="en-US")
            {
                naverTransComboBox.SelectedIndex = 0;
                googleTransComboBox.SelectedIndex = 0;
            }
        }       

        private void ChangeWinOcrLanguage(int index)
        {
            string resultCode = "";
            if (index < winLanguageCodeList.Count)
            {
                //Util.ShowLog(languageCodeList[WinOCR_Language_comboBox.SelectedIndex]);
                resultCode = winLanguageCodeList[index];
                if (resultCode == "ko")
                {
                    removeSpaceCheckBox.Checked = false;
                }
                else if (resultCode == "en" || resultCode == "en-US")
                {
                    removeSpaceCheckBox.Checked = false;
                    cbPerWordDic.Checked = true;
                }
                else if (resultCode == "ja" || resultCode == "zh-Hans-CN" || resultCode == "zh-Hant-TW")
                {
                    //20190106 일본어를 하면 자동으로 ocr 공백제거 선택
                    removeSpaceCheckBox.Checked = true;
                    cbPerWordDic.Checked = false;
                }

            }
            SetTransLangugage(resultCode);

            if(WinOCR_Language_comboBox.SelectedIndex != index)
            {
                WinOCR_Language_comboBox.SelectedIndex = index;
            }
        }

        private void WinOCR_Language_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ChangeWinOcrLanguage(WinOCR_Language_comboBox.SelectedIndex);
        }

        private void tabControl1_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }


        /// <summary>
        /// 구글 인증토큰 모두 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RemoveAllGoogleToekn_Click(object sender, EventArgs e)
        {
            FormManager.Instace.SetDisableSubMenuTopMost();
            if (DialogResult.OK == MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized }, "모든 구글 인증 토큰을 삭제하시겠습니까?", "구글 번역기", MessageBoxButtons.OKCancel))
            {
                TransManager.Instace.DeleteAllGsTransToken();
            }
            FormManager.Instace.ReSettingSubMenuTopMost();
        }

        private void donationButton_Click(object sender, EventArgs e)
        {
            ShowDonationPopup();
        }

        private void ShowDonationPopup()
        {
            FormManager.Instace.SetDisableSubMenuTopMost();

            FormManager.Instace.ShowDonatePage();

            FormManager.Instace.ReSettingSubMenuTopMost();
        }

        private void Button_NaverTransKeyList_Click(object sender, EventArgs e)
        {
            Action callback = delegate
            {
                if (TransManager.Instace.naverKeyList.Count > 0)
                {
                    var data = TransManager.Instace.naverKeyList[0];
                    naverIDKey = data.id;
                    naverSecretKey = data.secret;


                    NaverIDKeyTextBox.Text = naverIDKey;
                    NaverSecretKeyTextBox.Text = naverSecretKey;

                    NaverTranslateAPI.instance.ChangeValue(naverIDKey, naverSecretKey, data.isPaid);

                }
            };

            FormManager.Instace.ShowNaverKeyListUI(callback);
        }

    

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
            notifyIcon1.Icon = null;
        }

    }

}



