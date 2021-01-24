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
        private enum FileStepType
        {
            None,
            OCR,
            Trans,
            End,
        }
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
            public eState eNMTstate = NaverKeyData.eState.Normal;

            public NaverKeyData(string id, string secret)
            {
                this.id = id;
                this.secret = secret;
            }

            public void SetState(NaverKeyData.eState state, string apiType)
            {
                this.eNMTstate = state;

            }
        }

        public const int MAX_NAVER = 20;
        public GSTrans.Sheets sheets;

        public string googleKey;

        public List<string> transCodeList = new List<string>();             //얀덱스
        public List<string> resultCodeList = new List<string>();

        public int currentNaverIndex;
        public List<NaverKeyData> naverKeyList = new List<NaverKeyData>();
        public List<string> naverTransCodeList = new List<string>();
        public List<string> naverResultCodeList = new List<string>();

        public List<string> googleTransCodeList = new List<string>();
        public List<string> googleResultCodeList = new List<string>();


        private const int MAX_FORMER = 10000;
        public static bool isSaving = false;
        private Dictionary<SettingManager.TransType, Dictionary<string, string>> resultDic = new Dictionary<SettingManager.TransType, Dictionary<string, string>>();
        private Dictionary<SettingManager.TransType, List<KeyValuePair<string, string>>> saveResultDic = new Dictionary<SettingManager.TransType, List<System.Collections.Generic.KeyValuePair<string, string>>>();

        /// <summary>
        /// 이전에 기록한 결과 제거.
        /// </summary>
        public void ClearFormerDic()
        {
            foreach(var obj in resultDic)
            {
                obj.Value.Clear();
                ClearFormerResultFile(obj.Key);
            }

            foreach(var obj in saveResultDic)
            {
                obj.Value.Clear();
            }
        }

        /// <summary>
        /// 이전에 기록한 결과 사전 초기화.
        /// </summary>
        public void InitFormerDic()
        {
            // 1. 딕셔너리 만듬.
            // 2. 파일 불러옴.
            // 3. 만개 이상이면 반띵함.

            MakeFormerDic(resultDic);
            LoadFormerResultFile(SettingManager.TransType.google);
            LoadFormerResultFile(SettingManager.TransType.google_url);
            LoadFormerResultFile(SettingManager.TransType.naver);


        }

        private void MakeFormerDic(Dictionary<SettingManager.TransType, Dictionary<string, string>> dic)
        {
            if(dic == null)
            {
                dic = new Dictionary<SettingManager.TransType, Dictionary<string, string>>();
            }
            else
            {
                dic.Clear();
            }

            Dictionary<string, string> naverDic = new Dictionary<string, string>();
            Dictionary<string, string> googleDic = new Dictionary<string, string>();
            Dictionary<string, string> basicDic = new Dictionary<string, string>();

            dic.Add(SettingManager.TransType.google, googleDic);
            dic.Add(SettingManager.TransType.naver, naverDic);
            dic.Add(SettingManager.TransType.google_url, basicDic);

            if(saveResultDic == null)
            {
                saveResultDic = new Dictionary<SettingManager.TransType, List<KeyValuePair<string, string>>>();
            }
            else
            {
                saveResultDic.Clear();
            }

            List<KeyValuePair<string, string>> naverList = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> googleList = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> basicList = new List<KeyValuePair<string, string>>();


            saveResultDic.Add(SettingManager.TransType.google, googleList);
            saveResultDic.Add(SettingManager.TransType.naver, naverList);
            saveResultDic.Add(SettingManager.TransType.google_url, basicList);

        }

        private void LoadFormerResultFile(SettingManager.TransType transType)
        {
            resultDic[transType].Clear();
            saveResultDic[transType].Clear();

            Dictionary<string, string> dic = resultDic[transType];
            string path = string.Format(GlobalDefine.FORMER_TRANS_FILE, transType.ToString());
            using (StreamReader r = Util.OpenFile(path))
            {
                if (r != null)
                {
                    string line = "";

                    string ocr = "";
                    string result = "";

                    FileStepType eStep = FileStepType.None;

                    while ((line = r.ReadLine()) != null)
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
                                dic[ocr] = result;
                            }
                        }
                        else
                        {
                            if(eStep == FileStepType.OCR)
                            {
                                if(ocr =="")
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
        }

        public void SaveFormerResultFile(SettingManager.TransType transType)
        {
            if(!isSaving)
            {

                Task task = RunSaveFormerResultFile(transType);
            }        

        }

        private async Task RunSaveFormerResultFile(SettingManager.TransType transType)
        {
            isSaving = true;

            await Task.Run(() =>
            {
                try
                {
                    string saveData = "";
                    string path = string.Format(GlobalDefine.FORMER_TRANS_FILE, transType.ToString());

                    List<KeyValuePair<string, string>> list = null;

                    if (saveResultDic.ContainsKey(transType))
                    {
                        list = saveResultDic[transType];
                    }


                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            string data = "/s";
                            data += Environment.NewLine + list[i].Key + Environment.NewLine + "/t" + Environment.NewLine + list[i].Value + Environment.NewLine + "/e";

                            saveData += data + Environment.NewLine + Environment.NewLine;
                        }

                        Util.SaveFile(path, saveData, true);
                        list.Clear();
                    }
                }
                catch
                {

                }
               
                isSaving = false;
            }).ConfigureAwait(true);

           
        }

        public void ClearFormerResultFile(SettingManager.TransType transType)
        {
            if(!isSaving)
            {
                string saveData = "";
                string path = string.Format(GlobalDefine.FORMER_TRANS_FILE, transType.ToString());
                Util.SaveFile(path, saveData);
            }
          
        }


        private string GetFormerResult(SettingManager.TransType transType, string ocrValue)
        {
            string result = null;
            if (!isSaving)
            {
                bool isFound = false;
                if (!resultDic.ContainsKey(transType))
                {
                    resultDic.Add(transType, new Dictionary<string, string>());
                }

                if (resultDic[transType].TryGetValue(ocrValue, out result))
                {
                    isFound = true;
                }
       
            }

         

            return result;
        }

        private void AddFormerResult(SettingManager.TransType transType, string ocrValue, string result)
        {
            if(!isSaving)
            {
                if (resultDic[transType].Count >= MAX_FORMER)
                {
                    resultDic[transType].Clear();
                    saveResultDic[transType].Clear();
                    //파일을 열고 다 지운다.      

                    ClearFormerResultFile(transType);
                }

                resultDic[transType][ocrValue] = result;
                saveResultDic[transType].Add(new KeyValuePair<string, string>(ocrValue, result));

            }
          

        }

        public void InitGtrans(string sheetID, string clientID, string secretKey, string source, string result)
        {
            if (sheets == null)
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

            if (!isComplete && clientID != "")
            {
                SettingManager.isErrorEmptyGoogleToken = true;
            }
        }

        public void InitGTransToken()
        {
            if (sheets != null)
            {
                sheets.InitToken();
            }
        }

        public void DeleteAllGsTransToken()
        {
            Util.ShowLog("Delete google token");
            GSTrans.Sheets.DeleteToken();
        }

        public async Task<string> StartTrans(string text, SettingManager.TransType trasType)
        {
            if (text == "")
            {
                return "";
            }

            Task<string> task1 = Task<string>.Run(() => GetTrans2(text, trasType));
            string result = await task1;


            return result;

        }


        public async Task<string> GetTrans2(string text, SettingManager.TransType transType)
        {
            Util.ShowLog("OCR : " + text);
            try
            {
                bool isError = false;
                bool isContain = false;

                string formerResult = null;

                if (transType != SettingManager.TransType.db)
                {
                    formerResult = GetFormerResult(transType, text);

                    if(string.IsNullOrEmpty(formerResult))
                    {
                        isContain = false;
                    }
                    else
                    {
                        isContain = true;
                    }
                }

                string result = "";

                if (!isContain)
                {
                    if (transType == SettingManager.TransType.db)
                    {                
                        StringBuilder sb = new StringBuilder(text, 8192);
                        StringBuilder sb2 = new StringBuilder(8192);
                        Form1.ProcessGetDBText(sb, sb2);
                        result = sb2.ToString();
                    }
                    else
                    {
                        if (transType == SettingManager.TransType.naver)
                        {
                            result = NaverTranslateAPI.instance.GetResult(text, ref isError);
                            result = result.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.google)
                        {
                            result = sheets.Translate(text, ref isError);
                            result = result.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.google_url)
                        {
                            result = GoogleBasicTranslateAPI.instance.GetResult(text, ref isError);
                        }
                    }                 

                    if (!isError && transType != SettingManager.TransType.db)
                    {
                        AddFormerResult(transType, text, result);
                    }
                }
                else
                {
                    if(Form1.isShowFormerResultLog)
                    {
                        result = "[기억 결과 " + resultDic[transType].Count.ToString() + " ] " + formerResult;
                    }
                    else
                    {
                        result =  formerResult;
                    }
                   
                }

                return result;
            }
            catch (Exception e)
            {
                return "Error " + e;
            }
        }

        public static bool GetIsRemain()
        {
            bool isRemain = true;

            if (instance == null)
            {
                isRemain = false;
            }

            return isRemain;
        }

        public void InitTransCode()
        {
            //TODO : 코드와 콤보박스 모두 설정할 수 있도록 변경해야 한다.
            transCodeList.Add("en");
            transCodeList.Add("ja");
            transCodeList.Add("zh-CHS");
            transCodeList.Add("zh-CHT");
            transCodeList.Add("ko");
            transCodeList.Add("ru");
            transCodeList.Add("de");
            transCodeList.Add("pt");
            transCodeList.Add("es");
            transCodeList.Add("fr");
            transCodeList.Add("vi");
            transCodeList.Add("th");
     

            resultCodeList.Add("ko");
            resultCodeList.Add("en");
            resultCodeList.Add("ja");
            resultCodeList.Add("zh-CHS");
            resultCodeList.Add("zh-CHT");
            resultCodeList.Add("ru");
            resultCodeList.Add("de");
            resultCodeList.Add("pt");
            resultCodeList.Add("es");
            resultCodeList.Add("fr");
            resultCodeList.Add("vi");
            resultCodeList.Add("th");
        

            naverTransCodeList.Add("en");
            naverTransCodeList.Add("ja");
            naverTransCodeList.Add("zh-CN");
            naverTransCodeList.Add("zh-TW");
            naverTransCodeList.Add("es");
            naverTransCodeList.Add("fr");
            naverTransCodeList.Add("vi");
            naverTransCodeList.Add("th");
            naverTransCodeList.Add("id");
            naverTransCodeList.Add("ko");



            naverResultCodeList.Add("ko");
            naverResultCodeList.Add("en");


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
            googleTransCodeList.Add("fr");
            googleTransCodeList.Add("vi");
            googleTransCodeList.Add("th");

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
            googleResultCodeList.Add("fr");
            googleResultCodeList.Add("vi");
            googleResultCodeList.Add("th");
        }


        public void SetState(NaverKeyData.eState state)
        {

            if (naverKeyList.Count == 0)
            {

            }
            else
            {

                if (naverKeyList.Count > currentNaverIndex)
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
                if (currentNaverIndex >= naverKeyList.Count || currentNaverIndex == MAX_NAVER + 1)
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
                StreamReader r = new StreamReader(GlobalDefine.NAVER_ACCOUNT_FILE);

                string line;

                Dictionary<string, NaverKeyData> dataDic = new Dictionary<string, NaverKeyData>();

                for (int i = 0; i < naverKeyList.Count; i++)
                {
                    if (dataDic.ContainsKey(naverKeyList[i].id))
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


                        if (dataDic.ContainsKey(id) && dataDic[id].secret == secret)
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


                for (int i = 0; i < naverKeyList.Count; i++)
                {
                    Util.ShowLog("id : " + naverKeyList[i].id + " / secret : " + naverKeyList[i].secret);
                }

                r.Close();
                r.Dispose();

            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.NAVER_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();

                }
            }
        }


        public void SaveNaverKeyFile(string id, string secret)
        {

            id = id.Replace(" ", "");
            secret = secret.Replace(" ", "");
            try
            {
                using (StreamWriter newTask = new StreamWriter(GlobalDefine.NAVER_ACCOUNT_FILE, false))
                {

                    newTask.WriteLine(id);
                    newTask.WriteLine(secret);

                    //첫 번째는 넘김.
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
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.NAVER_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(GlobalDefine.NAVER_ACCOUNT_FILE, false))
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

            if (naverKeyList.Count > 0)
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
