namespace MORT
{
    partial class ColorPickerForm
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
            this.screenPictureBox = new System.Windows.Forms.PictureBox();
            this.imgPanel = new System.Windows.Forms.Panel();
            this.binaryScreenPictureBox = new System.Windows.Forms.PictureBox();
            this.zoomComboBox = new System.Windows.Forms.ComboBox();
            this.informationPanel = new System.Windows.Forms.Panel();
            this.processButton = new System.Windows.Forms.Button();
            this.colorBox = new System.Windows.Forms.PictureBox();
            this.bLabel = new System.Windows.Forms.Label();
            this.vLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gLabel = new System.Windows.Forms.Label();
            this.sLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rLabel = new System.Windows.Forms.Label();
            this.hLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.screenPictureBox)).BeginInit();
            this.imgPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binaryScreenPictureBox)).BeginInit();
            this.informationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // screenPictureBox
            // 
            this.screenPictureBox.Location = new System.Drawing.Point(0, 0);
            this.screenPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.screenPictureBox.Name = "screenPictureBox";
            this.screenPictureBox.Size = new System.Drawing.Size(100, 50);
            this.screenPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.screenPictureBox.TabIndex = 0;
            this.screenPictureBox.TabStop = false;
            this.screenPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screenPictureBox_MouseMove);
            // 
            // imgPanel
            // 
            this.imgPanel.AutoScroll = true;
            this.imgPanel.Controls.Add(this.binaryScreenPictureBox);
            this.imgPanel.Controls.Add(this.screenPictureBox);
            this.imgPanel.Location = new System.Drawing.Point(0, 0);
            this.imgPanel.Margin = new System.Windows.Forms.Padding(0);
            this.imgPanel.Name = "imgPanel";
            this.imgPanel.Size = new System.Drawing.Size(710, 267);
            this.imgPanel.TabIndex = 1;
            // 
            // binaryScreenPictureBox
            // 
            this.binaryScreenPictureBox.Location = new System.Drawing.Point(0, 0);
            this.binaryScreenPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.binaryScreenPictureBox.Name = "binaryScreenPictureBox";
            this.binaryScreenPictureBox.Size = new System.Drawing.Size(100, 50);
            this.binaryScreenPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.binaryScreenPictureBox.TabIndex = 1;
            this.binaryScreenPictureBox.TabStop = false;
            this.binaryScreenPictureBox.Visible = false;
            this.binaryScreenPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screenPictureBox_MouseMove);
            // 
            // zoomComboBox
            // 
            this.zoomComboBox.FormattingEnabled = true;
            this.zoomComboBox.Items.AddRange(new object[] {
            "100%",
            "200%",
            "300%",
            "400%"});
            this.zoomComboBox.Location = new System.Drawing.Point(3, 4);
            this.zoomComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.zoomComboBox.Name = "zoomComboBox";
            this.zoomComboBox.Size = new System.Drawing.Size(161, 20);
            this.zoomComboBox.TabIndex = 2;
            this.zoomComboBox.Text = "100%";
            this.zoomComboBox.TextUpdate += new System.EventHandler(this.zoomComboBox_TextUpdate);
            this.zoomComboBox.SelectedValueChanged += new System.EventHandler(this.zoomComboBox_SelectedValueChanged);
            // 
            // informationPanel
            // 
            this.informationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.informationPanel.BackColor = System.Drawing.Color.White;
            this.informationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.informationPanel.Controls.Add(this.processButton);
            this.informationPanel.Controls.Add(this.colorBox);
            this.informationPanel.Controls.Add(this.bLabel);
            this.informationPanel.Controls.Add(this.vLabel);
            this.informationPanel.Controls.Add(this.label1);
            this.informationPanel.Controls.Add(this.label9);
            this.informationPanel.Controls.Add(this.gLabel);
            this.informationPanel.Controls.Add(this.sLabel);
            this.informationPanel.Controls.Add(this.label2);
            this.informationPanel.Controls.Add(this.label8);
            this.informationPanel.Controls.Add(this.rLabel);
            this.informationPanel.Controls.Add(this.hLabel);
            this.informationPanel.Controls.Add(this.label3);
            this.informationPanel.Controls.Add(this.label7);
            this.informationPanel.Controls.Add(this.zoomComboBox);
            this.informationPanel.Location = new System.Drawing.Point(719, 1);
            this.informationPanel.Margin = new System.Windows.Forms.Padding(0);
            this.informationPanel.Name = "informationPanel";
            this.informationPanel.Size = new System.Drawing.Size(168, 267);
            this.informationPanel.TabIndex = 15;
            // 
            // processButton
            // 
            this.processButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.processButton.Location = new System.Drawing.Point(26, 232);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(114, 28);
            this.processButton.TabIndex = 29;
            this.processButton.Text = "이진화";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // colorBox
            // 
            this.colorBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colorBox.Location = new System.Drawing.Point(14, 40);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(40, 35);
            this.colorBox.TabIndex = 28;
            this.colorBox.TabStop = false;
            // 
            // bLabel
            // 
            this.bLabel.AutoSize = true;
            this.bLabel.Location = new System.Drawing.Point(43, 209);
            this.bLabel.Name = "bLabel";
            this.bLabel.Size = new System.Drawing.Size(11, 12);
            this.bLabel.TabIndex = 27;
            this.bLabel.Text = "0";
            // 
            // vLabel
            // 
            this.vLabel.AutoSize = true;
            this.vLabel.Location = new System.Drawing.Point(43, 133);
            this.vLabel.Name = "vLabel";
            this.vLabel.Size = new System.Drawing.Size(11, 12);
            this.vLabel.TabIndex = 21;
            this.vLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "H : ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 165);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "R : ";
            // 
            // gLabel
            // 
            this.gLabel.AutoSize = true;
            this.gLabel.Location = new System.Drawing.Point(43, 186);
            this.gLabel.Name = "gLabel";
            this.gLabel.Size = new System.Drawing.Size(11, 12);
            this.gLabel.TabIndex = 26;
            this.gLabel.Text = "0";
            // 
            // sLabel
            // 
            this.sLabel.AutoSize = true;
            this.sLabel.Location = new System.Drawing.Point(43, 110);
            this.sLabel.Name = "sLabel";
            this.sLabel.Size = new System.Drawing.Size(11, 12);
            this.sLabel.TabIndex = 20;
            this.sLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "S :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "G : ";
            // 
            // rLabel
            // 
            this.rLabel.AutoSize = true;
            this.rLabel.Location = new System.Drawing.Point(43, 165);
            this.rLabel.Name = "rLabel";
            this.rLabel.Size = new System.Drawing.Size(11, 12);
            this.rLabel.TabIndex = 25;
            this.rLabel.Text = "0";
            // 
            // hLabel
            // 
            this.hLabel.AutoSize = true;
            this.hLabel.Location = new System.Drawing.Point(43, 89);
            this.hLabel.Name = "hLabel";
            this.hLabel.Size = new System.Drawing.Size(11, 12);
            this.hLabel.TabIndex = 19;
            this.hLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "V :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "B : ";
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(886, 268);
            this.Controls.Add(this.informationPanel);
            this.Controls.Add(this.imgPanel);
            this.DoubleBuffered = true;
            this.Name = "ColorPickerForm";
            this.ShowIcon = false;
            this.Text = "색 추출";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ColorPickerForm_FormClosed);
            this.Resize += new System.EventHandler(this.ColorPickerForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.screenPictureBox)).EndInit();
            this.imgPanel.ResumeLayout(false);
            this.imgPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binaryScreenPictureBox)).EndInit();
            this.informationPanel.ResumeLayout(false);
            this.informationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox screenPictureBox;
        private System.Windows.Forms.Panel imgPanel;
        private System.Windows.Forms.ComboBox zoomComboBox;
        private System.Windows.Forms.Panel informationPanel;
        private System.Windows.Forms.PictureBox binaryScreenPictureBox;
        private System.Windows.Forms.PictureBox colorBox;
        private System.Windows.Forms.Label bLabel;
        private System.Windows.Forms.Label vLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label gLabel;
        private System.Windows.Forms.Label sLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label rLabel;
        private System.Windows.Forms.Label hLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button processButton;
    }
}