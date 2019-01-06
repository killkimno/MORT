using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSTrans {

    public class Sheets {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "MORT GSTrans v1";

        /// <summary>
        /// 행의 수입니다. 최소 50을 강력하게 권장합니다.
        /// </summary>
        public int RowCount = 50;

        /// <summary>
        /// 번역할 언어입니다. 기본값은 en 이며, 구글 번역 기준 en이 영어, ja이 일본어, ko가 한국어입니다.
        /// </summary>
        public string source = "en";
        /// <summary>
        /// 번역 결과로 나올 언어입니다. 기본값은 ko 이며, 구글 번역 기준 en이 영어, ja이 일본어, ko가 한국어입니다.
        /// </summary>
        public string target = "ko";

        /// <summary>
        /// 시트 주소입니다. 기본값에서 변경할 수 있습니다. 반드시 2열 & 5000행 이상이 존재해야 합니다.
        /// </summary>
        public string spreadsheetId = "1ffofZDHlFzRN3Qo85MR3i2cQCjlvahJKILqbiAAiZVw";
        public string clientID;
        public string secretKey;
        //public string range = "en-ko!A2:B";

        SheetsService service;

        Random r;
        string title = "MORT GSTrans";
        int? sheetId = 30;

        public bool isInit;

        public Sheets() {
          
         
         
        }
        
        /// <summary>
        /// 초기화 합니다. spreadsheetId를 변경하려면 초기화 전에 변경해주세요.
        /// </summary>
        public void Initialize() {
            try
            {
                //Resize
                Request RequestBody = new Request()
                {
                    AddSheet = new AddSheetRequest()
                    {
                        Properties = new SheetProperties()
                        {
                            SheetId = sheetId,
                            Title = title,
                            GridProperties = new GridProperties()
                            {
                                RowCount = RowCount,
                                ColumnCount = 2
                            },
                            TabColor = new Color()
                            {
                                Red = 1.0f,
                                Green = 0.3f,
                                Blue = 0.4f
                            }
                        }
                    }
                };

                BatchUpdateSpreadsheetRequest CreateRequest = new BatchUpdateSpreadsheetRequest();
                CreateRequest.Requests = new List<Request>() { RequestBody };
                bool isNew = false;
                try
                {
                    Console.Write("service ready");
                    SpreadsheetsResource.BatchUpdateRequest Deletion = new SpreadsheetsResource.BatchUpdateRequest(service, CreateRequest, spreadsheetId);
                    Deletion.Execute();
                    isNew = true;
                    Console.Write("service start");
                }
                catch (Google.GoogleApiException e)
                {
                    Console.Write("service error");
                    if (e.Message.Contains("already exists"))
                    {
                        RequestBody = new Request()
                        {
                            UpdateSheetProperties = new UpdateSheetPropertiesRequest()
                            {
                                Properties = new SheetProperties()
                                {
                                    SheetId = sheetId,
                                    Title = title,
                                    GridProperties = new GridProperties()
                                    {
                                        RowCount = RowCount,
                                        ColumnCount = 2
                                    },
                                    TabColor = new Color()
                                    {
                                        Red = 1.0f,
                                        Green = 0.3f,
                                        Blue = 0.4f
                                    }
                                },
                                Fields = "*"
                            }
                        };
                    }
                    else if (e.Message.Contains("Google.Apis.Requests.RequestError") && e.Message.Contains(title))
                    {
                        var sheet_metadata = service.Spreadsheets.Get(spreadsheetId).Execute();
                        for (int i = 0; i < sheet_metadata.Sheets.Count; i++)
                        {
                            if (sheet_metadata.Sheets[i].Properties.Title == title)
                            {
                                sheetId = sheet_metadata.Sheets[i].Properties.SheetId;
                                break;
                            }
                        }

                        RequestBody = new Request()
                        {
                            UpdateSheetProperties = new UpdateSheetPropertiesRequest()
                            {
                                Properties = new SheetProperties()
                                {
                                    SheetId = sheetId,
                                    Title = title,
                                    GridProperties = new GridProperties()
                                    {
                                        RowCount = RowCount,
                                        ColumnCount = 2
                                    },
                                    TabColor = new Color()
                                    {
                                        Red = 1.0f,
                                        Green = 0.3f,
                                        Blue = 0.4f
                                    }
                                },
                                Fields = "*"
                            }
                        };
                    }
                    else
                    {
                        throw e;
                    }
                    isNew = false;
                }
                finally
                {
                    if (!isNew)
                    {
                        CreateRequest = new BatchUpdateSpreadsheetRequest();
                        CreateRequest.Requests = new List<Request>() { RequestBody };

                        SpreadsheetsResource.BatchUpdateRequest Deletion = new SpreadsheetsResource.BatchUpdateRequest(service, CreateRequest, spreadsheetId);
                        Deletion.Execute();
                    }
                }
                isInit = true;

                Console.Write("Init complete");
            }
            catch
            {
                Console.Write("Init false");
                isInit = false;
            }
         
        }

        public async Task AuthorizeAsync()
        {

            UserCredential credential;

            using (var stream =
                new FileStream(@"GSTrans\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                credPath = Path.Combine(credPath, "MORT_GOOGLE_TRANS/TOKEN/" + clientID);

                ClientSecrets secret = new ClientSecrets();
                secret.ClientId = clientID;
                secret.ClientSecret = secretKey;


                // "411625139170-las0e2lofii2biqqpud2e5mrhgf5l848.apps.googleusercontent.com";
                // "NnuDC-x2ldyUypNhce448vAb";

                Console.WriteLine("data ready");

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    //GoogleClientSecrets.Load(stream).Secrets,
                    secret,
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
                Console.WriteLine("data cared compl");
                //Credential file saved to: credPath
            }


            Console.WriteLine("data  serv ready");
            // Google Sheets API 서비스 생성.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            Console.WriteLine("data serv comp");
            isInit = true;
        }
        public void InitToken()
        {
            isInit = false;
            r = new Random();
            AuthorizeAsync();

        }
        /// <summary>
        /// 초기화 합니다.
        /// </summary>
        /// <param name="sheetsID">spreadsheetId 값</param>
        public bool Init(string sheetsID, string clientID, string secretKey) {
            bool isComplete = true;
            spreadsheetId = sheetsID;
            this.clientID = clientID;
            this.secretKey = secretKey;

            if(clientID == "")
            {
                isInit = false;
                return false;
            }

            string credPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            credPath = Path.Combine(credPath, "MORT_GOOGLE_TRANS/TOKEN/" +clientID);
            Console.WriteLine(credPath);
            if(Directory.Exists(credPath))
            {
                int fCount = Directory.GetFiles(credPath, "*", SearchOption.TopDirectoryOnly).Length;
                if (fCount > 0)
                {
                    InitToken();
                    Initialize();
                }
                else
                {
                    isComplete = false;
                }
           
            }
            else
            {
               
                isComplete = false;
            }
          

            return isComplete;
        }

        public void DeleteToken()
        {
            try
            {
                string credPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                credPath = Path.Combine(credPath, "MORT_GOOGLE_TRANS/TOKEN/");
                Directory.Delete(credPath, true);
            }
            catch
            {

            }
        }

        public string Translate(string src) {

            if(!isInit)
            {
                return "초기화 실패 - 현재 사용할 수 없습니다.";
            }
            // 요청 파라미터 정의
            string range = Upload(src);
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            // 결과물 출력
            ValueRange response = request.Execute();

            string output = string.Format("{0}", response.Values[0][0]);

            if (output == "#VALUE!") {
                return string.Empty;
            }
            else {
                return output;
            }
        }

        string Upload(string str) {
            string range = string.Format("{0}", r.Next(1, RowCount + 1));
            string googleTranslate = @"=GOOGLETRANSLATE(A" + range + @",""" + source + @""",""" + target + @""")";

            ValueRange VRx = new ValueRange();
            IList<IList<object>> xx = new List<IList<object>>();
            xx.Add(new List<object> { "'" + str, googleTranslate });
            VRx.Values = xx;
            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(VRx, spreadsheetId, title + "!A" + range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            UpdateValuesResponse result = update.Execute();

            /* 두번에 나눠서 처리할 경우, 위 ValueInputOptionEnum를 USERENTERED 말고 RAW를 쓴다
            xx = new List<IList<object>>();
            xx.Add(new List<object> { googleTranslate });
            VRx.Values = xx;
            update = service.Spreadsheets.Values.Update(VRx, spreadsheetId, title + "!B" + range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            result = update.Execute();
            */
            return title + "!B" + range;
        }
    }
}