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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbJson = new System.Windows.Forms.Label();
            this.cbUseDefault = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbUseDefault);
            this.panel1.Controls.Add(this.lbJson);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btOpenFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 238);
            this.panel1.TabIndex = 2;
            // 
            // btOpenFile
            // 
            this.btOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btOpenFile.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOpenFile.Location = new System.Drawing.Point(222, 91);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(241, 55);
            this.btOpenFile.TabIndex = 2;
            this.btOpenFile.Text = "Json 파일 선택";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "상태 : 파일 없음";
            // 
            // lbJson
            // 
            this.lbJson.AutoSize = true;
            this.lbJson.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbJson.Location = new System.Drawing.Point(12, 41);
            this.lbJson.Name = "lbJson";
            this.lbJson.Size = new System.Drawing.Size(187, 20);
            this.lbJson.TabIndex = 4;
            this.lbJson.Text = "Json 파일 주소 : ......................";
            // 
            // cbUseDefault
            // 
            this.cbUseDefault.AutoSize = true;
            this.cbUseDefault.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cbUseDefault.Location = new System.Drawing.Point(16, 165);
            this.cbUseDefault.Name = "cbUseDefault";
            this.cbUseDefault.Size = new System.Drawing.Size(499, 34);
            this.cbUseDefault.TabIndex = 5;
            this.cbUseDefault.Text = "구글 OCR 우선 사용\r\n[다른 OCR을 사용하더라도 스냅샷 / 한 번만 번역하기 할 떄는 구글 OCR 을 사용합니다]";
            this.cbUseDefault.UseVisualStyleBackColor = true;
            // 
            // UIGoogleOcrSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(655, 238);
            this.Controls.Add(this.panel1);
            this.Name = "UIGoogleOcrSetting";
            this.Text = "UIGoogleOcrSetting";
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
        private System.Windows.Forms.Label label1;
    }
}