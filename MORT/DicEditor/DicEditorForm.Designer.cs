namespace MORT
{
    partial class DicEditorForm
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
            this.originalTextBox = new System.Windows.Forms.TextBox();
            this.resultTextbox = new System.Windows.Forms.TextBox();
            this.aceeoptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // originalTextBox
            // 
            this.originalTextBox.Location = new System.Drawing.Point(12, 23);
            this.originalTextBox.Multiline = true;
            this.originalTextBox.Name = "originalTextBox";
            this.originalTextBox.Size = new System.Drawing.Size(416, 45);
            this.originalTextBox.TabIndex = 0;
            // 
            // resultTextbox
            // 
            this.resultTextbox.Location = new System.Drawing.Point(12, 92);
            this.resultTextbox.Name = "resultTextbox";
            this.resultTextbox.Size = new System.Drawing.Size(416, 21);
            this.resultTextbox.TabIndex = 1;
            // 
            // aceeoptButton
            // 
            this.aceeoptButton.Location = new System.Drawing.Point(38, 143);
            this.aceeoptButton.Name = "aceeoptButton";
            this.aceeoptButton.Size = new System.Drawing.Size(130, 44);
            this.aceeoptButton.TabIndex = 2;
            this.aceeoptButton.Text = "추가";
            this.aceeoptButton.UseVisualStyleBackColor = true;
            this.aceeoptButton.Click += new System.EventHandler(this.aceeoptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(272, 143);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(130, 44);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "취소";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "교정 전";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "교정 후";
            // 
            // DicEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 222);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.aceeoptButton);
            this.Controls.Add(this.resultTextbox);
            this.Controls.Add(this.originalTextBox);
            this.Name = "DicEditorForm";
            this.Text = "교정단어 추가";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DicEditorForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox originalTextBox;
        private System.Windows.Forms.TextBox resultTextbox;
        private System.Windows.Forms.Button aceeoptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}