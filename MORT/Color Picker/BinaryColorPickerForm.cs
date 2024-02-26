using MORT.LocalizeManager;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MORT
{
    public partial class BinaryColorPickerForm : Form, ILocalizeForm
    {
        private PictureBox originalScreen;
        private PictureBox binaryScreen;

        public BinaryColorPickerForm()
        {
            InitializeComponent();
            LocalizeForm();
        }

        public void LocalizeForm()
        {
            btnTransform.LocalizeLabel("BinaryColor btnTransform");
            revokeButton.LocalizeLabel("BinaryColor revokeButton");
            lbThreshold.LocalizeLabel("BinaryColor Threshold");
            modeComboBox.LocalizeItems();
        }

        public void Setup(PictureBox original, PictureBox binary)
        {
            originalScreen = original;
            binaryScreen = binary;
        }

        public void SetValue(bool isHsv, bool isRgb, bool isThreshold, ColorGroup color, int threshold)
        {
            if(isHsv)
            {
                modeComboBox.SelectedIndex = 1;
            }
            else if(isRgb)
            {
                modeComboBox.SelectedIndex = 0;
            }
            else if(isThreshold)
            {
                modeComboBox.SelectedIndex = 2;
            }

            if(color != null)
            {
                rTextBox.Text = color.getValueR().ToString();
                gTextBox.Text = color.getValueG().ToString();
                bTextBox.Text = color.getValueB().ToString();

                s1TextBox.Text = color.getValueS1().ToString();
                s2TextBox.Text = color.getValueS2().ToString();

                v1TextBox.Text = color.getValueV1().ToString();
                v2TextBox.Text = color.getValueV2().ToString();
            }

            isLockAutoProcess = true;

            trackBar1.Value = threshold;

            if(isHsv || isRgb || isThreshold)
            {
                ProcessBinary();
            }

            isLockAutoProcess = false;
        }

        private void ProcessBinary()
        {
            Bitmap screenBitmap = (Bitmap)originalScreen.Image;
            Bitmap binaryBitmap = (Bitmap)binaryScreen.Image;


            if(modeComboBox.SelectedIndex == 0)
            {
                int r = Convert.ToInt32(rTextBox.Text);
                int g = Convert.ToInt32(gTextBox.Text);
                int b = Convert.ToInt32(bTextBox.Text);

                for(int x = 0; x < screenBitmap.Width; x++)
                {
                    for(int y = 0; y < screenBitmap.Height; y++)
                    {
                        Color rgbValue = screenBitmap.GetPixel(x, y);
                        if(rgbValue.R == r && rgbValue.G == g && rgbValue.B == b)
                            binaryBitmap.SetPixel(x, y, Color.Black);
                        else
                            binaryBitmap.SetPixel(x, y, Color.White);
                    }
                }

                binaryScreen.Refresh();
            }
            else if(modeComboBox.SelectedIndex == 1)
            {

                int s1 = Convert.ToInt32(s1TextBox.Text);
                int s2 = Convert.ToInt32(s2TextBox.Text);
                int v1 = Convert.ToInt32(v1TextBox.Text);
                int v2 = Convert.ToInt32(v2TextBox.Text);

                for(int x = 0; x < screenBitmap.Width; x++)
                {
                    for(int y = 0; y < screenBitmap.Height; y++)
                    {
                        Color rgbValue = screenBitmap.GetPixel(x, y);
                        double hue;
                        double saturation;
                        double value;
                        ColorPickerForm.RGB2HSV(rgbValue.R, rgbValue.G, rgbValue.B, out hue, out saturation, out value);
                        saturation = saturation * 100 / 255;
                        value = value * 100 / 255;
                        if((s1 <= (int)saturation && (int)saturation <= s2) && (v1 <= (int)value && (int)value <= v2))
                            binaryBitmap.SetPixel(x, y, Color.Black);
                        else
                            binaryBitmap.SetPixel(x, y, Color.White);

                    }
                }
                binaryScreen.Refresh();
            }
            else
            {
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetThreshold(Convert.ToSingle(tbThreshold.Text) / 255f);

                System.Drawing.Bitmap bmp = screenBitmap.Clone() as System.Drawing.Bitmap;
                bmp = ColorPickerForm.ConvertBlackAndWhite(bmp);
                Graphics g = System.Drawing.Graphics.FromImage(bmp);
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0,
                                 bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
                binaryScreen.Image = bmp;
                binaryScreen.Refresh();
            }
        }


        #region:::::::::::::::::::::::::::::::::::::::::::키값 입력:::::::::::::::::::::::::::::::::::::::::::
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(Char.IsDigit(e.KeyChar)) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void RGBTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if(thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if(value > 255)
                {
                    value = 255;
                }
                thisTextBox.Text = value.ToString();

            }
        }


        private void HSVTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if(thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if(value > 100)
                {
                    value = 100;
                }
                thisTextBox.Text = value.ToString();
            }
        }

        #endregion



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
            rgbOptionPanel.Visible = false;
            hsvOptionPanel.Visible = false;
            panel1.Visible = false;
            if(modeComboBox.SelectedIndex == 0)
            {
                rgbOptionPanel.Visible = true;
                hsvOptionPanel.Visible = false;
            }
            else if(modeComboBox.SelectedIndex == 1)
            {
                rgbOptionPanel.Visible = false;
                hsvOptionPanel.Visible = true;
            }
            else
            {
                panel1.Visible = true;
            }
        }

        private void revokeButton_Click(object sender, EventArgs e)
        {
            if(originalScreen != null && binaryScreen != null)
            {
                binaryScreen.Image = new Bitmap(originalScreen.Image);
            }
        }

        private bool lockTrackbar = false;
        private bool isLockAutoProcess = false;
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if(!lockTrackbar)
            {
                tbThreshold.Text = trackBar1.Value.ToString();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            lockTrackbar = true;
            int value = 127;

            Int32.TryParse(tbThreshold.Text, out value);

            trackBar1.Value = value;
            lockTrackbar = false;

            if(!isLockAutoProcess)
            {
                ProcessBinary();
            }

        }
    }
}
