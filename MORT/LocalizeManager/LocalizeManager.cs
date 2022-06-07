
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.LocalizeManager
{
    public enum AppLanguage
    {
        Auto, Korea, English
    }
    public class LocalizeData
    {
        public LocalizeData(string key, string ko, string en)
        {
            Key = key;
            Ko = ko;
            En = en;
        }


        public string Key { get; }
        public string Ko { get; }
        public string En { get; }
    }

    public interface ILocalize
    {
        string GetLocalizeString(string key, string defaultText);
    }


    internal class LocalizeManager
    {
        private static AppLanguage Language = AppLanguage.Korea;
        private static List<LocalizeData> LocalizeDatas = new List<LocalizeData>();  
        public static void Init(string data, AppLanguage language)
        {
            Language = language;
            using (TextReader sr = new StringReader(data))
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    if(fields.Length >= 3)
                    {
                        LocalizeDatas.Add( new LocalizeData(fields[0], fields[1], fields[2]));
                    }
                }
            }
        }

        public static AppLanguage ConvertAppLanguage(string value)
        {
            if(value == "en")
            {
                return AppLanguage.English;
            }
            else
            {
                return AppLanguage.Korea;
            }
        }

        public static string GetLocalizeString(string key, string defaultText, AppLanguage appLanguage = AppLanguage.Auto)
        {
            var data = LocalizeDatas.First(r => r.Key == key);

            if(appLanguage == AppLanguage.Auto)
            {
                appLanguage = Language;
            }

            if(data == null)
            {
                return defaultText;
            }

            switch(appLanguage)
            {
                case AppLanguage.Korea:
                    return data.Ko;

                case AppLanguage.English:
                    return data.En;

                default:
                    return defaultText;
            }
        }
    }
}
