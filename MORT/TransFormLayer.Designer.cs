namespace MORT
{
    partial class TransFormLayer
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransFormLayer));
            contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            sortMenu = new System.Windows.Forms.ToolStripMenuItem();
            SortTypeBasicMenu = new System.Windows.Forms.ToolStripMenuItem();
            SortTypeCenterMenu = new System.Windows.Forms.ToolStripMenuItem();
            removeMenu = new System.Windows.Forms.ToolStripMenuItem();
            forceTransparencyMenu = new System.Windows.Forms.ToolStripMenuItem();
            CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { sortMenu, removeMenu, forceTransparencyMenu, CloseToolStripMenuItem });
            contextMenu.Name = "contextMenuStrip1";
            contextMenu.Size = new System.Drawing.Size(167, 92);
            // 
            // sortMenu
            // 
            sortMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SortTypeBasicMenu, SortTypeCenterMenu });
            sortMenu.Name = "sortMenu";
            sortMenu.Size = new System.Drawing.Size(166, 22);
            sortMenu.Text = "정렬";
            // 
            // SortTypeBasicMenu
            // 
            SortTypeBasicMenu.Checked = true;
            SortTypeBasicMenu.CheckOnClick = true;
            SortTypeBasicMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            SortTypeBasicMenu.Name = "SortTypeBasicMenu";
            SortTypeBasicMenu.Size = new System.Drawing.Size(110, 22);
            SortTypeBasicMenu.Text = "기본";
            SortTypeBasicMenu.Click += SortTypeBasicMenu_Click;
            // 
            // SortTypeCenterMenu
            // 
            SortTypeCenterMenu.CheckOnClick = true;
            SortTypeCenterMenu.Name = "SortTypeCenterMenu";
            SortTypeCenterMenu.Size = new System.Drawing.Size(110, 22);
            SortTypeCenterMenu.Text = "가운데";
            SortTypeCenterMenu.Click += SortTypeCenterMenu_Click;
            // 
            // removeMenu
            // 
            removeMenu.Name = "removeMenu";
            removeMenu.Size = new System.Drawing.Size(166, 22);
            removeMenu.Text = "공백 제거";
            removeMenu.Click += removeMenu_Click;
            // 
            // forceTransparencyMenu
            // 
            forceTransparencyMenu.Name = "forceTransparencyMenu";
            forceTransparencyMenu.Size = new System.Drawing.Size(166, 22);
            forceTransparencyMenu.Text = "강제 투명화 유지";
            forceTransparencyMenu.Click += forceTransparencyMenu_Click;
            // 
            // CloseToolStripMenuItem
            // 
            CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            CloseToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            CloseToolStripMenuItem.Text = "닫기";
            CloseToolStripMenuItem.Click += CloseToolStripMenuItem_Click;
            // 
            // TransFormLayer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.AliceBlue;
            ClientSize = new System.Drawing.Size(973, 192);
            DoubleBuffered = true;
            Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ForeColor = System.Drawing.Color.White;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(0);
            MaximizeBox = false;
            Name = "TransFormLayer";
            Text = "TransFormLayer";
            TopMost = true;
            FormClosing += TransFormLayer_FormClosing;
            MouseClick += TransFormLayer_MouseClick;
            MouseDown += TransForm_MouseDown;
            MouseMove += TransForm_MouseMove;
            MouseUp += TransForm_MouseUp;
            Resize += TransFormLayer_Resize;
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem sortMenu;
        private System.Windows.Forms.ToolStripMenuItem SortTypeBasicMenu;
        private System.Windows.Forms.ToolStripMenuItem SortTypeCenterMenu;
        private System.Windows.Forms.ToolStripMenuItem removeMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceTransparencyMenu;
    }
}