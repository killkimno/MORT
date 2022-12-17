using MORT.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MORT.Form1;
using static MORT.Manager.OcrManager;
using static MORT.SettingManager;

namespace MORT.Service.ProcessTranslateService
{
    internal class ProcessTranslateService
    {
        private delegate void myDelegate(string transText);

        //번역 쓰레드
        private Thread thread;
        public bool IdleState => thread == null || isEndFlag;
        public bool ProcessingState => thread != null && thread.IsAlive;

        public int OcrProcessSpeed { get; set; } = 2000;                 //ocr 처리 딜레이 시간
        private bool _clipeBoardReady = false;
        public bool DebugUnlockOCRSpeed
        {
            get { return _isDebugUnlockOCRSpeed; }
            set { _isDebugUnlockOCRSpeed = value; }
        }
        private bool _isDebugUnlockOCRSpeed = false;

        // TODO : 하나로 통합해야 함
        public bool IsUseClipBoardFlag
        {
            get
            {
                return MySettingManager.NowIsSaveInClipboardFlag;
            }
        }

        public string NowOcrString { get; set; } = "";                   //현재 ocr 문장
        public SettingManager.TransType TransType { get; set; }
        public Action<bool> OnStopTranslate { get; }

        volatile bool isEndFlag = false;            //번역 끝내는 플레그
        private readonly Form _parent;
        private readonly SettingManager MySettingManager;
        private readonly WinOcrLoader _loader;
        private readonly bool _isAvailableWinOCR;
        private OcrMethodType _ocrMethodType = OcrMethodType.None;

        private string LocalizeString(string key, bool replaceLine = false)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key).Replace("[]", "");
        }

        public ProcessTranslateService(Form parent, SettingManager settingManager, WinOcrLoader loader, bool isAvailableWinOCR , Action<bool> OnStopTranslate)
        {
            _parent = parent;
            MySettingManager = settingManager;
            _loader = loader;
            _isAvailableWinOCR = isAvailableWinOCR;
            this.OnStopTranslate = OnStopTranslate;
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

            if (isDebugTransOneLine)
            {
                isRequireReplace = false;
            }
            else if (MySettingManager.NowTransType == SettingManager.TransType.db || MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                isRequireReplace = false;
            }

            if (isRequireReplace)
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

        /// <summary>
        /// 캡쳐를 이용해 OCR 처리를 위한 화면 모델을 가져온다
        /// </summary>
        /// <param name="ocrAreaCount"></param>
        /// <param name="imgDataList"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        private void MakeImgModelsFromCapture(int ocrAreaCount, List<ImgData> imgDataList, ref int positionX, ref int positionY)
        {
            byte[] byteData = default(byte[]);
            int width = 0;
            int height = 0;

            //캡쳐로부터 전체 이미지를 가져온다
            GetImgBytesFromCapture(ref byteData, ref width, ref height, ref positionX, ref positionY);

            for (int j = 0; j < ocrAreaCount; j++)
            {
                int x = 15;
                int y = 0;
                int channels = 4;
                IntPtr data = IntPtr.Zero;

                //코어에서 지정한 영역만큼 다시 재가공한다
                data = processGetImgDataFromByte(j, width, height, positionX, positionY, byteData, ref x, ref y, ref channels);

                if (data != IntPtr.Zero)
                {
                    var arr = new byte[x * y * channels];
                    Marshal.Copy(data, arr, 0, x * y * channels);
                    Marshal.FreeHGlobal(data);

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
        private void GetImgBytesFromCapture(ref byte[] byteData, ref int width, ref int height, ref int positionX, ref int positionY)
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

        /// <summary>
        /// OCR 처리를 위한 화면 데이터를 만든다
        /// </summary>
        /// <param name="ocrAreaCount"></param>
        /// <param name="imgDataList"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        private void MakeImgModels(int ocrAreaCount, List<ImgData> imgDataList, ref int positionX, ref int positionY)
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
        /// OCR이 인식한 데이터 기반으로 최종 OCR / 번역문을 ref 로 저장한다
        /// </summary>
        /// <param name="index">ocr 영역 인덱스</param>
        /// <param name="winOcrResultData">win ocr 결과</param>
        /// <param name="imgDataList">화면 데이터</param>
        /// <param name="currentOcr">현재 ocr이 인식한 ocr 문장</param>
        /// <param name="ocrResult">가공한 ocr 문장</param>
        /// <param name="finalTransResult">번역 결과</param>
        private void MakeFinalOcrAndTrans(int index, OCRDataManager.ResultData winOcrResultData, List<ImgData> imgDataList, string currentOcr,
            ref string ocrResult, ref string finalTransResult)
        {
            string transResult;

            List<string> ocrList = null;
            if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                if (winOcrResultData != null)
                {
                    ocrList = winOcrResultData.GetOcrText();
                    currentOcr = "";

                    for (int i = 0; i < ocrList.Count; i++)
                    {
                        ocrList[i] = AdjustText(ocrList[i]);

                        //OCR 영역 처리를 위해 한줄로 변환한다
                        currentOcr += System.Environment.NewLine + Util.GetSpliteToken(TransType) + ocrList[i];
                    }
                }
            }
            else
            {
                currentOcr = AdjustText(currentOcr);
            }

            System.Threading.Tasks.Task<string> transTask = null;

            transTask = TransManager.Instace.StartTrans(currentOcr, MySettingManager.NowTransType, ocrList);
            //번역 결과를 적용한다
            transResult = transTask.Result;

            if (winOcrResultData != null)
            {
                winOcrResultData.ApplyTransResult(transResult, TransType);
            }

            if (imgDataList.Count > 1)
            {
                if (MySettingManager.IsShowOCRIndex)
                {
                    if (!string.IsNullOrEmpty(currentOcr))
                    {
                        if (transResult != "not thing")
                        {
                            finalTransResult += (imgDataList[index].index + 1).ToString() + " : " + transResult + System.Environment.NewLine;

                        }
                    }

                    ocrResult += (imgDataList[index].index + 1).ToString() + " : " + currentOcr + System.Environment.NewLine;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentOcr))
                    {
                        if (transResult != "not thing")
                        {
                            finalTransResult += "- " + transResult;

                            if (index + 1 < imgDataList.Count)
                            {
                                finalTransResult += System.Environment.NewLine;

                            }
                        }

                        ocrResult += "- " + currentOcr;

                        if (index + 1 < imgDataList.Count)
                        {
                            ocrResult += System.Environment.NewLine;
                        }
                    }
                }
            }
            else
            {
                finalTransResult = transResult;
                ocrResult = currentOcr;
            }
        }

        //클립보드에 ocr 문장 저장
        private void SetClipBoard(string transText)
        {
            if (transText != null)
            {
                try
                {
                    _clipeBoardReady = false;
                    string replaceOcrText = transText.Replace(" ", "");
                    replaceOcrText = transText.Replace("not thing", " ");
                    if (replaceOcrText.CompareTo("") != 0)
                    {
                        Clipboard.SetText(replaceOcrText);               //인시로 둠
                    }

                    _clipeBoardReady = true;
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    _clipeBoardReady = true;
                    return;
                }
            }

            _clipeBoardReady = true;
        }

        public void DoTextToSpeach(string text)
        {
            if (_isAvailableWinOCR && MySettingManager.IsUseTTS)
            {
                int type = 0;
                if (MySettingManager.IsWaitTTSEnd)
                {
                    type = 1;
                }

                _loader.TextToSpeach(text, type);
            }
        }

        private void DoTrans(OcrMethodType ocrMethodType)
        {
            _ocrMethodType = ocrMethodType;
            bool isOnce = ocrMethodType != OcrMethodType.Normal;
            bool useGoogleOcr = false;

            if (MySettingManager.OCRType == SettingManager.OcrType.Google)
            {
                if (!isOnce)
                {

                    FormManager.Instace.ForceUpdateText(LocalizeString("Google Ocr Realtime Error"));
                    return;
                }
                else
                {
                    useGoogleOcr = true;
                }
            }
            else if (isOnce && OcrManager.Instace.CheckGoogleOcrPriorty)
            {
                //만약 항시 사용이면 useGoogle
                useGoogleOcr = true;
            }


            var transForm = FormManager.Instace.GetITransform();

            if (transForm != null)
            {
                transForm.StartTrans();
            }

            //캡쳐할 클라이언트 위치.
            int clientPositionX = 0;
            int clientPositionY = 0;

            string formerOcrString = "";    //바로 이전에 가져온 문장
            _clipeBoardReady = true;
            int lastTick = 0;
            try
            {
                while (isEndFlag == false)
                {
                    int diff = Math.Abs(System.Environment.TickCount - lastTick);

                    //TODO :빠른 속도를 원하면 저 주석 해제하면 됨
                    if (diff >= OcrProcessSpeed/* / 10*/ || _isDebugUnlockOCRSpeed)
                    {
                        lastTick = System.Environment.TickCount;

                        if (FormManager.Instace.MyBasicTransForm != null || FormManager.Instace.MyLayerTransForm != null || FormManager.Instace.MyOverTransForm != null)
                        {
                            string finalTransResult = "";

                            if (useGoogleOcr)
                            {
                                unsafe
                                {
                                    int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                    List<ImgData> imgDataList = new List<ImgData>();

                                    if (MySettingManager.isUseAttachedCapture)
                                    {
                                        MakeImgModelsFromCapture(ocrAreaCount, imgDataList, ref clientPositionX, ref clientPositionY);
                                    }
                                    else
                                    {
                                        MakeImgModels(ocrAreaCount, imgDataList, ref clientPositionX, ref clientPositionY);
                                    }

                                    if (isEndFlag)
                                    {
                                        break;
                                    }


                                    string ocrResult = "";
                                    string transResult = "";
                                    finalTransResult = "";

                                    OCRDataManager.Instace.ClearData();


                                    for (int j = 0; j < imgDataList.Count; j++)
                                    {
                                        var task = OcrManager.Instace.ProcessGoogleAsync(imgDataList[j]);
                                        string currentOcr = "";

                                        var result = task.Result;

                                        currentOcr = result.MainText;

                                        OcrResult point = new OcrResult(result);

                                        OCRDataManager.ResultData winOcrResultData = OCRDataManager.Instace.AddData(point, j, ocrMethodType == OcrMethodType.Snap);

                                        MakeFinalOcrAndTrans(j, winOcrResultData, imgDataList, currentOcr, ref ocrResult, ref finalTransResult);
                                    }

                                    NowOcrString = ocrResult;
                                    imgDataList.Clear();
                                    imgDataList = null;
                                }

                            }

                            #region :::::::::: 윈도우 OCR 처리 :::::::::::

                            //win ocr 처리.
                            else if (MySettingManager.OCRType == SettingManager.OcrType.Window)
                            {
                                if (_loader.GetIsAvailableOCR())
                                {
                                    unsafe
                                    {
                                        Util.CheckTimeSpan(true);
                                        int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                        List<ImgData> imgDataList = new List<ImgData>();

                                        if (MySettingManager.isUseAttachedCapture)
                                        {
                                            MakeImgModelsFromCapture(ocrAreaCount, imgDataList, ref clientPositionX, ref clientPositionY);
                                        }
                                        else
                                        {
                                            MakeImgModels(ocrAreaCount, imgDataList, ref clientPositionX, ref clientPositionY);
                                        }

                                        if (isEndFlag)
                                        {
                                            break;
                                        }

                                        string ocrResult = "";
                                        string transResult = "";
                                        finalTransResult = "";

                                        OCRDataManager.Instace.ClearData();

                                        for (int j = 0; j < imgDataList.Count; j++)
                                        {
                                            //잠시 막음 - 원래 이게 성장임
                                            _loader.SetImg(imgDataList[j].data, imgDataList[j].channels, imgDataList[j].x, imgDataList[j].y);

                                            Util.CheckTimeSpan(false);
                                            imgDataList[j].Clear();
                                            _loader.MakeBitMap();
                                            _loader.ProcessOcrFunc();

                                            while (!isEndFlag && !_loader.GetIsAvailableOCR())
                                            {
                                                //Thread.SpinWait(1);
                                                Thread.Sleep(2);
                                            }

                                            string currentOcr = _loader.GetText();

                                            IntPtr ptr = _loader.GetMar();
                                            WinOCRResultData point = (WinOCRResultData)Marshal.PtrToStructure(ptr, typeof(WinOCRResultData));
                                            OCRDataManager.ResultData winOcrResultData = OCRDataManager.Instace.AddData(new OcrResult(point), j, ocrMethodType == OcrMethodType.Snap);

                                            Marshal.FreeCoTaskMem(ptr);

                                            MakeFinalOcrAndTrans(j, winOcrResultData, imgDataList, currentOcr, ref ocrResult, ref finalTransResult);

                                        }

                                        NowOcrString = ocrResult;
                                        imgDataList.Clear();
                                        imgDataList = null;
                                    }
                                }
                                else
                                {
                                    //준비되지 않았으면 이전과 같게 처리.
                                    NowOcrString = formerOcrString;
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

                                    GetImgBytesFromCapture(ref byteData, ref width, ref height, ref positionX, ref positionY);

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


                                NowOcrString = sb.ToString();       //ocr 결과

                                //------------------OCR 줄바꿈 없애기 처리---------------------
                                NowOcrString = NowOcrString.Replace("\r\n", "\n");


                                if (!isDebugTransOneLine)    //디버그 - 한 줄씩 번역이 켜져 있으면 -> 줄바꿈 없애기를 안 한다
                                {
                                    if (MySettingManager.NowIsRemoveSpace)
                                    {
                                        NowOcrString = NowOcrString.Replace("\n", "");
                                    }
                                    else
                                    {
                                        NowOcrString = NowOcrString.Replace("\n", " ");
                                    }
                                }

                                //---------------------------------------
                                NowOcrString = NowOcrString.Replace("\t", System.Environment.NewLine);

                                finalTransResult = sb2.ToString();      //번역 결과.
                                sb.Clear();
                                sb2.Clear();


                                if (MySettingManager.NowTransType != SettingManager.TransType.db && formerOcrString.CompareTo(NowOcrString) != 0)
                                {
                                    System.Threading.Tasks.Task<string> test = TransManager.Instace.StartTrans(NowOcrString, MySettingManager.NowTransType);
                                    finalTransResult = test.Result;
                                }
                            }

                            //TODO : Async 문으로 변경하자

                            //OCR, 번역 끝 화면에 뿌리기
                            //새로 데이터 갱신해야 함.
                            if (formerOcrString.CompareTo(NowOcrString) != 0 || NowOcrString == "")
                            {
                                formerOcrString = NowOcrString;
                                if (IsUseClipBoardFlag == true && _clipeBoardReady)
                                {
                                    _parent.BeginInvoke(new myDelegate(SetClipBoard), new object[] { NowOcrString });
                                }

                                if (MySettingManager.NowSkin == SettingManager.Skin.dark && FormManager.Instace.MyBasicTransForm != null)
                                {
                                    FormManager.Instace.MyBasicTransForm.updateText(finalTransResult, NowOcrString, TransType, MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag);
                                }
                                else if (MySettingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyLayerTransForm != null)
                                {
                                    Action action = delegate
                                    {
                                        if (FormManager.Instace.MyLayerTransForm != null)
                                        {
                                            FormManager.Instace.MyLayerTransForm.updateText(finalTransResult, NowOcrString, MySettingManager.NowIsShowOcrResultFlag, MySettingManager.NowIsSaveOcrReulstFlag);
                                        }
                                    };
                                    _parent.BeginInvoke(action);
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

                                    _parent.BeginInvoke(action);
                                }

                                if (MySettingManager.NowSkin == SettingManager.Skin.over)
                                {
                                    string transResult = finalTransResult.Replace(Util.GetSpliteToken(TransType), "");
                                    DoTextToSpeach(transResult);
                                }
                                else
                                {
                                    DoTextToSpeach(finalTransResult);
                                }

                                if (isOnce)
                                {
                                    isEndFlag = true;
                                    _parent.BeginInvoke((Action)(() => OnStopTranslate(true)));
                                }
                            }
                            else
                            {
                                //이전과 같아서 그래픽만 갱신함.
                                if (MySettingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyLayerTransForm != null)
                                {
                                    FormManager.Instace.MyLayerTransForm.UpdatePaint();
                                }

                                if (MySettingManager.NowSkin == SettingManager.Skin.over && FormManager.Instace.MyOverTransForm != null)
                                {
                                    FormManager.Instace.MyOverTransForm.UpdatePaint();
                                }

                                if (isOnce)
                                {
                                    isEndFlag = true;
                                    _parent.BeginInvoke((Action)(() => OnStopTranslate(true)));
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


        public void ProcessTrans(OcrMethodType ocrMethodType)              //번역 시작 쓰레드
        {
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;               
            }

            thread = new Thread(() => DoTrans(ocrMethodType));
            thread.Start();
        }

        public void StopTranslate()
        {
            if (thread != null && thread.IsAlive == true)
            {
                isEndFlag = true;
                thread.Join();
                thread = null;
                isEndFlag = false;
            }
        }

        /// <summary>
        /// 작업을 처리한 후 번역 다시 시작 - 기존 번역이 없으면 무시
        /// </summary>
        /// <param name="callback"></param>
        public bool PauseAndRestartTranslate(Action callback, OcrMethodType ocrMethodType = OcrMethodType.None)
        {
            bool requireRestart = false;
            if (thread != null && thread.IsAlive == true)
            {
                requireRestart = true;
                isEndFlag = true;
                thread.Join();

                isEndFlag = false;
            }

            callback();

            if(requireRestart)
            {
                thread = new Thread(() => DoTrans(ocrMethodType == OcrMethodType.None ? _ocrMethodType : ocrMethodType));
                thread.Start();
            }

            return requireRestart;
        }
    }

}
