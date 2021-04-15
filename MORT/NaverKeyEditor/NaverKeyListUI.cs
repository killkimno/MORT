using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public partial class NaverKeyListUI : Form
    {
        private bool isLockAutoChange = false;
        private Action callback;
        List<TransManager.NaverKeyData> dataList = new List<TransManager.NaverKeyData>();
        public NaverKeyListUI()
        {
            InitializeComponent();
        }

        public void Init(Action callback)
        {
            this.callback = callback;
            isLockAutoChange = false;
            listBox_NaverKey.UseTabStops = true;
            listBox_NaverKey.UseCustomTabOffsets = true;
          

           
            dataList = TransManager.Instace.naverKeyList;

            RenderList();


       
            CheckButtonState();
        }

        private void RenderList()
        {
            ListBox.IntegerCollection offsets = listBox_NaverKey.CustomTabOffsets;
            offsets.Clear();
            listBox_NaverKey.Items.Clear();
            listBox_NaverKey.SelectedItem = null;

            for (int i = 0; i < dataList.Count; i++)
            {
                string apiType = "";

                if (dataList[i].isPaid)
                {
                    apiType = "유료 API";
                }
                else
                {
                    apiType = "무료 API";
                }

                listBox_NaverKey.Items.Add("id : " + dataList[i].id  + " \t타입 : " + apiType + " \t상태 : " + dataList[i].eNMTstate.ToString());
            }

            offsets.Add(120);
            offsets.Add(15);

        }

        private void CheckButtonState()
        {
            string id = TextBox_NaverID.Text;
            string secret = TextBox_NaverSecret.Text;


            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(secret))
            {
                //아무것도 못함.
                modfiButton.Enabled = false;
                radioFree.Enabled = false;
                radioPaid.Enabled = false;
                radioFree.Checked = false;
                radioPaid.Checked = false;
                return;
            }

            if (modfiButton.Enabled == false)
            {
                modfiButton.Enabled = true;
            }

            radioFree.Enabled = true;
            radioPaid.Enabled = true;

            if(!radioPaid.Checked && !radioPaid.Checked)
            {
                radioFree.Checked = true;
            }

            bool isFound = false;
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].id == id)
                {
                    isFound = true;
                    modfiButton.Text = "수정";
                    //무조건 수정

                    if(dataList[i].isPaid)
                    {
                        radioPaid.Checked = true;
                    }
                    else
                    {
                        radioFree.Checked = true;
                    }

                    break;
                }
            }

            if (!isFound)
            {
                modfiButton.Text = "추가";
            }
        }



        private void NaverKeyListUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save 루틴 필요.
            if (TransManager.GetIsRemain())
            {
                TransManager.Instace.naverKeyList = dataList;

                string id = "";
                string secret = "";
                bool isPaid = false;

                if (TransManager.Instace.naverKeyList.Count > 0)
                {
                    id = TransManager.Instace.naverKeyList[0].id;
                    secret = TransManager.Instace.naverKeyList[0].secret;

                    isPaid = TransManager.Instace.naverKeyList[0].isPaid;
                }


                TransManager.Instace.SaveNaverKeyFile(id, secret, isPaid);

                NaverTranslateAPI.instance.ChangeValue(id, secret, isPaid);
            }

            if (FormManager.GetIsRemain())
            {
                FormManager.Instace.DestoryNaverKeyListUI();
            }

            if(callback != null && callback.Target != null)
            {
                callback();
            }
        }

        private void OnClick_Add(object sender, EventArgs e)
        {
            string result = "";

            if (TextBox_NaverID.Text == string.Empty || TextBox_NaverSecret.Text == string.Empty)
            {
                return;
            }

            string id = TextBox_NaverID.Text;
            string secret = TextBox_NaverSecret.Text;

            id = id.Replace(" ", "");
            secret = secret.Replace(" ", "");

            bool isFound = false;
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].id == id)
                {
                    isFound = true;
                    dataList[i].secret = secret;
                    dataList[i].eNMTstate = TransManager.NaverKeyData.eState.Normal;

                    if (radioFree.Checked)
                    {
                        dataList[i].isPaid = false;
                    }
                    else if(radioPaid.Checked)
                    { 
                        dataList[i].isPaid = true;
                    }
                    break;
                }
            }

            if (!isFound)
            {
                if (dataList.Count >= TransManager.MAX_NAVER)
                {
                    string message = "최대 " + TransManager.MAX_NAVER + "개 까지만 등록 가능합니다";
                    MessageBox.Show(message);
                    return;
                }

                string apiType = "";

                if(radioFree.Checked)
                {
                    apiType = "무료 API";
                }
                else if(radioPaid.Checked)
                {
                    apiType = "유료 API";
                }

                result = "id : " + id  + " \t타입 : " + apiType + " \t상태 : " + TransManager.NaverKeyData.eState.Normal.ToString();
                listBox_NaverKey.Items.Add(result);

                dataList.Add(new TransManager.NaverKeyData(id, secret));
            }
            else
            {
                RenderList();
            }


            isLockAutoChange = true;
            TextBox_NaverID.Text = "";
            TextBox_NaverSecret.Text = "";
            isLockAutoChange = false;

            CheckButtonState();

        }

        private void OnClick_Remove(object sender, EventArgs e)
        {
            if (listBox_NaverKey.SelectedItem != null)
            {
                int index = listBox_NaverKey.SelectedIndex;

                if (index == 0)
                {
                    MessageBox.Show("첫 번째 항목은 지울 수 없습니다.");
                    return;
                }
                listBox_NaverKey.Items.Remove(listBox_NaverKey.SelectedItem);

                string id = dataList[index].id;


                if (id == TextBox_NaverID.Text)
                {
                    isLockAutoChange = true;

                    TextBox_NaverID.Text = "";
                    TextBox_NaverSecret.Text = "";

                    isLockAutoChange = false;
                }

                dataList.RemoveAt(index);

                CheckButtonState();

            }
        }

        private void listBox_NaverKey_SelectedValueChanged(object sender, EventArgs e)
        {
            int index = listBox_NaverKey.SelectedIndex;
            if (index < dataList.Count && listBox_NaverKey.SelectedItem != null)
            {
                isLockAutoChange = true;
                TextBox_NaverID.Text = dataList[index].id;
                TextBox_NaverSecret.Text = dataList[index].secret;
                isLockAutoChange = false;

                CheckButtonState();
            }
        }



        private void TextBox_NaverSecret_TextChanged(object sender, EventArgs e)
        {
            CheckButtonState();
        }
    }
}
