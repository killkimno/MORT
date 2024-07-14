using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Model
{
    internal class TranslateResultModel
    {
        public TranslateResultModel(string ocr, string result, DateTime dtCreate)
        {
            Ocr = ocr;
            Result = result;
            DtCreate = dtCreate;
        }

        public string Ocr { get; }
        public string Result { get; }
        public DateTime DtCreate { get; }
    }
}
