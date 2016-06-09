using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MORT
{
    public partial class KeyInputLabel : UserControl
    {
        public bool isFocus;
        public List<Keys> keyList = new List<Keys>();
        public KeyInputLabel()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys code = e.KeyCode;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
            {
                code = Keys.ShiftKey;
            }
            else if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                code = Keys.ControlKey;
            }
            else if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
            {
                code = Keys.Menu;
            }

            if(e.KeyCode == Keys.Escape)
            {
                textBox1.Text = "";
                keyList.Clear();
            }
            else if(e.KeyCode == Keys.Back)
            {
                textBox1.Text = "";
                keyList.Clear();
            }
            else
            {
                if(keyList.Count >=3)
                {
                    return;
                }
                for(int i = 0 ; i < keyList.Count; i++)
                {
                    if (keyList[i] == code)
                    {
                        return;
                    }
                }
               
                if (keyList.Count != 0)
                {
                    textBox1.Text += '+';
                }
                keyList.Add(code);
                textBox1.Text += code;

            }
            
        }

        public string GetKeyListToString()
        {
            string result = "";

            for (int i = 0; i < keyList.Count; i++ )
            {
                result += keyList[i];
                result += " ";
            }


                return result;
        }

        public void SetKeyList(string data)
        {
            char[] token = new char[] { ' ' };
            string[] keys = data.Split(token, StringSplitOptions.RemoveEmptyEntries);
            textBox1.Text = "";
            keyList.Clear();
            for (int i = 0; i < keys.Length; i++ )
            {
                Keys test = (Keys)System.Enum.Parse(typeof(Keys), keys[i]);
                keyList.Add(test);
                if (keyList.Count != 1)
                {
                    textBox1.Text += "+";
                }
                textBox1.Text += test;                
            }
        }

        public bool GetIsCorrect(List<Keys> list)
        {
            bool isCorrect = false;
            if(list.Count == keyList.Count && keyList.Count > 0)
            {
                for (int i = 0; i < list.Count; i++ )
                {
                    if(list[i]!= keyList[i])
                    {
                        isCorrect = false;
                        break;
                    }
                    else
                    {
                        isCorrect = true;
                    }
                }
            }

            return isCorrect;
        }

        public void ResetInput(List<Keys> list)
        {
            isFocus = false;
            this.keyList.Clear();
            textBox1.Text = "";
            for (int i = 0; i < list.Count; i++ )
            {
                if (keyList.Count != 0)
                {
                    textBox1.Text += '+';
                }
                keyList.Add(list[i]);
                textBox1.Text += list[i];
            }

        }

        public void SetText(string text)
        {
            textBox1.Text = text;
        }

        private void KeyInputLabel_Enter(object sender, EventArgs e)
        {
            isFocus = true;
        }

        private void KeyInputLabel_Leave(object sender, EventArgs e)
        {
            isFocus = false;
        }
    }
}
