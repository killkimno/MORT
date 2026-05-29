using Microsoft.VisualBasic.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class Updater : Form
    {
        private string newVersion = "";
        private string url = "";
        private string info = "";
        private string expectedHash = "";

        private string _prepare = "Preparing for download";
        private string _downloadError = "Error has occurred! Please manually update";
        private string _downloadComplete = "update completed! Run Mort again";
        private string _completeMessage = "The update has been completed. Would you like to check your update history?";
        private string _completeTitle = "update completed!";
        private string _errorMessage = "Failed to update. Please update Would you like to go to the download page?";
        private string _errorTitle = "Update failure!";
        private string _downloadConfig = "Settings file download";
        private string _downloading = "downloading";
        private string _hashMismatch = "Update file is corrupted or tampered. Please try again later.";


        //다운로드 URL은 외부 version.txt에서 전달되므로 평문 http일 수 있다. 실행 파일을 받기 전 https로 강제한다(MITM 방지)
        private static string ForceHttps(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return "https://" + url.Substring("http://".Length);
            }
            return url;
        }

        public void OpenURL(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {

            }

        }

        public Updater(string newVersion, string url, string info, string localize, string expectedHash = "")
        {
            InitializeComponent();
            this.newVersion = newVersion;
            this.url = ForceHttps(url);
            this.info = info;
            this.expectedHash = (expectedHash ?? "").Trim();
            ParseLocalize(localize);
        }

        private void ParseLocalize(string localize)
        {
            try
            {
                var keys = localize.Split('_');


                foreach (string arg in keys)
                {
                    Console.WriteLine(arg);
                }

                string prepare = keys[0];
                string downloadError = keys[1];
                string downloadComplete = keys[2];
                string completeMessage = keys[3];
                string completeTitle = keys[4];
                string errorMessage = keys[5];
                string errorTitle = keys[6];
                string downloadConfig = keys[7];
                string downloading = keys[8];

                _prepare = prepare;
                _downloadError = downloadError;
                _downloadComplete = downloadComplete;
                _completeMessage = completeMessage;
                _completeTitle = completeTitle;
                _errorMessage = errorMessage;
                _downloadConfig = downloadConfig;
                _downloading = downloading;
            }
            catch (Exception e)
            {
                _downloading = "에러 떴음 " + e.ToString();
            }

            //keys[9] (HashMismatch) - 구 호환: 길이 부족하거나 빈 문자열이면 영어 디폴트 유지
            try
            {
                var extra = localize.Split('_');
                if (extra.Length > 9 && !string.IsNullOrWhiteSpace(extra[9]))
                {
                    _hashMismatch = extra[9];
                }
            }
            catch
            {
            }
        }

        public void DoDownload()
        {
            try
            {
                lbStatus.Text = _prepare;
                using (var client = new WebClient())
                {
                    Uri uri = new Uri(url);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);
                    client.DownloadFileAsync(uri, "MORT_backup.exe"); 
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadConfig);
                }
            }
            catch
            {

            }
        }

        //다운받은 파일의 SHA256을 계산해 hex 대문자로 반환
        private static string ComputeSha256(string filePath)
        {
            using (var sha = SHA256.Create())
            using (var fs = File.OpenRead(filePath))
            {
                byte[] hash = sha.ComputeHash(fs);
                var sb = new System.Text.StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        //expectedHash가 비어있지 않으면 MORT_backup.exe와 비교한다. 불일치 시 다운로드 파일 삭제 + true 반환(isError 처리)
        private bool VerifyDownloadedHash()
        {
            if (string.IsNullOrEmpty(expectedHash))
            {
                Console.WriteLine("expectedHash empty, skipping verification");
                return false;
            }

            const string downloadFile = "MORT_backup.exe";
            if (!File.Exists(downloadFile))
            {
                Console.WriteLine("Downloaded file not found: " + downloadFile);
                return true;
            }

            string actual = ComputeSha256(downloadFile);
            if (!string.Equals(actual, expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Hash mismatch. expected={expectedHash} actual={actual}");
                try { File.Delete(downloadFile); } catch { }
                lbStatus.Text = _hashMismatch;
                return true;
            }

            Console.WriteLine("Hash verified: " + actual);
            return false;
        }

        private async Task AfterAsync(bool isError)
        {
            await Task.Delay(TimeSpan.FromSeconds(2.5f));

            if (!isError)
            {
                //무결성 검증 - 실패 시 RemoveOldFile에 진입조차 않음(기존 MORT.exe 보호)
                if (VerifyDownloadedHash())
                {
                    isError = true;
                }
            }

            if (!isError)
            {
                try
                {
                    RemoveOldFile("MORT.exe", "MORT_backup.exe", "MORT_2.exe");
                    RemoveOldFile("MORT.dll.config", "MORT_backup.dll.config", "MORT_2.dll.config");
                }
                catch (Exception excep)
                {
                    Console.WriteLine(excep);
                    isError = true;
                    lbStatus.Text = _downloadError;
                }
            }

            if (!isError)
            {
                lbStatus.Text = _downloadComplete;


                if (DialogResult.OK == MessageBox.Show(_completeMessage, _completeTitle, MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        OpenURL(info);
                    }
                    catch { }
                    DoClose();
                }
                else
                {
                    DoClose();
                }
            }
            else
            {
                if (DialogResult.OK == MessageBox.Show(_errorMessage, _errorTitle, MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        OpenURL(info);
                    }
                    catch { }

                    if (Application.MessageLoop)
                        Application.Exit();
                    else
                        Environment.Exit(1);
                }
            }
        }

        private void DoDownloadAfter(object sender, AsyncCompletedEventArgs e)
        {
            bool isError = false;
            if (e.Cancelled || e.Error != null)
            {
                isError = true;
            }

            var result = Task.Run(async () => await AfterAsync(isError));

        }

        private void DownloadConfig(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                lbStatus.Text = _downloadConfig;
                using (var client = new WebClient())
                {
                    string configUrl = url.Replace("MORT.exe", "MORT.dll.config");
                    Uri uri = new Uri(configUrl);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);
                    client.DownloadFileAsync(uri, "MORT_backup.dll.config");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DoDownloadAfter);
                }
            }
            catch
            {

            }

        }

        private void RemoveOldFile(string originalFile, string downloadFile, string backupFile)
        {

            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
                Console.WriteLine("Delete backup - " + backupFile);
            }

            if(WaitForFile(originalFile))
            {
                File.Move(originalFile, backupFile);
                Console.WriteLine("originalFile to back - " + originalFile);
            }
            else
            {
                throw new Exception("File still locked");
            }


            if (WaitForFile(downloadFile))
            {
                File.Move(downloadFile, originalFile);
                Console.WriteLine("downloadFile to originalFile - " + downloadFile);
            }
            else
            {
                throw new Exception("File still locked");
            }        

        }

        private bool WaitForFile(string fullPath)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath,
                        FileMode.Open, FileAccess.ReadWrite,
                        FileShare.None, 100))
                    {
                        fs.ReadByte();

                        // If we got this far the file is ready
                        break;
                    }
                }
                catch (Exception ex)
                {

                    if (numTries > 10)
                    {
                        return false;
                    }

                    // Wait for the lock to be released
                    System.Threading.Thread.Sleep(500);
                }
            }

            return true;
        }

        private async void DoClose()
        {
            Process.Start("MORT.exe");

            if (Application.MessageLoop)
                Application.Exit();
            else
                Environment.Exit(1);
        }

        private void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);

            lbStatus.Text = _downloading + e.ProgressPercentage.ToString() + "%";
            progressBar1.Value = e.ProgressPercentage;

          
        }

        private void Updater_Load(object sender, EventArgs e)
        {
           DoDownload();
        }
    }
}
