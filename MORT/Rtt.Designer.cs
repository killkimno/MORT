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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.settingButton = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.PictureBox();
            this.setCaptureAreaButton = new System.Windows.Forms.PictureBox();
            this.startTransButton = new System.Windows.Forms.PictureBox();
            this.stopButton = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.searchLabel = new System.Windows.Forms.Label();
            this.translateLabel = new System.Windows.Forms.Label();
            this.stopLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCaptureAreaButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTransButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopButton)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MORT.Properties.Resources.back_low;
            this.pictureBox1.Location = new System.Drawing.Point(0, 261);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 8);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MORT.Properties.Resources.title1;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(98, 48);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::MORT.Properties.Resources.back_top;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(202, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Red;
            this.pictureBox4.Location = new System.Drawing.Point(202, 0);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(35, 269);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // settingButton
            // 
            this.settingButton.Image = global::MORT.Properties.Resources.setting_button;
            this.settingButton.Location = new System.Drawing.Point(135, 9);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(35, 35);
            this.settingButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.settingButton.TabIndex = 4;
            this.settingButton.TabStop = false;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Image = global::MORT.Properties.Resources.exit_button2;
            this.closeButton.Location = new System.Drawing.Point(176, 9);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(36, 35);
            this.closeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.closeButton.TabIndex = 5;
            this.closeButton.TabStop = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // setCaptureAreaButton
            // 
            this.setCaptureAreaButton.Image = global::MORT.Properties.Resources.box_setting_button;
            this.setCaptureAreaButton.Location = new System.Drawing.Point(12, 58);
            this.setCaptureAreaButton.Name = "setCaptureAreaButton";
            this.setCaptureAreaButton.Size = new System.Drawing.Size(176, 109);
            this.setCaptureAreaButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.setCaptureAreaButton.TabIndex = 6;
            this.setCaptureAreaButton.TabStop = false;
            this.setCaptureAreaButton.Click += new System.EventHandler(this.setCaptureAreaButton_Click);
            this.setCaptureAreaButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.setCaptureAreaButton_MouseDown);
            this.setCaptureAreaButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.setCaptureAreaButton_MouseUp);
            // 
            // startTransButton
            // 
            this.startTransButton.Image = global::MORT.Properties.Resources.translate_start_button;
            this.startTransButton.Location = new System.Drawing.Point(12, 175);
            this.startTransButton.Name = "startTransButton";
            this.startTransButton.Size = new System.Drawing.Size(85, 85);
            this.startTransButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.startTransButton.TabIndex = 7;
            this.startTransButton.TabStop = false;
            this.startTransButton.Click += new System.EventHandler(this.startTransButton_Click);
            this.startTransButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startTransButton_MouseDown);
            this.startTransButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.startTransButton_MouseUp);
            // 
            // stopButton
            // 
            this.stopButton.Image = global::MORT.Properties.Resources.pause_button;
            this.stopButton.Location = new System.Drawing.Point(103, 175);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(85, 85);
            this.stopButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.stopButton.TabIndex = 8;
            this.stopButton.TabStop = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            this.stopButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.stopButton_MouseDown);
            this.stopButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.stopButton_MouseUp);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(151)))), ((int)(((byte)(255)))));
            this.titleLabel.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(15, 11);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(69, 25);
            this.titleLabel.TabIndex = 9;
            this.titleLabel.Text = "리모컨";
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseMove);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.searchLabel.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.searchLabel.ForeColor = System.Drawing.Color.White;
            this.searchLabel.Location = new System.Drawing.Point(68, 135);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(71, 25);
            this.searchLabel.TabIndex = 10;
            this.searchLabel.Text = "Search";
            this.searchLabel.Click += new System.EventHandler(this.setCaptureAreaButton_Click);
            this.searchLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.setCaptureAreaButton_MouseDown);
            this.searchLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.setCaptureAreaButton_MouseUp);
            // 
            // translateLabel
            // 
            this.translateLabel.AutoSize = true;
            this.translateLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(151)))), ((int)(((byte)(255)))));
            this.translateLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.translateLabel.ForeColor = System.Drawing.Color.White;
            this.translateLabel.Location = new System.Drawing.Point(16, 232);
            this.translateLabel.Name = "translateLabel";
            this.translateLabel.Size = new System.Drawing.Size(79, 21);
            this.translateLabel.TabIndex = 11;
            this.translateLabel.Text = "Translate";
            this.translateLabel.Click += new System.EventHandler(this.startTransButton_Click);
            this.translateLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startTransButton_MouseDown);
            this.translateLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.startTransButton_MouseUp);
            // 
            // stopLabel
            // 
            this.stopLabel.AutoSize = true;
            this.stopLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.stopLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.stopLabel.ForeColor = System.Drawing.Color.White;
            this.stopLabel.Location = new System.Drawing.Point(125, 232);
            this.stopLabel.Name = "stopLabel";
            this.stopLabel.Size = new System.Drawing.Size(45, 21);
            this.stopLabel.TabIndex = 12;
            this.stopLabel.Text = "Stop";
            this.stopLabel.Click += new System.EventHandler(this.stopButton_Click);
            this.stopLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.stopButton_MouseDown);
            this.stopLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.stopButton_MouseUp);
            // 
            // RTT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(212, 269);
            this.Controls.Add(this.stopLabel);
            this.Controls.Add(this.translateLabel);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startTransButton);
            this.Controls.Add(this.setCaptureAreaButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.settingButton);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTT";
            this.Text = "RTT";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTT_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RTT_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RTT_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCaptureAreaButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTransButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox settingButton;
        private System.Windows.Forms.PictureBox closeButton;
        private System.Windows.Forms.PictureBox setCaptureAreaButton;
        private System.Windows.Forms.PictureBox startTransButton;
        private System.Windows.Forms.PictureBox stopButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Label translateLabel;
        private System.Windows.Forms.Label stopLabel;
    }
}