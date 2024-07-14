using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Service.ProcessTranslateService
{
    internal class TranslateResultMemoryService
    {
        private int _memoryCount = 0;
        private bool _useMemory;

        private List<string> _memoryList = new List<string>();

        public void Init(int memoryCount)
        {
            _memoryCount = memoryCount;
            
            _useMemory = memoryCount > 0;
        }

        public string CheckMemoryResult(string result)
        {
            if(!_useMemory || _memoryCount == 0)
            {
                return result;
            }

            if(string.IsNullOrEmpty(result))
            {
                return result;
            }

            if(_memoryList.Count >= _memoryCount)
            {
                _memoryList.RemoveAt(0);
            }

            bool containResult = true;
            if(_memoryList.Count == 0)
            {
                containResult = false;
                _memoryList.Add(result);
            }
            else if(_memoryList.Any(r => string.Compare(r, result) !=0))
            {
                containResult = false;
                //내부에 포함되어 있으면 안 넣는다
                _memoryList.Add(result);
            }
         

            string memoryResult = "";
            if(containResult)
            {                
                memoryResult = result + System.Environment.NewLine + System.Environment.NewLine;
            }

            for(int i = _memoryList.Count - 1; i >= 0; i--)
            {
                memoryResult += _memoryList[i];
                if(1 != 0)
                {
                    memoryResult += System.Environment.NewLine + System.Environment.NewLine;
                }
            }

            return memoryResult;

        }
    }
}
