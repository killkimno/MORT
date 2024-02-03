using CloudVision;
using MORT.Model.OCR;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MORT
{
    public struct WinOCRResultData
    {
        public bool isEmpty;
        public int lineCount;       //라인 수.
        public string[] words;      //모든 문장.
        public double[] x;             //x값들
        public double[] y;             //y값들
        public double[] sizeX;         //size x;
        public double[] sizeY;         //size y;
        public int[] wordCounts;    //각 라인마다 워드 수.
        public double angle;
        public int wordsIndex;      //쓰이나??
    }

    public struct OcrResult
    {
        public bool isEmpty;
        public int lineCount;       //라인 수.
        public string[] words;      //모든 문장.
        public double[] x;             //x값들
        public double[] y;             //y값들
        public double[] sizeX;         //size x;
        public double[] sizeY;         //size y;
        public int[] wordCounts;    //각 라인마다 워드 수.
        public double angle;
        public int wordsIndex;      //쓰이나???


        public OcrResult(WinOCRResultData data)
        {
            isEmpty = data.isEmpty;
            lineCount = data.lineCount;
            words = data.words;
            x = data.x;
            y = data.y;
            sizeX = data.sizeX;
            sizeY = data.sizeY;
            wordCounts = data.wordCounts;
            angle = data.angle;
            wordsIndex = data.wordsIndex;
        }

        public OcrResult(GoogleOcrResult data)
        {
            isEmpty = data.isEmpty;
            lineCount = data.lineCount;
            words = data.words;
            x = data.x;
            y = data.y;
            sizeX = data.sizeX;
            sizeY = data.sizeY;
            wordCounts = data.wordCounts;
            angle = 0;
            wordsIndex = data.wordsIndex;
        }

        public OcrResult(EasyOcrResultModel data)
        {
            isEmpty = data.IsEmpty;
            lineCount = data.LineCount;
            words = data.Words;
            x = data.X;
            y = data.Y;
            sizeX = data.SizeX;
            sizeY = data.SizeY;
            wordCounts = data.WordCounts;
            angle = 0;
            wordsIndex = data.WordsIndex;
        }
    }

    public struct OCRResultData
    {
        public int ocrIndex;
        public Rectangle rect;
        public string ocrData;
        public string resultData;

        public bool isAlreadyUseDB;
    }


    public class OCRDataManager
    {
        public enum WordAngleType
        {
            Horizontal, Vertical,
        }

        /// <summary>
        /// 문장 데이터.
        /// </summary>
        public class TransData
        {
            public int index;
            //public int ocr;
            public string trans = "";

            public bool isInsert = false;
            public List<LineData> lineDataList = new List<LineData>();
            public Rectangle lineRect = new Rectangle();
            public WordAngleType angleType;

            public bool CheckIsSameLine(LineData lineData, bool mergeLine)
            {
                bool isSame = false;

                if(lineDataList.Count > 0)
                {
                    bool IsIntersects = GetIsIntersectsWith(lineDataList[lineDataList.Count -1], lineData, mergeLine);


                    if (IsIntersects)
                    {
                        isSame = true;
                    }
                }

                return isSame;
            }

        }


        /// <summary>
        /// 줄 데이터.
        /// </summary>
        public class LineData
        {
            public int groupIndex;
            public string lineString;
            public string transString= "";
            public List<string> wordList = new List<string>();
            public List<string> transWordList = new List<string>();
            public Rectangle lineRect = new Rectangle();
            public List<Rectangle> wordRectList = new List<Rectangle>();

            public WordAngleType angleType;


            public bool GetIsEndLine()
            {
                bool isEnd = false;
                string line = lineString.Replace(" ", "");

                if (line.Length >= 1)
                {
                    if (line[line.Length - 1] == '.' || line[line.Length - 1] == '?' || line[line.Length - 1] == '!')
                    {
                        isEnd = true;
                    }

                }
                return isEnd;
            }
        }

        public class ResultData
        {
            public int index;
            //TODO : 미리 다 해놓아야 한다.
            public bool SnapShot;
            public List<LineData> lineDataList = new List<LineData>();
            public List<TransData> transDataList = new List<TransData>();
            public Rectangle resultRect;

            //public string ocrString = "";
            public string transString = "";

            public string GetOCR()
            {
                string ocr = "";

                for(int i = 0; i < lineDataList.Count; i++)
                {
                    for(int j = 0; j < lineDataList[i].wordList.Count; j++)
                    {
                        ocr += lineDataList[i].wordList[j] + " ";
                    }
                }

                return ocr;
            }

            public string GetTrans()
            {
                string ocr = "";
                for (int i = 0; i < transDataList.Count; i++)
                {
                    ocr += transDataList[i].trans + " ";
                }

                return GetOCR();
            }

            public List<string> GetOcrText()
            {
                List<string> list = new List<string>();
                for(int i = 0; i < transDataList.Count; i++)
                {
                    string text = "";
                    for (int j = 0; j < transDataList[i].lineDataList.Count; j++)
                    {

                        text += transDataList[i].lineDataList[j].lineString;
                    }

                    list.Add(text);
                }

                return list;
            }

            public void InitLine(bool mergeLine)
            {
                TransData transData = null;
                for (int i = 0; i < lineDataList.Count; i++)
                {
                    bool isNew = false;

                    if (transData == null)
                    {
                        isNew = true;
                    }

                    if (isNew)
                    {
                        transData = new TransData();
                        transData.lineDataList.Add(lineDataList[i]);
                        transData.isInsert = true;
                        transData.angleType = lineDataList[i].angleType;
                        this.transDataList.Add(transData);
                    }
                    else
                    {

                        if (!Form1.IsDebugTransOneLine && transData.CheckIsSameLine(lineDataList[i],mergeLine))
                        {
                            //같은 라인이다.

                            bool isEnd = lineDataList[i].GetIsEndLine();
                            transData.lineDataList.Add(lineDataList[i]);

                            if (isEnd)
                            {
                                transData = null;
                            }
                        }
                        else
                        {
                            //같은 라인이 아니다.
                            transData = new TransData();
                            transData.lineDataList.Add(lineDataList[i]);
                            transData.isInsert = true;
                            transData.angleType = lineDataList[i].angleType;

                            this.transDataList.Add(transData);
                        }
                    }                    
                }

                for (int i = 0; i < transDataList.Count; i++)
                {
                    transDataList[i].lineRect = new Rectangle();

                    if (transDataList[i].lineDataList.Count >0)
                    {
                        var rect =  transDataList[i].lineDataList[0].lineRect;
                        for (int j = 1; j < transDataList[i].lineDataList.Count; j++)
                        {
                            rect = Rectangle.Union(rect, transDataList[i].lineDataList[j].lineRect);
                        }

                        transDataList[i].lineRect = rect;
                    }    
                }
            }

            /// <summary>
            /// 번역 결과 초기화.
            /// </summary>
            /// <param name="transString"></param>
            public void ApplyTransResult(string transString, SettingManager.TransType transType)
            {
                this.transString = transString;

                string[] words = Util.GetSpliteByToken(this.transString, transType);

                for(int i = 0; i < transDataList.Count && i < words.Length; i++)
                {
                    transDataList[i].trans = words[i];
                }            
            }

        }
        private static OCRDataManager instance;
        public static OCRDataManager Instace
        {
            get
            {
                if (instance == null)
                {
                    instance = new OCRDataManager();
                }
                return instance;
            }
        }
        private List<OCRResultData> resultList = new List<OCRResultData>(); //안 쓰임.
        private List<ResultData> dataList = new List<ResultData>();
        public bool MergeLine { get; set; } = false;


        public List<ResultData> GetData()
        {
            List<ResultData> list = new List<ResultData>();
            for(int i = 0; i< dataList.Count; i++)
            {
                list.Add(dataList[i]);
            }
            return dataList;
        }

        public ResultData GetData(int index)
        {
            ResultData data = null;

            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].index == index)
                {
                    data = dataList[i];
                }
            }

            return data;
        }
        public static int  GetFontSize(LineData data)
        {
            int size = 10;

            if (data.angleType == WordAngleType.Horizontal)
            {
                size = data.lineRect.Height;
            }
            else if (data.angleType == WordAngleType.Vertical)
            {
                size = data.lineRect.Width;
            }

            return size;
        }

        public static bool  GetIsIntersectsWith(LineData beforeData, LineData data, bool mergeLine)
        {
            bool isIntersect = false;

            if (beforeData.angleType == data.angleType)
            {
                Rectangle rect1 = new Rectangle(beforeData.lineRect.X, beforeData.lineRect.Y, beforeData.lineRect.Width, beforeData.lineRect.Height);
                int beforeFontSize = GetFontSize(beforeData);
                int fontSize = GetFontSize(data);

                int diff = Math.Abs(beforeFontSize - fontSize);
                float percent = (float)(diff) / (float)fontSize;
                Util.ShowLog("Before : " + beforeFontSize + " / current : " + fontSize + " / diff : " + diff + " / percent : " + (float)percent);
                if (percent > 0.9f)
                {                  
                    return false;
                }

                //세로 처리
                if (beforeData.angleType == WordAngleType.Horizontal)
                {
                    Rectangle rect2 = rect1;
                    rect2.Width += (int)(beforeFontSize * 4f);
                    isIntersect = rect2.IntersectsWith(data.lineRect);

                    if(!isIntersect && mergeLine)
                    {
                        //rect1.Inflate(0, -(int)(fontSize * 2.5f));
                        rect1.Height += (int)(beforeFontSize * 1.3f);
                        isIntersect = rect1.IntersectsWith(data.lineRect);
                    }
                    else
                    {
                        Util.ShowLog("Is Splite line?????????");
                    }
                   
                }
                //가로 처리
                else if(mergeLine)
                {
                    int adjust = (int)(beforeFontSize * 0.8f);

                    rect1.Width += adjust;
                    rect1.X -= adjust;
                    isIntersect = rect1.IntersectsWith(data.lineRect);
                }
            }


            return isIntersect;
        }

        public void ClearData()
        {
            if(dataList == null)
            {
                dataList = new List<ResultData>();
            }
            else
            {
                dataList.Clear();
            }    
        }

        public ResultData AddData(OcrResult data, int index, bool snapShot)
        {
            ResultData resultData = new ResultData();
            resultData.index = index;
            resultData.SnapShot = snapShot;

            //Util.ShowLog("line = " + point.lineCount);
            int count = 0;
            for (int i = 0; i < data.lineCount; i++)
            {
                LineData lineData = new LineData();
                string lineString = "";
                //Util.ShowLog("----line start----");

                for (int j2 = 0; j2 < data.wordCounts[i]; j2++)
                {
                    lineString += data.words[count] + " ";

                    Rectangle rect = new Rectangle((int)data.x[count], (int)data.y[count], (int)data.sizeX[count], (int)data.sizeY[count]);
                    lineData.wordList.Add(data.words[count]);
                    lineData.wordRectList.Add(rect);

                    count++;
                }
                Rectangle lineRect = new Rectangle();
                //줄 처리.
                if (lineData.wordRectList.Count > 1)
                {
                    lineRect = lineData.wordRectList[0];
                    for (int j = 1; j < lineData.wordRectList.Count; j++)
                    {
                        lineRect = Rectangle.Union(lineRect, lineData.wordRectList[j]);
                    }
                    lineData.lineRect = lineRect;
                }
                else if (lineData.wordRectList.Count == 1)
                {
                    lineRect = lineData.wordRectList[0];
                    lineData.lineRect = lineRect;
                }

                lineData.lineString = lineString;
                resultData.lineDataList.Add(lineData);

                if(lineRect.Height > lineRect.Width * 1.5f)
                {
                    lineData.angleType = WordAngleType.Vertical;
                }
                else
                {
                    lineData.angleType = WordAngleType.Horizontal;
                }
            }

            //전체 영역 처리.
            if (resultData.lineDataList.Count == 1)
            {
                resultData.resultRect = resultData.lineDataList[0].lineRect;
            }
            else if (resultData.lineDataList.Count > 1)
            {
                resultData.resultRect = resultData.lineDataList[0].lineRect;
                for (int i = 1; i < resultData.lineDataList.Count; i++)
                {
                    resultData.resultRect = Rectangle.Union(resultData.resultRect, resultData.lineDataList[i].lineRect);
                }
            }            
            resultData.InitLine(MergeLine);
            dataList.Add(resultData);


            return resultData;
        }
    }



}
