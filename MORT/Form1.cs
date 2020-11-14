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
using System.Windows.Forms;



namespace MORT
{

    public partial class Form1 : Form
    {

        public class ImgData
        {
            public List<byte> rList;
            public List<byte> gList;
            public List<byte> bList;

            public int x;
            public int y;

            public int index;
        }


        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::

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

        List<string> languageCodeList = new List<string>();

        bool isProgramStartFlag = false;                //모든게 다 로딩이 되었나
        public bool isAvailableWinOCR = true;           //윈도우 10 OCR 사용 가능한지 확인.
        private string winOcrErrorCode = "";
        public bool isShowWinOCRWarning = false;
        public SettingManager MySettingManager = new SettingManager(); //설정 관리자
        GlobalKeyboardHook gHook;
        List<int> nowKeyPressList = new List<int>();



        #region ::::::::::::::::::::::::::DLL:::::::::::::::::::::::::::::::::::::::::::::::::

        //MORT_CORE 침식함수
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setErode();

        //MORT_CORE 내부 동작 함수
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void processOcr(StringBuilder test, StringBuilder test1);

        //MORT_CORE 스펠링 체크
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetSpellingCheck(StringBuilder ocrResult, bool isUseJpn);

        //MORT_CORE DB만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetDBText(StringBuilder original, StringBuilder result);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr processGetImgData(int index, ref int x, ref int y, ref int channels);

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
        public static extern void setAdvencedImgOption(bool newIsUseRGBFlag, bool newIsUseHSVFlag, bool newIsUseErodeFlag, float imgZoomSize);

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

                for(int i = 0; i < modules.Length; i++)
                {
                    Console.WriteLine("sdfsdfdsfsf" + modules[i].ToString());
                }

                //loader.Initialize(1, "test2.Class1", "Test");
                //Type type = Type.GetType("namespace.qualified.TypeName, MORT_WIN10OCR.Class1");
                //Type type = modules[0].GetType("MORT_WIN10OCR.Class1");

                Type type = _assembly.GetType("MORT_WIN10OCR.Class1");
                MethodInfo method = type.GetMethod("TestOpenCv", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method2 = type.GetMethod("ProcessOCR", BindingFlags.Static | BindingFlags.Public);

                MethodInfo method3 = type.GetMethod("GetIsAvailable", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method4 = type.GetMethod("InitOcr", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method5 = type.GetMethod("GetIsAvailableDLL", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method6 = type.GetMethod("GetText", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method7 = type.GetMethod("GetAvailableLanguageList", BindingFlags.Static | BindingFlags.Public);

                MethodInfo method8 = type.GetMethod("TestMar", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method9 = type.GetMethod("TextToSpeach", BindingFlags.Static | BindingFlags.Public);



                matFunc = (Func<List<byte>, List<byte>, List<byte>, int, int, string>)Delegate.CreateDelegate(typeof(Func<List<byte>, List<byte>, List<byte>, int, int, string>), method);
                processOCRFunc = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method2);
                getTextFunc = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method6);
                getDLLAvailableFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method5);
                getOCRAvailableFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method3);
                initOCRFunc = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), method4);
                getLanguageListFunc = (Func<List<string>>)Delegate.CreateDelegate(typeof(Func<List<string>>), method7);

                textToSpeachFunc = (Action<string, int>)Delegate.CreateDelegate(typeof(Action<string, int>), method9);
                testFunc = (Func<IntPtr>)Delegate.CreateDelegate(typeof(Func<IntPtr>), method8);
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
                if(!string.IsNullOrEmpty(text))
                {
                    textToSpeachFunc(text, type);
                }
               
            }

            public string SetImg(List<byte> r, List<byte> g, List<byte> b, int x, int y)
            {
                string result = "yes";
                result = matFunc(r, g, b, x, y);


                return result;
            }

            public string ProcessOcrFunc()
            {
                string result = "yes";
                result = processOCRFunc();


                return result;
            }

            private Assembly _assembly;
            public Func<List<byte>, List<byte>, List<byte>, int, int, string> matFunc;
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
            //다른 창을 파괴하는 행위
            if (MySettingManager.NowSkin == SettingManager.Skin.dark &&
                (FormManager.Instace.MyLayerTransForm != null))
            {
                FormManager.Instace.DestoryTransForm();
                MakeTransForm();
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer &&
                (FormManager.Instace.MyBasicTransForm != null))
            {
                FormManager.Instace.DestoryTransForm();
                MakeTransForm();
            }

            bool isChange = false;
            if (MySettingManager.NowSkin == SettingManager.Skin.dark && !skinDarkRadioButton.Checked)
            {
                isChange = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer && !skinLayerRadioButton.Checked)
            {
                isChange = true;
            }
            //2019 01 06 다음버전으로 미룸
            //TODO : TEMP
            /*
            else if (MySettingManager.NowSkin == SettingManager.Skin.over && !skinOverRadioButton.Checked)
            {
                isChange = true;
            }
            */
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
                //TODO : TEMP
                /*
                else if(skinOverRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.over;
                }
                */
                MakeTransForm();
            }
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

            DateTime Tthen = DateTime.Now;
            do
            {
                Application.DoEvents();

            } while (Tthen.AddSeconds(0.7f) > DateTime.Now);
            logo.disableLogo(2.0f);

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
            /*
            //TODO : TEMP
            else if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                FormManager.Instace.MakeOverTransForm( isProcessTransFlag);
            }
            */

        }

        #endregion

        #region :::::::::::::::::::::::::::::::::::::::::::버전 확인 관련 :::::::::::::::::::::::::::::::::::::::::

        private bool GetCheckUpdate()
        {
            bool isCheckUpdate = true;

            string line = "";
            try
            {
                
                StreamReader r = new StreamReader(GlobalDefine.CHECK_UPDATE_FILE);
                line = r.ReadLine();
                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.CHECK_UPDATE_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                }

                using (StreamWriter newTask = new StreamWriter(GlobalDefine.CHECK_UPDATE_FILE, false))
                {
                    newTask.WriteLine("yes");
                    newTask.Close();
                }
            }

            if (line == null || line.CompareTo("") == 0 || line.CompareTo("yes") == 0)
            {
                isCheckUpdate = true;
            }
            else if (line.CompareTo("no") == 0)
            {
                isCheckUpdate = false;
            }

            return isCheckUpdate;
        }

        private void SetCheckUpdate(bool isUse)
        {
            if (isUse)
            {
                try
                {
                    using (StreamWriter newTask = new StreamWriter(GlobalDefine.CHECK_UPDATE_FILE, false))
                    {
                        newTask.WriteLine("yes");
                        newTask.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.CHECK_UPDATE_FILE))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter newTask = new StreamWriter(GlobalDefine.CHECK_UPDATE_FILE, false))
                        {
                            newTask.WriteLine("yes");
                            newTask.Close();
                        }
                    }
                }
            }
            else
            {
                try
                {
                    using (StreamWriter newTask = new StreamWriter(GlobalDefine.CHECK_UPDATE_FILE, false))
                    {
                        newTask.WriteLine("no");
                        newTask.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.CHECK_UPDATE_FILE))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter newTask = new StreamWriter(GlobalDefine.CHECK_UPDATE_FILE, false))
                        {
                            newTask.WriteLine("no");
                            newTask.Close();
                        }
                    }
                }
            }
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

        public void OpenSettingFile(string fileName)
        {

            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                LoadSettingfile(fileName);
                SetUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                LoadSettingfile(fileName);
                SetUIValueToSetting();
            }

            SaveSetting(GlobalDefine.USER_SETTING_FILE);
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

        private void CheckMortVersion(String content)
        {
            try
            {
                if (content != null)
                {
                    string newVersionString = "";
                    string downloadPage = "";

                    newVersionString = Util.ParseString(content, "@MORT_VERSION", '[', ']');
                    downloadPage = Util.ParseString(content, "@MORT_VERSION", '{', '}');
                   

                    Util.ShowLog(nowVersion + " / " + newVersionString + " / ");

                    if (nowVersion < Convert.ToInt32(newVersionString))
                    {
                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        string checkMessageSubtitle = "(" + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show("새로운 버전을 확인했습니다.\r\n업데이트하시겠습니까?  ", checkMessageSubtitle, MessageBoxButtons.OKCancel))
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

        private void CheckDicVersion(String content, string checkType,  string fileName)
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
                    Util.ShowLog(checkType + " : " +  dicVersion + " / " + newVersionString + " / download : " + downloadPage);

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
                                Util.ChangeFileData(GlobalDefine.DATA_VERSION_FILE,  checkType, newVersionString, '[', ']');

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
                            CheckDicVersion(content, "@MORT_DIC_ENG", @".\\DIC\\dic.txt");
                            CheckDicVersion(content, "@MORT_DIC_JPN", @".\\DIC\\dicJpn.txt");
                        }
                
                    }
         


                }
                catch (Exception e)
                {
                    Util.ShowLog(e.ToString());
                }
            }

        }

        private void InitTransCode()
        {
            yandexTransCodeComboBox.SelectedIndex = 0;
            yandexResultCodeComboBox.SelectedIndex = 0;

            naverTransComboBox.SelectedIndex = 0;

            googleTransComboBox.SelectedIndex = 0;
            googleResultCodeComboBox.SelectedIndex = 0;

            TransManager.Instace.InitTransCode();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();
        //폼 생성
        public Form1()
        {
            try
            {
                //SetProcessDPIAware();               

              
                InitializeComponent();

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
                    WinOCR_Language_comboBox.Items.Clear();
                    for (int i = 0; i < codeList.Count; i++)
                    {
                        string[] key = codeList[i].Split(',');
                        if (key.Length >= 2)
                        {
                            languageCodeList.Add(key[0]);
                            WinOCR_Language_comboBox.Items.Add(key[1]);
                        }
                    }

                    if (languageCodeList.Count > 0)
                    {
                        WinOCR_Language_comboBox.SelectedIndex = 0;
                        loader.InitOCR(languageCodeList[0]);
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
                OpenHotKeyFile();
                InitTransCode();


                LoadSettingfile(GlobalDefine.USER_SETTING_FILE);
                initOcr();
                //GDI+ 동작 여부 검사.
                CheckGDI();
                MakeLogo();

                MakeTransForm();
                SetUIValueToSetting();


                makeRTT();
                initKeyHooker();

                notifyIcon1.Visible = true;
                isProgramStartFlag = true;




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
        }

        //폼이 불러온 후 처리함.
        private void Form1_Load(object sender, EventArgs e)
        {

            using (Graphics graphics = this.CreateGraphics())
            {
                Util.SetDPI(graphics.DpiX, graphics.DpiY);
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
            try
            {
                using (StreamWriter newTask = new StreamWriter(GlobalDefine.HOTKEY_FILE, false))
                {
                    newTask.WriteLine(transKeyInputLabel.GetKeyListToString());
                    newTask.WriteLine(this.dicKeyInputLabel.GetKeyListToString());
                    newTask.WriteLine(this.quickKeyInputLabel.GetKeyListToString());
                    newTask.WriteLine(this.snapShotInputLabel.GetKeyListToString());
                    newTask.WriteLine(this.lbOneTrans.GetKeyListToString());
                    newTask.WriteLine(this.lbHideTranslate.GetKeyListToString());
                    newTask.Close();
                }


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.HOTKEY_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(GlobalDefine.HOTKEY_FILE, false))
                    {
                        newTask.WriteLine(transKeyInputLabel.GetKeyListToString());
                        newTask.WriteLine(this.dicKeyInputLabel.GetKeyListToString());
                        newTask.WriteLine(this.quickKeyInputLabel.GetKeyListToString());
                        newTask.WriteLine(this.snapShotInputLabel.GetKeyListToString());
                        newTask.WriteLine(this.lbOneTrans.GetKeyListToString());
                        newTask.WriteLine(this.lbHideTranslate.GetKeyListToString());
                        newTask.Close();
                    }
                }
            }

        }

        private void OpenHotKeyFile()
        {
            try
            {
                bool isOldFile = false;
                string filePath = GlobalDefine.HOTKEY_FILE;
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

                //한 번만 번역하기.
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

                if(isOldFile && File.Exists(GlobalDefine.HOTKEY_FILE_OLD))
                {
                    File.Delete(GlobalDefine.HOTKEY_FILE_OLD);
                }
                    

            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.HOTKEY_FILE))
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
            Keys code = e.KeyCode;

            //테스트용
            if(e.KeyCode == Keys.V)
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
            outlineColor1 = Color.FromArgb(100, 149, 237);
            outlineColor2 = Color.FromArgb(65, 105, 225);
            backgroundColor = Color.FromArgb(0, 0, 0);

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

            if (MessageBox.Show("종료하시겠습니까?", "종료하시겠습니까?", MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question) == DialogResult.Yes)
            {
                exitApplication();
            }

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
            TransManager.Instace.SaveNaverKeyFile(NaverIDKeyTextBox.Text, NaverSecretKeyTextBox.Text);
            

        }

        private void OpenNaverKeyFile()
        {
            try
            {
                TransManager.Instace.OpenNaverKeyFile();

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
            if(isAvailableWinOCR && MySettingManager.IsUseTTS)
            {
                int type = 0;
                if(MySettingManager.IsWaitTTSEnd)
                {
                    type = 1;
                }

                loader.TextToSpeach(text, type);
            }

        }


        public void ProcessTrans(bool isSnap = false)              //번역 시작 쓰레드
        {
            //isEndFlag = false;
            string formerOcrString = "";
            isClipeBoardReady = true;
            int lastTick = 0;
            try
            {
                while (isEndFlag == false)
                {
                    int diff = Math.Abs(System.Environment.TickCount - lastTick);

                    //TODO :빠른 속도를 원하면 저 주석 해제하면 됨
                    if (diff >= ocrProcessSpeed/* / 10*/)
                    {
                        lastTick = System.Environment.TickCount;

                        //TODO : TEMP FormManager.Instace.MyOverTransForm != null
                        if (FormManager.Instace.MyBasicTransForm != null || FormManager.Instace.MyLayerTransForm != null)
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
                                        int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                        List<ImgData> imgDataList = new List<ImgData>();
                                        //TODO : 이미지 모두 가져온 후 처리하는 걸로 바꾸어야 함.
                                        for (int j = 0; j < ocrAreaCount; j++)
                                        {
                                            int x = 15;
                                            int y = 0;
                                            int channels = 4;
                                            //IntPtr data = processGetImgData(j, ref x, ref y, ref channels);
                                            IntPtr data = IntPtr.Zero;
                                            data = processGetImgData(j, ref x, ref y, ref channels);

                                            if (data != IntPtr.Zero)
                                            {
                                                var arr = new byte[x * y * channels];
                                                Marshal.Copy(data, arr, 0, x * y * channels);

                                                Marshal.FreeHGlobal(data);

                                                List<byte> rList = new List<byte>();
                                                List<byte> gList = new List<byte>();
                                                List<byte> bList = new List<byte>();
                                                // Util.ShowLog(channels.ToString());
                                                //bgra.
                                                if (channels == 1)
                                                {
                                                    for (int i = 0; i < arr.Length; i++)
                                                    {
                                                        bList.Add(arr[i]);
                                                        gList.Add(arr[i]);
                                                        rList.Add(arr[i]);
                                                    }
                                                }
                                                else
                                                {
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

                                                ImgData imgData = new ImgData();
                                                imgData.rList = rList;
                                                imgData.gList = gList;
                                                imgData.bList = bList;
                                                imgData.x = x;
                                                imgData.y = y;
                                                imgData.index = j;
                                                imgDataList.Add(imgData);
                                            }
                                        }

                                        string ocrResult = "";
                                        string transResult = "";
                                        argv3 = "";
                                        for (int j = 0; j < imgDataList.Count; j++)
                                        {
                                            //Util.ShowLog(imgDataList[j].rList + " / " + imgDataList[j].gList + " / " + imgDataList[j].bList + " / " + imgDataList[j].x + " / " + imgDataList[j].y);
                                            loader.SetImg(imgDataList[j].rList, imgDataList[j].gList, imgDataList[j].bList, imgDataList[j].x, imgDataList[j].y);
                                            loader.ProcessOcrFunc();

                                            while (!isEndFlag && !loader.GetIsAvailableOCR())
                                            {
                                                //Thread.SpinWait(1);
                                                Thread.Sleep(2);
                                            }

                                            string result = loader.GetText();


                                            IntPtr ptr = loader.GetMar();
                                            WinOCRResultData point = (WinOCRResultData)Marshal.PtrToStructure(ptr, typeof(WinOCRResultData));
                                            OCRDataManager.Instace.InitData(point);

                                            Marshal.FreeCoTaskMem(ptr);

                                            if (MySettingManager.NowIsRemoveSpace == true)
                                            {
                                                result = result.Replace(" ", "");
                                            }

                                            //교정 사전 사용 여부 체크.
                                            if (MySettingManager.NowIsUseDicFileFlag)
                                            {
                                                StringBuilder sb = new StringBuilder(result, 8192);
                                                //Util.ShowLog(MySettingManager.NowIsUseJpnFlag + " Before : " + result);
                                                ProcessGetSpellingCheck(sb, MySettingManager.isUseMatchWordDic);
                                                result = sb.ToString();       //ocr 결과
                                                sb.Clear();
                                            }

                                            //------------------OCR 줄바꿈 없애기 처리---------------------

                                            if(MySettingManager.NowTransType != SettingManager.TransType.db)
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

                                            System.Threading.Tasks.Task<string> transTask = TransManager.Instace.StartTrans(result, MySettingManager.NowTransType);

                                            //------------------OCR 줄바꿈 없애기 처리---------------------

                                            if (MySettingManager.NowTransType == SettingManager.TransType.db)
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

                                            transResult = transTask.Result;

                                            if (imgDataList.Count > 1)
                                            {
                                                if (MySettingManager.IsShowOCRIndex)
                                                {
                                                    if(!string.IsNullOrEmpty(result))
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

                              
                                processOcr(sb, sb2);
                                nowOcrString = sb.ToString();       //ocr 결과

                                //------------------OCR 줄바꿈 없애기 처리---------------------
                                nowOcrString = nowOcrString.Replace("\r\n", "\n");

                                if(MySettingManager.NowIsRemoveSpace)
                                {
                                    nowOcrString = nowOcrString.Replace("\n", "");
                                }
                                else
                                {
                                    nowOcrString = nowOcrString.Replace("\n", " ");
                                }
                                //---------------------------------------
                                nowOcrString = nowOcrString.Replace("\t", System.Environment.NewLine);

                                // nowOcrString = nowOcrString.Replace("\n", "\r\n");
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
                                    FormManager.Instace.MyLayerTransForm.updateText(argv3, nowOcrString, transType, MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag);
                                }

                                DoTextToSpeach(argv3);
                              

                                /*
                                //TODO : TEMP
                                else if (MySettingManager.NowSkin == SettingManager.Skin.over && FormManager.Instace.MyOverTransForm != null)
                                {
                                    FormManager.Instace.MyOverTransForm.updateText(argv3, nowOcrString,  MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag);
                                }
                                */
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
            if(isBeforeSnapShot)
            {
                SetCaptureArea();
            }

            if(FormManager.Instace.GetOcrAreaCount() == 0)
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

            if(MySettingManager.NowTransType == SettingManager.TransType.google_url)
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
            if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                if (FormManager.Instace.MyLayerTransForm != null)
                {
                    //한번만 번역 & 강제 투명화 -> 번역이 끝나도 투명상태 유지.
                    if (isOnceTrans)
                    {
                        if (!FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency)
                        {
                            FormManager.Instace.MyLayerTransForm.setVisibleBackground();
                            FormManager.Instace.MyLayerTransForm.disableOverHitLayer();
                        }
                    }
                    else
                    {
                        FormManager.Instace.MyLayerTransForm.setVisibleBackground();
                        FormManager.Instace.MyLayerTransForm.disableOverHitLayer();
                    }
              
                }
            }

            else
            {

                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.StopTrans();
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


        public void clickCaptureAreaButton()            //영역 검색 버튼 클릭
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
            CloseApplication();
            e.Cancel = true;//종료를 취소하고 
        }

        #region:::::::::::::::::::::::::::::::::::::::::::체크박스 및 라디오 클릭:::::::::::::::::::::::::::::::::::::::::::

        private void checkRGB_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkHSV.Checked == true)
            {
                checkHSV.Checked = false;
                MySettingManager.NowIsUseHSVFlag = false;
                MySettingManager.NowIsUseRGBFlag = true;

            }
        }
        private void checkHSV_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkRGB.Checked == true)
            {
                checkRGB.Checked = false;
                MySettingManager.NowIsUseHSVFlag = true;
                MySettingManager.NowIsUseRGBFlag = false;
            }
        }

        private void languageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (languageComboBox.SelectedIndex == 0)
            {
                tessDataTextBox.Text = "eng";
                yandexTransCodeComboBox.SelectedIndex = 0;
                naverTransComboBox.SelectedIndex = 0;
                googleTransComboBox.SelectedIndex = 0;
                removeSpaceCheckBox.Checked = false;
                cbPerWordDic.Checked = true;
            }
            else if (languageComboBox.SelectedIndex == 1)
            {
                tessDataTextBox.Text = "jpn";
                yandexTransCodeComboBox.SelectedIndex = 1;
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
                removeSpaceCheckBox.Checked = true;
                cbPerWordDic.Checked = false;
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

        #endregion


        private void acceptButton_Click(object sender, EventArgs e)
        {
            acceptButton.Focus();
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                SetUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                SetUIValueToSetting();
            }

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
            //설정 저장
            SaveSetting(GlobalDefine.USER_SETTING_FILE);

            bool isError = GetIsHasError();

            if (!isError)
            {
                FormManager.ShowPopupMessage("MORT", "적용 완료");
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
            clickCaptureAreaButton();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && isProgramStartFlag == true)
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

        private void settingSaveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                SetUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                SetUIValueToSetting();
            }
            SaveFileDialog savePanel = new SaveFileDialog();
            savePanel.RestoreDirectory = false;
            savePanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            savePanel.Filter = "Config File (*.conf)|*.conf";
            if (savePanel.ShowDialog() == DialogResult.OK)
            {
                SaveSetting(savePanel.FileName);
            }
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
                SetUIValueToSetting();
                thread = new Thread(() => ProcessTrans(false));
                thread.Start();
            }
            else
            {
                MySettingManager.SetDefault();
                SetValueToUIValue();
                SetUIValueToSetting();
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

        //OCR 방식 변경
        private void OCR_Type_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tesseract_panel.Visible = false;
            WinOCR_panel.Visible = false;
            //string selectItem = OCR_Type_comboBox.SelectedItem.ToString();
            SettingManager.OcrType ocrType = SettingManager.GetOcrType(OCR_Type_comboBox.SelectedIndex);
            if (ocrType == SettingManager.OcrType.Tesseract)
            {
                Tesseract_panel.Visible = true;

            }
            else if (ocrType == SettingManager.OcrType.Window)
            {
                WinOCR_panel.Visible = true;


                if (isProgramStartFlag && isAvailableWinOCR && !isShowWinOCRWarning && languageCodeList.Count == 1)
                {
                    if (languageCodeList[0] == "ko")
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
                if (languageComboBox.SelectedIndex != 1)
                {
                    languageComboBox.SelectedIndex = 1;
                    yandexTransCodeComboBox.SelectedIndex = 1;
                    tessDataTextBox.Text = "jpn";
                    naverTransComboBox.SelectedIndex = 1;
                }
            }
        }

        //번역 방식 변경.
        private void TransType_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB_Panel.Visible = false;
            Yandex_Panel.Visible = false;
            Naver_Panel.Visible = false;
            Google_Panel.Visible = false;
            pnGoogleBasic.Visible = false;

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
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.Z);
            transKeyInputLabel.ResetInput(list);
        }

        //단축키 - 교정 사전 초기값.
        private void InitDicKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.S);
            this.dicKeyInputLabel.ResetInput(list);
        }

        //단축키 - 빠른 영역 초기값.
        private void InitQuickKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.X);
            this.quickKeyInputLabel.ResetInput(list);
        }

        //단축키 - 스냅샷 초기값.
        private void InitSnapShotKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.A);
            this.snapShotInputLabel.ResetInput(list);
        }

        //한 번만 번역하기 초기값
        private void InitOneTranslateKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.C);
            this.lbOneTrans.ResetInput(list);
        }


        //단축키 - 창 숨김 초기값.
        private void InitHideTransKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.D);
            this.lbHideTranslate.ResetInput(list);
        }









        #region ::::::::: 윈 OCR 언어 선택 관련 :::::::::::

        private void SetTransLangugageForWinOCR(string resultCode)
        {
            if (resultCode == "ko")
            {
                for (int i = 0; i < TransManager.Instace.transCodeList.Count; i++)
                {
                    if (TransManager.Instace.transCodeList[i] == "ko")
                    {
                        yandexTransCodeComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (resultCode == "ja")
            {
                yandexTransCodeComboBox.SelectedIndex = 1;
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
            }
            else if (resultCode == "en")
            {
                yandexTransCodeComboBox.SelectedIndex = 0;
                naverTransComboBox.SelectedIndex = 0;
                googleTransComboBox.SelectedIndex = 0;
            }
        }

        #endregion

        private void WinOCR_Language_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string resultCode = "";
            if (WinOCR_Language_comboBox.SelectedIndex < languageCodeList.Count)
            {
                //Util.ShowLog(languageCodeList[WinOCR_Language_comboBox.SelectedIndex]);
                string selectCode = languageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                if (selectCode == "ko")
                {
                    resultCode = "ko";
                }
                else if (selectCode == "en" || selectCode == "en-US")
                {
                    resultCode = "en";
                    removeSpaceCheckBox.Checked = false;
                    cbPerWordDic.Checked = true;
                }
                else if (selectCode == "ja")
                {
                    resultCode = "ja";

                    //20190106 일본어를 하면 자동으로 ocr 공백제거 선택
                    removeSpaceCheckBox.Checked = true;
                    cbPerWordDic.Checked = false;
                }
            }
            SetTransLangugageForWinOCR(resultCode);
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

        private void button1_Click(object sender, EventArgs e)
        {

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

            /*
            if (DialogResult.OK == MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized },
                " 후원 계좌\n하나은행 764-910283-44807 김무영\n\n 네이버 페이\nID : killkimno\n받는사람 : 김무영\n", "후원하기", MessageBoxButtons.OK))
            {
                {
                    try
                    {

                    }
                    catch { }
                }
            }

            */
            FormManager.Instace.ReSettingSubMenuTopMost();
        }

        private void Button_NaverTransKeyList_Click(object sender, EventArgs e)
        {
            FormManager.Instace.ShowNaverKeyListUI();
        }

        private void btnTransHelp_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221760617100");
            }
            catch { }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
            notifyIcon1.Icon = null;
        }
    }

}



