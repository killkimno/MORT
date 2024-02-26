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
            if(disposing && (components != null))
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
            rgbOptionPanel = new System.Windows.Forms.Panel();
            bTextBox = new System.Windows.Forms.TextBox();
            gTextBox = new System.Windows.Forms.TextBox();
            rTextBox = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            label15 = new System.Windows.Forms.Label();
            label18 = new System.Windows.Forms.Label();
            modeComboBox = new System.Windows.Forms.ComboBox();
            hsvOptionPanel = new System.Windows.Forms.Panel();
            v2TextBox = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            v1TextBox = new System.Windows.Forms.TextBox();
            s2TextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            s1TextBox = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            label17 = new System.Windows.Forms.Label();
            btnTransform = new System.Windows.Forms.Button();
            revokeButton = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            lbThreshold = new System.Windows.Forms.Label();
            trackBar1 = new System.Windows.Forms.TrackBar();
            tbThreshold = new System.Windows.Forms.TextBox();
            rgbOptionPanel.SuspendLayout();
            hsvOptionPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // rgbOptionPanel
            // 
            rgbOptionPanel.Controls.Add(bTextBox);
            rgbOptionPanel.Controls.Add(gTextBox);
            rgbOptionPanel.Controls.Add(rTextBox);
            rgbOptionPanel.Controls.Add(label14);
            rgbOptionPanel.Controls.Add(label15);
            rgbOptionPanel.Controls.Add(label18);
            rgbOptionPanel.Location = new System.Drawing.Point(10, 51);
            rgbOptionPanel.Margin = new System.Windows.Forms.Padding(0);
            rgbOptionPanel.Name = "rgbOptionPanel";
            rgbOptionPanel.Size = new System.Drawing.Size(157, 146);
            rgbOptionPanel.TabIndex = 31;
            // 
            // bTextBox
            // 
            bTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            bTextBox.Location = new System.Drawing.Point(31, 79);
            bTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            bTextBox.Name = "bTextBox";
            bTextBox.Size = new System.Drawing.Size(47, 25);
            bTextBox.TabIndex = 27;
            bTextBox.Text = "0";
            // 
            // gTextBox
            // 
            gTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gTextBox.Location = new System.Drawing.Point(31, 41);
            gTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gTextBox.Name = "gTextBox";
            gTextBox.Size = new System.Drawing.Size(47, 25);
            gTextBox.TabIndex = 24;
            gTextBox.Text = "0";
            // 
            // rTextBox
            // 
            rTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rTextBox.Location = new System.Drawing.Point(31, 4);
            rTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            rTextBox.Name = "rTextBox";
            rTextBox.Size = new System.Drawing.Size(47, 25);
            rTextBox.TabIndex = 21;
            rTextBox.Text = "0";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label14.Location = new System.Drawing.Point(3, 10);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(21, 15);
            label14.TabIndex = 18;
            label14.Text = "R :";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label15.Location = new System.Drawing.Point(3, 48);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(22, 15);
            label15.TabIndex = 19;
            label15.Text = "G :";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label18.Location = new System.Drawing.Point(3, 85);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(21, 15);
            label18.TabIndex = 20;
            label18.Text = "B :";
            // 
            // modeComboBox
            // 
            modeComboBox.FormattingEnabled = true;
            modeComboBox.Items.AddRange(new object[] { "RGB", "HSV", "Threshold" });
            modeComboBox.Location = new System.Drawing.Point(10, 11);
            modeComboBox.Margin = new System.Windows.Forms.Padding(0);
            modeComboBox.Name = "modeComboBox";
            modeComboBox.Size = new System.Drawing.Size(157, 23);
            modeComboBox.TabIndex = 30;
            modeComboBox.Text = "RGB";
            modeComboBox.SelectedIndexChanged += modeComboBox_SelectedIndexChanged_1;
            // 
            // hsvOptionPanel
            // 
            hsvOptionPanel.Controls.Add(v2TextBox);
            hsvOptionPanel.Controls.Add(label5);
            hsvOptionPanel.Controls.Add(v1TextBox);
            hsvOptionPanel.Controls.Add(s2TextBox);
            hsvOptionPanel.Controls.Add(label4);
            hsvOptionPanel.Controls.Add(s1TextBox);
            hsvOptionPanel.Controls.Add(label13);
            hsvOptionPanel.Controls.Add(label17);
            hsvOptionPanel.Location = new System.Drawing.Point(10, 51);
            hsvOptionPanel.Margin = new System.Windows.Forms.Padding(0);
            hsvOptionPanel.Name = "hsvOptionPanel";
            hsvOptionPanel.Size = new System.Drawing.Size(157, 146);
            hsvOptionPanel.TabIndex = 32;
            hsvOptionPanel.Visible = false;
            // 
            // v2TextBox
            // 
            v2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            v2TextBox.Location = new System.Drawing.Point(105, 41);
            v2TextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            v2TextBox.Name = "v2TextBox";
            v2TextBox.Size = new System.Drawing.Size(47, 25);
            v2TextBox.TabIndex = 28;
            v2TextBox.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label5.ForeColor = System.Drawing.Color.Black;
            label5.Location = new System.Drawing.Point(83, 44);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(17, 17);
            label5.TabIndex = 29;
            label5.Text = "~";
            // 
            // v1TextBox
            // 
            v1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            v1TextBox.Location = new System.Drawing.Point(29, 41);
            v1TextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            v1TextBox.Name = "v1TextBox";
            v1TextBox.Size = new System.Drawing.Size(47, 25);
            v1TextBox.TabIndex = 27;
            v1TextBox.Text = "0";
            // 
            // s2TextBox
            // 
            s2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            s2TextBox.Location = new System.Drawing.Point(105, 4);
            s2TextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            s2TextBox.Name = "s2TextBox";
            s2TextBox.Size = new System.Drawing.Size(47, 25);
            s2TextBox.TabIndex = 25;
            s2TextBox.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label4.ForeColor = System.Drawing.Color.Black;
            label4.Location = new System.Drawing.Point(83, 6);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(17, 17);
            label4.TabIndex = 26;
            label4.Text = "~";
            // 
            // s1TextBox
            // 
            s1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            s1TextBox.Location = new System.Drawing.Point(29, 4);
            s1TextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            s1TextBox.Name = "s1TextBox";
            s1TextBox.Size = new System.Drawing.Size(47, 25);
            s1TextBox.TabIndex = 24;
            s1TextBox.Text = "0";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label13.Location = new System.Drawing.Point(3, 10);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(21, 15);
            label13.TabIndex = 19;
            label13.Text = "S :";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label17.Location = new System.Drawing.Point(3, 48);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(22, 15);
            label17.TabIndex = 20;
            label17.Text = "V :";
            // 
            // btnTransform
            // 
            btnTransform.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnTransform.Location = new System.Drawing.Point(9, 226);
            btnTransform.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnTransform.Name = "btnTransform";
            btnTransform.Size = new System.Drawing.Size(77, 40);
            btnTransform.TabIndex = 33;
            btnTransform.Text = "변환하기";
            btnTransform.UseVisualStyleBackColor = true;
            btnTransform.Click += button1_Click;
            // 
            // revokeButton
            // 
            revokeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            revokeButton.Location = new System.Drawing.Point(91, 226);
            revokeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            revokeButton.Name = "revokeButton";
            revokeButton.Size = new System.Drawing.Size(77, 40);
            revokeButton.TabIndex = 34;
            revokeButton.Text = "복구";
            revokeButton.UseVisualStyleBackColor = true;
            revokeButton.Click += revokeButton_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(lbThreshold);
            panel1.Controls.Add(trackBar1);
            panel1.Controls.Add(tbThreshold);
            panel1.Location = new System.Drawing.Point(10, 51);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(157, 146);
            panel1.TabIndex = 33;
            panel1.Visible = false;
            // 
            // lbThreshold
            // 
            lbThreshold.Location = new System.Drawing.Point(5, 108);
            lbThreshold.Name = "lbThreshold";
            lbThreshold.Size = new System.Drawing.Size(146, 21);
            lbThreshold.TabIndex = 29;
            lbThreshold.Text = "임계값";
            lbThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar1
            // 
            trackBar1.Location = new System.Drawing.Point(5, 9);
            trackBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar1.Maximum = 255;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new System.Drawing.Size(146, 45);
            trackBar1.TabIndex = 28;
            trackBar1.ValueChanged += trackBar1_ValueChanged;
            // 
            // tbThreshold
            // 
            tbThreshold.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tbThreshold.Location = new System.Drawing.Point(52, 69);
            tbThreshold.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbThreshold.Name = "tbThreshold";
            tbThreshold.Size = new System.Drawing.Size(47, 25);
            tbThreshold.TabIndex = 27;
            tbThreshold.Text = "0";
            tbThreshold.TextChanged += textBox2_TextChanged;
            // 
            // BinaryColorPickerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(176, 280);
            Controls.Add(panel1);
            Controls.Add(revokeButton);
            Controls.Add(btnTransform);
            Controls.Add(hsvOptionPanel);
            Controls.Add(rgbOptionPanel);
            Controls.Add(modeComboBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "BinaryColorPickerForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            FormClosed += BinaryColorPickerForm_FormClosed;
            rgbOptionPanel.ResumeLayout(false);
            rgbOptionPanel.PerformLayout();
            hsvOptionPanel.ResumeLayout(false);
            hsvOptionPanel.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
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
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.Button revokeButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbThreshold;
        private System.Windows.Forms.Label lbThreshold;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}