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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransFormLayer));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sortMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SortTypeBasicMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SortTypeCenterMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.removeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.forceTransparencyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortMenu,
            this.removeMenu,
            this.forceTransparencyMenu,
            this.CloseToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(167, 92);
            // 
            // sortMenu
            // 
            this.sortMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SortTypeBasicMenu,
            this.SortTypeCenterMenu});
            this.sortMenu.Name = "sortMenu";
            this.sortMenu.Size = new System.Drawing.Size(166, 22);
            this.sortMenu.Text = "정렬";
            // 
            // SortTypeBasicMenu
            // 
            this.SortTypeBasicMenu.Checked = true;
            this.SortTypeBasicMenu.CheckOnClick = true;
            this.SortTypeBasicMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SortTypeBasicMenu.Name = "SortTypeBasicMenu";
            this.SortTypeBasicMenu.Size = new System.Drawing.Size(110, 22);
            this.SortTypeBasicMenu.Text = "기본";
            this.SortTypeBasicMenu.Click += new System.EventHandler(this.SortTypeBasicMenu_Click);
            // 
            // SortTypeCenterMenu
            // 
            this.SortTypeCenterMenu.CheckOnClick = true;
            this.SortTypeCenterMenu.Name = "SortTypeCenterMenu";
            this.SortTypeCenterMenu.Size = new System.Drawing.Size(110, 22);
            this.SortTypeCenterMenu.Text = "가운데";
            this.SortTypeCenterMenu.Click += new System.EventHandler(this.SortTypeCenterMenu_Click);
            // 
            // removeMenu
            // 
            this.removeMenu.Name = "removeMenu";
            this.removeMenu.Size = new System.Drawing.Size(166, 22);
            this.removeMenu.Text = "공백 제거";
            this.removeMenu.Click += new System.EventHandler(this.removeMenu_Click);
            // 
            // forceTransparencyMenu
            // 
            this.forceTransparencyMenu.Name = "forceTransparencyMenu";
            this.forceTransparencyMenu.Size = new System.Drawing.Size(166, 22);
            this.forceTransparencyMenu.Text = "강제 투명화 유지";
            this.forceTransparencyMenu.Click += new System.EventHandler(this.forceTransparencyMenu_Click);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.CloseToolStripMenuItem.Text = "닫기";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // TransFormLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(973, 192);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("굴림", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TransFormLayer";
            this.Text = "TransFormLayer";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransFormLayer_FormClosing);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TransFormLayer_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TransForm_MouseUp);
            this.Resize += new System.EventHandler(this.TransFormLayer_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

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