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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTT));
            settingButton = new System.Windows.Forms.PictureBox();
            closeButton = new System.Windows.Forms.PictureBox();
            setCaptureAreaButton = new System.Windows.Forms.PictureBox();
            startTransButton = new System.Windows.Forms.PictureBox();
            stopButton = new System.Windows.Forms.PictureBox();
            snapButton = new System.Windows.Forms.PictureBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)settingButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)closeButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)setCaptureAreaButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)startTransButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)stopButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)snapButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // settingButton
            // 
            settingButton.Image = Properties.Resources.Remote_Option1;
            settingButton.Location = new System.Drawing.Point(6, 6);
            settingButton.Name = "settingButton";
            settingButton.Size = new System.Drawing.Size(18, 21);
            settingButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            settingButton.TabIndex = 4;
            settingButton.TabStop = false;
            settingButton.Click += settingButton_Click;
            settingButton.MouseDown += settingButton_MouseDown;
            settingButton.MouseUp += settingButton_MouseUp;
            // 
            // closeButton
            // 
            closeButton.Image = Properties.Resources.Remote_Exit;
            closeButton.Location = new System.Drawing.Point(170, 2);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(29, 29);
            closeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            closeButton.TabIndex = 5;
            closeButton.TabStop = false;
            closeButton.Click += closeButton_Click;
            closeButton.MouseDown += closeButton_MouseDown;
            closeButton.MouseUp += closeButton_MouseUp;
            // 
            // setCaptureAreaButton
            // 
            setCaptureAreaButton.Image = Properties.Resources.Remote_Search;
            setCaptureAreaButton.Location = new System.Drawing.Point(6, 47);
            setCaptureAreaButton.Name = "setCaptureAreaButton";
            setCaptureAreaButton.Size = new System.Drawing.Size(187, 74);
            setCaptureAreaButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            setCaptureAreaButton.TabIndex = 6;
            setCaptureAreaButton.TabStop = false;
            setCaptureAreaButton.Click += setCaptureAreaButton_Click;
            setCaptureAreaButton.MouseDown += setCaptureAreaButton_MouseDown;
            setCaptureAreaButton.MouseLeave += setCaptureAreaButton_MouseLeave;
            setCaptureAreaButton.MouseUp += setCaptureAreaButton_MouseUp;
            // 
            // startTransButton
            // 
            startTransButton.Image = Properties.Resources.Remote_Translate;
            startTransButton.Location = new System.Drawing.Point(7, 217);
            startTransButton.Name = "startTransButton";
            startTransButton.Size = new System.Drawing.Size(187, 74);
            startTransButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            startTransButton.TabIndex = 7;
            startTransButton.TabStop = false;
            startTransButton.Click += startTransButton_Click;
            startTransButton.MouseDown += startTransButton_MouseDown;
            startTransButton.MouseUp += startTransButton_MouseUp;
            // 
            // stopButton
            // 
            stopButton.Image = Properties.Resources.Remote_Stop;
            stopButton.Location = new System.Drawing.Point(7, 217);
            stopButton.Name = "stopButton";
            stopButton.Size = new System.Drawing.Size(187, 74);
            stopButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            stopButton.TabIndex = 8;
            stopButton.TabStop = false;
            stopButton.Click += stopButton_Click;
            stopButton.MouseDown += stopButton_MouseDown;
            stopButton.MouseUp += stopButton_MouseUp;
            // 
            // snapButton
            // 
            snapButton.Image = Properties.Resources.Remote_Snap_Shot;
            snapButton.Location = new System.Drawing.Point(7, 132);
            snapButton.Name = "snapButton";
            snapButton.Size = new System.Drawing.Size(187, 74);
            snapButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            snapButton.TabIndex = 13;
            snapButton.TabStop = false;
            snapButton.Click += snapButton_Click;
            snapButton.MouseDown += setSnapShotButton_MouseDown;
            snapButton.MouseUp += setSnapShotButton_MouseUp;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Remote_Site;
            pictureBox1.Location = new System.Drawing.Point(7, 302);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(190, 13);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // RTT
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.FromArgb(42, 42, 42);
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(202, 327);
            Controls.Add(pictureBox1);
            Controls.Add(snapButton);
            Controls.Add(stopButton);
            Controls.Add(startTransButton);
            Controls.Add(setCaptureAreaButton);
            Controls.Add(closeButton);
            Controls.Add(settingButton);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(202, 327);
            Name = "RTT";
            Text = "RTT";
            FormClosing += RTT_FormClosing;
            MouseDown += RTT_MouseDown;
            MouseMove += RTT_MouseMove;
            ((System.ComponentModel.ISupportInitialize)settingButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)closeButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)setCaptureAreaButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)startTransButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)stopButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)snapButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
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