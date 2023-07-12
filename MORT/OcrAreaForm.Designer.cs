namespace MORT
{
    partial class OcrAreaForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.exit_button = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.color_picker_button = new System.Windows.Forms.PictureBox();
            this.color_group_button = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.exit_button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_picker_button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_group_button)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 58);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panealBorder_MouseDown);
            this.panel1.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panealBorder_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panealBorder_MouseUp);
            // 
            // exit_button
            // 
            this.exit_button.Image = global::MORT.Properties.Resources.close_button;
            this.exit_button.Location = new System.Drawing.Point(267, 0);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(20, 20);
            this.exit_button.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exit_button.TabIndex = 1;
            this.exit_button.TabStop = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.Black;
            this.titleLabel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(139, 15);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "label1";
            this.titleLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.titleLabel_Paint);
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseDown);
            this.titleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseMove);
            this.titleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseUp);
            // 
            // color_picker_button
            // 
            this.color_picker_button.Image = global::MORT.Properties.Resources.color_picker_button;
            this.color_picker_button.Location = new System.Drawing.Point(242, 0);
            this.color_picker_button.Name = "color_picker_button";
            this.color_picker_button.Size = new System.Drawing.Size(20, 20);
            this.color_picker_button.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.color_picker_button.TabIndex = 2;
            this.color_picker_button.TabStop = false;
            this.color_picker_button.Click += new System.EventHandler(this.color_picker_button_Click);
            // 
            // color_group_button
            // 
            this.color_group_button.Image = global::MORT.Properties.Resources.color_group_button;
            this.color_group_button.Location = new System.Drawing.Point(216, 0);
            this.color_group_button.Name = "color_group_button";
            this.color_group_button.Size = new System.Drawing.Size(20, 20);
            this.color_group_button.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.color_group_button.TabIndex = 3;
            this.color_group_button.TabStop = false;
            this.color_group_button.Click += new System.EventHandler(this.color_group_button_Click);
            // 
            // OcrAreaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.color_group_button);
            this.Controls.Add(this.color_picker_button);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OcrAreaForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "OcrAreaForm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OcrAreaForm_FormClosed);
            this.Move += new System.EventHandler(this.OcrAreaForm_Move);
            this.Resize += new System.EventHandler(this.OcrAreaForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.exit_button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_picker_button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.color_group_button)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox exit_button;
        private System.Windows.Forms.PictureBox color_picker_button;
        private System.Windows.Forms.PictureBox color_group_button;
    }
}