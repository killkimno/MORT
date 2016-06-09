//만든이 : 몽키해드
//블로그 주소 : http://killkimno.blog.me/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Media;



namespace MORT
{
    
    public partial class Form1 : Form
    {

        List<string> transCodeList = new List<string>();
        List<string> resultCodeList = new List<string>();

        List<string> naverTransCodeList = new List<string>();
        List<string> naverResultCodeList = new List<string>();


        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::
       
        public delegate void PDelegateSetSpellCheck();
        
        string nowOcrString = "";                   //현재 ocr 문장

        //현재 버전
        int nowVersion = 1140;
        
        //IntPtr observerHwnd;
        //번역 쓰레드
        Thread thread;
        volatile bool isEndFlag = false;            //번역 끝내는 플레그
        bool isTranslateFormTopMostFlag = true;     //번역창이 최상위냐 아니냐
        bool isUseGoogleCount;

        //enum Skin {dark, layer };                  //스킨 열거형
        //Skin nowSkin = Skin.dark;                   //현재 스킨 - 다크
        private Point mousePoint;                   //창 이동 관련
        int ocrProcessSpeed = 2000;                 //ocr 처리 딜레이 시간

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

        List<ColorGroup> colorGroup = new List<ColorGroup>();   //색 그룹 리스트

        bool isProcessTransFlag = false;

        SettingManager.TransType transType;
        /*
        bool isUseBingFlag = true;                  //빙 번역기 사용
        bool isUseDBFlag = false;                    //DB 사용
        bool isUseNaver = false;                    //네이버 번역기 사용
        */
        string bingAccountKey = "i2nV6GJf/7gPC7WTCq1VMlg6bN7OerxF857zqif7HSc=";
        string naverIDKey = "";
        string naverSecretKey = "";

        bool isProgramStartFlag = false;                //모든게 다 로딩이 되었나

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

        //MORT_CORE 이미지 영역 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCutPoint(int []newX, int []newY, int []newX2, int []newY2, int size);

        //MORT_CORE 초기화
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initOcr();

        //MORT_CORE 폰트 교육자료 설정
        [DllImport(@"DLL\\MORT_CORE.dll",CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTessdata(string tessData, bool isUseJpnFlag);

        //MORT_CORE RGB, HSV 값 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setFiducialValue(int []newValueR, int []newValueG, int []newValueB, int []newValueS1, int []newValueS2, int []newValueV1, int []newValueV2, int size);

        //MORT_CORE 빙 / DB 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll",CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setUseDB(bool newIsUseDBFlag, string newDbFileText);

        //MORT_CORE 교정 사전 사용
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setUseCheckSpelling(bool newIsUseCheckSpellingFlag, string newDicFileText);

        //MORT_CORE 이미지 보정 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setAdvencedImgOption(bool newIsUseRGBFlag, bool newIsUseHSVFlag, bool newIsUseErodeFlag, float imgZoomSize);

        //MORT_CORE NHocr 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsUseNHocr(bool isUseNHocr);

        //MORT_CORE 대소문자 구분 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsStringUpper(bool isUpper);

        //MORT_CORE 활성화 윈도우 캡쳐 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsActiveWindow(bool isActiveWindow);

        //MORT_CORE 사용할 색그룹 추가
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddOcrColorSet(int[] colorList, int size);

        //MORT_CORE 사용할 색그룹 초기화
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearOcrColorSet();

        #endregion


        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::Skin Change Function:::::::::::::::::::::::::::::::::::::::::::
        public void changeSkin()
        {
            if(MySettingManager.NowSkin == SettingManager.Skin.dark && FormManager.Instace.MyLayerTransForm != null)
            {
                FormManager.Instace.DestoryTransForm();
                makeTransForm();
            }

            else if(MySettingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyBasicTransForm != null)
            {
                FormManager.Instace.DestoryTransForm();
                makeTransForm();
            }

            if (MySettingManager.NowSkin == SettingManager.Skin.dark && skinLayerRadioButton.Checked)
            {
                FormManager.Instace.DestoryTransForm();
                MySettingManager.NowSkin = SettingManager.Skin.layer;
                makeTransForm();
            }
            else if(MySettingManager.NowSkin == SettingManager.Skin.layer && skinDarkRadioButton.Checked)
            {
                FormManager.Instace.DestoryTransForm();
                MySettingManager.NowSkin = SettingManager.Skin.dark;
                makeTransForm();                
            }
        }
        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::폼 생성 관련 함수:::::::::::::::::::::::::::::::::::::::::::

        private void makeSearchOptionForm()
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
            FormManager.Instace.MakeDicEditorForm(nowOcrString, MySettingManager.NowIsUseJpnFlag,  MySettingManager.NowDicFile);

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
        }


        private void makeTransForm()
        {
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                FormManager.Instace.MakeBasicTransForm(bingAccountKey, isTranslateFormTopMostFlag);
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                FormManager.Instace.MakeLayerTransForm(bingAccountKey, isTranslateFormTopMostFlag, isProcessTransFlag);                
            }

        }

        #endregion

        #region :::::::::::::::::::::::::::::::::::::::::::버전 확인 관련 :::::::::::::::::::::::::::::::::::::::::
        
        private bool GetCheckUpdate()
        {
            bool isCheckUpdate = true;

            string line = "";
            try
            {
                StreamReader r = new StreamReader(@"checkUpdate.txt");
                line = r.ReadLine();
                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"checkUpdate.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                }

                using (StreamWriter newTask = new StreamWriter(@"checkUpdate.txt", false))
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
                    using (StreamWriter newTask = new StreamWriter(@"checkUpdate.txt", false))
                    {
                        newTask.WriteLine("yes");
                        newTask.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(@"checkUpdate.txt"))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter newTask = new StreamWriter(@"checkUpdate.txt", false))
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
                    using (StreamWriter newTask = new StreamWriter(@"checkUpdate.txt", false))
                    {
                        newTask.WriteLine("no");
                        newTask.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(@"checkUpdate.txt"))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter newTask = new StreamWriter(@"checkUpdate.txt", false))
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
            catch(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                TransFormLayer.isActiveGDI = false;
                CustomLabel.isActiveGDI = false;
                if (DialogResult.OK == MessageBox.Show("GDI+ 가 작동하지 않습니다. \n레이어 번역창의 일부 기능을 사용할 수 없습니다.\n해결법을 확인해 보겠습니까? ", "GDI+ 에서 일반 오류가 발생했습니다.", MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        System.Diagnostics.Process.Start("http://killkimno.blog.me/70185869419");
                    }
                    catch { }
                }
            }
            

        }

        //Setting 메니져에 저장된 값을 기본 셋팅에 적용함.
        void SetValueToUIValue()
        {
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                skinDarkRadioButton.Checked = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                skinLayerRadioButton.Checked = true;
            }

            showOcrCheckBox.Checked = MySettingManager.NowIsShowOcrResultFlag;
            saveOCRCheckBox.Checked = MySettingManager.NowIsSaveOcrReulstFlag;
            isClipBoardcheckBox1.Checked = MySettingManager.NowIsSaveInClipboardFlag;

            TransType_Combobox.SelectedIndex = (int)MySettingManager.NowTransType;

            checkStringUpper.Checked = MySettingManager.IsUseStringUpper;
            checkRGB.Checked = MySettingManager.NowIsUseRGBFlag;
            checkHSV.Checked = MySettingManager.NowIsUseHSVFlag;
            checkErode.Checked = MySettingManager.NowIsUseErodeFlag;

            switch (MySettingManager.NowOCRSpeed)
            {
                case 1:
                    speedRadioButton1.Checked = true;
                    break;

                case 2:
                    speedRadioButton2.Checked = true;
                    break;

                case 3:
                    speedRadioButton3.Checked = true;
                    break;

                case 4:
                    speedRadioButton4.Checked = true;
                    break;

                case 5:
                    speedRadioButton5.Checked = true;
                    break;

                default:
                    speedRadioButton3.Checked = true;
                    break;

            }

            dbFileTextBox.Text = MySettingManager.NowDBFile;
            tessDataTextBox.Text = MySettingManager.NowTessData;
            dicFileTextBox.Text = MySettingManager.NowDicFile;

            checkDic.Checked = MySettingManager.NowIsUseDicFileFlag;
            setCheckSpellingToolStripMenuItem.Checked = MySettingManager.NowIsUseDicFileFlag;

            //언어 설정.
            if (MySettingManager.NowIsUseEngFlag)
            {
                languageComboBox.SelectedIndex = 0;
            }
            else if(MySettingManager.NowIsUseJpnFlag)
            {
                languageComboBox.SelectedIndex = 1;
            }
            else if(MySettingManager.NowIsUseOtherLangFlag)
            {
                languageComboBox.SelectedIndex = 2;
            }

            //번역 코드 설정.    
            //빙.
            for (int i = 0; i < transCodeList.Count; i++ )
            {
                if(transCodeList[i].Equals(MySettingManager.TransCode))
                {
                    transCodeComboBox.SelectedIndex = i;
                    break;
                }
            }


            for (int i = 0; i < resultCodeList.Count; i++)
            {
                if (resultCodeList[i].Equals(MySettingManager.ResultCode))
                {
                    resultCodeComboBox.SelectedIndex = i;
                    break;
                }
            }
                        
            //네이버.
            for (int i = 0; i < naverTransCodeList.Count; i++)
            {
                if (naverTransCodeList[i].Equals(MySettingManager.NaverTransCode))
                {
                    naverTransComboBox.SelectedIndex = i;
                    break;
                }
            }


            isUseNHocrBox.Checked = MySettingManager.NowIsUseNHocr;

            initColorGroup();

            colorGroup = MySettingManager.NowColorGroup;
            if (colorGroup.Count != 0)
            {
                rTextBox.Text = colorGroup[0].getValueR().ToString();
                gTextBox.Text = colorGroup[0].getValueG().ToString();
                bTextBox.Text = colorGroup[0].getValueB().ToString();

                v1TextBox.Text = colorGroup[0].getValueV1().ToString();
                v2TextBox.Text = colorGroup[0].getValueV2().ToString();
                s1TextBox.Text = colorGroup[0].getValueS1().ToString();
                s2TextBox.Text = colorGroup[0].getValueS2().ToString();

                for (int i = 2; i <= MySettingManager.NowColorGroupCount; i++)
                {
                    groupCombo.Items.Add(i);
                }
            }

            locationXList = MySettingManager.NowLocationXList;
            locationYList = MySettingManager.NowLocationYList;
            sizeXList = MySettingManager.NowSizeXList;
            sizeYList = MySettingManager.NowSizeYList;

            textFont = MySettingManager.TextFont;
            textColor = MySettingManager.TextColor;
            outlineColor1 = MySettingManager.OutLineColor1;
            outlineColor2 = MySettingManager.OutLineColor2;
            backgroundColor = MySettingManager.BackgroundColor;

            if (MySettingManager.NowSortType == SettingManager.SortType.Center)
                alignmentCenterCheckBox.Checked = true;
            else
                alignmentCenterCheckBox.Checked = false;
            useBackColorCheckBox.Checked = MySettingManager.NowIsUseBackColor;
            removeSpaceCheckBox.Checked = MySettingManager.NowIsRemoveSpace;

            //엑티브 윈도우
            activeWinodeCheckBox.Checked = MySettingManager.NowIsActiveWindow;
            //구글 카운트
            allowGoogleCountCheckBox.Checked = isUseGoogleCount;

            //업데이트 확인
            checkUpdateCheckBox.Checked = GetCheckUpdate();

            topMostcheckBox.Checked = isTranslateFormTopMostFlag;
            setTranslateTopMostToolStripMenuItem.Checked = isTranslateFormTopMostFlag;
            
            MySettingManager.TextColor = Color.FromArgb(15, 15, 15);
            fontButton.Text = textFont.FontFamily.Name;
            fontSizeUpDown.Value = (int)textFont.Size;
            SetColorBoxColor(textColorBox, textColor);
            SetColorBoxColor(outlineColor1Box, outlineColor1);
            SetColorBoxColor(outlineColor2Box, outlineColor2);
            SetColorBoxColor(backgroundColorBox, backgroundColor);

            imgZoomsizeUpDown.Value = (decimal)MySettingManager.ImgZoomSize;


            FormManager.Instace.ResetCaputreAreaForm();
        }

        //파일로 부터 세팅 불러옴
        void openSettingfile(string fileName)
        {
            MySettingManager.openSettingfile(fileName);
            SetValueToUIValue();
            
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
                gHook.HookedKeys.Add(key);
            gHook.hook();
        }

        //버전 확인.
        void CheckVersion()
        {
            if (GetCheckUpdate())
            {
                try
                {
                    //http://killkimno.github.io/MORT_VERSION/version.txt
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("http://killkimno.github.io/MORT_VERSION/version.txt");
                    StreamReader reader = new StreamReader(stream);
                    String content = reader.ReadToEnd();
                    if (content != null)
                    {
                        int point = 0;
                        string newVersionString = "";
                        string downloadPage = "";
                        point = content.LastIndexOf("MORT_VERSION");
                        for (int i = point + 12; i < content.Length; i++)
                        {
                            if (content[i] == '[')
                            {
                                i++;
                                while (content[i] != ']')
                                {
                                    newVersionString = newVersionString + content[i];
                                    i++;
                                }

                            }

                            if (content[i] == '{')
                            {
                                i++;
                                while (content[i] != '}')
                                {
                                    downloadPage = downloadPage + content[i];
                                    i++;
                                }
                                break;
                            }

                        }

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
                catch (Exception e)
                {

                }
            }           
        }

        private void InitTransCode()
        {
            transCodeComboBox.SelectedIndex = 0;
            resultCodeComboBox.SelectedIndex = 0;

            naverTransComboBox.SelectedIndex = 0;

            transCodeList.Add("en");
            transCodeList.Add("ja");
            transCodeList.Add("zh-CHS");
            transCodeList.Add("zh-CHT");
            transCodeList.Add("ko");
            transCodeList.Add("ru");
            transCodeList.Add("de");
            transCodeList.Add("pt");

            resultCodeList.Add("ko");
            resultCodeList.Add("en");
            resultCodeList.Add("ja");
            resultCodeList.Add("zh-CHS");
            resultCodeList.Add("zh-CHT");
            resultCodeList.Add("ru");
            resultCodeList.Add("de");
            resultCodeList.Add("pt");

            naverTransCodeList.Add("en");
            naverTransCodeList.Add("ja");
            naverTransCodeList.Add("zh-CN");

            naverResultCodeList.Add("ko");
        }

        //폼 생성
        public Form1()
        {
            try
            {
                InitializeComponent();

                FormManager.Instace.MyMainForm = this;
                notifyIcon1.Visible = false;

                NaverTranslateAPI.instance = new NaverTranslateAPI();

                NaverTranslateAPI.instance.Init("43R0flRPIkMw3X531whI", "l9PcHlYOBE");
                NaverTranslateAPI.instance.SetTransCode("en", "ko");

                CheckUseCount();
                openBingKeyFile();
                OpenNaverKeyFile();
                OpenHotKeyFile();
                InitTransCode();
                
                
                openSettingfile(@".\\setting\\setting.conf");
                initOcr();
                //GDI+ 동작 여부 검사.
                CheckGDI();
                MakeLogo();

                makeTransForm();
                SetUIValueToSetting();
                               

                makeRTT();
                initKeyHooker();

                WebCounter.Dispose();

                notifyIcon1.Visible = true;
                isProgramStartFlag = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                try
                {
                    System.Diagnostics.Process.Start("http://killkimno.blog.me/70185869419");
                }
                catch { }
                this.Close();
                //MessageBox.Show(e.Message);
            }
        }
        #endregion

        #region ::::::::: 통계 관련 :::::::::::

        //설정에서 통계 사용 여부
        private void SetCheckUseGoogleCount(bool isUse)
        {
            isUseGoogleCount = isUse;

            if(isUse)
            {
                try
                {
                    using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                    {
                        newTask.WriteLine("yes");
                        newTask.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(@"checkUseCount.txt"))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
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
                    using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                    {
                        newTask.WriteLine("no");
                        newTask.Close();
                    }
                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(@"checkUseCount.txt"))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                        {
                            newTask.WriteLine("no");
                            newTask.Close();
                        }
                    }
                }
            }
        }

        private void CheckUseCount()
        {
            string line = ""; 
            try
            {
                StreamReader r = new StreamReader(@"checkUseCount.txt");
                line = r.ReadLine();
                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"checkUseCount.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            isUseGoogleCount = false;

            if (line == null || line.CompareTo("") == 0)
            {
                if (DialogResult.OK == MessageBox.Show("구글 통계 사용을 허용하시겠습니까?.\r\n통계 이외의 목적으로는 사용되지 않습니다. ", "통계를 사용하시겠습니까?", MessageBoxButtons.OKCancel))
                {
                    isUseGoogleCount = true;
                    try
                    {
                        Logo.SetTopmost(false);
                        using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                        {
                            WebCounter.Navigate("http://goo.gl/1J12p8");
                            newTask.WriteLine("yes");
                            newTask.Close();
                            Logo.SetTopmost(true);
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        using (System.IO.FileStream fs = System.IO.File.Create(@"checkUseCount.txt"))
                        {
                            fs.Close();
                            fs.Dispose();
                            using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                            {
                                newTask.WriteLine("yes");
                                newTask.Close();
                                Logo.SetTopmost(true);
                            }
                        }
                    }
                }
                else
                {
                    isUseGoogleCount = false;
                    try
                    {
                        using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                        {
                            newTask.WriteLine("no");
                            newTask.Close();
                        }


                    }
                    catch (FileNotFoundException)
                    {
                        using (System.IO.FileStream fs = System.IO.File.Create(@"checkUseCount.txt"))
                        {
                            fs.Close();
                            fs.Dispose();
                            using (StreamWriter newTask = new StreamWriter(@"checkUseCount.txt", false))
                            {
                                newTask.WriteLine("no");
                                newTask.Close();
                            }
                        }
                    }
                }
            }
            else if(line.CompareTo("yes") == 0)
            {
                isUseGoogleCount = true;
                WebCounter.Navigate("http://goo.gl/1J12p8");
            }
        }


        #endregion

        #region::::::::::::::::::::::::::::::::::::::::::키 후킹::::::::::::::::::::::::::::::::::::::::::::::::::

        private void SaveHotKeyFile()
        {
            try
            {
                using (StreamWriter newTask = new StreamWriter(@"hotKeyStting.txt", false))
                {
                    newTask.WriteLine(transKeyInputLabel.GetKeyListToString());
                    newTask.WriteLine(this.dicKeyInputLabel .GetKeyListToString());
                    newTask.WriteLine(this.quickKeyInputLabel .GetKeyListToString());
                    newTask.Close();
                }


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"hotKeyStting.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(@"hotKeyStting.txt", false))
                    {
                        newTask.WriteLine(transKeyInputLabel.GetKeyListToString());
                        newTask.WriteLine(this.dicKeyInputLabel.GetKeyListToString());
                        newTask.WriteLine(this.quickKeyInputLabel.GetKeyListToString());
                        newTask.Close();
                    }
                }
            }

        }

        private void OpenHotKeyFile()
        {
            try
            {
                StreamReader r = new StreamReader(@"hotKeyStting.txt");

                string line = r.ReadLine();
                
                if(line == null || line == "")
                {
                    line = "";
                    InitTansKey();
                }
                else
                {
                    transKeyInputLabel.SetKeyList(line);
                }
                
                line = r.ReadLine();
                if(line == null || line =="")
                {
                    line = "";
                    InitQuickKey();
                }
                else
                {
                    quickKeyInputLabel.SetKeyList(line);
                }
                
                line = r.ReadLine();
                if (line == null || line == "")
                {
                    line = "";
                    InitDicKey();

                }
                else
                {
                    dicKeyInputLabel.SetKeyList(line); 
                }
                
                r.Close();
                r.Dispose();

            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"hotKeyStting.txt"))
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
            if(transKeyInputLabel.isFocus || quickKeyInputLabel.isFocus || dicKeyInputLabel.isFocus)
            {
                return;
            }
            bool isHas = false;
   
            for (int i = 0; i < inputKeyList.Count; i++ )
            {
                if (inputKeyList[i] == code)
                {
                    isHas = true;
                }
            }

            if(!isHas)
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
                    StartTrnas();
                }
                else if (thread != null && thread.IsAlive == true)
                {
                    StopTrans();
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


        private void ShowResultFont()
        {
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

            if(dr == DialogResult.OK)
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
            this.colorDialog1.Color = backgroundColor;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                backgroundColor = this.colorDialog1.Color;
                SetColorBoxColor(backgroundColorBox, this.colorDialog1.Color);
            }
        }


        #endregion

        //설정 파일로 저장.
        private void saveSetting(string fileName)
        {
            MySettingManager.NowTransType = transType;

            /*
            if (isUseBingFlag == true)
            {
                MySettingManager.NowTransType = SettingManager.TransType.bing;
            }
            else if (isUseDBFlag == true)
            {
                MySettingManager.NowTransType = SettingManager.TransType.db;
            }
            */

            MySettingManager.NowOCRSpeed = (ocrProcessSpeed / 500) - 1;
            MySettingManager.NowColorGroupCount = groupCombo.Items.Count - 2;
            MySettingManager.NowColorGroup = colorGroup;
            MySettingManager.NowOCRGroupcount = locationXList.Count;
            MySettingManager.NowLocationXList = locationXList;
            MySettingManager.NowLocationYList = locationYList;
            MySettingManager.NowSizeXList = sizeXList;
            MySettingManager.NowSizeYList = sizeYList;

            MySettingManager.saveSetting(fileName);

        }

        //프로그램 닫기
        private void CloseApplication()
        {
            Boolean isFindFormFlag = false;

            
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "TransForm")
                {
                    if (frm.Visible == true)
                    {
                        isFindFormFlag = true;
                        break;
                    }

                }
            }
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "TransFormLayer")
                {
                    if (frm.Visible == true)
                    {
                        isFindFormFlag = true;
                        break;
                    }
                }
            }

            if (isFindFormFlag == false)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "RTT")
                    {
                        if (frm.Visible == true)
                        {
                            isFindFormFlag = true;
                            break;
                        }
                    }
                }
            }

            if (isFindFormFlag == false)
            {
                if (MessageBox.Show("종료하시겠습니까?", "종료하시겠습니까?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    exitApplication();
                }
            }
            else
            {
                this.Visible = false;//어플리케이션을 숨긴다. 
            }
        }

        //클립보드에 ocr 문장 저장
        private void setClipboard(string transText)
        {
            try
            {
                string replaceOcrText = transText.Replace(" ", "");
                replaceOcrText = transText.Replace("not thing", " ");
                if (replaceOcrText.CompareTo("") != 0)
                    Clipboard.SetText(replaceOcrText);               //인시로 둠
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                return;
            }
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
        private void saveBingKeyFile()
        {
            try
            {
                using (StreamWriter newTask = new StreamWriter(@"bingAccount.txt", false))
                {
                    newTask.WriteLine(bingAccountTextBox.Text);
                    newTask.Close();
                }


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"bingAccount.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(@"bingAccount.txt", false))
                    {
                        newTask.WriteLine(bingAccountTextBox.Text);
                        newTask.Close();
                    }
                }
            }

        }

        private void openBingKeyFile()
        {
            try
            {
                StreamReader r = new StreamReader(@"bingAccount.txt");
                string line = r.ReadLine();
                bingAccountKey = line;
                bingAccountTextBox.Text = line;
                r.Close();
                r.Dispose();
                
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"bingAccount.txt"))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }
        }

        private void SaveNaverKeyFile()
        {
            try
            {
                using (StreamWriter newTask = new StreamWriter(@"naverAccount.txt", false))
                {                    
                    newTask.WriteLine(NaverIDKeyTextBox.Text);
                    newTask.WriteLine(NaverSecretKeyTextBox.Text);
                    newTask.Close();
                }


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"naverAccount.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(@"naverAccount.txt", false))
                    {
                        newTask.WriteLine(bingAccountTextBox.Text);
                        newTask.Close();
                    }
                }
            }

        }

        private void OpenNaverKeyFile()
        {
            try
            {
                StreamReader r = new StreamReader(@"naverAccount.txt");
                string line = r.ReadLine();
                naverIDKey = line;
                NaverIDKeyTextBox.Text = line;
                line = r.ReadLine();
                naverSecretKey = line;
                NaverSecretKeyTextBox.Text = line;

                r.Close();
                r.Dispose();

            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"naverAccount.txt"))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }
        }


        #endregion


        //환경 설정 적용
        public void SetUIValueToSetting()
        {
            try
            {
                changeSkin();
                MySettingManager.NowIsShowOcrResultFlag = showOcrCheckBox.Checked;
                MySettingManager.NowIsSaveOcrReulstFlag = saveOCRCheckBox.Checked;
                IsUseClipBoardFlag = isClipBoardcheckBox1.Checked;

                transType = (SettingManager.TransType)TransType_Combobox.SelectedIndex;

                MySettingManager.IsUseStringUpper = checkStringUpper.Checked;
                MySettingManager.NowIsUseRGBFlag = checkRGB.Checked;
                MySettingManager.NowIsUseHSVFlag = checkHSV.Checked;
                MySettingManager.NowIsUseErodeFlag = checkErode.Checked;
                MySettingManager.NowIsUseNHocr = isUseNHocrBox.Checked;

                if (speedRadioButton1.Checked == true)
                {
                    ocrProcessSpeed = 1000;
                }
                else if (speedRadioButton2.Checked == true)
                {
                    ocrProcessSpeed = 1500;
                }
                else if (speedRadioButton3.Checked == true)
                {
                    ocrProcessSpeed = 2000;
                }
                else if (speedRadioButton4.Checked == true)
                {
                    ocrProcessSpeed = 2500;
                }
                else if (speedRadioButton5.Checked == true)
                {
                    ocrProcessSpeed = 3000;
                }

                MySettingManager.NowDBFile = dbFileTextBox.Text;
                MySettingManager.NowTessData = tessDataTextBox.Text;
                if (bingAccountTextBox.Text == "")          //빙 번역기 계정 키를 아무것도 입력 안 할경우
                {
                    bingAccountKey = "i2nV6GJf/7gPC7WTCq1VMlg6bN7OerxF857zqif7HSc=";
                }
                else
                {
                    bingAccountKey = bingAccountTextBox.Text;   //입력했을 땐 입력한 걸로
                }

                naverIDKey = NaverIDKeyTextBox.Text;
                naverSecretKey = NaverSecretKeyTextBox.Text;

                NaverTranslateAPI.instance.Init(naverIDKey, naverSecretKey);


                saveBingKeyFile();
                SaveNaverKeyFile();
                SaveHotKeyFile();

                SetCheckUpdate(checkUpdateCheckBox.Checked);
                SetCheckUseGoogleCount(allowGoogleCountCheckBox.Checked);
                
                //언어 설정.
                MySettingManager.NowIsUseEngFlag = false;
                MySettingManager.NowIsUseJpnFlag = false;
                MySettingManager.NowIsUseOtherLangFlag = false;
                if(languageComboBox.SelectedIndex == 0)
                {
                    //영어.
                    MySettingManager.NowIsUseEngFlag = true;

                }
                else if (languageComboBox.SelectedIndex == 1)
                {
                    //일본어
                    MySettingManager.NowIsUseJpnFlag = true;

                }
                else if (languageComboBox.SelectedIndex == 2)
                {
                    //기타
                    MySettingManager.NowIsUseOtherLangFlag = true;
                }


                //번역 코드 설정.
                string transCode = transCodeList[transCodeComboBox.SelectedIndex];
                string resultCode = resultCodeList[resultCodeComboBox.SelectedIndex];


                MySettingManager.TransCode = transCode;
                MySettingManager.ResultCode = resultCode;


                transCode = transCodeList[naverTransComboBox.SelectedIndex];
                resultCode = naverResultCodeList[0];
                MySettingManager.NaverTransCode = transCode;
                MySettingManager.NaverResultCode = resultCode;

                NaverTranslateAPI.instance.SetTransCode  (transCode, resultCode);

                    //폰트 관련
                textFont = new Font(textFont.FontFamily, (int)fontSizeUpDown.Value);
                MySettingManager.TextFont = textFont;
                MySettingManager.TextColor = textColor;
                MySettingManager.OutLineColor1 = outlineColor1;
                MySettingManager.OutLineColor2 = outlineColor2;
                MySettingManager.BackgroundColor = backgroundColor;

                MySettingManager.NowIsUseBackColor = this.useBackColorCheckBox.Checked;
                if (this.alignmentCenterCheckBox.Checked)
                    MySettingManager.NowSortType = SettingManager.SortType.Center;
                else
                    MySettingManager.NowSortType = SettingManager.SortType.Normal;
                MySettingManager.NowIsRemoveSpace = this.removeSpaceCheckBox.Checked;

                MySettingManager.NowIsActiveWindow = activeWinodeCheckBox.Checked;
               

                //번역창 최상단
                isTranslateFormTopMostFlag = topMostcheckBox.Checked;
                setTranslateTopMostToolStripMenuItem.Checked = topMostcheckBox.Checked;


                if (MySettingManager.NowSkin == SettingManager.Skin.dark)
                {
                    FormManager.Instace.MyBasicTransForm.setBingAccountKey(bingAccountKey);
                    FormManager.Instace.MyBasicTransForm.SetTransCode(transCode, resultCode);    
                }
                else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                {
                    FormManager.Instace.MyLayerTransForm.setBingAccountKey(bingAccountKey);
                    FormManager.Instace.MyLayerTransForm.SetTransCode(transCode, resultCode);
                    FormManager.Instace.MyLayerTransForm.UpdateTransform();
                }

                MySettingManager.ImgZoomSize = (float)imgZoomsizeUpDown.Value;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
            //색 리스트
            int[] valueRArray = new int[groupCombo.Items.Count];
            int[] valueGArray = new int[groupCombo.Items.Count];
            int[] valueBArray = new int[groupCombo.Items.Count];

            int[] valueS1Array = new int[groupCombo.Items.Count];
            int[] valueS2Array = new int[groupCombo.Items.Count];
            int[] valueV1Array = new int[groupCombo.Items.Count];
            int[] valueV2Array = new int[groupCombo.Items.Count];

            colorGroup[nowColorGroupIndex].setRGBValuse(Convert.ToInt32(rTextBox.Text), Convert.ToInt32(gTextBox.Text), Convert.ToInt32(bTextBox.Text));
            colorGroup[nowColorGroupIndex].setHSVValuse(Convert.ToInt32(s1TextBox.Text), Convert.ToInt32(s2TextBox.Text), Convert.ToInt32(v1TextBox.Text), Convert.ToInt32(v2TextBox.Text));
            MySettingManager.NowIsUseDicFileFlag = checkDic.Checked;
            MySettingManager.NowDicFile = dicFileTextBox.Text;
            for (int i = 0; i < colorGroup.Count; i++)
            {
                colorGroup[i].checkHSVRange();
                valueRArray[i] = colorGroup[i].getValueR();
                valueGArray[i] = colorGroup[i].getValueG();
                valueBArray[i] = colorGroup[i].getValueB();

                valueS1Array[i] = colorGroup[i].getValueS1();
                valueS2Array[i] = colorGroup[i].getValueS2();
                valueV1Array[i] = colorGroup[i].getValueV1();
                valueV2Array[i] = colorGroup[i].getValueV2();
            }


            groupLabel.Text = (groupCombo.Items.Count - 2).ToString();  //색 그룹 개수 표시

            //ColorGroupForm testForm = new ColorGroupForm();
            //testForm.Show ();
            //testForm.ShowGrupForm();
            try
            {
                SetIsUseNHocr(MySettingManager.NowIsUseNHocr);
                if(!MySettingManager.NowIsUseNHocr)
                {
                    bool isUseUnicode = false;
                    if (MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseUnicode = true;
                    }

                    if (MySettingManager.NowIsUseOtherLangFlag)
                    {
                        isUseUnicode = true;
                    }

                    setTessdata(MySettingManager.NowTessData, isUseUnicode);
                }
                
                setFiducialValue(valueRArray, valueGArray, valueBArray, valueS1Array, valueS2Array, valueV1Array, valueV2Array, groupCombo.Items.Count - 2);
                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.NowDicFile);
                bool isUseDBFlag = false;
                if(transType == SettingManager.TransType.db)
                {
                    isUseDBFlag = true;
                }
                SetIsStringUpper(MySettingManager.IsUseStringUpper);
                setUseDB(isUseDBFlag, MySettingManager.NowDBFile);
                setAdvencedImgOption(MySettingManager.NowIsUseRGBFlag, MySettingManager.NowIsUseHSVFlag, MySettingManager.NowIsUseErodeFlag, MySettingManager.ImgZoomSize);
                setCaptureArea();
                SetIsActiveWindow(MySettingManager.NowIsActiveWindow);
               
                
                //setCutPoint(locationXList.ToArray(), locationYList.ToArray(), sizeXList.ToArray(), sizeYList.ToArray(), locationXList.Count);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }            
        }


        public void ProcessTrans()              //번역 시작 쓰레드
        {
         int lastTick =0 ;

         try
         {
             while (isEndFlag == false)
             {

                 if (System.Environment.TickCount - lastTick >= ocrProcessSpeed)
                 {

                     lastTick = System.Environment.TickCount;

                     if (FormManager.Instace.MyBasicTransForm != null || FormManager.Instace.MyLayerTransForm != null)
                     {
                         StringBuilder sb = new StringBuilder(8192);
                         StringBuilder sb2 = new StringBuilder(8192);
                         processOcr(sb, sb2);
                         nowOcrString = sb.ToString();       //ocr 결과
                         string argv3 = sb2.ToString();      //번역 결과.
                         sb.Clear();
                         sb2.Clear();

                         if (MySettingManager.NowIsRemoveSpace == true)
                         {
                            nowOcrString = nowOcrString.Replace(" ", "");
                         }

                         if (IsUseClipBoardFlag == true)
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

            if(FormManager.Instace.quickOcrAreaForm != null)
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

        //델리게이트 이용
        public void setSpellCheck()
        {
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.NowDicFile);
                thread = new Thread(new ThreadStart(ProcessTrans));
                thread.Start();
            }
            else
            {
                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.NowDicFile);
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

        public void StartTrnas()
        {

            if (FormManager.Instace.MySearchOptionForm != null)
            {
                FormManager.Instace.MySearchOptionForm.acceptCaptureArea();
            }

            isProcessTransFlag = true;
            if (thread == null)
            {
                isEndFlag = false;
                thread = new Thread(new ThreadStart(ProcessTrans));
                thread.Start();
            }

            makeTransForm();
        }
        public void StopTrans()
        {
            isProcessTransFlag = false;
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
                    FormManager.Instace.MyLayerTransForm.setVisibleBackground();
                    FormManager.Instace.MyLayerTransForm.disableOverHitLayer();
                }
            }
            else
            {
                if(FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.StopTrans();
                }
            }
        }

        private void startTransLateButton_Click(object sender, EventArgs e)
        {
            StartTrnas();
        }

        //ocr 영역 적용

        
        public void setCaptureArea()   
        {
            int BorderWidth = SystemInformation.FrameBorderSize.Width;
            int TitlebarHeight = SystemInformation.CaptionHeight + BorderWidth;
            locationXList = new List<int>();
            locationYList = new List<int>();
            sizeXList = new List<int>();
            sizeYList = new List<int>();

            List<int> tempXList = new List<int>();
            List<int> tempYList = new List<int>();
            List<int> tempSizeXList = new List<int>();
            List<int> tempSizeYList = new List<int>();

            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++ )
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = foundedForm.Size.Width - BorderWidth * 2;
                int sizeY = foundedForm.Size.Height - TitlebarHeight - BorderWidth;

                locationXList.Add(locationX);
                locationYList.Add(locationY);
                sizeXList.Add(sizeX);
                sizeYList.Add(sizeY);

                tempXList.Add(locationX);
                tempYList.Add(locationY);
                tempSizeXList.Add(sizeX);
                tempSizeYList.Add(sizeY);
            }

            MySettingManager.NowOCRGroupcount = locationYList.Count;
            MySettingManager.NowLocationXList = locationXList;
            MySettingManager.NowLocationYList = locationYList;
            MySettingManager.NowSizeXList = sizeXList;
            MySettingManager.NowSizeYList = sizeYList;

            //퀵 사이즈 전용.
            int quickX = 0;
            int quickY = 0;
            int quickSizeX = 0;
            int quickSizeY = 0;


            if(FormManager.Instace.quickOcrAreaForm != null)
            {
                quickX = FormManager.Instace.quickOcrAreaForm.Location.X + BorderWidth;
                quickY = FormManager.Instace.quickOcrAreaForm.Location.Y + TitlebarHeight;
                quickSizeX = FormManager.Instace.quickOcrAreaForm.Size.Width - BorderWidth * 2;
                quickSizeY = FormManager.Instace.quickOcrAreaForm.Size.Height - TitlebarHeight - BorderWidth;

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
                SetUseColorGroup();
                thread = new Thread(new ThreadStart(ProcessTrans));
                thread.Start();
            }
            else
            {
                setCutPoint(tempXList.ToArray(), tempYList.ToArray(), tempSizeXList.ToArray(), tempSizeYList.ToArray(), tempXList.Count);
                SetUseColorGroup();
            }          
             
        }


        public void clickCaptureAreaButton()            //영역 검색 버튼 클릭
        {
            int searchAreaQuantity = 0;

            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++ )
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];
                searchAreaQuantity++;
                foundedForm.Opacity = 1.0f;
            }
           
            if( FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.Opacity = 1.0f;
            }

            makeSearchOptionForm();
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


        private void isUseNHocrBox_CheckedChanged(object sender, MouseEventArgs e)
        {
            
            CheckBox thisBox = (CheckBox)sender;
            if (thisBox.Checked == true)
            {         
                
                if(languageComboBox.SelectedIndex != 1)
                {
                    languageComboBox.SelectedIndex = 1;
                    transCodeComboBox.SelectedIndex = 1;
                    tessDataTextBox.Text = "jpn";

                    naverTransComboBox.SelectedIndex = 1;
                }
            }
             
        }

        private void languageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (languageComboBox.SelectedIndex == 0)
            {
                tessDataTextBox.Text = "eng";
                isUseNHocrBox.Checked = false;
                transCodeComboBox.SelectedIndex = 0;
                naverTransComboBox.SelectedIndex = 0;
            }
            else if (languageComboBox.SelectedIndex == 1)
            {
                tessDataTextBox.Text = "jpn";
                transCodeComboBox.SelectedIndex = 1;
                naverTransComboBox.SelectedIndex = 1;
            }
            else if (languageComboBox.SelectedIndex == 2)
            {
                isUseNHocrBox.Checked = false;
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

                    for(int i = 0; i < MySettingManager.UseColorGroup.Count; i++)
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
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                SetUIValueToSetting();
                thread = new Thread(new ThreadStart(ProcessTrans));
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
            saveSetting(@".\\setting\\setting.conf");
            MessageBox.Show("적용 완료");

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
            makeTransForm();
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
                StartTrnas();
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

            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "TransForm")
                    {
                        TransForm foundedForm = (TransForm)frm;
                        foundedForm.setTopMostFlag(isTranslateFormTopMostFlag);
                        return;
                    }
                }
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "TransFormLayer")
                    {
                        TransFormLayer foundedForm = (TransFormLayer)frm;
                        foundedForm.setTopMostFlag(isTranslateFormTopMostFlag);
                        return;
                    }
                }
            }
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
            makeTransForm();
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

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=83XY786Q9BEA4");
                }
                catch { }
            }
        }

        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {                
                System.Diagnostics.Process.Start("http://killkimno.blog.me/70179867557");
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

            Pen myPen = new Pen(Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70))))), 3);
            e.Graphics.DrawRectangle(myPen,
            myPanel.ClientRectangle.Left,
            myPanel.ClientRectangle.Top,
            myPanel.ClientRectangle.Width - 1,
            myPanel.ClientRectangle.Height - 1);
            base.OnPaint(e);
        }

        private void acceptButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.acceptButton.Image = global::MORT.Properties.Resources.accept_button_click;
            acceptLabel.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
        }

        private void acceptButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.acceptButton.Image = global::MORT.Properties.Resources.accept_button;
            acceptLabel.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(151)))), ((int)(((byte)(255)))));
        }

        private void skinDarkRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            showOcrCheckBox.Checked = true;
        }

        private void skinLayerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            showOcrCheckBox.Checked = false;
        }

        private void settingSaveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                SetUIValueToSetting();
                thread = new Thread(new ThreadStart(ProcessTrans));
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
                saveSetting(savePanel.FileName);
            }
        }

        private void settingLoadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.RestoreDirectory = false;
            openPanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            openPanel.Filter = "Config File (*.conf)|*.conf";


            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;

                if (openPanel.ShowDialog() == DialogResult.OK)
                {
                    openSettingfile(openPanel.FileName);
                }

                SetUIValueToSetting();
                thread = new Thread(new ThreadStart(ProcessTrans));
                thread.Start();
            }
            else
            {
                if (openPanel.ShowDialog() == DialogResult.OK)
                {
                    openSettingfile(openPanel.FileName);
                }

                SetUIValueToSetting();
            }
            saveSetting(@".\\setting\\setting.conf");
        }

        private void settingDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isTranslateFormTopMostFlag = true;


            if(FormManager.Instace.quickOcrAreaForm != null)
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
                thread = new Thread(new ThreadStart(ProcessTrans));
                thread.Start();
            }
            else
            {
                MySettingManager.SetDefault();
                SetValueToUIValue();
                SetUIValueToSetting();
            }

            if(MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                if(FormManager.Instace.MyLayerTransForm != null)
                {
                    FormManager.Instace.MyLayerTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                }
            }
            else if(MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                }
            }
            saveSetting(@".\\setting\\setting.conf");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F)
            {
                if (thread == null)
                {
                    StartTrnas();
                }
                else if (thread != null && thread.IsAlive == true)
                {
                    StopTrans();
                }
            }
        }

        
        private void checkDic_CheckedChanged(object sender, EventArgs e)
        {
            setCheckSpellingToolStripMenuItem.Checked = checkDic.Checked;
        }

        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Process.Start(" https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=83XY786Q9BEA4");

                System.Diagnostics.Process.Start("https://goo.gl/#analytics/goo.gl/1J12p8/month");
            }
            catch { }
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
            if(tabControl1.SelectedIndex == 1)
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
                //System.Diagnostics.Process.Start(" https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=83XY786Q9BEA4");

                System.Diagnostics.Process.Start("http://killkimno.blog.me/220342229480");
            }
            catch { }
        }

        private void error_Information_Button_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Process.Start(" https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=83XY786Q9BEA4");

                System.Diagnostics.Process.Start("http://killkimno.blog.me/70185869419");
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



        private void TransType_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB_Panel.Visible = false;
            Bing_Panel.Visible = false;
            Naver_Panel.Visible = false;

            if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.db)
            {
                DB_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.bing)
            {
                Bing_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.naver)
            {
                Naver_Panel.Visible = true;
            }
        }

        private void InitTansKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.Z);
            transKeyInputLabel.ResetInput(list);
        }

        private void InitDicKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.S);
            this.dicKeyInputLabel.ResetInput(list);
        }

        private void InitQuickKey()
        {
            List<Keys> list = new List<Keys>();

            list.Add(Keys.ControlKey);
            list.Add(Keys.ShiftKey);
            list.Add(Keys.X);
            this.quickKeyInputLabel.ResetInput(list);
        }

        private void transKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitTansKey();
        }

        private void dicKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitDicKey();
        }

        private void quickKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitQuickKey();
        }

      
    }

}



