using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MORT
{
    public class FormManager
    {
        #region ::::::::: 텍스트 ::::::::::

        public const string CUSTOM_LABEL_TEXT = "설정 결과를 미리 봅니다.\n 폰트 와 색은 레이어 번역창에만 적용됩니다.\n\n1 2 3 4 5 6\nTank division!";
        public const string CUSTOM_LABEL_TEXT2 = "\n\n{0}다중 OCR 사용시 영역1 문장\n{1}그리고 영역2 문장";
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
        //public TransFormOver MyOverTransForm;
        public RTT MyRemoteController;
        public SearchOptionForm MySearchOptionForm;
        public List<OcrAreaForm> OcrAreaFormList = new List<OcrAreaForm>();
        public List<OcrAreaForm> exceptionAreaFormList = new List<OcrAreaForm>();
        public OcrAreaForm quickOcrAreaForm;
        public OcrAreaForm snapOcrAreaForm;
        public DicEditorForm MyDicEditorForm;
        public ColorGroupForm colorGroupForm;
        public NaverKeyListUI naverKeyListUI;

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

        #region ::::::::::::::::::::: 설정 관련 ::::::::::::::::::::


        private bool isTransformTopMost;

        /// <summary>
        /// 하위 메뉴 모드 가림.
        /// </summary>
        public void HideSubMenu()
        {


        }

        /// <summary>
        /// 하위 메뉴의 탑 모스들 해제함.
        /// </summary>
        public void SetDisableSubMenuTopMost()
        {
            isTransformTopMost = false;

            if (MyBasicTransForm != null && MyBasicTransForm.TopMost)
            {
                isTransformTopMost = true;
                MyBasicTransForm.TopMost = false;
            }

            if (MyLayerTransForm != null && MyLayerTransForm.TopMost)
            {
                isTransformTopMost = true;
                MyLayerTransForm.TopMost = false;
            }

            /*
            if (MyOverTransForm != null && MyOverTransForm.TopMost)
            {
                isTransformTopMost = true;
                MyOverTransForm.TopMost = false;
            }
            */
        }

        /// <summary>
        /// 하위 메뉴의 탑 모스트 다시 설정.
        /// </summary>
        public void ReSettingSubMenuTopMost()
        {
            if (isTransformTopMost)
            {

                if (MyBasicTransForm != null)
                {
                    MyBasicTransForm.TopMost = true;
                }

                if (MyLayerTransForm != null)
                {
                    MyLayerTransForm.TopMost = true;
                }

            }
        }

        public void SetSubMenuTopMost(bool isTopMost)
        {


            if (MyBasicTransForm != null)
            {
                MyBasicTransForm.TopMost = isTopMost;
            }

            if (MyLayerTransForm != null)
            {
                MyLayerTransForm.TopMost = isTopMost;
            }



        }

        #endregion

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

        }

        public void DestorySearchOptionForm()
        {
            MySearchOptionForm = null;
        }

        #region :::::::::::::::::::::::::::::: 네이버 키 관련 ::::::::::::::::::::::::::::::

        public void ShowNaverKeyListUI()
        {
            if (naverKeyListUI == null)
            {
                naverKeyListUI = new NaverKeyListUI();
                naverKeyListUI.StartPosition = FormStartPosition.Manual;

            }

            naverKeyListUI.Activate();
            naverKeyListUI.Init();
            naverKeyListUI.Show();
        }

        public void DestoryNaverKeyListUI()
        {
            naverKeyListUI = null;
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

                Form1.PDelegateSetSpellCheck PDsetSpellCheck = new Form1.PDelegateSetSpellCheck(MyMainForm.setSpellCheck);
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

        public void MakeCpatureAreaForm()
        {
            screenForm.MakeScreenForm(screenForm.ScreenType.Normal);
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
                MyRemoteController.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 320);
                MyRemoteController.InstanceRef = MyMainForm;
                MyRemoteController.ToggleStartButton(false);
                MyRemoteController.Show();
            }
            else
            {
                MyRemoteController.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 320);
                MyRemoteController.Activate();
                MyRemoteController.Show();
            }

        }


        #region :::::::::::::::::::::::::::::: 번역창 관련 ::::::::::::::::::::::::::::::


        public string GetTransCode()
        {
            string transCode = "en";
            transCode = MyMainForm.MySettingManager.TransCode;

            return transCode;
        }

        public string GetResultCode()
        {
            string resultCode = "ko";
            resultCode = MyMainForm.MySettingManager.ResultCode;

            return resultCode;
        }

        public void HideTransFrom()
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

            if(form != null)
            {
                if(form.Visible)
                {
                    form.Hide();
                }
                else
                {
                    form.Show();
                }
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
                MyBasicTransForm.setTopMostFlag(isTranslateFormTopMostFlag);

                MyBasicTransForm.Show();

            }
            else
            {
                MyBasicTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyBasicTransForm.Activate();
                MyBasicTransForm.StartTrans();
                MyBasicTransForm.Show();
            }

            //만약 번역창을 찾지 못했으면
        }


        public void MakeLayerTransForm(bool isTranslateFormTopMostFlag, bool isProcessTransFlag)
        {
            
            if (MyLayerTransForm == null)
            {
                MyLayerTransForm = new TransFormLayer();

                MyLayerTransForm.Name = "TransFormLayer";
                MyLayerTransForm.StartPosition = FormStartPosition.Manual;
                MyLayerTransForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                MyLayerTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyLayerTransForm.Show();
                MyLayerTransForm.UpdateTransform();

            }
            else
            {
                Util.ShowLog("Active layer");
                MyLayerTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyLayerTransForm.Activate();
                MyLayerTransForm.Show();
                MyLayerTransForm.UpdateTransform();
            }

           
            if (isProcessTransFlag == false)
            {
                MyLayerTransForm.disableOverHitLayer();
                MyLayerTransForm.setVisibleBackground();
            }
            else
            {
                MyLayerTransForm.setOverHitLayer();
                MyLayerTransForm.setInvisibleBackground();
            }
        }

        //TODO : TEMP
        public void MakeOverTransForm(bool isProcessTransFlag)
        {
            /*
            if (MyOverTransForm == null)
            {
                MyOverTransForm = new TransFormOver();

                MyOverTransForm.Name = "TransFormOver";
                MyOverTransForm.StartPosition = FormStartPosition.Manual;
                MyOverTransForm.Location = new Point(0,0);
                MyOverTransForm.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                
                MyOverTransForm.setTopMostFlag(true);
                MyOverTransForm.SetTransCode(GetTransCode(), GetResultCode());
                MyOverTransForm.Show();
                MyOverTransForm.UpdateTransform();
                MyOverTransForm.HideTaksBar();


            }
            else
            {
              
                MyOverTransForm.setTopMostFlag(true);
                MyOverTransForm.SetTransCode(GetTransCode(), GetResultCode());
                MyOverTransForm.Activate();
                MyOverTransForm.Show();
                MyOverTransForm.UpdateTransform();
            }

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
            */
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

            MyBasicTransForm = null;
            MyLayerTransForm = null;

        }

        #endregion


        #region :::::::::::::::::::::::::::::: 메세지 창 관련 ::::::::::::::::::::::::::::::

        public static void ShowPopupMessage(string title, string message, Action callback = null)
        {
            DialogResult result = MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true }, message, title);

            if (callback != null)
            {

            }

        }

        #endregion
    }
}
