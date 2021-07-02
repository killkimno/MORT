using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    class GlobalDefine
    {
        public const string GOOGLE_ACCOUNT_FILE = @"UserData/googleAccount.txt";
        public const string NAVER_ACCOUNT_FILE = @"UserData/naverAccount.txt";
        public const string HOTKEY_FILE = @"UserData/hotKeySetting.txt";
        public const string HOTKEY_FILE_OLD = @"UserData/hotKeyStting.txt";
        public const string CHECK_UPDATE_FILE = @"UserData/checkUpdate.txt";
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
        public static void SetSpliteToken(string naver, string google)
        {
            GlobalDefine.SPLITE_TOEKN_NAVER = naver;
            GlobalDefine.SPLITE_TOEKN_GOOGLE = google;
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

            //만약 찾은 위치 다음이 공백, 키값이 아니면 취소하고 끝내야 한다
            if(point  < data.Length)
            {
                if(!(data[point ] == ' ' || data[point ] == startKey || data[point] == '\n' || data[point] == '\r'))
                {
                    return "";
                }
            }


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


        public static string ParseStringFromFile(string file, string key, char startKey, char endKey)
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

         

        public static void ChangeFileData(string file, string key, string value, char startKey, char endKey)
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
