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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingBrowserUI));
            listView1 = new System.Windows.Forms.ListView();
            originalTitle = new System.Windows.Forms.ColumnHeader();
            korTitle = new System.Windows.Forms.ColumnHeader();
            lbTitle = new System.Windows.Forms.Label();
            lbTitleResult = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lbLinkShop = new System.Windows.Forms.LinkLabel();
            lbLinkExtra = new System.Windows.Forms.LinkLabel();
            tbSearch = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            btSearch = new System.Windows.Forms.Button();
            btApplay = new System.Windows.Forms.Button();
            tbInformation = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { originalTitle, korTitle });
            listView1.GridLines = true;
            listView1.Location = new System.Drawing.Point(12, 74);
            listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(604, 252);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // originalTitle
            // 
            originalTitle.Text = "제목";
            originalTitle.Width = 300;
            // 
            // korTitle
            // 
            korTitle.Text = "한국어 제목";
            korTitle.Width = 287;
            // 
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            lbTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbTitle.Location = new System.Drawing.Point(13, 356);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new System.Drawing.Size(66, 15);
            lbTitle.TabIndex = 2;
            lbTitle.Text = "제       목 :";
            // 
            // lbTitleResult
            // 
            lbTitleResult.AutoSize = true;
            lbTitleResult.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbTitleResult.Location = new System.Drawing.Point(85, 356);
            lbTitleResult.Name = "lbTitleResult";
            lbTitleResult.Size = new System.Drawing.Size(132, 15);
            lbTitleResult.TabIndex = 3;
            lbTitleResult.Text = "Titanfall 2  / 타이탄폴2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(13, 378);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(66, 15);
            label2.TabIndex = 4;
            label2.Text = "게임 링크 :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(13, 399);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 15);
            label1.TabIndex = 5;
            label1.Text = "추가 링크 :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(13, 439);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(59, 15);
            label3.TabIndex = 6;
            label3.Text = "부가 설명";
            // 
            // lbLinkShop
            // 
            lbLinkShop.AutoSize = true;
            lbLinkShop.Location = new System.Drawing.Point(86, 380);
            lbLinkShop.Name = "lbLinkShop";
            lbLinkShop.Size = new System.Drawing.Size(186, 15);
            lbLinkShop.TabIndex = 7;
            lbLinkShop.TabStop = true;
            lbLinkShop.Text = "https://store.steampowered.com/";
            lbLinkShop.LinkClicked += lbLinkShop_LinkClicked;
            // 
            // lbLinkExtra
            // 
            lbLinkExtra.AutoSize = true;
            lbLinkExtra.Location = new System.Drawing.Point(86, 402);
            lbLinkExtra.Name = "lbLinkExtra";
            lbLinkExtra.Size = new System.Drawing.Size(273, 15);
            lbLinkExtra.TabIndex = 8;
            lbLinkExtra.TabStop = true;
            lbLinkExtra.Text = "https://blog.naver.com/killkimno/222061747727";
            lbLinkExtra.LinkClicked += lbLinkExtra_LinkClicked;
            // 
            // tbSearch
            // 
            tbSearch.Location = new System.Drawing.Point(78, 21);
            tbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new System.Drawing.Size(431, 23);
            tbSearch.TabIndex = 9;
            tbSearch.TextChanged += tbSearch_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(13, 22);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(59, 15);
            label4.TabIndex = 10;
            label4.Text = "설정 검색";
            // 
            // btSearch
            // 
            btSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btSearch.Location = new System.Drawing.Point(515, 17);
            btSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btSearch.Name = "btSearch";
            btSearch.Size = new System.Drawing.Size(75, 29);
            btSearch.TabIndex = 11;
            btSearch.Text = "검색";
            btSearch.UseVisualStyleBackColor = true;
            btSearch.Click += btSearch_Click;
            // 
            // btApplay
            // 
            btApplay.AutoSize = true;
            btApplay.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btApplay.Location = new System.Drawing.Point(109, 645);
            btApplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btApplay.Name = "btApplay";
            btApplay.Size = new System.Drawing.Size(400, 50);
            btApplay.TabIndex = 12;
            btApplay.Text = "적용";
            btApplay.UseVisualStyleBackColor = true;
            btApplay.Click += btAppaly_Click;
            // 
            // tbInformation
            // 
            tbInformation.Location = new System.Drawing.Point(12, 461);
            tbInformation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbInformation.Name = "tbInformation";
            tbInformation.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            tbInformation.Size = new System.Drawing.Size(604, 175);
            tbInformation.TabIndex = 13;
            tbInformation.Text = "";
            tbInformation.LinkClicked += tbInformation_LinkClicked;
            // 
            // SettingBrowserUI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(629, 710);
            Controls.Add(tbInformation);
            Controls.Add(btApplay);
            Controls.Add(btSearch);
            Controls.Add(label4);
            Controls.Add(tbSearch);
            Controls.Add(lbLinkExtra);
            Controls.Add(lbLinkShop);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(lbTitleResult);
            Controls.Add(lbTitle);
            Controls.Add(listView1);
            DoubleBuffered = true;
            Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "SettingBrowserUI";
            Text = "설정 검색";
            FormClosing += SettingBrowserUI_FormClosing;
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Button btApplay;
        private System.Windows.Forms.RichTextBox tbInformation;
    }
}