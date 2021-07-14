using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{

    public enum UpdateType
    {
        None, Major, Minor,
    }

    class GlobalDefine
    {
        public enum TesseractLanguageType
        {
            English = 0, Japen = 1, ETC = 2,
        }

        public const string GOOGLE_ACCOUNT_FILE = @"UserData/googleAccount.txt";
        public const string NAVER_ACCOUNT_FILE = @"UserData/naverAccount.txt";
        public const string HOTKEY_FILE = @"UserData/hotKeySetting_v2.txt";
        public const string HOTKEY_FILE_OLD_V2 = @"UserData/hotKeySetting.txt";
        public const string HOTKEY_FILE_OLD = @"UserData/hotKeyStting.txt";
        public const string CHECK_UPDATE_FILE = @"UserData/checkUpdate.txt";
        public const string USER_OPTION_SETTING_FILE = @"UserData/UserOptionSetting.txt";
        public const string USER_SETTING_FILE = @"UserData/setting.conf";   // SaveSetting(@".\\setting\\setting.conf");
        public const string DATA_VERSION_FILE = @"VersionData.txt";

        public const string FORMER_TRANS_FILE = @"UserData/transResult_{0}.txt";   //string.format을 써야 함


        public const string SETTING_PATH= @"setting/";
        public const string DB_PATH = @"DB/";


        public static string SPLITE_TOEKN
        {
            get
            {
                return "//////" + System.Environment.NewLine ;
            }
        }

        public static string SPLITE_TOEKN_NAVER = "//////";
        public static string SPLITE_TOEKN_GOOGLE = "//////";
        public static bool IS_USE_ADVENCED_TOKEN = false;


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
                    token = GlobalDefine.SPLITE_TOEKN_NAVER;
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

            if (GlobalDefine.IS_USE_ADVENCED_TOKEN)
            {
                if (token.Length >= 7)
                {
                    newToken = token.Remove(0, 3);
                    isUseAdvenced = true;
                }
                else if (token.Length >= 6)
                {
                    newToken = token.Remove(0, 2);
                    isUseAdvenced = true;
                }
            }
          

            string[] tokens = { newToken };
            lines = result.Split(tokens, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> resultLines = new List<string>(lines.Length);
            if (isUseAdvenced)
            {
                char firstToken = token[0];

                for (int i = 0; i < lines.Length; i++)
                {
                    int removeCount = 0;
                    
                    if(lines[i].Length >= 1)
                    {
                        //앞 부분 처리
                        if(lines[i][0] == firstToken)
                        {
                            for (int j = 0; j < lines[i].Length; j++)
                            {
                                if (lines[i][j] == firstToken)
                                {
                                    removeCount++;
                                }
                                else
                                {
                                    if (removeCount != -0)
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
                            if (lines[i][lines[i].Length - 1] == firstToken)
                            {
                                for (int j = lines[i].Length - 1; j >= 0; j--)
                                {
                                    if (lines[i][j] == firstToken)
                                    {
                                        removeCount++;
                                    }
                                    else
                                    {
                                        if (removeCount != 0)
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
                int minMinor = 0;
                int maxMinor = 0;

                string[] keys = minorVersion.Split('-');

                if(keys != null && keys.Length >= 2)
                {
                    minMinor = Convert.ToInt32(keys[0]);
                    maxMinor = Convert.ToInt32(keys[1]);

                    if(minMinor <= nowVersion && nowVersion <= maxMinor)
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
        /// 특정 문장에서 특정 규칙으로 데이터 가져오기
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="startKet"></param>
        /// <param name="endKey"></param>
        /// <returns></returns>
        public static string ParseString(string data, string key, char startKey, char endKey)
        {
            string result = "";

            int point = data.LastIndexOf(key);
            if(point != -1)
            {
                point += key.Length;
            }
            else
            {
                return "";
            }

            bool isSatrt = false;

          

            for (int i = point; i < data.Length; i++)
            {
                if(!isSatrt)
                {
                    if (data[i] == startKey)
                    {
                        isSatrt = true;
                    }
                }
                else
                {
                    if (data[i] == endKey)
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
                while (true)
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if(line.Equals(key))
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
            using (StreamReader r = Util.OpenFile(file))
            {
                if (r != null)
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
                    using (System.IO.FileStream fs = System.IO.File.Create(file))
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }

                r = new StreamReader(file);
                              


            }
            catch (FileNotFoundException)
            {
               
            }

            return r;
        }

        public static void SaveFile(string file, string data ,bool isAppend = false)
        {
            try
            {
                if (!File.Exists(file))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(file))
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
                Encoding utf8WithoutBom = new UTF8Encoding(false);
                using (StreamWriter newTask = new StreamWriter(file, isAppend, utf8WithoutBom))
                {
                  
                    newTask.Write(data);
                    newTask.Close();
                }

            }
            catch
            {

            }
        }

         

        public static void ChangeFileData(string file, string key, string value, char startKey = '[', char endKey = ']')
        {
            Util.ShowLog("Change File : " +  file + " key : " + key + " value : " + value);
            using(StreamReader r = Util.OpenFile(file))
            {
                string data = r.ReadToEnd();

                r.Close();
                r.Dispose();

                int point = data.LastIndexOf(key);
                if (point != -1)
                {
                    point += key.Length;
                }

                bool isSatrt = false;
                int startPoint = -1;
                int endPoint = -1;

                if(point != -1)
                {
                    for (int i = point; i < data.Length; i++)
                    {
                        if (!isSatrt)
                        {
                            if (data[i] == startKey)
                            {
                                isSatrt = true;
                                startPoint = i;
                            }
                        }
                        else
                        {
                            if (data[i] == endKey)
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


        private static DateTime dtCheck  = DateTime.MinValue;

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
            if (!isInittoolTip)
            {
                toolTipList = new List<string>();
                string[] tool = Properties.Settings.Default.TOOLTIP_LIST.Split(',');

                for (int i = 0; i < tool.Length; i++)
                {
                    toolTipList.Add(tool[i]);
                    Util.ShowLog(tool[i]);
                }

                isInittoolTip = true;
            }

            string result = "";

            if (toolTipList.Count > 0)
            {
                Random r = new Random();
                int rand = r.Next(0, toolTipList.Count - 1);

                if (rand < toolTipList.Count)
                {
                    result = toolTipList[rand];
                }
            }


            return result;
        }

    }

}
