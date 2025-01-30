
namespace MORT
{
    partial class Form1
    {


        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            ContextOption = new System.Windows.Forms.ContextMenuStrip(components);
            optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showTransToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rTTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            setTranslateTopMostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setCheckSpellingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setCutPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            transToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            settingSaveToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            settingLoadToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            settingDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            optionMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            설정저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            설정불러오기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fontDialog = new System.Windows.Forms.FontDialog();
            colorDialog1 = new System.Windows.Forms.ColorDialog();
            acceptButton = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            donationButton = new System.Windows.Forms.Button();
            toolTip_OCR = new System.Windows.Forms.ToolTip(components);
            tbMain = new Dotnetrix_Samples.TabControl();
            tpBasic = new System.Windows.Forms.TabPage();
            panel8 = new System.Windows.Forms.Panel();
            pnAdjustImg = new System.Windows.Forms.Panel();
            btImgResult = new System.Windows.Forms.Button();
            cbThreshold = new System.Windows.Forms.CheckBox();
            tbThreshold = new System.Windows.Forms.TextBox();
            groupCombo = new System.Windows.Forms.ComboBox();
            groupLabel = new System.Windows.Forms.Label();
            lbImgGroupCount = new System.Windows.Forms.Label();
            lbImgGroup = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            v2TextBox = new System.Windows.Forms.TextBox();
            lbAdjustImg = new System.Windows.Forms.Label();
            checkRGB = new System.Windows.Forms.CheckBox();
            s2TextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            rTextBox = new System.Windows.Forms.TextBox();
            checkErode = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            checkHSV = new System.Windows.Forms.CheckBox();
            gTextBox = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            v1TextBox = new System.Windows.Forms.TextBox();
            bTextBox = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            s1TextBox = new System.Windows.Forms.TextBox();
            pnOCR = new System.Windows.Forms.Panel();
            btOcrHelp = new System.Windows.Forms.Button();
            label48 = new System.Windows.Forms.Label();
            OCR_Type_comboBox = new System.Windows.Forms.ComboBox();
            isClipBoardcheckBox1 = new System.Windows.Forms.CheckBox();
            saveOCRCheckBox = new System.Windows.Forms.CheckBox();
            ocrLabel = new System.Windows.Forms.Label();
            showOcrCheckBox = new System.Windows.Forms.CheckBox();
            WinOCR_panel = new System.Windows.Forms.Panel();
            btnAddWinOcrLanguage = new System.Windows.Forms.Button();
            WinOCR_Language_comboBox = new System.Windows.Forms.ComboBox();
            lbWinOCRLanguage = new System.Windows.Forms.Label();
            pnEasyOcr = new System.Windows.Forms.Panel();
            btnInstallEasyOcr = new System.Windows.Forms.Button();
            cbEasyOcrCode = new System.Windows.Forms.ComboBox();
            lbEasyOcrLanguage = new System.Windows.Forms.Label();
            Tesseract_panel = new System.Windows.Forms.Panel();
            cbFastTess = new System.Windows.Forms.CheckBox();
            tesseractLanguageComboBox = new System.Windows.Forms.ComboBox();
            lbTesseractLanguage = new System.Windows.Forms.Label();
            label18 = new System.Windows.Forms.Label();
            tessDataTextBox = new System.Windows.Forms.TextBox();
            pnGoogleOcr = new System.Windows.Forms.Panel();
            cbGoogleOcrLanguge = new System.Windows.Forms.ComboBox();
            lbGoogleOCRLanguage = new System.Windows.Forms.Label();
            lbGoogleOcrStatus = new System.Windows.Forms.Label();
            btnSettingGoogleOCR = new System.Windows.Forms.Button();
            pnNHocr = new System.Windows.Forms.Panel();
            lbNHOcrInfo = new System.Windows.Forms.Label();
            pnTranslate = new System.Windows.Forms.Panel();
            pnGoogleBasic = new System.Windows.Forms.Panel();
            lbBasicStatus = new System.Windows.Forms.Label();
            lbBasicInfo = new System.Windows.Forms.Label();
            pnCustomApi = new System.Windows.Forms.Panel();
            lbCustomApiInformation = new System.Windows.Forms.Label();
            pnDeepLX = new System.Windows.Forms.Panel();
            lbDeepLXInformation = new System.Windows.Forms.Label();
            btnTransHelp = new System.Windows.Forms.Button();
            cbPerWordDic = new System.Windows.Forms.CheckBox();
            lbTransType = new System.Windows.Forms.Label();
            TransType_Combobox = new System.Windows.Forms.ComboBox();
            checkDic = new System.Windows.Forms.CheckBox();
            dicFileTextBox = new System.Windows.Forms.TextBox();
            lbDicFile = new System.Windows.Forms.Label();
            lbTransTypeTitle = new System.Windows.Forms.Label();
            pnDeepl = new System.Windows.Forms.Panel();
            btnCheckDeeplState = new System.Windows.Forms.Button();
            lbDeepLStatus = new System.Windows.Forms.Label();
            lbDeepLInfo = new System.Windows.Forms.Label();
            DB_Panel = new System.Windows.Forms.Panel();
            cbDBMultiGet = new System.Windows.Forms.CheckBox();
            checkStringUpper = new System.Windows.Forms.CheckBox();
            dbFileTextBox = new System.Windows.Forms.TextBox();
            lbDbFile = new System.Windows.Forms.Label();
            Naver_Panel = new System.Windows.Forms.Panel();
            Button_NaverTransKeyList = new System.Windows.Forms.Button();
            lbPapagoSecret = new System.Windows.Forms.Label();
            NaverSecretKeyTextBox = new System.Windows.Forms.TextBox();
            NaverIDKeyTextBox = new System.Windows.Forms.TextBox();
            lbPapagoID = new System.Windows.Forms.Label();
            Google_Panel = new System.Windows.Forms.Panel();
            button_RemoveAllGoogleToekn = new System.Windows.Forms.Button();
            textBox_GoogleSecretKey = new System.Windows.Forms.TextBox();
            lbSheetSecret = new System.Windows.Forms.Label();
            textBox_GoogleClientID = new System.Windows.Forms.TextBox();
            lbSheetID = new System.Windows.Forms.Label();
            googleSheet_textBox = new System.Windows.Forms.TextBox();
            lbGoogleSheetAddress = new System.Windows.Forms.Label();
            pnEzTrans = new System.Windows.Forms.Panel();
            lbEzTransInfo = new System.Windows.Forms.Label();
            pnPapagoWeb = new System.Windows.Forms.Panel();
            lbPapagoWebInfo = new System.Windows.Forms.Label();
            tpText = new System.Windows.Forms.TabPage();
            panel5 = new System.Windows.Forms.Panel();
            panel17 = new System.Windows.Forms.Panel();
            lbPreview = new System.Windows.Forms.Label();
            fontResultLabel = new CustomLabel();
            panel10 = new System.Windows.Forms.Panel();
            defaultColorButton = new System.Windows.Forms.Button();
            lbFontBackground = new System.Windows.Forms.Label();
            lbFontOutlineColor2 = new System.Windows.Forms.Label();
            lbFontOutlineColor1 = new System.Windows.Forms.Label();
            backgroundColorBox = new System.Windows.Forms.PictureBox();
            outlineColor2Box = new System.Windows.Forms.PictureBox();
            outlineColor1Box = new System.Windows.Forms.PictureBox();
            lbFontBasicColor = new System.Windows.Forms.Label();
            textColorBox = new System.Windows.Forms.PictureBox();
            lbFontColor = new System.Windows.Forms.Label();
            panel9 = new System.Windows.Forms.Panel();
            cbShowOCRIndex = new System.Windows.Forms.CheckBox();
            useBackColorCheckBox = new System.Windows.Forms.CheckBox();
            removeSpaceCheckBox = new System.Windows.Forms.CheckBox();
            alignmentCenterCheckBox = new System.Windows.Forms.CheckBox();
            lbTextAdditionalSettings = new System.Windows.Forms.Label();
            panel7 = new System.Windows.Forms.Panel();
            fontSizeUpDown = new System.Windows.Forms.NumericUpDown();
            lbFontSize = new System.Windows.Forms.Label();
            lbFont = new System.Windows.Forms.Label();
            fontButton = new System.Windows.Forms.Button();
            lbFontSetting = new System.Windows.Forms.Label();
            tpExtra = new System.Windows.Forms.TabPage();
            panel11 = new System.Windows.Forms.Panel();
            panel21 = new System.Windows.Forms.Panel();
            btAdvencedOption = new System.Windows.Forms.Button();
            lbAdvencedConfig = new System.Windows.Forms.Label();
            panel25 = new System.Windows.Forms.Panel();
            btSettingUpload = new System.Windows.Forms.Button();
            btSettingBrowser = new System.Windows.Forms.Button();
            lbSearchConfig = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            speedRadioButton5 = new System.Windows.Forms.RadioButton();
            lbSpeed = new System.Windows.Forms.Label();
            lbSpeedInformation = new System.Windows.Forms.Label();
            speedRadioButton4 = new System.Windows.Forms.RadioButton();
            speedRadioButton1 = new System.Windows.Forms.RadioButton();
            speedRadioButton3 = new System.Windows.Forms.RadioButton();
            speedRadioButton2 = new System.Windows.Forms.RadioButton();
            panel13 = new System.Windows.Forms.Panel();
            defaultButton = new System.Windows.Forms.Button();
            saveConfigButton = new System.Windows.Forms.Button();
            openConfigButton = new System.Windows.Forms.Button();
            lbSettingFile = new System.Windows.Forms.Label();
            panel12 = new System.Windows.Forms.Panel();
            topMostcheckBox = new System.Windows.Forms.CheckBox();
            checkUpdateCheckBox = new System.Windows.Forms.CheckBox();
            label15 = new System.Windows.Forms.Label();
            panel14 = new System.Windows.Forms.Panel();
            btAttachCapture = new System.Windows.Forms.Button();
            SetDefaultZoomSizeButton = new System.Windows.Forms.Button();
            imgZoomsizeUpDown = new System.Windows.Forms.NumericUpDown();
            lbImgZoom = new System.Windows.Forms.Label();
            activeWinodeCheckBox = new System.Windows.Forms.CheckBox();
            lbImgCapture = new System.Windows.Forms.Label();
            tpTranslation = new System.Windows.Forms.TabPage();
            panel19 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            cbDeepLLanguageTo = new System.Windows.Forms.ComboBox();
            lbDeepLTo = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            cbDeepLLanguage = new System.Windows.Forms.ComboBox();
            lbDeepL = new System.Windows.Forms.Label();
            lbDeepLFrom = new System.Windows.Forms.Label();
            panel27 = new System.Windows.Forms.Panel();
            cbTTSWaitEnd = new System.Windows.Forms.CheckBox();
            cbUseTTS = new System.Windows.Forms.CheckBox();
            label66 = new System.Windows.Forms.Label();
            panel22 = new System.Windows.Forms.Panel();
            googleResultCodeComboBox = new System.Windows.Forms.ComboBox();
            lbGoogleTo = new System.Windows.Forms.Label();
            label56 = new System.Windows.Forms.Label();
            googleTransComboBox = new System.Windows.Forms.ComboBox();
            lbGoogle = new System.Windows.Forms.Label();
            lbGoogleFrom = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            skinOverRadioButton = new System.Windows.Forms.RadioButton();
            skinLayerRadioButton = new System.Windows.Forms.RadioButton();
            lbTransformType = new System.Windows.Forms.Label();
            skinDarkRadioButton = new System.Windows.Forms.RadioButton();
            panel15 = new System.Windows.Forms.Panel();
            lbPapagoLanguageCodeInformation = new System.Windows.Forms.Label();
            cbNaverResultCode = new System.Windows.Forms.ComboBox();
            lbPaPagoTo = new System.Windows.Forms.Label();
            label43 = new System.Windows.Forms.Label();
            naverTransComboBox = new System.Windows.Forms.ComboBox();
            lbPaPago = new System.Windows.Forms.Label();
            lbPaPagoFrom = new System.Windows.Forms.Label();
            tpETC = new System.Windows.Forms.TabPage();
            panel18 = new System.Windows.Forms.Panel();
            panel16 = new System.Windows.Forms.Panel();
            btnOpenDiscord = new System.Windows.Forms.Button();
            openBlogButton = new System.Windows.Forms.Button();
            btnGitHub = new System.Windows.Forms.Button();
            lbLink = new System.Windows.Forms.Label();
            panel20 = new System.Windows.Forms.Panel();
            about_Button = new System.Windows.Forms.Button();
            error_Information_Button = new System.Windows.Forms.Button();
            help_Button = new System.Windows.Forms.Button();
            lbETC = new System.Windows.Forms.Label();
            panel23 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            transKeyInputLabel = new KeyInputLabel();
            btnHideTransEmpty = new System.Windows.Forms.Button();
            dicKeyInputLabel = new KeyInputLabel();
            btnHideTransDefault = new System.Windows.Forms.Button();
            quickKeyInputLabel = new KeyInputLabel();
            lbHideTranslate = new KeyInputLabel();
            transKeyInputResetButton = new System.Windows.Forms.Button();
            dicKeyInputResetButton = new System.Windows.Forms.Button();
            btnOneTransEmpty = new System.Windows.Forms.Button();
            quickKeyInputResetButton = new System.Windows.Forms.Button();
            btnOneTransDefault = new System.Windows.Forms.Button();
            transKeyInputEmptyButton = new System.Windows.Forms.Button();
            lbOneTrans = new KeyInputLabel();
            dicKeyInputEmptyButton = new System.Windows.Forms.Button();
            quickKeyInputEmptyButton = new System.Windows.Forms.Button();
            snapShotKeyInputEmptyButton = new System.Windows.Forms.Button();
            snapShotInputLabel = new KeyInputLabel();
            snapShotKeyInputResetButton = new System.Windows.Forms.Button();
            lbHotKeyHideTransWindow = new System.Windows.Forms.Label();
            lbHotKeyOnceTranslate = new System.Windows.Forms.Label();
            lbHotKeySnapShot = new System.Windows.Forms.Label();
            lbHotKeyInformation = new System.Windows.Forms.Label();
            lbHotKeyQuickOCR = new System.Windows.Forms.Label();
            lbHotKeyDic = new System.Windows.Forms.Label();
            lbHotKeyDoTrans = new System.Windows.Forms.Label();
            lbHotkey = new System.Windows.Forms.Label();
            tpQuickSetting = new System.Windows.Forms.TabPage();
            panel28 = new System.Windows.Forms.Panel();
            panel31 = new System.Windows.Forms.Panel();
            cbSetBasicDefaultPage = new System.Windows.Forms.CheckBox();
            btQuickJap = new System.Windows.Forms.Button();
            lbQuickSettingInformation = new System.Windows.Forms.Label();
            btQucickEnglish = new System.Windows.Forms.Button();
            lbQuickSetting = new System.Windows.Forms.Label();
            tpDebuging = new System.Windows.Forms.TabPage();
            panel24 = new System.Windows.Forms.Panel();
            panel26 = new System.Windows.Forms.Panel();
            plDebugOn = new System.Windows.Forms.Panel();
            cbShowOverlayWordArea = new System.Windows.Forms.CheckBox();
            cbSetLineTrans = new System.Windows.Forms.CheckBox();
            btClearFormerResult = new System.Windows.Forms.Button();
            cbShowFormerLog = new System.Windows.Forms.CheckBox();
            cbUnlockSpeed = new System.Windows.Forms.CheckBox();
            cbSaveCaptureResult = new System.Windows.Forms.CheckBox();
            cbSaveCapture = new System.Windows.Forms.CheckBox();
            cbShowReplace = new System.Windows.Forms.CheckBox();
            plDebugOff = new System.Windows.Forms.Panel();
            label63 = new System.Windows.Forms.Label();
            btnDebugOn = new System.Windows.Forms.Button();
            lbDebugging = new System.Windows.Forms.Label();
            ContextOption.SuspendLayout();
            optionMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tbMain.SuspendLayout();
            tpBasic.SuspendLayout();
            panel8.SuspendLayout();
            pnAdjustImg.SuspendLayout();
            pnOCR.SuspendLayout();
            WinOCR_panel.SuspendLayout();
            pnEasyOcr.SuspendLayout();
            Tesseract_panel.SuspendLayout();
            pnGoogleOcr.SuspendLayout();
            pnNHocr.SuspendLayout();
            pnTranslate.SuspendLayout();
            pnGoogleBasic.SuspendLayout();
            pnCustomApi.SuspendLayout();
            pnDeepLX.SuspendLayout();
            pnDeepl.SuspendLayout();
            DB_Panel.SuspendLayout();
            Naver_Panel.SuspendLayout();
            Google_Panel.SuspendLayout();
            pnEzTrans.SuspendLayout();
            pnPapagoWeb.SuspendLayout();
            tpText.SuspendLayout();
            panel5.SuspendLayout();
            panel17.SuspendLayout();
            panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)backgroundColorBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor2Box).BeginInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor1Box).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textColorBox).BeginInit();
            panel9.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontSizeUpDown).BeginInit();
            tpExtra.SuspendLayout();
            panel11.SuspendLayout();
            panel21.SuspendLayout();
            panel25.SuspendLayout();
            panel3.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgZoomsizeUpDown).BeginInit();
            tpTranslation.SuspendLayout();
            panel19.SuspendLayout();
            panel4.SuspendLayout();
            panel27.SuspendLayout();
            panel22.SuspendLayout();
            panel1.SuspendLayout();
            panel15.SuspendLayout();
            tpETC.SuspendLayout();
            panel18.SuspendLayout();
            panel16.SuspendLayout();
            panel20.SuspendLayout();
            panel23.SuspendLayout();
            panel2.SuspendLayout();
            tpQuickSetting.SuspendLayout();
            panel28.SuspendLayout();
            panel31.SuspendLayout();
            tpDebuging.SuspendLayout();
            panel24.SuspendLayout();
            panel26.SuspendLayout();
            plDebugOn.SuspendLayout();
            plDebugOff.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = ContextOption;
            notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "MORT";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            // 
            // ContextOption
            // 
            ContextOption.ImageScalingSize = new System.Drawing.Size(24, 24);
            ContextOption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { optionToolStripMenuItem, showTransToolStripMenuItem, rTTToolStripMenuItem, toolStripSeparator7, setTranslateTopMostToolStripMenuItem, setCheckSpellingToolStripMenuItem, setCutPointToolStripMenuItem, toolStripSeparator5, transToolStripMenuItem, toolStripSeparator6, settingToolStripMenuItem, toolStripSeparator8, aboutToolStripMenuItem, checkUpdateToolStripMenuItem, ExitToolStripMenuItem });
            ContextOption.Name = "contextMenuStrip1";
            ContextOption.Size = new System.Drawing.Size(173, 314);
            // 
            // optionToolStripMenuItem
            // 
            optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            optionToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            optionToolStripMenuItem.Text = "옵션";
            optionToolStripMenuItem.Click += ContextOption_Click;
            // 
            // showTransToolStripMenuItem
            // 
            showTransToolStripMenuItem.Name = "showTransToolStripMenuItem";
            showTransToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            showTransToolStripMenuItem.Text = "번역창";
            showTransToolStripMenuItem.Click += showTransToolStripMenuItem_Click;
            // 
            // rTTToolStripMenuItem
            // 
            rTTToolStripMenuItem.Name = "rTTToolStripMenuItem";
            rTTToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            rTTToolStripMenuItem.Text = "리모컨";
            rTTToolStripMenuItem.Click += rTTToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(207, 6);
            // 
            // setTranslateTopMostToolStripMenuItem
            // 
            setTranslateTopMostToolStripMenuItem.Checked = true;
            setTranslateTopMostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            setTranslateTopMostToolStripMenuItem.Name = "setTranslateTopMostToolStripMenuItem";
            setTranslateTopMostToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            setTranslateTopMostToolStripMenuItem.Text = "번역창 고정";
            setTranslateTopMostToolStripMenuItem.Click += setTranslateTopMostToolStripMenuItem_Click;
            // 
            // setCheckSpellingToolStripMenuItem
            // 
            setCheckSpellingToolStripMenuItem.Checked = true;
            setCheckSpellingToolStripMenuItem.CheckOnClick = true;
            setCheckSpellingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            setCheckSpellingToolStripMenuItem.Name = "setCheckSpellingToolStripMenuItem";
            setCheckSpellingToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            setCheckSpellingToolStripMenuItem.Text = "교정사전 사용";
            setCheckSpellingToolStripMenuItem.Click += setCheckSpellingToolStripMenuItem_Click;
            // 
            // setCutPointToolStripMenuItem
            // 
            setCutPointToolStripMenuItem.Name = "setCutPointToolStripMenuItem";
            setCutPointToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            setCutPointToolStripMenuItem.Text = "영역 설정";
            setCutPointToolStripMenuItem.Click += setCutPointToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(207, 6);
            // 
            // transToolStripMenuItem
            // 
            transToolStripMenuItem.Name = "transToolStripMenuItem";
            transToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            transToolStripMenuItem.Text = "번역 시작";
            transToolStripMenuItem.Click += ContextTranslate_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(207, 6);
            // 
            // settingToolStripMenuItem
            // 
            settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { settingSaveToolStripMenuItem2, settingLoadToolStripMenuItem2, settingDefaultToolStripMenuItem });
            settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            settingToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            settingToolStripMenuItem.Text = "설정";
            // 
            // settingSaveToolStripMenuItem2
            // 
            settingSaveToolStripMenuItem2.Name = "settingSaveToolStripMenuItem2";
            settingSaveToolStripMenuItem2.Size = new System.Drawing.Size(152, 26);
            settingSaveToolStripMenuItem2.Text = "저장";
            settingSaveToolStripMenuItem2.Click += settingSaveToolStripMenuItem2_Click;
            // 
            // settingLoadToolStripMenuItem2
            // 
            settingLoadToolStripMenuItem2.Name = "settingLoadToolStripMenuItem2";
            settingLoadToolStripMenuItem2.Size = new System.Drawing.Size(152, 26);
            settingLoadToolStripMenuItem2.Text = "불러오기";
            settingLoadToolStripMenuItem2.Click += settingLoadToolStripMenuItem2_Click;
            // 
            // settingDefaultToolStripMenuItem
            // 
            settingDefaultToolStripMenuItem.Name = "settingDefaultToolStripMenuItem";
            settingDefaultToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            settingDefaultToolStripMenuItem.Text = "초기화";
            settingDefaultToolStripMenuItem.Click += settingDefaultToolStripMenuItem_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(207, 6);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // checkUpdateToolStripMenuItem
            // 
            checkUpdateToolStripMenuItem.Name = "checkUpdateToolStripMenuItem";
            checkUpdateToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            checkUpdateToolStripMenuItem.Text = "업데이트";
            checkUpdateToolStripMenuItem.Click += checkUpdateToolStripMenuItem_Click;
            // 
            // ExitToolStripMenuItem
            // 
            ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            ExitToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            ExitToolStripMenuItem.Text = "종료";
            ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // optionMenuStrip
            // 
            optionMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            optionMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 설정저장ToolStripMenuItem, 설정불러오기ToolStripMenuItem });
            optionMenuStrip.Name = "optionMenuStrip";
            optionMenuStrip.Size = new System.Drawing.Size(173, 52);
            // 
            // 설정저장ToolStripMenuItem
            // 
            설정저장ToolStripMenuItem.Name = "설정저장ToolStripMenuItem";
            설정저장ToolStripMenuItem.Size = new System.Drawing.Size(172, 24);
            설정저장ToolStripMenuItem.Text = "설정 저장";
            // 
            // 설정불러오기ToolStripMenuItem
            // 
            설정불러오기ToolStripMenuItem.Name = "설정불러오기ToolStripMenuItem";
            설정불러오기ToolStripMenuItem.Size = new System.Drawing.Size(172, 24);
            설정불러오기ToolStripMenuItem.Text = "설정 불러오기";
            // 
            // acceptButton
            // 
            acceptButton.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            acceptButton.FlatAppearance.BorderSize = 0;
            acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            acceptButton.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            acceptButton.ForeColor = System.Drawing.Color.White;
            acceptButton.Location = new System.Drawing.Point(498, 736);
            acceptButton.Margin = new System.Windows.Forms.Padding(0);
            acceptButton.Name = "acceptButton";
            acceptButton.Size = new System.Drawing.Size(244, 69);
            acceptButton.TabIndex = 44;
            acceptButton.Text = "적 용";
            acceptButton.UseVisualStyleBackColor = false;
            acceptButton.Click += acceptButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            pictureBox1.Location = new System.Drawing.Point(0, 741);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(76, 165);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 45;
            pictureBox1.TabStop = false;
            // 
            // donationButton
            // 
            donationButton.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            donationButton.FlatAppearance.BorderSize = 0;
            donationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            donationButton.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            donationButton.ForeColor = System.Drawing.Color.White;
            donationButton.Location = new System.Drawing.Point(125, 736);
            donationButton.Margin = new System.Windows.Forms.Padding(0);
            donationButton.Name = "donationButton";
            donationButton.Size = new System.Drawing.Size(244, 69);
            donationButton.TabIndex = 46;
            donationButton.Text = "후원하기";
            donationButton.UseVisualStyleBackColor = false;
            donationButton.Click += donationButton_Click;
            // 
            // toolTip_OCR
            // 
            toolTip_OCR.AutoPopDelay = 500000000;
            toolTip_OCR.InitialDelay = 300;
            toolTip_OCR.ReshowDelay = 100;
            // 
            // tbMain
            // 
            tbMain.Alignment = System.Windows.Forms.TabAlignment.Left;
            tbMain.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            tbMain.Controls.Add(tpBasic);
            tbMain.Controls.Add(tpText);
            tbMain.Controls.Add(tpExtra);
            tbMain.Controls.Add(tpTranslation);
            tbMain.Controls.Add(tpETC);
            tbMain.Controls.Add(tpQuickSetting);
            tbMain.Controls.Add(tpDebuging);
            tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            tbMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tbMain.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tbMain.ItemSize = new System.Drawing.Size(44, 76);
            tbMain.Location = new System.Drawing.Point(0, 0);
            tbMain.Margin = new System.Windows.Forms.Padding(0);
            tbMain.Multiline = true;
            tbMain.Name = "tbMain";
            tbMain.Padding = new System.Drawing.Point(0, 0);
            tbMain.SelectedIndex = 0;
            tbMain.Size = new System.Drawing.Size(780, 741);
            tbMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tbMain.TabIndex = 43;
            tbMain.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tpBasic
            // 
            tpBasic.Controls.Add(panel8);
            tpBasic.ForeColor = System.Drawing.SystemColors.ControlText;
            tpBasic.Location = new System.Drawing.Point(80, 4);
            tpBasic.Margin = new System.Windows.Forms.Padding(0);
            tpBasic.Name = "tpBasic";
            tpBasic.Size = new System.Drawing.Size(696, 733);
            tpBasic.TabIndex = 0;
            tpBasic.Text = "기본설정";
            tpBasic.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            panel8.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel8.Controls.Add(pnAdjustImg);
            panel8.Controls.Add(pnOCR);
            panel8.Controls.Add(pnTranslate);
            panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            panel8.Location = new System.Drawing.Point(0, 0);
            panel8.Margin = new System.Windows.Forms.Padding(0);
            panel8.Name = "panel8";
            panel8.Size = new System.Drawing.Size(696, 733);
            panel8.TabIndex = 39;
            // 
            // pnAdjustImg
            // 
            pnAdjustImg.Controls.Add(btImgResult);
            pnAdjustImg.Controls.Add(cbThreshold);
            pnAdjustImg.Controls.Add(tbThreshold);
            pnAdjustImg.Controls.Add(groupCombo);
            pnAdjustImg.Controls.Add(groupLabel);
            pnAdjustImg.Controls.Add(lbImgGroupCount);
            pnAdjustImg.Controls.Add(lbImgGroup);
            pnAdjustImg.Controls.Add(label1);
            pnAdjustImg.Controls.Add(v2TextBox);
            pnAdjustImg.Controls.Add(lbAdjustImg);
            pnAdjustImg.Controls.Add(checkRGB);
            pnAdjustImg.Controls.Add(s2TextBox);
            pnAdjustImg.Controls.Add(label3);
            pnAdjustImg.Controls.Add(label16);
            pnAdjustImg.Controls.Add(rTextBox);
            pnAdjustImg.Controls.Add(checkErode);
            pnAdjustImg.Controls.Add(label4);
            pnAdjustImg.Controls.Add(checkHSV);
            pnAdjustImg.Controls.Add(gTextBox);
            pnAdjustImg.Controls.Add(label5);
            pnAdjustImg.Controls.Add(v1TextBox);
            pnAdjustImg.Controls.Add(bTextBox);
            pnAdjustImg.Controls.Add(label7);
            pnAdjustImg.Controls.Add(label8);
            pnAdjustImg.Controls.Add(s1TextBox);
            pnAdjustImg.Location = new System.Drawing.Point(4, 484);
            pnAdjustImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnAdjustImg.Name = "pnAdjustImg";
            pnAdjustImg.Size = new System.Drawing.Size(666, 229);
            pnAdjustImg.TabIndex = 37;
            pnAdjustImg.Paint += panealBorder_Paint;
            // 
            // btImgResult
            // 
            btImgResult.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btImgResult.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btImgResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btImgResult.ForeColor = System.Drawing.Color.White;
            btImgResult.Location = new System.Drawing.Point(305, 136);
            btImgResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btImgResult.Name = "btImgResult";
            btImgResult.Size = new System.Drawing.Size(245, 31);
            btImgResult.TabIndex = 51;
            btImgResult.Text = "보정 결과 확인하기";
            btImgResult.UseVisualStyleBackColor = false;
            btImgResult.Click += OnClickShowImgResult;
            // 
            // cbThreshold
            // 
            cbThreshold.AutoSize = true;
            cbThreshold.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbThreshold.ForeColor = System.Drawing.Color.White;
            cbThreshold.Location = new System.Drawing.Point(15, 102);
            cbThreshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbThreshold.Name = "cbThreshold";
            cbThreshold.Size = new System.Drawing.Size(157, 27);
            cbThreshold.TabIndex = 49;
            cbThreshold.Text = "임계값으로 추출";
            cbThreshold.UseVisualStyleBackColor = true;
            cbThreshold.CheckedChanged += cbThreshold_CheckedChanged;
            // 
            // tbThreshold
            // 
            tbThreshold.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            tbThreshold.Location = new System.Drawing.Point(204, 101);
            tbThreshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tbThreshold.Name = "tbThreshold";
            tbThreshold.Size = new System.Drawing.Size(58, 29);
            tbThreshold.TabIndex = 50;
            tbThreshold.Text = "0";
            tbThreshold.KeyPress += textBox_KeyPress;
            tbThreshold.Leave += thresholdTextLeave;
            // 
            // groupCombo
            // 
            groupCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            groupCombo.FormattingEnabled = true;
            groupCombo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            groupCombo.Location = new System.Drawing.Point(271, 180);
            groupCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupCombo.Name = "groupCombo";
            groupCombo.Size = new System.Drawing.Size(69, 28);
            groupCombo.TabIndex = 47;
            groupCombo.SelectedIndexChanged += groupCombo_SelectedIndexChanged;
            // 
            // groupLabel
            // 
            groupLabel.AutoSize = true;
            groupLabel.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            groupLabel.ForeColor = System.Drawing.Color.White;
            groupLabel.Location = new System.Drawing.Point(438, 180);
            groupLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            groupLabel.Name = "groupLabel";
            groupLabel.Size = new System.Drawing.Size(20, 23);
            groupLabel.TabIndex = 46;
            groupLabel.Text = "0";
            // 
            // lbImgGroupCount
            // 
            lbImgGroupCount.AutoSize = true;
            lbImgGroupCount.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbImgGroupCount.ForeColor = System.Drawing.Color.White;
            lbImgGroupCount.Location = new System.Drawing.Point(349, 180);
            lbImgGroupCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbImgGroupCount.Name = "lbImgGroupCount";
            lbImgGroupCount.Size = new System.Drawing.Size(83, 23);
            lbImgGroupCount.TabIndex = 45;
            lbImgGroupCount.Text = "그룹 수 : ";
            // 
            // lbImgGroup
            // 
            lbImgGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lbImgGroup.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbImgGroup.ForeColor = System.Drawing.Color.White;
            lbImgGroup.Location = new System.Drawing.Point(114, 182);
            lbImgGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbImgGroup.Name = "lbImgGroup";
            lbImgGroup.Size = new System.Drawing.Size(149, 29);
            lbImgGroup.TabIndex = 43;
            lbImgGroup.Text = "범위 그룹";
            lbImgGroup.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(460, 66);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(22, 23);
            label1.TabIndex = 18;
            label1.Text = "~";
            // 
            // v2TextBox
            // 
            v2TextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            v2TextBox.Location = new System.Drawing.Point(485, 64);
            v2TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            v2TextBox.Name = "v2TextBox";
            v2TextBox.Size = new System.Drawing.Size(58, 29);
            v2TextBox.TabIndex = 16;
            v2TextBox.Text = "0";
            v2TextBox.KeyPress += textBox_KeyPress;
            v2TextBox.Leave += hsvTextLeave;
            // 
            // lbAdjustImg
            // 
            lbAdjustImg.AutoSize = true;
            lbAdjustImg.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbAdjustImg.ForeColor = System.Drawing.Color.White;
            lbAdjustImg.Location = new System.Drawing.Point(5, 4);
            lbAdjustImg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbAdjustImg.Name = "lbAdjustImg";
            lbAdjustImg.Size = new System.Drawing.Size(114, 25);
            lbAdjustImg.TabIndex = 8;
            lbAdjustImg.Text = "이미지 보정";
            // 
            // checkRGB
            // 
            checkRGB.AutoSize = true;
            checkRGB.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkRGB.ForeColor = System.Drawing.Color.White;
            checkRGB.Location = new System.Drawing.Point(15, 31);
            checkRGB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkRGB.Name = "checkRGB";
            checkRGB.Size = new System.Drawing.Size(123, 27);
            checkRGB.TabIndex = 8;
            checkRGB.Text = "RGB로 추출";
            checkRGB.UseVisualStyleBackColor = true;
            checkRGB.MouseDown += checkRGB_MouseDown;
            // 
            // s2TextBox
            // 
            s2TextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            s2TextBox.Location = new System.Drawing.Point(299, 64);
            s2TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            s2TextBox.Name = "s2TextBox";
            s2TextBox.Size = new System.Drawing.Size(58, 29);
            s2TextBox.TabIndex = 14;
            s2TextBox.Text = "0";
            s2TextBox.KeyPress += textBox_KeyPress;
            s2TextBox.Leave += hsvTextLeave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(175, 31);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(21, 23);
            label3.TabIndex = 2;
            label3.Text = "R";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label16.ForeColor = System.Drawing.Color.White;
            label16.Location = new System.Drawing.Point(271, 66);
            label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(22, 23);
            label16.TabIndex = 16;
            label16.Text = "~";
            // 
            // rTextBox
            // 
            rTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            rTextBox.Location = new System.Drawing.Point(204, 28);
            rTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            rTextBox.Name = "rTextBox";
            rTextBox.Size = new System.Drawing.Size(58, 29);
            rTextBox.TabIndex = 9;
            rTextBox.Text = "0";
            rTextBox.KeyPress += textBox_KeyPress;
            rTextBox.Leave += rgbTextLeave;
            // 
            // checkErode
            // 
            checkErode.AutoSize = true;
            checkErode.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkErode.ForeColor = System.Drawing.Color.White;
            checkErode.Location = new System.Drawing.Point(15, 141);
            checkErode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkErode.Name = "checkErode";
            checkErode.Size = new System.Drawing.Size(289, 27);
            checkErode.TabIndex = 17;
            checkErode.Text = "침식 사용 (굵은 글씨체일때 사용)";
            checkErode.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label4.ForeColor = System.Drawing.Color.White;
            label4.Location = new System.Drawing.Point(270, 31);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(22, 23);
            label4.TabIndex = 4;
            label4.Text = "G";
            // 
            // checkHSV
            // 
            checkHSV.AutoSize = true;
            checkHSV.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkHSV.ForeColor = System.Drawing.Color.White;
            checkHSV.Location = new System.Drawing.Point(15, 65);
            checkHSV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkHSV.Name = "checkHSV";
            checkHSV.Size = new System.Drawing.Size(123, 27);
            checkHSV.TabIndex = 12;
            checkHSV.Text = "HSV로 추출";
            checkHSV.UseVisualStyleBackColor = true;
            checkHSV.MouseDown += checkHSV_MouseDown;
            // 
            // gTextBox
            // 
            gTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            gTextBox.Location = new System.Drawing.Point(299, 28);
            gTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            gTextBox.Name = "gTextBox";
            gTextBox.Size = new System.Drawing.Size(58, 29);
            gTextBox.TabIndex = 10;
            gTextBox.Text = "0";
            gTextBox.KeyPress += textBox_KeyPress;
            gTextBox.Leave += rgbTextLeave;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(365, 31);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(21, 23);
            label5.TabIndex = 6;
            label5.Text = "B";
            // 
            // v1TextBox
            // 
            v1TextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            v1TextBox.Location = new System.Drawing.Point(394, 64);
            v1TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            v1TextBox.Name = "v1TextBox";
            v1TextBox.Size = new System.Drawing.Size(58, 29);
            v1TextBox.TabIndex = 15;
            v1TextBox.Text = "0";
            v1TextBox.KeyPress += textBox_KeyPress;
            v1TextBox.Leave += hsvTextLeave;
            // 
            // bTextBox
            // 
            bTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            bTextBox.Location = new System.Drawing.Point(394, 28);
            bTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            bTextBox.Name = "bTextBox";
            bTextBox.Size = new System.Drawing.Size(58, 29);
            bTextBox.TabIndex = 11;
            bTextBox.Text = "0";
            bTextBox.KeyPress += textBox_KeyPress;
            bTextBox.Leave += rgbTextLeave;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label7.ForeColor = System.Drawing.Color.White;
            label7.Location = new System.Drawing.Point(365, 65);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(21, 23);
            label7.TabIndex = 11;
            label7.Text = "V";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label8.ForeColor = System.Drawing.Color.White;
            label8.Location = new System.Drawing.Point(175, 65);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(20, 23);
            label8.TabIndex = 9;
            label8.Text = "S";
            // 
            // s1TextBox
            // 
            s1TextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            s1TextBox.Location = new System.Drawing.Point(204, 64);
            s1TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            s1TextBox.Name = "s1TextBox";
            s1TextBox.Size = new System.Drawing.Size(58, 29);
            s1TextBox.TabIndex = 13;
            s1TextBox.Text = "0";
            s1TextBox.KeyPress += textBox_KeyPress;
            s1TextBox.Leave += hsvTextLeave;
            // 
            // pnOCR
            // 
            pnOCR.Controls.Add(btOcrHelp);
            pnOCR.Controls.Add(label48);
            pnOCR.Controls.Add(OCR_Type_comboBox);
            pnOCR.Controls.Add(isClipBoardcheckBox1);
            pnOCR.Controls.Add(saveOCRCheckBox);
            pnOCR.Controls.Add(ocrLabel);
            pnOCR.Controls.Add(showOcrCheckBox);
            pnOCR.Controls.Add(WinOCR_panel);
            pnOCR.Controls.Add(pnEasyOcr);
            pnOCR.Controls.Add(Tesseract_panel);
            pnOCR.Controls.Add(pnGoogleOcr);
            pnOCR.Controls.Add(pnNHocr);
            pnOCR.Location = new System.Drawing.Point(4, 4);
            pnOCR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnOCR.Name = "pnOCR";
            pnOCR.Size = new System.Drawing.Size(666, 194);
            pnOCR.TabIndex = 37;
            pnOCR.Paint += panealBorder_Paint;
            // 
            // btOcrHelp
            // 
            btOcrHelp.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            btOcrHelp.FlatAppearance.BorderSize = 0;
            btOcrHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btOcrHelp.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btOcrHelp.ForeColor = System.Drawing.Color.White;
            btOcrHelp.Location = new System.Drawing.Point(355, 39);
            btOcrHelp.Margin = new System.Windows.Forms.Padding(0);
            btOcrHelp.Name = "btOcrHelp";
            btOcrHelp.Size = new System.Drawing.Size(35, 31);
            btOcrHelp.TabIndex = 59;
            btOcrHelp.Text = "?";
            btOcrHelp.UseVisualStyleBackColor = false;
            btOcrHelp.Click += OnClick_btOcrHelp;
            // 
            // label48
            // 
            label48.AutoSize = true;
            label48.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label48.ForeColor = System.Drawing.Color.White;
            label48.Location = new System.Drawing.Point(20, 42);
            label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label48.Name = "label48";
            label48.Size = new System.Drawing.Size(51, 23);
            label48.TabIndex = 50;
            label48.Text = "OCR ";
            // 
            // OCR_Type_comboBox
            // 
            OCR_Type_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            OCR_Type_comboBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            OCR_Type_comboBox.FormattingEnabled = true;
            OCR_Type_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            OCR_Type_comboBox.Items.AddRange(new object[] { "OCR Tesseract", "OCR Win OCR", "OCR NHocr", "OCR Google", "OCR Easy OCR" });
            OCR_Type_comboBox.Location = new System.Drawing.Point(131, 39);
            OCR_Type_comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            OCR_Type_comboBox.Name = "OCR_Type_comboBox";
            OCR_Type_comboBox.Size = new System.Drawing.Size(205, 29);
            OCR_Type_comboBox.TabIndex = 51;
            OCR_Type_comboBox.SelectedIndexChanged += OCR_Type_comboBox_SelectedIndexChanged;
            // 
            // isClipBoardcheckBox1
            // 
            isClipBoardcheckBox1.AutoSize = true;
            isClipBoardcheckBox1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            isClipBoardcheckBox1.ForeColor = System.Drawing.Color.White;
            isClipBoardcheckBox1.Location = new System.Drawing.Point(459, 154);
            isClipBoardcheckBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            isClipBoardcheckBox1.Name = "isClipBoardcheckBox1";
            isClipBoardcheckBox1.Size = new System.Drawing.Size(157, 27);
            isClipBoardcheckBox1.TabIndex = 26;
            isClipBoardcheckBox1.Text = "클립보드에 저장";
            isClipBoardcheckBox1.UseVisualStyleBackColor = true;
            // 
            // saveOCRCheckBox
            // 
            saveOCRCheckBox.AutoSize = true;
            saveOCRCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            saveOCRCheckBox.ForeColor = System.Drawing.Color.White;
            saveOCRCheckBox.Location = new System.Drawing.Point(241, 154);
            saveOCRCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            saveOCRCheckBox.Name = "saveOCRCheckBox";
            saveOCRCheckBox.Size = new System.Drawing.Size(147, 27);
            saveOCRCheckBox.TabIndex = 24;
            saveOCRCheckBox.Text = "OCR 결과 저장";
            saveOCRCheckBox.UseVisualStyleBackColor = true;
            // 
            // ocrLabel
            // 
            ocrLabel.AutoSize = true;
            ocrLabel.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ocrLabel.ForeColor = System.Drawing.Color.White;
            ocrLabel.Location = new System.Drawing.Point(5, 4);
            ocrLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ocrLabel.Name = "ocrLabel";
            ocrLabel.Size = new System.Drawing.Size(51, 25);
            ocrLabel.TabIndex = 8;
            ocrLabel.Text = "OCR";
            // 
            // showOcrCheckBox
            // 
            showOcrCheckBox.AutoSize = true;
            showOcrCheckBox.Checked = true;
            showOcrCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            showOcrCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            showOcrCheckBox.ForeColor = System.Drawing.Color.White;
            showOcrCheckBox.Location = new System.Drawing.Point(34, 154);
            showOcrCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            showOcrCheckBox.Name = "showOcrCheckBox";
            showOcrCheckBox.Size = new System.Drawing.Size(147, 27);
            showOcrCheckBox.TabIndex = 2;
            showOcrCheckBox.Text = "OCR 결과 출력";
            showOcrCheckBox.UseVisualStyleBackColor = true;
            // 
            // WinOCR_panel
            // 
            WinOCR_panel.Controls.Add(btnAddWinOcrLanguage);
            WinOCR_panel.Controls.Add(WinOCR_Language_comboBox);
            WinOCR_panel.Controls.Add(lbWinOCRLanguage);
            WinOCR_panel.Location = new System.Drawing.Point(10, 68);
            WinOCR_panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            WinOCR_panel.Name = "WinOCR_panel";
            WinOCR_panel.Size = new System.Drawing.Size(589, 79);
            WinOCR_panel.TabIndex = 54;
            // 
            // btnAddWinOcrLanguage
            // 
            btnAddWinOcrLanguage.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnAddWinOcrLanguage.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnAddWinOcrLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddWinOcrLanguage.ForeColor = System.Drawing.Color.White;
            btnAddWinOcrLanguage.Location = new System.Drawing.Point(345, 10);
            btnAddWinOcrLanguage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnAddWinOcrLanguage.Name = "btnAddWinOcrLanguage";
            btnAddWinOcrLanguage.Size = new System.Drawing.Size(240, 31);
            btnAddWinOcrLanguage.TabIndex = 61;
            btnAddWinOcrLanguage.Text = "언어 추가";
            btnAddWinOcrLanguage.UseVisualStyleBackColor = false;
            btnAddWinOcrLanguage.Click += btnAddWinOcrLanguage_Click;
            // 
            // WinOCR_Language_comboBox
            // 
            WinOCR_Language_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            WinOCR_Language_comboBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            WinOCR_Language_comboBox.FormattingEnabled = true;
            WinOCR_Language_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            WinOCR_Language_comboBox.Items.AddRange(new object[] { "초기화 실패" });
            WinOCR_Language_comboBox.Location = new System.Drawing.Point(121, 8);
            WinOCR_Language_comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            WinOCR_Language_comboBox.Name = "WinOCR_Language_comboBox";
            WinOCR_Language_comboBox.Size = new System.Drawing.Size(205, 29);
            WinOCR_Language_comboBox.TabIndex = 52;
            WinOCR_Language_comboBox.SelectionChangeCommitted += WinOCR_Language_comboBox_SelectionChangeCommitted;
            // 
            // lbWinOCRLanguage
            // 
            lbWinOCRLanguage.AutoSize = true;
            lbWinOCRLanguage.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbWinOCRLanguage.ForeColor = System.Drawing.Color.White;
            lbWinOCRLanguage.Location = new System.Drawing.Point(10, 11);
            lbWinOCRLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbWinOCRLanguage.Name = "lbWinOCRLanguage";
            lbWinOCRLanguage.Size = new System.Drawing.Size(50, 23);
            lbWinOCRLanguage.TabIndex = 50;
            lbWinOCRLanguage.Text = "언어 ";
            // 
            // pnEasyOcr
            // 
            pnEasyOcr.Controls.Add(btnInstallEasyOcr);
            pnEasyOcr.Controls.Add(cbEasyOcrCode);
            pnEasyOcr.Controls.Add(lbEasyOcrLanguage);
            pnEasyOcr.Location = new System.Drawing.Point(10, 68);
            pnEasyOcr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnEasyOcr.Name = "pnEasyOcr";
            pnEasyOcr.Size = new System.Drawing.Size(589, 79);
            pnEasyOcr.TabIndex = 61;
            // 
            // btnInstallEasyOcr
            // 
            btnInstallEasyOcr.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnInstallEasyOcr.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnInstallEasyOcr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnInstallEasyOcr.ForeColor = System.Drawing.Color.White;
            btnInstallEasyOcr.Location = new System.Drawing.Point(345, 10);
            btnInstallEasyOcr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnInstallEasyOcr.Name = "btnInstallEasyOcr";
            btnInstallEasyOcr.Size = new System.Drawing.Size(240, 31);
            btnInstallEasyOcr.TabIndex = 60;
            btnInstallEasyOcr.Text = "Easy OCR 설치";
            btnInstallEasyOcr.UseVisualStyleBackColor = false;
            btnInstallEasyOcr.Click += btnInstallEasyOcr_Click;
            // 
            // cbEasyOcrCode
            // 
            cbEasyOcrCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbEasyOcrCode.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbEasyOcrCode.FormattingEnabled = true;
            cbEasyOcrCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbEasyOcrCode.Items.AddRange(new object[] { "자동" });
            cbEasyOcrCode.Location = new System.Drawing.Point(121, 11);
            cbEasyOcrCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbEasyOcrCode.Name = "cbEasyOcrCode";
            cbEasyOcrCode.Size = new System.Drawing.Size(205, 29);
            cbEasyOcrCode.TabIndex = 55;
            cbEasyOcrCode.SelectionChangeCommitted += cbEasyOcrOcde_SelectionChangeCommitted;
            // 
            // lbEasyOcrLanguage
            // 
            lbEasyOcrLanguage.AutoSize = true;
            lbEasyOcrLanguage.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbEasyOcrLanguage.ForeColor = System.Drawing.Color.White;
            lbEasyOcrLanguage.Location = new System.Drawing.Point(10, 15);
            lbEasyOcrLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbEasyOcrLanguage.Name = "lbEasyOcrLanguage";
            lbEasyOcrLanguage.Size = new System.Drawing.Size(50, 23);
            lbEasyOcrLanguage.TabIndex = 54;
            lbEasyOcrLanguage.Text = "언어 ";
            // 
            // Tesseract_panel
            // 
            Tesseract_panel.Controls.Add(cbFastTess);
            Tesseract_panel.Controls.Add(tesseractLanguageComboBox);
            Tesseract_panel.Controls.Add(lbTesseractLanguage);
            Tesseract_panel.Controls.Add(label18);
            Tesseract_panel.Controls.Add(tessDataTextBox);
            Tesseract_panel.Location = new System.Drawing.Point(10, 68);
            Tesseract_panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Tesseract_panel.Name = "Tesseract_panel";
            Tesseract_panel.Size = new System.Drawing.Size(589, 79);
            Tesseract_panel.TabIndex = 53;
            // 
            // cbFastTess
            // 
            cbFastTess.AutoSize = true;
            cbFastTess.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbFastTess.ForeColor = System.Drawing.Color.White;
            cbFastTess.Location = new System.Drawing.Point(24, 52);
            cbFastTess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbFastTess.Name = "cbFastTess";
            cbFastTess.Size = new System.Drawing.Size(431, 27);
            cbFastTess.TabIndex = 55;
            cbFastTess.Text = "고속 모드 (빠르나 정확도가 떨어짐, Tesseract 전용)";
            cbFastTess.UseVisualStyleBackColor = true;
            // 
            // tesseractLanguageComboBox
            // 
            tesseractLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            tesseractLanguageComboBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            tesseractLanguageComboBox.FormattingEnabled = true;
            tesseractLanguageComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            tesseractLanguageComboBox.Items.AddRange(new object[] { "en", "ja", "Language ETC" });
            tesseractLanguageComboBox.Location = new System.Drawing.Point(434, 8);
            tesseractLanguageComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tesseractLanguageComboBox.Name = "tesseractLanguageComboBox";
            tesseractLanguageComboBox.Size = new System.Drawing.Size(93, 29);
            tesseractLanguageComboBox.TabIndex = 52;
            tesseractLanguageComboBox.SelectionChangeCommitted += tesseractLanguageComboBox_SelectionChangeCommitted;
            // 
            // lbTesseractLanguage
            // 
            lbTesseractLanguage.AutoSize = true;
            lbTesseractLanguage.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbTesseractLanguage.ForeColor = System.Drawing.Color.White;
            lbTesseractLanguage.Location = new System.Drawing.Point(341, 12);
            lbTesseractLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbTesseractLanguage.Name = "lbTesseractLanguage";
            lbTesseractLanguage.Size = new System.Drawing.Size(84, 23);
            lbTesseractLanguage.TabIndex = 51;
            lbTesseractLanguage.Text = "추출 언어";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label18.ForeColor = System.Drawing.Color.White;
            label18.Location = new System.Drawing.Point(12, 11);
            label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(81, 23);
            label18.TabIndex = 50;
            label18.Text = "Tessdata";
            // 
            // tessDataTextBox
            // 
            tessDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            tessDataTextBox.Location = new System.Drawing.Point(121, 9);
            tessDataTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tessDataTextBox.Name = "tessDataTextBox";
            tessDataTextBox.Size = new System.Drawing.Size(175, 26);
            tessDataTextBox.TabIndex = 49;
            tessDataTextBox.Text = "eng";
            // 
            // pnGoogleOcr
            // 
            pnGoogleOcr.Controls.Add(cbGoogleOcrLanguge);
            pnGoogleOcr.Controls.Add(lbGoogleOCRLanguage);
            pnGoogleOcr.Controls.Add(lbGoogleOcrStatus);
            pnGoogleOcr.Controls.Add(btnSettingGoogleOCR);
            pnGoogleOcr.Location = new System.Drawing.Point(10, 68);
            pnGoogleOcr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnGoogleOcr.Name = "pnGoogleOcr";
            pnGoogleOcr.Size = new System.Drawing.Size(589, 79);
            pnGoogleOcr.TabIndex = 60;
            // 
            // cbGoogleOcrLanguge
            // 
            cbGoogleOcrLanguge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbGoogleOcrLanguge.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbGoogleOcrLanguge.FormattingEnabled = true;
            cbGoogleOcrLanguge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbGoogleOcrLanguge.Items.AddRange(new object[] { "자동" });
            cbGoogleOcrLanguge.Location = new System.Drawing.Point(121, 11);
            cbGoogleOcrLanguge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbGoogleOcrLanguge.Name = "cbGoogleOcrLanguge";
            cbGoogleOcrLanguge.Size = new System.Drawing.Size(205, 29);
            cbGoogleOcrLanguge.TabIndex = 55;
            cbGoogleOcrLanguge.SelectedIndexChanged += cbGoogleOcrLanguge_SelectedIndexChanged;
            // 
            // lbGoogleOCRLanguage
            // 
            lbGoogleOCRLanguage.AutoSize = true;
            lbGoogleOCRLanguage.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbGoogleOCRLanguage.ForeColor = System.Drawing.Color.White;
            lbGoogleOCRLanguage.Location = new System.Drawing.Point(10, 15);
            lbGoogleOCRLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbGoogleOCRLanguage.Name = "lbGoogleOCRLanguage";
            lbGoogleOCRLanguage.Size = new System.Drawing.Size(50, 23);
            lbGoogleOCRLanguage.TabIndex = 54;
            lbGoogleOCRLanguage.Text = "언어 ";
            // 
            // lbGoogleOcrStatus
            // 
            lbGoogleOcrStatus.AutoSize = true;
            lbGoogleOcrStatus.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbGoogleOcrStatus.ForeColor = System.Drawing.Color.White;
            lbGoogleOcrStatus.Location = new System.Drawing.Point(118, 49);
            lbGoogleOcrStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbGoogleOcrStatus.Name = "lbGoogleOcrStatus";
            lbGoogleOcrStatus.Size = new System.Drawing.Size(393, 23);
            lbGoogleOcrStatus.TabIndex = 53;
            lbGoogleOcrStatus.Text = "스냅샷 / 한 번만 번역하기에서만 사용 가능합니다";
            // 
            // btnSettingGoogleOCR
            // 
            btnSettingGoogleOCR.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnSettingGoogleOCR.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnSettingGoogleOCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSettingGoogleOCR.ForeColor = System.Drawing.Color.White;
            btnSettingGoogleOCR.Location = new System.Drawing.Point(344, 10);
            btnSettingGoogleOCR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnSettingGoogleOCR.Name = "btnSettingGoogleOCR";
            btnSettingGoogleOCR.Size = new System.Drawing.Size(241, 31);
            btnSettingGoogleOCR.TabIndex = 52;
            btnSettingGoogleOCR.Text = "API 설정";
            btnSettingGoogleOCR.UseVisualStyleBackColor = false;
            btnSettingGoogleOCR.Click += OnClick_btGoogleOcrSetting;
            // 
            // pnNHocr
            // 
            pnNHocr.Controls.Add(lbNHOcrInfo);
            pnNHocr.Location = new System.Drawing.Point(10, 68);
            pnNHocr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnNHocr.Name = "pnNHocr";
            pnNHocr.Size = new System.Drawing.Size(589, 79);
            pnNHocr.TabIndex = 56;
            // 
            // lbNHOcrInfo
            // 
            lbNHOcrInfo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lbNHOcrInfo.AutoEllipsis = true;
            lbNHOcrInfo.AutoSize = true;
            lbNHOcrInfo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbNHOcrInfo.ForeColor = System.Drawing.Color.White;
            lbNHOcrInfo.Location = new System.Drawing.Point(68, 18);
            lbNHOcrInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbNHOcrInfo.Name = "lbNHOcrInfo";
            lbNHOcrInfo.Size = new System.Drawing.Size(472, 46);
            lbNHOcrInfo.TabIndex = 18;
            lbNHOcrInfo.Text = "특정 상황에 한해서만 Tesseract OCR 보다 인식이 잘됩니다.\r\n가능하면 Tessract이나 WinOCR을 사용해 주세요";
            lbNHOcrInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnTranslate
            // 
            pnTranslate.Controls.Add(pnGoogleBasic);
            pnTranslate.Controls.Add(pnCustomApi);
            pnTranslate.Controls.Add(pnDeepLX);
            pnTranslate.Controls.Add(btnTransHelp);
            pnTranslate.Controls.Add(cbPerWordDic);
            pnTranslate.Controls.Add(lbTransType);
            pnTranslate.Controls.Add(TransType_Combobox);
            pnTranslate.Controls.Add(checkDic);
            pnTranslate.Controls.Add(dicFileTextBox);
            pnTranslate.Controls.Add(lbDicFile);
            pnTranslate.Controls.Add(lbTransTypeTitle);
            pnTranslate.Controls.Add(pnDeepl);
            pnTranslate.Controls.Add(DB_Panel);
            pnTranslate.Controls.Add(Naver_Panel);
            pnTranslate.Controls.Add(Google_Panel);
            pnTranslate.Controls.Add(pnEzTrans);
            pnTranslate.Controls.Add(pnPapagoWeb);
            pnTranslate.Location = new System.Drawing.Point(4, 205);
            pnTranslate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnTranslate.Name = "pnTranslate";
            pnTranslate.Size = new System.Drawing.Size(666, 271);
            pnTranslate.TabIndex = 37;
            pnTranslate.Paint += panealBorder_Paint;
            // 
            // pnGoogleBasic
            // 
            pnGoogleBasic.Controls.Add(lbBasicStatus);
            pnGoogleBasic.Controls.Add(lbBasicInfo);
            pnGoogleBasic.Location = new System.Drawing.Point(9, 76);
            pnGoogleBasic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnGoogleBasic.Name = "pnGoogleBasic";
            pnGoogleBasic.Size = new System.Drawing.Size(629, 118);
            pnGoogleBasic.TabIndex = 53;
            // 
            // lbBasicStatus
            // 
            lbBasicStatus.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbBasicStatus.ForeColor = System.Drawing.Color.White;
            lbBasicStatus.Location = new System.Drawing.Point(4, 75);
            lbBasicStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbBasicStatus.Name = "lbBasicStatus";
            lbBasicStatus.Size = new System.Drawing.Size(419, 22);
            lbBasicStatus.TabIndex = 18;
            lbBasicStatus.Text = "상태 : 고품질";
            lbBasicStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbBasicInfo
            // 
            lbBasicInfo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbBasicInfo.ForeColor = System.Drawing.Color.White;
            lbBasicInfo.Location = new System.Drawing.Point(4, 26);
            lbBasicInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbBasicInfo.Name = "lbBasicInfo";
            lbBasicInfo.Size = new System.Drawing.Size(611, 42);
            lbBasicInfo.TabIndex = 17;
            lbBasicInfo.Text = "구글 기본 번역기의 고품질 번역은 시간당 100회까지만 적용되며\r\n초과시 낮은 품질로 번역됩니다.";
            lbBasicInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnCustomApi
            // 
            pnCustomApi.Controls.Add(lbCustomApiInformation);
            pnCustomApi.Location = new System.Drawing.Point(9, 76);
            pnCustomApi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnCustomApi.Name = "pnCustomApi";
            pnCustomApi.Size = new System.Drawing.Size(604, 118);
            pnCustomApi.TabIndex = 55;
            // 
            // lbCustomApiInformation
            // 
            lbCustomApiInformation.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbCustomApiInformation.ForeColor = System.Drawing.Color.White;
            lbCustomApiInformation.Location = new System.Drawing.Point(4, 10);
            lbCustomApiInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbCustomApiInformation.Name = "lbCustomApiInformation";
            lbCustomApiInformation.Size = new System.Drawing.Size(586, 42);
            lbCustomApiInformation.TabIndex = 17;
            lbCustomApiInformation.Text = "커스텀 API는 고급 설정에서 설정하시면 됩니다";
            lbCustomApiInformation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnDeepLX
            // 
            pnDeepLX.Controls.Add(lbDeepLXInformation);
            pnDeepLX.Location = new System.Drawing.Point(9, 76);
            pnDeepLX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnDeepLX.Name = "pnDeepLX";
            pnDeepLX.Size = new System.Drawing.Size(604, 118);
            pnDeepLX.TabIndex = 55;
            // 
            // lbDeepLXInformation
            // 
            lbDeepLXInformation.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDeepLXInformation.ForeColor = System.Drawing.Color.White;
            lbDeepLXInformation.Location = new System.Drawing.Point(4, 10);
            lbDeepLXInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDeepLXInformation.Name = "lbDeepLXInformation";
            lbDeepLXInformation.Size = new System.Drawing.Size(586, 46);
            lbDeepLXInformation.TabIndex = 17;
            lbDeepLXInformation.Text = "Translate via DeepLX, use the default port (1188). The language code for translation is set the same as for Google Translate.";
            lbDeepLXInformation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTransHelp
            // 
            btnTransHelp.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            btnTransHelp.FlatAppearance.BorderSize = 0;
            btnTransHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTransHelp.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnTransHelp.ForeColor = System.Drawing.Color.White;
            btnTransHelp.Location = new System.Drawing.Point(355, 38);
            btnTransHelp.Margin = new System.Windows.Forms.Padding(0);
            btnTransHelp.Name = "btnTransHelp";
            btnTransHelp.Size = new System.Drawing.Size(35, 31);
            btnTransHelp.TabIndex = 58;
            btnTransHelp.Text = "?";
            btnTransHelp.UseVisualStyleBackColor = false;
            btnTransHelp.Click += OnClick_btnTransHelp;
            // 
            // cbPerWordDic
            // 
            cbPerWordDic.AutoSize = true;
            cbPerWordDic.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbPerWordDic.ForeColor = System.Drawing.Color.White;
            cbPerWordDic.Location = new System.Drawing.Point(14, 235);
            cbPerWordDic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbPerWordDic.Name = "cbPerWordDic";
            cbPerWordDic.Size = new System.Drawing.Size(386, 27);
            cbPerWordDic.TabIndex = 57;
            cbPerWordDic.Text = "단어 단위로 교정 (완벽히 일치한 단어만 교정)";
            cbPerWordDic.UseVisualStyleBackColor = true;
            // 
            // lbTransType
            // 
            lbTransType.AutoSize = true;
            lbTransType.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbTransType.ForeColor = System.Drawing.Color.White;
            lbTransType.Location = new System.Drawing.Point(11, 41);
            lbTransType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbTransType.Name = "lbTransType";
            lbTransType.Size = new System.Drawing.Size(78, 23);
            lbTransType.TabIndex = 20;
            lbTransType.Text = "번역방법";
            // 
            // TransType_Combobox
            // 
            TransType_Combobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            TransType_Combobox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            TransType_Combobox.FormattingEnabled = true;
            TransType_Combobox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            TransType_Combobox.Items.AddRange(new object[] { "TRANSLATE GOOGLE", "TRANSLATE DB", "TRANSLATE PAPAGO WEB", "TRANSLATE NAVER", "TRANSLATE GOOGLE SHEET", "TRANSLATE DEEPL", "TRANSLATE EZTRANS", "TRANSLATE CUSTOM API", "TRANSLATE DEEPLX" });
            TransType_Combobox.Location = new System.Drawing.Point(131, 38);
            TransType_Combobox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            TransType_Combobox.Name = "TransType_Combobox";
            TransType_Combobox.Size = new System.Drawing.Size(205, 29);
            TransType_Combobox.TabIndex = 49;
            TransType_Combobox.SelectedIndexChanged += TransType_Combobox_SelectedIndexChanged;
            // 
            // checkDic
            // 
            checkDic.AutoSize = true;
            checkDic.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkDic.ForeColor = System.Drawing.Color.White;
            checkDic.Location = new System.Drawing.Point(14, 201);
            checkDic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkDic.Name = "checkDic";
            checkDic.Size = new System.Drawing.Size(140, 27);
            checkDic.TabIndex = 24;
            checkDic.Text = "교정사전 사용";
            checkDic.UseVisualStyleBackColor = true;
            checkDic.CheckedChanged += checkDic_CheckedChanged;
            // 
            // dicFileTextBox
            // 
            dicFileTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dicFileTextBox.Location = new System.Drawing.Point(281, 199);
            dicFileTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            dicFileTextBox.Name = "dicFileTextBox";
            dicFileTextBox.Size = new System.Drawing.Size(314, 29);
            dicFileTextBox.TabIndex = 23;
            // 
            // lbDicFile
            // 
            lbDicFile.AutoSize = true;
            lbDicFile.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDicFile.ForeColor = System.Drawing.Color.White;
            lbDicFile.Location = new System.Drawing.Point(174, 202);
            lbDicFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDicFile.Name = "lbDicFile";
            lbDicFile.Size = new System.Drawing.Size(78, 23);
            lbDicFile.TabIndex = 22;
            lbDicFile.Text = "파일이름";
            // 
            // lbTransTypeTitle
            // 
            lbTransTypeTitle.AutoSize = true;
            lbTransTypeTitle.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbTransTypeTitle.ForeColor = System.Drawing.Color.White;
            lbTransTypeTitle.Location = new System.Drawing.Point(5, 4);
            lbTransTypeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbTransTypeTitle.Name = "lbTransTypeTitle";
            lbTransTypeTitle.Size = new System.Drawing.Size(95, 25);
            lbTransTypeTitle.TabIndex = 8;
            lbTransTypeTitle.Text = "번역 설정";
            // 
            // pnDeepl
            // 
            pnDeepl.Controls.Add(btnCheckDeeplState);
            pnDeepl.Controls.Add(lbDeepLStatus);
            pnDeepl.Controls.Add(lbDeepLInfo);
            pnDeepl.Location = new System.Drawing.Point(9, 76);
            pnDeepl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnDeepl.Name = "pnDeepl";
            pnDeepl.Size = new System.Drawing.Size(604, 118);
            pnDeepl.TabIndex = 54;
            // 
            // btnCheckDeeplState
            // 
            btnCheckDeeplState.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnCheckDeeplState.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnCheckDeeplState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCheckDeeplState.ForeColor = System.Drawing.Color.White;
            btnCheckDeeplState.Location = new System.Drawing.Point(402, 66);
            btnCheckDeeplState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnCheckDeeplState.Name = "btnCheckDeeplState";
            btnCheckDeeplState.Size = new System.Drawing.Size(188, 31);
            btnCheckDeeplState.TabIndex = 52;
            btnCheckDeeplState.Text = "상태 확인하기";
            btnCheckDeeplState.UseVisualStyleBackColor = false;
            btnCheckDeeplState.Click += OnClickCheckDeeplState;
            // 
            // lbDeepLStatus
            // 
            lbDeepLStatus.AutoSize = true;
            lbDeepLStatus.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDeepLStatus.ForeColor = System.Drawing.Color.White;
            lbDeepLStatus.Location = new System.Drawing.Point(4, 78);
            lbDeepLStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDeepLStatus.Name = "lbDeepLStatus";
            lbDeepLStatus.Size = new System.Drawing.Size(111, 23);
            lbDeepLStatus.TabIndex = 18;
            lbDeepLStatus.Text = "상태 : 고품질";
            lbDeepLStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDeepLInfo
            // 
            lbDeepLInfo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDeepLInfo.ForeColor = System.Drawing.Color.White;
            lbDeepLInfo.Location = new System.Drawing.Point(4, 10);
            lbDeepLInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDeepLInfo.Name = "lbDeepLInfo";
            lbDeepLInfo.Size = new System.Drawing.Size(586, 42);
            lbDeepLInfo.TabIndex = 17;
            lbDeepLInfo.Text = "사용을 위해 마이크로소프트 엣지가 필요합니다";
            lbDeepLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DB_Panel
            // 
            DB_Panel.Controls.Add(cbDBMultiGet);
            DB_Panel.Controls.Add(checkStringUpper);
            DB_Panel.Controls.Add(dbFileTextBox);
            DB_Panel.Controls.Add(lbDbFile);
            DB_Panel.Location = new System.Drawing.Point(9, 76);
            DB_Panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            DB_Panel.Name = "DB_Panel";
            DB_Panel.Size = new System.Drawing.Size(565, 118);
            DB_Panel.TabIndex = 50;
            // 
            // cbDBMultiGet
            // 
            cbDBMultiGet.AutoSize = true;
            cbDBMultiGet.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbDBMultiGet.ForeColor = System.Drawing.Color.White;
            cbDBMultiGet.Location = new System.Drawing.Point(6, 81);
            cbDBMultiGet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbDBMultiGet.Name = "cbDBMultiGet";
            cbDBMultiGet.Size = new System.Drawing.Size(514, 27);
            cbDBMultiGet.TabIndex = 26;
            cbDBMultiGet.Text = "DB 부분 일치 검색 - 문장과 부분 일치한 번역문 모두 가져오기";
            cbDBMultiGet.UseVisualStyleBackColor = true;
            // 
            // checkStringUpper
            // 
            checkStringUpper.AutoSize = true;
            checkStringUpper.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkStringUpper.ForeColor = System.Drawing.Color.White;
            checkStringUpper.Location = new System.Drawing.Point(6, 50);
            checkStringUpper.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkStringUpper.Name = "checkStringUpper";
            checkStringUpper.Size = new System.Drawing.Size(279, 27);
            checkStringUpper.TabIndex = 25;
            checkStringUpper.Text = "DB 검색 시 대소문자 구분 안 함";
            checkStringUpper.UseVisualStyleBackColor = true;
            // 
            // dbFileTextBox
            // 
            dbFileTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dbFileTextBox.Location = new System.Drawing.Point(122, 4);
            dbFileTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            dbFileTextBox.Name = "dbFileTextBox";
            dbFileTextBox.Size = new System.Drawing.Size(314, 29);
            dbFileTextBox.TabIndex = 19;
            dbFileTextBox.Text = "empty.txt";
            // 
            // lbDbFile
            // 
            lbDbFile.AutoSize = true;
            lbDbFile.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDbFile.ForeColor = System.Drawing.Color.White;
            lbDbFile.Location = new System.Drawing.Point(4, 10);
            lbDbFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDbFile.Name = "lbDbFile";
            lbDbFile.Size = new System.Drawing.Size(90, 23);
            lbDbFile.TabIndex = 16;
            lbDbFile.Text = "파일이름  ";
            // 
            // Naver_Panel
            // 
            Naver_Panel.Controls.Add(Button_NaverTransKeyList);
            Naver_Panel.Controls.Add(lbPapagoSecret);
            Naver_Panel.Controls.Add(NaverSecretKeyTextBox);
            Naver_Panel.Controls.Add(NaverIDKeyTextBox);
            Naver_Panel.Controls.Add(lbPapagoID);
            Naver_Panel.Location = new System.Drawing.Point(9, 76);
            Naver_Panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Naver_Panel.Name = "Naver_Panel";
            Naver_Panel.Size = new System.Drawing.Size(604, 118);
            Naver_Panel.TabIndex = 52;
            // 
            // Button_NaverTransKeyList
            // 
            Button_NaverTransKeyList.AutoSize = true;
            Button_NaverTransKeyList.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            Button_NaverTransKeyList.FlatAppearance.BorderSize = 0;
            Button_NaverTransKeyList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Button_NaverTransKeyList.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            Button_NaverTransKeyList.ForeColor = System.Drawing.Color.White;
            Button_NaverTransKeyList.Location = new System.Drawing.Point(450, 8);
            Button_NaverTransKeyList.Margin = new System.Windows.Forms.Padding(0);
            Button_NaverTransKeyList.Name = "Button_NaverTransKeyList";
            Button_NaverTransKeyList.Size = new System.Drawing.Size(138, 105);
            Button_NaverTransKeyList.TabIndex = 52;
            Button_NaverTransKeyList.Text = "키 관리";
            Button_NaverTransKeyList.UseVisualStyleBackColor = false;
            Button_NaverTransKeyList.Click += Button_NaverTransKeyList_Click;
            // 
            // lbPapagoSecret
            // 
            lbPapagoSecret.AutoSize = true;
            lbPapagoSecret.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPapagoSecret.ForeColor = System.Drawing.Color.White;
            lbPapagoSecret.Location = new System.Drawing.Point(4, 48);
            lbPapagoSecret.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPapagoSecret.Name = "lbPapagoSecret";
            lbPapagoSecret.Size = new System.Drawing.Size(83, 23);
            lbPapagoSecret.TabIndex = 23;
            lbPapagoSecret.Text = "Secret 키";
            // 
            // NaverSecretKeyTextBox
            // 
            NaverSecretKeyTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            NaverSecretKeyTextBox.Location = new System.Drawing.Point(122, 42);
            NaverSecretKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            NaverSecretKeyTextBox.Name = "NaverSecretKeyTextBox";
            NaverSecretKeyTextBox.Size = new System.Drawing.Size(314, 29);
            NaverSecretKeyTextBox.TabIndex = 22;
            // 
            // NaverIDKeyTextBox
            // 
            NaverIDKeyTextBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            NaverIDKeyTextBox.Location = new System.Drawing.Point(122, 4);
            NaverIDKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            NaverIDKeyTextBox.Name = "NaverIDKeyTextBox";
            NaverIDKeyTextBox.Size = new System.Drawing.Size(314, 29);
            NaverIDKeyTextBox.TabIndex = 21;
            // 
            // lbPapagoID
            // 
            lbPapagoID.AutoSize = true;
            lbPapagoID.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPapagoID.ForeColor = System.Drawing.Color.White;
            lbPapagoID.Location = new System.Drawing.Point(4, 10);
            lbPapagoID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPapagoID.Name = "lbPapagoID";
            lbPapagoID.Size = new System.Drawing.Size(57, 23);
            lbPapagoID.TabIndex = 17;
            lbPapagoID.Text = "ID 키 ";
            // 
            // Google_Panel
            // 
            Google_Panel.Controls.Add(button_RemoveAllGoogleToekn);
            Google_Panel.Controls.Add(textBox_GoogleSecretKey);
            Google_Panel.Controls.Add(lbSheetSecret);
            Google_Panel.Controls.Add(textBox_GoogleClientID);
            Google_Panel.Controls.Add(lbSheetID);
            Google_Panel.Controls.Add(googleSheet_textBox);
            Google_Panel.Controls.Add(lbGoogleSheetAddress);
            Google_Panel.Location = new System.Drawing.Point(9, 76);
            Google_Panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Google_Panel.Name = "Google_Panel";
            Google_Panel.Size = new System.Drawing.Size(604, 115);
            Google_Panel.TabIndex = 53;
            // 
            // button_RemoveAllGoogleToekn
            // 
            button_RemoveAllGoogleToekn.AutoSize = true;
            button_RemoveAllGoogleToekn.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            button_RemoveAllGoogleToekn.FlatAppearance.BorderSize = 0;
            button_RemoveAllGoogleToekn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button_RemoveAllGoogleToekn.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            button_RemoveAllGoogleToekn.ForeColor = System.Drawing.Color.White;
            button_RemoveAllGoogleToekn.Location = new System.Drawing.Point(450, 8);
            button_RemoveAllGoogleToekn.Margin = new System.Windows.Forms.Padding(0);
            button_RemoveAllGoogleToekn.Name = "button_RemoveAllGoogleToekn";
            button_RemoveAllGoogleToekn.Size = new System.Drawing.Size(160, 105);
            button_RemoveAllGoogleToekn.TabIndex = 45;
            button_RemoveAllGoogleToekn.Text = "모든 인증\r\n초기화";
            button_RemoveAllGoogleToekn.UseVisualStyleBackColor = false;
            button_RemoveAllGoogleToekn.Click += button_RemoveAllGoogleToekn_Click;
            // 
            // textBox_GoogleSecretKey
            // 
            textBox_GoogleSecretKey.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            textBox_GoogleSecretKey.Location = new System.Drawing.Point(122, 81);
            textBox_GoogleSecretKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            textBox_GoogleSecretKey.Name = "textBox_GoogleSecretKey";
            textBox_GoogleSecretKey.Size = new System.Drawing.Size(314, 29);
            textBox_GoogleSecretKey.TabIndex = 27;
            // 
            // lbSheetSecret
            // 
            lbSheetSecret.AutoSize = true;
            lbSheetSecret.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbSheetSecret.ForeColor = System.Drawing.Color.White;
            lbSheetSecret.Location = new System.Drawing.Point(4, 84);
            lbSheetSecret.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbSheetSecret.Name = "lbSheetSecret";
            lbSheetSecret.Size = new System.Drawing.Size(83, 23);
            lbSheetSecret.TabIndex = 26;
            lbSheetSecret.Text = "Secret 키";
            // 
            // textBox_GoogleClientID
            // 
            textBox_GoogleClientID.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            textBox_GoogleClientID.Location = new System.Drawing.Point(122, 42);
            textBox_GoogleClientID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            textBox_GoogleClientID.Name = "textBox_GoogleClientID";
            textBox_GoogleClientID.Size = new System.Drawing.Size(314, 29);
            textBox_GoogleClientID.TabIndex = 25;
            // 
            // lbSheetID
            // 
            lbSheetID.AutoSize = true;
            lbSheetID.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbSheetID.ForeColor = System.Drawing.Color.White;
            lbSheetID.Location = new System.Drawing.Point(4, 48);
            lbSheetID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbSheetID.Name = "lbSheetID";
            lbSheetID.Size = new System.Drawing.Size(81, 23);
            lbSheetID.TabIndex = 24;
            lbSheetID.Text = "Client ID";
            // 
            // googleSheet_textBox
            // 
            googleSheet_textBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            googleSheet_textBox.Location = new System.Drawing.Point(122, 4);
            googleSheet_textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            googleSheet_textBox.Name = "googleSheet_textBox";
            googleSheet_textBox.Size = new System.Drawing.Size(314, 29);
            googleSheet_textBox.TabIndex = 21;
            // 
            // lbGoogleSheetAddress
            // 
            lbGoogleSheetAddress.AutoSize = true;
            lbGoogleSheetAddress.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbGoogleSheetAddress.ForeColor = System.Drawing.Color.White;
            lbGoogleSheetAddress.Location = new System.Drawing.Point(4, 10);
            lbGoogleSheetAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbGoogleSheetAddress.Name = "lbGoogleSheetAddress";
            lbGoogleSheetAddress.Size = new System.Drawing.Size(84, 23);
            lbGoogleSheetAddress.TabIndex = 17;
            lbGoogleSheetAddress.Text = "시트 주소";
            // 
            // pnEzTrans
            // 
            pnEzTrans.Controls.Add(lbEzTransInfo);
            pnEzTrans.Location = new System.Drawing.Point(9, 76);
            pnEzTrans.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnEzTrans.Name = "pnEzTrans";
            pnEzTrans.Size = new System.Drawing.Size(604, 118);
            pnEzTrans.TabIndex = 54;
            // 
            // lbEzTransInfo
            // 
            lbEzTransInfo.AutoSize = true;
            lbEzTransInfo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbEzTransInfo.ForeColor = System.Drawing.Color.White;
            lbEzTransInfo.Location = new System.Drawing.Point(99, 26);
            lbEzTransInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbEzTransInfo.Name = "lbEzTransInfo";
            lbEzTransInfo.Size = new System.Drawing.Size(407, 69);
            lbEzTransInfo.TabIndex = 17;
            lbEzTransInfo.Text = "일본어 전용\r\nezTrans XP가 설치 되어 있어야 합니다.\r\n자세한 사용법은 번역 설정 도움말을 확인해 주세요.";
            lbEzTransInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnPapagoWeb
            // 
            pnPapagoWeb.Controls.Add(lbPapagoWebInfo);
            pnPapagoWeb.Location = new System.Drawing.Point(9, 76);
            pnPapagoWeb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pnPapagoWeb.Name = "pnPapagoWeb";
            pnPapagoWeb.Size = new System.Drawing.Size(629, 118);
            pnPapagoWeb.TabIndex = 59;
            // 
            // lbPapagoWebInfo
            // 
            lbPapagoWebInfo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPapagoWebInfo.ForeColor = System.Drawing.Color.White;
            lbPapagoWebInfo.Location = new System.Drawing.Point(4, 26);
            lbPapagoWebInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPapagoWebInfo.Name = "lbPapagoWebInfo";
            lbPapagoWebInfo.Size = new System.Drawing.Size(611, 42);
            lbPapagoWebInfo.TabIndex = 17;
            lbPapagoWebInfo.Text = "파파고 웹 번역기 입니다.\r\n스냅샷 위주로 사용해 주세요.";
            lbPapagoWebInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpText
            // 
            tpText.Controls.Add(panel5);
            tpText.Location = new System.Drawing.Point(80, 4);
            tpText.Margin = new System.Windows.Forms.Padding(0);
            tpText.Name = "tpText";
            tpText.Size = new System.Drawing.Size(696, 733);
            tpText.TabIndex = 1;
            tpText.Text = "텍스트";
            tpText.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel5.Controls.Add(panel17);
            panel5.Controls.Add(panel10);
            panel5.Controls.Add(panel9);
            panel5.Controls.Add(panel7);
            panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            panel5.Location = new System.Drawing.Point(0, 0);
            panel5.Margin = new System.Windows.Forms.Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(696, 733);
            panel5.TabIndex = 0;
            // 
            // panel17
            // 
            panel17.Controls.Add(lbPreview);
            panel17.Controls.Add(fontResultLabel);
            panel17.Location = new System.Drawing.Point(4, 351);
            panel17.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel17.Name = "panel17";
            panel17.Size = new System.Drawing.Size(666, 359);
            panel17.TabIndex = 40;
            panel17.Paint += panealBorder_Paint;
            // 
            // lbPreview
            // 
            lbPreview.AutoSize = true;
            lbPreview.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPreview.ForeColor = System.Drawing.Color.White;
            lbPreview.Location = new System.Drawing.Point(5, 4);
            lbPreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPreview.Name = "lbPreview";
            lbPreview.Size = new System.Drawing.Size(88, 25);
            lbPreview.TabIndex = 8;
            lbPreview.Text = "미리보기";
            // 
            // fontResultLabel
            // 
            fontResultLabel.AlignmentRight = false;
            fontResultLabel.Font = new System.Drawing.Font("Malgun Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            fontResultLabel.IsAlignmentCenter = false;
            fontResultLabel.IsFillBackColor = false;
            fontResultLabel.Location = new System.Drawing.Point(15, 42);
            fontResultLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            fontResultLabel.Name = "fontResultLabel";
            fontResultLabel.OutlineForeColor = System.Drawing.Color.Blue;
            fontResultLabel.OutlineForecolor2 = System.Drawing.Color.FromArgb(128, 128, 255);
            fontResultLabel.Size = new System.Drawing.Size(639, 311);
            fontResultLabel.TabIndex = 39;
            fontResultLabel.Text = "-설정 결과를 미리 봅니다.\r\n-어두운 번역창에는 적용되지 않습니다.\r\n\r\n-1 2 3 4 5 6\r\n-Tank division!";
            fontResultLabel.TextColor = System.Drawing.Color.White;
            fontResultLabel.TextFont = new System.Drawing.Font("Malgun Gothic", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            // 
            // panel10
            // 
            panel10.Controls.Add(defaultColorButton);
            panel10.Controls.Add(lbFontBackground);
            panel10.Controls.Add(lbFontOutlineColor2);
            panel10.Controls.Add(lbFontOutlineColor1);
            panel10.Controls.Add(backgroundColorBox);
            panel10.Controls.Add(outlineColor2Box);
            panel10.Controls.Add(outlineColor1Box);
            panel10.Controls.Add(lbFontBasicColor);
            panel10.Controls.Add(textColorBox);
            panel10.Controls.Add(lbFontColor);
            panel10.Location = new System.Drawing.Point(4, 98);
            panel10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel10.Name = "panel10";
            panel10.Size = new System.Drawing.Size(666, 134);
            panel10.TabIndex = 38;
            panel10.Paint += panealBorder_Paint;
            // 
            // defaultColorButton
            // 
            defaultColorButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            defaultColorButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            defaultColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            defaultColorButton.ForeColor = System.Drawing.Color.White;
            defaultColorButton.Location = new System.Drawing.Point(11, 99);
            defaultColorButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            defaultColorButton.Name = "defaultColorButton";
            defaultColorButton.Size = new System.Drawing.Size(645, 31);
            defaultColorButton.TabIndex = 25;
            defaultColorButton.Text = "기본 색으로";
            defaultColorButton.UseVisualStyleBackColor = false;
            defaultColorButton.Click += defaultColorButton_Click;
            // 
            // lbFontBackground
            // 
            lbFontBackground.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontBackground.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontBackground.ForeColor = System.Drawing.Color.White;
            lbFontBackground.Location = new System.Drawing.Point(442, 60);
            lbFontBackground.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontBackground.Name = "lbFontBackground";
            lbFontBackground.Size = new System.Drawing.Size(112, 25);
            lbFontBackground.TabIndex = 30;
            lbFontBackground.Text = "배경색";
            lbFontBackground.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFontOutlineColor2
            // 
            lbFontOutlineColor2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontOutlineColor2.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontOutlineColor2.ForeColor = System.Drawing.Color.White;
            lbFontOutlineColor2.Location = new System.Drawing.Point(330, 60);
            lbFontOutlineColor2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontOutlineColor2.Name = "lbFontOutlineColor2";
            lbFontOutlineColor2.Size = new System.Drawing.Size(112, 25);
            lbFontOutlineColor2.TabIndex = 29;
            lbFontOutlineColor2.Text = "외곽선2";
            lbFontOutlineColor2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFontOutlineColor1
            // 
            lbFontOutlineColor1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontOutlineColor1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontOutlineColor1.ForeColor = System.Drawing.Color.White;
            lbFontOutlineColor1.Location = new System.Drawing.Point(218, 60);
            lbFontOutlineColor1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontOutlineColor1.Name = "lbFontOutlineColor1";
            lbFontOutlineColor1.Size = new System.Drawing.Size(112, 25);
            lbFontOutlineColor1.TabIndex = 28;
            lbFontOutlineColor1.Text = "외곽선1";
            lbFontOutlineColor1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // backgroundColorBox
            // 
            backgroundColorBox.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            backgroundColorBox.Location = new System.Drawing.Point(468, 30);
            backgroundColorBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            backgroundColorBox.Name = "backgroundColorBox";
            backgroundColorBox.Size = new System.Drawing.Size(30, 30);
            backgroundColorBox.TabIndex = 27;
            backgroundColorBox.TabStop = false;
            backgroundColorBox.Click += backgroundColorBox_Click;
            // 
            // outlineColor2Box
            // 
            outlineColor2Box.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            outlineColor2Box.Location = new System.Drawing.Point(359, 30);
            outlineColor2Box.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            outlineColor2Box.Name = "outlineColor2Box";
            outlineColor2Box.Size = new System.Drawing.Size(30, 30);
            outlineColor2Box.TabIndex = 26;
            outlineColor2Box.TabStop = false;
            outlineColor2Box.Click += outlineColor2Box_Click;
            // 
            // outlineColor1Box
            // 
            outlineColor1Box.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            outlineColor1Box.Location = new System.Drawing.Point(246, 30);
            outlineColor1Box.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            outlineColor1Box.Name = "outlineColor1Box";
            outlineColor1Box.Size = new System.Drawing.Size(30, 30);
            outlineColor1Box.TabIndex = 25;
            outlineColor1Box.TabStop = false;
            outlineColor1Box.Click += outlineColor1Box_Click;
            // 
            // lbFontBasicColor
            // 
            lbFontBasicColor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontBasicColor.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontBasicColor.ForeColor = System.Drawing.Color.White;
            lbFontBasicColor.Location = new System.Drawing.Point(105, 60);
            lbFontBasicColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontBasicColor.Name = "lbFontBasicColor";
            lbFontBasicColor.Size = new System.Drawing.Size(112, 25);
            lbFontBasicColor.TabIndex = 24;
            lbFontBasicColor.Text = "색상";
            lbFontBasicColor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textColorBox
            // 
            textColorBox.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            textColorBox.Location = new System.Drawing.Point(134, 30);
            textColorBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            textColorBox.Name = "textColorBox";
            textColorBox.Size = new System.Drawing.Size(30, 30);
            textColorBox.TabIndex = 24;
            textColorBox.TabStop = false;
            textColorBox.Click += textColorBox_Click;
            // 
            // lbFontColor
            // 
            lbFontColor.AutoSize = true;
            lbFontColor.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontColor.ForeColor = System.Drawing.Color.White;
            lbFontColor.Location = new System.Drawing.Point(5, 4);
            lbFontColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontColor.Name = "lbFontColor";
            lbFontColor.Size = new System.Drawing.Size(31, 25);
            lbFontColor.TabIndex = 8;
            lbFontColor.Text = "색";
            // 
            // panel9
            // 
            panel9.Controls.Add(cbShowOCRIndex);
            panel9.Controls.Add(useBackColorCheckBox);
            panel9.Controls.Add(removeSpaceCheckBox);
            panel9.Controls.Add(alignmentCenterCheckBox);
            panel9.Controls.Add(lbTextAdditionalSettings);
            panel9.Location = new System.Drawing.Point(4, 239);
            panel9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel9.Name = "panel9";
            panel9.Size = new System.Drawing.Size(666, 105);
            panel9.TabIndex = 38;
            panel9.Paint += panealBorder_Paint;
            // 
            // cbShowOCRIndex
            // 
            cbShowOCRIndex.AutoSize = true;
            cbShowOCRIndex.Checked = true;
            cbShowOCRIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            cbShowOCRIndex.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbShowOCRIndex.ForeColor = System.Drawing.Color.White;
            cbShowOCRIndex.Location = new System.Drawing.Point(21, 66);
            cbShowOCRIndex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbShowOCRIndex.Name = "cbShowOCRIndex";
            cbShowOCRIndex.Size = new System.Drawing.Size(168, 24);
            cbShowOCRIndex.TabIndex = 12;
            cbShowOCRIndex.Text = "OCR 영역 번호 표시";
            cbShowOCRIndex.UseVisualStyleBackColor = true;
            cbShowOCRIndex.CheckedChanged += cbShowOCRIndex_CheckedChanged;
            // 
            // useBackColorCheckBox
            // 
            useBackColorCheckBox.AutoSize = true;
            useBackColorCheckBox.Checked = true;
            useBackColorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            useBackColorCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            useBackColorCheckBox.ForeColor = System.Drawing.Color.White;
            useBackColorCheckBox.Location = new System.Drawing.Point(506, 32);
            useBackColorCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            useBackColorCheckBox.Name = "useBackColorCheckBox";
            useBackColorCheckBox.Size = new System.Drawing.Size(111, 24);
            useBackColorCheckBox.TabIndex = 11;
            useBackColorCheckBox.Text = "배경색 사용";
            useBackColorCheckBox.UseVisualStyleBackColor = true;
            useBackColorCheckBox.CheckedChanged += useBackColorCheckBox_CheckedChanged;
            // 
            // removeSpaceCheckBox
            // 
            removeSpaceCheckBox.AutoSize = true;
            removeSpaceCheckBox.Checked = true;
            removeSpaceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            removeSpaceCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            removeSpaceCheckBox.ForeColor = System.Drawing.Color.White;
            removeSpaceCheckBox.Location = new System.Drawing.Point(232, 32);
            removeSpaceCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            removeSpaceCheckBox.Name = "removeSpaceCheckBox";
            removeSpaceCheckBox.Size = new System.Drawing.Size(168, 24);
            removeSpaceCheckBox.TabIndex = 10;
            removeSpaceCheckBox.Text = "OCR 결과 공백 제거";
            removeSpaceCheckBox.UseVisualStyleBackColor = true;
            removeSpaceCheckBox.CheckedChanged += removeSpaceCheckBox_CheckedChanged;
            // 
            // alignmentCenterCheckBox
            // 
            alignmentCenterCheckBox.AutoSize = true;
            alignmentCenterCheckBox.Checked = true;
            alignmentCenterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            alignmentCenterCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            alignmentCenterCheckBox.ForeColor = System.Drawing.Color.White;
            alignmentCenterCheckBox.Location = new System.Drawing.Point(21, 32);
            alignmentCenterCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            alignmentCenterCheckBox.Name = "alignmentCenterCheckBox";
            alignmentCenterCheckBox.Size = new System.Drawing.Size(111, 24);
            alignmentCenterCheckBox.TabIndex = 9;
            alignmentCenterCheckBox.Text = "가운데 정렬";
            alignmentCenterCheckBox.UseVisualStyleBackColor = true;
            alignmentCenterCheckBox.CheckedChanged += alignmentCenterCheckBox_CheckedChanged;
            // 
            // lbTextAdditionalSettings
            // 
            lbTextAdditionalSettings.AutoSize = true;
            lbTextAdditionalSettings.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbTextAdditionalSettings.ForeColor = System.Drawing.Color.White;
            lbTextAdditionalSettings.Location = new System.Drawing.Point(5, 4);
            lbTextAdditionalSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbTextAdditionalSettings.Name = "lbTextAdditionalSettings";
            lbTextAdditionalSettings.Size = new System.Drawing.Size(88, 25);
            lbTextAdditionalSettings.TabIndex = 8;
            lbTextAdditionalSettings.Text = "부가설정";
            // 
            // panel7
            // 
            panel7.Controls.Add(fontSizeUpDown);
            panel7.Controls.Add(lbFontSize);
            panel7.Controls.Add(lbFont);
            panel7.Controls.Add(fontButton);
            panel7.Controls.Add(lbFontSetting);
            panel7.Location = new System.Drawing.Point(4, 4);
            panel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel7.Name = "panel7";
            panel7.Size = new System.Drawing.Size(666, 86);
            panel7.TabIndex = 37;
            panel7.Paint += panealBorder_Paint;
            // 
            // fontSizeUpDown
            // 
            fontSizeUpDown.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            fontSizeUpDown.ForeColor = System.Drawing.Color.White;
            fontSizeUpDown.Location = new System.Drawing.Point(398, 45);
            fontSizeUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            fontSizeUpDown.Minimum = new decimal(new int[] { 8, 0, 0, 0 });
            fontSizeUpDown.Name = "fontSizeUpDown";
            fontSizeUpDown.Size = new System.Drawing.Size(59, 27);
            fontSizeUpDown.TabIndex = 24;
            fontSizeUpDown.Value = new decimal(new int[] { 8, 0, 0, 0 });
            fontSizeUpDown.ValueChanged += fontSizeUpDown_ValueChanged;
            // 
            // lbFontSize
            // 
            lbFontSize.AutoSize = true;
            lbFontSize.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontSize.ForeColor = System.Drawing.Color.White;
            lbFontSize.Location = new System.Drawing.Point(319, 45);
            lbFontSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontSize.Name = "lbFontSize";
            lbFontSize.Size = new System.Drawing.Size(54, 23);
            lbFontSize.TabIndex = 22;
            lbFontSize.Text = "크기 :";
            // 
            // lbFont
            // 
            lbFont.AutoSize = true;
            lbFont.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFont.ForeColor = System.Drawing.Color.White;
            lbFont.Location = new System.Drawing.Point(16, 45);
            lbFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFont.Name = "lbFont";
            lbFont.Size = new System.Drawing.Size(60, 23);
            lbFont.TabIndex = 20;
            lbFont.Text = "글꼴 : ";
            // 
            // fontButton
            // 
            fontButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            fontButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            fontButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fontButton.ForeColor = System.Drawing.Color.White;
            fontButton.Location = new System.Drawing.Point(82, 41);
            fontButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            fontButton.Name = "fontButton";
            fontButton.Size = new System.Drawing.Size(190, 31);
            fontButton.TabIndex = 9;
            fontButton.Text = "폰트설정";
            fontButton.UseVisualStyleBackColor = false;
            fontButton.Click += fontButton_Click;
            // 
            // lbFontSetting
            // 
            lbFontSetting.AutoSize = true;
            lbFontSetting.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbFontSetting.ForeColor = System.Drawing.Color.White;
            lbFontSetting.Location = new System.Drawing.Point(5, 4);
            lbFontSetting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbFontSetting.Name = "lbFontSetting";
            lbFontSetting.Size = new System.Drawing.Size(88, 25);
            lbFontSetting.TabIndex = 8;
            lbFontSetting.Text = "폰트설정";
            // 
            // tpExtra
            // 
            tpExtra.Controls.Add(panel11);
            tpExtra.Location = new System.Drawing.Point(80, 4);
            tpExtra.Margin = new System.Windows.Forms.Padding(0);
            tpExtra.Name = "tpExtra";
            tpExtra.Size = new System.Drawing.Size(696, 733);
            tpExtra.TabIndex = 2;
            tpExtra.Text = "부가설정";
            tpExtra.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            panel11.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel11.Controls.Add(panel21);
            panel11.Controls.Add(panel25);
            panel11.Controls.Add(panel3);
            panel11.Controls.Add(panel13);
            panel11.Controls.Add(panel12);
            panel11.Controls.Add(panel14);
            panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            panel11.Location = new System.Drawing.Point(0, 0);
            panel11.Margin = new System.Windows.Forms.Padding(0);
            panel11.Name = "panel11";
            panel11.Size = new System.Drawing.Size(696, 733);
            panel11.TabIndex = 1;
            // 
            // panel21
            // 
            panel21.Controls.Add(btAdvencedOption);
            panel21.Controls.Add(lbAdvencedConfig);
            panel21.Location = new System.Drawing.Point(5, 625);
            panel21.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel21.Name = "panel21";
            panel21.Size = new System.Drawing.Size(666, 86);
            panel21.TabIndex = 41;
            panel21.Paint += panealBorder_Paint;
            // 
            // btAdvencedOption
            // 
            btAdvencedOption.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btAdvencedOption.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btAdvencedOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btAdvencedOption.ForeColor = System.Drawing.Color.White;
            btAdvencedOption.Location = new System.Drawing.Point(10, 44);
            btAdvencedOption.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btAdvencedOption.Name = "btAdvencedOption";
            btAdvencedOption.Size = new System.Drawing.Size(645, 31);
            btAdvencedOption.TabIndex = 25;
            btAdvencedOption.Text = "고급 설정";
            btAdvencedOption.UseVisualStyleBackColor = false;
            btAdvencedOption.Click += OnClick_btAdvencedOption;
            // 
            // lbAdvencedConfig
            // 
            lbAdvencedConfig.AutoSize = true;
            lbAdvencedConfig.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbAdvencedConfig.ForeColor = System.Drawing.Color.White;
            lbAdvencedConfig.Location = new System.Drawing.Point(5, 4);
            lbAdvencedConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbAdvencedConfig.Name = "lbAdvencedConfig";
            lbAdvencedConfig.Size = new System.Drawing.Size(95, 25);
            lbAdvencedConfig.TabIndex = 8;
            lbAdvencedConfig.Text = "고급 설정";
            // 
            // panel25
            // 
            panel25.Controls.Add(btSettingUpload);
            panel25.Controls.Add(btSettingBrowser);
            panel25.Controls.Add(lbSearchConfig);
            panel25.Location = new System.Drawing.Point(5, 490);
            panel25.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel25.Name = "panel25";
            panel25.Size = new System.Drawing.Size(666, 128);
            panel25.TabIndex = 40;
            panel25.Paint += panealBorder_Paint;
            // 
            // btSettingUpload
            // 
            btSettingUpload.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btSettingUpload.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btSettingUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btSettingUpload.ForeColor = System.Drawing.Color.White;
            btSettingUpload.Location = new System.Drawing.Point(10, 81);
            btSettingUpload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btSettingUpload.Name = "btSettingUpload";
            btSettingUpload.Size = new System.Drawing.Size(645, 31);
            btSettingUpload.TabIndex = 26;
            btSettingUpload.Text = "설정 업로드";
            btSettingUpload.UseVisualStyleBackColor = false;
            btSettingUpload.Click += OnClick_btSettingUpload;
            // 
            // btSettingBrowser
            // 
            btSettingBrowser.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btSettingBrowser.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btSettingBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btSettingBrowser.ForeColor = System.Drawing.Color.White;
            btSettingBrowser.Location = new System.Drawing.Point(10, 44);
            btSettingBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btSettingBrowser.Name = "btSettingBrowser";
            btSettingBrowser.Size = new System.Drawing.Size(645, 31);
            btSettingBrowser.TabIndex = 25;
            btSettingBrowser.Text = "설정 검색";
            btSettingBrowser.UseVisualStyleBackColor = false;
            btSettingBrowser.Click += Onclick_btSettingBrowser;
            // 
            // lbSearchConfig
            // 
            lbSearchConfig.AutoSize = true;
            lbSearchConfig.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbSearchConfig.ForeColor = System.Drawing.Color.White;
            lbSearchConfig.Location = new System.Drawing.Point(5, 4);
            lbSearchConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbSearchConfig.Name = "lbSearchConfig";
            lbSearchConfig.Size = new System.Drawing.Size(95, 25);
            lbSearchConfig.TabIndex = 8;
            lbSearchConfig.Text = "설정 검색";
            // 
            // panel3
            // 
            panel3.Controls.Add(speedRadioButton5);
            panel3.Controls.Add(lbSpeed);
            panel3.Controls.Add(lbSpeedInformation);
            panel3.Controls.Add(speedRadioButton4);
            panel3.Controls.Add(speedRadioButton1);
            panel3.Controls.Add(speedRadioButton3);
            panel3.Controls.Add(speedRadioButton2);
            panel3.Location = new System.Drawing.Point(4, 214);
            panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(666, 99);
            panel3.TabIndex = 42;
            panel3.Paint += panealBorder_Paint;
            // 
            // speedRadioButton5
            // 
            speedRadioButton5.AutoSize = true;
            speedRadioButton5.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            speedRadioButton5.ForeColor = System.Drawing.Color.White;
            speedRadioButton5.Location = new System.Drawing.Point(508, 34);
            speedRadioButton5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            speedRadioButton5.Name = "speedRadioButton5";
            speedRadioButton5.Size = new System.Drawing.Size(105, 27);
            speedRadioButton5.TabIndex = 9;
            speedRadioButton5.Text = "매우 느림";
            speedRadioButton5.UseVisualStyleBackColor = true;
            // 
            // lbSpeed
            // 
            lbSpeed.AutoSize = true;
            lbSpeed.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbSpeed.ForeColor = System.Drawing.Color.White;
            lbSpeed.Location = new System.Drawing.Point(5, 4);
            lbSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbSpeed.Name = "lbSpeed";
            lbSpeed.Size = new System.Drawing.Size(88, 25);
            lbSpeed.TabIndex = 8;
            lbSpeed.Text = "처리속도";
            // 
            // lbSpeedInformation
            // 
            lbSpeedInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbSpeedInformation.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbSpeedInformation.ForeColor = System.Drawing.Color.White;
            lbSpeedInformation.Location = new System.Drawing.Point(41, 62);
            lbSpeedInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbSpeedInformation.Name = "lbSpeedInformation";
            lbSpeedInformation.Size = new System.Drawing.Size(582, 32);
            lbSpeedInformation.TabIndex = 4;
            lbSpeedInformation.Text = "주의 : 빠름 이상으로 설정할 경우 게임이 느려질 수 있습니다.\r\n";
            lbSpeedInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // speedRadioButton4
            // 
            speedRadioButton4.AutoSize = true;
            speedRadioButton4.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            speedRadioButton4.ForeColor = System.Drawing.Color.White;
            speedRadioButton4.Location = new System.Drawing.Point(398, 34);
            speedRadioButton4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            speedRadioButton4.Name = "speedRadioButton4";
            speedRadioButton4.Size = new System.Drawing.Size(65, 27);
            speedRadioButton4.TabIndex = 3;
            speedRadioButton4.Text = "느림";
            speedRadioButton4.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton1
            // 
            speedRadioButton1.AutoSize = true;
            speedRadioButton1.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            speedRadioButton1.ForeColor = System.Drawing.Color.White;
            speedRadioButton1.Location = new System.Drawing.Point(29, 34);
            speedRadioButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            speedRadioButton1.Name = "speedRadioButton1";
            speedRadioButton1.Size = new System.Drawing.Size(105, 27);
            speedRadioButton1.TabIndex = 0;
            speedRadioButton1.Text = "매우 빠름";
            speedRadioButton1.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton3
            // 
            speedRadioButton3.AutoSize = true;
            speedRadioButton3.Checked = true;
            speedRadioButton3.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            speedRadioButton3.ForeColor = System.Drawing.Color.White;
            speedRadioButton3.Location = new System.Drawing.Point(288, 34);
            speedRadioButton3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            speedRadioButton3.Name = "speedRadioButton3";
            speedRadioButton3.Size = new System.Drawing.Size(65, 27);
            speedRadioButton3.TabIndex = 2;
            speedRadioButton3.TabStop = true;
            speedRadioButton3.Text = "보통";
            speedRadioButton3.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton2
            // 
            speedRadioButton2.AutoSize = true;
            speedRadioButton2.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            speedRadioButton2.ForeColor = System.Drawing.Color.White;
            speedRadioButton2.Location = new System.Drawing.Point(178, 34);
            speedRadioButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            speedRadioButton2.Name = "speedRadioButton2";
            speedRadioButton2.Size = new System.Drawing.Size(65, 27);
            speedRadioButton2.TabIndex = 1;
            speedRadioButton2.Text = "빠름";
            speedRadioButton2.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            panel13.Controls.Add(defaultButton);
            panel13.Controls.Add(saveConfigButton);
            panel13.Controls.Add(openConfigButton);
            panel13.Controls.Add(lbSettingFile);
            panel13.Location = new System.Drawing.Point(4, 319);
            panel13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel13.Name = "panel13";
            panel13.Size = new System.Drawing.Size(666, 164);
            panel13.TabIndex = 39;
            panel13.Paint += panealBorder_Paint;
            // 
            // defaultButton
            // 
            defaultButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            defaultButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            defaultButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            defaultButton.ForeColor = System.Drawing.Color.White;
            defaultButton.Location = new System.Drawing.Point(10, 119);
            defaultButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            defaultButton.Name = "defaultButton";
            defaultButton.Size = new System.Drawing.Size(645, 31);
            defaultButton.TabIndex = 27;
            defaultButton.Text = "초기화";
            defaultButton.UseVisualStyleBackColor = false;
            defaultButton.Click += defaultButton_Click;
            // 
            // saveConfigButton
            // 
            saveConfigButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            saveConfigButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            saveConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            saveConfigButton.ForeColor = System.Drawing.Color.White;
            saveConfigButton.Location = new System.Drawing.Point(10, 81);
            saveConfigButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            saveConfigButton.Name = "saveConfigButton";
            saveConfigButton.Size = new System.Drawing.Size(645, 31);
            saveConfigButton.TabIndex = 26;
            saveConfigButton.Text = "설정 저장하기";
            saveConfigButton.UseVisualStyleBackColor = false;
            saveConfigButton.Click += settingSaveToolStripMenuItem2_Click;
            // 
            // openConfigButton
            // 
            openConfigButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            openConfigButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            openConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            openConfigButton.ForeColor = System.Drawing.Color.White;
            openConfigButton.Location = new System.Drawing.Point(10, 44);
            openConfigButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            openConfigButton.Name = "openConfigButton";
            openConfigButton.Size = new System.Drawing.Size(645, 31);
            openConfigButton.TabIndex = 25;
            openConfigButton.Text = "설정 불러오기";
            openConfigButton.UseVisualStyleBackColor = false;
            openConfigButton.Click += settingLoadToolStripMenuItem2_Click;
            // 
            // lbSettingFile
            // 
            lbSettingFile.AutoSize = true;
            lbSettingFile.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbSettingFile.ForeColor = System.Drawing.Color.White;
            lbSettingFile.Location = new System.Drawing.Point(5, 4);
            lbSettingFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbSettingFile.Name = "lbSettingFile";
            lbSettingFile.Size = new System.Drawing.Size(95, 25);
            lbSettingFile.TabIndex = 8;
            lbSettingFile.Text = "설정 파일";
            // 
            // panel12
            // 
            panel12.Controls.Add(topMostcheckBox);
            panel12.Controls.Add(checkUpdateCheckBox);
            panel12.Controls.Add(label15);
            panel12.Location = new System.Drawing.Point(4, 130);
            panel12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel12.Name = "panel12";
            panel12.Size = new System.Drawing.Size(666, 78);
            panel12.TabIndex = 38;
            panel12.Paint += panealBorder_Paint;
            // 
            // topMostcheckBox
            // 
            topMostcheckBox.AutoSize = true;
            topMostcheckBox.Checked = true;
            topMostcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            topMostcheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            topMostcheckBox.ForeColor = System.Drawing.Color.White;
            topMostcheckBox.Location = new System.Drawing.Point(249, 45);
            topMostcheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            topMostcheckBox.Name = "topMostcheckBox";
            topMostcheckBox.Size = new System.Drawing.Size(157, 27);
            topMostcheckBox.TabIndex = 12;
            topMostcheckBox.Text = "번역창 최상위로";
            topMostcheckBox.UseVisualStyleBackColor = true;
            // 
            // checkUpdateCheckBox
            // 
            checkUpdateCheckBox.AutoSize = true;
            checkUpdateCheckBox.Checked = true;
            checkUpdateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            checkUpdateCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkUpdateCheckBox.ForeColor = System.Drawing.Color.White;
            checkUpdateCheckBox.Location = new System.Drawing.Point(21, 45);
            checkUpdateCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            checkUpdateCheckBox.Name = "checkUpdateCheckBox";
            checkUpdateCheckBox.Size = new System.Drawing.Size(146, 27);
            checkUpdateCheckBox.TabIndex = 11;
            checkUpdateCheckBox.Text = "최신 버전 확인";
            checkUpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label15.ForeColor = System.Drawing.Color.White;
            label15.Location = new System.Drawing.Point(5, 4);
            label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(45, 25);
            label15.TabIndex = 8;
            label15.Text = "ETC";
            // 
            // panel14
            // 
            panel14.Controls.Add(btAttachCapture);
            panel14.Controls.Add(SetDefaultZoomSizeButton);
            panel14.Controls.Add(imgZoomsizeUpDown);
            panel14.Controls.Add(lbImgZoom);
            panel14.Controls.Add(activeWinodeCheckBox);
            panel14.Controls.Add(lbImgCapture);
            panel14.Location = new System.Drawing.Point(4, 4);
            panel14.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel14.Name = "panel14";
            panel14.Size = new System.Drawing.Size(666, 120);
            panel14.TabIndex = 37;
            panel14.Paint += panealBorder_Paint;
            // 
            // btAttachCapture
            // 
            btAttachCapture.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btAttachCapture.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btAttachCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btAttachCapture.ForeColor = System.Drawing.Color.White;
            btAttachCapture.Location = new System.Drawing.Point(11, 81);
            btAttachCapture.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btAttachCapture.Name = "btAttachCapture";
            btAttachCapture.Size = new System.Drawing.Size(644, 31);
            btAttachCapture.TabIndex = 27;
            btAttachCapture.Text = "화면을 가져올 윈도우 지정하기";
            btAttachCapture.UseVisualStyleBackColor = false;
            btAttachCapture.Click += OnClick_btAttachCapture;
            // 
            // SetDefaultZoomSizeButton
            // 
            SetDefaultZoomSizeButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            SetDefaultZoomSizeButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            SetDefaultZoomSizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            SetDefaultZoomSizeButton.ForeColor = System.Drawing.Color.White;
            SetDefaultZoomSizeButton.Location = new System.Drawing.Point(556, 45);
            SetDefaultZoomSizeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            SetDefaultZoomSizeButton.Name = "SetDefaultZoomSizeButton";
            SetDefaultZoomSizeButton.Size = new System.Drawing.Size(70, 29);
            SetDefaultZoomSizeButton.TabIndex = 28;
            SetDefaultZoomSizeButton.Text = "기본값";
            SetDefaultZoomSizeButton.UseVisualStyleBackColor = false;
            SetDefaultZoomSizeButton.Click += SetDefaultZoomSizeButton_Click;
            // 
            // imgZoomsizeUpDown
            // 
            imgZoomsizeUpDown.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            imgZoomsizeUpDown.DecimalPlaces = 1;
            imgZoomsizeUpDown.ForeColor = System.Drawing.Color.White;
            imgZoomsizeUpDown.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            imgZoomsizeUpDown.Location = new System.Drawing.Point(490, 45);
            imgZoomsizeUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            imgZoomsizeUpDown.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            imgZoomsizeUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            imgZoomsizeUpDown.Name = "imgZoomsizeUpDown";
            imgZoomsizeUpDown.Size = new System.Drawing.Size(59, 27);
            imgZoomsizeUpDown.TabIndex = 53;
            imgZoomsizeUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // lbImgZoom
            // 
            lbImgZoom.AutoSize = true;
            lbImgZoom.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbImgZoom.ForeColor = System.Drawing.Color.White;
            lbImgZoom.Location = new System.Drawing.Point(336, 45);
            lbImgZoom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbImgZoom.Name = "lbImgZoom";
            lbImgZoom.Size = new System.Drawing.Size(151, 23);
            lbImgZoom.TabIndex = 51;
            lbImgZoom.Text = "추출 이미지 확대 :";
            // 
            // activeWinodeCheckBox
            // 
            activeWinodeCheckBox.AutoSize = true;
            activeWinodeCheckBox.Checked = true;
            activeWinodeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            activeWinodeCheckBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            activeWinodeCheckBox.ForeColor = System.Drawing.Color.White;
            activeWinodeCheckBox.Location = new System.Drawing.Point(21, 45);
            activeWinodeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            activeWinodeCheckBox.Name = "activeWinodeCheckBox";
            activeWinodeCheckBox.Size = new System.Drawing.Size(288, 27);
            activeWinodeCheckBox.TabIndex = 10;
            activeWinodeCheckBox.Text = "활성화된 윈도우에서 이미지 캡쳐\r\n";
            activeWinodeCheckBox.UseVisualStyleBackColor = true;
            // 
            // lbImgCapture
            // 
            lbImgCapture.AutoSize = true;
            lbImgCapture.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbImgCapture.ForeColor = System.Drawing.Color.White;
            lbImgCapture.Location = new System.Drawing.Point(5, 4);
            lbImgCapture.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbImgCapture.Name = "lbImgCapture";
            lbImgCapture.Size = new System.Drawing.Size(114, 25);
            lbImgCapture.TabIndex = 8;
            lbImgCapture.Text = "이미지 캡쳐";
            // 
            // tpTranslation
            // 
            tpTranslation.Controls.Add(panel19);
            tpTranslation.Location = new System.Drawing.Point(80, 4);
            tpTranslation.Margin = new System.Windows.Forms.Padding(0);
            tpTranslation.Name = "tpTranslation";
            tpTranslation.Size = new System.Drawing.Size(696, 733);
            tpTranslation.TabIndex = 4;
            tpTranslation.Text = "번역설정";
            tpTranslation.UseVisualStyleBackColor = true;
            // 
            // panel19
            // 
            panel19.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel19.Controls.Add(panel4);
            panel19.Controls.Add(panel27);
            panel19.Controls.Add(panel22);
            panel19.Controls.Add(panel1);
            panel19.Controls.Add(panel15);
            panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            panel19.Location = new System.Drawing.Point(0, 0);
            panel19.Margin = new System.Windows.Forms.Padding(0);
            panel19.Name = "panel19";
            panel19.Size = new System.Drawing.Size(696, 733);
            panel19.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(cbDeepLLanguageTo);
            panel4.Controls.Add(lbDeepLTo);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(cbDeepLLanguage);
            panel4.Controls.Add(lbDeepL);
            panel4.Controls.Add(lbDeepLFrom);
            panel4.Location = new System.Drawing.Point(4, 331);
            panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(664, 95);
            panel4.TabIndex = 57;
            panel4.Paint += panealBorder_Paint;
            // 
            // cbDeepLLanguageTo
            // 
            cbDeepLLanguageTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbDeepLLanguageTo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbDeepLLanguageTo.FormattingEnabled = true;
            cbDeepLLanguageTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbDeepLLanguageTo.Items.AddRange(new object[] { "한국어", "영어", "일본어", "중국어 - 간체", "중국어 - 번체", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            cbDeepLLanguageTo.Location = new System.Drawing.Point(380, 44);
            cbDeepLLanguageTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbDeepLLanguageTo.Name = "cbDeepLLanguageTo";
            cbDeepLLanguageTo.Size = new System.Drawing.Size(124, 29);
            cbDeepLLanguageTo.TabIndex = 53;
            // 
            // lbDeepLTo
            // 
            lbDeepLTo.AutoSize = true;
            lbDeepLTo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDeepLTo.ForeColor = System.Drawing.Color.White;
            lbDeepLTo.Location = new System.Drawing.Point(521, 48);
            lbDeepLTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDeepLTo.Name = "lbDeepLTo";
            lbDeepLTo.Size = new System.Drawing.Size(67, 23);
            lbDeepLTo.TabIndex = 52;
            lbDeepLTo.Text = "로 번역";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(311, 48);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(29, 23);
            label6.TabIndex = 51;
            label6.Text = "->";
            // 
            // cbDeepLLanguage
            // 
            cbDeepLLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbDeepLLanguage.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbDeepLLanguage.FormattingEnabled = true;
            cbDeepLLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbDeepLLanguage.Items.AddRange(new object[] { "영어", "일본어", "중국어 간체", "중국어 번체", "한국어", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            cbDeepLLanguage.Location = new System.Drawing.Point(62, 44);
            cbDeepLLanguage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbDeepLLanguage.Name = "cbDeepLLanguage";
            cbDeepLLanguage.Size = new System.Drawing.Size(124, 29);
            cbDeepLLanguage.TabIndex = 50;
            // 
            // lbDeepL
            // 
            lbDeepL.AutoSize = true;
            lbDeepL.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDeepL.ForeColor = System.Drawing.Color.White;
            lbDeepL.Location = new System.Drawing.Point(5, 4);
            lbDeepL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDeepL.Name = "lbDeepL";
            lbDeepL.Size = new System.Drawing.Size(158, 25);
            lbDeepL.TabIndex = 8;
            lbDeepL.Text = "DeepL 번역 설정";
            // 
            // lbDeepLFrom
            // 
            lbDeepLFrom.AutoSize = true;
            lbDeepLFrom.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDeepLFrom.ForeColor = System.Drawing.Color.White;
            lbDeepLFrom.Location = new System.Drawing.Point(212, 48);
            lbDeepLFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDeepLFrom.Name = "lbDeepLFrom";
            lbDeepLFrom.Size = new System.Drawing.Size(44, 23);
            lbDeepLFrom.TabIndex = 49;
            lbDeepLFrom.Text = "에서";
            // 
            // panel27
            // 
            panel27.Controls.Add(cbTTSWaitEnd);
            panel27.Controls.Add(cbUseTTS);
            panel27.Controls.Add(label66);
            panel27.Location = new System.Drawing.Point(4, 431);
            panel27.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel27.Name = "panel27";
            panel27.Size = new System.Drawing.Size(664, 105);
            panel27.TabIndex = 54;
            panel27.Paint += panealBorder_Paint;
            // 
            // cbTTSWaitEnd
            // 
            cbTTSWaitEnd.AutoSize = true;
            cbTTSWaitEnd.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            cbTTSWaitEnd.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbTTSWaitEnd.ForeColor = System.Drawing.Color.White;
            cbTTSWaitEnd.Location = new System.Drawing.Point(169, 32);
            cbTTSWaitEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbTTSWaitEnd.Name = "cbTTSWaitEnd";
            cbTTSWaitEnd.Size = new System.Drawing.Size(226, 27);
            cbTTSWaitEnd.TabIndex = 12;
            cbTTSWaitEnd.Text = "음성이 끝날 때 까지 대기";
            cbTTSWaitEnd.UseVisualStyleBackColor = false;
            // 
            // cbUseTTS
            // 
            cbUseTTS.AutoSize = true;
            cbUseTTS.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbUseTTS.ForeColor = System.Drawing.Color.White;
            cbUseTTS.Location = new System.Drawing.Point(25, 32);
            cbUseTTS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbUseTTS.Name = "cbUseTTS";
            cbUseTTS.Size = new System.Drawing.Size(102, 27);
            cbUseTTS.TabIndex = 9;
            cbUseTTS.Text = "TTS 사용";
            cbUseTTS.UseVisualStyleBackColor = true;
            cbUseTTS.CheckedChanged += cbUseTTS_CheckedChanged;
            // 
            // label66
            // 
            label66.AutoSize = true;
            label66.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label66.ForeColor = System.Drawing.Color.White;
            label66.Location = new System.Drawing.Point(5, 4);
            label66.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label66.Name = "label66";
            label66.Size = new System.Drawing.Size(45, 25);
            label66.TabIndex = 8;
            label66.Text = "TTS";
            // 
            // panel22
            // 
            panel22.Controls.Add(googleResultCodeComboBox);
            panel22.Controls.Add(lbGoogleTo);
            panel22.Controls.Add(label56);
            panel22.Controls.Add(googleTransComboBox);
            panel22.Controls.Add(lbGoogle);
            panel22.Controls.Add(lbGoogleFrom);
            panel22.Location = new System.Drawing.Point(4, 229);
            panel22.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel22.Name = "panel22";
            panel22.Size = new System.Drawing.Size(664, 95);
            panel22.TabIndex = 56;
            panel22.Paint += panealBorder_Paint;
            // 
            // googleResultCodeComboBox
            // 
            googleResultCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            googleResultCodeComboBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            googleResultCodeComboBox.FormattingEnabled = true;
            googleResultCodeComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            googleResultCodeComboBox.Items.AddRange(new object[] { "한국어", "영어", "일본어", "중국어 - 간체", "중국어 - 번체", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            googleResultCodeComboBox.Location = new System.Drawing.Point(380, 44);
            googleResultCodeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            googleResultCodeComboBox.Name = "googleResultCodeComboBox";
            googleResultCodeComboBox.Size = new System.Drawing.Size(124, 29);
            googleResultCodeComboBox.TabIndex = 53;
            // 
            // lbGoogleTo
            // 
            lbGoogleTo.AutoSize = true;
            lbGoogleTo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbGoogleTo.ForeColor = System.Drawing.Color.White;
            lbGoogleTo.Location = new System.Drawing.Point(521, 48);
            lbGoogleTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbGoogleTo.Name = "lbGoogleTo";
            lbGoogleTo.Size = new System.Drawing.Size(67, 23);
            lbGoogleTo.TabIndex = 52;
            lbGoogleTo.Text = "로 번역";
            // 
            // label56
            // 
            label56.AutoSize = true;
            label56.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label56.ForeColor = System.Drawing.Color.White;
            label56.Location = new System.Drawing.Point(311, 48);
            label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label56.Name = "label56";
            label56.Size = new System.Drawing.Size(29, 23);
            label56.TabIndex = 51;
            label56.Text = "->";
            // 
            // googleTransComboBox
            // 
            googleTransComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            googleTransComboBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            googleTransComboBox.FormattingEnabled = true;
            googleTransComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            googleTransComboBox.Items.AddRange(new object[] { "영어", "일본어", "중국어 간체", "중국어 번체", "한국어", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            googleTransComboBox.Location = new System.Drawing.Point(62, 44);
            googleTransComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            googleTransComboBox.Name = "googleTransComboBox";
            googleTransComboBox.Size = new System.Drawing.Size(124, 29);
            googleTransComboBox.TabIndex = 50;
            // 
            // lbGoogle
            // 
            lbGoogle.AutoSize = true;
            lbGoogle.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbGoogle.ForeColor = System.Drawing.Color.White;
            lbGoogle.Location = new System.Drawing.Point(5, 4);
            lbGoogle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbGoogle.Name = "lbGoogle";
            lbGoogle.Size = new System.Drawing.Size(429, 25);
            lbGoogle.TabIndex = 8;
            lbGoogle.Text = "구글 번역 설정 (기본 번역기 , 구글 시트 번역기)";
            // 
            // lbGoogleFrom
            // 
            lbGoogleFrom.AutoSize = true;
            lbGoogleFrom.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbGoogleFrom.ForeColor = System.Drawing.Color.White;
            lbGoogleFrom.Location = new System.Drawing.Point(212, 48);
            lbGoogleFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbGoogleFrom.Name = "lbGoogleFrom";
            lbGoogleFrom.Size = new System.Drawing.Size(44, 23);
            lbGoogleFrom.TabIndex = 49;
            lbGoogleFrom.Text = "에서";
            // 
            // panel1
            // 
            panel1.Controls.Add(skinOverRadioButton);
            panel1.Controls.Add(skinLayerRadioButton);
            panel1.Controls.Add(lbTransformType);
            panel1.Controls.Add(skinDarkRadioButton);
            panel1.Location = new System.Drawing.Point(4, 4);
            panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(664, 61);
            panel1.TabIndex = 55;
            panel1.Paint += panealBorder_Paint;
            // 
            // skinOverRadioButton
            // 
            skinOverRadioButton.AutoSize = true;
            skinOverRadioButton.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            skinOverRadioButton.ForeColor = System.Drawing.Color.White;
            skinOverRadioButton.Location = new System.Drawing.Point(188, 29);
            skinOverRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            skinOverRadioButton.Name = "skinOverRadioButton";
            skinOverRadioButton.Size = new System.Drawing.Size(99, 27);
            skinOverRadioButton.TabIndex = 10;
            skinOverRadioButton.Text = "오버레이";
            skinOverRadioButton.UseVisualStyleBackColor = true;
            // 
            // skinLayerRadioButton
            // 
            skinLayerRadioButton.AutoSize = true;
            skinLayerRadioButton.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            skinLayerRadioButton.ForeColor = System.Drawing.Color.White;
            skinLayerRadioButton.Location = new System.Drawing.Point(96, 29);
            skinLayerRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            skinLayerRadioButton.Name = "skinLayerRadioButton";
            skinLayerRadioButton.Size = new System.Drawing.Size(82, 27);
            skinLayerRadioButton.TabIndex = 9;
            skinLayerRadioButton.Text = "레이어";
            skinLayerRadioButton.UseVisualStyleBackColor = true;
            // 
            // lbTransformType
            // 
            lbTransformType.AutoSize = true;
            lbTransformType.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbTransformType.ForeColor = System.Drawing.Color.White;
            lbTransformType.Location = new System.Drawing.Point(5, 4);
            lbTransformType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbTransformType.Name = "lbTransformType";
            lbTransformType.Size = new System.Drawing.Size(114, 25);
            lbTransformType.TabIndex = 8;
            lbTransformType.Text = "번역창 방식";
            // 
            // skinDarkRadioButton
            // 
            skinDarkRadioButton.AutoSize = true;
            skinDarkRadioButton.Checked = true;
            skinDarkRadioButton.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            skinDarkRadioButton.ForeColor = System.Drawing.Color.White;
            skinDarkRadioButton.Location = new System.Drawing.Point(14, 29);
            skinDarkRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            skinDarkRadioButton.Name = "skinDarkRadioButton";
            skinDarkRadioButton.Size = new System.Drawing.Size(82, 27);
            skinDarkRadioButton.TabIndex = 6;
            skinDarkRadioButton.TabStop = true;
            skinDarkRadioButton.Text = "어두운";
            skinDarkRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel15
            // 
            panel15.Controls.Add(lbPapagoLanguageCodeInformation);
            panel15.Controls.Add(cbNaverResultCode);
            panel15.Controls.Add(lbPaPagoTo);
            panel15.Controls.Add(label43);
            panel15.Controls.Add(naverTransComboBox);
            panel15.Controls.Add(lbPaPago);
            panel15.Controls.Add(lbPaPagoFrom);
            panel15.Location = new System.Drawing.Point(4, 72);
            panel15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel15.Name = "panel15";
            panel15.Size = new System.Drawing.Size(664, 149);
            panel15.TabIndex = 54;
            panel15.Paint += panealBorder_Paint;
            // 
            // lbPapagoLanguageCodeInformation
            // 
            lbPapagoLanguageCodeInformation.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPapagoLanguageCodeInformation.ForeColor = System.Drawing.Color.White;
            lbPapagoLanguageCodeInformation.Location = new System.Drawing.Point(26, 92);
            lbPapagoLanguageCodeInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPapagoLanguageCodeInformation.Name = "lbPapagoLanguageCodeInformation";
            lbPapagoLanguageCodeInformation.Size = new System.Drawing.Size(611, 42);
            lbPapagoLanguageCodeInformation.TabIndex = 56;
            lbPapagoLanguageCodeInformation.Text = "방식에 따라 지원되지 않는 언어가 있습니다\r\n실제 지원하는 언어는 API 문서를 참고하시기 바랍니다";
            lbPapagoLanguageCodeInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbNaverResultCode
            // 
            cbNaverResultCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbNaverResultCode.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbNaverResultCode.FormattingEnabled = true;
            cbNaverResultCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbNaverResultCode.Items.AddRange(new object[] { "한국어", "영어" });
            cbNaverResultCode.Location = new System.Drawing.Point(380, 38);
            cbNaverResultCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbNaverResultCode.Name = "cbNaverResultCode";
            cbNaverResultCode.Size = new System.Drawing.Size(124, 29);
            cbNaverResultCode.TabIndex = 55;
            // 
            // lbPaPagoTo
            // 
            lbPaPagoTo.AutoSize = true;
            lbPaPagoTo.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPaPagoTo.ForeColor = System.Drawing.Color.White;
            lbPaPagoTo.Location = new System.Drawing.Point(521, 41);
            lbPaPagoTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPaPagoTo.Name = "lbPaPagoTo";
            lbPaPagoTo.Size = new System.Drawing.Size(67, 23);
            lbPaPagoTo.TabIndex = 54;
            lbPaPagoTo.Text = "로 번역";
            // 
            // label43
            // 
            label43.AutoSize = true;
            label43.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label43.ForeColor = System.Drawing.Color.White;
            label43.Location = new System.Drawing.Point(311, 48);
            label43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label43.Name = "label43";
            label43.Size = new System.Drawing.Size(29, 23);
            label43.TabIndex = 51;
            label43.Text = "->";
            // 
            // naverTransComboBox
            // 
            naverTransComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            naverTransComboBox.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            naverTransComboBox.FormattingEnabled = true;
            naverTransComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            naverTransComboBox.Items.AddRange(new object[] { "영어", "일본어", "중국어 간체", "중국어 번체", "스페인어", "프랑스어", "베트남어", "태국어", "인도네시아어", "한국어" });
            naverTransComboBox.Location = new System.Drawing.Point(62, 44);
            naverTransComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            naverTransComboBox.Name = "naverTransComboBox";
            naverTransComboBox.Size = new System.Drawing.Size(124, 29);
            naverTransComboBox.TabIndex = 50;
            // 
            // lbPaPago
            // 
            lbPaPago.AutoSize = true;
            lbPaPago.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPaPago.ForeColor = System.Drawing.Color.White;
            lbPaPago.Location = new System.Drawing.Point(5, 4);
            lbPaPago.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPaPago.Name = "lbPaPago";
            lbPaPago.Size = new System.Drawing.Size(230, 25);
            lbPaPago.TabIndex = 8;
            lbPaPago.Text = "네이버(파파고) 번역 설정";
            // 
            // lbPaPagoFrom
            // 
            lbPaPagoFrom.AutoSize = true;
            lbPaPagoFrom.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbPaPagoFrom.ForeColor = System.Drawing.Color.White;
            lbPaPagoFrom.Location = new System.Drawing.Point(212, 48);
            lbPaPagoFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbPaPagoFrom.Name = "lbPaPagoFrom";
            lbPaPagoFrom.Size = new System.Drawing.Size(44, 23);
            lbPaPagoFrom.TabIndex = 49;
            lbPaPagoFrom.Text = "에서";
            // 
            // tpETC
            // 
            tpETC.Controls.Add(panel18);
            tpETC.Location = new System.Drawing.Point(80, 4);
            tpETC.Margin = new System.Windows.Forms.Padding(0);
            tpETC.Name = "tpETC";
            tpETC.Size = new System.Drawing.Size(696, 733);
            tpETC.TabIndex = 3;
            tpETC.Text = "그 외";
            tpETC.UseVisualStyleBackColor = true;
            // 
            // panel18
            // 
            panel18.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel18.Controls.Add(panel16);
            panel18.Controls.Add(panel20);
            panel18.Controls.Add(panel23);
            panel18.Dock = System.Windows.Forms.DockStyle.Fill;
            panel18.Location = new System.Drawing.Point(0, 0);
            panel18.Margin = new System.Windows.Forms.Padding(0);
            panel18.Name = "panel18";
            panel18.Size = new System.Drawing.Size(696, 733);
            panel18.TabIndex = 2;
            // 
            // panel16
            // 
            panel16.Controls.Add(btnOpenDiscord);
            panel16.Controls.Add(openBlogButton);
            panel16.Controls.Add(btnGitHub);
            panel16.Controls.Add(lbLink);
            panel16.Location = new System.Drawing.Point(4, 472);
            panel16.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel16.Name = "panel16";
            panel16.Size = new System.Drawing.Size(664, 160);
            panel16.TabIndex = 42;
            panel16.Paint += panealBorder_Paint;
            // 
            // btnOpenDiscord
            // 
            btnOpenDiscord.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnOpenDiscord.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnOpenDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOpenDiscord.ForeColor = System.Drawing.Color.White;
            btnOpenDiscord.Location = new System.Drawing.Point(9, 119);
            btnOpenDiscord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnOpenDiscord.Name = "btnOpenDiscord";
            btnOpenDiscord.Size = new System.Drawing.Size(645, 31);
            btnOpenDiscord.TabIndex = 26;
            btnOpenDiscord.Text = "디스코드 채널로 이동";
            btnOpenDiscord.UseVisualStyleBackColor = false;
            btnOpenDiscord.Click += OnClickOpenDiscord;
            // 
            // openBlogButton
            // 
            openBlogButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            openBlogButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            openBlogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            openBlogButton.ForeColor = System.Drawing.Color.White;
            openBlogButton.Location = new System.Drawing.Point(10, 81);
            openBlogButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            openBlogButton.Name = "openBlogButton";
            openBlogButton.Size = new System.Drawing.Size(645, 31);
            openBlogButton.TabIndex = 25;
            openBlogButton.Text = "블로그 방문";
            openBlogButton.UseVisualStyleBackColor = false;
            openBlogButton.Click += OnClickopenBlog;
            // 
            // btnGitHub
            // 
            btnGitHub.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnGitHub.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnGitHub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGitHub.ForeColor = System.Drawing.Color.White;
            btnGitHub.Location = new System.Drawing.Point(10, 44);
            btnGitHub.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnGitHub.Name = "btnGitHub";
            btnGitHub.Size = new System.Drawing.Size(645, 31);
            btnGitHub.TabIndex = 25;
            btnGitHub.Text = "Github로 이동";
            btnGitHub.UseVisualStyleBackColor = false;
            btnGitHub.Click += OnClick_GitHub;
            // 
            // lbLink
            // 
            lbLink.AutoSize = true;
            lbLink.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbLink.ForeColor = System.Drawing.Color.White;
            lbLink.Location = new System.Drawing.Point(5, 4);
            lbLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbLink.Name = "lbLink";
            lbLink.Size = new System.Drawing.Size(50, 25);
            lbLink.TabIndex = 8;
            lbLink.Text = "링크";
            // 
            // panel20
            // 
            panel20.Controls.Add(about_Button);
            panel20.Controls.Add(error_Information_Button);
            panel20.Controls.Add(help_Button);
            panel20.Controls.Add(lbETC);
            panel20.Location = new System.Drawing.Point(4, 302);
            panel20.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel20.Name = "panel20";
            panel20.Size = new System.Drawing.Size(664, 162);
            panel20.TabIndex = 41;
            panel20.Paint += panealBorder_Paint;
            // 
            // about_Button
            // 
            about_Button.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            about_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            about_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            about_Button.ForeColor = System.Drawing.Color.White;
            about_Button.Location = new System.Drawing.Point(10, 119);
            about_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            about_Button.Name = "about_Button";
            about_Button.Size = new System.Drawing.Size(645, 31);
            about_Button.TabIndex = 26;
            about_Button.Text = "About";
            about_Button.UseVisualStyleBackColor = false;
            about_Button.Click += about_Button_Click;
            // 
            // error_Information_Button
            // 
            error_Information_Button.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            error_Information_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            error_Information_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            error_Information_Button.ForeColor = System.Drawing.Color.White;
            error_Information_Button.Location = new System.Drawing.Point(10, 81);
            error_Information_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            error_Information_Button.Name = "error_Information_Button";
            error_Information_Button.Size = new System.Drawing.Size(645, 31);
            error_Information_Button.TabIndex = 25;
            error_Information_Button.Text = "에러 메시지 목록";
            error_Information_Button.UseVisualStyleBackColor = false;
            error_Information_Button.Click += error_Information_Button_Click;
            // 
            // help_Button
            // 
            help_Button.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            help_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            help_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            help_Button.ForeColor = System.Drawing.Color.White;
            help_Button.Location = new System.Drawing.Point(10, 44);
            help_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            help_Button.Name = "help_Button";
            help_Button.Size = new System.Drawing.Size(645, 31);
            help_Button.TabIndex = 25;
            help_Button.Text = "MORT 사용법";
            help_Button.UseVisualStyleBackColor = false;
            help_Button.Click += help_Button_Click;
            // 
            // lbETC
            // 
            lbETC.AutoSize = true;
            lbETC.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbETC.ForeColor = System.Drawing.Color.White;
            lbETC.Location = new System.Drawing.Point(5, 4);
            lbETC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbETC.Name = "lbETC";
            lbETC.Size = new System.Drawing.Size(57, 25);
            lbETC.TabIndex = 8;
            lbETC.Text = "그 외";
            // 
            // panel23
            // 
            panel23.Controls.Add(panel2);
            panel23.Controls.Add(lbHotKeyHideTransWindow);
            panel23.Controls.Add(lbHotKeyOnceTranslate);
            panel23.Controls.Add(lbHotKeySnapShot);
            panel23.Controls.Add(lbHotKeyInformation);
            panel23.Controls.Add(lbHotKeyQuickOCR);
            panel23.Controls.Add(lbHotKeyDic);
            panel23.Controls.Add(lbHotKeyDoTrans);
            panel23.Controls.Add(lbHotkey);
            panel23.Location = new System.Drawing.Point(4, 4);
            panel23.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel23.Name = "panel23";
            panel23.Size = new System.Drawing.Size(664, 299);
            panel23.TabIndex = 37;
            panel23.Paint += panealBorder_Paint;
            // 
            // panel2
            // 
            panel2.Controls.Add(transKeyInputLabel);
            panel2.Controls.Add(btnHideTransEmpty);
            panel2.Controls.Add(dicKeyInputLabel);
            panel2.Controls.Add(btnHideTransDefault);
            panel2.Controls.Add(quickKeyInputLabel);
            panel2.Controls.Add(lbHideTranslate);
            panel2.Controls.Add(transKeyInputResetButton);
            panel2.Controls.Add(dicKeyInputResetButton);
            panel2.Controls.Add(btnOneTransEmpty);
            panel2.Controls.Add(quickKeyInputResetButton);
            panel2.Controls.Add(btnOneTransDefault);
            panel2.Controls.Add(transKeyInputEmptyButton);
            panel2.Controls.Add(lbOneTrans);
            panel2.Controls.Add(dicKeyInputEmptyButton);
            panel2.Controls.Add(quickKeyInputEmptyButton);
            panel2.Controls.Add(snapShotKeyInputEmptyButton);
            panel2.Controls.Add(snapShotInputLabel);
            panel2.Controls.Add(snapShotKeyInputResetButton);
            panel2.Location = new System.Drawing.Point(161, 30);
            panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(451, 230);
            panel2.TabIndex = 62;
            // 
            // transKeyInputLabel
            // 
            transKeyInputLabel.Location = new System.Drawing.Point(20, 18);
            transKeyInputLabel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            transKeyInputLabel.Name = "transKeyInputLabel";
            transKeyInputLabel.Size = new System.Drawing.Size(248, 26);
            transKeyInputLabel.TabIndex = 26;
            // 
            // btnHideTransEmpty
            // 
            btnHideTransEmpty.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnHideTransEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnHideTransEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHideTransEmpty.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnHideTransEmpty.ForeColor = System.Drawing.Color.White;
            btnHideTransEmpty.Location = new System.Drawing.Point(370, 191);
            btnHideTransEmpty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnHideTransEmpty.Name = "btnHideTransEmpty";
            btnHideTransEmpty.Size = new System.Drawing.Size(70, 29);
            btnHideTransEmpty.TabIndex = 61;
            btnHideTransEmpty.Text = "비우기";
            btnHideTransEmpty.UseVisualStyleBackColor = false;
            btnHideTransEmpty.Click += btnHideTransEmpty_Click;
            // 
            // dicKeyInputLabel
            // 
            dicKeyInputLabel.Location = new System.Drawing.Point(20, 51);
            dicKeyInputLabel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            dicKeyInputLabel.Name = "dicKeyInputLabel";
            dicKeyInputLabel.Size = new System.Drawing.Size(248, 26);
            dicKeyInputLabel.TabIndex = 28;
            // 
            // btnHideTransDefault
            // 
            btnHideTransDefault.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnHideTransDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnHideTransDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHideTransDefault.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnHideTransDefault.ForeColor = System.Drawing.Color.White;
            btnHideTransDefault.Location = new System.Drawing.Point(292, 191);
            btnHideTransDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnHideTransDefault.Name = "btnHideTransDefault";
            btnHideTransDefault.Size = new System.Drawing.Size(70, 29);
            btnHideTransDefault.TabIndex = 60;
            btnHideTransDefault.Text = "기본값";
            btnHideTransDefault.UseVisualStyleBackColor = false;
            btnHideTransDefault.Click += btnHideTransDefault_Click;
            // 
            // quickKeyInputLabel
            // 
            quickKeyInputLabel.Location = new System.Drawing.Point(20, 85);
            quickKeyInputLabel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            quickKeyInputLabel.Name = "quickKeyInputLabel";
            quickKeyInputLabel.Size = new System.Drawing.Size(248, 26);
            quickKeyInputLabel.TabIndex = 30;
            // 
            // lbHideTranslate
            // 
            lbHideTranslate.Location = new System.Drawing.Point(20, 186);
            lbHideTranslate.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            lbHideTranslate.Name = "lbHideTranslate";
            lbHideTranslate.Size = new System.Drawing.Size(248, 32);
            lbHideTranslate.TabIndex = 59;
            // 
            // transKeyInputResetButton
            // 
            transKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            transKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            transKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            transKeyInputResetButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            transKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            transKeyInputResetButton.Location = new System.Drawing.Point(292, 14);
            transKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            transKeyInputResetButton.Name = "transKeyInputResetButton";
            transKeyInputResetButton.Size = new System.Drawing.Size(70, 29);
            transKeyInputResetButton.TabIndex = 44;
            transKeyInputResetButton.Text = "기본값";
            transKeyInputResetButton.UseVisualStyleBackColor = false;
            transKeyInputResetButton.Click += transKeyInputResetButton_Click;
            // 
            // dicKeyInputResetButton
            // 
            dicKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            dicKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            dicKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            dicKeyInputResetButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dicKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            dicKeyInputResetButton.Location = new System.Drawing.Point(292, 48);
            dicKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            dicKeyInputResetButton.Name = "dicKeyInputResetButton";
            dicKeyInputResetButton.Size = new System.Drawing.Size(70, 29);
            dicKeyInputResetButton.TabIndex = 45;
            dicKeyInputResetButton.Text = "기본값";
            dicKeyInputResetButton.UseVisualStyleBackColor = false;
            dicKeyInputResetButton.Click += dicKeyInputResetButton_Click;
            // 
            // btnOneTransEmpty
            // 
            btnOneTransEmpty.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnOneTransEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnOneTransEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOneTransEmpty.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnOneTransEmpty.ForeColor = System.Drawing.Color.White;
            btnOneTransEmpty.Location = new System.Drawing.Point(370, 155);
            btnOneTransEmpty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnOneTransEmpty.Name = "btnOneTransEmpty";
            btnOneTransEmpty.Size = new System.Drawing.Size(70, 29);
            btnOneTransEmpty.TabIndex = 57;
            btnOneTransEmpty.Text = "비우기";
            btnOneTransEmpty.UseVisualStyleBackColor = false;
            btnOneTransEmpty.Click += btnOneTransEmpty_Click;
            // 
            // quickKeyInputResetButton
            // 
            quickKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            quickKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            quickKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            quickKeyInputResetButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            quickKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            quickKeyInputResetButton.Location = new System.Drawing.Point(292, 81);
            quickKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            quickKeyInputResetButton.Name = "quickKeyInputResetButton";
            quickKeyInputResetButton.Size = new System.Drawing.Size(70, 29);
            quickKeyInputResetButton.TabIndex = 46;
            quickKeyInputResetButton.Text = "기본값";
            quickKeyInputResetButton.UseVisualStyleBackColor = false;
            quickKeyInputResetButton.Click += quickKeyInputResetButton_Click;
            // 
            // btnOneTransDefault
            // 
            btnOneTransDefault.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnOneTransDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnOneTransDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOneTransDefault.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnOneTransDefault.ForeColor = System.Drawing.Color.White;
            btnOneTransDefault.Location = new System.Drawing.Point(292, 155);
            btnOneTransDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnOneTransDefault.Name = "btnOneTransDefault";
            btnOneTransDefault.Size = new System.Drawing.Size(70, 29);
            btnOneTransDefault.TabIndex = 56;
            btnOneTransDefault.Text = "기본값";
            btnOneTransDefault.UseVisualStyleBackColor = false;
            btnOneTransDefault.Click += btnOneTransDefault_Click;
            // 
            // transKeyInputEmptyButton
            // 
            transKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            transKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            transKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            transKeyInputEmptyButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            transKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            transKeyInputEmptyButton.Location = new System.Drawing.Point(370, 14);
            transKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            transKeyInputEmptyButton.Name = "transKeyInputEmptyButton";
            transKeyInputEmptyButton.Size = new System.Drawing.Size(70, 29);
            transKeyInputEmptyButton.TabIndex = 47;
            transKeyInputEmptyButton.Text = "비우기";
            transKeyInputEmptyButton.UseVisualStyleBackColor = false;
            transKeyInputEmptyButton.Click += transKeyInputEmptyButton_Click;
            // 
            // lbOneTrans
            // 
            lbOneTrans.Location = new System.Drawing.Point(20, 152);
            lbOneTrans.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            lbOneTrans.Name = "lbOneTrans";
            lbOneTrans.Size = new System.Drawing.Size(248, 26);
            lbOneTrans.TabIndex = 55;
            // 
            // dicKeyInputEmptyButton
            // 
            dicKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            dicKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            dicKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            dicKeyInputEmptyButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dicKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            dicKeyInputEmptyButton.Location = new System.Drawing.Point(370, 48);
            dicKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            dicKeyInputEmptyButton.Name = "dicKeyInputEmptyButton";
            dicKeyInputEmptyButton.Size = new System.Drawing.Size(70, 29);
            dicKeyInputEmptyButton.TabIndex = 48;
            dicKeyInputEmptyButton.Text = "비우기";
            dicKeyInputEmptyButton.UseVisualStyleBackColor = false;
            dicKeyInputEmptyButton.Click += dicKeyInputEmptyButton_Click;
            // 
            // quickKeyInputEmptyButton
            // 
            quickKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            quickKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            quickKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            quickKeyInputEmptyButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            quickKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            quickKeyInputEmptyButton.Location = new System.Drawing.Point(370, 81);
            quickKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            quickKeyInputEmptyButton.Name = "quickKeyInputEmptyButton";
            quickKeyInputEmptyButton.Size = new System.Drawing.Size(70, 29);
            quickKeyInputEmptyButton.TabIndex = 49;
            quickKeyInputEmptyButton.Text = "비우기";
            quickKeyInputEmptyButton.UseVisualStyleBackColor = false;
            quickKeyInputEmptyButton.Click += quickKeyInputEmptyButton_Click;
            // 
            // snapShotKeyInputEmptyButton
            // 
            snapShotKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            snapShotKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            snapShotKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            snapShotKeyInputEmptyButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            snapShotKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            snapShotKeyInputEmptyButton.Location = new System.Drawing.Point(370, 119);
            snapShotKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            snapShotKeyInputEmptyButton.Name = "snapShotKeyInputEmptyButton";
            snapShotKeyInputEmptyButton.Size = new System.Drawing.Size(70, 29);
            snapShotKeyInputEmptyButton.TabIndex = 53;
            snapShotKeyInputEmptyButton.Text = "비우기";
            snapShotKeyInputEmptyButton.UseVisualStyleBackColor = false;
            snapShotKeyInputEmptyButton.Click += snapShotKeyInputEmptyButton_Click;
            // 
            // snapShotInputLabel
            // 
            snapShotInputLabel.Location = new System.Drawing.Point(20, 119);
            snapShotInputLabel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            snapShotInputLabel.Name = "snapShotInputLabel";
            snapShotInputLabel.Size = new System.Drawing.Size(248, 26);
            snapShotInputLabel.TabIndex = 51;
            // 
            // snapShotKeyInputResetButton
            // 
            snapShotKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            snapShotKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            snapShotKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            snapShotKeyInputResetButton.Font = new System.Drawing.Font("Malgun Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            snapShotKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            snapShotKeyInputResetButton.Location = new System.Drawing.Point(292, 119);
            snapShotKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            snapShotKeyInputResetButton.Name = "snapShotKeyInputResetButton";
            snapShotKeyInputResetButton.Size = new System.Drawing.Size(70, 29);
            snapShotKeyInputResetButton.TabIndex = 52;
            snapShotKeyInputResetButton.Text = "기본값";
            snapShotKeyInputResetButton.UseVisualStyleBackColor = false;
            snapShotKeyInputResetButton.Click += snapShotKeyInputResetButton_Click;
            // 
            // lbHotKeyHideTransWindow
            // 
            lbHotKeyHideTransWindow.AutoSize = true;
            lbHotKeyHideTransWindow.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeyHideTransWindow.ForeColor = System.Drawing.Color.White;
            lbHotKeyHideTransWindow.Location = new System.Drawing.Point(18, 220);
            lbHotKeyHideTransWindow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeyHideTransWindow.Name = "lbHotKeyHideTransWindow";
            lbHotKeyHideTransWindow.Size = new System.Drawing.Size(134, 23);
            lbHotKeyHideTransWindow.TabIndex = 58;
            lbHotKeyHideTransWindow.Text = "번역창 숨기기 : ";
            // 
            // lbHotKeyOnceTranslate
            // 
            lbHotKeyOnceTranslate.AutoSize = true;
            lbHotKeyOnceTranslate.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeyOnceTranslate.ForeColor = System.Drawing.Color.White;
            lbHotKeyOnceTranslate.Location = new System.Drawing.Point(18, 181);
            lbHotKeyOnceTranslate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeyOnceTranslate.Name = "lbHotKeyOnceTranslate";
            lbHotKeyOnceTranslate.Size = new System.Drawing.Size(117, 23);
            lbHotKeyOnceTranslate.TabIndex = 54;
            lbHotKeyOnceTranslate.Text = "한 번만 번역 :";
            // 
            // lbHotKeySnapShot
            // 
            lbHotKeySnapShot.AutoSize = true;
            lbHotKeySnapShot.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeySnapShot.ForeColor = System.Drawing.Color.White;
            lbHotKeySnapShot.Location = new System.Drawing.Point(18, 148);
            lbHotKeySnapShot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeySnapShot.Name = "lbHotKeySnapShot";
            lbHotKeySnapShot.Size = new System.Drawing.Size(71, 23);
            lbHotKeySnapShot.TabIndex = 50;
            lbHotKeySnapShot.Text = "스냅샷 :";
            // 
            // lbHotKeyInformation
            // 
            lbHotKeyInformation.Anchor = System.Windows.Forms.AnchorStyles.None;
            lbHotKeyInformation.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeyInformation.ForeColor = System.Drawing.Color.White;
            lbHotKeyInformation.Location = new System.Drawing.Point(21, 262);
            lbHotKeyInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeyInformation.Name = "lbHotKeyInformation";
            lbHotKeyInformation.Size = new System.Drawing.Size(620, 32);
            lbHotKeyInformation.TabIndex = 43;
            lbHotKeyInformation.Text = "ESC, 백스페이바로 비울 수 있습니다.";
            lbHotKeyInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbHotKeyQuickOCR
            // 
            lbHotKeyQuickOCR.AutoSize = true;
            lbHotKeyQuickOCR.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeyQuickOCR.ForeColor = System.Drawing.Color.White;
            lbHotKeyQuickOCR.Location = new System.Drawing.Point(18, 114);
            lbHotKeyQuickOCR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeyQuickOCR.Name = "lbHotKeyQuickOCR";
            lbHotKeyQuickOCR.Size = new System.Drawing.Size(134, 23);
            lbHotKeyQuickOCR.TabIndex = 29;
            lbHotKeyQuickOCR.Text = "빠른 OCR 영역 :";
            // 
            // lbHotKeyDic
            // 
            lbHotKeyDic.AutoSize = true;
            lbHotKeyDic.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeyDic.ForeColor = System.Drawing.Color.White;
            lbHotKeyDic.Location = new System.Drawing.Point(18, 80);
            lbHotKeyDic.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeyDic.Name = "lbHotKeyDic";
            lbHotKeyDic.Size = new System.Drawing.Size(140, 23);
            lbHotKeyDic.TabIndex = 27;
            lbHotKeyDic.Text = "교정 사전 열기 : ";
            // 
            // lbHotKeyDoTrans
            // 
            lbHotKeyDoTrans.AutoSize = true;
            lbHotKeyDoTrans.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbHotKeyDoTrans.ForeColor = System.Drawing.Color.White;
            lbHotKeyDoTrans.Location = new System.Drawing.Point(18, 46);
            lbHotKeyDoTrans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotKeyDoTrans.Name = "lbHotKeyDoTrans";
            lbHotKeyDoTrans.Size = new System.Drawing.Size(141, 23);
            lbHotKeyDoTrans.TabIndex = 25;
            lbHotKeyDoTrans.Text = "번역 시작/중지 : ";
            // 
            // lbHotkey
            // 
            lbHotkey.AutoSize = true;
            lbHotkey.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbHotkey.ForeColor = System.Drawing.Color.White;
            lbHotkey.Location = new System.Drawing.Point(5, 4);
            lbHotkey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbHotkey.Name = "lbHotkey";
            lbHotkey.Size = new System.Drawing.Size(69, 25);
            lbHotkey.TabIndex = 8;
            lbHotkey.Text = "단축키";
            // 
            // tpQuickSetting
            // 
            tpQuickSetting.Controls.Add(panel28);
            tpQuickSetting.Location = new System.Drawing.Point(80, 4);
            tpQuickSetting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tpQuickSetting.Name = "tpQuickSetting";
            tpQuickSetting.Size = new System.Drawing.Size(696, 733);
            tpQuickSetting.TabIndex = 6;
            tpQuickSetting.Text = "빠른설정";
            tpQuickSetting.UseVisualStyleBackColor = true;
            // 
            // panel28
            // 
            panel28.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel28.Controls.Add(panel31);
            panel28.Dock = System.Windows.Forms.DockStyle.Fill;
            panel28.Location = new System.Drawing.Point(0, 0);
            panel28.Margin = new System.Windows.Forms.Padding(0);
            panel28.Name = "panel28";
            panel28.Size = new System.Drawing.Size(696, 733);
            panel28.TabIndex = 3;
            // 
            // panel31
            // 
            panel31.Controls.Add(cbSetBasicDefaultPage);
            panel31.Controls.Add(btQuickJap);
            panel31.Controls.Add(lbQuickSettingInformation);
            panel31.Controls.Add(btQucickEnglish);
            panel31.Controls.Add(lbQuickSetting);
            panel31.Location = new System.Drawing.Point(4, 4);
            panel31.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel31.Name = "panel31";
            panel31.Size = new System.Drawing.Size(666, 694);
            panel31.TabIndex = 55;
            panel31.Paint += panealBorder_Paint;
            // 
            // cbSetBasicDefaultPage
            // 
            cbSetBasicDefaultPage.AutoSize = true;
            cbSetBasicDefaultPage.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbSetBasicDefaultPage.ForeColor = System.Drawing.Color.White;
            cbSetBasicDefaultPage.Location = new System.Drawing.Point(25, 376);
            cbSetBasicDefaultPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbSetBasicDefaultPage.Name = "cbSetBasicDefaultPage";
            cbSetBasicDefaultPage.Size = new System.Drawing.Size(294, 27);
            cbSetBasicDefaultPage.TabIndex = 18;
            cbSetBasicDefaultPage.Text = "기본설정 탭을 시작 화면으로 설정";
            cbSetBasicDefaultPage.UseVisualStyleBackColor = true;
            // 
            // btQuickJap
            // 
            btQuickJap.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btQuickJap.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btQuickJap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btQuickJap.ForeColor = System.Drawing.Color.White;
            btQuickJap.Location = new System.Drawing.Point(25, 206);
            btQuickJap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btQuickJap.Name = "btQuickJap";
            btQuickJap.Size = new System.Drawing.Size(616, 76);
            btQuickJap.TabIndex = 12;
            btQuickJap.Text = "일본어 게임";
            btQuickJap.UseVisualStyleBackColor = false;
            btQuickJap.Click += OnClickQuickJap;
            // 
            // lbQuickSettingInformation
            // 
            lbQuickSettingInformation.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbQuickSettingInformation.ForeColor = System.Drawing.Color.White;
            lbQuickSettingInformation.Location = new System.Drawing.Point(25, 324);
            lbQuickSettingInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbQuickSettingInformation.Name = "lbQuickSettingInformation";
            lbQuickSettingInformation.Size = new System.Drawing.Size(612, 29);
            lbQuickSettingInformation.TabIndex = 11;
            lbQuickSettingInformation.Text = "처음 사용자를 위한 설정값을 불러옵니다";
            lbQuickSettingInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btQucickEnglish
            // 
            btQucickEnglish.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btQucickEnglish.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btQucickEnglish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btQucickEnglish.ForeColor = System.Drawing.Color.White;
            btQucickEnglish.Location = new System.Drawing.Point(25, 94);
            btQucickEnglish.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btQucickEnglish.Name = "btQucickEnglish";
            btQucickEnglish.Size = new System.Drawing.Size(616, 76);
            btQucickEnglish.TabIndex = 10;
            btQucickEnglish.Text = "영문 게임";
            btQucickEnglish.UseVisualStyleBackColor = false;
            btQucickEnglish.Click += OnClickQucickEnglish;
            // 
            // lbQuickSetting
            // 
            lbQuickSetting.AutoSize = true;
            lbQuickSetting.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbQuickSetting.ForeColor = System.Drawing.Color.White;
            lbQuickSetting.Location = new System.Drawing.Point(5, 4);
            lbQuickSetting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbQuickSetting.Name = "lbQuickSetting";
            lbQuickSetting.Size = new System.Drawing.Size(244, 25);
            lbQuickSetting.TabIndex = 8;
            lbQuickSetting.Text = "어느 게임을 번역하시나요?";
            // 
            // tpDebuging
            // 
            tpDebuging.Controls.Add(panel24);
            tpDebuging.Location = new System.Drawing.Point(80, 4);
            tpDebuging.Margin = new System.Windows.Forms.Padding(0);
            tpDebuging.Name = "tpDebuging";
            tpDebuging.Size = new System.Drawing.Size(696, 733);
            tpDebuging.TabIndex = 5;
            tpDebuging.Text = "디버깅";
            tpDebuging.UseVisualStyleBackColor = true;
            // 
            // panel24
            // 
            panel24.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel24.Controls.Add(panel26);
            panel24.Dock = System.Windows.Forms.DockStyle.Fill;
            panel24.Location = new System.Drawing.Point(0, 0);
            panel24.Margin = new System.Windows.Forms.Padding(0);
            panel24.Name = "panel24";
            panel24.Size = new System.Drawing.Size(696, 733);
            panel24.TabIndex = 3;
            // 
            // panel26
            // 
            panel26.Controls.Add(plDebugOn);
            panel26.Controls.Add(plDebugOff);
            panel26.Controls.Add(lbDebugging);
            panel26.Location = new System.Drawing.Point(4, 4);
            panel26.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel26.Name = "panel26";
            panel26.Size = new System.Drawing.Size(666, 694);
            panel26.TabIndex = 37;
            panel26.Paint += panealBorder_Paint;
            // 
            // plDebugOn
            // 
            plDebugOn.Controls.Add(cbShowOverlayWordArea);
            plDebugOn.Controls.Add(cbSetLineTrans);
            plDebugOn.Controls.Add(btClearFormerResult);
            plDebugOn.Controls.Add(cbShowFormerLog);
            plDebugOn.Controls.Add(cbUnlockSpeed);
            plDebugOn.Controls.Add(cbSaveCaptureResult);
            plDebugOn.Controls.Add(cbSaveCapture);
            plDebugOn.Controls.Add(cbShowReplace);
            plDebugOn.Location = new System.Drawing.Point(4, 32);
            plDebugOn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            plDebugOn.Name = "plDebugOn";
            plDebugOn.Size = new System.Drawing.Size(634, 640);
            plDebugOn.TabIndex = 56;
            // 
            // cbShowOverlayWordArea
            // 
            cbShowOverlayWordArea.AutoSize = true;
            cbShowOverlayWordArea.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbShowOverlayWordArea.ForeColor = System.Drawing.Color.White;
            cbShowOverlayWordArea.Location = new System.Drawing.Point(18, 266);
            cbShowOverlayWordArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbShowOverlayWordArea.Name = "cbShowOverlayWordArea";
            cbShowOverlayWordArea.Size = new System.Drawing.Size(307, 27);
            cbShowOverlayWordArea.TabIndex = 28;
            cbShowOverlayWordArea.Text = "오버레이 번역창 - 문자 영역 보이기";
            cbShowOverlayWordArea.UseVisualStyleBackColor = true;
            cbShowOverlayWordArea.CheckedChanged += cbShowOverlayWordArea_CheckedChanged;
            // 
            // cbSetLineTrans
            // 
            cbSetLineTrans.AutoSize = true;
            cbSetLineTrans.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbSetLineTrans.ForeColor = System.Drawing.Color.White;
            cbSetLineTrans.Location = new System.Drawing.Point(18, 232);
            cbSetLineTrans.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbSetLineTrans.Name = "cbSetLineTrans";
            cbSetLineTrans.Size = new System.Drawing.Size(443, 27);
            cbSetLineTrans.TabIndex = 27;
            cbSetLineTrans.Text = "줄 단위로 번역하기 (문장단위로 번역하기 사용 안 함)";
            cbSetLineTrans.UseVisualStyleBackColor = true;
            cbSetLineTrans.CheckedChanged += cbSetLineTrans_CheckedChanged;
            // 
            // btClearFormerResult
            // 
            btClearFormerResult.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btClearFormerResult.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btClearFormerResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btClearFormerResult.ForeColor = System.Drawing.Color.White;
            btClearFormerResult.Location = new System.Drawing.Point(18, 194);
            btClearFormerResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btClearFormerResult.Name = "btClearFormerResult";
            btClearFormerResult.Size = new System.Drawing.Size(609, 31);
            btClearFormerResult.TabIndex = 26;
            btClearFormerResult.Text = "번역 기억하기 모두 삭제";
            btClearFormerResult.UseVisualStyleBackColor = false;
            btClearFormerResult.Click += btClearFormerResult_Click;
            // 
            // cbShowFormerLog
            // 
            cbShowFormerLog.AutoSize = true;
            cbShowFormerLog.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbShowFormerLog.ForeColor = System.Drawing.Color.White;
            cbShowFormerLog.Location = new System.Drawing.Point(18, 160);
            cbShowFormerLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbShowFormerLog.Name = "cbShowFormerLog";
            cbShowFormerLog.Size = new System.Drawing.Size(220, 27);
            cbShowFormerLog.TabIndex = 15;
            cbShowFormerLog.Text = "번역 기억하기 결과 출력";
            cbShowFormerLog.UseVisualStyleBackColor = true;
            cbShowFormerLog.CheckedChanged += cbShowFormerLog_CheckedChanged;
            // 
            // cbUnlockSpeed
            // 
            cbUnlockSpeed.AutoSize = true;
            cbUnlockSpeed.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbUnlockSpeed.ForeColor = System.Drawing.Color.White;
            cbUnlockSpeed.Location = new System.Drawing.Point(18, 126);
            cbUnlockSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbUnlockSpeed.Name = "cbUnlockSpeed";
            cbUnlockSpeed.Size = new System.Drawing.Size(186, 27);
            cbUnlockSpeed.TabIndex = 14;
            cbUnlockSpeed.Text = "번역 속도 제한 해제";
            cbUnlockSpeed.UseVisualStyleBackColor = true;
            cbUnlockSpeed.CheckedChanged += cbUnlockSpeed_CheckedChanged;
            // 
            // cbSaveCaptureResult
            // 
            cbSaveCaptureResult.AutoSize = true;
            cbSaveCaptureResult.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbSaveCaptureResult.ForeColor = System.Drawing.Color.White;
            cbSaveCaptureResult.Location = new System.Drawing.Point(18, 92);
            cbSaveCaptureResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbSaveCaptureResult.Name = "cbSaveCaptureResult";
            cbSaveCaptureResult.Size = new System.Drawing.Size(415, 27);
            cbSaveCaptureResult.TabIndex = 13;
            cbSaveCaptureResult.Text = "이미지 캡쳐 보정 결과 저장 - captue_Result.bmp";
            cbSaveCaptureResult.UseVisualStyleBackColor = true;
            // 
            // cbSaveCapture
            // 
            cbSaveCapture.AutoSize = true;
            cbSaveCapture.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbSaveCapture.ForeColor = System.Drawing.Color.White;
            cbSaveCapture.Location = new System.Drawing.Point(18, 59);
            cbSaveCapture.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbSaveCapture.Name = "cbSaveCapture";
            cbSaveCapture.Size = new System.Drawing.Size(390, 27);
            cbSaveCapture.TabIndex = 12;
            cbSaveCapture.Text = "이미지 캡쳐 원본 저장 - captue_Original.bmp";
            cbSaveCapture.UseVisualStyleBackColor = true;
            // 
            // cbShowReplace
            // 
            cbShowReplace.AutoSize = true;
            cbShowReplace.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbShowReplace.ForeColor = System.Drawing.Color.White;
            cbShowReplace.Location = new System.Drawing.Point(18, 25);
            cbShowReplace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            cbShowReplace.Name = "cbShowReplace";
            cbShowReplace.Size = new System.Drawing.Size(180, 27);
            cbShowReplace.TabIndex = 11;
            cbShowReplace.Text = "교정사전 결과 표시";
            cbShowReplace.UseVisualStyleBackColor = true;
            // 
            // plDebugOff
            // 
            plDebugOff.Controls.Add(label63);
            plDebugOff.Controls.Add(btnDebugOn);
            plDebugOff.Location = new System.Drawing.Point(8, 235);
            plDebugOff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            plDebugOff.Name = "plDebugOff";
            plDebugOff.Size = new System.Drawing.Size(626, 268);
            plDebugOff.TabIndex = 57;
            plDebugOff.Visible = false;
            // 
            // label63
            // 
            label63.Anchor = System.Windows.Forms.AnchorStyles.None;
            label63.AutoSize = true;
            label63.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label63.ForeColor = System.Drawing.Color.White;
            label63.Location = new System.Drawing.Point(65, 25);
            label63.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label63.Name = "label63";
            label63.Size = new System.Drawing.Size(519, 92);
            label63.TabIndex = 43;
            label63.Text = "MORT 디버깅 기능을 활성화 합니다.\r\n개발, 진단용 기능이기 때문에 평상시에는 사용할 필요가 없습니다.\r\n\r\n※ 디버깅을 활성화 했을 시 성능에 영향을 줄 수 있습니다.\r\n";
            label63.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnDebugOn
            // 
            btnDebugOn.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnDebugOn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnDebugOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDebugOn.ForeColor = System.Drawing.Color.White;
            btnDebugOn.Location = new System.Drawing.Point(265, 198);
            btnDebugOn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnDebugOn.Name = "btnDebugOn";
            btnDebugOn.Size = new System.Drawing.Size(70, 29);
            btnDebugOn.TabIndex = 46;
            btnDebugOn.Text = "활성화";
            btnDebugOn.UseVisualStyleBackColor = false;
            btnDebugOn.Click += OnClick_DebugOn;
            // 
            // lbDebugging
            // 
            lbDebugging.AutoSize = true;
            lbDebugging.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lbDebugging.ForeColor = System.Drawing.Color.White;
            lbDebugging.Location = new System.Drawing.Point(5, 4);
            lbDebugging.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lbDebugging.Name = "lbDebugging";
            lbDebugging.Size = new System.Drawing.Size(69, 25);
            lbDebugging.TabIndex = 8;
            lbDebugging.Text = "디버깅";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            ClientSize = new System.Drawing.Size(780, 822);
            Controls.Add(donationButton);
            Controls.Add(pictureBox1);
            Controls.Add(acceptButton);
            Controls.Add(tbMain);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Monkeyhead's OCR RealTime Translator ";
            FormClosing += Form1_FormClosing;
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            Shown += Form1_Shown;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            ContextOption.ResumeLayout(false);
            optionMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tbMain.ResumeLayout(false);
            tpBasic.ResumeLayout(false);
            panel8.ResumeLayout(false);
            pnAdjustImg.ResumeLayout(false);
            pnAdjustImg.PerformLayout();
            pnOCR.ResumeLayout(false);
            pnOCR.PerformLayout();
            WinOCR_panel.ResumeLayout(false);
            WinOCR_panel.PerformLayout();
            pnEasyOcr.ResumeLayout(false);
            pnEasyOcr.PerformLayout();
            Tesseract_panel.ResumeLayout(false);
            Tesseract_panel.PerformLayout();
            pnGoogleOcr.ResumeLayout(false);
            pnGoogleOcr.PerformLayout();
            pnNHocr.ResumeLayout(false);
            pnNHocr.PerformLayout();
            pnTranslate.ResumeLayout(false);
            pnTranslate.PerformLayout();
            pnGoogleBasic.ResumeLayout(false);
            pnCustomApi.ResumeLayout(false);
            pnDeepLX.ResumeLayout(false);
            pnDeepl.ResumeLayout(false);
            pnDeepl.PerformLayout();
            DB_Panel.ResumeLayout(false);
            DB_Panel.PerformLayout();
            Naver_Panel.ResumeLayout(false);
            Naver_Panel.PerformLayout();
            Google_Panel.ResumeLayout(false);
            Google_Panel.PerformLayout();
            pnEzTrans.ResumeLayout(false);
            pnEzTrans.PerformLayout();
            pnPapagoWeb.ResumeLayout(false);
            tpText.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)backgroundColorBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor2Box).EndInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor1Box).EndInit();
            ((System.ComponentModel.ISupportInitialize)textColorBox).EndInit();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fontSizeUpDown).EndInit();
            tpExtra.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel21.ResumeLayout(false);
            panel21.PerformLayout();
            panel25.ResumeLayout(false);
            panel25.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgZoomsizeUpDown).EndInit();
            tpTranslation.ResumeLayout(false);
            panel19.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel27.ResumeLayout(false);
            panel27.PerformLayout();
            panel22.ResumeLayout(false);
            panel22.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            tpETC.ResumeLayout(false);
            panel18.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panel20.ResumeLayout(false);
            panel20.PerformLayout();
            panel23.ResumeLayout(false);
            panel23.PerformLayout();
            panel2.ResumeLayout(false);
            tpQuickSetting.ResumeLayout(false);
            panel28.ResumeLayout(false);
            panel31.ResumeLayout(false);
            panel31.PerformLayout();
            tpDebuging.ResumeLayout(false);
            panel24.ResumeLayout(false);
            panel26.ResumeLayout(false);
            panel26.PerformLayout();
            plDebugOn.ResumeLayout(false);
            plDebugOn.PerformLayout();
            plDebugOff.ResumeLayout(false);
            plDebugOff.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip ContextOption;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.TextBox bTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox gTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox rTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox v1TextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox s1TextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkHSV;
        private System.Windows.Forms.CheckBox checkRGB;
        private System.Windows.Forms.CheckBox checkErode;
        private System.Windows.Forms.TextBox v2TextBox;
        private System.Windows.Forms.TextBox s2TextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox showOcrCheckBox;
        private System.Windows.Forms.ToolStripMenuItem setCutPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem showTransToolStripMenuItem;
        private System.Windows.Forms.CheckBox saveOCRCheckBox;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.Panel pnOCR;
        private System.Windows.Forms.Label ocrLabel;
        private System.Windows.Forms.Panel pnTranslate;
        private System.Windows.Forms.Label lbTransTypeTitle;
        private System.Windows.Forms.Panel pnAdjustImg;
        private System.Windows.Forms.Label lbAdjustImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem rTTToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem setTranslateTopMostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkUpdateToolStripMenuItem;
        private System.Windows.Forms.CheckBox isClipBoardcheckBox1;
        private System.Windows.Forms.ToolStripMenuItem setCheckSpellingToolStripMenuItem;
        private System.Windows.Forms.Label lbImgGroup;
        private System.Windows.Forms.ComboBox groupCombo;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.Label lbImgGroupCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ContextMenuStrip optionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 설정저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설정불러오기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingSaveToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem settingLoadToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem settingDefaultToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkDic;
        private System.Windows.Forms.TextBox dicFileTextBox;
        private System.Windows.Forms.Label lbDicFile;
        private Dotnetrix_Samples.TabControl tbMain;
        private System.Windows.Forms.TabPage tpBasic;
        private System.Windows.Forms.TabPage tpText;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lbFontSetting;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TabPage tpExtra;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label lbTextAdditionalSettings;
        private System.Windows.Forms.Label lbFontSize;
        private System.Windows.Forms.Label lbFont;
        private System.Windows.Forms.CheckBox alignmentCenterCheckBox;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label lbFontColor;
        private System.Windows.Forms.CheckBox removeSpaceCheckBox;
        private System.Windows.Forms.Label lbFontOutlineColor2;
        private System.Windows.Forms.Label lbFontOutlineColor1;
        private System.Windows.Forms.PictureBox backgroundColorBox;
        private System.Windows.Forms.PictureBox outlineColor2Box;
        private System.Windows.Forms.PictureBox outlineColor1Box;
        private System.Windows.Forms.Label lbFontBasicColor;
        private System.Windows.Forms.PictureBox textColorBox;
        private System.Windows.Forms.CheckBox useBackColorCheckBox;
        private System.Windows.Forms.Label lbFontBackground;
        private System.Windows.Forms.NumericUpDown fontSizeUpDown;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button defaultColorButton;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label lbImgCapture;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.CheckBox checkUpdateCheckBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox activeWinodeCheckBox;
        private System.Windows.Forms.CheckBox topMostcheckBox;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Button openConfigButton;
        private System.Windows.Forms.Label lbSettingFile;
        private CustomLabel fontResultLabel;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label lbPreview;
        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.Button saveConfigButton;
        private System.Windows.Forms.Label lbImgZoom;
        private System.Windows.Forms.Button SetDefaultZoomSizeButton;
        private System.Windows.Forms.NumericUpDown imgZoomsizeUpDown;
        private System.Windows.Forms.TabPage tpETC;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Button error_Information_Button;
        private System.Windows.Forms.Button help_Button;
        private System.Windows.Forms.Label lbETC;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.Label lbHotKeyDoTrans;
        private System.Windows.Forms.Label lbHotkey;
        private System.Windows.Forms.Button about_Button;
        private System.Windows.Forms.TabPage tpTranslation;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton speedRadioButton5;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.RadioButton speedRadioButton4;
        private System.Windows.Forms.RadioButton speedRadioButton1;
        private System.Windows.Forms.RadioButton speedRadioButton3;
        private System.Windows.Forms.RadioButton speedRadioButton2;
        private System.Windows.Forms.Label lbDbFile;
        private System.Windows.Forms.TextBox dbFileTextBox;
        private System.Windows.Forms.Panel DB_Panel;
        private System.Windows.Forms.ComboBox TransType_Combobox;
        private System.Windows.Forms.Label lbTransType;
        private System.Windows.Forms.Panel Naver_Panel;
        private System.Windows.Forms.Label lbPapagoSecret;
        private System.Windows.Forms.TextBox NaverSecretKeyTextBox;
        private System.Windows.Forms.TextBox NaverIDKeyTextBox;
        private System.Windows.Forms.Label lbPapagoID;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.ComboBox naverTransComboBox;
        private System.Windows.Forms.Label lbPaPagoFrom;
        private System.Windows.Forms.Label lbPaPago;
        private KeyInputLabel quickKeyInputLabel;
        private System.Windows.Forms.Label lbHotKeyQuickOCR;
        private KeyInputLabel dicKeyInputLabel;
        private System.Windows.Forms.Label lbHotKeyDic;
        private KeyInputLabel transKeyInputLabel;
        private System.Windows.Forms.Label lbHotKeyInformation;
        private System.Windows.Forms.Button quickKeyInputResetButton;
        private System.Windows.Forms.Button dicKeyInputResetButton;
        private System.Windows.Forms.Button transKeyInputResetButton;
        private System.Windows.Forms.CheckBox checkStringUpper;
        private System.Windows.Forms.Button quickKeyInputEmptyButton;
        private System.Windows.Forms.Button dicKeyInputEmptyButton;
        private System.Windows.Forms.Button transKeyInputEmptyButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton skinLayerRadioButton;
        private System.Windows.Forms.Label lbTransformType;
        private System.Windows.Forms.RadioButton skinDarkRadioButton;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox OCR_Type_comboBox;
        private System.Windows.Forms.Panel Tesseract_panel;
        private System.Windows.Forms.Panel WinOCR_panel;
        private System.Windows.Forms.ComboBox WinOCR_Language_comboBox;
        private System.Windows.Forms.Label lbWinOCRLanguage;
        private System.Windows.Forms.ComboBox tesseractLanguageComboBox;
        private System.Windows.Forms.Label lbTesseractLanguage;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tessDataTextBox;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel Google_Panel;
        private System.Windows.Forms.TextBox googleSheet_textBox;
        private System.Windows.Forms.Label lbGoogleSheetAddress;
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.ComboBox googleResultCodeComboBox;
        private System.Windows.Forms.Label lbGoogleTo;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.ComboBox googleTransComboBox;
        private System.Windows.Forms.Label lbGoogle;
        private System.Windows.Forms.Label lbGoogleFrom;
        private System.Windows.Forms.Button button_RemoveAllGoogleToekn;
        private System.Windows.Forms.TextBox textBox_GoogleSecretKey;
        private System.Windows.Forms.Label lbSheetSecret;
        private System.Windows.Forms.TextBox textBox_GoogleClientID;
        private System.Windows.Forms.Label lbSheetID;
        private System.Windows.Forms.Button snapShotKeyInputEmptyButton;
        private System.Windows.Forms.Button snapShotKeyInputResetButton;
        private KeyInputLabel snapShotInputLabel;
        private System.Windows.Forms.Label lbHotKeySnapShot;
        private System.Windows.Forms.Button donationButton;
        private System.Windows.Forms.Button btnOneTransEmpty;
        private System.Windows.Forms.Button btnOneTransDefault;
        private KeyInputLabel lbOneTrans;
        private System.Windows.Forms.Label lbHotKeyOnceTranslate;
        private System.Windows.Forms.Button Button_NaverTransKeyList;
        private System.Windows.Forms.CheckBox cbPerWordDic;
        private System.Windows.Forms.ToolTip toolTip_OCR;
        private System.Windows.Forms.Panel pnGoogleBasic;
        private System.Windows.Forms.Label lbBasicInfo;
        private System.Windows.Forms.Button btnTransHelp;
        private System.Windows.Forms.TabPage tpDebuging;
        private System.Windows.Forms.Panel panel24;
        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.Label lbDebugging;
        private System.Windows.Forms.CheckBox cbShowOCRIndex;
        private System.Windows.Forms.Button btnHideTransEmpty;
        private System.Windows.Forms.Button btnHideTransDefault;
        private KeyInputLabel lbHideTranslate;
        private System.Windows.Forms.Label lbHotKeyHideTransWindow;
        private System.Windows.Forms.CheckBox cbFastTess;
        private System.Windows.Forms.Panel plDebugOn;
        private System.Windows.Forms.CheckBox cbShowReplace;
        private System.Windows.Forms.Panel plDebugOff;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Button btnDebugOn;
        private System.Windows.Forms.CheckBox cbSaveCapture;
        private System.Windows.Forms.CheckBox cbSaveCaptureResult;
        private System.Windows.Forms.CheckBox cbDBMultiGet;
        private System.Windows.Forms.ComboBox cbNaverResultCode;
        private System.Windows.Forms.Label lbPaPagoTo;
        private System.Windows.Forms.Label lbSpeedInformation;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Button openBlogButton;
        private System.Windows.Forms.Button btnGitHub;
        private System.Windows.Forms.Label lbLink;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Button btSettingUpload;
        private System.Windows.Forms.Button btSettingBrowser;
        private System.Windows.Forms.Label lbSearchConfig;
        private System.Windows.Forms.Panel panel27;
        private System.Windows.Forms.CheckBox cbTTSWaitEnd;
        private System.Windows.Forms.CheckBox cbUseTTS;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.CheckBox cbUnlockSpeed;
        private System.Windows.Forms.CheckBox cbShowFormerLog;
        private System.Windows.Forms.Button btClearFormerResult;
        private System.Windows.Forms.Button btAttachCapture;
        private System.Windows.Forms.RadioButton skinOverRadioButton;
        private System.Windows.Forms.CheckBox cbShowOverlayWordArea;
        private System.Windows.Forms.CheckBox cbSetLineTrans;
        private System.Windows.Forms.Button btnOpenDiscord;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.Button btAdvencedOption;
        private System.Windows.Forms.Label lbAdvencedConfig;
        private System.Windows.Forms.Panel pnEzTrans;
        private System.Windows.Forms.Label lbEzTransInfo;
        private System.Windows.Forms.TabPage tpQuickSetting;
        private System.Windows.Forms.Panel panel28;
        private System.Windows.Forms.Panel panel31;
        private System.Windows.Forms.CheckBox cbSetBasicDefaultPage;
        private System.Windows.Forms.Button btQuickJap;
        private System.Windows.Forms.Label lbQuickSettingInformation;
        private System.Windows.Forms.Button btQucickEnglish;
        private System.Windows.Forms.Label lbQuickSetting;
        private System.Windows.Forms.Button btOcrHelp;
        private System.Windows.Forms.CheckBox cbThreshold;
        private System.Windows.Forms.TextBox tbThreshold;
        private System.Windows.Forms.Button btImgResult;
        private System.Windows.Forms.Panel pnNHocr;
        private System.Windows.Forms.Label lbNHOcrInfo;
        private System.Windows.Forms.Panel pnGoogleOcr;
        private System.Windows.Forms.Button btnSettingGoogleOCR;
        private System.Windows.Forms.Label lbGoogleOcrStatus;
        private System.Windows.Forms.ComboBox cbGoogleOcrLanguge;
        private System.Windows.Forms.Label lbGoogleOCRLanguage;
        private System.Windows.Forms.Label lbBasicStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnDeepl;
        private System.Windows.Forms.Label lbDeepLStatus;
        private System.Windows.Forms.Label lbDeepLInfo;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cbDeepLLanguageTo;
        private System.Windows.Forms.Label lbDeepLTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbDeepLLanguage;
        private System.Windows.Forms.Label lbDeepL;
        private System.Windows.Forms.Label lbDeepLFrom;
        private System.Windows.Forms.Button btnCheckDeeplState;
        private System.Windows.Forms.Panel pnCustomApi;
        private System.Windows.Forms.Label lbCustomApiInformation;
        private System.Windows.Forms.Panel pnDeepLX;
        private System.Windows.Forms.Label lbDeepLXInformation;
        private System.Windows.Forms.Panel pnEasyOcr;
        private System.Windows.Forms.ComboBox cbEasyOcrCode;
        private System.Windows.Forms.Label lbEasyOcrLanguage;
        private System.Windows.Forms.Button btnInstallEasyOcr;
        private System.Windows.Forms.Panel pnPapagoWeb;
        private System.Windows.Forms.Label lbPapagoWebInfo;
        private System.Windows.Forms.Label lbPapagoLanguageCodeInformation;
        private System.Windows.Forms.Button btnAddWinOcrLanguage;
    }




}

