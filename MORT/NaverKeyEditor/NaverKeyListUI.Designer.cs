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
            this.listBox_NaverKey = new System.Windows.Forms.ListBox();
            this.modfiButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TextBox_NaverID = new System.Windows.Forms.TextBox();
            this.TextBox_NaverSecret = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_NaverKey
            // 
            this.listBox_NaverKey.FormattingEnabled = true;
            this.listBox_NaverKey.ItemHeight = 12;
            this.listBox_NaverKey.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "7",
            "7",
            "8",
            "9"});
            this.listBox_NaverKey.Location = new System.Drawing.Point(12, 45);
            this.listBox_NaverKey.Name = "listBox_NaverKey";
            this.listBox_NaverKey.Size = new System.Drawing.Size(754, 292);
            this.listBox_NaverKey.TabIndex = 0;
            this.listBox_NaverKey.SelectedValueChanged += new System.EventHandler(this.listBox_NaverKey_SelectedValueChanged);
            // 
            // modfiButton
            // 
            this.modfiButton.Enabled = false;
            this.modfiButton.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.modfiButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.modfiButton.Location = new System.Drawing.Point(364, 358);
            this.modfiButton.Name = "modfiButton";
            this.modfiButton.Size = new System.Drawing.Size(139, 40);
            this.modfiButton.TabIndex = 1;
            this.modfiButton.Text = "추가";
            this.modfiButton.UseVisualStyleBackColor = true;
            this.modfiButton.Click += new System.EventHandler(this.OnClick_Add);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.Location = new System.Drawing.Point(654, 358);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "삭제";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClick_Remove);
            // 
            // TextBox_NaverID
            // 
            this.TextBox_NaverID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBox_NaverID.Location = new System.Drawing.Point(87, 349);
            this.TextBox_NaverID.Name = "TextBox_NaverID";
            this.TextBox_NaverID.Size = new System.Drawing.Size(252, 25);
            this.TextBox_NaverID.TabIndex = 22;
            this.TextBox_NaverID.TextChanged += new System.EventHandler(this.TextBox_NaverSecret_TextChanged);
            // 
            // TextBox_NaverSecret
            // 
            this.TextBox_NaverSecret.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBox_NaverSecret.Location = new System.Drawing.Point(87, 380);
            this.TextBox_NaverSecret.Name = "TextBox_NaverSecret";
            this.TextBox_NaverSecret.Size = new System.Drawing.Size(252, 25);
            this.TextBox_NaverSecret.TabIndex = 23;
            this.TextBox_NaverSecret.TextChanged += new System.EventHandler(this.TextBox_NaverSecret_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "ID : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "Secret : ";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button3.Location = new System.Drawing.Point(509, 358);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(139, 40);
            this.button3.TabIndex = 28;
            this.button3.Text = "미구현";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "ID\r\n";
            // 
            // NaverKeyListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBox_NaverSecret);
            this.Controls.Add(this.TextBox_NaverID);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.modfiButton);
            this.Controls.Add(this.listBox_NaverKey);
            this.Name = "NaverKeyListUI";
            this.Text = "NaverKeyListUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NaverKeyListUI_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_NaverKey;
        private System.Windows.Forms.Button modfiButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TextBox_NaverID;
        private System.Windows.Forms.TextBox TextBox_NaverSecret;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
    }
}