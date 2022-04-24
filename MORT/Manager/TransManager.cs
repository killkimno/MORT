using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT
{
    class TransManager : IDisposable
    {
        public class TransCodeData
        {
            public string title;
            public string languageCode;
            public string googleCode;
            public string naverCode;
            public bool isSupportNaverResult;

            public TransCodeData(string title, string languageCode, string googleCode, string naverCode, bool isSupportNaverResult)
            {
                this.title = title;
                this.languageCode = languageCode;
                this.googleCode = googleCode;
                this.naverCode = naverCode;
                this.isSupportNaverResult = isSupportNaverResult;
            }
        }
        private class TransData
        {
            public int index;
            public string text = "";
            public string result = "";
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
            public bool isPaid = false;

            public NaverKeyData(string id, string secret, bool isPaid = false)
            {
                this.id = id;
                this.secret = secret;
                this.isPaid = isPaid;
            }

            public void SetState(NaverKeyData.eState state, string apiType)
            {
                this.eNMTstate = state;

            }
        }

        public const int MAX_NAVER = 20;
        public GSTrans.Sheets sheets;

        public string googleKey;

        //TODO : 다음 업데이트 때 시트 안에서 처리로 바꿔야함.
        //백업용 구글 시트 번역 언어
        private string _backupSheetSource = "";
        private string _backupSheetTarget = "";

        public int currentNaverIndex;
        public List<NaverKeyData> naverKeyList = new List<NaverKeyData>();



        private const int MAX_FORMER = 10000;
        public static bool isSaving = false;
        private Dictionary<SettingManager.TransType, Dictionary<string, string>> resultDic = new Dictionary<SettingManager.TransType, Dictionary<string, string>>();
        private Dictionary<SettingManager.TransType, List<KeyValuePair<string, string>>> saveResultDic = new Dictionary<SettingManager.TransType, List<System.Collections.Generic.KeyValuePair<string, string>>>();

        private Dictionary<string, string> userTranslationDic = new Dictionary<string, string>();

        private bool isTranslationDbStyle = false;


        private PipeServer.PipeServer _ezTransPipeServer = new PipeServer.PipeServer();
        public bool InitEzTrans()
        {
            return _ezTransPipeServer.InitPipe();
        }

        public void LoadUserTranslation(List<string> files)
        {
            var transType =  FormManager.Instace.MyMainForm.MySettingManager.NowTransType;

            if(transType != SettingManager.TransType.db)
            {
                isTranslationDbStyle = false;
                userTranslationDic.Clear();
                var languageType = FormManager.Instace.MyMainForm.MySettingManager.GetOcrLanguage();
                if (AdvencedOptionManager.IsTranslationDbStyle && (languageType == OcrLanguageType.English || languageType == OcrLanguageType.Japen))
                {
                    string data = "";
                    foreach (var obj in files)
                    {
                        string path = GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt";
                        var stream = Util.OpenFile(path);
                        if (stream != null)
                        {
                            data += stream.ReadToEnd();
                            data += System.Environment.NewLine;
                        }
                    }

                    Util.SaveFile(GlobalDefine.DB_PATH + AdvencedOptionManager.TEMP_USER_TRANSLATION_DB_FILE, data);

                    bool isStringUpper = AdvencedOptionManager.IsTranslationStringUpper;

                    
                    Form1.SetIsStringUpper(isStringUpper);
                    Form1.setUseDB(true, false, AdvencedOptionManager.TEMP_USER_TRANSLATION_DB_FILE);
                    isTranslationDbStyle = true;
                }
                else
                {
                    foreach (var obj in files)
                    {
                        string path = GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt";
                        userTranslationDic = Util.LoadDBFile(path, userTranslationDic);
                    }

                    isTranslationDbStyle = false;
                }
            }

           

          
        }

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

            dic = Util.LoadDBFile(path, dic);

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


        private string GetUserTransResult(string ocrText)
        {
            string result = null;

            if(isTranslationDbStyle)
            {
                StringBuilder sb = new StringBuilder(ocrText, 8192);
                StringBuilder sb2 = new StringBuilder(8192);
                Form1.ProcessGetDBText(sb, sb2);

                result = sb2.ToString();

                if (result == "not thing")
                {
                    result = null;
                }
            }
            else
            {
                if (userTranslationDic.TryGetValue(ocrText, out result))
                {

                }
            }
            

            return result;
        }

        /// <summary>
        /// 유저 사전과 이전 결과를 이용해 가져온다.
        /// </summary>
        /// <param name="transType"></param>
        /// <param name="ocrValue"></param>
        /// <returns></returns>
        private string GetFormerResult(SettingManager.TransType transType, string ocrValue)
        {
            string result = GetUserTransResult(ocrValue);

            if (result == null && !isSaving)
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

            //백업
            _backupSheetSource = source;
            _backupSheetTarget = result;

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

        public async Task<string> StartTrans(string text, SettingManager.TransType trasType, List<string> textList = null)
        {
            if (text == "")
            {
                return "";
            }


            Task<string> task1 = null;

            if(textList == null)
            {
                task1 = Task<string>.Run(() => GetTransAsync(text, trasType));
            }
            else
            {
                task1 = Task<string>.Run(() => GetTransLinesAsync(textList, trasType));
            }
            string result = await task1;


            return result;

        }
        
        public async Task<string> GetTransLinesAsync(List<string> textList, SettingManager.TransType transType)
        {

            int removeLength = System.Environment.NewLine.Length ;
            Dictionary<int, TransData> textDic = new Dictionary<int, TransData>();

            for(int i = 0; i < textList.Count;i++)
            {
                TransData data = new TransData();
                data.index = i;
                data.text = textList[i].TrimEnd();

                data.result = "";

                textDic.Add(data.index, data);
            }

            string text = "";
            try
            {
                bool isError = false;
                bool isContain = false;
                bool isUseDic = true;
                string formerResult = null;


                if (transType == SettingManager.TransType.db || transType == SettingManager.TransType.ezTrans)
                {
                    isUseDic = false;
                }


                if (isUseDic)
                {
                    string require = "";
                    foreach(var obj in textDic)
                    {
                        obj.Value.result = GetFormerResult(transType, obj.Value.text);

                        if(string.IsNullOrEmpty( obj.Value.result ))
                        {
                            obj.Value.result = "";
                            require += Util.GetSpliteToken(transType) + obj.Value.text + System.Environment.NewLine;
                        }
                        else
                        {
                            if (Form1.isDebugShowFormerResultLog)
                            {
                                obj.Value.result = "[기억 결과 " + resultDic[transType].Count.ToString() + " ] " + obj.Value.result;
                            }
                        }
                    }

                    if(require != "")
                    {
                        string transResult = "";
                        if (transType == SettingManager.TransType.naver)
                        {
                            transResult = NaverTranslateAPI.instance.GetResult(require, ref isError);
                            transResult = transResult.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.google)
                        {
                            transResult = DoGoogleSheetTranslate(require, ref isError);                       
                            transResult = transResult.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.google_url)
                        {
                            transResult = GoogleBasicTranslateAPI.instance.DoTrans(require, ref isError);
                        }


                        if(isError)
                        {
                            //문제가 있으면 바로 끝낸다.
                            return transResult;
                        }
                        else
                        {
                            //string[] separatingStrings = { Util.GetSpliteToken(transType) };
                            //string[] words = transResult.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                            string[] words = Util.GetSpliteByToken(transResult, transType);
                            int index = 0;
                            foreach (var obj in textDic)
                            {
                                if (string.IsNullOrEmpty(obj.Value.result))
                                {
                                    if (words.Length > index)
                                    {
                                        obj.Value.result = words[index++];

                                         AddFormerResult(transType, obj.Value.text, obj.Value.result);
                                    }
                                }
                            }
                        }
                    }
                }
                else 
                {
                    if(transType == SettingManager.TransType.db)
                    {
                        foreach (var obj in textDic)
                        {
                            StringBuilder sb = new StringBuilder(obj.Value.text, 8192);
                            StringBuilder sb2 = new StringBuilder(8192);
                            Form1.ProcessGetDBText(sb, sb2);

                            obj.Value.result = sb2.ToString();

                            if (obj.Value.result == "not thing")
                            {
                                obj.Value.result = "";
                            }
                        }
                    }
                    else if(transType == SettingManager.TransType.ezTrans)
                    {
                        foreach(var obj in textDic)
                        {
                            if (_ezTransPipeServer.InitResponse)
                            {
                                obj.Value.result = GetUserTransResult(obj.Value.text);
                                if(obj.Value.result == null)
                                {
                                    obj.Value.result = await  _ezTransPipeServer.DoTransAsync(obj.Value.text);
                                }
                               
                            }
                            else
                            {
                                obj.Value.result = "이지트랜스를 사용할 수 없습니다.";
                            }
                        }
                       
                    }
                                
                }
                string result = "";

                foreach(var obj in textDic)
                {
                    result += Util.GetSpliteToken(transType) + obj.Value.result + System.Environment.NewLine;
                }
                return result;
            }
            catch (Exception e)
            {
                return "Error " + e;
            }
        }


        public async Task<string> GetTransAsync(string text, SettingManager.TransType transType)
        {
            text = text.TrimEnd();
            Util.ShowLog("OCR : " + text);
            try
            {
                bool isError = false;
                bool isContain = false;

                string formerResult = null;
                bool isUseDic = true;

                if(transType == SettingManager.TransType.db || transType == SettingManager.TransType.ezTrans)
                {
                    isUseDic = false;
                }

                if (isUseDic)
                {                    
                    formerResult = GetFormerResult(transType, text);

                    if (string.IsNullOrEmpty(formerResult))
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
                    else if(transType == SettingManager.TransType.ezTrans)
                    {
                        if(_ezTransPipeServer.InitResponse)
                        {
                            result = GetUserTransResult(text);
                            if (result == null)
                            {
                                result = await _ezTransPipeServer.DoTransAsync(text);
                            }
                      
                        }
                        else
                        {
                            result = "이지트랜스를 사용할 수 없습니다.";
                        }
                 
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
                            result = DoGoogleSheetTranslate(text, ref isError);
                            result = result.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.google_url)
                        {
                            result = GoogleBasicTranslateAPI.instance.DoTrans(text, ref isError);
                        }
                    }                 

                    if (!isError && isUseDic)
                    {
                        AddFormerResult(transType, text, result);
                    }
                }
                else
                {
                    if(Form1.isDebugShowFormerResultLog)
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

        public string DoGoogleSheetTranslate(string original, ref bool isError)
        {
            string result = "";

            sheets.target = _backupSheetTarget;
            sheets.source = _backupSheetSource;

            if(sheets.source != "ja" && AdvencedOptionManager.IsExecutive)
            {
                sheets.target = "ja";
                original = sheets.Translate(original, ref isError);
                result = original;


                Util.ShowLog("Ori : " + original);
                if(!isError)
                {
                    sheets.source = "ja";
                    sheets.target = _backupSheetTarget;
                    result = sheets.Translate(original, ref isError);
                }
            }
            else
            {
                result = sheets.Translate(original, ref isError);
            }

            return result;
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
        private void AddTransCode(string title, string ocrCode, string naverCode, string googleCode, bool isSupportNaverResult = false)
        {
            TransCodeData data = new TransCodeData(title, ocrCode, googleCode, naverCode, isSupportNaverResult);
            codeDataList.Add(data);
        }

        public TransCodeData GetTransCodeData(string code)
        {
            TransCodeData data = null;


            for(int i = 0; i < codeDataList.Count; i++)
            {
                if(Util.GetIsEqualWinCode(code, codeDataList[i].languageCode))
                {
                    data = codeDataList[i];
                    break;
                }
            }

            return data;
        }

        List<TransCodeData> codeDataList = new List<TransCodeData>();
        public void InitTransCode(System.Windows.Forms.ComboBox cbNaver, System.Windows.Forms.ComboBox cbNaverResult, 
                                    System.Windows.Forms.ComboBox cbGoogle, System.Windows.Forms.ComboBox cbGoogleResult)
        {
            //TODO : 코드와 콤보박스 모두 설정할 수 있도록 변경해야 한다.


            codeDataList.Clear();
            AddTransCode("영어", "en", "en", "en", true);
            AddTransCode("일본어", "ja", "ja", "ja");
            AddTransCode("중국어 간체", "zh-Hans-CN", "zh-CN", "zh-CN");
            AddTransCode("중국어 번체", "zh-Hant-TW", "zh-TW", "zh-TW");
            AddTransCode("스페인어", "es", "es", "es");
            AddTransCode("프랑스어", "fr-FR", "fr", "fr");
            AddTransCode("베트남어", "vi", "vi", "vi");
            AddTransCode("태국어", "th", "th", "th");
            AddTransCode("인도네시아어", "id", "id", "id");
            AddTransCode("한국어", "", "ko", "ko", true);
            AddTransCode("러시아어", "ru", "", "ru");
            AddTransCode("독일어", "de-DE", "", "de");
            AddTransCode("브라질어", "pt-BR", "", "pt-BR");
            AddTransCode("포르투갈어", "pt-PT", "", "pt-PT");

            cbNaver.Items.Clear();
            cbNaverResult.Items.Clear();
            cbGoogle.Items.Clear();
            cbGoogleResult.Items.Clear();

            foreach(var obj in codeDataList)
            {
                //네이버 설정
                if(obj.naverCode != "")
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = obj.title;
                    item.Value = obj;

                    cbNaver.Items.Add(item);

                    //네이버 결과
                    if(obj.isSupportNaverResult)
                    {
                        if(obj.naverCode == "ko")
                        {
                            cbNaverResult.Items.Insert(0, item);
                        }
                        else
                        {
                            cbNaverResult.Items.Add(item);
                        }
                    }
                }
                
                //구글 
                if(obj.googleCode != "")
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = obj.title;
                    item.Value = obj;

                    cbGoogle.Items.Add(item);

                    if(obj.googleCode == "ko")
                    {
                        cbGoogleResult.Items.Insert(0, item);
                    }
                    else
                    {
                        cbGoogleResult.Items.Add(item);
                    }
                }
            }
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

        public List<NaverKeyData> SortNaverKeyList()
        {
            List<NaverKeyData> list = naverKeyList;

            List<NaverKeyData> freeList = new List<NaverKeyData>();
            List<NaverKeyData> paidList = new List<NaverKeyData>();

            foreach(var obj in list)
            {
                if(obj.isPaid)
                {
                    paidList.Add(obj);
                }
                else
                {
                    freeList.Add(obj);
                }
            }

            list.Clear();
            
            foreach(var obj in freeList)
            {
                list.Add(obj);
            }

            foreach(var obj in paidList)
            {
                list.Add(obj);
            }
            currentNaverIndex = 0;

         
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].eNMTstate == NaverKeyData.eState.Normal)
                {
                    currentNaverIndex = i;
                    break;
                }
            }




            return list;
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
                    if (!dataDic.ContainsKey(naverKeyList[i].id))
                    {
                        dataDic.Add(naverKeyList[i].id, naverKeyList[i]);
                    }
                }

                naverKeyList.Clear();
                
                while ((line = r.ReadLine()) != null)
                {
                    string id = line;
                    string secret = "";
                    bool isPaid = false;
                    line = r.ReadLine();
                    if (line != null)
                    {
                        string[] keys = line.Split('\t');

                        if(keys.Length == 1)
                        {
                            secret = line;
                        }
                        else
                        {
                            if(keys.Length >= 2)
                            {
                                secret = keys[0];
                                isPaid = Convert.ToBoolean(keys[1]);
                            }                         
                        }
                   

                        if (dataDic.ContainsKey(id) && dataDic[id].secret == secret)
                        {
                            naverKeyList.Add(dataDic[id]);
                        }
                        else
                        {
                            NaverKeyData data = new NaverKeyData(id, secret, isPaid);
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
                    Util.ShowLog("id : " + naverKeyList[i].id + " / secret : " + naverKeyList[i].secret + " / isPaid : " + naverKeyList[i].isPaid);
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


        public void SaveNaverKeyFile(string id, string secret, bool isPaid = false)
        {
            id = id.Replace(" ", "");
            secret = secret.Replace(" ", "");

            try
            {
                using (StreamWriter newTask = new StreamWriter(GlobalDefine.NAVER_ACCOUNT_FILE, false))
                {

                    newTask.WriteLine(id);
                    newTask.WriteLine(secret + "\t" + isPaid.ToString());

                    //첫 번째는 넘김.
                    for (int i = 1; i < naverKeyList.Count; i++)
                    {
                        newTask.WriteLine(naverKeyList[i].id);
                        newTask.WriteLine(naverKeyList[i].secret + "\t" + naverKeyList[i].isPaid.ToString());
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
                        newTask.WriteLine(secret + "\t" + isPaid.ToString());

                        for (int i = 1; i < naverKeyList.Count; i++)
                        {
                            newTask.WriteLine(naverKeyList[i].id);
                            newTask.WriteLine(naverKeyList[i].secret + "\t" + naverKeyList[i].isPaid.ToString());
                        }

                        if (naverKeyList.Count > 0)
                        {
                            naverKeyList[0].id = id;
                            naverKeyList[0].secret = secret;
                            naverKeyList[0].isPaid = isPaid;
                        }
                        else
                        {
                            naverKeyList.Add(new NaverKeyData(id, secret, isPaid));
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

        public void Dispose()
        {
            if(_ezTransPipeServer != null)
            {
                _ezTransPipeServer.Close();
            }
        }
    }
}
