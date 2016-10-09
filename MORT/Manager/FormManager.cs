using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MORT
{
    public class FormManager
    {


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

        #region :::::::::::관리 폼::::::::::
       
        public Form1 MyMainForm;
        public TransForm MyBasicTransForm;
        public TransFormLayer MyLayerTransForm;
        public RTT MyRemoteController;
        public SearchOptionForm MySearchOptionForm;
        public List<OcrAreaForm> OcrAreaFormList = new List<OcrAreaForm>();
        public OcrAreaForm quickOcrAreaForm;
        public DicEditorForm MyDicEditorForm;
        public ColorGroupForm colorGroupForm;

        #endregion

        public enum TransFormState { None, Basic, Layer };
        

        public void CloseApplication()
        {
            
        }

        public void MakeSearchOptionForm()
        {
            if (MySearchOptionForm == null)
            {
                MySearchOptionForm = new SearchOptionForm();
                MySearchOptionForm.Name = "SearchOptionForm";
                MySearchOptionForm.StartPosition = FormStartPosition.Manual;
                MySearchOptionForm.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 320);
                MyRemoteController.InstanceRef = MyMainForm;
                MySearchOptionForm.Show();
            }
            else
            {
                MySearchOptionForm.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 400, Screen.PrimaryScreen.Bounds.Height - 320);
                MySearchOptionForm.Activate();
                MySearchOptionForm.Show();
            }

        }

        public void DestorySearchOptionForm()
        {
            MySearchOptionForm = null;
        }

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

      

        public bool getIsShowOcrAreaFlag()
        {
            bool isShowOcrAreaFlag = false;

            if (FormManager.Instace.MySearchOptionForm != null)
            {
                isShowOcrAreaFlag = true;
            }


            return isShowOcrAreaFlag;
        }

        public void MakeQuickCaptureAreaForm()
        {
            screenForm.MakeScreenForm(true);
        }

        public void MakeCpatureAreaForm()
        {
            screenForm.MakeScreenForm(false);
        }
            
        public void ResetCaputreAreaForm()
        {
            DestoryAllOcrAreaForm();
            InitUseColorGroup();  
            for (int i = 0; i < MyMainForm.MySettingManager.NowOCRGroupcount; i++)
            {
                screenForm.makeOcrAreaForm(MyMainForm.MySettingManager.NowLocationXList[i], MyMainForm.MySettingManager.NowLocationYList[i], MyMainForm.MySettingManager.NowSizeXList[i], MyMainForm.MySettingManager.NowSizeYList[i], getIsShowOcrAreaFlag());

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

        public void MakeQuickOcrAreaForm(OcrAreaForm newForm)
        {
            quickOcrAreaForm = newForm;
        }

        public void DestoryAllOcrAreaForm()
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

        public int GetOcrAreaCount()
        {
            int count = 0;
            if(quickOcrAreaForm != null)
            {
                count++;
            }

            count += OcrAreaFormList.Count;
            return count;
        }

        public void SetInvisibleOcrArea()
        {
            foreach (var pair in OcrAreaFormList)
            {
                pair.Opacity = 0;
            }

            if(quickOcrAreaForm != null)
            {
                quickOcrAreaForm.Opacity = 0;
            }
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

        public void MakeBasicTransForm(string bingAccountKey, bool isTranslateFormTopMostFlag)
        {
            if (MyBasicTransForm == null)
            {
                MyBasicTransForm = new TransForm();
                MyBasicTransForm.Name = "TransForm";
                MyBasicTransForm.StartPosition = FormStartPosition.Manual;
                MyBasicTransForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                MyBasicTransForm.setBingAccountKey(bingAccountKey);
                MyBasicTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyBasicTransForm.SetTransCode(GetTransCode(), GetResultCode());

                MyBasicTransForm.Show();

            }
            else
            {
                MyBasicTransForm.setBingAccountKey(bingAccountKey);
                MyBasicTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyBasicTransForm.SetTransCode(GetTransCode() , GetResultCode());
                MyBasicTransForm.Activate();
                MyBasicTransForm.StartTrans();
                MyBasicTransForm.Show();
            }

            //만약 번역창을 찾지 못했으면
        }

        public void MakeLayerTransForm(string bingAccountKey, bool isTranslateFormTopMostFlag, bool isProcessTransFlag)
        {

            if (MyLayerTransForm == null)
            {
                MyLayerTransForm = new TransFormLayer();

                MyLayerTransForm.Name = "TransFormLayer";
                MyLayerTransForm.StartPosition = FormStartPosition.Manual;
                MyLayerTransForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                MyLayerTransForm.setBingAccountKey(bingAccountKey);
                MyLayerTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyLayerTransForm.SetTransCode(GetTransCode(), GetResultCode());
                MyLayerTransForm.Show();
                MyLayerTransForm.UpdateTransform();

            }
            else
            {
                MyLayerTransForm.setBingAccountKey(bingAccountKey);
                MyLayerTransForm.setTopMostFlag(isTranslateFormTopMostFlag);
                MyLayerTransForm.SetTransCode(GetTransCode(), GetResultCode());
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
    }
}
