using System;
using System.Collections.Generic;
using System.IO;

namespace MORT.SettingData
{

    public interface ISettingDataParse
    {
        string ToSave();
        void LoadValue(string fileData);
        void SetDefault();
    }

    public interface ISettingData<T> : ISettingDataParse
    {
        T Value { get; set; }
    }

    public static class SettingDataFactory
    {
        public static ISettingData<T> Create<T>(string key, List<ISettingDataParse> parList, T defaultValue)
        {
            if (typeof(T) == typeof(bool))
            {
                var result = (ISettingData<T>)new BoolSettingData(key, defaultValue);
                parList.Add(result);
                return result;
            }
            else if (typeof(T) == typeof(int))
            {
                var result = (ISettingData<T>)new IntSettingData(key, defaultValue);
                parList.Add(result);
                return result;
            }
            else if (typeof(T) == typeof(string))
            {
                var result = (ISettingData<T>)new StringSettingData(key, defaultValue);
                parList.Add(result);
                return result;
            }

            throw new InvalidOperationException();
        }

        public static ISettingData<List<T>> CreateList<T, TData>(string key, List<ISettingDataParse> parList)
        {
            if (typeof(TData) == typeof(TranslationFileSettingData))
            {
                var result = (ISettingData<List<T>>)new TranslationFileSettingData(key);
                parList.Add(result);
                return result;
            }
            else if (typeof(TData) == typeof(HotKeySettingData))
            {
                var result = (ISettingData<List<T>>)new HotKeySettingData();
                parList.Add(result);
                return result;
            }

            throw new InvalidOperationException();
        }
    }

    public class BaseSettingData<T> : ISettingData<T>
    {
        public T Value { get; set; }
        protected T _defaultValue;
        protected string FileKey { get; set; }

        protected string ValueString;
        public virtual string ToSave()
        {
            return $"{FileKey}[{Value.ToString()}]{System.Environment.NewLine}";
        }     

        public virtual void SetDefault()
        {
            Value = _defaultValue;
        }

        public virtual void LoadValue(string fileData)
        {

        }
    }

    public class BoolSettingData : BaseSettingData<bool>
    {
        public BoolSettingData(string key, object defaultValue)
        {
            _defaultValue = (bool)defaultValue;
            FileKey = key;
        }

        public override void LoadValue(string fileData)
        {
            bool result;
            if (Util.TryParseBool(out result, fileData, FileKey))
            {
                Value = result;
            }
            else
            {
                Value = _defaultValue;
            }

            ValueString = Value.ToString();
        }
    }

    public class IntSettingData : BaseSettingData<int>
    {
        public IntSettingData(string key, object defaultValue)
        {
            FileKey = key;
            _defaultValue = (int)defaultValue;
        }

        public override void LoadValue(string fileData)
        {
            int result;
            if(Util.TryParseInt(out result, fileData, FileKey))
            {
                Value = result;
            }
            else
            {
                Value = _defaultValue;
            }

            ValueString = Value.ToString();
        }
    }

    public class StringSettingData : BaseSettingData<string>
    {
        public StringSettingData(string key, object defaultValue)
        {
            FileKey = key;
            _defaultValue = (string)defaultValue;
        }

        public override void LoadValue(string fileData)
        {
            string result;
            if (Util.TryParseString(out result, fileData, FileKey))
            {
                Value = result;
            }
            else
            {
                Value = _defaultValue;
            }

            ValueString = Value.ToString();
        }
    }

    public class TranslationFileSettingData : BaseSettingData<List<string>>
    {
        public TranslationFileSettingData(string key)
        {
            Value = new List<string>();
            FileKey = key;
        }

        public override void LoadValue(string fileData)
        {
            string sep = "\t";
            string content = Util.ParseString(fileData, FileKey, '<', '>');
            string[] keys = content.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Value.Clear();
            foreach (var obj in keys)
            {
                //파일이 존재하는 것 만 넣는다.
                if (File.Exists(GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt"))
                {
                    Value.Add(obj);
                }
            }
        }

        public override string ToSave()
        {
            string result = "";
            if (Value.Count > 0)
            {
                string files = FileKey + "<";

                for (int i = 0; i < Value.Count; i++)
                {
                    files += Value[i];

                    if (i + 1 != Value.Count)
                    {
                        files += "\t";
                    }
                }

                files += ">";

                result += files;
            }

            return result;
        }

        public override void SetDefault()
        {
            Value.Clear();
        }


    }

    public class HotKeySettingData : BaseSettingData<List<HotKeyData>>
    {
        public const int OPEN_SETTING_MAX = 4;

        public const string KEY_HOTKEY_FORMAT = "@HOTKEY_{0}_{1} ";
        public const string KEY_HOTKEY_EXTRA_VALUE = "@HOTKEY_EXTRA_VALUE_{0}_{1} ";

        public HotKeySettingData()
        {
            Value = new List<HotKeyData>();
            FileKey = "";
        }

        public override void LoadValue(string fileData)
        {
            //설정파일 단축키 가져오기
            for (int i = 0; i < OPEN_SETTING_MAX; i++)
            {
                LoadHotkey(i, KeyInputLabel.KeyType.OpenSetting, fileData, KEY_HOTKEY_EXTRA_VALUE);
            }

            //DBTranslate, NaverTranslate, GoogleTranslate, GoogleSheetTranslate, EzTrans
            LoadHotkey(0, KeyInputLabel.KeyType.LayerTransparency, fileData);

            LoadHotkey(0, KeyInputLabel.KeyType.DBTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.NaverTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.GoogleTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.GoogleSheetTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.EzTrans, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.DeepL, fileData);
        }

        public override string ToSave()
        {
            string result = "";

            for (int i = 0; i < Value.Count; i++)
            {
                string key = string.Format(KEY_HOTKEY_FORMAT, Value[i].index, Value[i].keyType.ToString());
                string keyResult = Value[i].keyResult;

                result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                if(Value[i].extraData != "")
                {
                    key = string.Format(KEY_HOTKEY_EXTRA_VALUE, i.ToString(), Value[i].keyType);
                    keyResult = Value[i].extraData;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;
                }
            }

            return result;
        }

        private void LoadHotkey(int index, KeyInputLabel.KeyType keyType, string fileData, string extraKey = "")
        {

            string hotKey = string.Format(KEY_HOTKEY_FORMAT,index, keyType);
            string keyResult = Util.GetNextLine(fileData, hotKey);

            string extraFileKey = "";
            string extraFileResult = "";

            if (extraKey != "")
            {
                extraFileKey = string.Format(extraKey, index.ToString(), keyType);
                extraFileResult = Util.GetNextLine(fileData, extraFileKey);
            }

            HotKeyData hotKeyData = new HotKeyData(index, keyType, keyResult, extraFileResult);
            Value.Add(hotKeyData);

        }

        public override void SetDefault()
        {
            Value.Clear();
        }

    }


}
