using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT
{
    class TransManager
    {
        private static TransManager instance;
        public static TransManager Instace
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransManager();
                }
                return instance;
            }
        }

        public class NaverKeyData
        {
           
            public enum eState
            {
                Normal, Error, Limit,
            }
            public string id;
            public string secret;
            public eState eSMTstate = NaverKeyData.eState.Normal;
            public eState eNMTstate = NaverKeyData.eState.Normal;

            public NaverKeyData(string id , string secret)
            {
                this.id = id;
                this.secret = secret;
            }

            public void SetState(NaverKeyData.eState state, string apiType)
            {
                if(apiType == MORT.NaverTranslateAPI.API_NMT)
                {
                    this.eNMTstate = state;
                }
                else if(apiType == MORT.NaverTranslateAPI.API_SMT)
                {
                    this.eSMTstate = state;
                }
             
            }
        }

        public const int MAX_NAVER = 15;
        public GSTrans.Sheets sheets;

        public string googleKey;

        public List<string> transCodeList = new List<string>();
        public List<string> resultCodeList = new List<string>();

        public int currentNaverIndex;
        public List<NaverKeyData> naverKeyList = new List<NaverKeyData>();
        public List<string> naverTransCodeList = new List<string>();
        public List<string> naverResultCodeList = new List<string>();

        public List<string> googleTransCodeList = new List<string>();
        public List<string> googleResultCodeList = new List<string>();

        public void InitGtrans(string sheetID , string clientID, string secretKey, string source, string result)
        {
            if(sheets == null )
            {
                sheets = new GSTrans.Sheets();
            }
            googleKey = sheetID;
            sheets.spreadsheetId = @sheetID;// @"1k4dlDiXjuJnIS0K1EYuMxB40f_cZP3t0sGtS5cv3J3I";
            

            bool isComplete = sheets.Init(@sheetID, clientID, secretKey);

            //sheets.RowCount = 30;
            //초기화 - 반드시 해줘야 함 - 시트가 제대로 준비되었는지 확인하고, 준비되지 않았을 경우 셋팅합니다.
            //sheets.Initialize();

            //초기화 후 언제나 변경 가능한 변수 설정
            sheets.source = source;
            sheets.target = result;

            if(!isComplete && clientID != "")
            {
                SettingManager.isErrorEmptyGoogleToken = true;
            }
        }

        public void InitGTransToken()
        {
            if(sheets != null)
            {
                sheets.InitToken();
            }
        }

        public void DeleteAllGsTransToken()
        {
            if(sheets != null)
            {
                sheets.DeleteToken();
            }
        }

        public async Task<string> StartTrans(string text, SettingManager.TransType trasType)
        {
            Task<string> task1 = Task<string>.Run(() => GetTrans2(text, trasType));

            string result = await task1;

            return result;

        }        


        public async Task<string> GetTrans2(string text, SettingManager.TransType trasType)
        {
            try
            {
                string result = "";
                //trasType = SettingManager.TransType.google;
                if (trasType == SettingManager.TransType.db)
                {
                    StringBuilder sb = new StringBuilder(text, 8192);
                    StringBuilder sb2 = new StringBuilder(8192);
                    Form1.ProcessGetDBText(sb, sb2);
                    result = sb2.ToString();
                }
                else if (trasType == SettingManager.TransType.yandex)
                {
                    result = YandexAPI.instance.GetResult(text);
                }
                else if (trasType == SettingManager.TransType.naver)
                {
                    result = NaverTranslateAPI.instance.GetResult(text);
                }
                else if (trasType == SettingManager.TransType.google)
                {
                    result = sheets.Translate(text);
                }
                else if(trasType == SettingManager.TransType.google_url)
                {
                    result = GoogleBasicTranslateAPI.instance.GetResult(text);
                }

                return result;
            }
            catch(Exception e)
            {
                return "Error " + e;
            }
        }

        public static bool GetIsRemain()
        {
            bool isRemain = true;

            if(instance == null)
            {
                isRemain = false;
            }

            return isRemain;
        }

        public void InitTransCode()
        {

            transCodeList.Add("en");
            transCodeList.Add("ja");
            transCodeList.Add("zh-CHS");
            transCodeList.Add("zh-CHT");
            transCodeList.Add("ko");
            transCodeList.Add("ru");
            transCodeList.Add("de");
            transCodeList.Add("pt");
            transCodeList.Add("es");

            resultCodeList.Add("ko");
            resultCodeList.Add("en");
            resultCodeList.Add("ja");
            resultCodeList.Add("zh-CHS");
            resultCodeList.Add("zh-CHT");
            resultCodeList.Add("ru");
            resultCodeList.Add("de");
            resultCodeList.Add("pt");
            resultCodeList.Add("es");

            naverTransCodeList.Add("en");
            naverTransCodeList.Add("ja");
            naverTransCodeList.Add("zh-CN");
            naverTransCodeList.Add("es");

            naverResultCodeList.Add("ko");


            googleTransCodeList.Add("en");
            googleTransCodeList.Add("ja");
            googleTransCodeList.Add("zh-CN");
            googleTransCodeList.Add("zh-TW");
            googleTransCodeList.Add("ko");
            googleTransCodeList.Add("ru");
            googleTransCodeList.Add("de");
            googleTransCodeList.Add("pt-BR");
            googleTransCodeList.Add("pt-PT");
            googleTransCodeList.Add("es");

            googleResultCodeList.Add("ko");
            googleResultCodeList.Add("en");
            googleResultCodeList.Add("ja");
            googleResultCodeList.Add("zh-CN");
            googleResultCodeList.Add("zh-TW");
            googleResultCodeList.Add("ru");
            googleResultCodeList.Add("de");
            googleResultCodeList.Add("pt-BR");
            googleResultCodeList.Add("pt-PT");
            googleResultCodeList.Add("es");
        }


        public void SetState(NaverKeyData.eState state)
        {

            if (naverKeyList.Count == 0)
            {
                
            }
            else
            {
             
                if(naverKeyList.Count > currentNaverIndex)
                {
                    naverKeyList[currentNaverIndex].SetState(state, NaverTranslateAPI.instance.GetAPIType());
                }
               
            }
        }

        public NaverKeyData GetNextNaverKey()
        {
            NaverKeyData data = null;

            if (naverKeyList.Count == 0)
            {
                data = new NaverKeyData("", "");
            }
            else
            {
                currentNaverIndex++;
                if(currentNaverIndex >= naverKeyList.Count || currentNaverIndex == MAX_NAVER + 1)
                {
                    currentNaverIndex = 0;
                }

                data = naverKeyList[currentNaverIndex];
            }

            return data;
        }

        public void OpenNaverKeyFile()
        {
            currentNaverIndex = 0;
            try
            {
                StreamReader r = new StreamReader(@"naverAccount.txt");

                string line;

                Dictionary<string, NaverKeyData> dataDic = new Dictionary<string, NaverKeyData>();

                for(int i = 0; i < naverKeyList.Count; i++)
                {
                    if(dataDic.ContainsKey(naverKeyList[i].id))
                    {
                        dataDic.Add(naverKeyList[i].id, naverKeyList[i]);
                    }                 
                }
                naverKeyList.Clear();
                while ((line = r.ReadLine()) != null)
                {
                    string id = line;
                    string secret = "";
                    line = r.ReadLine();
                    if (line != null)
                    {
                        secret = line;
                      

                        if(dataDic.ContainsKey(id) && dataDic[id].secret == secret)
                        {
                            naverKeyList.Add(dataDic[id]);
                        }
                        else
                        {
                            NaverKeyData data = new NaverKeyData(id, secret);
                            naverKeyList.Add(data);
                        }                     

                    }
                    else
                    {
                        break;
                    }
                }


                for(int i = 0; i < naverKeyList.Count; i++)
                {
                    Console.WriteLine("id : " + naverKeyList[i].id + " / secret : " + naverKeyList[i].secret);
                }

                r.Close();
                r.Dispose();

            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"naverAccount.txt"))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }
        }


        public void SaveNaverKeyFile(string id,string secret)
        {           
           
            id = id.Replace(" ", "");
            secret = secret.Replace(" ", "");
            try
            {
                using (StreamWriter newTask = new StreamWriter(@"naverAccount.txt", false))
                {

                    newTask.WriteLine(id);
                    newTask.WriteLine(secret);
                    
                    //첫 번째는 넘김.
                    for(int i = 1; i< naverKeyList.Count; i++)
                    {
                        newTask.WriteLine(naverKeyList[i].id);
                        newTask.WriteLine(naverKeyList[i].secret);
                    }

                    if(naverKeyList.Count > 0)
                    {
                        naverKeyList[0].id = id;
                        naverKeyList[0].secret = secret;
                    }
                    else
                    {
                        naverKeyList.Add(new NaverKeyData(id, secret));
                    }
                    newTask.Close();
                }


            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"naverAccount.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(@"naverAccount.txt", false))
                    {
                        newTask.WriteLine(id);
                        newTask.WriteLine(secret);

                        for (int i = 1; i < naverKeyList.Count; i++)
                        {
                            newTask.WriteLine(naverKeyList[i].id);
                            newTask.WriteLine(naverKeyList[i].secret);
                        }

                        if (naverKeyList.Count > 0)
                        {
                            naverKeyList[0].id = id;
                            naverKeyList[0].secret = secret;
                        }
                        else
                        {
                            naverKeyList.Add(new NaverKeyData(id, secret));
                        }

                        newTask.Close();
                    }
                }
            }
        }

        public NaverKeyData GetNaverKey()
        {
            NaverKeyData data = null;

            if(naverKeyList.Count > 0)
            {
                data = naverKeyList[0];
            }
            else
            {
                data = new NaverKeyData("", "");
            }

            return data;
        }

        
    }
}
