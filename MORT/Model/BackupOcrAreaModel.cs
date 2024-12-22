

using System.Collections.Generic;

namespace MORT.Model
{
    public class BackupOcrAreaModel
    {
        public BackupOcrAreaModel(List<int> xList, List<int> yList, List<int> sizeX, List<int> sizeY, List<int> exceptionX, List<int> exceptionY, List<int> exceptionSizeX, List<int> exceptionSizeY)
        {
            YList.AddRange(yList);
            XList.AddRange(xList);
            SizeX.AddRange(sizeX);
            SizeY.AddRange(sizeY);
            ExceptionX.AddRange(exceptionX);
            ExceptionY.AddRange(exceptionY);
            ExceptionSizeX.AddRange(exceptionSizeX);
            ExceptionSizeY.AddRange(exceptionSizeY);
        }

        public List<int> YList { get; } = new List<int>();
        public List<int> XList { get; } = new List<int>();
        public List<int> SizeX { get; } = new List<int>();
        public List<int> SizeY { get; } = new List<int>();
        public List<int> ExceptionX { get; } = new List<int>();
        public List<int> ExceptionY { get; } = new List<int>();
        public List<int> ExceptionSizeX { get; } = new List<int>();
        public List<int> ExceptionSizeY { get; } = new List<int>();
    }
}
