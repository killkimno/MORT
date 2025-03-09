namespace MORT
{
    partial class Logo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logo));
            pictureBox1 = new System.Windows.Forms.PictureBox();
            lbVersion = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.MORT_LOGO;
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(822, 540);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lbVersion
            // 
            lbVersion.AutoSize = true;
            lbVersion.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            lbVersion.Location = new System.Drawing.Point(55, 512);
            lbVersion.Name = "lbVersion";
            lbVersion.Size = new System.Drawing.Size(146, 15);
            lbVersion.TabIndex = 1;
            lbVersion.Text = "Build : 1.19 - 2019 10 05";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            label2.Location = new System.Drawing.Point(687, 512);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(123, 15);
            label2.TabIndex = 2;
            label2.Text = "illustrated by : 김마손";
            label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            label4.Location = new System.Drawing.Point(657, 494);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(153, 15);
            label4.TabIndex = 4;
            label4.Text = "Programmed by : 몽키해드";
            label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // webBrowser1
            // 
            webBrowser1.Location = new System.Drawing.Point(0, 0);
            webBrowser1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            webBrowser1.Name = "webBrowser1";
            webBrowser1.Size = new System.Drawing.Size(0, 0);
            webBrowser1.TabIndex = 5;
            webBrowser1.TabStop = false;
            webBrowser1.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            webBrowser1.Visible = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;
            // 
            // Logo
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(822, 542);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(lbVersion);
            Controls.Add(pictureBox1);
            Controls.Add(webBrowser1);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Logo";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Logo";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}