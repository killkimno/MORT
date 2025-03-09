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
            transTextBox = new System.Windows.Forms.TextBox();
            StopStateLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // transTextBox
            // 
            transTextBox.BackColor = System.Drawing.Color.FromArgb(72, 88, 101);
            transTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            transTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            transTextBox.Font = new System.Drawing.Font("맑은 고딕", 11F);
            transTextBox.ForeColor = System.Drawing.Color.White;
            transTextBox.Location = new System.Drawing.Point(0, 0);
            transTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            transTextBox.Multiline = true;
            transTextBox.Name = "transTextBox";
            transTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            transTextBox.Size = new System.Drawing.Size(973, 308);
            transTextBox.TabIndex = 0;
            transTextBox.Text = "Monkeyhead's OCR Realtime TransLate 1.18dV\r\n제작자 : 몽키해드\r\n로고제작 : 김마손\r\n블로그 :\r\n몽키해드 : http://blog.naver.com/killkimno/\r\n김마손 : http://blog.naver.com/sabon2000\r\n";
            // 
            // StopStateLabel
            // 
            StopStateLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            StopStateLabel.AutoSize = true;
            StopStateLabel.BackColor = System.Drawing.Color.FromArgb(72, 88, 101);
            StopStateLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
            StopStateLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            StopStateLabel.Location = new System.Drawing.Point(866, 270);
            StopStateLabel.Name = "StopStateLabel";
            StopStateLabel.Size = new System.Drawing.Size(86, 21);
            StopStateLabel.TabIndex = 5;
            StopStateLabel.Text = "번역중지...";
            StopStateLabel.Visible = false;
            // 
            // TransForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(973, 308);
            Controls.Add(StopStateLabel);
            Controls.Add(transTextBox);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "TransForm";
            ShowInTaskbar = false;
            Text = "번역창";
            TopMost = true;
            FormClosing += TransForm_FormClosing;
            MouseDown += TransForm_MouseDown;
            MouseMove += TransForm_MouseMove;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox transTextBox;
        private System.Windows.Forms.Label StopStateLabel;
    }
}