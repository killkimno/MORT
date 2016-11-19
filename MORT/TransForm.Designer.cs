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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.StateLabel = new System.Windows.Forms.Label();
            this.StopStateLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // transTextBox
            // 
            this.transTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.transTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.transTextBox.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.transTextBox.ForeColor = System.Drawing.Color.White;
            this.transTextBox.Location = new System.Drawing.Point(25, 48);
            this.transTextBox.Multiline = true;
            this.transTextBox.Name = "transTextBox";
            this.transTextBox.Size = new System.Drawing.Size(912, 182);
            this.transTextBox.TabIndex = 0;
            this.transTextBox.Text = "Monkeyhead\'s OCR Realtime TransLate 1.16V\r\n제작자 : 몽키해드\r\n스킨 : 슐라인\r\n로고제작 : 김엠엘\r\n블로그 " +
    ":\r\n몽키해드 : http://killkimno.blog.me/\r\n슐라인 : http://blog.naver.com/kluge_\r\n엠엘이 : h" +
    "ttp://blog.naver.com/sabon2000\r\n";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MORT.Properties.Resources.title;
            this.pictureBox1.Location = new System.Drawing.Point(0, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(107, 41);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseMove);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::MORT.Properties.Resources.minimal_button;
            this.pictureBox2.Location = new System.Drawing.Point(881, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(42, 41);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = global::MORT.Properties.Resources.exit_button1;
            this.pictureBox3.Location = new System.Drawing.Point(932, 1);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(41, 41);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(151)))), ((int)(((byte)(255)))));
            this.StateLabel.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StateLabel.ForeColor = System.Drawing.Color.White;
            this.StateLabel.Location = new System.Drawing.Point(19, 9);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(69, 25);
            this.StateLabel.TabIndex = 4;
            this.StateLabel.Text = "번역창";
            this.StateLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseDown);
            this.StateLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseMove);
            // 
            // StopStateLabel
            // 
            this.StopStateLabel.AutoSize = true;
            this.StopStateLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.StopStateLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StopStateLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.StopStateLabel.Location = new System.Drawing.Point(875, 214);
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
            this.BackgroundImage = global::MORT.Properties.Resources.translate_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(973, 246);
            this.Controls.Add(this.StopStateLabel);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.transTextBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransForm";
            this.Text = "번역창";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox transTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Label StopStateLabel;
    }
}