
namespace Thumper_Modding_Tool_resharp
{
	partial class ThumperModdingTool
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
			if (disposing && (components != null)) {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThumperModdingTool));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.changeGameDirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hashPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thumpNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeGameDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.btnModMode = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnLevelAdd = new System.Windows.Forms.Button();
            this.btnLevelRemove = new System.Windows.Forms.Button();
            this.btnLevelUp = new System.Windows.Forms.Button();
            this.btnLevelDown = new System.Windows.Forms.Button();
            this.dgvLevels = new System.Windows.Forms.DataGridView();
            this.LevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Difficulty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sublevels = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.richDescript = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnHash = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panelHash = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picSplashScreen = new System.Windows.Forms.PictureBox();
            this.btnSplashScreen = new System.Windows.Forms.Button();
            this.btnSplashScreenReset = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLevels)).BeginInit();
            this.panelHash.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSplashScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem1,
            this.thumpNetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(519, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem1
            // 
            this.optionsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeGameDirToolStripMenuItem1,
            this.hashPanelToolStripMenuItem,
            this.resetSettingsToolStripMenuItem});
            this.optionsToolStripMenuItem1.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem1.Name = "optionsToolStripMenuItem1";
            this.optionsToolStripMenuItem1.Size = new System.Drawing.Size(63, 22);
            this.optionsToolStripMenuItem1.Text = "Options";
            // 
            // changeGameDirToolStripMenuItem1
            // 
            this.changeGameDirToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.changeGameDirToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.changeGameDirToolStripMenuItem1.Name = "changeGameDirToolStripMenuItem1";
            this.changeGameDirToolStripMenuItem1.Size = new System.Drawing.Size(203, 22);
            this.changeGameDirToolStripMenuItem1.Text = "Change Game Dir";
            this.changeGameDirToolStripMenuItem1.Click += new System.EventHandler(this.changeGameDirToolStripMenuItem_Click);
            // 
            // hashPanelToolStripMenuItem
            // 
            this.hashPanelToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.hashPanelToolStripMenuItem.CheckOnClick = true;
            this.hashPanelToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.hashPanelToolStripMenuItem.Name = "hashPanelToolStripMenuItem";
            this.hashPanelToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.hashPanelToolStripMenuItem.Text = "Hash Panel";
            this.hashPanelToolStripMenuItem.Click += new System.EventHandler(this.hashPanelToolStripMenuItem_Click);
            // 
            // resetSettingsToolStripMenuItem
            // 
            this.resetSettingsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.resetSettingsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.resetSettingsToolStripMenuItem.Name = "resetSettingsToolStripMenuItem";
            this.resetSettingsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.resetSettingsToolStripMenuItem.Text = "[!!!] Reset Settings [!!!]";
            this.resetSettingsToolStripMenuItem.Click += new System.EventHandler(this.resetSettingsToolStripMenuItem_Click);
            // 
            // thumpNetToolStripMenuItem
            // 
            this.thumpNetToolStripMenuItem.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thumpNetToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.thumpNetToolStripMenuItem.Name = "thumpNetToolStripMenuItem";
            this.thumpNetToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.thumpNetToolStripMenuItem.Text = "Download Levels";
            this.thumpNetToolStripMenuItem.Click += new System.EventHandler(this.thumpNetToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeGameDirToolStripMenuItem});
            this.optionsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // changeGameDirToolStripMenuItem
            // 
            this.changeGameDirToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.changeGameDirToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.changeGameDirToolStripMenuItem.Name = "changeGameDirToolStripMenuItem";
            this.changeGameDirToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.changeGameDirToolStripMenuItem.Text = "Change Game Dir";
            this.changeGameDirToolStripMenuItem.Click += new System.EventHandler(this.changeGameDirToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mod Mode";
            // 
            // btnModMode
            // 
            this.btnModMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnModMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModMode.Font = new System.Drawing.Font("Trebuchet MS", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModMode.ForeColor = System.Drawing.Color.Crimson;
            this.btnModMode.Location = new System.Drawing.Point(187, 27);
            this.btnModMode.Name = "btnModMode";
            this.btnModMode.Size = new System.Drawing.Size(105, 45);
            this.btnModMode.TabIndex = 3;
            this.btnModMode.Text = "OFF";
            this.btnModMode.UseVisualStyleBackColor = false;
            this.btnModMode.Click += new System.EventHandler(this.btnModMode_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(63, 77);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 25);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.toolTip1.SetToolTip(this.btnUpdate, "Update Thumper with these levels and splash screen");
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnLevelAdd
            // 
            this.btnLevelAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelAdd.BackColor = System.Drawing.Color.YellowGreen;
            this.btnLevelAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLevelAdd.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelAdd.Location = new System.Drawing.Point(12, 296);
            this.btnLevelAdd.Name = "btnLevelAdd";
            this.btnLevelAdd.Size = new System.Drawing.Size(74, 25);
            this.btnLevelAdd.TabIndex = 6;
            this.btnLevelAdd.Text = "Add Level";
            this.toolTip1.SetToolTip(this.btnLevelAdd, "You can click and drag level folders into the list\r\nto quickly add them");
            this.btnLevelAdd.UseVisualStyleBackColor = false;
            this.btnLevelAdd.Click += new System.EventHandler(this.btnLevelAdd_Click);
            // 
            // btnLevelRemove
            // 
            this.btnLevelRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelRemove.BackColor = System.Drawing.Color.Crimson;
            this.btnLevelRemove.Enabled = false;
            this.btnLevelRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLevelRemove.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelRemove.Location = new System.Drawing.Point(86, 296);
            this.btnLevelRemove.Name = "btnLevelRemove";
            this.btnLevelRemove.Size = new System.Drawing.Size(74, 25);
            this.btnLevelRemove.TabIndex = 7;
            this.btnLevelRemove.Text = "Remove";
            this.btnLevelRemove.UseVisualStyleBackColor = false;
            this.btnLevelRemove.Click += new System.EventHandler(this.btnLevelRemove_Click);
            // 
            // btnLevelUp
            // 
            this.btnLevelUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelUp.BackColor = System.Drawing.Color.Cyan;
            this.btnLevelUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLevelUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelUp.Location = new System.Drawing.Point(166, 296);
            this.btnLevelUp.Name = "btnLevelUp";
            this.btnLevelUp.Size = new System.Drawing.Size(23, 25);
            this.btnLevelUp.TabIndex = 8;
            this.btnLevelUp.Text = "↑";
            this.toolTip1.SetToolTip(this.btnLevelUp, "Move selected level up");
            this.btnLevelUp.UseCompatibleTextRendering = true;
            this.btnLevelUp.UseVisualStyleBackColor = false;
            this.btnLevelUp.Click += new System.EventHandler(this.btnLevelUp_Click);
            // 
            // btnLevelDown
            // 
            this.btnLevelDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLevelDown.BackColor = System.Drawing.Color.Cyan;
            this.btnLevelDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLevelDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLevelDown.Location = new System.Drawing.Point(189, 296);
            this.btnLevelDown.Name = "btnLevelDown";
            this.btnLevelDown.Size = new System.Drawing.Size(22, 25);
            this.btnLevelDown.TabIndex = 9;
            this.btnLevelDown.Text = "↓";
            this.toolTip1.SetToolTip(this.btnLevelDown, "Move selected level down");
            this.btnLevelDown.UseCompatibleTextRendering = true;
            this.btnLevelDown.UseVisualStyleBackColor = false;
            this.btnLevelDown.Click += new System.EventHandler(this.btnLevelDown_Click);
            // 
            // dgvLevels
            // 
            this.dgvLevels.AllowDrop = true;
            this.dgvLevels.AllowUserToAddRows = false;
            this.dgvLevels.AllowUserToDeleteRows = false;
            this.dgvLevels.AllowUserToResizeColumns = false;
            this.dgvLevels.AllowUserToResizeRows = false;
            this.dgvLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLevels.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dgvLevels.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvLevels.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvLevels.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLevels.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLevels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLevels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LevelName,
            this.Difficulty,
            this.Sublevels});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLevels.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLevels.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLevels.EnableHeadersVisualStyles = false;
            this.dgvLevels.GridColor = System.Drawing.Color.Black;
            this.dgvLevels.Location = new System.Drawing.Point(12, 104);
            this.dgvLevels.MultiSelect = false;
            this.dgvLevels.Name = "dgvLevels";
            this.dgvLevels.ReadOnly = true;
            this.dgvLevels.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLevels.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLevels.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvLevels.RowTemplate.Height = 200;
            this.dgvLevels.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLevels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLevels.Size = new System.Drawing.Size(280, 192);
            this.dgvLevels.TabIndex = 41;
            this.dgvLevels.SelectionChanged += new System.EventHandler(this.dgvLevels_SelectionChanged);
            this.dgvLevels.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvLevels_DragDrop);
            this.dgvLevels.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvLevels_DragEnter);
            // 
            // LevelName
            // 
            this.LevelName.HeaderText = "Level Name";
            this.LevelName.Name = "LevelName";
            this.LevelName.ReadOnly = true;
            this.LevelName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Difficulty
            // 
            this.Difficulty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Difficulty.FillWeight = 40F;
            this.Difficulty.HeaderText = "Difficulty";
            this.Difficulty.Name = "Difficulty";
            this.Difficulty.ReadOnly = true;
            this.Difficulty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Difficulty.Width = 59;
            // 
            // Sublevels
            // 
            this.Sublevels.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Sublevels.FillWeight = 40F;
            this.Sublevels.HeaderText = "Sublevels";
            this.Sublevels.Name = "Sublevels";
            this.Sublevels.ReadOnly = true;
            this.Sublevels.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Sublevels.Width = 66;
            // 
            // richDescript
            // 
            this.richDescript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richDescript.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.richDescript.ForeColor = System.Drawing.Color.White;
            this.richDescript.Location = new System.Drawing.Point(302, 211);
            this.richDescript.Name = "richDescript";
            this.richDescript.Size = new System.Drawing.Size(205, 110);
            this.richDescript.TabIndex = 42;
            this.richDescript.Text = "";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(298, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 22);
            this.label3.TabIndex = 43;
            this.label3.Text = "Level Description";
            // 
            // BtnHash
            // 
            this.BtnHash.BackColor = System.Drawing.Color.MediumPurple;
            this.BtnHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnHash.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnHash.Location = new System.Drawing.Point(3, 3);
            this.BtnHash.Name = "BtnHash";
            this.BtnHash.Size = new System.Drawing.Size(83, 28);
            this.BtnHash.TabIndex = 44;
            this.BtnHash.Text = "Hash";
            this.BtnHash.UseVisualStyleBackColor = false;
            this.BtnHash.Click += new System.EventHandler(this.BtnHash_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(3, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(95, 20);
            this.textBox1.TabIndex = 45;
            this.textBox1.Text = "input";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(3, 63);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(95, 20);
            this.textBox2.TabIndex = 46;
            // 
            // panelHash
            // 
            this.panelHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHash.BackColor = System.Drawing.Color.Maroon;
            this.panelHash.Controls.Add(this.textBox2);
            this.panelHash.Controls.Add(this.BtnHash);
            this.panelHash.Controls.Add(this.textBox1);
            this.panelHash.Location = new System.Drawing.Point(406, 61);
            this.panelHash.Name = "panelHash";
            this.panelHash.Size = new System.Drawing.Size(101, 89);
            this.panelHash.TabIndex = 47;
            this.panelHash.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.ReshowDelay = 100;
            // 
            // picSplashScreen
            // 
            this.picSplashScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSplashScreen.Location = new System.Drawing.Point(302, 61);
            this.picSplashScreen.Name = "picSplashScreen";
            this.picSplashScreen.Size = new System.Drawing.Size(98, 98);
            this.picSplashScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSplashScreen.TabIndex = 48;
            this.picSplashScreen.TabStop = false;
            this.toolTip1.SetToolTip(this.picSplashScreen, "Splash screen. Save by dragging to a folder or your desktop");
            this.picSplashScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picSplashScreen_MouseMove);
            // 
            // btnSplashScreen
            // 
            this.btnSplashScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplashScreen.BackColor = System.Drawing.Color.YellowGreen;
            this.btnSplashScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplashScreen.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSplashScreen.Location = new System.Drawing.Point(300, 158);
            this.btnSplashScreen.Name = "btnSplashScreen";
            this.btnSplashScreen.Size = new System.Drawing.Size(55, 25);
            this.btnSplashScreen.TabIndex = 49;
            this.btnSplashScreen.Text = "Change";
            this.toolTip1.SetToolTip(this.btnSplashScreen, "Change splash screen image (DDS format)");
            this.btnSplashScreen.UseVisualStyleBackColor = false;
            this.btnSplashScreen.Click += new System.EventHandler(this.btnSplashScreen_Click);
            // 
            // btnSplashScreenReset
            // 
            this.btnSplashScreenReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplashScreenReset.BackColor = System.Drawing.Color.Crimson;
            this.btnSplashScreenReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplashScreenReset.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSplashScreenReset.Location = new System.Drawing.Point(350, 158);
            this.btnSplashScreenReset.Name = "btnSplashScreenReset";
            this.btnSplashScreenReset.Size = new System.Drawing.Size(50, 25);
            this.btnSplashScreenReset.TabIndex = 52;
            this.btnSplashScreenReset.Text = "Reset";
            this.toolTip1.SetToolTip(this.btnSplashScreenReset, "Reset splash screen to the original Drool logo");
            this.btnSplashScreenReset.UseVisualStyleBackColor = false;
            this.btnSplashScreenReset.Click += new System.EventHandler(this.btnSplashScreenReset_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 22);
            this.label2.TabIndex = 50;
            this.label2.Text = "Levels";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(298, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 22);
            this.label4.TabIndex = 51;
            this.label4.Text = "Splash Screen";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(299, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 16);
            this.label5.TabIndex = 53;
            this.label5.Text = "Recommended: 512x512";
            // 
            // ThumperModdingTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(519, 333);
            this.Controls.Add(this.btnSplashScreenReset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSplashScreen);
            this.Controls.Add(this.picSplashScreen);
            this.Controls.Add(this.panelHash);
            this.Controls.Add(this.richDescript);
            this.Controls.Add(this.dgvLevels);
            this.Controls.Add(this.btnLevelDown);
            this.Controls.Add(this.btnLevelUp);
            this.Controls.Add(this.btnLevelRemove);
            this.Controls.Add(this.btnLevelAdd);
            this.Controls.Add(this.btnModMode);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ThumperModdingTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thumper Mod Loader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThumperModdingTool_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLevels)).EndInit();
            this.panelHash.ResumeLayout(false);
            this.panelHash.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSplashScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnModMode;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnLevelAdd;
		private System.Windows.Forms.Button btnLevelRemove;
		private System.Windows.Forms.Button btnLevelUp;
		private System.Windows.Forms.Button btnLevelDown;
		private System.Windows.Forms.DataGridView dgvLevels;
		private System.Windows.Forms.RichTextBox richDescript;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolStripMenuItem changeGameDirToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem changeGameDirToolStripMenuItem1;
		private System.Windows.Forms.Button BtnHash;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panelHash;
        private System.Windows.Forms.ToolStripMenuItem hashPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetSettingsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Difficulty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sublevels;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem thumpNetToolStripMenuItem;
        private System.Windows.Forms.PictureBox picSplashScreen;
        private System.Windows.Forms.Button btnSplashScreen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSplashScreenReset;
        private System.Windows.Forms.Label label5;
    }
}

