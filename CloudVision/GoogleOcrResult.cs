using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudVision
{
    public struct GoogleOcrResult
    {
        public string MainText;
        public bool isEmpty;
        public int lineCount;       //라인 수.
        public string[] words;      //모든 문장.
        public double[] x;             //x값들
        public double[] y;             //y값들
        public double[] sizeX;         //size x;
        public double[] sizeY;         //size y;
        public int[] wordCounts;    //각 라인마다 워드 수.
        public int wordsIndex;

    }
}
