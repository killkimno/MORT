using Microsoft.VisualBasic.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
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

        private string _prepare = "Preparing for download";
        private string _downloadError = "Error has occurred! Please manually update";
        private string _downloadComplete = "update completed! Run Mort again";
        private string _completeMessage = "The update has been completed. Would you like to check your update history?";
        private string _completeTitle = "update completed!";
        private string _errorMessage = "Failed to update. Please update Would you like to go to the download page?";
        private string _errorTitle = "Update failure!";
        private string _downloadConfig = "Settings file download";
        private string _downloading = "downloading";


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

        public Updater(string newVersion, string url, string info, string localize)
        {
            InitializeComponent();
            this.newVersion = newVersion;
            this.url = url;
            this.info = info;
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

        private async Task AfterAsync(bool isError)
        {
            await Task.Delay(TimeSpan.FromSeconds(2.5f));
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
