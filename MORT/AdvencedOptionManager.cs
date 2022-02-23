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
            public List<HotKeyData> hotKeyList = new List<HotKeyData>();

            //번역창
            public bool isUseAutoSizeFont = false;
            public int minAutoSizeFont = 10;
            public int maxAutoSizeFont = 50;

            //번역집
            public List<string> translationFileList = new List<string>();
            public bool isTranslationDbStyle = false;

            public bool isTranslationStringUpper = true;

            //일본어 중역
            public bool IsExecutive = false;

            //교정사전 추가 횟수
            public int DicReProcessCount = 0;

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
            set { data.IsExecutive = value; }
            get { return data.IsExecutive; }
        }


        public static bool IsTranslationStringUpper
        {
            set { data.isTranslationStringUpper = value; }
            get { return data.isTranslationStringUpper; }
        }

        public static List<string> TranslationFileList
        {
            set { data.translationFileList = value; }
            get { return data.translationFileList; }
        }

        public static bool IsTranslationDbStyle
        {
            set { data.isTranslationDbStyle = value; }
            get { return data.isTranslationDbStyle; }
        }

        public static bool IsAutoFontSize
        {
            get { return data.isUseAutoSizeFont; }
        }

        public static int MinAutoFontSize
        {
            get { return data.minAutoSizeFont; }
        }

        public static int MaxAutoFontSize
        {
            get { return data.maxAutoSizeFont; }
        }


        public static float GetResultAutoFontSize(float fontSize)
        {
            float result = fontSize;
            if(fontSize > data.maxAutoSizeFont)
            {
                result = data.maxAutoSizeFont;
            }
            else if(fontSize < data.minAutoSizeFont)
            {
                result = data.minAutoSizeFont;
            }


            return result;
        }

        public static void SetOverLay(bool isAutoSize, int minSize, int maxSize)
        {
            data.isUseAutoSizeFont = isAutoSize;
            data.minAutoSizeFont = minSize;
            data.maxAutoSizeFont = maxSize;

        }

        /// <summary>
        /// 일본어 중역 사용
        /// </summary>
        /// <param name="isUse"></param>
        public static void SetExecutive(bool isUse)
        {
            data.IsExecutive = isUse;
        }

        public static int DicReProcessCount => data.DicReProcessCount;

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

            data.DicReProcessCount = count;
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
            data.hotKeyList.Clear();

            foreach(var obj in list)
            {
                data.hotKeyList.Add(obj);
            }
        }

        public static List<HotKeyData> GetHotKeyList()
        {
            return data.hotKeyList;
        }

        public static HotKeyData GetHotKeyResult(List<Keys> inputList)
        {
            HotKeyData result = null;

            for(int i = 0; i < data.hotKeyList.Count; i++)
            {
                if(KeyInputLabel.GetResult(inputList, data.hotKeyList[i].keyList))
                {
                    result = data.hotKeyList[i];
                    break;
                }
            }
            return result;
        }

        private static void LoadHotkey(KeyInputLabel.KeyType keyType, string fileData)
        {

            string hotKey = string.Format(KEY_HOTKEY_FORMAT, keyType);
            string keyResult = Util.GetNextLine(fileData, hotKey);

            HotKeyData hotKeyData = new HotKeyData(0, keyType, keyResult);
            data.hotKeyList.Add(hotKeyData);

        }

        //파일에서 초기화 -> 한 번만 한다.
        public static void Init()
        {
            using (StreamReader r = Util.OpenFile(GlobalDefine.ADVENCED_SETTING_FILE))
            {
                if (r != null)
                {
                    string fileData = r.ReadToEnd();
                    if(fileData != "")
                    {
                        //설정파일 단축키 가져오기
                        for (int i = 0; i < OPEN_SETTING_MAX; i++)
                        {
                            string key = string.Format(KEY_HOTKEY_OPEN_SETTING, i.ToString());
                            string result = Util.GetNextLine(fileData, key);


                            string fileKey = string.Format(KEY_HOTKEY_FILE_PATH, i.ToString());
                            string fileResult = Util.GetNextLine(fileData, fileKey);


                            HotKeyData hotKeyData = new HotKeyData(i, KeyInputLabel.KeyType.OpenSetting, result, fileResult);
                            data.hotKeyList.Add(hotKeyData);

                        }

                        LoadHotkey(KeyInputLabel.KeyType.LayerTransparency, fileData);

                        data.isUseAutoSizeFont = Util.ParseBool(fileData, KEY_FONT_AUTO_SIZE);
                        data.minAutoSizeFont = Util.ParseInt(fileData, KEY_FONT_AUTO_MIN_SIZE);
                        data.maxAutoSizeFont = Util.ParseInt(fileData, KEY_FONT_AUTO_MAX_SIZE);

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
            string sep = "\t";
            string content = Util.ParseString(fileData, KEY_TRANSLATION_LIST, '<', '>');
            string[] keys = content.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            data.translationFileList.Clear();
            foreach (var obj in keys)
            {
                //파일이 존재하는 것 만 넣는다.
                if(File.Exists(GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt"))
                {
                    data.translationFileList.Add(obj);
                }
            }

            data.isTranslationDbStyle = Util.ParseBool(fileData, KEY_TRANSLATION_SEARCH_TYPE);
            data.isTranslationStringUpper = Util.ParseBool(fileData, KEY_TRANSLATION_STRING_UPPER);

        }

        private static void LoadTranslatorSetting(string fileData)
        {
            data.IsExecutive = Util.ParseBool(fileData, KEY_TRANSLATOR_EXECUTIVE);
        }

        private static void LoadDicSetting(string fileData)
        {
            data.DicReProcessCount = Util.ParseInt(fileData, KEY_DIC_REPROCESS_COUNT);
        }

        private static void LoadAppSetting(string fileData)
        {
            data.EnableSystemTrayMode = SettingDataFactory.Create<bool>(KEY_ENABLE_SYSTEM_TRAY_MODE, data.ParseList);
        }

        private static void LoadClipboardSetting(string fileData)
        {
            data.UseClipboardTrans = SettingDataFactory.Create<bool>(KEY_IS_USE_CLIPBOARD_TRANS, data.ParseList);
            data.ShowClipboardOriginal = SettingDataFactory.Create<bool>(KEY_IS_SHOW_CLIPBOARD_ORIGINAL, data.ParseList); 
            data.ShowClipboardProcessing = SettingDataFactory.Create<bool>(KEY_IS_SHOW_CLIPBOARD_PROICESSING, data.ParseList);
        }


        public static void Reset()
        {
            data.EnableSystemTrayMode.Value = false;

            data.hotKeyList.Clear();
            data.isUseAutoSizeFont =false;
            data.minAutoSizeFont = 10;
            data.maxAutoSizeFont = 50;

            data.translationFileList.Clear();
            data.isTranslationDbStyle = true;
            data.isTranslationStringUpper = true;

            data.IsExecutive = false;
            data.DicReProcessCount = 0;

            data.ShowClipboardOriginal.Value = false;
            data.UseClipboardTrans.Value = false;

        }

        public static void Save()
        {
            string result = "";


            for (int i = 0; i < data.hotKeyList.Count; i++)
            {
                if (data.hotKeyList[i].keyType == KeyInputLabel.KeyType.OpenSetting)
                {
                    string key = string.Format(KEY_HOTKEY_OPEN_SETTING, i.ToString());
                    string keyResult = data.hotKeyList[i].keyResult;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                    key = string.Format(KEY_HOTKEY_FILE_PATH, i.ToString());
                    keyResult = data.hotKeyList[i].extraData;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                }
                else
                {
                    string key = string.Format(KEY_HOTKEY_FORMAT, data.hotKeyList[i].keyType.ToString());
                    string keyResult = data.hotKeyList[i].keyResult;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;
                }

            }


            result += KEY_FONT_AUTO_SIZE + '[' + data.isUseAutoSizeFont.ToString() + "]" + System.Environment.NewLine;
            result += KEY_FONT_AUTO_MIN_SIZE + '[' + data.minAutoSizeFont.ToString() + "]" + System.Environment.NewLine;
            result += KEY_FONT_AUTO_MAX_SIZE + '[' + data.maxAutoSizeFont.ToString() + "]" + System.Environment.NewLine;


            if(data.translationFileList.Count > 0)
            {
                string files = KEY_TRANSLATION_LIST + "<";

                for (int i = 0; i < data.translationFileList.Count; i++)
                {
                    files += data.translationFileList[i];

                    if(i+1 != data.translationFileList.Count)
                    {
                        files += "\t";
                    }
                }

                files += ">";

                result += files;
            }


            result += KEY_TRANSLATION_SEARCH_TYPE + '[' + data.isTranslationDbStyle.ToString() + ']' + System.Environment.NewLine;
            result += KEY_TRANSLATION_STRING_UPPER + '[' + data.isTranslationStringUpper.ToString() + ']' + System.Environment.NewLine;


            result += KEY_TRANSLATOR_EXECUTIVE + '[' + data.IsExecutive.ToString() + ']' + System.Environment.NewLine;

            //교정사전
            result += KEY_DIC_REPROCESS_COUNT + '[' + data.DicReProcessCount.ToString() + ']' + System.Environment.NewLine;

            foreach(var obj in data.ParseList)
            {
                result += obj.ToSave();
            }

            Util.SaveFile(GlobalDefine.ADVENCED_SETTING_FILE, result);
        }




    }

}
