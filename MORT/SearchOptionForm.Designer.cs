namespace MORT
{
    partial class SearchOptionForm
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
            this.addButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.defaultButton = new System.Windows.Forms.Button();
            this.AddExceptButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(23, 12);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(300, 61);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "OCR 영역 추가";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(227, 86);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(96, 43);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "확정";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // defaultButton
            // 
            this.defaultButton.Location = new System.Drawing.Point(125, 86);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(96, 43);
            this.defaultButton.TabIndex = 4;
            this.defaultButton.Text = "초기화";
            this.defaultButton.UseVisualStyleBackColor = true;
            this.defaultButton.Click += new System.EventHandler(this.defaultButton_Click);
            // 
            // AddExceptButton
            // 
            this.AddExceptButton.Location = new System.Drawing.Point(23, 86);
            this.AddExceptButton.Name = "AddExceptButton";
            this.AddExceptButton.Size = new System.Drawing.Size(96, 43);
            this.AddExceptButton.TabIndex = 5;
            this.AddExceptButton.Text = "제외 영역 추가";
            this.AddExceptButton.UseVisualStyleBackColor = true;
            this.AddExceptButton.Click += new System.EventHandler(this.AddExceptButton_Click);
            // 
            // SearchOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 155);
            this.Controls.Add(this.AddExceptButton);
            this.Controls.Add(this.defaultButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.addButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchOptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "OCR 영역 관리";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DestoryForm);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.Button AddExceptButton;
    }
}