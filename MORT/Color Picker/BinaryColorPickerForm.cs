using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MORT
{
    public partial class BinaryColorPickerForm : Form
    {
        private PictureBox originalScreen;
        private PictureBox binaryScreen;

        public BinaryColorPickerForm()
        {
            InitializeComponent();
        }

        public void Setup( PictureBox original, PictureBox binary)
        {
            originalScreen = original;
            binaryScreen = binary;
        }

        private void ProcessBinary()
        {
            Bitmap screenBitmap = (Bitmap)originalScreen.Image;
            Bitmap binaryBitmap = (Bitmap)binaryScreen.Image;


            if (modeComboBox.SelectedIndex == 0)
            {
                int r = Convert.ToInt32(rTextBox.Text);
                int g = Convert.ToInt32(gTextBox.Text);
                int b = Convert.ToInt32(bTextBox.Text);

                for (int x = 0; x < screenBitmap.Width; x++)
                {
                    for (int y = 0; y < screenBitmap.Height; y++)
                    {
                        Color rgbValue = screenBitmap.GetPixel(x, y);
                        if (rgbValue.R == r && rgbValue.G == g && rgbValue.B == b)
                            binaryBitmap.SetPixel(x, y, Color.Black);
                        else
                            binaryBitmap.SetPixel(x, y, Color.White);
                    }
                }

                binaryScreen.Refresh();
            }
            else
            {

                int s1 = Convert.ToInt32(s1TextBox.Text);
                int s2 = Convert.ToInt32(s2TextBox.Text);
                int v1 = Convert.ToInt32(v1TextBox.Text);
                int v2 = Convert.ToInt32(v2TextBox.Text);

                for (int x = 0; x < screenBitmap.Width; x++)
                {
                    for (int y = 0; y < screenBitmap.Height; y++)
                    {
                        Color rgbValue = screenBitmap.GetPixel(x, y);
                        double hue;
                        double saturation;
                        double value;
                        ColorPickerForm.RGB2HSV(rgbValue.R, rgbValue.G, rgbValue.B, out hue, out saturation, out value);
                        saturation = saturation * 100 / 255;
                        value = value * 100 / 255;
                        if ((s1 <= (int)saturation && (int)saturation <= s2) && (v1 <= (int)value && (int)value <= v2))
                            binaryBitmap.SetPixel(x, y, Color.Black);
                        else
                            binaryBitmap.SetPixel(x, y, Color.White);
       
                    }
                }
                binaryScreen.Refresh();
            }
        }


        #region:::::::::::::::::::::::::::::::::::::::::::키값 입력:::::::::::::::::::::::::::::::::::::::::::
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void RGBTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 255)
                {
                    value = 255;
                }
                thisTextBox.Text = value.ToString();

            }
        }


        private void HSVTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 100)
                {
                    value = 100;
                }
                thisTextBox.Text = value.ToString();
            }
        }

        #endregion

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = modeComboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                rgbOptionPanel.Visible = true;
                hsvOptionPanel.Visible = false;
            }
            else if (selectedIndex == 1)
            {
                rgbOptionPanel.Visible = false;
                hsvOptionPanel.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessBinary();
        }

        private void BinaryColorPickerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(originalScreen != null)
                originalScreen.Visible = true;

            if(binaryScreen != null)
                binaryScreen.Visible = false;
        }

        private void modeComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (modeComboBox.SelectedIndex == 0)
            {
                rgbOptionPanel.Visible = true;
                hsvOptionPanel.Visible = false;
            }
            else
            {
                rgbOptionPanel.Visible = false;
                hsvOptionPanel.Visible = true;
            }
        }

        private void revokeButton_Click(object sender, EventArgs e)
        {
            if (originalScreen != null && binaryScreen != null)
            {
                binaryScreen.Image = new Bitmap(originalScreen.Image);
            }
        }
    }
}
