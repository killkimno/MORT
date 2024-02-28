namespace MORT
{
    partial class ColorGroupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorGroupForm));
            checkedListBox = new System.Windows.Forms.CheckedListBox();
            acceptButton = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            cbReset = new System.Windows.Forms.Button();
            lbInformation = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // checkedListBox
            // 
            checkedListBox.CheckOnClick = true;
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Location = new System.Drawing.Point(12, 15);
            checkedListBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            checkedListBox.Name = "checkedListBox";
            checkedListBox.Size = new System.Drawing.Size(260, 238);
            checkedListBox.TabIndex = 1;
            // 
            // acceptButton
            // 
            acceptButton.Location = new System.Drawing.Point(12, 312);
            acceptButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            acceptButton.Name = "acceptButton";
            acceptButton.Size = new System.Drawing.Size(123, 45);
            acceptButton.TabIndex = 2;
            acceptButton.Text = "적용";
            acceptButton.UseVisualStyleBackColor = true;
            acceptButton.Click += acceptButton_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(155, 312);
            btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(117, 45);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "취소";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += button2_Click;
            // 
            // cbReset
            // 
            cbReset.Location = new System.Drawing.Point(12, 268);
            cbReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            cbReset.Name = "cbReset";
            cbReset.Size = new System.Drawing.Size(260, 38);
            cbReset.TabIndex = 4;
            cbReset.Text = "초기화";
            cbReset.UseVisualStyleBackColor = true;
            cbReset.Click += button3_Click;
            // 
            // lbInformation
            // 
            lbInformation.Location = new System.Drawing.Point(23, 371);
            lbInformation.Name = "lbInformation";
            lbInformation.Size = new System.Drawing.Size(239, 60);
            lbInformation.TabIndex = 5;
            lbInformation.Text = "참고 : \r\n\r\n색 그룹을 모두 해제하면 해당 OCR 영역은 \r\n이미지 보정을 사용하지 않습니다";
            lbInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ColorGroupForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(284, 445);
            Controls.Add(lbInformation);
            Controls.Add(cbReset);
            Controls.Add(btnCancel);
            Controls.Add(acceptButton);
            Controls.Add(checkedListBox);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "ColorGroupForm";
            Text = "색 그룹 편집";
            Load += ColorGroupForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button cbReset;
        private System.Windows.Forms.Label lbInformation;
    }
}