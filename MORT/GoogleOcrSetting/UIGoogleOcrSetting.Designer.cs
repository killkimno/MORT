namespace MORT.GoogleOcrSetting
{
    partial class UIGoogleOcrSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIGoogleOcrSetting));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReadMe = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUseDefault = new System.Windows.Forms.CheckBox();
            this.lbJson = new System.Windows.Forms.Label();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnReadMe);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbCount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbUseDefault);
            this.panel1.Controls.Add(this.lbJson);
            this.panel1.Controls.Add(this.btOpenFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 356);
            this.panel1.TabIndex = 2;
            // 
            // btnReadMe
            // 
            this.btnReadMe.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReadMe.Location = new System.Drawing.Point(240, 185);
            this.btnReadMe.Name = "btnReadMe";
            this.btnReadMe.Size = new System.Drawing.Size(241, 55);
            this.btnReadMe.TabIndex = 10;
            this.btnReadMe.Text = "사용법 확인";
            this.btnReadMe.UseVisualStyleBackColor = true;
            this.btnReadMe.Click += new System.EventHandler(this.btnReadMe_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(15, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(234, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "(실제 사용량과 다를 수 있습니다)";
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCount.Location = new System.Drawing.Point(88, 29);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(65, 20);
            this.lbCount.TabIndex = 8;
            this.lbCount.Text = "0 / 1000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(15, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Json 파일 주소 : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "사용량 : ";
            // 
            // cbUseDefault
            // 
            this.cbUseDefault.AutoSize = true;
            this.cbUseDefault.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cbUseDefault.Location = new System.Drawing.Point(12, 310);
            this.cbUseDefault.Name = "cbUseDefault";
            this.cbUseDefault.Size = new System.Drawing.Size(495, 34);
            this.cbUseDefault.TabIndex = 5;
            this.cbUseDefault.Text = "구글 OCR 우선 사용\r\n[다른 OCR을 사용하더라도 스냅샷 / 한 번만 번역하기 할 떄는 구글 OCR을 사용합니다]";
            this.cbUseDefault.UseVisualStyleBackColor = true;
            this.cbUseDefault.CheckedChanged += new System.EventHandler(this.cbUseDefault_CheckedChanged);
            // 
            // lbJson
            // 
            this.lbJson.AutoSize = true;
            this.lbJson.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbJson.Location = new System.Drawing.Point(142, 9);
            this.lbJson.Name = "lbJson";
            this.lbJson.Size = new System.Drawing.Size(74, 20);
            this.lbJson.TabIndex = 4;
            this.lbJson.Text = "설정 필요";
            // 
            // btOpenFile
            // 
            this.btOpenFile.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOpenFile.Location = new System.Drawing.Point(240, 110);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(241, 55);
            this.btOpenFile.TabIndex = 2;
            this.btOpenFile.Text = "Json 파일 선택";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // UIGoogleOcrSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(702, 356);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UIGoogleOcrSetting";
            this.Text = "구글 OCR 설정";
            this.Load += new System.EventHandler(this.UIGoogleOcrSetting_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.CheckBox cbUseDefault;
        private System.Windows.Forms.Label lbJson;
        private System.Windows.Forms.Button btnReadMe;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}