using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MORT
{

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;

        }
    }

    public enum FileStepType
    {
        None,
        OCR,
        Trans,
        End,
    }

    public enum UpdateType
    {
        None, Major, Minor, RequireRelease,
    }

    class GlobalDefine
    {
        public enum TesseractLanguageType
        {
            English = 0, Japen = 1, ETC = 2,
        }

        public const int GoogleOcrLimit = 900;
        public const string GOOGLE_OCR_PATH_FILE = @"UserData/googleOcrJsonPath.txt";

        public const string CUSTOM_TRANSCODE_FILE = @"UserData/UserTransCode.txt";
        public const string GOOGLE_ACCOUNT_FILE = @"UserData/googleAccount.txt";
        public const string NAVER_ACCOUNT_FILE = @"UserData/naverAccount.txt";
        public const string HOTKEY_FILE = @"UserData/hotKeySetting_v2.txt";
        public const string HOTKEY_FILE_OLD_V2 = @"UserData/hotKeySetting.txt";
        public const string HOTKEY_FILE_OLD = @"UserData/hotKeyStting.txt";
        public const string CHECK_UPDATE_FILE = @"UserData/checkUpdate.txt";
        public const string ADVENCED_SETTING_FILE = @"UserData/AdvencedOptionSetting.txt";
        public const string ADVENCED_TRANSRATION_PATH = @"UserData/TranslationFiles/";
        public const string USER_OPTION_SETTING_FILE = @"UserData/UserOptionSetting.txt";
        public const string USER_SETTING_FILE = @"UserData/setting.conf";   // SaveSetting(@".\\setting\\setting.conf");
        public const string DATA_VERSION_FILE = @"VersionData.txt";



        public const string FORMER_TRANS_FILE = @"UserData/transResult_{0}.txt";   //string.format을 써야 함


        public const string SETTING_PATH = @"setting/";
        public const string DB_PATH = @"DB/";


        public static string SPLITE_TOEKN
        {
            get
            {
                return "//////" + System.Environment.NewLine;
            }
        }

        public static string SPLITE_TOEKN_NAVER = "//////";
        public static string SPLITE_TOEKN_GOOGLE = "//////";
        public static string SPLITE_TOEKN_DEEPL = "@@@@@@";
        public static bool IS_USE_ADVENCED_TOKEN = false;

        public static string DeeplFrontUrl = "https://www.deepl.com/translator#en/ko/tank%20divsion";
        public static string DeeplFormat = "https://www.deepl.com/translator#{0}/{1}/{2}";
        public static string DeeplElementTarget = "document.getElementsByClassName(\"lmt__textarea lmt__textarea_dummydiv\")[1].innerHTML";


    }

    class Util
    {
        public static List<string> toolTipList = new List<string>();

        public static float dpiX = -1;
        public static float dpiY = -1;
        public static float dpiMulti = 1;
        public const float BASE_DPI = 96;

        public const int OCR_FORM_BORDER = 3;
        public const int OCR_FORM_SECOND_BORDER = 8;
        public const int OCR_FORM_TITLEBAR = 20;

        public static int ocrFormBorder = 3;
        public static int ocrformSecondBorder = 8;
        public static int ocrFormTitleBar = 20;
        public static int ocrFormMAX = 31;

        public static bool isInittoolTip = false;


        public static Color ParseColor(string color, Color defaultColor)
        {
            var keys = color.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if(keys.Length >= 3)
            {
                Int32.TryParse(keys[0], out var r);
                Int32.TryParse(keys[1], out var g);
                Int32.TryParse(keys[2], out var b);
                int a = 255;
                if(keys.Length >= 4)
                {
                    Int32.TryParse(keys[3], out a);
                }

                return Color.FromArgb(a, r, g, b);
            }
            else
            {
                return defaultColor;
            }
        }

        public static string ColorToString(Color color)
        {
            return $"{color.R},{color.G},{color.B},{color.A}";
        }

        public static void OpenURL(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {

            }

        }
        public static int GetScreenTopPosition()
        {
            int screenTop = 0;
            foreach(Screen s in Screen.AllScreens)
            {
                //r = Rectangle.Union(r, s.Bounds);
                if(s.Bounds.Top < screenTop)
                {
                    screenTop = s.Bounds.Top;
                }

            }

            return screenTop;
        }

        public static bool GetIsEqualWinCode(string code, string target)
        {
            bool isEqual = false;

            if(code == "en" || code == "en-US")
            {
                if(target == "en" || target == "en-US")
                {
                    isEqual = true;
                }
            }
            else
            {
                if(code.Equals(target))
                {
                    isEqual = true;
                }
            }

            return isEqual;
        }

        public static string GetSpliteToken(SettingManager.TransType transType)
        {
            string token = "";

            switch(transType)
            {
                case SettingManager.TransType.google:
                case SettingManager.TransType.google_url:
                    token = GlobalDefine.SPLITE_TOEKN_GOOGLE;
                    break;
                case SettingManager.TransType.naver:
                case SettingManager.TransType.papago_web:
                    token = GlobalDefine.SPLITE_TOEKN_NAVER;
                    break;

                case SettingManager.TransType.deepl:
                    token = GlobalDefine.SPLITE_TOEKN_DEEPL;
                    break;

                default:
                    token = GlobalDefine.SPLITE_TOEKN_NAVER;
                    break;
            }

            token += System.Environment.NewLine;
            return token;
        }

        public static string[] GetSpliteByToken(string result, SettingManager.TransType transType)
        {
            string[] lines = null;

            //토큰의 길이를 가져온다
            // - 3의 길이를 가져온다 단 길이가 6이면 -2만 한다
            //토큰 첫 번째 문자를 가져온다.
            //토큰 첫번째 문자가 연속으로 -3 이상의 길이가 나오고, 줄바꿈을 만날경우 토큰 문자열로 인식한다
            //
            string token = GetSpliteToken(transType);
            string newToken = token;

            bool isUseAdvenced = false;

            if(GlobalDefine.IS_USE_ADVENCED_TOKEN)
            {
                if(token.Length >= 7)
                {
                    newToken = token.Remove(0, 3);
                    isUseAdvenced = true;
                }
                else if(token.Length >= 6)
                {
                    newToken = token.Remove(0, 2);
                    isUseAdvenced = true;
                }
            }


            string[] tokens = { newToken };
            lines = result.Split(tokens, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> resultLines = new List<string>(lines.Length);
            if(isUseAdvenced)
            {
                char firstToken = token[0];

                for(int i = 0; i < lines.Length; i++)
                {
                    int removeCount = 0;

                    if(lines[i].Length >= 1)
                    {
                        //앞 부분 처리
                        if(lines[i][0] == firstToken)
                        {
                            for(int j = 0; j < lines[i].Length; j++)
                            {
                                if(lines[i][j] == firstToken)
                                {
                                    removeCount++;
                                }
                                else
                                {
                                    if(removeCount != -0)
                                    {
                                        lines[i] = lines[i].Remove(0, removeCount);
                                    }

                                    break;
                                }
                            }

                            if(removeCount == lines[i].Length)
                            {
                                lines[i] = "";
                            }
                        }


                        if(lines[i].Length > 0)
                        {
                            //뒷 부분 처리
                            if(lines[i][lines[i].Length - 1] == firstToken)
                            {
                                for(int j = lines[i].Length - 1; j >= 0; j--)
                                {
                                    if(lines[i][j] == firstToken)
                                    {
                                        removeCount++;
                                    }
                                    else
                                    {
                                        if(removeCount != 0)
                                        {
                                            lines[i] = lines[i].Remove(lines[i].Length - removeCount, removeCount);
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }


                    if(lines[i].Length != 0)
                    {
                        resultLines.Add(lines[i]);
                    }

                }

                lines = resultLines.ToArray();

            }

            return lines;
        }

        public static void SetSpliteToken(string naver, string google, string useAdvenced)
        {
            GlobalDefine.SPLITE_TOEKN_NAVER = naver;
            GlobalDefine.SPLITE_TOEKN_GOOGLE = google;

            try
            {
                bool isUseAdvenced = false;
                if(!string.IsNullOrEmpty(useAdvenced))
                {
                    isUseAdvenced = bool.Parse(useAdvenced);
                }

                GlobalDefine.IS_USE_ADVENCED_TOKEN = isUseAdvenced;

            }
            catch
            {
                GlobalDefine.IS_USE_ADVENCED_TOKEN = false;
            }
        }

        /// <summary>
        /// 업데이트 타입을 가져온다.
        /// </summary>
        /// <param name="nowVersion"></param>
        /// <param name="newVersion"></param>
        /// <param name="minorVersion"></param>
        /// <returns></returns>
        public static UpdateType GetUpdateType(int nowVersion, string newVersion, string minorVersion)
        {
            UpdateType result = UpdateType.None;

            if(nowVersion < Convert.ToInt32(newVersion))
            {
                //TODO : 마이너 버전 버전이 신규 버전 보다 높은지 체크해야 한다.
                int minMinor = 0;
                int maxMinor = 0;

                string[] keys = minorVersion.Split('-');

                if(keys != null && keys.Length >= 2)
                {
                    minMinor = Convert.ToInt32(keys[0]);
                    maxMinor = Convert.ToInt32(keys[1]);

                    if(maxMinor < Convert.ToInt32(newVersion))
                    {
                        result = UpdateType.Major;
                    }
                    else if(minMinor <= nowVersion && nowVersion <= maxMinor)
                    {
                        //마이너 업데이트
                        result = UpdateType.Minor;
                    }
                    else
                    {
                        //메이저 업데이트
                        result = UpdateType.Major;
                    }
                }
                else
                {
                    result = UpdateType.Major;
                }
            }


            return result;
        }

        /// <summary>
        /// 값 체크를 한다
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="startKey"></param>
        /// <param name="endKey"></param>
        /// <returns></returns>
        public static bool TryParseBool(out bool result, string data, string key, char startKey = '[', char endKey = ']')
        {
            string parseResult = ParseString(data, key, startKey, endKey);

            if(string.IsNullOrEmpty(parseResult))
            {
                result = false;

                return false;
            }

            Boolean.TryParse(parseResult, out result);

            return true;
        }

        public static bool TryParseString(out string result, string data, string key, char startKey = '[', char endKey = ']')
        {
            string parseResult = ParseString(data, key, startKey, endKey);

            if(string.IsNullOrEmpty(parseResult))
            {
                result = "";

                return false;
            }

            result = parseResult;

            return true;
        }

        public static bool TryParseInt(out int result, string data, string key, char startKey = '[', char endKey = ']')
        {
            string parseResult = ParseString(data, key, startKey, endKey);

            if(string.IsNullOrEmpty(parseResult))
            {
                result = -1;
                return false;
            }

            int.TryParse(parseResult, out result);

            return true;
        }

        public static bool ParseBool(string data, string key, char startKey = '[', char endKey = ']')
        {
            bool isResult = false;

            string result = ParseString(data, key, startKey, endKey);

            Boolean.TryParse(result, out isResult);

            return isResult;
        }

        public static int ParseInt(string data, string key, char startKey = '[', char endKey = ']')
        {
            int value = -1;

            string result = ParseString(data, key, startKey, endKey);

            int.TryParse(result, out value);

            return value;
        }

        /// <summary>
        /// 특정 문장에서 특정 규칙으로 데이터 가져오기
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="startKet"></param>
        /// <param name="endKey"></param>
        /// <returns></returns>
        public static string ParseString(string data, string key, char startKey = '[', char endKey = ']')
        {
            string result = "";

            int point = data.LastIndexOf(key, StringComparison.InvariantCulture);
            if(point != -1)
            {
                point += key.Length;
            }
            else
            {
                return "";
            }

            bool isSatrt = false;



            for(int i = point; i < data.Length; i++)
            {
                if(!isSatrt)
                {
                    if(data[i] == startKey)
                    {
                        isSatrt = true;
                    }
                }
                else
                {
                    if(data[i] == endKey)
                    {
                        isSatrt = true;
                        break;
                    }
                    else
                    {
                        result = result + data[i];
                    }
                }

            }

            return result;
        }


        public static string GetNextLine(string data, string key)
        {
            string result = "";

            using(StringReader reader = new StringReader(data))
            {
                string line = "";
                while(true)
                {
                    line = reader.ReadLine();
                    if(line != null)
                    {
                        if(line.Equals(key, StringComparison.InvariantCulture))
                        {
                            line = reader.ReadLine();

                            if(line != null)
                            {
                                result = line;
                            }

                            break;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }


            return result;
        }


        public static string ParseStringFromFile(string file, string key, char startKey = '[', char endKey = ']')
        {

            string result = "";
            using(StreamReader r = Util.OpenFile(file))
            {
                if(r != null)
                {
                    string data = r.ReadToEnd();

                    result = Util.ParseString(data, key, startKey, endKey);
                }

                r.Close();
            }


            return result;
        }

        public static StreamReader OpenFile(string file)
        {
            StreamReader r = null;
            try
            {
                if(!File.Exists(file))
                {
                    using(System.IO.FileStream fs = System.IO.File.Create(file))
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                r = new StreamReader(file);



            }
            catch(FileNotFoundException)
            {

            }

            return r;
        }

        public static void SaveFile(string file, string data, bool isAppend = false)
        {
            try
            {
                if(!File.Exists(file))
                {
                    using(System.IO.FileStream fs = System.IO.File.Create(file))
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
                Encoding utf8WithoutBom = new UTF8Encoding(false);
                using(StreamWriter newTask = new StreamWriter(file, isAppend, utf8WithoutBom))
                {

                    newTask.Write(data, StringComparison.InvariantCulture);
                    newTask.Close();
                }

            }
            catch
            {

            }
        }



        public static void ChangeFileData(string file, string key, string value, char startKey = '[', char endKey = ']')
        {
            Util.ShowLog("Change File : " + file + " key : " + key + " value : " + value);
            using(StreamReader r = Util.OpenFile(file))
            {
                string data = r.ReadToEnd();

                r.Close();
                r.Dispose();

                int point = data.LastIndexOf(key, StringComparison.InvariantCulture);
                if(point != -1)
                {
                    point += key.Length;
                }

                bool isSatrt = false;
                int startPoint = -1;
                int endPoint = -1;

                if(point != -1)
                {
                    for(int i = point; i < data.Length; i++)
                    {
                        if(!isSatrt)
                        {
                            if(data[i] == startKey)
                            {
                                isSatrt = true;
                                startPoint = i;
                            }
                        }
                        else
                        {
                            if(data[i] == endKey)
                            {
                                isSatrt = true;
                                endPoint = i;
                                break;
                            }
                        }
                    }
                }


                if(startPoint != -1 && endPoint != -1)
                {
                    data = data.Remove(startPoint, endPoint - startPoint + 1);
                    data = data.Insert(startPoint, startKey + value + endKey);

                    Util.SaveFile(file, data);
                }
                else
                {
                    //새로 만들어서 넣어야 한다.
                    string line = key + " " + startKey + value + endKey;
                    data = line + System.Environment.NewLine + data;
                    Util.SaveFile(file, data);
                }
            }
        }


        private static DateTime dtCheck = DateTime.MinValue;

        public static void CheckTimeSpan(bool isStart)
        {
            if(!isStart)
            {
                DateTime dtNow = DateTime.Now;
                TimeSpan span = new TimeSpan(dtNow.Ticks - dtCheck.Ticks);

                ShowLog("Check Result = " + span.ToString() + " / " + dtNow.Ticks + " / " + dtCheck.Ticks);
            }
            else
            {
                dtCheck = DateTime.Now;
            }
        }

        public static void ShowLog(string log)
        {
            Console.WriteLine(log);
        }

        public static void SetDPI(float dpiX, float dpiY)
        {
            Util.dpiX = dpiX;
            Util.dpiY = dpiY;

            dpiMulti = GetDpiMulti();

            ocrFormBorder = (int)(OCR_FORM_BORDER * dpiMulti);
            ocrformSecondBorder = (int)(OCR_FORM_SECOND_BORDER * dpiMulti);
            ocrFormTitleBar = (int)(OCR_FORM_TITLEBAR * dpiMulti);

            ocrFormMAX = ocrFormBorder + ocrformSecondBorder + ocrFormTitleBar;
        }

        public static int GetBorderWidth()
        {
            int result = 0;

            result = (int)(SystemInformation.FrameBorderSize.Width * GetDpiMulti());


            return result;
        }


        public static int GetTitlebarHeight()
        {
            int result = 0;

            result = (int)((SystemInformation.CaptionHeight + GetBorderWidth()) * GetDpiMulti());


            return result;
        }

        public static float GetDpiMulti()
        {
            float result = 1;

            result = dpiX / BASE_DPI;

            return result;
        }

        //툴팁 관련.
        public static string GetToolTip()
        {
            if(!isInittoolTip)
            {
                toolTipList = new List<string>();
                string[] tool = Properties.Settings.Default.TOOLTIP_LIST.Split(',');

                for(int i = 0; i < tool.Length; i++)
                {
                    toolTipList.Add(tool[i]);
                    Util.ShowLog(tool[i]);
                }

                isInittoolTip = true;
            }

            string result = "";

            if(toolTipList.Count > 0)
            {
                Random r = new Random();
                int rand = r.Next(0, toolTipList.Count - 1);

                if(rand < toolTipList.Count)
                {
                    result = toolTipList[rand];
                }
            }


            return result;
        }

        #region ::::::::: DB 관련 ::::::::::

        public static void SaveDBFile(string path, Dictionary<string, string> dic)
        {
            string result = "";
            foreach(var obj in dic)
            {
                string data = "/s" + System.Environment.NewLine;
                data += obj.Key + System.Environment.NewLine;
                data += "/t" + System.Environment.NewLine;
                data += obj.Value + System.Environment.NewLine;
                data += "/e" + System.Environment.NewLine + System.Environment.NewLine;


                result += data;
            }

            SaveFile(path, result);
        }


        public static Dictionary<string, string> LoadDBFile(string file, Dictionary<string, string> dic = null)
        {
            if(dic == null)
            {
                dic = new Dictionary<string, string>();
            }

            using(StreamReader r = Util.OpenFile(file))
            {
                if(r != null)
                {
                    string line = "";

                    string ocr = "";
                    string result = "";

                    FileStepType eStep = FileStepType.None;

                    while((line = r.ReadLine()) != null)
                    {
                        if(line == "/s")
                        {
                            ocr = "";
                            result = "";
                            eStep = FileStepType.OCR;
                        }
                        else if(line == "/t")
                        {
                            eStep = FileStepType.Trans;
                        }
                        else if(line == "/e")
                        {
                            eStep = FileStepType.End;

                            if(ocr != "" & result != "")
                            {
                                ocr = ocr.TrimEnd();
                                dic[ocr] = result;
                            }
                        }
                        else
                        {
                            if(eStep == FileStepType.OCR)
                            {
                                if(ocr == "")
                                {
                                    ocr = line;

                                }
                                else
                                {
                                    ocr += System.Environment.NewLine + line;
                                }

                            }
                            else if(eStep == FileStepType.Trans)
                            {
                                if(result == "")
                                {
                                    result = line;
                                }
                                else
                                {
                                    result += System.Environment.NewLine + line;
                                }

                            }
                        }
                    }
                }

                r.Close();
            }

            return dic;
        }

        #endregion

    }

}
