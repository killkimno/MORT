
namespace MORT
{
    partial class UIAdvencedOption
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
            this.button2 = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.DicTab = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.udReProcessDicCount = new System.Windows.Forms.NumericUpDown();
            this.TransTab = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cbShowProcessClipboard = new System.Windows.Forms.CheckBox();
            this.cbIsShowClipboardOriginal = new System.Windows.Forms.CheckBox();
            this.cbIsUseClipboardTrans = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbJpnExecutive = new System.Windows.Forms.CheckBox();
            this.TransZipTab = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbDbOption = new System.Windows.Forms.GroupBox();
            this.cbCheckStringUpper = new System.Windows.Forms.CheckBox();
            this.cbUseDbStyle = new System.Windows.Forms.CheckBox();
            this.tbInformation = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btAllOff = new System.Windows.Forms.Button();
            this.btAllOn = new System.Windows.Forms.Button();
            this.cblTransration = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TransFormTab = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cbTopMost = new System.Windows.Forms.CheckBox();
            this.cbIgonreEmpty = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.udMaxSFontize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.udMinFontSize = new System.Windows.Forms.NumericUpDown();
            this.cbOverlayAutoSize = new System.Windows.Forms.CheckBox();
            this.HotKeyTab = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.AppConfigTab = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cbEnableSystemTray = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.btnFont = new System.Windows.Forms.Button();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.ctLayerTransparencyHotKey = new MORT.CustomControl.CtHotKey();
            this.ctSettingHotKey4 = new MORT.CustomControl.CtSettingHotKey();
            this.ctSettingHotKey3 = new MORT.CustomControl.CtSettingHotKey();
            this.ctSettingHotKey2 = new MORT.CustomControl.CtSettingHotKey();
            this.ctSettingHotKey1 = new MORT.CustomControl.CtSettingHotKey();
            this.DicTab.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udReProcessDicCount)).BeginInit();
            this.TransTab.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.TransZipTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbDbOption.SuspendLayout();
            this.TransFormTab.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxSFontize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinFontSize)).BeginInit();
            this.HotKeyTab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.AppConfigTab.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(398, 444);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(386, 44);
            this.button2.TabIndex = 83;
            this.button2.Text = "적용";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.OnClickApply);
            // 
            // btReset
            // 
            this.btReset.BackColor = System.Drawing.Color.Gainsboro;
            this.btReset.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReset.ForeColor = System.Drawing.Color.Black;
            this.btReset.Location = new System.Drawing.Point(17, 444);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(380, 44);
            this.btReset.TabIndex = 84;
            this.btReset.Text = "초기화";
            this.btReset.UseVisualStyleBackColor = false;
            this.btReset.Click += new System.EventHandler(this.OnClickReset);
            // 
            // DicTab
            // 
            this.DicTab.Controls.Add(this.groupBox6);
            this.DicTab.Location = new System.Drawing.Point(4, 39);
            this.DicTab.Name = "DicTab";
            this.DicTab.Padding = new System.Windows.Forms.Padding(3);
            this.DicTab.Size = new System.Drawing.Size(767, 382);
            this.DicTab.TabIndex = 19;
            this.DicTab.Text = "교정 사전";
            this.DicTab.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.udReProcessDicCount);
            this.groupBox6.Location = new System.Drawing.Point(20, 30);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(711, 153);
            this.groupBox6.TabIndex = 69;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "OCR 결과 교정 사전";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(13, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(336, 30);
            this.label6.TabIndex = 9;
            this.label6.Text = "- 교정이 완료된 문장을 다시 읽은후 재 교정 하는 횟수입니다\r\n- 교정된 문장을 다시 교정해 원하는 결과가 나오게 합니다";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(13, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "재교정 처리 횟수 (기본값 : 0)";
            // 
            // udReProcessDicCount
            // 
            this.udReProcessDicCount.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.udReProcessDicCount.Location = new System.Drawing.Point(184, 26);
            this.udReProcessDicCount.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udReProcessDicCount.Name = "udReProcessDicCount";
            this.udReProcessDicCount.Size = new System.Drawing.Size(62, 23);
            this.udReProcessDicCount.TabIndex = 7;
            this.udReProcessDicCount.ValueChanged += new System.EventHandler(this.udReProcessDicCount_ValueChanged);
            // 
            // TransTab
            // 
            this.TransTab.Controls.Add(this.groupBox7);
            this.TransTab.Controls.Add(this.groupBox5);
            this.TransTab.Location = new System.Drawing.Point(4, 39);
            this.TransTab.Name = "TransTab";
            this.TransTab.Padding = new System.Windows.Forms.Padding(3);
            this.TransTab.Size = new System.Drawing.Size(767, 382);
            this.TransTab.TabIndex = 18;
            this.TransTab.Text = "번역 설정";
            this.TransTab.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cbShowProcessClipboard);
            this.groupBox7.Controls.Add(this.cbIsShowClipboardOriginal);
            this.groupBox7.Controls.Add(this.cbIsUseClipboardTrans);
            this.groupBox7.Location = new System.Drawing.Point(20, 189);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(711, 159);
            this.groupBox7.TabIndex = 69;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "클립보드 감지";
            // 
            // cbShowProcessClipboard
            // 
            this.cbShowProcessClipboard.AutoSize = true;
            this.cbShowProcessClipboard.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbShowProcessClipboard.Location = new System.Drawing.Point(15, 128);
            this.cbShowProcessClipboard.Name = "cbShowProcessClipboard";
            this.cbShowProcessClipboard.Size = new System.Drawing.Size(142, 19);
            this.cbShowProcessClipboard.TabIndex = 6;
            this.cbShowProcessClipboard.Text = "클립보드 번역중 표시";
            this.cbShowProcessClipboard.UseVisualStyleBackColor = true;
            // 
            // cbIsShowClipboardOriginal
            // 
            this.cbIsShowClipboardOriginal.AutoSize = true;
            this.cbIsShowClipboardOriginal.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbIsShowClipboardOriginal.Location = new System.Drawing.Point(15, 98);
            this.cbIsShowClipboardOriginal.Name = "cbIsShowClipboardOriginal";
            this.cbIsShowClipboardOriginal.Size = new System.Drawing.Size(142, 19);
            this.cbIsShowClipboardOriginal.TabIndex = 5;
            this.cbIsShowClipboardOriginal.Text = "클립보드에 원문 표시";
            this.cbIsShowClipboardOriginal.UseVisualStyleBackColor = true;
            // 
            // cbIsUseClipboardTrans
            // 
            this.cbIsUseClipboardTrans.AutoSize = true;
            this.cbIsUseClipboardTrans.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbIsUseClipboardTrans.Location = new System.Drawing.Point(15, 37);
            this.cbIsUseClipboardTrans.Name = "cbIsUseClipboardTrans";
            this.cbIsUseClipboardTrans.Size = new System.Drawing.Size(466, 49);
            this.cbIsUseClipboardTrans.TabIndex = 4;
            this.cbIsUseClipboardTrans.Text = "클립보드 번역 사용\r\n[실시간 번역을 안 하고 있을 때 클립보드에서 변경된 텍스트를 감지해 번역합니다\r\n 오버레이 번역창에서는 사용불가능]\r\n";
            this.cbIsUseClipboardTrans.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbJpnExecutive);
            this.groupBox5.Location = new System.Drawing.Point(20, 30);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(711, 153);
            this.groupBox5.TabIndex = 68;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "구글 번역 (기본 번역기, 구글 시트 번역기)";
            // 
            // cbJpnExecutive
            // 
            this.cbJpnExecutive.AutoSize = true;
            this.cbJpnExecutive.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbJpnExecutive.Location = new System.Drawing.Point(15, 37);
            this.cbJpnExecutive.Name = "cbJpnExecutive";
            this.cbJpnExecutive.Size = new System.Drawing.Size(424, 34);
            this.cbJpnExecutive.TabIndex = 4;
            this.cbJpnExecutive.Text = "일본어 중역 사용  원문 -> 일본어로 번역 -> 일본어 결과를 한국어로 번역\r\n[번역 품질이 오를 수 있으나 느려집니다]";
            this.cbJpnExecutive.UseVisualStyleBackColor = true;
            // 
            // TransZipTab
            // 
            this.TransZipTab.AutoScroll = true;
            this.TransZipTab.Controls.Add(this.groupBox3);
            this.TransZipTab.Location = new System.Drawing.Point(4, 39);
            this.TransZipTab.Name = "TransZipTab";
            this.TransZipTab.Padding = new System.Windows.Forms.Padding(3);
            this.TransZipTab.Size = new System.Drawing.Size(767, 382);
            this.TransZipTab.TabIndex = 17;
            this.TransZipTab.Text = "번역집";
            this.TransZipTab.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbDbOption);
            this.groupBox3.Controls.Add(this.cbUseDbStyle);
            this.groupBox3.Controls.Add(this.tbInformation);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btAllOff);
            this.groupBox3.Controls.Add(this.btAllOn);
            this.groupBox3.Controls.Add(this.cblTransration);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(20, 30);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(711, 330);
            this.groupBox3.TabIndex = 67;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "번역집";
            // 
            // gbDbOption
            // 
            this.gbDbOption.Controls.Add(this.cbCheckStringUpper);
            this.gbDbOption.Location = new System.Drawing.Point(302, 168);
            this.gbDbOption.Name = "gbDbOption";
            this.gbDbOption.Size = new System.Drawing.Size(344, 100);
            this.gbDbOption.TabIndex = 16;
            this.gbDbOption.TabStop = false;
            this.gbDbOption.Text = "DB방식 문장 검색 옵션";
            // 
            // cbCheckStringUpper
            // 
            this.cbCheckStringUpper.AutoSize = true;
            this.cbCheckStringUpper.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbCheckStringUpper.Location = new System.Drawing.Point(6, 20);
            this.cbCheckStringUpper.Name = "cbCheckStringUpper";
            this.cbCheckStringUpper.Size = new System.Drawing.Size(134, 19);
            this.cbCheckStringUpper.TabIndex = 17;
            this.cbCheckStringUpper.Text = "대소문자 구분 안 함";
            this.cbCheckStringUpper.UseVisualStyleBackColor = true;
            // 
            // cbUseDbStyle
            // 
            this.cbUseDbStyle.AutoSize = true;
            this.cbUseDbStyle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbUseDbStyle.Location = new System.Drawing.Point(302, 123);
            this.cbUseDbStyle.Name = "cbUseDbStyle";
            this.cbUseDbStyle.Size = new System.Drawing.Size(246, 34);
            this.cbUseDbStyle.TabIndex = 15;
            this.cbUseDbStyle.Text = "비슷한 문장이면 가져오기 (DB 검색방식)\r\n[영문, 일본어만 가능]";
            this.cbUseDbStyle.UseVisualStyleBackColor = true;
            this.cbUseDbStyle.CheckedChanged += new System.EventHandler(this.cbUseDbStyle_CheckedChanged);
            // 
            // tbInformation
            // 
            this.tbInformation.Location = new System.Drawing.Point(302, 41);
            this.tbInformation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbInformation.Size = new System.Drawing.Size(344, 61);
            this.tbInformation.TabIndex = 14;
            this.tbInformation.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(299, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "번역집 정보";
            // 
            // btAllOff
            // 
            this.btAllOff.Location = new System.Drawing.Point(28, 274);
            this.btAllOff.Name = "btAllOff";
            this.btAllOff.Size = new System.Drawing.Size(75, 23);
            this.btAllOff.TabIndex = 10;
            this.btAllOff.Text = "모두 해제";
            this.btAllOff.UseVisualStyleBackColor = true;
            this.btAllOff.Click += new System.EventHandler(this.OnClickTransrtionAllOff);
            // 
            // btAllOn
            // 
            this.btAllOn.Location = new System.Drawing.Point(150, 274);
            this.btAllOn.Name = "btAllOn";
            this.btAllOn.Size = new System.Drawing.Size(75, 23);
            this.btAllOn.TabIndex = 9;
            this.btAllOn.Text = "모두 선택";
            this.btAllOn.UseVisualStyleBackColor = true;
            this.btAllOn.Click += new System.EventHandler(this.OnClickTransrtionAllOn);
            // 
            // cblTransration
            // 
            this.cblTransration.CheckOnClick = true;
            this.cblTransration.FormattingEnabled = true;
            this.cblTransration.Location = new System.Drawing.Point(28, 40);
            this.cblTransration.Name = "cblTransration";
            this.cblTransration.Size = new System.Drawing.Size(197, 228);
            this.cblTransration.TabIndex = 8;
            this.cblTransration.SelectedIndexChanged += new System.EventHandler(this.cblTransration_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(25, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "사용할 번역집";
            // 
            // TransFormTab
            // 
            this.TransFormTab.AutoScroll = true;
            this.TransFormTab.Controls.Add(this.groupBox10);
            this.TransFormTab.Controls.Add(this.groupBox9);
            this.TransFormTab.Controls.Add(this.groupBox2);
            this.TransFormTab.Location = new System.Drawing.Point(4, 39);
            this.TransFormTab.Name = "TransFormTab";
            this.TransFormTab.Padding = new System.Windows.Forms.Padding(3);
            this.TransFormTab.Size = new System.Drawing.Size(767, 382);
            this.TransFormTab.TabIndex = 16;
            this.TransFormTab.Text = "번역창";
            this.TransFormTab.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cbTopMost);
            this.groupBox9.Controls.Add(this.cbIgonreEmpty);
            this.groupBox9.Location = new System.Drawing.Point(20, 262);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(711, 103);
            this.groupBox9.TabIndex = 69;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "일반";
            // 
            // cbTopMost
            // 
            this.cbTopMost.AutoSize = true;
            this.cbTopMost.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbTopMost.Location = new System.Drawing.Point(15, 64);
            this.cbTopMost.Name = "cbTopMost";
            this.cbTopMost.Size = new System.Drawing.Size(226, 19);
            this.cbTopMost.TabIndex = 6;
            this.cbTopMost.Text = "번역할 때만 번역창 최상단 기능 적용";
            this.cbTopMost.UseVisualStyleBackColor = true;
            // 
            // cbIgonreEmpty
            // 
            this.cbIgonreEmpty.AutoSize = true;
            this.cbIgonreEmpty.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbIgonreEmpty.Location = new System.Drawing.Point(15, 39);
            this.cbIgonreEmpty.Name = "cbIgonreEmpty";
            this.cbIgonreEmpty.Size = new System.Drawing.Size(370, 19);
            this.cbIgonreEmpty.TabIndex = 5;
            this.cbIgonreEmpty.Text = "번역 결과가 없을 경우 번역창을 비우지 않음 (오버레이 창 제외)";
            this.cbIgonreEmpty.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.udMaxSFontize);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.udMinFontSize);
            this.groupBox2.Controls.Add(this.cbOverlayAutoSize);
            this.groupBox2.Location = new System.Drawing.Point(20, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(711, 138);
            this.groupBox2.TabIndex = 67;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "오버레이 번역창";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(13, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "폰트 최대 크기";
            // 
            // udMaxSFontize
            // 
            this.udMaxSFontize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.udMaxSFontize.Location = new System.Drawing.Point(104, 96);
            this.udMaxSFontize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udMaxSFontize.Name = "udMaxSFontize";
            this.udMaxSFontize.Size = new System.Drawing.Size(62, 23);
            this.udMaxSFontize.TabIndex = 7;
            this.udMaxSFontize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udMaxSFontize.ValueChanged += new System.EventHandler(this.udMaxSFontize_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(13, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "폰트 최소 크기";
            // 
            // udMinFontSize
            // 
            this.udMinFontSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.udMinFontSize.Location = new System.Drawing.Point(104, 62);
            this.udMinFontSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udMinFontSize.Name = "udMinFontSize";
            this.udMinFontSize.Size = new System.Drawing.Size(62, 23);
            this.udMinFontSize.TabIndex = 5;
            this.udMinFontSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udMinFontSize.ValueChanged += new System.EventHandler(this.udMinFontSize_ValueChanged);
            // 
            // cbOverlayAutoSize
            // 
            this.cbOverlayAutoSize.AutoSize = true;
            this.cbOverlayAutoSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbOverlayAutoSize.Location = new System.Drawing.Point(15, 37);
            this.cbOverlayAutoSize.Name = "cbOverlayAutoSize";
            this.cbOverlayAutoSize.Size = new System.Drawing.Size(239, 19);
            this.cbOverlayAutoSize.TabIndex = 4;
            this.cbOverlayAutoSize.Text = "자동 폰트 크기 - 원문에 맞춰 크기 조절";
            this.cbOverlayAutoSize.UseVisualStyleBackColor = true;
            // 
            // HotKeyTab
            // 
            this.HotKeyTab.AutoScroll = true;
            this.HotKeyTab.Controls.Add(this.groupBox4);
            this.HotKeyTab.Controls.Add(this.groupBox1);
            this.HotKeyTab.Location = new System.Drawing.Point(4, 39);
            this.HotKeyTab.Name = "HotKeyTab";
            this.HotKeyTab.Padding = new System.Windows.Forms.Padding(3);
            this.HotKeyTab.Size = new System.Drawing.Size(767, 382);
            this.HotKeyTab.TabIndex = 15;
            this.HotKeyTab.Text = "고급 단축키";
            this.HotKeyTab.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ctLayerTransparencyHotKey);
            this.groupBox4.Location = new System.Drawing.Point(20, 374);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(711, 93);
            this.groupBox4.TabIndex = 68;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "번역창 관련";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ctSettingHotKey4);
            this.groupBox1.Controls.Add(this.ctSettingHotKey3);
            this.groupBox1.Controls.Add(this.ctSettingHotKey2);
            this.groupBox1.Controls.Add(this.ctSettingHotKey1);
            this.groupBox1.Location = new System.Drawing.Point(20, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(711, 337);
            this.groupBox1.TabIndex = 66;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "설정 불러오기";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.AppConfigTab);
            this.tabControl1.Controls.Add(this.HotKeyTab);
            this.tabControl1.Controls.Add(this.TransFormTab);
            this.tabControl1.Controls.Add(this.TransZipTab);
            this.tabControl1.Controls.Add(this.TransTab);
            this.tabControl1.Controls.Add(this.DicTab);
            this.tabControl1.ItemSize = new System.Drawing.Size(70, 35);
            this.tabControl1.Location = new System.Drawing.Point(12, 14);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(26, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 425);
            this.tabControl1.TabIndex = 0;
            // 
            // AppConfigTab
            // 
            this.AppConfigTab.Controls.Add(this.groupBox8);
            this.AppConfigTab.Location = new System.Drawing.Point(4, 39);
            this.AppConfigTab.Name = "AppConfigTab";
            this.AppConfigTab.Padding = new System.Windows.Forms.Padding(3);
            this.AppConfigTab.Size = new System.Drawing.Size(767, 382);
            this.AppConfigTab.TabIndex = 20;
            this.AppConfigTab.Text = "앱 설정";
            this.AppConfigTab.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.cbEnableSystemTray);
            this.groupBox8.Location = new System.Drawing.Point(20, 30);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(711, 92);
            this.groupBox8.TabIndex = 68;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "일반";
            // 
            // cbEnableSystemTray
            // 
            this.cbEnableSystemTray.AutoSize = true;
            this.cbEnableSystemTray.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbEnableSystemTray.Location = new System.Drawing.Point(15, 37);
            this.cbEnableSystemTray.Name = "cbEnableSystemTray";
            this.cbEnableSystemTray.Size = new System.Drawing.Size(287, 19);
            this.cbEnableSystemTray.TabIndex = 4;
            this.cbEnableSystemTray.Text = "창을 닫을 시 MORT를 시스템 트레이드로 최소화";
            this.cbEnableSystemTray.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.btnFont);
            this.groupBox10.Location = new System.Drawing.Point(20, 174);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(711, 81);
            this.groupBox10.TabIndex = 70;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "어두운 번역창";
            // 
            // btnFont
            // 
            this.btnFont.BackColor = System.Drawing.Color.Gainsboro;
            this.btnFont.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFont.ForeColor = System.Drawing.Color.Black;
            this.btnFont.Location = new System.Drawing.Point(172, 32);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(380, 28);
            this.btnFont.TabIndex = 85;
            this.btnFont.Text = "폰트 설정";
            this.btnFont.UseVisualStyleBackColor = false;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // fontDialog
            // 
            this.fontDialog.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            // 
            // ctLayerTransparencyHotKey
            // 
            this.ctLayerTransparencyHotKey.Location = new System.Drawing.Point(6, 20);
            this.ctLayerTransparencyHotKey.Name = "ctLayerTransparencyHotKey";
            this.ctLayerTransparencyHotKey.Size = new System.Drawing.Size(465, 75);
            this.ctLayerTransparencyHotKey.TabIndex = 0;
            // 
            // ctSettingHotKey4
            // 
            this.ctSettingHotKey4.Location = new System.Drawing.Point(6, 263);
            this.ctSettingHotKey4.Name = "ctSettingHotKey4";
            this.ctSettingHotKey4.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey4.TabIndex = 3;
            // 
            // ctSettingHotKey3
            // 
            this.ctSettingHotKey3.Location = new System.Drawing.Point(6, 182);
            this.ctSettingHotKey3.Name = "ctSettingHotKey3";
            this.ctSettingHotKey3.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey3.TabIndex = 2;
            // 
            // ctSettingHotKey2
            // 
            this.ctSettingHotKey2.Location = new System.Drawing.Point(6, 101);
            this.ctSettingHotKey2.Name = "ctSettingHotKey2";
            this.ctSettingHotKey2.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey2.TabIndex = 1;
            // 
            // ctSettingHotKey1
            // 
            this.ctSettingHotKey1.Location = new System.Drawing.Point(6, 20);
            this.ctSettingHotKey1.Name = "ctSettingHotKey1";
            this.ctSettingHotKey1.Size = new System.Drawing.Size(465, 75);
            this.ctSettingHotKey1.TabIndex = 0;
            // 
            // UIAdvencedOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "UIAdvencedOption";
            this.ShowIcon = false;
            this.Text = "고급 설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UIAdvencedOption_FormClosing);
            this.Load += new System.EventHandler(this.UIAdvencedOption_Load);
            this.DicTab.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udReProcessDicCount)).EndInit();
            this.TransTab.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.TransZipTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbDbOption.ResumeLayout(false);
            this.gbDbOption.PerformLayout();
            this.TransFormTab.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxSFontize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinFontSize)).EndInit();
            this.HotKeyTab.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.AppConfigTab.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.TabPage DicTab;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown udReProcessDicCount;
        private System.Windows.Forms.TabPage TransTab;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox cbShowProcessClipboard;
        private System.Windows.Forms.CheckBox cbIsShowClipboardOriginal;
        private System.Windows.Forms.CheckBox cbIsUseClipboardTrans;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbJpnExecutive;
        private System.Windows.Forms.TabPage TransZipTab;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox gbDbOption;
        private System.Windows.Forms.CheckBox cbCheckStringUpper;
        private System.Windows.Forms.CheckBox cbUseDbStyle;
        private System.Windows.Forms.RichTextBox tbInformation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btAllOff;
        private System.Windows.Forms.Button btAllOn;
        private System.Windows.Forms.CheckedListBox cblTransration;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage TransFormTab;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown udMaxSFontize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown udMinFontSize;
        private System.Windows.Forms.CheckBox cbOverlayAutoSize;
        private System.Windows.Forms.TabPage HotKeyTab;
        private System.Windows.Forms.GroupBox groupBox4;
        private CustomControl.CtHotKey ctLayerTransparencyHotKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private CustomControl.CtSettingHotKey ctSettingHotKey4;
        private CustomControl.CtSettingHotKey ctSettingHotKey3;
        private CustomControl.CtSettingHotKey ctSettingHotKey2;
        private CustomControl.CtSettingHotKey ctSettingHotKey1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage AppConfigTab;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox cbEnableSystemTray;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox cbTopMost;
        private System.Windows.Forms.CheckBox cbIgonreEmpty;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.FontDialog fontDialog;
    }
}