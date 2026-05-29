using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MORT.VersionCheck
{
    internal class VersionCheckLogic
    {
        //개발용 버전인가?
        public readonly bool IsDevVersion = false;
        private readonly IMainFormContract contract;

        //현재 버전
        private int nowVersion = 1171;

        public VersionCheckLogic(IMainFormContract contract)
        {
            this.contract = contract;
        }

        #region :::::::::::::::::::::::::::::::::::::::::::버전 확인 관련 :::::::::::::::::::::::::::::::::::::::::

        public bool GetCheckUpdate()
        {
            bool isCheckUpdate = true;


            //구버전 파일은 없앤다.
            if (File.Exists(GlobalDefine.CHECK_UPDATE_FILE))
            {
                File.Delete(GlobalDefine.CHECK_UPDATE_FILE);
            }

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_UPDATE ", '[', ']');


            if (!Boolean.TryParse(result, out isCheckUpdate))
            {
                isCheckUpdate = true;
            }


            return isCheckUpdate;
        }

        public void SetCheckUpdate(bool isUse)
        {
            Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_UPDATE ", isUse.ToString());
        }

        #endregion

        //version.txt 등 외부에서 받은 URL은 평문 http로 올 수 있으므로 다운로드 전 https로 강제한다(MITM 방지)
        private static string ForceHttps(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return "https://" + url.Substring("http://".Length);
            }
            return url;
        }

        //업데이트 파일 옆 .sha256 다운로드. 실패/없음 -> 빈 문자열 반환(호출 측에서 "준비 안 됨"으로 해석).
        private static string TryDownloadHash(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string content = client.DownloadString(url);
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return "";
                    }
                    //파일 안에 해시만 있는 경우, sha256sum 호환 포맷("hash *filename") 모두 첫 토큰만 취한다
                    string first = content.Trim().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    return first.Trim();
                }
            }
            catch (Exception e)
            {
                Util.ShowLog("TryDownloadHash failed: " + e.Message);
                return "";
            }
        }

        //버전 확인.
        public void CheckVersion()
        {
            if (GetCheckUpdate())
            {
                try
                {
                    nowVersion = Properties.Settings.Default.MORT_VERSION_VALUE;
                    //http://killkimno.github.io/MORT_VERSION/version.txt
                    using (WebClient client = new WebClient())
                    {
                        Stream stream = client.OpenRead("https://killkimno.github.io/MORT_VERSION/version.txt");
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            String content = reader.ReadToEnd();
                            content = content.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                            Util.ShowLog("--- Version : " + content);

                            CheckMortVersion(content);

                            if (!Program.IS_FORCE_QUITE)
                            {
                                CheckDicVersion(content, "@MORT_DIC_ENG", @".\\DIC\\dic.txt");
                                CheckDicVersion(content, "@MORT_DIC_JPN", @".\\DIC\\dicJpn.txt");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Util.ShowLog(e.ToString());
                }
            }

        }

        public void CheckDefaultSetting()
        {
            try
            {
                //http://killkimno.github.io/MORT_VERSION/default_setting.txt
                using (WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead("https://killkimno.github.io/MORT_VERSION/default_setting.txt");
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        String content = reader.ReadToEnd();
                        content = content.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                        Util.ShowLog("--- default setting : " + content);

                        string naver = Util.ParseString(content, "@SPLITE_NAVER_TOKEN", '{', '}');
                        string google = Util.ParseString(content, "@SPLITE_GOOGLE_TOEKN ", '{', '}');
                        string isUseAdvence = Util.ParseString(content, "@ENABLE_ADVENCED_TOKEN", '{', '}');

                        string deeplUrl = Util.ParseString(content, "@DEEPL_URL", '{', '}');
                        string deeplFormat = Util.ParseString(content, "@DEEPL_FORMAT", '"', '"');
                        string deeplElementTarget = Util.ParseString(content, "@DEEPL_TARGET", '{', '}');
                        if (naver != "" && google != "")
                        {
                            Util.SetSpliteToken(naver, google, isUseAdvence);
                        }

                        if(deeplUrl != "" && deeplFormat != "")
                        {
                            GlobalDefine.DeeplFrontUrl = deeplUrl;
                            GlobalDefine.DeeplFormat = deeplFormat;
                            GlobalDefine.DeeplElementTarget = deeplElementTarget;

                            //TODO : 현재 {}로 하기 때문에 아래 문구를 넣을 수 없다, 방식을 바꿔야 한다
                            GlobalDefine.DeeplElementTarget = @"
    (function() {
        var mainDiv = document.getElementsByClassName('relative flex flex-1 flex-col')[0];
        if (!mainDiv) {
            return '';
        }

        var alternativesPanel = mainDiv.querySelector('[data-testid=""translator-target-result-alternatives-panel""]');
        if (alternativesPanel) {
            alternativesPanel.remove();
        }

        return mainDiv.innerText;
    })();
    ";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Util.ShowLog(e.ToString());
            }
        }

        /// <summary>
        /// MORT 버전 확인
        /// </summary>
        /// <param name="content"></param>
        private void CheckMortVersion(String content)
        {
            try
            {
                if (content != null)
                {
                    string versionKey = "@MORT_VERSION ";
                    string minorKey = "@MORT_MINOR_VERSION ";
                    string bitUpdateKey = "@MORT_64_BIT_MESSSAGE ";
                    string newVersionString = "";
                    string minorVersionString = "";
                    string bitMessageString = "";
                    string downloadPage = "";

                    if (IsDevVersion)
                    {
                        versionKey = "@DEV_VERSION ";
                        minorKey = "@DEV_MINOR_VERSION ";
                    }

                    newVersionString = Util.ParseString(content, versionKey, '[', ']');
                    downloadPage = Util.ParseString(content, versionKey, '{', '}');
                    minorVersionString = Util.ParseString(content, minorKey, '[', ']');

                    //1.230까지만 쓴다
                    bitMessageString = Util.ParseString(content, bitUpdateKey, '[', ']');


                    UpdateType updateType = Util.GetUpdateType(nowVersion, newVersionString, minorVersionString);

                    Util.ShowLog("------------------" + System.Environment.NewLine + "Now : " + nowVersion + " / New : " + newVersionString + " / Minor : " + minorVersionString + " / Result : " + updateType.ToString());
                    
                    //1. 버전 비교를 한다.
                    if (updateType == UpdateType.Major)
                    {
                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        if (string.IsNullOrEmpty(bitMessageString) || nowVersion >= 1240)
                        {
                            bitMessageString = LocalizeString("New Update Message", true);
                        }

                        string checkMessageSubtitle = "(" + LocalizeManager.LocalizeManager.GetLocalizeString("Major Update") + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show(bitMessageString, checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            Logo.SetTopmost(false);
                            try
                            {
                                Logo.SetTopmost(true);
                                contract.ForceDisableTopMost();
                                Util.OpenURL(downloadPage);
                            }
                            catch { }
                        }
                        else
                        {

                        }
                    }
                    else if (updateType == UpdateType.Minor)
                    {

                        string fileUrl = Util.ParseString(content, $"{minorKey}[{minorVersionString}]", '{', '}');

                        //업데이트 사전 검증: 바이너리 옆 .sha256 파일을 먼저 받아 본다.
                        //- URL 미기재(@MORT_MINOR_HASH 없음) -> 구 version.txt 호환, 검증 건너뜀(경고 로그)
                        //- URL은 있는데 다운 실패 -> 바이너리가 아직 준비 안 됨, 사용자에게 묻지 않고 조용히 종료
                        string hashUrl = Util.ParseString(content, $"@MORT_MINOR_HASH [{minorVersionString}]", '{', '}');
                        string expectedHash = "";

                        if (!string.IsNullOrEmpty(hashUrl))
                        {
                            expectedHash = TryDownloadHash(ForceHttps(hashUrl));
                            if (string.IsNullOrEmpty(expectedHash))
                            {
                                Util.ShowLog("Update sha256 not available yet, skipping update prompt: " + hashUrl);
                                return;
                            }
                        }
                        else
                        {
                            Util.ShowLog("Warning: @MORT_MINOR_HASH not in version.txt, proceeding without verification");
                        }

                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        string checkMessageSubtitle = "(" + LocalizeManager.LocalizeManager.GetLocalizeString("Minor Update") + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show(LocalizeString("Minor Update Message", true), checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            string str = GetUpdaterLocalizeString();
                            Program.IS_FORCE_QUITE = true;

                            ProcessStartInfo psi = new ProcessStartInfo();
                            psi.WorkingDirectory = System.Environment.CurrentDirectory;
                            psi.FileName = "Updater.exe";

                            psi.ArgumentList.Add(newVersionString);
                            psi.ArgumentList.Add(fileUrl);
                            psi.ArgumentList.Add(downloadPage);
                            psi.ArgumentList.Add(str);
                            psi.ArgumentList.Add(expectedHash);
                            //psi.Arguments = $"{newVersionString} {fileUrl} {downloadPage} {str} {expectedHash}";
                            Process.Start(psi);

                            if (Application.MessageLoop)
                                Application.Exit();
                            else
                                Environment.Exit(1);

                            return;
                        }
                        else
                        {

                        }

                    }
                    else if (updateType == UpdateType.None)
                    {
                        if (IsDevVersion)
                        {
                            int finishedVersion = Util.ParseInt(content, "@MORT_DEV_FINISHED_VERSION ");

                            if (finishedVersion >= nowVersion)
                            {
                                string checkMessageSubtitle = LocalizeManager.LocalizeManager.GetLocalizeString("Release Update Title");
                                if (DialogResult.OK == MessageBox.Show(LocalizeManager.LocalizeManager.GetLocalizeString("Release Update Message"), checkMessageSubtitle, MessageBoxButtons.OKCancel))
                                {
                                    Logo.SetTopmost(false);
                                    try
                                    {
                                        Logo.SetTopmost(true);
                                        contract.ForceDisableTopMost();
                                        downloadPage = Util.ParseString(content, "@MORT_VERSION ", '{', '}');

                                        Util.OpenURL(downloadPage);
                                    }
                                    catch { }
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                
            }
        }

        private string GetUpdaterLocalizeString()
        {
            List<string> dataList = new List<string>();
            
            dataList.Add(LocalizeString("Updater Prepare"));
            dataList.Add(LocalizeString("Updater DownloadError"));
            dataList.Add(LocalizeString("Updater Complete"));
            dataList.Add(LocalizeString("Updater CompleteMessage"));
            dataList.Add(LocalizeString("Updater CompleteTitle"));
            dataList.Add(LocalizeString("Updater ErrorMessage"));
            dataList.Add(LocalizeString("Updater ErrorTitle"));
            dataList.Add(LocalizeString("Updater DownloadConfig"));
            dataList.Add(LocalizeString("Updater Downloading"));
            dataList.Add(LocalizeString("Updater HashMismatch"));   // keys[9] - 스프레드시트에서 키 추가 시 자동 적용

            return dataList.Aggregate((text, next) => text + "_" + next);
        }

        private void UpdateDic(string fileName, String data)
        {
            //Util.ShowLog("Dic data : " + data);
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    using (StreamWriter file = new StreamWriter(fileName, false, utf8WithoutBom))
                    {
                        file.WriteLine(data);
                        file.Write(System.Environment.NewLine);

                        Util.ShowLog("Dic update complete");
                    }

                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter file = new StreamWriter(fileName, true, utf8WithoutBom))
                        {
                            file.WriteLine(data);
                            file.Write(System.Environment.NewLine);
                        }
                    }
                }
            }
        }

        private void CheckDicVersion(String content, string checkType, string fileName)
        {
            try
            {
                if (content != null)
                {
                    int dicVersion = 0;
                    string result = Util.ParseStringFromFile(GlobalDefine.DATA_VERSION_FILE, checkType, '[', ']');

                    if (!string.IsNullOrEmpty(result))
                    {
                        dicVersion = Convert.ToInt32(result);
                    }


                    string newVersionString = "";
                    string downloadPage = "";

                    newVersionString = Util.ParseString(content, checkType, '[', ']');
                    downloadPage = Util.ParseString(content, checkType, '{', '}');
                    //var path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                    Util.ShowLog(checkType + " : " + dicVersion + " / " + newVersionString + " / download : " + downloadPage);

                    if (dicVersion < Convert.ToInt32(newVersionString))
                    {
                        using (WebClient client = new WebClient())
                        {
                            Stream stream = client.OpenRead(ForceHttps(downloadPage));
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                String dicData = reader.ReadToEnd();
                                dicData = dicData.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                                UpdateDic(fileName, dicData);
                                Util.ChangeFileData(GlobalDefine.DATA_VERSION_FILE, checkType, newVersionString, '[', ']');

                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        //TODO : 인터페이스로 변경하자
        private string LocalizeString(string key, bool replaceLine = false)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key).Replace("[]", "");
        }

    }
}
