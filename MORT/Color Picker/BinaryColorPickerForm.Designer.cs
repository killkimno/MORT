namespace MORT
{
    partial class BinaryColorPickerForm
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
            this.rgbOptionPanel = new System.Windows.Forms.Panel();
            this.bTextBox = new System.Windows.Forms.TextBox();
            this.gTextBox = new System.Windows.Forms.TextBox();
            this.rTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.hsvOptionPanel = new System.Windows.Forms.Panel();
            this.v2TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.v1TextBox = new System.Windows.Forms.TextBox();
            this.s2TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.s1TextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.revokeButton = new System.Windows.Forms.Button();
            this.rgbOptionPanel.SuspendLayout();
            this.hsvOptionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // rgbOptionPanel
            // 
            this.rgbOptionPanel.Controls.Add(this.bTextBox);
            this.rgbOptionPanel.Controls.Add(this.gTextBox);
            this.rgbOptionPanel.Controls.Add(this.rTextBox);
            this.rgbOptionPanel.Controls.Add(this.label14);
            this.rgbOptionPanel.Controls.Add(this.label15);
            this.rgbOptionPanel.Controls.Add(this.label18);
            this.rgbOptionPanel.Location = new System.Drawing.Point(10, 41);
            this.rgbOptionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rgbOptionPanel.Name = "rgbOptionPanel";
            this.rgbOptionPanel.Size = new System.Drawing.Size(157, 117);
            this.rgbOptionPanel.TabIndex = 31;
            // 
            // bTextBox
            // 
            this.bTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bTextBox.Location = new System.Drawing.Point(31, 63);
            this.bTextBox.Name = "bTextBox";
            this.bTextBox.Size = new System.Drawing.Size(47, 25);
            this.bTextBox.TabIndex = 27;
            this.bTextBox.Text = "0";
            // 
            // gTextBox
            // 
            this.gTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gTextBox.Location = new System.Drawing.Point(31, 33);
            this.gTextBox.Name = "gTextBox";
            this.gTextBox.Size = new System.Drawing.Size(47, 25);
            this.gTextBox.TabIndex = 24;
            this.gTextBox.Text = "0";
            // 
            // rTextBox
            // 
            this.rTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rTextBox.Location = new System.Drawing.Point(31, 3);
            this.rTextBox.Name = "rTextBox";
            this.rTextBox.Size = new System.Drawing.Size(47, 25);
            this.rTextBox.TabIndex = 21;
            this.rTextBox.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(3, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 15);
            this.label14.TabIndex = 18;
            this.label14.Text = "R :";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(3, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(22, 15);
            this.label15.TabIndex = 19;
            this.label15.Text = "G :";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(3, 68);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(21, 15);
            this.label18.TabIndex = 20;
            this.label18.Text = "B :";
            // 
            // modeComboBox
            // 
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Items.AddRange(new object[] {
            "RGB",
            "HSV"});
            this.modeComboBox.Location = new System.Drawing.Point(10, 9);
            this.modeComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(157, 20);
            this.modeComboBox.TabIndex = 30;
            this.modeComboBox.Text = "RGB";
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged_1);
            // 
            // hsvOptionPanel
            // 
            this.hsvOptionPanel.Controls.Add(this.v2TextBox);
            this.hsvOptionPanel.Controls.Add(this.label5);
            this.hsvOptionPanel.Controls.Add(this.v1TextBox);
            this.hsvOptionPanel.Controls.Add(this.s2TextBox);
            this.hsvOptionPanel.Controls.Add(this.label4);
            this.hsvOptionPanel.Controls.Add(this.s1TextBox);
            this.hsvOptionPanel.Controls.Add(this.label13);
            this.hsvOptionPanel.Controls.Add(this.label17);
            this.hsvOptionPanel.Location = new System.Drawing.Point(10, 41);
            this.hsvOptionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.hsvOptionPanel.Name = "hsvOptionPanel";
            this.hsvOptionPanel.Size = new System.Drawing.Size(157, 117);
            this.hsvOptionPanel.TabIndex = 32;
            this.hsvOptionPanel.Visible = false;
            // 
            // v2TextBox
            // 
            this.v2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.v2TextBox.Location = new System.Drawing.Point(105, 33);
            this.v2TextBox.Name = "v2TextBox";
            this.v2TextBox.Size = new System.Drawing.Size(47, 25);
            this.v2TextBox.TabIndex = 28;
            this.v2TextBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(83, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 17);
            this.label5.TabIndex = 29;
            this.label5.Text = "~";
            // 
            // v1TextBox
            // 
            this.v1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.v1TextBox.Location = new System.Drawing.Point(29, 33);
            this.v1TextBox.Name = "v1TextBox";
            this.v1TextBox.Size = new System.Drawing.Size(47, 25);
            this.v1TextBox.TabIndex = 27;
            this.v1TextBox.Text = "0";
            // 
            // s2TextBox
            // 
            this.s2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s2TextBox.Location = new System.Drawing.Point(105, 3);
            this.s2TextBox.Name = "s2TextBox";
            this.s2TextBox.Size = new System.Drawing.Size(47, 25);
            this.s2TextBox.TabIndex = 25;
            this.s2TextBox.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(83, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 17);
            this.label4.TabIndex = 26;
            this.label4.Text = "~";
            // 
            // s1TextBox
            // 
            this.s1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.s1TextBox.Location = new System.Drawing.Point(29, 3);
            this.s1TextBox.Name = "s1TextBox";
            this.s1TextBox.Size = new System.Drawing.Size(47, 25);
            this.s1TextBox.TabIndex = 24;
            this.s1TextBox.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(3, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 15);
            this.label13.TabIndex = 19;
            this.label13.Text = "S :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(3, 38);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(22, 15);
            this.label17.TabIndex = 20;
            this.label17.Text = "V :";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(9, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 32);
            this.button1.TabIndex = 33;
            this.button1.Text = "변환하기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // revokeButton
            // 
            this.revokeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.revokeButton.Location = new System.Drawing.Point(91, 181);
            this.revokeButton.Name = "revokeButton";
            this.revokeButton.Size = new System.Drawing.Size(77, 32);
            this.revokeButton.TabIndex = 34;
            this.revokeButton.Text = "복구";
            this.revokeButton.UseVisualStyleBackColor = true;
            this.revokeButton.Click += new System.EventHandler(this.revokeButton_Click);
            // 
            // BinaryColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(176, 224);
            this.Controls.Add(this.revokeButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.hsvOptionPanel);
            this.Controls.Add(this.rgbOptionPanel);
            this.Controls.Add(this.modeComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BinaryColorPickerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BinaryColorPickerForm_FormClosed);
            this.rgbOptionPanel.ResumeLayout(false);
            this.rgbOptionPanel.PerformLayout();
            this.hsvOptionPanel.ResumeLayout(false);
            this.hsvOptionPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel rgbOptionPanel;
        private System.Windows.Forms.TextBox bTextBox;
        private System.Windows.Forms.TextBox gTextBox;
        private System.Windows.Forms.TextBox rTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.Panel hsvOptionPanel;
        private System.Windows.Forms.TextBox v2TextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox v1TextBox;
        private System.Windows.Forms.TextBox s2TextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox s1TextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button revokeButton;
    }
}