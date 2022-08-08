
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
        public static AppLanguage Language { get; private set; } = AppLanguage.Korea;
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

                    if(fields.Length >= 4)
                    {
                        LocalizeDatas.Add( new LocalizeData(fields[1], fields[2], fields[3]));
                    }
                }
            }
        }

        public static AppLanguage ConvertAppLanguage(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                CultureInfo ci = CultureInfo.CurrentUICulture;
                value = ci.TwoLetterISOLanguageName;
            }

            if(value == "ko")
            {
                return AppLanguage.Korea;           
            }
            else
            {
                return AppLanguage.English;
            }
        }

        public static string GetLocalizeString(string key, string defaultText = "", AppLanguage appLanguage = AppLanguage.Auto)
        {
            var data = LocalizeDatas.FirstOrDefault(r => r.Key == key);

            if(appLanguage == AppLanguage.Auto)
            {
                appLanguage = Language;
            }

            if(data == null)
            {
                return key;
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
