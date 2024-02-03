using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Model.OCR
{
    public class EasyOcrResultModel
    {
        public string MainText { get; }
        public bool IsEmpty { get; }
        public int LineCount { get; }       //라인 수.
        public string[] Words;      //모든 문장.
        public double[] X { get; }             //x값들
        public double[] Y { get; }             //y값들
        public double[] SizeX { get; }         //size x;
        public double[] SizeY { get; }         //size y;
        public int[] WordCounts { get; }    //각 라인마다 워드 수.   쓰이나?
        public int WordsIndex { get; }

        public static EasyOcrResultModel Empty { get; } = new EasyOcrResultModel();

        public EasyOcrResultModel(List<List<(int x, int y)>> points, List<string> text)
        {
            if (text?.Count == 0)
            {
                return;
            }

            foreach (var line in text)
            {
                MainText += line + System.Environment.NewLine;
            }


            Words = text.ToArray();

            IsEmpty = string.IsNullOrEmpty(MainText);
            LineCount = text.Count;
            WordsIndex = 1;

            X = new double[LineCount];
            Y = new double[LineCount];
            SizeX = new double[LineCount];
            SizeY = new double[LineCount];
            WordCounts = new int[LineCount];

            for (int i = 0; i < points.Count; i++)
            {
                X[i] = points[i][0].x;
                Y[i] = points[i][0].y;

                SizeX[i] = (double)(points[i][1].x - points[i][0].x);
                SizeY[i] = (double)(points[i][3].y - points[i][0].y);

                WordCounts[i] = 1;
            }   
        }

        public EasyOcrResultModel()
        {
            IsEmpty = true;
            X = new double[0];
            Y = new double[0];
            SizeX = new double[0];
            SizeY = new double[0];
            WordCounts = new int[0];
        }
    }
}
