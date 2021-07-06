using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.Updater
{
    public partial class Updater : Form
    {
        private string newVersion = "";
        private string url = "";
        private string info = "";

        public Updater()
        {
            InitializeComponent();
        }

        public void DoDownload(string newVersion, string url, string info)
        {

            this.newVersion = newVersion;
            this.url = url;
            this.info = info;
            try
            {
                lbStatus.Text = "다운로드 준비중";
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


        private void DownloadConfig(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                lbStatus.Text = "설정 파일 다운로드";
                using (var client = new WebClient())
                {
                    Uri uri = new Uri(url + ".config");
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);
                    client.DownloadFileAsync(uri, "MORT_backup.exe.config");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DoDownloadAfter);
                }
            }
            catch
            {

            }

        }

        private void DoDownloadAfter(object sender, AsyncCompletedEventArgs e)
        {
          
            bool isError = false;
            if (e.Cancelled || e.Error != null)
            {
                isError = true;   
            }


            if(!isError)
            {
                try
                {
                    RemoveOldFile("MORT.exe", "MORT_backup.exe", "MORT_2.exe");
                    RemoveOldFile("MORT.exe.config", "MORT_backup.exe.config", "MORT_2.exe.config");
                }
                catch
                {
                    isError = true;
                    lbStatus.Text = "오류가 발생했습니다! 수동 업데이트를 해주시기 바랍니다";
                }
            }

            isError = true;

            if (!isError)
            {
                lbStatus.Text = "업데이트 완료!" + System.Environment.NewLine + "MORT를 다시 실행합니다";
                
         
                if (DialogResult.OK == MessageBox.Show("업데이트를 완료했습니다.\r\n업데이트 내역을 확인해 보시겠습니까?", "업데이트 완료!", MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(info);
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

                if (DialogResult.OK == MessageBox.Show("업데이트를 실패했습니다.\r\n수도 업데이트를 해주시기 바랍니다\r\n\r\n다운로드 페이지로 이동하시겠습니까?", "업데이트 실패!", MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch { }
                    Application.Exit();
                }
            }
        }

        private void RemoveOldFile(string originalFile, string downloadFile, string backupFile)
        {
            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
            }

            File.Move(originalFile, backupFile);
            File.Move(downloadFile, originalFile);

        }

        private async void DoClose()
        {

            await Task.Delay(500);


            Process.Start("MORT.exe");
         
           
        }

        private void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {
            Util.ShowLog("!");
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);

            lbStatus.Text = "받는 중 : " + e.ProgressPercentage.ToString() + "%";
            progressBar1.Value = e.ProgressPercentage;

          
        }
    }
}
