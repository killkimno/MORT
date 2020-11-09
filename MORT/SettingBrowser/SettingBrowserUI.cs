using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.SettingBrowser
{
    public partial class SettingBrowserUI : Form
    {
        private const string DATA_PATH = "http://killkimno.github.io/MORT_VERSION/Data/SETTING/";
        private const string MAIN_PATH = "Main";
        private class Data
        {
            public string path;
            public string title;
            public string korTitle;

            public Dictionary<string, SettingData> dataList = new Dictionary<string, SettingData>();
            public bool isInit = false;

            public Data()
            {

            }
            //메인 기준으로 먼저 설정한다.
            public Data(string content)
            {
                string[] keys = content.Split('\t');

                if (keys.Length >= 2)
                {
                    path = keys[0];
                    title = keys[1];

                    if (keys.Length >= 3)
                    {
                        korTitle = keys[2];
                    }
                    isInit = true;


                    SettingData data = new SettingData();
                    data.path = MAIN_PATH;
                    dataList.Add(data.path, data);

                }
                else
                {
                    isInit = false;
                }
            }

        }

        private class SettingData
        {
            public string path;
            public string gamePath;
            private string title;
            private string korTitle;
            private string shopLink;
            private string extraLink;
            private string information;
            private string content;

            private string dbPath;
            private string settingPath;

            public bool isInit;

            public SettingData()
            {

            }

            public void Init(string path, string gamePath, string content)
            {
                isInit = false;

                this.path = path;
                this.gamePath = gamePath;
                title= Util.ParseString(content, "@TITLE", '[', ']');
                korTitle = Util.ParseString(content, "@KOR_TITLE", '[', ']');
                shopLink = Util.ParseString(content, "@STORE", '[', ']');
                extraLink = Util.ParseString(content, "@LINK", '[', ']');

                information = Util.ParseString(content, "@INFO", '[', ']');

                information = information.Replace("\r\n", "\n");
                information = information.Replace("\n", System.Environment.NewLine);

                dbPath = Util.ParseString(content, "@DB", '[', ']');
                settingPath = Util.ParseString(content, "@SETTING ", '[', ']');

                isInit = true;
            }

            public string GetTitle()
            {
                string title = this.title;

                if(!string.IsNullOrEmpty(korTitle))
                {
                    title += " / " + korTitle;
                }
                

                return title;
            }

            public string GetShop()
            {
                return shopLink;
            }

            public string GetExtraLink()
            {
                return extraLink;
            }

            public string GetInformation()
            {
                return information;
            }

            public string GetDBPath()
            {
                string result = "";

                if(!string.IsNullOrEmpty(dbPath))
                {
                    result = DATA_PATH + gamePath + "/" + path + "/" + dbPath;
                }
            
                return result;
            }

            public string GetDBFileName()
            {
                return dbPath;
            }

            public string GetSettingPath()
            {
                string result = "";

                if (!string.IsNullOrEmpty(settingPath))
                {
                    result = DATA_PATH + gamePath + "/" + path + "/" + settingPath;
                }

                Util.ShowLog(result);
                return result;
            }

            public string GetSettingFileName()
            {
                return settingPath;
            }

        }

        private List<Data> dataList = new List<Data>();
        private SettingData selectedData = null;
        public SettingBrowserUI()
        {
            InitializeComponent();
        }

        public void Init()
        {
            tbInformation.SelectionHangingIndent = 11;
            listView1.FullRowSelect = true;
            listView1.BeginUpdate();

            listView1.Items.Clear();

            DownloadList();
            for (int i = 0; i< dataList.Count; i++)
            {
                Data data = dataList[i];
                ListViewItem item = new ListViewItem(data.title);
                item.SubItems.Add(data.korTitle);
                item.Tag = data;
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();
        }


        private void DownloadList()
        {
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead("http://killkimno.github.io/MORT_VERSION/Data/SETTING/list.txt");
                using (StreamReader reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Data data = new Data(line);

                        if(data.isInit)
                        {
                            dataList.Add(data);
                        }                    

                    }
                }

            }
        }

        private void ShowInformation(ListViewItem item, string key)
        {
            selectedData = null;
            Data data = item.Tag as Data;

            bool isRequireDownload = false;
            SettingData settingData = null;
            //추후에 
            if (data.dataList.ContainsKey(key))
            {
                settingData = data.dataList[key];

                if(!settingData.isInit)
                {
                    isRequireDownload = true;
                }
            }
            else
            {
                isRequireDownload = true;
            }


            if(isRequireDownload)
            {
                using (WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead(DATA_PATH + data.path + "/" + key + "/info.txt");
                    using (StreamReader reader = new StreamReader(stream))
                    {

                        if(settingData == null)
                        {
                            settingData = new SettingData();
                        }
                        settingData.Init(key,data.path, reader.ReadToEnd());
                    }
                }
            }


            if(settingData != null && settingData.isInit)
            {
                lbTitleResult.Text = settingData.GetTitle();
                lbLinkShop.Text = settingData.GetShop();
                lbLinkExtra.Text = settingData.GetExtraLink();
                tbInformation.Text = settingData.GetInformation();
                selectedData = settingData;
            }


           
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = null;

            if(listView1.Items.Count > 0)
            {
                items = listView1.SelectedItems;

                if(items.Count > 0)
                {
                    ListViewItem item = items[0];
                    ShowInformation(item, MAIN_PATH);
                }
            }
         
        }

        private void lbLinkShop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lbLinkExtra_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void tbInformation_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {

                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch { }
        }

        private void btAppaly_Click(object sender, EventArgs e)
        {
            if(selectedData != null)
            {
                string settingPath = selectedData.GetSettingPath();
                string dbPath = selectedData.GetDBPath();

                if(settingPath != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        Stream stream = client.OpenRead(settingPath);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string data = reader.ReadToEnd();
                            data = data.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                            Util.SaveFile(GlobalDefine.SETTING_PATH + selectedData.GetSettingFileName(), data);
                        }

                    }
                }

                if(dbPath != "")
                {
                    using (WebClient client = new WebClient())
                    {
                        Stream stream = client.OpenRead(dbPath);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string data = reader.ReadToEnd();
                            data = data.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                            Util.SaveFile(GlobalDefine.DB_PATH + selectedData.GetDBFileName(), data);
                        }

                    }
                }
               
            }
        }
    }
}
