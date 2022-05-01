using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudVision
{
    public struct GoogleOcrResult
    {
        public bool Main;
        public string Text;
        public int SizeX;
        public int SizeY;
        public int X;
        public int Y;
    }
}
