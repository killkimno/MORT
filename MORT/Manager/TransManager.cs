using System;
using System.Collections.Generic;
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

        public GSTrans.Sheets sheets;
        public string googleKey;

        public List<string> transCodeList = new List<string>();
        public List<string> resultCodeList = new List<string>();

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

                return result;
            }
            catch
            {
                return "Error";
            }
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

            resultCodeList.Add("ko");
            resultCodeList.Add("en");
            resultCodeList.Add("ja");
            resultCodeList.Add("zh-CHS");
            resultCodeList.Add("zh-CHT");
            resultCodeList.Add("ru");
            resultCodeList.Add("de");
            resultCodeList.Add("pt");

            naverTransCodeList.Add("en");
            naverTransCodeList.Add("ja");
            naverTransCodeList.Add("zh-CN");

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

            googleResultCodeList.Add("ko");
            googleResultCodeList.Add("en");
            googleResultCodeList.Add("ja");
            googleResultCodeList.Add("zh-CN");
            googleResultCodeList.Add("zh-TW");
            googleResultCodeList.Add("ru");
            googleResultCodeList.Add("de");
            googleResultCodeList.Add("pt-BR");
            googleResultCodeList.Add("pt-PT");
        }
    }
}
