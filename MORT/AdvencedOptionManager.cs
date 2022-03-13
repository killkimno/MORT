using MORT.SettingData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public class HotKeyData
    {
        public int index;
        public KeyInputLabel.KeyType keyType;
        public List<Keys> keyList = new List<Keys>();
        public string extraData = "";
        public string keyResult = "";

        public HotKeyData()
        {

        }

        public HotKeyData(MORT.CustomControl.CtHotKey ctHotKey, string keyResult)
        {
            this.index = 0;
            this.keyType = ctHotKey.keyType;
            this.keyList = new List<Keys>();

            foreach (var obj in ctHotKey.GetKeys())
            {
                this.keyList.Add(obj);
            }

            this.keyResult = keyResult;
            this.extraData = "";
        }

        public HotKeyData(int index, KeyInputLabel.KeyType key, List<Keys> keyList, string keyResult, string extraData = "")
        {
            this.index = index;
            this.keyType = key;
            this.keyList = new List<Keys>();

            foreach(var obj in keyList)
            {
                this.keyList.Add(obj);
            }

            this.extraData = extraData;
            this.keyResult = keyResult;
        }

        public HotKeyData(int index, KeyInputLabel.KeyType key, string keyResult, string extraData = "")
        {
            this.index = index;
            this.keyType = key;
            this.keyList = new List<Keys>();
            this.keyResult = keyResult;

            char[] token = new char[] { ' ' };
            string[] keys = keyResult.Split(token, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Keys test = (Keys)System.Enum.Parse(typeof(Keys), keys[i]);
                    keyList.Add(test);
                }

            }
            catch
            {
                keyList.Clear();
                this.keyResult = "";
            }
          

            this.extraData = extraData;
           
        }

    }

    public class AdvencedOptionManager
    {
        public const string TEMP_USER_TRANSLATION_DB_FILE = @"TEMP_USER_TRANSLATION.txt";

        public const int OPEN_SETTING_MAX = 4;

        public const string KEY_HOTKEY_FORMAT = "@HOTKEY_{0} ";
        public const string KEY_HOTKEY_OPEN_SETTING = "@HOTKEY_OPEN_SETTING_{0} ";
        public const string KEY_HOTKEY_FILE_PATH = "@HOTKEY_OPEN_FILE_PATH_{0} ";

        private const string KEY_USE_TOP_MOST_WHEN_TRANSLATE = "@USE_TOP_MOST_OPTION_WHEN_TRANSLATE ";
        private const string KEY_USE_IGONORE_EMPTY_TRANSLATE = "@USE_IGONORE_EMPTY_TRANSLATE ";

        public const string KEY_FONT_AUTO_SIZE = "@OVERLAY_FONT_AUTO_SIZE ";
        public const string KEY_FONT_AUTO_MIN_SIZE = "@OVERLAY_FONT_AUTO_MIN_SIZE ";
        public const string KEY_FONT_AUTO_MAX_SIZE = "@OVERLAY_FONT_AUTO_MAX_SIZE ";

        public const string KEY_TRANSLATION_LIST = "@TRANSLATION_FILES ";   // <>로 해야한다
        public const string KEY_TRANSLATION_SEARCH_TYPE = "@TRANSLATION_SEARCH_TYPE ";      //번역집 검색 방식

        public const string KEY_TRANSLATOR_EXECUTIVE = "@TRANSLATOR_EXECUTIVE ";      //번역기 중역 사용

        public const string KEY_TRANSLATION_PARTIAL = "@TRANSLATION_PARTIAL ";              //번역집 부붕 검색
        public const string KEY_TRANSLATION_STRING_UPPER = "@TRANSLATION_STRING_UPPER ";    //번역집 대소문자 무시

        public const string KEY_DIC_REPROCESS_COUNT = "@DIC_REPROCESS_COUNT ";   //교정사전 추가 횟수

        public const string KEY_IS_USE_CLIPBOARD_TRANS = "@IS_USE_CLIPBOARD_TRANS ";
        public const string KEY_IS_SHOW_CLIPBOARD_ORIGINAL = "@IS_SHOW_CLIPBOARD_ORIGINAL ";
        public const string KEY_IS_SHOW_CLIPBOARD_PROICESSING = "@IS_SHOW_CLIPBOARD_PROICESSING ";  //클립보드 번역중 표시

        public const string KEY_ENABLE_SYSTEM_TRAY_MODE = "@ENABLE_SYSTEM_TRAY_MODE ";  //클립보드 번역중 표시

        public class Data
        {
            public List<ISettingDataParse> ParseList = new List<ISettingDataParse>();
            public ISettingData<bool> BoolSettingData;

            //고급 단축키
            public ISettingData<List<HotKeyData>> hotKeyList;

            //번역창
            public ISettingData<bool> UseAutoSizeFont;
            public ISettingData<int> MinAutoSizeFont;
            public ISettingData<int> MaxAutoSizeFont;

            /// <summary>
            /// 번역할 때 만 탑 모스트 적용
            /// </summary>
            public ISettingData<bool> UseTopMostOptionWhenTranslate;
            /// <summary>
            /// 빈 번역 결과 무시
            /// </summary>
            public ISettingData<bool> UseIgnoreEmptyTranslate;

            //번역집
            public ISettingData<List<string>> translationFileList;
            public ISettingData<bool> isTranslationDbStyle;

            public ISettingData<bool> TranslationStringUpper;

            //일본어 중역
            public ISettingData<bool> IsExecutive;

            //교정사전 추가 횟수
            public ISettingData<int> DicReProcessCount;

            //클립보드 번역 사용
            public ISettingData<bool> UseClipboardTrans;
            public ISettingData<bool> ShowClipboardOriginal;
            public ISettingData<bool> ShowClipboardProcessing;

            //앱 설정
            public ISettingData<bool> EnableSystemTrayMode;
        }

        public static Data data = new Data();

        public static bool EnableSystemTrayMode
        {
            set { data.EnableSystemTrayMode.Value = value; }
            get { return data.EnableSystemTrayMode.Value; }
        }

        public static bool IsExecutive
        {
            set { data.IsExecutive.Value = value; }
            get { return data.IsExecutive.Value; }
        }


        public static bool IsTranslationStringUpper
        {
            set { data.TranslationStringUpper.Value = value; }
            get { return data.TranslationStringUpper.Value ; }
        }

        public static List<string> TranslationFileList
        {
            set { data.translationFileList.Value = value; }
            get { return data.translationFileList.Value; }
        }

        public static bool IsTranslationDbStyle
        {
            set { data.isTranslationDbStyle.Value = value; }
            get { return data.isTranslationDbStyle.Value; }
        }

        public static bool UseTopMostOptionWhenTranslate => data.UseTopMostOptionWhenTranslate.Value;
        public static bool UseIgonoreEmptyTranslate => data.UseIgnoreEmptyTranslate.Value;

        public static bool IsAutoFontSize
        {
            get { return data.UseAutoSizeFont.Value; }
        }


        public static int MinAutoFontSize
        {
            get { return data.MinAutoSizeFont.Value; }
        }

        public static int MaxAutoFontSize
        {
            get { return data.MaxAutoSizeFont.Value; }
        }


        public static float GetResultAutoFontSize(float fontSize)
        {
            float result = fontSize;
            if(fontSize > data.MaxAutoSizeFont.Value)
            {
                result = data.MaxAutoSizeFont.Value;
            }
            else if(fontSize < data.MinAutoSizeFont.Value)
            {
                result = data.MinAutoSizeFont.Value;
            }


            return result;
        }

        public static void SetOverLay(bool isAutoSize, int minSize, int maxSize)
        {
            data.UseAutoSizeFont.Value = isAutoSize;
            data.MinAutoSizeFont.Value = minSize;
            data.MaxAutoSizeFont.Value = maxSize;
        }

        public static void SetTranslationFormSetting(bool useTopMostOptionWhenTranslate, bool igonoreEmptyTranslate)
        {
            data.UseTopMostOptionWhenTranslate.Value = useTopMostOptionWhenTranslate;
            data.UseIgnoreEmptyTranslate.Value = igonoreEmptyTranslate;
        }

        /// <summary>
        /// 일본어 중역 사용
        /// </summary>
        /// <param name="isUse"></param>
        public static void SetExecutive(bool isUse)
        {
            data.IsExecutive.Value = isUse;
        }

        public static int DicReProcessCount => data.DicReProcessCount.Value;

        public static void SetDicReProcessCount(int count)
        {
            if(count < 0)
            {
                count = 0;
            }
            else if(count > 3)
            {
                count = 3;
            }

            data.DicReProcessCount.Value = count;
        }

        //클립보드 설정
        public static bool IsUseClipboardTrans => data.UseClipboardTrans.Value;
        public static bool IsShowClipboardOriginal => data.ShowClipboardOriginal.Value;
        public static bool IsShowClipboardProcessing => data.ShowClipboardProcessing.Value;


        public static void SetClipboardTrans(bool isUse, bool isShowOriginal, bool isShowProcessing)
        {
            data.UseClipboardTrans.Value = isUse;
            data.ShowClipboardOriginal.Value = isShowOriginal;
            data.ShowClipboardProcessing.Value = isShowProcessing;
        }


        public static void SetHotKey(List<HotKeyData> list)
        {
            data.hotKeyList.Value.Clear();
            data.hotKeyList.Value = list;
        }

        public static List<HotKeyData> GetHotKeyList()
        {
            return data.hotKeyList.Value;
        }

        public static HotKeyData GetHotKeyResult(List<Keys> inputList)
        {
            HotKeyData result = null;

            for(int i = 0; i < data.hotKeyList.Value.Count; i++)
            {
                if(KeyInputLabel.GetResult(inputList, data.hotKeyList.Value[i].keyList))
                {
                    result = data.hotKeyList.Value[i];
                    break;
                }
            }
            return result;
        }


        //파일에서 초기화 -> 한 번만 한다.
        public static void Init()
        {
            data.ParseList.Clear();

            using (StreamReader r = Util.OpenFile(GlobalDefine.ADVENCED_SETTING_FILE))
            {
                if (r != null)
                {
                    string fileData = r.ReadToEnd();
                    if(fileData != "")
                    {                      

                        data.hotKeyList = SettingDataFactory.CreateList<HotKeyData, HotKeySettingData>("", data.ParseList);

                        LoadTranslationFormSetting();

                        //앱 설정
                        LoadAppSetting(fileData);

                        //번역집
                        LoadTranslationFileList(fileData);

                        //번역기
                        LoadTranslatorSetting(fileData);

                        //교정사전
                        LoadDicSetting(fileData);

                        //클립보드 설정
                        LoadClipboardSetting(fileData);

                      
                        foreach(var obj in data.ParseList)
                        {
                            obj.LoadValue(fileData);
                            Console.WriteLine(obj.ToSave());
                        }
                    }                    
                }

                r.Close();
            }

            if(!Directory.Exists(GlobalDefine.ADVENCED_TRANSRATION_PATH))
            {
                Directory.CreateDirectory(GlobalDefine.ADVENCED_TRANSRATION_PATH);
            }
        }

        private static void LoadTranslationFileList(string fileData)
        {
            data.translationFileList  = SettingDataFactory.CreateList<string, TranslationFileSettingData>(KEY_TRANSLATION_LIST, data.ParseList);
            data.isTranslationDbStyle = SettingDataFactory.Create<bool>(KEY_TRANSLATION_SEARCH_TYPE, data.ParseList, true);
            data.TranslationStringUpper = SettingDataFactory.Create<bool>(KEY_TRANSLATION_STRING_UPPER, data.ParseList, true);
        }

        private static void LoadTranslatorSetting(string fileData)
        {
            data.IsExecutive = SettingDataFactory.Create<bool>(KEY_TRANSLATOR_EXECUTIVE, data.ParseList, false);
        }

        private static void LoadDicSetting(string fileData)
        {
            data.DicReProcessCount = SettingDataFactory.Create<int>(KEY_DIC_REPROCESS_COUNT, data.ParseList, 0);
        }

        private static void LoadTranslationFormSetting()
        {
            data.UseAutoSizeFont = SettingDataFactory.Create<bool>(KEY_FONT_AUTO_SIZE, data.ParseList, false);
            data.MinAutoSizeFont = SettingDataFactory.Create<int>(KEY_FONT_AUTO_MIN_SIZE, data.ParseList, 10);
            data.MaxAutoSizeFont = SettingDataFactory.Create<int>(KEY_FONT_AUTO_MAX_SIZE, data.ParseList, 50);

            data.UseTopMostOptionWhenTranslate = SettingDataFactory.Create<bool>(KEY_USE_TOP_MOST_WHEN_TRANSLATE, data.ParseList, false);
            data.UseIgnoreEmptyTranslate = SettingDataFactory.Create<bool>(KEY_USE_IGONORE_EMPTY_TRANSLATE, data.ParseList, false);
        }
        private static void LoadAppSetting(string fileData)
        {
            data.EnableSystemTrayMode = SettingDataFactory.Create<bool>(KEY_ENABLE_SYSTEM_TRAY_MODE, data.ParseList, false);
        }

        private static void LoadClipboardSetting(string fileData)
        {
            data.UseClipboardTrans = SettingDataFactory.Create<bool>(KEY_IS_USE_CLIPBOARD_TRANS, data.ParseList, false);
            data.ShowClipboardOriginal = SettingDataFactory.Create<bool>(KEY_IS_SHOW_CLIPBOARD_ORIGINAL, data.ParseList, false); 
            data.ShowClipboardProcessing = SettingDataFactory.Create<bool>(KEY_IS_SHOW_CLIPBOARD_PROICESSING, data.ParseList, false);
        }


        public static void Reset()
        {
            foreach (var obj in data.ParseList)
            {
                obj.SetDefault();
            }
        }

        public static void Save()
        {
            string result = "";

           
            foreach(var obj in data.ParseList)
            {
                result += obj.ToSave();
            }

            Util.SaveFile(GlobalDefine.ADVENCED_SETTING_FILE, result);
        }

    }

}
