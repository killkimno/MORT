
namespace MORT
{
    partial class UIAdvencedOption
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage16 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ctSettingHotKey3 = new MORT.CustomControl.CtSettingHotKey();
            this.ctSettingHotKey2 = new MORT.CustomControl.CtSettingHotKey();
            this.ctSettingHotKey1 = new MORT.CustomControl.CtSettingHotKey();
            this.tabPage17 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage16.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage16);
            this.tabControl1.Controls.Add(this.tabPage17);
            this.tabControl1.ItemSize = new System.Drawing.Size(70, 35);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(26, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 425);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage16
            // 
            this.tabPage16.AutoScroll = true;
            this.tabPage16.Controls.Add(this.groupBox1);
            this.tabPage16.Location = new System.Drawing.Point(4, 39);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage16.Size = new System.Drawing.Size(767, 382);
            this.tabPage16.TabIndex = 15;
            this.tabPage16.Text = "고급 단축키";
            this.tabPage16.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ctSettingHotKey3);
            this.groupBox1.Controls.Add(this.ctSettingHotKey2);
            this.groupBox1.Controls.Add(this.ctSettingHotKey1);
            this.groupBox1.Location = new System.Drawing.Point(20, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(711, 495);
            this.groupBox1.TabIndex = 66;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "설정 불러오기";
            // 
            // ctSettingHotKey3
            // 
            this.ctSettingHotKey3.Location = new System.Drawing.Point(6, 182);
            this.ctSettingHotKey3.Name = "ctSettingHotKey3";
            this.ctSettingHotKey3.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey3.TabIndex = 2;
            // 
            // ctSettingHotKey2
            // 
            this.ctSettingHotKey2.Location = new System.Drawing.Point(6, 101);
            this.ctSettingHotKey2.Name = "ctSettingHotKey2";
            this.ctSettingHotKey2.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey2.TabIndex = 1;
            // 
            // ctSettingHotKey1
            // 
            this.ctSettingHotKey1.Location = new System.Drawing.Point(6, 20);
            this.ctSettingHotKey1.Name = "ctSettingHotKey1";
            this.ctSettingHotKey1.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey1.TabIndex = 0;
            // 
            // tabPage17
            // 
            this.tabPage17.Location = new System.Drawing.Point(4, 39);
            this.tabPage17.Name = "tabPage17";
            this.tabPage17.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage17.Size = new System.Drawing.Size(767, 382);
            this.tabPage17.TabIndex = 16;
            this.tabPage17.Text = "tabPage17";
            this.tabPage17.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(398, 444);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(386, 44);
            this.button2.TabIndex = 83;
            this.button2.Text = "적용";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.OnClickApply);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gainsboro;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(17, 444);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(380, 44);
            this.button4.TabIndex = 84;
            this.button4.Text = "초기화";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // UIAdvencedOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Name = "UIAdvencedOption";
            this.Text = "UIAdvencedOption";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UIAdvencedOption_FormClosing);
            this.Load += new System.EventHandler(this.UIAdvencedOption_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage16.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage16;
        private System.Windows.Forms.TabPage tabPage17;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private CustomControl.CtSettingHotKey ctSettingHotKey1;
        private CustomControl.CtSettingHotKey ctSettingHotKey3;
        private CustomControl.CtSettingHotKey ctSettingHotKey2;
    }
}