using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public float angle;

    }

    public struct OCRResultData
    {
        public int ocrIndex;
        public Rectangle rect;
        public string ocrData;
        public string resultData;

        public bool isAlreadyUseDB;
    }


    class OCRDataManager
    {
        public enum WordAngleType
        {
            Horizontal, Vertical,
        }

        public class LineData
        {
            public int groupIndex;
            public string lineString;
            public List<string> wordList = new List<string>();
            public Rectangle lineRect = new Rectangle();
            public List<Rectangle> wordRectList = new List<Rectangle>();

            public WordAngleType angleType;
        }

        public class ResultData
        {
            public int index;
            public List<LineData> lineDataList = new List<LineData>();
            public Rectangle resultRect;

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
        private List<OCRResultData> resultList = new List<OCRResultData>();
        private List<ResultData> dataList = new List<ResultData>();

        public void clearData()
        {
            dataList.Clear();
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
        public int GetFontSize(LineData data)
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

        public bool GetIsIntersectsWith(LineData beforeData, LineData data)
        {
            bool isIntersect = false;

            if (beforeData.angleType == data.angleType)
            {
                Rectangle rect1 = new Rectangle(beforeData.lineRect.X, beforeData.lineRect.Y, beforeData.lineRect.Width, beforeData.lineRect.Height);
                int fontSize = GetFontSize(beforeData);

                if (beforeData.angleType == WordAngleType.Horizontal)
                {
                    //rect1.Inflate(0, -(int)(fontSize * 2.5f));
                    rect1.Height += (int)(fontSize * 2.5f);
                    isIntersect = rect1.IntersectsWith(data.lineRect);
                    //Util.ShowLog("result : " + isIntersect +"  before " + beforeData.lineRect.ToString() + " after : " + rect1.ToString() + " font : " + fontSize);
                }
                else
                {

                }
            }


            return isIntersect;
        }
        public void CalculateLines(ResultData data)
        {
            int groupIndex = 0;
            /*
             * 처리 해야 할 것.
             * 1. 그룹 인덱스
             * 2. 
             * 
             * 
             * 
             */

            if (data.lineDataList.Count > 0)
            {
                WordAngleType type = data.lineDataList[0].angleType;
                data.lineDataList[0].groupIndex = groupIndex;
                LineData beforeData = data.lineDataList[0];
                for (int i = 1; i < data.lineDataList.Count; i++)
                {
                    bool isIntersect = GetIsIntersectsWith(beforeData, data.lineDataList[i]);
                    if (isIntersect)
                    {
                        data.lineDataList[i].groupIndex = groupIndex;
                    }
                    else
                    {
                        groupIndex++;
                    }

                    beforeData = data.lineDataList[i];


                    //Util.ShowLog("isIntersect : " + isIntersect.ToString() + " / " + data.lineDataList[i].lineString);
                }
            }


        }

        public void InitData(WinOCRResultData data)
        {
            //일단은 ocr 영역 1개로 처리한다는 가정하에 만든다.
            dataList = new List<ResultData>();

            ResultData resultData = new ResultData();
            resultData.index = 1;

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
                    // Util.ShowLog("words : " + data.words[count] + " rect : " + rect.ToString());

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
                //Util.ShowLog("Line string : " + lineString + " Rect : " + lineRect.ToString()  );

                lineData.lineString = lineString;

                resultData.lineDataList.Add(lineData);
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

            CalculateLines(resultData);
            dataList.Add(resultData);




        }
    }



}
