
namespace MORT
{
    partial class UIQuickSetting
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
            this.pnSetFont = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rbUnknown = new System.Windows.Forms.RadioButton();
            this.rbWhite = new System.Windows.Forms.RadioButton();
            this.rbBlack = new System.Windows.Forms.RadioButton();
            this.btNext = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.pnSetOcr = new System.Windows.Forms.Panel();
            this.lbOcrArea = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pnOcrComplete = new System.Windows.Forms.Panel();
            this.lbOCR = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pnFinal = new System.Windows.Forms.Panel();
            this.btShowBasic = new System.Windows.Forms.Button();
            this.btShowTrnaslate = new System.Windows.Forms.Button();
            this.lbEnd = new System.Windows.Forms.Label();
            this.pnSetFont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnSetOcr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pnOcrComplete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.pnFinal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSetFont
            // 
            this.pnSetFont.Controls.Add(this.pictureBox2);
            this.pnSetFont.Controls.Add(this.pictureBox1);
            this.pnSetFont.Controls.Add(this.rbUnknown);
            this.pnSetFont.Controls.Add(this.rbWhite);
            this.pnSetFont.Controls.Add(this.rbBlack);
            this.pnSetFont.Location = new System.Drawing.Point(12, 40);
            this.pnSetFont.Name = "pnSetFont";
            this.pnSetFont.Size = new System.Drawing.Size(661, 336);
            this.pnSetFont.TabIndex = 0;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MORT.Properties.Resources.White_Font;
            this.pictureBox2.Location = new System.Drawing.Point(29, 131);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(503, 69);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MORT.Properties.Resources.Black_Font;
            this.pictureBox1.Location = new System.Drawing.Point(29, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(444, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // rbUnknown
            // 
            this.rbUnknown.AutoSize = true;
            this.rbUnknown.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbUnknown.Location = new System.Drawing.Point(29, 211);
            this.rbUnknown.Name = "rbUnknown";
            this.rbUnknown.Size = new System.Drawing.Size(264, 19);
            this.rbUnknown.TabIndex = 2;
            this.rbUnknown.TabStop = true;
            this.rbUnknown.Text = "기본값 - 잘 모르겠어요! or 여러 색이 나와요";
            this.rbUnknown.UseVisualStyleBackColor = true;
            // 
            // rbWhite
            // 
            this.rbWhite.AutoSize = true;
            this.rbWhite.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbWhite.Location = new System.Drawing.Point(29, 110);
            this.rbWhite.Name = "rbWhite";
            this.rbWhite.Size = new System.Drawing.Size(77, 19);
            this.rbWhite.TabIndex = 1;
            this.rbWhite.TabStop = true;
            this.rbWhite.Text = "밝은 흰색";
            this.rbWhite.UseVisualStyleBackColor = true;
            // 
            // rbBlack
            // 
            this.rbBlack.AutoSize = true;
            this.rbBlack.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rbBlack.Location = new System.Drawing.Point(29, 14);
            this.rbBlack.Name = "rbBlack";
            this.rbBlack.Size = new System.Drawing.Size(89, 19);
            this.rbBlack.TabIndex = 0;
            this.rbBlack.TabStop = true;
            this.rbBlack.Text = "진한 검은색";
            this.rbBlack.UseVisualStyleBackColor = true;
            // 
            // btNext
            // 
            this.btNext.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btNext.Location = new System.Drawing.Point(12, 382);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(661, 55);
            this.btNext.TabIndex = 0;
            this.btNext.Text = "다음";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(13, 13);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(65, 25);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "label1";
            // 
            // pnSetOcr
            // 
            this.pnSetOcr.Controls.Add(this.lbOcrArea);
            this.pnSetOcr.Controls.Add(this.pictureBox3);
            this.pnSetOcr.Location = new System.Drawing.Point(12, 40);
            this.pnSetOcr.Name = "pnSetOcr";
            this.pnSetOcr.Size = new System.Drawing.Size(661, 336);
            this.pnSetOcr.TabIndex = 5;
            // 
            // lbOcrArea
            // 
            this.lbOcrArea.AutoSize = true;
            this.lbOcrArea.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbOcrArea.Location = new System.Drawing.Point(29, 258);
            this.lbOcrArea.Name = "lbOcrArea";
            this.lbOcrArea.Size = new System.Drawing.Size(387, 30);
            this.lbOcrArea.TabIndex = 5;
            this.lbOcrArea.Text = "게임 대사가 나오는 영역을 설정해 주세요.\r\nOCR 영역 설정 상태에서 화면을 드래그해서 영역을 설정하시면 됩니다\r\n";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::MORT.Properties.Resources.OCR_Example;
            this.pictureBox3.Location = new System.Drawing.Point(4, 14);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(643, 219);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // pnOcrComplete
            // 
            this.pnOcrComplete.Controls.Add(this.lbOCR);
            this.pnOcrComplete.Controls.Add(this.pictureBox4);
            this.pnOcrComplete.Location = new System.Drawing.Point(12, 40);
            this.pnOcrComplete.Name = "pnOcrComplete";
            this.pnOcrComplete.Size = new System.Drawing.Size(661, 336);
            this.pnOcrComplete.TabIndex = 6;
            // 
            // lbOCR
            // 
            this.lbOCR.AutoSize = true;
            this.lbOCR.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbOCR.Location = new System.Drawing.Point(29, 258);
            this.lbOCR.Name = "lbOCR";
            this.lbOCR.Size = new System.Drawing.Size(400, 30);
            this.lbOCR.TabIndex = 5;
            this.lbOCR.Text = "OCR 영역을 추가하셨습니다!\r\nOCR 영역은 리모컨에서 Search 버튼을 눌러 수정 / 추가하실 수 있습니다";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::MORT.Properties.Resources.RTT_Example;
            this.pictureBox4.Location = new System.Drawing.Point(131, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(431, 213);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            // 
            // pnFinal
            // 
            this.pnFinal.Controls.Add(this.btShowBasic);
            this.pnFinal.Controls.Add(this.btShowTrnaslate);
            this.pnFinal.Controls.Add(this.lbEnd);
            this.pnFinal.Location = new System.Drawing.Point(12, 40);
            this.pnFinal.Name = "pnFinal";
            this.pnFinal.Size = new System.Drawing.Size(661, 336);
            this.pnFinal.TabIndex = 7;
            // 
            // btShowBasic
            // 
            this.btShowBasic.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btShowBasic.Location = new System.Drawing.Point(32, 98);
            this.btShowBasic.Name = "btShowBasic";
            this.btShowBasic.Size = new System.Drawing.Size(592, 55);
            this.btShowBasic.TabIndex = 7;
            this.btShowBasic.Text = "기본 사용법 확인하기";
            this.btShowBasic.UseVisualStyleBackColor = true;
            this.btShowBasic.Click += new System.EventHandler(this.btShowBasic_Click);
            // 
            // btShowTrnaslate
            // 
            this.btShowTrnaslate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btShowTrnaslate.Location = new System.Drawing.Point(32, 25);
            this.btShowTrnaslate.Name = "btShowTrnaslate";
            this.btShowTrnaslate.Size = new System.Drawing.Size(592, 55);
            this.btShowTrnaslate.TabIndex = 6;
            this.btShowTrnaslate.Text = "번역 설정법 확인하기";
            this.btShowTrnaslate.UseVisualStyleBackColor = true;
            this.btShowTrnaslate.Click += new System.EventHandler(this.btShowTrnaslate_Click);
            // 
            // lbEnd
            // 
            this.lbEnd.AutoSize = true;
            this.lbEnd.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbEnd.Location = new System.Drawing.Point(29, 205);
            this.lbEnd.Name = "lbEnd";
            this.lbEnd.Size = new System.Drawing.Size(347, 75);
            this.lbEnd.TabIndex = 5;
            this.lbEnd.Text = "빠른 설정이 완료되었습니다.\r\n이제 리모컨의 Translate 버튼을 눌러 번역하시면 됩니다.\r\n\r\nMORT를 제대로 사용하기 위해선 번역기 설정" +
    "을 하셔야 합니다.\r\n번역기 설정은 위 링크를 통해 확인해 주세요.";
            // 
            // UIQuickSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(689, 450);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.pnSetOcr);
            this.Controls.Add(this.pnSetFont);
            this.Controls.Add(this.pnOcrComplete);
            this.Controls.Add(this.pnFinal);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UIQuickSetting";
            this.ShowIcon = false;
            this.Text = "빠른 설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UIQuickSetting_FormClosing);
            this.pnSetFont.ResumeLayout(false);
            this.pnSetFont.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnSetOcr.ResumeLayout(false);
            this.pnSetOcr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pnOcrComplete.ResumeLayout(false);
            this.pnOcrComplete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.pnFinal.ResumeLayout(false);
            this.pnFinal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnSetFont;
        private System.Windows.Forms.RadioButton rbUnknown;
        private System.Windows.Forms.RadioButton rbWhite;
        private System.Windows.Forms.RadioButton rbBlack;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnSetOcr;
        private System.Windows.Forms.Label lbOcrArea;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel pnOcrComplete;
        private System.Windows.Forms.Label lbOCR;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel pnFinal;
        private System.Windows.Forms.Button btShowBasic;
        private System.Windows.Forms.Button btShowTrnaslate;
        private System.Windows.Forms.Label lbEnd;
    }
}