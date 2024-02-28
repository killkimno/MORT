using MORT.GoogleOcrSetting;
using MORT.Manager;
using MORT.OcrApi.EasyOcr;
using MORT.ScreenCapture;
using MORT.Service.PythonService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MORT
{
    public class FormManager
    {
        #region ::::::::: 텍스트 ::::::::::

        public static string CUSTOM_LABEL_TEXT{get; private set; } = "설정 결과를 미리 봅니다.\n폰트와 색은 어두운 번역창에 적용되지 않습니다.\n\n1 2 3 4 5 6\nTank division!";
        public static string CUSTOM_LABEL_TEXT2 { get; private set; } = "\n\n{0}다중 OCR 사용시 영역1 문장\n{1}그리고 영역2 문장";

        public static void InitCustomLabelString(string first, string second)
        {
            first = first.Replace("{}", "");
            second = second.Replace("{}", "");
            CUSTOM_LABEL_TEXT = first;
            CUSTOM_LABEL_TEXT2 = $"\n\n{second}";
        }
        #endregion


        private static FormManager instance;
        public static FormManager Instace
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormManager();
                }
                return instance;
            }
        }


        #region :::::::::: 관련 데이터 ::::::::::

        public static int BorderWidth;
        public static int BorderHeight;
        public static int TitlebarHeight;

        #endregion 


        #region :::::::::::관리 폼::::::::::

        public Form1 MyMainForm;
        public TransForm MyBasicTransForm;
        public TransFormLayer MyLayerTransForm;
        //TODO : TEMP
        public TransFormOver MyOverTransForm;
        public RTT MyRemoteController;
        public SearchOptionForm MySearchOptionForm;
        public List<OcrAreaForm> OcrAreaFormList = new List<OcrAreaForm>();
        public List<OcrAreaForm> exceptionAreaFormList = new List<OcrAreaForm>();
        public OcrAreaForm quickOcrAreaForm;
        public OcrAreaForm snapOcrAreaForm;
        public DicEditorForm MyDicEditorForm;
        public ColorGroupForm colorGroupForm;
        public NaverKeyListUI naverKeyListUI;

        public SettingBrowser.SettingBrowserUI settingBrowserUI;
        public UIQuickSetting uiQuickSetting;

        public DonatePage donatePage;
        private EasyOcrInstaller.EasyOcrInstaller _easyOcrInstaller;
        public UIAdvencedOption uiAdvencedOption;
        public UIGoogleOcrSetting UIGoogleOcrSetting;

        public PopupScreenCapture screenCaptureUI;

        #endregion

        public enum TransFormState { None, Basic, Layer, Over };

        public static bool GetIsRemain()
        {
            bool isRemain = true;
            if (instance == null)
            {
                isRemain = false;
            }

            return isRemain;
        }

     
        public void CloseApplication()
        {

        }

        public void MakeSearchOptionForm()
        {
            Point location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 320);
            if (MyRemoteController != null)
            {
                location = MyRemoteController.Location;
            }

            if (MySearchOptionForm == null)
            {
                MySearchOptionForm = new SearchOptionForm();
                MySearchOptionForm.Name = "SearchOptionForm";
                MySearchOptionForm.StartPosition = FormStartPosition.Manual;
                MySearchOptionForm.Location = location;
                MyRemoteController.InstanceRef = MyMainForm;
                MySearchOptionForm.Show();
            }
            else
            {
                MySearchOptionForm.Location = location;
                MySearchOptionForm.Activate();
                MySearchOptionForm.Show();
            }

            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];
                foundedForm.Activate();
            }
            foreach(var obj in FormManager.instance.exceptionAreaFormList)
            {
                obj.Activate();
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.Activate();
            }

        }

        public void DestorySearchOptionForm()
        {
            MySearchOptionForm = null;
        }

        #region :::::::::::::::::::::::::::::: 네이버 키 관련 ::::::::::::::::::::::::::::::

        public void ShowNaverKeyListUI(Action callback)
        {
            if (naverKeyListUI == null)
            {
                naverKeyListUI = new NaverKeyListUI();
                naverKeyListUI.StartPosition = FormStartPosition.CenterScreen;

            }

            naverKeyListUI.Activate();
            naverKeyListUI.Init(callback);
            naverKeyListUI.Show();
        }

        public void DestoryNaverKeyListUI()
        {
            naverKeyListUI = null;
        }

        #endregion


        #region :::::::::::::::::::::::::::::: 네이버 키 관련 ::::::::::::::::::::::::::::::
        public void ShowSettingBrowserUI()
        {
            if (settingBrowserUI == null)
            {
                settingBrowserUI = new SettingBrowser.SettingBrowserUI();
                settingBrowserUI.StartPosition = FormStartPosition.Manual;

            }

            settingBrowserUI.Activate();
            settingBrowserUI.Init();
            settingBrowserUI.Show();
        }

        public void DestorySettingBrowserUI()
        {
            settingBrowserUI = null;
        }



        #endregion

        #region :::::::::::::::::::::::::::::: 스크린 캡쳐 UI ::::::::::::::::::::::::::::::

        public void ShowScreenCapture()
        {
            if (screenCaptureUI == null || screenCaptureUI.isClosed)
            {
                screenCaptureUI = new PopupScreenCapture();

                //test
                Action callback = delegate
                {
                    MyMainForm.MySettingManager.isUseAttachedCapture = true;
                    Console.WriteLine("뭔가가 선택 됨");
                };

                Action stopCallback = delegate
                {
                    if (this != null && MyMainForm != null && MyMainForm.MySettingManager != null)
                    {
                        MyMainForm.MySettingManager.isUseAttachedCapture = false;
                    }
                };


                Action closeCallback = delegate
                {
                    if(this != null && MyMainForm != null && MyMainForm.MySettingManager != null)
                    {
                        MyMainForm.MySettingManager.isUseAttachedCapture = false;
                        screenCaptureUI = null;
                    }
                   
                };

                bool useEnglish = LocalizeManager.LocalizeManager.Language == LocalizeManager.AppLanguage.English;


                screenCaptureUI.Show();
                screenCaptureUI.Start(callback, closeCallback, stopCallback, useEnglish, AdvencedOptionManager.EnableYellowBorder);
            }
            else
            {
                screenCaptureUI.Activate();
            }      
        }




        #endregion


        #region :::::::::::::::::::::::::::::: 빠른 설정 ::::::::::::::::::::::::::::::

        public void ShowQuickSetting(OcrLanguageType languageType)
        {

            if (uiQuickSetting == null)
            {
                uiQuickSetting = new UIQuickSetting();
                uiQuickSetting.StartPosition = FormStartPosition.CenterScreen;

            }

            uiQuickSetting.Activate();
            uiQuickSetting.Show(languageType);
        }

        public void DestoryQuickSetting()
        {
            uiQuickSetting = null;
        }

        #endregion



        #region :::::::::::::::::::::::::::::: 빠른 설정 ::::::::::::::::::::::::::::::

        public void ShowAdvencedOption()
        {
            Form1.IsLockHotKey = true;
            if (uiAdvencedOption == null)
            {
                uiAdvencedOption = new UIAdvencedOption();
                uiAdvencedOption.StartPosition = FormStartPosition.CenterScreen;

            }

            uiAdvencedOption.Activate();
            uiAdvencedOption.Show();
        }

        public void DestoryAdvencedOption()
        {
            Form1.IsLockHotKey = false;
            uiAdvencedOption = null;
        }

        #endregion

        #region :::::::::::::::::::::::::::::: 구글 OCR 설정 ::::::::::::::::::::::::::::::

        public void ShowGoogleOcrSetting()
        {
            if (UIGoogleOcrSetting == null)
            {
                UIGoogleOcrSetting = new UIGoogleOcrSetting();
                UIGoogleOcrSetting.StartPosition = FormStartPosition.CenterScreen;
                UIGoogleOcrSetting.FormClosed += (object sender, FormClosedEventArgs e) => UIGoogleOcrSetting = null;

            }

            UIGoogleOcrSetting.Activate();
            UIGoogleOcrSetting.Show();
        }

        #endregion


        #region :::::::::::::::::::::::::::::: 후원하기 페이지 ::::::::::::::::::::::::::::::

        public void ShowDonatePage()
        {
            if (donatePage == null)
            {
                donatePage = new DonatePage();
                donatePage.StartPosition = FormStartPosition.CenterScreen;

            }

            donatePage.Activate();
            donatePage.Show();
        }

        public void DestoryDonatePage()
        {
            donatePage = null;
        }


        #endregion

        #region :::::::::: Easy OCR 관련 ::::::::::

        public void ShowEasyOcrInstaller(OcrManager ocrManager, PythonModouleService pythonModouleService)
        {
            if(_easyOcrInstaller == null)
            {
                _easyOcrInstaller = new EasyOcrInstaller.EasyOcrInstaller();
                _easyOcrInstaller.StartPosition = FormStartPosition.CenterScreen;
            }
            _easyOcrInstaller.Show(ocrManager, pythonModouleService, () => { _easyOcrInstaller = null; });
        }


        #endregion

        #region :::::::::::::::::::::::::::::: 교정 사전 ::::::::::::::::::::::::::::::

        public void MakeDicEditorForm(string nowOcrString, bool isUseJpnFlag, string dicFileText)
        {
            if (MyDicEditorForm == null)
            {
                DicEditorForm form = new DicEditorForm(nowOcrString, isUseJpnFlag, dicFileText);
                MyDicEditorForm = form;

                form.Name = "DicEditorForm";
                form.StartPosition = FormStartPosition.Manual;

                Form1.PDelegateSetSpellCheck PDsetSpellCheck = new Form1.PDelegateSetSpellCheck(MyMainForm.ApplySpellCheck);
                form.SetSpellCheckFunction(PDsetSpellCheck);

            }
            MyDicEditorForm.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            MyDicEditorForm.Activate();

            MyDicEditorForm.Show();
        }

        public void DestoryDicEditorForm()
        {
            MyDicEditorForm = null;
        }

        #endregion


        #region :::::::::::::::::::::::::::::: 색 그룹 관련 ::::::::::::::::::::::::::::::

        public void ShowColorGroupForm(OcrAreaForm ocrAreaForm)
        {

            if (colorGroupForm == null || colorGroupForm.IsDisposed)
            {
                ColorGroupForm form = new ColorGroupForm();
                colorGroupForm = form;

            }

            colorGroupForm.Show();
            colorGroupForm.ShowGroupForm(ocrAreaForm);
        }

        public void InitUseColorGroup()
        {
            List<List<int>> useColorGroup = new List<List<int>>();

            //컬러 그룹수 초기화.
            if (useColorGroup.Count < MyMainForm.MySettingManager.NowOCRGroupcount)
            {
                for (int i = useColorGroup.Count; i < MyMainForm.MySettingManager.NowOCRGroupcount; i++)
                {
                    useColorGroup.Add(new List<int>());
                }
            }

            List<ColorGroup> colorGroup = MyMainForm.MySettingManager.NowColorGroup;

            for (int i = 0; i < MyMainForm.MySettingManager.NowOCRGroupcount; i++)
            {
                useColorGroup[i] = new List<int>();
                for (int j = 0; j < colorGroup.Count; j++)
                {
                    useColorGroup[i].Add(1);
                }
            }

            for (int i = 0; i < MyMainForm.MySettingManager.NowOCRGroupcount && i < MyMainForm.MySettingManager.UseColorGroup.Count; i++)
            {
                for (int j = 0; j < colorGroup.Count && j < MyMainForm.MySettingManager.UseColorGroup[i].Count; j++)
                {
                    useColorGroup[i][j] = MyMainForm.MySettingManager.UseColorGroup[i][j];
                }
            }

            //퀵 컬러그룹
            MyMainForm.MySettingManager.QuickOcrUsecolorGroup.Clear();
            ResetUseColorGroup();
            for (int i = 0; i < colorGroup.Count; i++)
            {
                MyMainForm.MySettingManager.QuickOcrUsecolorGroup.Add(1);
            }




            MyMainForm.MySettingManager.UseColorGroup = useColorGroup;


        }

        public void ResetUseColorGroup()
        {
            for (int i = 0; i < MyMainForm.MySettingManager.UseColorGroup.Count; i++)
            {
                MyMainForm.MySettingManager.UseColorGroup[i].Clear();
            }
            MyMainForm.MySettingManager.UseColorGroup.Clear();

            MyMainForm.MySettingManager.QuickOcrUsecolorGroup.Clear();
        }

        #endregion

        public void ShowColorPickResult(bool isUseRgb, bool isUseHSv, bool isUseThreshold, ColorGroup colorGroup, int threshold)
        {
            OcrAreaForm target = null;
            if (OcrAreaFormList.Count > 0)
            {
                target = OcrAreaFormList[0];
            }
            else if (quickOcrAreaForm != null)
            {
                target = quickOcrAreaForm;
            }
            
            if(target == null)
            {
                FormManager.ShowPopupMessage("", LocalizeManager.LocalizeManager.GetLocalizeString("ShowColorPickResultNullTarget"));
            }
            else
            {
                target.ShowColorPicker(true, isUseRgb, isUseHSv, isUseThreshold, colorGroup, threshold);
            }



        }

        #region :::::::::::::::::::::::::::::: OCR 영역창 관련  ::::::::::::::::::::::::::::::



        public bool GetIsShowOcrAreaFlag()
        {
            bool isShowOcrAreaFlag = false;

            if (FormManager.Instace.MySearchOptionForm != null)
            {
                isShowOcrAreaFlag = true;
            }


            return isShowOcrAreaFlag;
        }

        //퀵 영역 만들기.
        public void MakeQuickCaptureAreaForm()
        {
            screenForm.MakeScreenForm(screenForm.ScreenType.Quick);
        }

        //스냅 샷 영역 만들기.
        public void MakeSnapShotAreaForm(Action callback = null)
        {
            screenForm.MakeScreenForm(screenForm.ScreenType.Snap, callback);
        }

        public void MakeCpatureAreaForm(Action callback = null)
        {
            screenForm.MakeScreenForm(screenForm.ScreenType.Normal, callback);
        }

        public void MakeExceptionAreaForm()
        {
            screenForm.MakeScreenForm(screenForm.ScreenType.Exception);
        }

        public void ResetCaputreAreaForm()
        {
            DestoryAllOcrAreaForm(false);
            InitUseColorGroup();

            bool isShowOcrRare = GetIsShowOcrAreaFlag();
            for (int i = 0; i < MyMainForm.MySettingManager.NowOCRGroupcount; i++)
            {
                screenForm.MakeAreaForm( screenForm.ScreenType.Normal, MyMainForm.MySettingManager.NowLocationXList[i], MyMainForm.MySettingManager.NowLocationYList[i], MyMainForm.MySettingManager.NowSizeXList[i], MyMainForm.MySettingManager.NowSizeYList[i], isShowOcrRare);

            }
            for (int i = 0; i < MyMainForm.MySettingManager.nowExceptionGroupCount; i++)
            {
                screenForm.MakeAreaForm(screenForm.ScreenType.Exception, 
                    MyMainForm.MySettingManager.nowExceptionLocationXList[i], MyMainForm.MySettingManager.nowExceptionLocationYList[i],
                    MyMainForm.MySettingManager.nowExceptionSizeXList[i], MyMainForm.MySettingManager.nowExceptionSizeYList[i], isShowOcrRare);

            }
        }

        public void RefreshOCRAreaForm()
        {
            if(OcrAreaFormList != null)
            {
                for(int i = 0; i < OcrAreaFormList.Count; i++)
                {
                    OcrAreaFormList[i].Refresh();
                }
            }

            if (exceptionAreaFormList != null)
            {
                for (int i = 0; i < exceptionAreaFormList.Count; i++)
                {
                    exceptionAreaFormList[i].Refresh();
                }
            }
        }



        public void AddOcrAreaForm(OcrAreaForm newForm)
        {
            OcrAreaFormList.Add(newForm);

            List<List<int>> useColorGroup = MyMainForm.MySettingManager.UseColorGroup;
            List<ColorGroup> colorGroup = MyMainForm.MySettingManager.NowColorGroup;
            if (useColorGroup.Count < OcrAreaFormList.Count)
            {

                for (int i = useColorGroup.Count; i < OcrAreaFormList.Count; i++)
                {
                    useColorGroup.Add(new List<int>());

                    for (int j = 0; j < colorGroup.Count; j++)
                    {
                        useColorGroup[i].Add(1);
                    }
                }
            }
        }
         
        public void AddExceptionAreaForm(OcrAreaForm newForm)
        {
            exceptionAreaFormList.Add(newForm);
        }

        public void MakeQuickOcrAreaForm(OcrAreaForm newForm)
        {
            quickOcrAreaForm = newForm;
        }

        public void MakeSnapShotOcrAreaForm(OcrAreaForm newForm)
        {
            snapOcrAreaForm = newForm;
        }


        public void DestoryAllOcrAreaForm(bool isRemoveQuick)
        {
            List<OcrAreaForm> backup = new List<OcrAreaForm>();

            for (int i = 0; i < OcrAreaFormList.Count; i++)
            {
                backup.Add(OcrAreaFormList[i]);
            }

            OcrAreaFormList.Clear();

            for (int i = 0; i < backup.Count; i++)
            {
                backup[i].Close();
            }
            backup.Clear();

            if(snapOcrAreaForm != null)
            {
                snapOcrAreaForm.Close();
            }

            if(quickOcrAreaForm != null && isRemoveQuick)
            {
                quickOcrAreaForm.Close();
            }

            for (int i = 0; i < exceptionAreaFormList.Count; i++)
            {
                backup.Add(exceptionAreaFormList[i]);
            }

            exceptionAreaFormList.Clear();

            for (int i = 0; i < backup.Count; i++)
            {
                backup[i].Close();
            }
            backup.Clear();

            OcrAreaForm.ocrAreaIndex = 0;
            OcrAreaForm.exceptionAreaIndex = 0;


        }

        public void DestoryOcrAreaForm(int index)
        {
            if (OcrAreaFormList.Count > 0)
            {
                List<List<int>> useColorGroup = MyMainForm.MySettingManager.UseColorGroup;
                List<ColorGroup> colorGroup = MyMainForm.MySettingManager.NowColorGroup;

                //부족한 수 만큼 다시 만듬.
                if (useColorGroup.Count < OcrAreaFormList.Count)
                {

                    for (int i = useColorGroup.Count; i < OcrAreaFormList.Count; i++)
                    {
                        useColorGroup.Add(new List<int>());

                        for (int j = 0; j < colorGroup.Count; j++)
                        {
                            useColorGroup[i].Add(1);
                        }
                    }
                }

                OcrAreaFormList.RemoveAt(index - 1);

                foreach (var pair in OcrAreaFormList)
                {
                    if (pair.Index > index)
                    {
                        pair.reSetTitleLabel(index);
                    }
                }

                useColorGroup.RemoveAt(index - 1);
            }
        }

        public void DestoryExceptionArea(int index)
        {
            if(index >0 && index <= exceptionAreaFormList.Count)
            {
                exceptionAreaFormList.RemoveAt(index - 1);
            }
        
        }

        public int GetOcrAreaCount()
        {
            int count = 0;
            if (quickOcrAreaForm != null)
            {
                count++;
            }

            if (snapOcrAreaForm != null)
            {
                return 1;
            }

            count += OcrAreaFormList.Count;
            return count;
        }

        public void SetInvisibleOcrArea()
        {
            foreach (var pair in OcrAreaFormList)
            {
                pair.SetVisible(false);
            }

            if (quickOcrAreaForm != null)
            {
                quickOcrAreaForm.SetVisible(false);
            }

            foreach (var pair in exceptionAreaFormList)
            {
                pair.SetVisible(false);
            }
        }

        public void SetTopMostOcrArea(bool isTopMost)
        {
            foreach (var pair in OcrAreaFormList)
            {
                pair.TopMost = isTopMost;
            }

            if (quickOcrAreaForm != null)
            {
                quickOcrAreaForm.TopMost = isTopMost;
            }

            foreach (var pair in exceptionAreaFormList)
            {
                pair.TopMost = isTopMost;
            }
        }

        public OcrAreaForm GetOCRArea(int index)
        {
            OcrAreaForm area = null;
            for (int i = 0; i < OcrAreaFormList.Count; i++)
            {
                if (OcrAreaFormList[i].Index == index)
                {
                    area = OcrAreaFormList[i];
                    break;
                }
            }
            return area;
        }

        #endregion

        public void MakeRTT()
        {
            if (MyRemoteController == null)
            {
                MyRemoteController = new RTT();
                MyRemoteController.Name = "RTT";
                MyRemoteController.StartPosition = FormStartPosition.Manual;
                MyRemoteController.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 420);
                MyRemoteController.InstanceRef = MyMainForm;
                MyRemoteController.ToggleStartButton(false);
                MyRemoteController.Show();
            }
            else
            {
                MyRemoteController.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 420);
                MyRemoteController.Activate();
                MyRemoteController.Show();
            }

         

        }


        #region :::::::::::::::::::::::::::::: 번역창 관련 ::::::::::::::::::::::::::::::

        public ITransform GetITransform()
        {
            ITransform transform = null;

            if (MyBasicTransForm != null)
            {
                transform = MyBasicTransForm;
            }

            if (MyLayerTransForm != null)
            {
                transform = MyLayerTransForm;
            }

            if(MyOverTransForm != null)
            {
                transform = MyOverTransForm;
            }

            return transform;
        }

        public void ApplyWarningMessage(string message)
        {
            ITransform transform = GetITransform();

            if (transform != null)
            {
                transform.ApplyWarningMessage(message, DateTime.Now.AddSeconds(10));
            }
        }

        public void ClearWarningMessage()
        {
            ITransform transform = GetITransform();

            if (transform != null)
            {
                transform.ClearWarningMessage();
            }
        }

        public void SetForceTransparency(bool isTranslating)
        {
            ITransform transform = GetITransform();

            if(transform != null)
            {
                transform.ForceTransparency();
                transform.DoUpdate(isTranslating);
            }
        }

        public bool HideTransFrom()
        {
            bool result = false;
            Form form = null;

            var transform = GetITransform();
            if (transform != null)
            {
                form = (Form)transform;
            }

            if (form != null)
            {
                if (form.Visible)
                {
                    form.Hide();
                }
                else
                {
                    form.Show();
                }

                result = form.Visible;
            }

            return result;
        }

        public void SetTopMostTransform(bool isTopMost)
        {

            var transform = GetITransform();

            if(transform  != null)
            {
                transform.SetTopMost(isTopMost, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
            }
        }


        /// <summary>
        /// 임시로 top most를 설정한다
        /// </summary>
        /// <param name="isTopMost"></param>
        public void SetTemporaryDisableTopMostTransform()
        {
            Form form = null;
            if (MyBasicTransForm != null)
            {
                form = MyBasicTransForm;
            }

            if (MyLayerTransForm != null)
            {
                form = MyLayerTransForm;
            }

            if (form != null)
            {
                form.TopMost = false;
            }
        }

        public void ResetTemporaryDisableTopMostTransform()
        {           
            var transform = GetITransform();

            if(transform != null)
            {
                transform.ApplyTopMost();
            }
        }


        public void MakeBasicTransForm(bool isTranslateFormTopMostFlag)
        {
            if (MyBasicTransForm == null)
            {
                MyBasicTransForm = new TransForm();
                MyBasicTransForm.Name = "TransForm";
                MyBasicTransForm.StartPosition = FormStartPosition.Manual;
                MyBasicTransForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                MyBasicTransForm.SetTopMost(isTranslateFormTopMostFlag, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                MyBasicTransForm.Show();

                MyMainForm.ApplyBasicFont();
            }
            else
            {
                MyBasicTransForm.SetTopMost(isTranslateFormTopMostFlag, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                MyBasicTransForm.Activate();
                MyBasicTransForm.Show();
            }

            MyBasicTransForm.ApplyRTL(AdvencedOptionManager.EnableRTL);

        }


        public void MakeLayerTransForm(bool isTranslateFormTopMostFlag, bool isProcessTransFlag)
        {
            
            if (MyLayerTransForm == null)
            {
                MyLayerTransForm = new TransFormLayer();

                MyLayerTransForm.Name = "TransFormLayer";
                MyLayerTransForm.StartPosition = FormStartPosition.Manual;
                MyLayerTransForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                MyLayerTransForm.SetTopMost(isTranslateFormTopMostFlag, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                MyLayerTransForm.Show();
                MyLayerTransForm.UpdateTransform();

            }
            else
            {
                Util.ShowLog("Active layer");
                MyLayerTransForm.SetTopMost(isTranslateFormTopMostFlag, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                MyLayerTransForm.Activate();
                MyLayerTransForm.Show();
                MyLayerTransForm.UpdateTransform();
            }

           
            if (isProcessTransFlag == false)
            {
                MyLayerTransForm.DisableOverHitLayer();
                MyLayerTransForm.SetVisibleBackground();
            }
            else
            {
                MyLayerTransForm.SetOverHitLayer();
                MyLayerTransForm.SetInvisibleBackground();
            }
        }


        public void MakeOverTransForm(bool isTranslateFormTopMostFlag, bool isProcessTransFlag)
        {
            if (MyOverTransForm == null)
            {
                MyOverTransForm = new TransFormOver();

                MyOverTransForm.Name = "TransFormOver";
                MyOverTransForm.StartPosition = FormStartPosition.Manual;

                //-----------v2
                
                var screens = Screen.AllScreens;


                Rectangle rect = new Rectangle();
                foreach(var obj in screens)
                {
                    rect = Rectangle.Union(rect, obj.Bounds);
                }
                MyOverTransForm.Location = new Point(rect.X, rect.Y);
                MyOverTransForm.Size = new Size(rect.Width, rect.Height);

                int x = Screen.PrimaryScreen.Bounds.X - rect.X;
                int y = Screen.PrimaryScreen.Bounds.Y - rect.Y;
                MyOverTransForm.SetAdjustPosition(x,y);

                //--------------------


                //-----------v1
                //MyOverTransForm.Location = new Point(0, 0);
                //MyOverTransForm.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                //------------------

                MyOverTransForm.SetTopMost(true, false);
              
                MyOverTransForm.UpdateTransform();
                MyOverTransForm.HideTaksBar();
                MyOverTransForm.Show();

                Util.ShowLog("Make over");
            }
            else
            {

                MyOverTransForm.SetTopMost(true, false);
                MyOverTransForm.Activate();
             
                MyOverTransForm.UpdateTransform();
                MyOverTransForm.Show();
            }

      


            Util.ShowLog("Make over???????");

            if (isProcessTransFlag == false)
            {
                MyOverTransForm.setOverHitLayer();
                MyOverTransForm.setVisibleBackground();
            }
            else
            {
                MyOverTransForm.setOverHitLayer();
                MyOverTransForm.setInvisibleBackground();
            }

        }

        public void DestoryTransForm()
        {
            if (MyBasicTransForm != null)
            {
                MyBasicTransForm.destroyForm();
            }

            if (MyLayerTransForm != null)
            {
                MyLayerTransForm.destroyForm();
            }

            if(MyOverTransForm != null)
            {
                MyOverTransForm.destroyForm();
            }

            MyBasicTransForm = null;
            MyLayerTransForm = null;
            MyOverTransForm = null;

        }

        public void SetVisibleTrans()
        {
            if(MyLayerTransForm != null)
            {
                MyLayerTransForm.SetVisibleBackground();
                MyLayerTransForm.DisableOverHitLayer();
            }

            if(MyOverTransForm != null)
            {
                MyOverTransForm.setVisibleBackground();
                MyOverTransForm.disableOverHitLayer();
            }
        }

        public void VisibleOverlayTrans(int waitTime)
        {
            if(MyOverTransForm!=null)
            {
                MyOverTransForm.VisibleOverlayTransAsync(waitTime);
            }
      
        }
     

        public void AddText(string addText)
        {
            if (MyBasicTransForm != null)
            {
                MyBasicTransForm.AddText(addText);
            }

            if (MyLayerTransForm != null)
            {
                MyLayerTransForm.AddText(addText);
            }

            if (MyOverTransForm != null)
            {
               // MyOverTransForm.destroyForm();
            }
        }

        public void ForceUpdateText(string text)
        {
            var transform = GetITransform();

            transform?.ForceUpdateText(text);
        }



        #endregion


        #region :::::::::::::::::::::::::::::: 메세지 창 관련 ::::::::::::::::::::::::::::::

        public static void ShowPopupMessage(string title, string message, Action callback = null)
        {
            DialogResult result = MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true }, message, title);

            if (callback != null)
            {
                callback();
            }

        }
        public static void ShowTwoButtonPopupMessage(string title, string message, Action callback = null)
        {
            DialogResult result = MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true }, message, title, 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            
            if (result == DialogResult.Yes && callback != null)
            {
                callback();
            }

        }

        #endregion
    }
}
