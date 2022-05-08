namespace MORT
{
    partial class TransForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransForm));
            this.transTextBox = new System.Windows.Forms.TextBox();
            this.StopStateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // transTextBox
            // 
            this.transTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(88)))), ((int)(((byte)(101)))));
            this.transTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.transTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transTextBox.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.transTextBox.ForeColor = System.Drawing.Color.White;
            this.transTextBox.Location = new System.Drawing.Point(0, 0);
            this.transTextBox.Multiline = true;
            this.transTextBox.Name = "transTextBox";
            this.transTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.transTextBox.Size = new System.Drawing.Size(973, 246);
            this.transTextBox.TabIndex = 0;
            this.transTextBox.Text = "Monkeyhead\'s OCR Realtime TransLate 1.18dV\r\n제작자 : 몽키해드\r\n로고제작 : 김마손\r\n블로그 :\r\n몽키해드 :" +
    " http://blog.naver.com/killkimno/\r\n김마손 : http://blog.naver.com/sabon2000\r\n";
            // 
            // StopStateLabel
            // 
            this.StopStateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StopStateLabel.AutoSize = true;
            this.StopStateLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(88)))), ((int)(((byte)(101)))));
            this.StopStateLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StopStateLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.StopStateLabel.Location = new System.Drawing.Point(866, 216);
            this.StopStateLabel.Name = "StopStateLabel";
            this.StopStateLabel.Size = new System.Drawing.Size(86, 21);
            this.StopStateLabel.TabIndex = 5;
            this.StopStateLabel.Text = "번역중지...";
            this.StopStateLabel.Visible = false;
            // 
            // TransForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(973, 246);
            this.Controls.Add(this.StopStateLabel);
            this.Controls.Add(this.transTextBox);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransForm";
            this.Text = "번역창";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox transTextBox;
        private System.Windows.Forms.Label StopStateLabel;
    }
}