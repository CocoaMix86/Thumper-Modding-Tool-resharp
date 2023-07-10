using System;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Thumper_Modding_Tool_resharp
{
	public partial class ThumperModdingTool : Form
	{
		private readonly CommonOpenFileDialog cfd_lvl = new CommonOpenFileDialog() { IsFolderPicker = true, Multiselect = false };
		public ThumperModdingTool()
        {
			InitializeComponent();
		}

		ObservableCollection<LevelTraits> LoadedLevels = new ObservableCollection<LevelTraits>();

		///         ///
		/// EVENTS  ///
		///         ///

		private void Form1_Load(object sender, EventArgs e)
		{
			InitializeTracks(dgvLevels);
			LoadedLevels.CollectionChanged += LoadedLevels_CollectionChanged;

			Read_Config(true);

			if (Properties.Settings.Default.mod_mode == "OFF") {
				//update visual elements on the form
				btnModMode.BackColor = Color.FromArgb(64,0,0);
                btnModMode.ForeColor = Color.Crimson;
                btnModMode.Text = "OFF";
			}
			else {
				//update visual elements on the form
				btnModMode.BackColor = Color.YellowGreen;
                btnModMode.ForeColor = Color.White;
                btnModMode.Text = "ON";
				btnUpdate.Enabled = true;
				btnUpdate.Visible = true;
			}
		}

		private void LoadedLevels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//clear and reload level list DGV whenever this collection updates
			dgvLevels.RowCount = 0;
			foreach (var _level in LoadedLevels) {
				//populate rows with level name, and difficulty strings
				dgvLevels.Rows.Add(new object[] { _level.name, _level.difficulty, _level.sublevels });
			}
		}

		private void dgvLevels_SelectionChanged(object sender, EventArgs e)
		{
            //load selected level's description
			if (dgvLevels.SelectedCells.Count <= 0)
            {
				richDescript.Text = string.Empty;
				return;
			}

			int i = dgvLevels.SelectedCells[0].OwningRow.Index;
			string s = string.Empty;
			if (!string.IsNullOrWhiteSpace(LoadedLevels[i].descript))
			{
				s = LoadedLevels[i].descript;
				s += Environment.NewLine + Environment.NewLine;
            }
            s += $"Author: {LoadedLevels[i].author}";
			richDescript.Text = s;
		}


		///         ///
		/// BUTTONS ///
		///         ///

		private void btnLevelAdd_Click(object sender, EventArgs e)
		{
			if (dgvLevels.RowCount >= 8) {
				MessageBox.Show("Max levels reached already.");
				return;
			}
            //initialize the FolderBrowser to start where the app is launched
            cfd_lvl.Title = "Select the Level Folder";
            cfd_lvl.InitialDirectory = Application.StartupPath;
			if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok)
				AddLevel(cfd_lvl.FileName);
		}

		private void AddLevel(string dir)
		{
            dynamic _leveldata;
            dynamic _levelmaster;
            int sublevels = 0;
            var _path = dir;
            //create dynamic object from parsed JSON
            //this allows me to call each value further down
            if (File.Exists($@"{_path}\LEVEL DETAILS.txt"))
            {
                _leveldata = JsonConvert.DeserializeObject(File.ReadAllText($@"{_path}\LEVEL DETAILS.txt"));
                //try-catch block on parsing master, in case it has issues
                try
                {
                    _levelmaster = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{_path}\master_sequin.txt"), @"#.*", ""));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error parsing:\n{ex.Message} in file \"master_sequin.txt\" for the selected level\n\nLEVEL NOT ADDED");
                    return;
                }
            }
            else
            {
                //if LEVEL DETAILS.txt does not exist, return. Do not add level
                MessageBox.Show("\"LEVEL DETAILS.txt\" for the selected level could not be found.");
                return;
            }
            //check if the level has already been added
            foreach (LevelTraits lt in LoadedLevels)
            {
                //if exists, tell user, then return and do not add level
                if (lt.name == (string)_leveldata.level_name)
                {
                    //MessageBox.Show("That level has already been added");
                    //return;
                }
            }
            //check which sublevels have checkpoint enabled. This determines how many sublevels exist
            foreach (var lvl in _levelmaster["groupings"])
            {
                if ((string)lvl["checkpoint"] == "True")
                    sublevels++;
            }
            //add level to the List, initializing each value from parsed JSON
            LoadedLevels.Add(new LevelTraits()
            {
                name = _leveldata.level_name,
                difficulty = _leveldata.difficulty,
                descript = _leveldata.description,
                path = _path,
                folder_name = Path.GetFileName(_path),
                author = _leveldata.author,
                sublevels = sublevels
            });
            dgvLevels.Rows[dgvLevels.Rows.Count - 1].Selected = true;

            btnLevelRemove.Enabled = true;
        }

		private void btnLevelRemove_Click(object sender, EventArgs e)
		{
			LoadedLevels.RemoveAt(dgvLevels.CurrentRow.Index);
			btnLevelRemove.Enabled = LoadedLevels.Count != 0;
		}

		private void btnLevelUp_Click(object sender, EventArgs e)
		{
			try {
				int totalRows = LoadedLevels.Count;
				// get index of the row for the selected cell
				int rowIndex = dgvLevels.SelectedCells[0].OwningRow.Index;
				if (rowIndex == 0)
					return;
				//move track in list
				LevelTraits selectedTrack = LoadedLevels[rowIndex];
				LoadedLevels.Remove(selectedTrack);
				LoadedLevels.Insert(rowIndex - 1, selectedTrack);
				dgvLevels.Rows[rowIndex - 1].Selected = true;
			}
			catch { }
		}

		private void btnLevelDown_Click(object sender, EventArgs e)
		{
			try {
				int totalRows = LoadedLevels.Count;
				// get index of the row for the selected cell
				int rowIndex = dgvLevels.SelectedCells[0].OwningRow.Index;
				if (rowIndex == totalRows - 1)
					return;
				//move track in list
				LevelTraits selectedLevel = LoadedLevels[rowIndex];
				LoadedLevels.Remove(selectedLevel);
				LoadedLevels.Insert(rowIndex + 1, selectedLevel);
                dgvLevels.Rows[rowIndex + 1].Selected = true;
            }
            catch { }
		}

		private void btnModMode_Click(object sender, EventArgs e)
		{
			if (Thumper_Running())
				return;
			if (Properties.Settings.Default.game_dir == "") {
				MessageBox.Show("Please select a game directory before enabling mods. You can do so from the OPTIONS menu.");
				return;
			}
			if (LoadedLevels.Count == 0 && Properties.Settings.Default.mod_mode == "OFF") {
				MessageBox.Show("No levels loaded. Please add one first.");
				return;
			}

			btnModMode.Text = "Please Wait...";

			if (Properties.Settings.Default.mod_mode == "OFF") {
				Backup_SaveData(Properties.Settings.Default.game_dir);
				Make_Custom_Levels(Properties.Settings.Default.game_dir);
				Make_Custom_Savedata(Properties.Settings.Default.game_dir);
				//set mod mode property in exe and save it
				Properties.Settings.Default.mod_mode = "ON";
				Properties.Settings.Default.Save();
				//update visual elements on the form
				btnModMode.BackColor = Color.YellowGreen;
				btnModMode.ForeColor = Color.White;
                btnModMode.Text = "ON";
				btnUpdate.Enabled = true;
				btnUpdate.Visible = true;
			}
			else {
				Restore_Levels(Properties.Settings.Default.game_dir);
				Restore_Savedata(Properties.Settings.Default.game_dir);
				//set mod mode property in exe and save it
				Properties.Settings.Default.mod_mode = "OFF";
				Properties.Settings.Default.Save();
				//update visual elements on the form
				btnModMode.BackColor = Color.FromArgb(64,0,0);
                btnModMode.ForeColor = Color.Crimson;
                btnModMode.Text = "OFF";
				btnUpdate.Enabled = false;
				btnUpdate.Visible = false;
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Make_Custom_Levels(Properties.Settings.Default.game_dir);
			Make_Custom_Savedata(Properties.Settings.Default.game_dir);
		}

		private void changeGameDirToolStripMenuItem_Click(object sender, EventArgs e) => Read_Config(false);


		///         ///
		/// METHODS ///
		///         ///

		public void InitializeTracks(DataGridView grid)
		{
			//track editor cell formatting
			grid.DefaultCellStyle.Font = new Font(new FontFamily("Arial"), 10);
			grid.DefaultCellStyle.ForeColor = Color.White;
			grid.DefaultCellStyle.SelectionBackColor = Color.FromName("Highlight");
			grid.DefaultCellStyle.SelectionForeColor = Color.FromName("HighlightText");
			grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
			grid.RowHeadersVisible = false;
			grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			grid.ReadOnly = false;
			grid.RowTemplate.Height = 20;

			//double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
			//used to significantly improve rendering performance
			if (!SystemInformation.TerminalServerSession) {
				Type dgvType = grid.GetType();
				PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
				pi.SetValue(grid, true, null);
			}
		}

		public static byte[] StringToByteArray(String hex)
		{
			int NumberChars = hex.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			return bytes;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			textBox2.Text = "";
			byte[] bytes = BitConverter.GetBytes(Hash32(textBox1.Text));
			Array.Reverse(bytes);
			foreach (byte b in bytes)
				textBox2.Text += b.ToString("X").PadLeft(2, '0').ToLower();

			if (textBox2.Text[0] == '0')
				textBox2.Text = textBox2.Text.Substring(1);
		}

        private void hashPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
			panelHash.Visible = hashPanelToolStripMenuItem.Checked;
        }

        private void resetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Properties.Settings.Default.Reset();
			Properties.Settings.Default.Save();
			Application.Restart();
        }

		private void dgvLevels_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (Directory.Exists(data[0]))
				{
					e.Effect = DragDropEffects.Copy;
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}
        private void dgvLevels_DragDrop(object sender, DragEventArgs e)
        {
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (data.Length + LoadedLevels.Count > 8)
			{
				MessageBox.Show("There can only be a total of 8 custom levels at any one time.", "Level Limit Reached");
				return;
			}
			foreach (string dir in data)
			{
				if (Directory.Exists(dir))
					AddLevel(dir);
            }
        }
    }
}
