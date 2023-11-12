namespace MORT.EasyOcrInstaller
{
    partial class EasyOcrInstaller
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
            btnInstall = new System.Windows.Forms.Button();
            btnEnableGpuInstallStep = new System.Windows.Forms.Button();
            pnMain = new System.Windows.Forms.Panel();
            checkBox1 = new System.Windows.Forms.CheckBox();
            pnEnableGPU = new System.Windows.Forms.Panel();
            radioButton2 = new System.Windows.Forms.RadioButton();
            radioButton1 = new System.Windows.Forms.RadioButton();
            btnEnableGpuInstall = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            comboBox1 = new System.Windows.Forms.ComboBox();
            pnLog = new System.Windows.Forms.Panel();
            tbLog = new System.Windows.Forms.TextBox();
            pnMain.SuspendLayout();
            pnEnableGPU.SuspendLayout();
            pnLog.SuspendLayout();
            SuspendLayout();
            // 
            // btnInstall
            // 
            btnInstall.Location = new System.Drawing.Point(3, 48);
            btnInstall.Name = "btnInstall";
            btnInstall.Size = new System.Drawing.Size(858, 46);
            btnInstall.TabIndex = 0;
            btnInstall.Text = "기본 설치";
            btnInstall.UseVisualStyleBackColor = true;
            btnInstall.Click += btnInstall_Click;
            // 
            // btnEnableGpuInstallStep
            // 
            btnEnableGpuInstallStep.Location = new System.Drawing.Point(3, 100);
            btnEnableGpuInstallStep.Name = "btnEnableGpuInstallStep";
            btnEnableGpuInstallStep.Size = new System.Drawing.Size(858, 46);
            btnEnableGpuInstallStep.TabIndex = 1;
            btnEnableGpuInstallStep.Text = "GPU 가속 버전으로 설치 - cuda 필요";
            btnEnableGpuInstallStep.UseVisualStyleBackColor = true;
            // 
            // pnMain
            // 
            pnMain.Controls.Add(checkBox1);
            pnMain.Controls.Add(btnInstall);
            pnMain.Controls.Add(btnEnableGpuInstallStep);
            pnMain.Location = new System.Drawing.Point(9, 17);
            pnMain.Name = "pnMain";
            pnMain.Size = new System.Drawing.Size(861, 201);
            pnMain.TabIndex = 2;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(12, 171);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(90, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "강제 재설치";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // pnEnableGPU
            // 
            pnEnableGPU.Controls.Add(radioButton2);
            pnEnableGPU.Controls.Add(radioButton1);
            pnEnableGPU.Controls.Add(btnEnableGpuInstall);
            pnEnableGPU.Controls.Add(textBox1);
            pnEnableGPU.Controls.Add(label2);
            pnEnableGPU.Controls.Add(label1);
            pnEnableGPU.Controls.Add(comboBox1);
            pnEnableGPU.Location = new System.Drawing.Point(11, 279);
            pnEnableGPU.Name = "pnEnableGPU";
            pnEnableGPU.Size = new System.Drawing.Size(861, 272);
            pnEnableGPU.TabIndex = 3;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(18, 114);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(117, 19);
            radioButton2.TabIndex = 5;
            radioButton2.TabStop = true;
            radioButton2.Text = "명령어 직접 입력";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(18, 47);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(86, 19);
            radioButton1.TabIndex = 4;
            radioButton1.TabStop = true;
            radioButton1.Text = "CUDA 선택";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // btnEnableGpuInstall
            // 
            btnEnableGpuInstall.Location = new System.Drawing.Point(3, 176);
            btnEnableGpuInstall.Name = "btnEnableGpuInstall";
            btnEnableGpuInstall.Size = new System.Drawing.Size(858, 46);
            btnEnableGpuInstall.TabIndex = 2;
            btnEnableGpuInstall.Text = "서리";
            btnEnableGpuInstall.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(275, 114);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(433, 23);
            textBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(275, 88);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 15);
            label2.TabIndex = 2;
            label2.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(275, 18);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(275, 47);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(433, 23);
            comboBox1.TabIndex = 0;
            // 
            // pnLog
            // 
            pnLog.Controls.Add(tbLog);
            pnLog.Location = new System.Drawing.Point(9, 574);
            pnLog.Name = "pnLog";
            pnLog.Size = new System.Drawing.Size(861, 201);
            pnLog.TabIndex = 3;
            // 
            // tbLog
            // 
            tbLog.Location = new System.Drawing.Point(10, 18);
            tbLog.Multiline = true;
            tbLog.Name = "tbLog";
            tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            tbLog.Size = new System.Drawing.Size(836, 162);
            tbLog.TabIndex = 0;
            // 
            // EasyOcrInstaller
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(888, 796);
            Controls.Add(pnLog);
            Controls.Add(pnEnableGPU);
            Controls.Add(pnMain);
            Name = "EasyOcrInstaller";
            Text = "EasyOcrInstaller";
            FormClosed += EasyOcrInstaller_FormClosed;
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            pnEnableGPU.ResumeLayout(false);
            pnEnableGPU.PerformLayout();
            pnLog.ResumeLayout(false);
            pnLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnEnableGpuInstallStep;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnEnableGPU;
        private System.Windows.Forms.Button btnEnableGpuInstall;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel pnLog;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}