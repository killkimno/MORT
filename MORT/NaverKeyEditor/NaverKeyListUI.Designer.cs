namespace MORT
{
    partial class NaverKeyListUI
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
            listBox_NaverKey = new System.Windows.Forms.ListBox();
            modfiButton = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            TextBox_NaverID = new System.Windows.Forms.TextBox();
            TextBox_NaverSecret = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            radioFree = new System.Windows.Forms.RadioButton();
            radioPaid = new System.Windows.Forms.RadioButton();
            label4 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // listBox_NaverKey
            // 
            listBox_NaverKey.FormattingEnabled = true;
            listBox_NaverKey.ItemHeight = 15;
            listBox_NaverKey.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "7", "7", "8", "9" });
            listBox_NaverKey.Location = new System.Drawing.Point(12, 56);
            listBox_NaverKey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listBox_NaverKey.Name = "listBox_NaverKey";
            listBox_NaverKey.Size = new System.Drawing.Size(754, 334);
            listBox_NaverKey.TabIndex = 0;
            listBox_NaverKey.SelectedValueChanged += listBox_NaverKey_SelectedValueChanged;
            // 
            // modfiButton
            // 
            modfiButton.Enabled = false;
            modfiButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            modfiButton.ForeColor = System.Drawing.SystemColors.ControlText;
            modfiButton.Location = new System.Drawing.Point(482, 448);
            modfiButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            modfiButton.Name = "modfiButton";
            modfiButton.Size = new System.Drawing.Size(139, 50);
            modfiButton.TabIndex = 1;
            modfiButton.Text = "추가";
            modfiButton.UseVisualStyleBackColor = true;
            modfiButton.Click += OnClick_Add;
            // 
            // button2
            // 
            button2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button2.Location = new System.Drawing.Point(627, 448);
            button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(139, 50);
            button2.TabIndex = 2;
            button2.Text = "삭제";
            button2.UseVisualStyleBackColor = true;
            button2.Click += OnClick_Remove;
            // 
            // TextBox_NaverID
            // 
            TextBox_NaverID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TextBox_NaverID.Location = new System.Drawing.Point(87, 436);
            TextBox_NaverID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            TextBox_NaverID.Name = "TextBox_NaverID";
            TextBox_NaverID.Size = new System.Drawing.Size(252, 25);
            TextBox_NaverID.TabIndex = 22;
            TextBox_NaverID.TextChanged += TextBox_NaverSecret_TextChanged;
            // 
            // TextBox_NaverSecret
            // 
            TextBox_NaverSecret.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TextBox_NaverSecret.Location = new System.Drawing.Point(87, 475);
            TextBox_NaverSecret.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            TextBox_NaverSecret.Name = "TextBox_NaverSecret";
            TextBox_NaverSecret.Size = new System.Drawing.Size(252, 25);
            TextBox_NaverSecret.TabIndex = 23;
            TextBox_NaverSecret.TextChanged += TextBox_NaverSecret_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 441);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(30, 15);
            label1.TabIndex = 26;
            label1.Text = "ID : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(18, 482);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(51, 15);
            label2.TabIndex = 27;
            label2.Text = "Secret : ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 38);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(19, 15);
            label3.TabIndex = 29;
            label3.Text = "ID\r\n";
            // 
            // radioFree
            // 
            radioFree.AutoSize = true;
            radioFree.Location = new System.Drawing.Point(20, 514);
            radioFree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            radioFree.Name = "radioFree";
            radioFree.Size = new System.Drawing.Size(71, 19);
            radioFree.TabIndex = 30;
            radioFree.TabStop = true;
            radioFree.Text = "무료 API";
            radioFree.UseVisualStyleBackColor = true;
            // 
            // radioPaid
            // 
            radioPaid.AutoSize = true;
            radioPaid.Location = new System.Drawing.Point(105, 514);
            radioPaid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            radioPaid.Name = "radioPaid";
            radioPaid.Size = new System.Drawing.Size(71, 19);
            radioPaid.TabIndex = 31;
            radioPaid.TabStop = true;
            radioPaid.Text = "유료 API";
            radioPaid.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(18, 408);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(490, 15);
            label4.TabIndex = 32;
            label4.Text = "2024/02/29 부터 무료 API를 사용할 수 없습니다, 대신 파파고 웹 번역기를 사용해 주세요";
            // 
            // NaverKeyListUI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(789, 544);
            Controls.Add(label4);
            Controls.Add(radioPaid);
            Controls.Add(radioFree);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TextBox_NaverSecret);
            Controls.Add(TextBox_NaverID);
            Controls.Add(button2);
            Controls.Add(modfiButton);
            Controls.Add(listBox_NaverKey);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "NaverKeyListUI";
            Text = "NaverKeyListUI";
            FormClosing += NaverKeyListUI_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBox_NaverKey;
        private System.Windows.Forms.Button modfiButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TextBox_NaverID;
        private System.Windows.Forms.TextBox TextBox_NaverSecret;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioFree;
        private System.Windows.Forms.RadioButton radioPaid;
        private System.Windows.Forms.Label label4;
    }
}