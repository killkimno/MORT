using System;

namespace MORT.Model
{
    internal class TranslateMemoryModel
    {
        public TranslateMemoryModel(string result, DateTime dtCreate)
        {
            Result = result;
            DtCreate = dtCreate;
        }

        public string Result { get; }
        public DateTime DtCreate { get; }
    }
}
