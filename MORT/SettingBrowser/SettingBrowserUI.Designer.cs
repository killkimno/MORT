namespace MORT.SettingBrowser
{
    partial class SettingBrowserUI
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.originalTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.korTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbTitleResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbLinkShop = new System.Windows.Forms.LinkLabel();
            this.lbLinkExtra = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btSearch = new System.Windows.Forms.Button();
            this.btAppaly = new System.Windows.Forms.Button();
            this.tbInformation = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.originalTitle,
            this.korTitle});
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 74);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(604, 252);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // originalTitle
            // 
            this.originalTitle.Text = "제목";
            this.originalTitle.Width = 300;
            // 
            // korTitle
            // 
            this.korTitle.Text = "한국어 제목";
            this.korTitle.Width = 300;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(13, 356);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(66, 15);
            this.lbTitle.TabIndex = 2;
            this.lbTitle.Text = "제       목 :";
            // 
            // lbTitleResult
            // 
            this.lbTitleResult.AutoSize = true;
            this.lbTitleResult.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitleResult.Location = new System.Drawing.Point(85, 356);
            this.lbTitleResult.Name = "lbTitleResult";
            this.lbTitleResult.Size = new System.Drawing.Size(132, 15);
            this.lbTitleResult.TabIndex = 3;
            this.lbTitleResult.Text = "Titanfall 2  / 타이탄폴2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(13, 378);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "게임 링크 :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(13, 399);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "추가 링크 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(13, 439);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "부가 설명";
            // 
            // lbLinkShop
            // 
            this.lbLinkShop.AutoSize = true;
            this.lbLinkShop.Location = new System.Drawing.Point(86, 380);
            this.lbLinkShop.Name = "lbLinkShop";
            this.lbLinkShop.Size = new System.Drawing.Size(186, 15);
            this.lbLinkShop.TabIndex = 7;
            this.lbLinkShop.TabStop = true;
            this.lbLinkShop.Text = "https://store.steampowered.com/";
            this.lbLinkShop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbLinkShop_LinkClicked);
            // 
            // lbLinkExtra
            // 
            this.lbLinkExtra.AutoSize = true;
            this.lbLinkExtra.Location = new System.Drawing.Point(86, 402);
            this.lbLinkExtra.Name = "lbLinkExtra";
            this.lbLinkExtra.Size = new System.Drawing.Size(273, 15);
            this.lbLinkExtra.TabIndex = 8;
            this.lbLinkExtra.TabStop = true;
            this.lbLinkExtra.Text = "https://blog.naver.com/killkimno/222061747727";
            this.lbLinkExtra.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbLinkExtra_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(78, 21);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(431, 23);
            this.textBox1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(13, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "설정 검색";
            // 
            // btSearch
            // 
            this.btSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btSearch.Location = new System.Drawing.Point(541, 21);
            this.btSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 29);
            this.btSearch.TabIndex = 11;
            this.btSearch.Text = "검색";
            this.btSearch.UseVisualStyleBackColor = true;
            // 
            // btAppaly
            // 
            this.btAppaly.AutoSize = true;
            this.btAppaly.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btAppaly.Location = new System.Drawing.Point(109, 645);
            this.btAppaly.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAppaly.Name = "btAppaly";
            this.btAppaly.Size = new System.Drawing.Size(400, 50);
            this.btAppaly.TabIndex = 12;
            this.btAppaly.Text = "적용";
            this.btAppaly.UseVisualStyleBackColor = true;
            this.btAppaly.Click += new System.EventHandler(this.btAppaly_Click);
            // 
            // tbInformation
            // 
            this.tbInformation.Location = new System.Drawing.Point(12, 461);
            this.tbInformation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbInformation.Size = new System.Drawing.Size(604, 175);
            this.tbInformation.TabIndex = 13;
            this.tbInformation.Text = "";
            this.tbInformation.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.tbInformation_LinkClicked);
            // 
            // SettingBrowserUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 710);
            this.Controls.Add(this.tbInformation);
            this.Controls.Add(this.btAppaly);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbLinkExtra);
            this.Controls.Add(this.lbLinkShop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbTitleResult);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.listView1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SettingBrowserUI";
            this.Text = "SettingBrowserUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader originalTitle;
        private System.Windows.Forms.ColumnHeader korTitle;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbTitleResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lbLinkShop;
        private System.Windows.Forms.LinkLabel lbLinkExtra;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Button btAppaly;
        private System.Windows.Forms.RichTextBox tbInformation;
    }
}