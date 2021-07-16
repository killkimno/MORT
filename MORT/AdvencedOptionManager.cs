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
        public const int OPEN_SETTING_MAX = 3;
        public const string KEY_HOTKEY_OPEN_SETTING = "@HOTKEY_OPEN_SETTING_{0} ";
        public const string KEY_HOTKEY_FILE_PATH = "@HOTKEY_OPEN_FILE_PATH_{0} ";
        public class Data
        {
            //고급 단축키
            public List<HotKeyData> hotKeyList = new List<HotKeyData>();
        }

        public static Data data = new Data();


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

                    }
                    
                }

                r.Close();
            }
        }

        public static void Save()
        {
            string result = "";


            for(int i = 0; i < data.hotKeyList.Count; i++)
            {
                if(data.hotKeyList[i].keyType == KeyInputLabel.KeyType.OpenSetting)
                {
                    string key = string.Format(KEY_HOTKEY_OPEN_SETTING, i.ToString());
                    string keyResult = data.hotKeyList[i].keyResult;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                    key = string.Format(KEY_HOTKEY_FILE_PATH, i.ToString());
                    keyResult = data.hotKeyList[i].extraData;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                }
            

            }
           

            Util.SaveFile(GlobalDefine.ADVENCED_SETTING_FILE, result);
        }




    }

}
