using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace MORT
{
    public class SettingManager
    {
        public enum Skin { dark, layer, over };   //앞 소문자 바꾸며 안 됨! -> 기존 버전과 호환성
        public enum TransType { db, yandex, naver, google }; //앞 소문자 바꾸며 안 됨! -> 기존 버전과 호환성
        public enum OcrType { Tesseract, Window, NHocr};
        public enum SortType { Normal, Center };

        TransType nowTransType;
        OcrType ocrType;
        Skin nowSkin;
        string nowTessData = "eng";
        Boolean nowIsShowOcrReulstFlag = true;
        Boolean nowIsSaveOcrReulstFlag = false;

        //언어 관련.
        Boolean nowIsUseEngFlag = true;
        Boolean nowIsUseJpnFlag = false;
        Boolean nowIsUseOtherLangFlag = false;
        Boolean isUseStringUpper = false;   //대소문자 구분 사용 안 함.

        string transCode = "en";
        string resultCode = "ko";

        string naverTransCode = "en";
        string naverResultCode = "ko";
        string naverApiType = MORT.NaverTranslateAPI.API_SMT;

        string googleTransCode = "en";
        string googleResultCode = "ko";

        //윈도우 10 관련
        string windowLanguageCode = "";

        Boolean nowIsUseNHocr = false;
        Boolean nowIsSaveInClipboardFlag = false;
        int nowOCRSpeed = 3;
        string nowDBFile = "empty.txt";
        string nowDicFile = "myDic.txt";
        Boolean nowIsUseDicFileFlag = true;
        Boolean nowIsUseErodeFlag = false;
        int nowColorGroupCount = 1;
        Boolean nowIsUseRGBFlag = false;
        Boolean nowIsUseHSVFlag = false;
        List<List<int>> useColorGroup = new List<List<int>>();
        List<int> quickOcrUseColorGroup = new List<int>();
        List<ColorGroup> nowColorGroup = new List<ColorGroup>();
        int nowOCRGroupcount = 0;
        List<int> nowLocationXList = new List<int>();
        List<int> nowLocationYList = new List<int>();
        List<int> nowSizeXList = new List<int>();
        List<int> nowSizeYList = new List<int>();

        SortType nowSortType = SortType.Normal;
        Boolean nowIsRemoveSpaceFlag = false;
        Boolean nowIsActiveWindow = false;
        Boolean nowIsUseBackColor = false;
        float imgZoomSize;

        Font textFont;
        Color textColor;
        Color outLineColor1;
        Color outLineColor2;
        Color backgroundColor;

        public static bool isErrorEmptyGoogleToken = false;


        public Boolean NowIsUseBackColor
        {
            get
            {
                return nowIsUseBackColor;
            }
            set
            {
                nowIsUseBackColor = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }

        public Color OutLineColor1
        {
            get
            {
                return outLineColor1;
            }
            set
            {
                outLineColor1 = value;
            }
        }

        public Color OutLineColor2
        {
            get
            {
                return outLineColor2;
            }
            set
            {
                outLineColor2 = value;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
            }
        }

        public Font TextFont
        {
            get
            {
                return textFont;
            }

            set
            {
                textFont = value;
            }
        }

        public bool NowIsUseDicFileFlag
        {
            get
            {
                return nowIsUseDicFileFlag;
            }
            set
            {
                nowIsUseDicFileFlag = value;
            }
        }

        public string NowDicFile
        {
            get
            {
                return nowDicFile;
            }
            set
            {
                nowDicFile = value;
            }
        }

        public Skin NowSkin
        {
            get
            {
                return nowSkin;
            }
            set
            {
                nowSkin = value;
            }
        }

        public TransType NowTransType
        {
            get
            {
                return nowTransType;
            }
            set
            {
                nowTransType = value;
            }
        }

        public OcrType OCRType
        {
            get
            {
                return ocrType;
            }
            set
            {
                ocrType = value;
            }
        }

        public string NowTessData
        {
            get
            {
                return nowTessData;
            }
            set
            {
                nowTessData = value;
            }
        }

        public bool NowIsShowOcrResultFlag
        {
            get
            {
                return nowIsShowOcrReulstFlag;
            }
            set
            {
                nowIsShowOcrReulstFlag = value;
            }
        }

        public bool NowIsSaveOcrReulstFlag
        {
            get
            {
                return nowIsSaveOcrReulstFlag;
            }
            set
            {
                nowIsSaveOcrReulstFlag = value;
            }
        }

        public bool NowIsUseEngFlag
        {
            get
            {
                return nowIsUseEngFlag;
            }
            set
            {
                nowIsUseEngFlag = value;
            }
        }

        public bool NowIsUseJpnFlag
        {
            get
            {
                return nowIsUseJpnFlag;
            }
            set
            {
                nowIsUseJpnFlag = value;
            }
        }

        public bool NowIsUseOtherLangFlag
        {
            get
            {
                return nowIsUseOtherLangFlag;
            }
            set
            {
                nowIsUseOtherLangFlag = value;
            }
        }

        public bool IsUseStringUpper
        {
            get
            {
                return isUseStringUpper;
            }
            set
            {
                isUseStringUpper = value;
            }
        }

        public string TransCode
        {
            get
            {
                return transCode;
            }
            set
            {
                transCode = value;
            }
        }

        public string ResultCode
        {
            get
            {
                return resultCode;
            }
            set
            {
                resultCode = value;
            }
        }

        public string NaverTransCode
        {
            get
            {
                return naverTransCode;
            }
            set
            {
                naverTransCode = value;
            }
        }

        public string NaverResultCode
        {
            get
            {
                return naverResultCode;
            }
            set
            {
                naverResultCode = value;
            }
        }

        public string NaverApiType
        {
            get
            {
                return naverApiType;
            }
            set
            {
                naverApiType = value;
            }        
        }

        public string GoogleTransCode
        {
            get
            {
                return googleTransCode;
            }
            set
            {
                googleTransCode = value;
            }
        }

        public string GoogleResultCode
        {
            get
            {
                return googleResultCode;
            }
            set
            {
                googleResultCode = value;
            }
        }



        public string WindowLanguageCode
        {
            get
            {
                return windowLanguageCode;
            }
            set
            {
                windowLanguageCode = value;
            }
        }

        public bool NowIsUseNHocr
        {
            get
            {
                return nowIsUseNHocr;
            }
            set
            {
                nowIsUseNHocr = value;
            }
        }

        public bool NowIsSaveInClipboardFlag
        {
            get
            {
                return nowIsSaveInClipboardFlag;
            }
            set
            {
                nowIsSaveInClipboardFlag = value;
            }
        }

        public int NowOCRSpeed
        {
            get
            {
                return nowOCRSpeed;
            }
            set
            {
                nowOCRSpeed = value;
            }
        }

        public string NowDBFile
        {
            get
            {
                return nowDBFile;
            }
            set
            {
                nowDBFile = value;
            }
        }

        public bool NowIsUseErodeFlag
        {
            get
            {
                return nowIsUseErodeFlag;
            }
            set
            {
                nowIsUseErodeFlag = value;
            }
        }
        public List<List<int>> UseColorGroup
        {
            get
            {
                return useColorGroup;
            }
            set
            {
                useColorGroup = value;
            }
        }

        public List<int> QuickOcrUsecolorGroup
        {

            get
            {
                return quickOcrUseColorGroup;
            }
            set
            {
                quickOcrUseColorGroup = value;
            }
        }

        public int NowColorGroupCount
        {
            get
            {
               return nowColorGroupCount;
            }
            set
            {
                nowColorGroupCount = value;
            }
        }

        public List<ColorGroup> NowColorGroup
        {
            get
            {
                return nowColorGroup;
            }
            set
            {
                nowColorGroup = value;
            }
        }

        public int NowOCRGroupcount
        {
            get
            {
                return nowOCRGroupcount;
            }
            set
            {
                nowOCRGroupcount = value;
            }
        }

        public bool NowIsUseRGBFlag
        {
            get
            {
                return nowIsUseRGBFlag;
            }
            set
            {
                nowIsUseRGBFlag = value;
            }
        }

        public bool NowIsUseHSVFlag
        {
            get
            {
                return nowIsUseHSVFlag;
            }
            set
            {
                nowIsUseHSVFlag = value;
            }
        }

        public List<int> NowLocationXList
        {
            get
            {
                return nowLocationXList;
            }
            set
            {
                nowLocationXList = value;
            }
        }

        public List<int> NowLocationYList
        {
            get
            {
                return nowLocationYList;
            }
            set
            {
                nowLocationYList = value;
            }
        }

        public List<int> NowSizeXList
        {
            get
            {
                return nowSizeXList;
            }
            set
            {
                nowSizeXList = value;
            }
        }

        public List<int> NowSizeYList
        {
            get
            {
                return nowSizeYList;
            }
            set
            {
                nowSizeYList = value;
            }
        }

        public SortType NowSortType
        {
            get
            {
                return nowSortType;
            }
            set
            {
                nowSortType = value ;
            }
        }

        public Boolean NowIsRemoveSpace
        {
            get
            {
                return nowIsRemoveSpaceFlag;
            }
            set
            {
                nowIsRemoveSpaceFlag = value;
            }
        }

        public Boolean NowIsActiveWindow
        {
            get
            {
                return nowIsActiveWindow;
            }
            set
            {
                nowIsActiveWindow = value;
            }
        }

        public float ImgZoomSize
        {
            get { return imgZoomSize; }
            set { imgZoomSize = value; }
        }

        public SettingManager()
        {
          
        }

        public int GetLocationX(int index)
        {
            int x = 0;

            int BorderWidth = FormManager.BorderWidth;
            int TitlebarHeight = FormManager.TitlebarHeight;

            if(nowLocationXList.Count > index)
            {
                x = nowLocationXList[index];
            }
         
            //x = Form1.testx;
            return x;
        }

        public int GetLocationY(int index)
        {
            int y = 0;

            int BorderWidth = FormManager.BorderWidth;
            int TitlebarHeight = FormManager.TitlebarHeight;

            if (nowLocationYList.Count > index)
            {
                y = nowLocationYList[index];
            }
           // y = Form1.testy;
            return y;
        }


        public void saveSetting(string fileName)
        {
            try
            {
                using (StreamWriter newTask = new StreamWriter(fileName, false))
                {
                    //스킨
                    string skinString = "#SKIN = @" + nowSkin.ToString(); ;
                    newTask.WriteLine(skinString);

                    //tess 데이타
                    string tessDataString = "#TESS_DATA = @";
                    tessDataString = tessDataString + nowTessData;
                    newTask.WriteLine(tessDataString);

                    //ocr 결과 보여주기
                    string showOCRResultString = "#SHOW_OCR_RESULT = @";
                    showOCRResultString = showOCRResultString + nowIsShowOcrReulstFlag.ToString();
                    newTask.WriteLine(showOCRResultString);

                    //ocr 결과 저장
                    string saveOCRResultString = "#SAVE_OCR_RESULT = @";
                    saveOCRResultString = saveOCRResultString + nowIsSaveOcrReulstFlag.ToString();
                    newTask.WriteLine(saveOCRResultString);

                    //문자 대소문자 구분 안함.
                    string useToUpper = "#STRING_TOUPPER = @" + isUseStringUpper.ToString();
                    newTask.WriteLine(useToUpper);

                    //영어 사용
                    string useEngString = "#USE_ENG = @" + nowIsUseEngFlag.ToString();
                    newTask.WriteLine(useEngString);

                    //일본어 사용
                    string useJpnString = "#USE_JPN = @" + nowIsUseJpnFlag.ToString();
                    newTask.WriteLine(useJpnString);

                    //다른 언어 사용
                    string useOtherLangString = "#USE_OTHER_LANG = @" + this.nowIsUseOtherLangFlag.ToString();
                    newTask.WriteLine(useOtherLangString);

                    string transCodeString = "#TRANS_CODE = @" + transCode;
                    newTask.WriteLine(transCodeString);

                    string resultCodeString = "#RESULT_CODE = @" + resultCode;
                    newTask.WriteLine(resultCodeString);

                    string naverTransCodeString = "#NAVER_TRANS_CODE = @" + naverTransCode;
                    newTask.WriteLine(naverTransCodeString);

                    string naverResultCodeString = "#NAVER_RESULT_CODE = @" + naverResultCode;
                    newTask.WriteLine(naverResultCodeString);

                    string naverApiTypeString = "#NAVER_API_TYPE = @" + NaverApiType;
                    newTask.WriteLine(naverApiTypeString);

                    string googleTransCodeString = "#GOOGLE_TRANS_CODE = @" + googleTransCode;
                    newTask.WriteLine(googleTransCodeString);

                    string googleResultCodeString = "#GOOGLE_RESULT_CODE = @" + googleResultCode;
                    newTask.WriteLine(googleResultCodeString);

                    string windowLanguageCodeString = "#WINDOW_OCR_LANGUAGE = @" + windowLanguageCode;
                    newTask.WriteLine(windowLanguageCodeString);
                    

                    //NHocr 사용 - 더이상 사용 안 함.
                    //string useNHocrString = "#USE_NHOCR = @" + nowIsUseNHocr.ToString();
                    //newTask.WriteLine(useNHocrString);

                    //클립보드에 저장
                    string saveInClipboardString = "#SAVE_IN_CLIPBOARD = @" + nowIsSaveInClipboardFlag.ToString();
                    newTask.WriteLine(saveInClipboardString);

                    //ocr 속도
                    string ocrSpeedString = "#OCR_SPEED = @" + nowOCRSpeed.ToString();
                    newTask.WriteLine(ocrSpeedString);

                    //OCR 방식
                    string ocrTypeString = "#OCR_TPYE = @" + ocrType.ToString();
                    newTask.WriteLine(ocrTypeString);

                    //번역 방식
                    string transTypeString = "#TRANS_TPYE = @" + nowTransType.ToString();
                    newTask.WriteLine(transTypeString);

                    //db 파일
                    string dbFileString = "#DB_FILE = @" + nowDBFile;
                    newTask.WriteLine(dbFileString);

                    //dic 파일
                    string dicFileString = "#DIC_FILE =@" + nowDicFile;
                    newTask.WriteLine(dicFileString);

                    //dic 사용
                    string useDicFlagString = "#USE_DIC = @";
                    useDicFlagString = useDicFlagString + nowIsUseDicFileFlag.ToString();
                    newTask.WriteLine(useDicFlagString);

                    //침식 사용
                    string useErodeString = "#USE_ERODE = @" + nowIsUseErodeFlag.ToString();
                    newTask.WriteLine(useErodeString);

                    //컬러 그룹
                    string colorGroupString = "#COLOR_GROUP = @" + nowColorGroupCount.ToString();
                    newTask.WriteLine(colorGroupString);

                    //RGB
                    string rgbString = "#RGB = @" + nowIsUseRGBFlag.ToString() ;
                    newTask.WriteLine(rgbString);

                    //rgb 값들
                    for (int i = 0; i < nowColorGroupCount; i++)
                    {
                        newTask.WriteLine(nowColorGroup[i].getValueR().ToString());
                        newTask.WriteLine(nowColorGroup[i].getValueG().ToString());
                        newTask.WriteLine(nowColorGroup[i].getValueB().ToString());
                    }

                    //HSV
                    string hsvString = "#HSV = @" + nowIsUseHSVFlag.ToString();
                    newTask.WriteLine(hsvString);

                    for (int i = 0; i < nowColorGroupCount; i++)
                    {
                        newTask.WriteLine(nowColorGroup[i].getValueS1().ToString());
                        newTask.WriteLine(nowColorGroup[i].getValueS2().ToString());
                        newTask.WriteLine(nowColorGroup[i].getValueV1().ToString());
                        newTask.WriteLine(nowColorGroup[i].getValueV2().ToString());
                    }

                    //OCR 그룹
                    string ocrGroupString = "#OCR_GROUP = @" + nowOCRGroupcount.ToString();
                    newTask.WriteLine(ocrGroupString);

                    for (int i = 0; i < nowOCRGroupcount; i++)
                    {
                        newTask.WriteLine(nowLocationXList[i].ToString());
                        newTask.WriteLine(nowLocationYList[i].ToString());
                        newTask.WriteLine(nowSizeXList[i].ToString());
                        newTask.WriteLine(nowSizeYList[i].ToString());
                    }

                    //OCR 사용하는 색그룹
                    string useColorGroupString = "#USE_OCR_COLOR_GROUP = @" + useColorGroup.Count.ToString();
                    newTask.WriteLine(useColorGroupString);

                    for(int i = 0 ; i < useColorGroup.Count; i++)
                    {
                        string data = "";
                        for(int j = 0; j < useColorGroup[i].Count; j++)
                        {
                            data += useColorGroup[i][j].ToString();// +" ";
                            if (j + 1 != useColorGroup[i].Count)
                            {
                                data += " ";
                            }       
                        }
                        newTask.WriteLine(data);                        
                    }

                    //텍스트 정렬
                    string textSort = "#TEXT_SORT = @" + nowSortType.ToString();
                    newTask.WriteLine(textSort);                    
                    
                    //공백 제거
                    string removeSpace = "#USE_REMOVE_SPACE = @" + nowIsRemoveSpaceFlag.ToString();
                    newTask.WriteLine(removeSpace);
                    
                    //폰트 이름
                    string fontName = "#FONT_NAME = @" + textFont.FontFamily.Name;
                    newTask.WriteLine(fontName);

                    string fontSize = "#FONT_SIZE = @" + textFont.Size.ToString();
                    newTask.WriteLine(fontSize);

                    //텍스트 컬러
                    string textColor = "#TEXT_COLOR = @";
                    newTask.WriteLine(textColor);
                    newTask.WriteLine(((int)(255)).ToString());
                    newTask.WriteLine(this.textColor.R.ToString());
                    newTask.WriteLine(this.textColor.R.ToString());
                    newTask.WriteLine(this.textColor.G.ToString());
                    newTask.WriteLine(this.textColor.B.ToString());

                    //외곽선1 컬러
                    string outline1 = "#OUTLINE1_COLOR = @";
                    newTask.WriteLine(outline1);
                    newTask.WriteLine(((int)(255)).ToString());
                    newTask.WriteLine(this.outLineColor1.R.ToString());
                    newTask.WriteLine(this.outLineColor1.G.ToString());
                    newTask.WriteLine(this.outLineColor1.B.ToString());

                    //외곽선2 컬러
                    string outline2 = "#OUTLINE2_COLOR = @";
                    newTask.WriteLine(outline2);
                    newTask.WriteLine(((int)(255)).ToString());
                    newTask.WriteLine(this.outLineColor2.R.ToString());
                    newTask.WriteLine(this.outLineColor2.G.ToString());
                    newTask.WriteLine(this.outLineColor2.B.ToString());

                    //배경 컬러
                    string backColor = "#BACK_COLOR = @" + this.nowIsUseBackColor.ToString();
                    newTask.WriteLine(backColor);
                    newTask.WriteLine(((int)(255)).ToString());
                    newTask.WriteLine(this.backgroundColor.R.ToString());
                    newTask.WriteLine(this.backgroundColor.G.ToString());
                    newTask.WriteLine(this.backgroundColor.B.ToString());

                    //활성화 윈도우 이미지 가져오기
                    string attachWindow = "#USE_ACTIVE_WINDOW = @" + nowIsActiveWindow.ToString();
                    newTask.WriteLine(attachWindow);
                    
                    //이미지 리사이즈 크기
                    string imgZoomSizeString = "#IMG_ZOOM_SIZE = @" + string.Format("{0:F1}", imgZoomSize);
                    newTask.WriteLine(imgZoomSizeString);
                    newTask.Close();

                }
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    fs.Close();
                    fs.Dispose();
                    
                }
                saveSetting(fileName);
            }

        }


        public void SetDefault()
        {
            nowSkin = Skin.layer;
            nowTransType = TransType.yandex;
            ocrType = OcrType.Tesseract;
            nowTessData = "eng";
            nowIsShowOcrReulstFlag = true;
            nowIsSaveOcrReulstFlag = false;
            isUseStringUpper = false;
            nowIsUseEngFlag = true;
            nowIsUseJpnFlag = false;
            nowIsUseOtherLangFlag = false;
            transCode = "en";
            resultCode = "ko";
            naverTransCode = "en";
            naverResultCode = "ko";
            naverApiType = MORT.NaverTranslateAPI.API_SMT;

            googleTransCode = "en";
            googleResultCode = "ko";

            windowLanguageCode = "";
            nowIsUseNHocr = false;
            nowIsSaveInClipboardFlag = false;
            nowOCRSpeed = 3;
            nowDBFile = "empty.txt";
            nowDicFile = "myDic.txt";
            nowIsUseDicFileFlag = true;
            nowIsUseErodeFlag = false;
            nowColorGroupCount = 1;
            nowColorGroup.Clear();
            nowColorGroup = new List<ColorGroup>();
            nowColorGroup.Add(new ColorGroup());
            nowOCRGroupcount = 0;
            quickOcrUseColorGroup.Clear();
            nowIsUseHSVFlag = false;
            nowIsUseRGBFlag = false;
            nowLocationXList.Clear();
            nowLocationXList = new List<int>();
            nowLocationYList.Clear();
            nowLocationYList = new List<int>();
            nowSizeXList.Clear();
            nowSizeXList = new List<int>();
            nowSizeYList.Clear();
            nowSizeYList = new List<int>();
            nowSortType = SortType.Normal;
            nowIsRemoveSpaceFlag = false;
            nowIsActiveWindow = false;
            nowIsUseBackColor = false;
           
            textFont = new Font("맑은 고딕", 15);
            textColor = new Color();
            outLineColor2 = new Color();
            outLineColor1 = new Color();
            backgroundColor = new Color();

            textColor = Color.FromArgb(255, 255, 255);
            outLineColor1 = Color.FromArgb(100, 149, 237);
            outLineColor2 = Color.FromArgb(65, 105, 225);
            backgroundColor = Color.FromArgb(0, 0, 0);
            imgZoomSize = 2;

        }


        public void openSettingfile(string fileName)
        {
            SetDefault();
            try
            {
                StreamReader r = new StreamReader(fileName);
                string line;


                while ((line = r.ReadLine()) != null)
                {
                    if (line.StartsWith("#SKIN"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("dark") == 0)
                            {
                                nowSkin = Skin.dark;
                            }
                            else if (resultString.CompareTo("layer") == 0)
                            {
                                nowSkin = Skin.layer;
                            }
                            else if (resultString.CompareTo("over") == 0)
                            {
                                nowSkin = Skin.over;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#TESS_DATA"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowTessData = resultString;
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#SHOW_OCR_RESULT"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsShowOcrReulstFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsShowOcrReulstFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#SAVE_OCR_RESULT"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsSaveOcrReulstFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsSaveOcrReulstFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#STRING_TOUPPER"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                isUseStringUpper = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                isUseStringUpper = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#USE_JPN"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseJpnFlag = true;
                                nowIsUseOtherLangFlag = false;
                                nowIsUseEngFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseJpnFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }

                    else if (line.StartsWith("#USE_ENG"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseEngFlag = true;
                                nowIsUseJpnFlag = false;
                                nowIsUseOtherLangFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseEngFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }

                    else if (line.StartsWith("#USE_OTHER_LANG"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseOtherLangFlag = true;
                                nowIsUseEngFlag = false;
                                nowIsUseJpnFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseOtherLangFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }

                    else if (line.StartsWith("#TRANS_CODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            transCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#RESULT_CODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            resultCode = resultString;
                        }
                    }

                    else if (line.StartsWith("#NAVER_TRANS_CODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            naverTransCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#NAVER_RESULT_CODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            naverResultCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#NAVER_API_TYPE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            NaverApiType = resultString;
                        }
                    }

                    else if (line.StartsWith("#GOOGLE_TRANS_CODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            googleTransCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#GOOGLE_RESULT_CODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            googleResultCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#WINDOW_OCR_LANGUAGE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            windowLanguageCode = resultString;
                        }
                    }
                    //1.15 이하 버전용. -> nhocr 사용시 ocr 타입 무시.
                    else if (line.StartsWith("#USE_NHOCR"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseNHocr = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseNHocr = false;
                            }
                        }
                    }
                    else if (line.StartsWith("#SAVE_IN_CLIPBOARD"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsSaveInClipboardFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsSaveInClipboardFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }

                    }
                    else if (line.StartsWith("#OCR_SPEED"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowOCRSpeed = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#OCR_TPYE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("Tesseract") == 0)
                            {
                                ocrType = OcrType.Tesseract;
                            }
                            else if (resultString.CompareTo("Window") == 0)
                            {
                                ocrType = OcrType.Window;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#TRANS_TPYE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("yandex") == 0)
                            {
                                nowTransType = TransType.yandex;
                            }
                            else if (resultString.CompareTo("db") == 0)
                            {
                                nowTransType = TransType.db;
                            }
                            else if (resultString.CompareTo("naver") == 0)
                            {
                                nowTransType = TransType.naver;
                            }
                            else if (resultString.CompareTo("google") == 0)
                            {
                                nowTransType = TransType.google;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#DB_FILE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowDBFile = resultString;
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#DIC_FILE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowDicFile = resultString;
                        }
                    }
                    else if (line.StartsWith("#USE_DIC"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseDicFileFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseDicFileFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#USE_ERODE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseErodeFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseErodeFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#COLOR_GROUP"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowColorGroupCount = Convert.ToInt32(resultString);
                            nowColorGroup.Clear();
                            for (int i = 0; i < nowColorGroupCount; i++)
                            {
                                nowColorGroup.Add(new ColorGroup());
                            }

                            quickOcrUseColorGroup.Clear();

                            for(int i = 0 ; i < nowColorGroupCount; i++)
                            {
                                quickOcrUseColorGroup.Add(1);
                            }
                        }
                    }
                    else if (line.StartsWith("#USE_OCR_COLOR_GROUP"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            int count = Convert.ToInt32(resultString);
                            useColorGroup.Clear();
                           
                            
                            for (int i = 0; i < count; i++)
                            {
                                useColorGroup.Add(new List<int>());

                                string[] data = (r.ReadLine()).Split(' ');
                                for(int j = 0 ; j < data.Length; j++)
                                {
                                    useColorGroup[i].Add(Convert.ToInt32(data[j]));
                                }
                            }


                        }
                    }

                    else if (line.StartsWith("#RGB"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseRGBFlag = true;
                                nowIsUseHSVFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseRGBFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }

                        for (int i = 0; i < nowColorGroupCount; i++)
                        {
                            int colorR = Convert.ToInt32(r.ReadLine());
                            int colorG = Convert.ToInt32(r.ReadLine());
                            int colorB = Convert.ToInt32(r.ReadLine());
                            nowColorGroup[i].setRGBValuse(colorR, colorG, colorB);
                        }

                    }
                    else if (line.StartsWith("#HSV"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseHSVFlag = true;
                                nowIsUseRGBFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseHSVFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }

                        for (int i = 0; i < nowColorGroupCount; i++)
                        {
                            int colorS1 = Convert.ToInt32(r.ReadLine());
                            int colorS2 = Convert.ToInt32(r.ReadLine());
                            int colorV1 = Convert.ToInt32(r.ReadLine());
                            int colorV2 = Convert.ToInt32(r.ReadLine());
                            nowColorGroup[i].setHSVValuse(colorS1, colorS2, colorV1, colorV2);
                            nowColorGroup[i].checkHSVRange();
                        }
                    }
                    else if (line.StartsWith("#OCR_GROUP"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowOCRGroupcount = Convert.ToInt32(resultString);

                            for (int i = 0; i < nowOCRGroupcount; i++)
                            {
                                nowLocationXList.Add(Convert.ToInt32(r.ReadLine()));
                                nowLocationYList.Add(Convert.ToInt32(r.ReadLine()));
                                nowSizeXList.Add(Convert.ToInt32(r.ReadLine()));
                                nowSizeYList.Add(Convert.ToInt32(r.ReadLine()));
                            }
                        }
                    }
                    //폰트 색
                    else if (line.StartsWith("#TEXT_COLOR"))
                    {
                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        textColor = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#OUTLINE1_COLOR"))
                    {
                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        outLineColor1 = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#OUTLINE2_COLOR"))
                    {
                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        outLineColor2 = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#BACK_COLOR"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseBackColor = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseBackColor = false;
                            }
                        }

                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        backgroundColor = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#FONT_NAME"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            textFont = new Font(resultString, textFont.Size);
                        }
                    }

                    else if (line.StartsWith("#FONT_SIZE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            textFont = new Font(textFont.FontFamily, Convert.ToSingle(resultString));
                        }
                    }



                    else if (line.StartsWith("#TEXT_SORT"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("Normal") == 0)
                            {
                                nowSortType = SortType.Normal;
                            }
                            else if (resultString.CompareTo("Center") == 0)
                            {
                                nowSortType = SortType.Center;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#USE_REMOVE_SPACE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsRemoveSpaceFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsRemoveSpaceFlag = false;
                            }
                        }
                    }
                    else if (line.StartsWith("#USE_ACTIVE_WINDOW"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsActiveWindow = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsActiveWindow = false;
                            }
                        }
                    }
                    else if (line.StartsWith("#IMG_ZOOM_SIZE"))
                    {
                        int index = line.IndexOf("@");
                        if (index != -1)
                        {
                            string imgZoomSizeString = line.Substring(index + 1);
                            imgZoomSize = (float)(Convert.ToDouble(imgZoomSizeString));
                        }
                    }
                }
                r.Close();
                r.Dispose();       

            }
            catch (FileNotFoundException)
            {
                SetDefault();
            }
            catch (Exception e)
            {
                SetDefault();
            }

            if(nowIsUseNHocr)
            {
                OCRType = OcrType.NHocr;
            }

        }


        public static OcrType GetOcrType(string ocr)
        {
            OcrType result = OcrType.Tesseract;

            if (ocr.CompareTo("Tessract") == 0)
            {
                result = OcrType.Tesseract;
            }
            else if (ocr.CompareTo("Win OCR") == 0)
            {
                result = OcrType.Window;
            }
            else if(ocr.CompareTo("NHocr") == 0)
            {
                result = OcrType.NHocr;
            }

            return result;

        }
    }
}
