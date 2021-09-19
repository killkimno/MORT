
namespace MORT.CustomControl
{
    partial class CtSettingHotKey
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
            this.btSelect = new System.Windows.Forms.Button();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.lbFile = new System.Windows.Forms.Label();
            this.btClear = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbHotKey = new MORT.KeyInputLabel();
            this.SuspendLayout();
            // 
            // btSelect
            // 
            this.btSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btSelect.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSelect.ForeColor = System.Drawing.Color.White;
            this.btSelect.Location = new System.Drawing.Point(335, 39);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(118, 23);
            this.btSelect.TabIndex = 86;
            this.btSelect.Text = "파일선택";
            this.btSelect.UseVisualStyleBackColor = false;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(118, 42);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(198, 21);
            this.tbFile.TabIndex = 85;
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFile.ForeColor = System.Drawing.Color.Black;
            this.lbFile.Location = new System.Drawing.Point(3, 42);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(93, 17);
            this.lbFile.TabIndex = 84;
            this.lbFile.Text = "설정 파일명 1:";
            // 
            // btClear
            // 
            this.btClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btClear.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClear.ForeColor = System.Drawing.Color.White;
            this.btClear.Location = new System.Drawing.Point(335, 10);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(118, 23);
            this.btClear.TabIndex = 83;
            this.btClear.Text = "단축키 비우기";
            this.btClear.UseVisualStyleBackColor = false;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(3, 13);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(111, 17);
            this.lbTitle.TabIndex = 81;
            this.lbTitle.Text = "설정 불러오기 1 :";
            // 
            // lbHotKey
            // 
            this.lbHotKey.Location = new System.Drawing.Point(118, 10);
            this.lbHotKey.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbHotKey.Name = "lbHotKey";
            this.lbHotKey.Size = new System.Drawing.Size(198, 26);
            this.lbHotKey.TabIndex = 82;
            // 
            // CtSettingHotKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.lbHotKey);
            this.Controls.Add(this.lbTitle);
            this.Name = "CtSettingHotKey";
            this.Size = new System.Drawing.Size(465, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.Button btClear;
        private KeyInputLabel lbHotKey;
        private System.Windows.Forms.Label lbTitle;
    }
}
