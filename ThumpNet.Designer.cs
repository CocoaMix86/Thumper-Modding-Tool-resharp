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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThumpNet));
            this.pnl_levels = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compactViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newestFirstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oldestFirstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_levels
            // 
            this.pnl_levels.AutoScroll = true;
            this.pnl_levels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_levels.Location = new System.Drawing.Point(0, 27);
            this.pnl_levels.Name = "pnl_levels";
            this.pnl_levels.Padding = new System.Windows.Forms.Padding(5);
            this.pnl_levels.Size = new System.Drawing.Size(814, 474);
            this.pnl_levels.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.compactViewToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.txtSearch});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(814, 27);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(55, 23);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // compactViewToolStripMenuItem
            // 
            this.compactViewToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.compactViewToolStripMenuItem.Name = "compactViewToolStripMenuItem";
            this.compactViewToolStripMenuItem.Size = new System.Drawing.Size(96, 23);
            this.compactViewToolStripMenuItem.Text = "Compact View";
            this.compactViewToolStripMenuItem.Click += new System.EventHandler(this.compactViewToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newestFirstToolStripMenuItem,
            this.oldestFirstToolStripMenuItem});
            this.sortToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(40, 23);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // newestFirstToolStripMenuItem
            // 
            this.newestFirstToolStripMenuItem.Checked = true;
            this.newestFirstToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.newestFirstToolStripMenuItem.Name = "newestFirstToolStripMenuItem";
            this.newestFirstToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.newestFirstToolStripMenuItem.Text = "Newest first";
            this.newestFirstToolStripMenuItem.Click += new System.EventHandler(this.newestFirstToolStripMenuItem_Click);
            // 
            // oldestFirstToolStripMenuItem
            // 
            this.oldestFirstToolStripMenuItem.Name = "oldestFirstToolStripMenuItem";
            this.oldestFirstToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.oldestFirstToolStripMenuItem.Text = "Oldest first";
            this.oldestFirstToolStripMenuItem.Click += new System.EventHandler(this.oldestFirstToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearCacheToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 23);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // clearCacheToolStripMenuItem
            // 
            this.clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
            this.clearCacheToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearCacheToolStripMenuItem.Text = "Clear Cache";
            this.clearCacheToolStripMenuItem.Click += new System.EventHandler(this.clearCacheToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 23);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 23);
            this.txtSearch.TextChanged += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // ThumpNet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(814, 501);
            this.Controls.Add(this.pnl_levels);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThumpNet";
            this.Text = "ThumpNet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThumpNet_FormClosing);
            this.Load += new System.EventHandler(this.ThumpNet_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnl_levels;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compactViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newestFirstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oldestFirstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox txtSearch;
    }
}