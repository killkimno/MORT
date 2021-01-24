namespace MORT
{
    partial class RTT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTT));
            this.settingButton = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.PictureBox();
            this.setCaptureAreaButton = new System.Windows.Forms.PictureBox();
            this.startTransButton = new System.Windows.Forms.PictureBox();
            this.stopButton = new System.Windows.Forms.PictureBox();
            this.snapButton = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCaptureAreaButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTransButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.snapButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // settingButton
            // 
            this.settingButton.Image = global::MORT.Properties.Resources.Remote_Option1;
            this.settingButton.Location = new System.Drawing.Point(6, 6);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(18, 21);
            this.settingButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.settingButton.TabIndex = 4;
            this.settingButton.TabStop = false;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
            this.settingButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.settingButton_MouseDown);
            this.settingButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.settingButton_MouseUp);
            // 
            // closeButton
            // 
            this.closeButton.Image = global::MORT.Properties.Resources.Remote_Exit;
            this.closeButton.Location = new System.Drawing.Point(170, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(29, 29);
            this.closeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.closeButton.TabIndex = 5;
            this.closeButton.TabStop = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            this.closeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.closeButton_MouseDown);
            this.closeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.closeButton_MouseUp);
            // 
            // setCaptureAreaButton
            // 
            this.setCaptureAreaButton.Image = global::MORT.Properties.Resources.Remote_Search;
            this.setCaptureAreaButton.Location = new System.Drawing.Point(6, 47);
            this.setCaptureAreaButton.Name = "setCaptureAreaButton";
            this.setCaptureAreaButton.Size = new System.Drawing.Size(187, 74);
            this.setCaptureAreaButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.setCaptureAreaButton.TabIndex = 6;
            this.setCaptureAreaButton.TabStop = false;
            this.setCaptureAreaButton.Click += new System.EventHandler(this.setCaptureAreaButton_Click);
            this.setCaptureAreaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.setCaptureAreaButton_MouseDown);
            this.setCaptureAreaButton.MouseLeave += new System.EventHandler(this.setCaptureAreaButton_MouseLeave);
            this.setCaptureAreaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.setCaptureAreaButton_MouseUp);
            // 
            // startTransButton
            // 
            this.startTransButton.Image = global::MORT.Properties.Resources.Remote_Translate;
            this.startTransButton.Location = new System.Drawing.Point(7, 217);
            this.startTransButton.Name = "startTransButton";
            this.startTransButton.Size = new System.Drawing.Size(187, 74);
            this.startTransButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.startTransButton.TabIndex = 7;
            this.startTransButton.TabStop = false;
            this.startTransButton.Click += new System.EventHandler(this.startTransButton_Click);
            this.startTransButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startTransButton_MouseDown);
            this.startTransButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.startTransButton_MouseUp);
            // 
            // stopButton
            // 
            this.stopButton.Image = global::MORT.Properties.Resources.Remote_Stop;
            this.stopButton.Location = new System.Drawing.Point(7, 217);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(187, 74);
            this.stopButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.stopButton.TabIndex = 8;
            this.stopButton.TabStop = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            this.stopButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.stopButton_MouseDown);
            this.stopButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.stopButton_MouseUp);
            // 
            // snapButton
            // 
            this.snapButton.Image = global::MORT.Properties.Resources.Remote_Snap_Shot;
            this.snapButton.Location = new System.Drawing.Point(7, 132);
            this.snapButton.Name = "snapButton";
            this.snapButton.Size = new System.Drawing.Size(187, 74);
            this.snapButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.snapButton.TabIndex = 13;
            this.snapButton.TabStop = false;
            this.snapButton.Click += new System.EventHandler(this.snapButton_Click);
            this.snapButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.setSnapShotButton_MouseDown);
            this.snapButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.setSnapShotButton_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MORT.Properties.Resources.Remote_Site;
            this.pictureBox1.Location = new System.Drawing.Point(7, 302);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(190, 13);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // RTT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(202, 327);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.snapButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startTransButton);
            this.Controls.Add(this.setCaptureAreaButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.settingButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTT";
            this.Text = "RTT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTT_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RTT_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RTT_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.settingButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCaptureAreaButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTransButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.snapButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox settingButton;
        private System.Windows.Forms.PictureBox closeButton;
        private System.Windows.Forms.PictureBox setCaptureAreaButton;
        private System.Windows.Forms.PictureBox startTransButton;
        private System.Windows.Forms.PictureBox stopButton;
        private System.Windows.Forms.PictureBox snapButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}