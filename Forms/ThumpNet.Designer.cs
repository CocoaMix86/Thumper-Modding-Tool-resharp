namespace Thumper_Modding_Tool_resharp
{
    partial class ThumpNet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThumpNet));
            this.pnl_levels = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compactViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCacheFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnTxtSearchClear = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.toolstripSortButtons = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolstripSortTime = new System.Windows.Forms.ToolStripButton();
            this.toolstripSortAlpha = new System.Windows.Forms.ToolStripButton();
            this.toolstripSortDif = new System.Windows.Forms.ToolStripButton();
            this.toolstripSortAuthor = new System.Windows.Forms.ToolStripButton();
            this.toolstripSortDirection = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolstripSortButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_levels
            // 
            this.pnl_levels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_levels.AutoScroll = true;
            this.pnl_levels.Location = new System.Drawing.Point(0, 46);
            this.pnl_levels.Name = "pnl_levels";
            this.pnl_levels.Padding = new System.Windows.Forms.Padding(5);
            this.pnl_levels.Size = new System.Drawing.Size(814, 455);
            this.pnl_levels.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Maroon;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.compactViewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.searchToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(814, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // compactViewToolStripMenuItem
            // 
            this.compactViewToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.compactViewToolStripMenuItem.Name = "compactViewToolStripMenuItem";
            this.compactViewToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.compactViewToolStripMenuItem.Text = "Compact View";
            this.compactViewToolStripMenuItem.Click += new System.EventHandler(this.compactViewToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearCacheToolStripMenuItem,
            this.openCacheFolderToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // clearCacheToolStripMenuItem
            // 
            this.clearCacheToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.clearCacheToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
            this.clearCacheToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.clearCacheToolStripMenuItem.Text = "Clear Cache";
            this.clearCacheToolStripMenuItem.Click += new System.EventHandler(this.clearCacheToolStripMenuItem_Click);
            // 
            // openCacheFolderToolStripMenuItem
            // 
            this.openCacheFolderToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.openCacheFolderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openCacheFolderToolStripMenuItem.Name = "openCacheFolderToolStripMenuItem";
            this.openCacheFolderToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openCacheFolderToolStripMenuItem.Text = "Open Cache Folder";
            this.openCacheFolderToolStripMenuItem.Click += new System.EventHandler(this.openCacheFolderToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Enabled = false;
            this.searchToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.searchToolStripMenuItem.Text = "Search:";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            // 
            // btnTxtSearchClear
            // 
            this.btnTxtSearchClear.AutoSize = true;
            this.btnTxtSearchClear.BackColor = System.Drawing.Color.Transparent;
            this.btnTxtSearchClear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnTxtSearchClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTxtSearchClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTxtSearchClear.ForeColor = System.Drawing.Color.Red;
            this.btnTxtSearchClear.Location = new System.Drawing.Point(269, 1);
            this.btnTxtSearchClear.MinimumSize = new System.Drawing.Size(2, 22);
            this.btnTxtSearchClear.Name = "btnTxtSearchClear";
            this.btnTxtSearchClear.Size = new System.Drawing.Size(15, 22);
            this.btnTxtSearchClear.TabIndex = 2;
            this.btnTxtSearchClear.Text = "x";
            this.toolTip1.SetToolTip(this.btnTxtSearchClear, "clear search");
            this.btnTxtSearchClear.Click += new System.EventHandler(this.btnTxtSearchClear_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.AllowDrop = true;
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(283, 1);
            this.txtSearch.MaxLength = 100;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(464, 22);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // toolstripSortButtons
            // 
            this.toolstripSortButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.toolstripSortButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripSortButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolstripSortTime,
            this.toolstripSortAlpha,
            this.toolstripSortDif,
            this.toolstripSortAuthor,
            this.toolStripSeparator1,
            this.toolstripSortDirection});
            this.toolstripSortButtons.Location = new System.Drawing.Point(0, 24);
            this.toolstripSortButtons.Name = "toolstripSortButtons";
            this.toolstripSortButtons.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolstripSortButtons.Size = new System.Drawing.Size(814, 25);
            this.toolstripSortButtons.TabIndex = 3;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel1.Text = "Sort Mode:";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // toolstripSortTime
            // 
            this.toolstripSortTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSortTime.Image = global::Thumper_Modding_Tool_resharp.Properties.Resources.icon_time_32;
            this.toolstripSortTime.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSortTime.Name = "toolstripSortTime";
            this.toolstripSortTime.Size = new System.Drawing.Size(23, 22);
            this.toolstripSortTime.Tag = "time";
            this.toolstripSortTime.ToolTipText = "Time uploaded";
            this.toolstripSortTime.Click += new System.EventHandler(this.toolstripSort_Click);
            // 
            // toolstripSortAlpha
            // 
            this.toolstripSortAlpha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSortAlpha.Image = global::Thumper_Modding_Tool_resharp.Properties.Resources.icon_alpha_32;
            this.toolstripSortAlpha.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSortAlpha.Name = "toolstripSortAlpha";
            this.toolstripSortAlpha.Size = new System.Drawing.Size(23, 22);
            this.toolstripSortAlpha.Tag = "alphabetical";
            this.toolstripSortAlpha.ToolTipText = "Alphabetical";
            this.toolstripSortAlpha.Click += new System.EventHandler(this.toolstripSort_Click);
            // 
            // toolstripSortDif
            // 
            this.toolstripSortDif.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSortDif.Image = global::Thumper_Modding_Tool_resharp.Properties.Resources.icon_difficulty;
            this.toolstripSortDif.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSortDif.Name = "toolstripSortDif";
            this.toolstripSortDif.Size = new System.Drawing.Size(23, 22);
            this.toolstripSortDif.Tag = "difficulty";
            this.toolstripSortDif.ToolTipText = "Difficulty";
            this.toolstripSortDif.Click += new System.EventHandler(this.toolstripSort_Click);
            // 
            // toolstripSortAuthor
            // 
            this.toolstripSortAuthor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSortAuthor.Image = global::Thumper_Modding_Tool_resharp.Properties.Resources.icon_person_32;
            this.toolstripSortAuthor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSortAuthor.Name = "toolstripSortAuthor";
            this.toolstripSortAuthor.Size = new System.Drawing.Size(23, 22);
            this.toolstripSortAuthor.Tag = "author";
            this.toolstripSortAuthor.ToolTipText = "Author Name";
            this.toolstripSortAuthor.Click += new System.EventHandler(this.toolstripSort_Click);
            // 
            // toolstripSortDirection
            // 
            this.toolstripSortDirection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripSortDirection.Image = global::Thumper_Modding_Tool_resharp.Properties.Resources.icon_sort_32;
            this.toolstripSortDirection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSortDirection.Name = "toolstripSortDirection";
            this.toolstripSortDirection.Size = new System.Drawing.Size(23, 22);
            this.toolstripSortDirection.ToolTipText = "Reverse  sort order";
            this.toolstripSortDirection.Click += new System.EventHandler(this.toolstripSortDirection_Click);
            // 
            // ThumpNet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(814, 501);
            this.Controls.Add(this.toolstripSortButtons);
            this.Controls.Add(this.btnTxtSearchClear);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.pnl_levels);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThumpNet";
            this.Text = "ThumpNet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThumpNet_FormClosing);
            this.Load += new System.EventHandler(this.ThumpNet_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolstripSortButtons.ResumeLayout(false);
            this.toolstripSortButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnl_levels;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compactViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label btnTxtSearchClear;
        private System.Windows.Forms.ToolStripMenuItem openCacheFolderToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolStrip toolstripSortButtons;
        private System.Windows.Forms.ToolStripButton toolstripSortTime;
        private System.Windows.Forms.ToolStripButton toolstripSortAlpha;
        private System.Windows.Forms.ToolStripButton toolstripSortDif;
        private System.Windows.Forms.ToolStripButton toolstripSortAuthor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolstripSortDirection;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}