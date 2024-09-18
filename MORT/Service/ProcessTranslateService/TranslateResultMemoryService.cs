using MORT.Model;
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
        private int _remainSecond = 0;

        private List<TranslateMemoryModel> _memoryList = new List<TranslateMemoryModel>();

        public void Init(bool enable, int memoryCount, int remainSecond)
        {
            _memoryCount = memoryCount;
            _remainSecond = remainSecond;
            _useMemory = enable;
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

            RemoveExpiredMemory();

            if(_memoryList.Count >= _memoryCount)
            {
                _memoryList.RemoveAt(0);
            }

            bool containResult = true;
            if(_memoryList.Count == 0)
            {
                containResult = false;
                _memoryList.Add(new TranslateMemoryModel(result, DateTime.Now));
            }
            else if(_memoryList.Any(r => string.Compare(r.Result, result) != 0))
            {
                containResult = false;
                //내부에 포함되어 있으면 안 넣는다
                _memoryList.Add(new TranslateMemoryModel(result, DateTime.Now));
            }


            string memoryResult = "";
            if(containResult)
            {
                memoryResult = result + System.Environment.NewLine + System.Environment.NewLine;
            }

            for(int i = _memoryList.Count - 1; i >= 0; i--)
            {
                memoryResult += _memoryList[i].Result;
                if(1 != 0)
                {
                    memoryResult += System.Environment.NewLine + System.Environment.NewLine;
                }
            }

            return memoryResult;

        }

        private void RemoveExpiredMemory()
        {
            if(_remainSecond == 0)
            {
                return;
            }

            int expiredCount = 0;
            var expiredTime = DateTime.Now;
            for(int i = 0; i < _memoryList.Count; i++)
            {
                if(_memoryList[i].DtCreate.AddSeconds(_remainSecond) > expiredTime)
                {
                    break;
                }

                expiredCount++;
            }

            if(expiredCount > 0)
            {
                _memoryList.RemoveRange(0, expiredCount);
            }
        }
    }
}
