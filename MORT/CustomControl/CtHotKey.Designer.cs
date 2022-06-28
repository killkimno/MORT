
namespace MORT.CustomControl
{
    partial class CtHotKey
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btClear = new System.Windows.Forms.Button();
            this.lbHotKey = new MORT.KeyInputLabel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbInformation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btClear
            // 
            this.btClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btClear.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClear.ForeColor = System.Drawing.Color.White;
            this.btClear.Location = new System.Drawing.Point(380, 6);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(118, 23);
            this.btClear.TabIndex = 89;
            this.btClear.Text = "비우기";
            this.btClear.UseVisualStyleBackColor = false;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // lbHotKey
            // 
            this.lbHotKey.Location = new System.Drawing.Point(163, 6);
            this.lbHotKey.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbHotKey.Name = "lbHotKey";
            this.lbHotKey.Size = new System.Drawing.Size(198, 26);
            this.lbHotKey.TabIndex = 88;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(3, 9);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(112, 17);
            this.lbTitle.TabIndex = 87;
            this.lbTitle.Text = "설정 불러오기 1 :";
            // 
            // lbInformation
            // 
            this.lbInformation.AutoSize = true;
            this.lbInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbInformation.ForeColor = System.Drawing.Color.Black;
            this.lbInformation.Location = new System.Drawing.Point(3, 38);
            this.lbInformation.Name = "lbInformation";
            this.lbInformation.Size = new System.Drawing.Size(34, 17);
            this.lbInformation.TabIndex = 90;
            this.lbInformation.Text = "설명";
            // 
            // CtHotKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbInformation);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.lbHotKey);
            this.Controls.Add(this.lbTitle);
            this.Name = "CtHotKey";
            this.Size = new System.Drawing.Size(520, 63);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btClear;
        private KeyInputLabel lbHotKey;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbInformation;
    }
}
