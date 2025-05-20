using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using DDS;

namespace Thumper_Mod_Loader
{
    public partial class ThumperModdingTool : Form
    {
        #region Form Constructor
        public ThumperModdingTool()
        {
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripOverride();
            ((DataGridViewImageColumn)dgvLevels.Columns[0]).ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBufferForms(dgvLevels);
            LoadedLevels.CollectionChanged += LoadedLevels_CollectionChanged;
            Read_Config(true);

            if (!Properties.Settings.Default.mod_mode) {
                // update visual elements on the form
                btnModMode.BackColor = Color.FromArgb(64, 0, 0);
                btnModMode.ForeColor = Color.Crimson;
                btnModMode.Text = "is OFF";
            }
            else {
                // update visual elements on the form
                btnModMode.BackColor = Color.YellowGreen;
                btnModMode.ForeColor = Color.White;
                btnModMode.Text = "is ON";
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
            }

            // load all previously loaded levels
            if (Properties.Settings.Default.level_paths == null)
                Properties.Settings.Default.level_paths = new List<string>();
            foreach (string s in Properties.Settings.Default.level_paths)
                AddLevel(new FileInfo(s), true);

            // custom splash screen
            picSplashScreen.AllowDrop = true;
            LoadSplashScreen();

            // Update title to reflect version number
            this.Text = Title;
        }

        private void ThumperModdingTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.level_paths.Clear();
            foreach (LevelTraits lt in LoadedLevels) {
                Properties.Settings.Default.level_paths.Add(lt.FilePath.FullName);
            }
            Properties.Settings.Default.Save();
        }
        #endregion
        #region Variables
        private readonly string Title = $"Thumper Mod Loader v3alpha51";
        private readonly OpenFileDialog cfd_lvl = new() { Multiselect = false };
        private readonly OpenFileDialog ofd_img = new() { Title = "Choose Image", Filter = "DDS files(*.DDS)|*.DDS" };
        //private ThumpNet tnet = null;
        public static ObservableCollection<LevelTraits> LoadedLevels = new();
        private bool _ChangesMade = false;
        public bool ChangesMade
        {
            get { return _ChangesMade; }
            set {
                _ChangesMade = value;
                Text = Title + (ChangesMade ? " [Changes Made]" : "");
            }
        }
        #endregion
        #region Event Handlers
        ///         ///
        /// EVENTS  ///
        ///         ///
        private void LoadedLevels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int i = 0;
            if (dgvLevels.GetCellCount(DataGridViewElementStates.Selected) > 0)
                i = dgvLevels.SelectedCells[0].RowIndex;

            //clear and reload level list DGV whenever this collection updates
            dgvLevels.RowCount = 0;
            foreach (var _level in LoadedLevels) {
                //populate rows with level name, and difficulty strings
                dgvLevels.Rows.Add(new object[] { _level.thumbnail, _level.Name, Properties.Resources.ResourceManager.GetObject(_level.Difficulty.ToLower()), _level.Sublevels });
                dgvLevels.Rows[^1].Height = 50;
            }

            if (i - 1 >= 0) dgvLevels.Rows[i - 1].Selected = true;
        }

        private void dgvLevels_SelectionChanged(object sender, EventArgs e)
        {
            // load selected level's description
            if (dgvLevels.SelectedCells.Count <= 0) {
                richDescript.Text = string.Empty;
                return;
            }

            int i = dgvLevels.CurrentRow.Index;
            lblCreator.Text = $"Creator: {LoadedLevels[i].Authors}";
            richDescript.Text = $"{LoadedLevels[i].Description}";
            pictureDifficulty.Image = (Image)Properties.Resources.ResourceManager.GetObject(LoadedLevels[i].Difficulty!.ToLower())!;
        }

        private void dgvLevels_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Directory.Exists(data[0]) || (File.Exists(data[0]) && data[0].EndsWith(".tcl", StringComparison.OrdinalIgnoreCase))) {
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
            if (data.Length + LoadedLevels.Count > 8) {
                MessageBox.Show("There can only be a total of 8 custom levels at any one time.", "Thumper Mod Loader");
                return;
            }
            foreach (string dir in data) {
                if (Directory.Exists(dir)) {
                    if (Directory.GetFiles(dir, "*.TCL", SearchOption.AllDirectories).Any()) {
                        FileInfo _locateTCL = new FileInfo(Directory.GetFiles(dir, "*.TCL", SearchOption.AllDirectories).FirstOrDefault());
                        if (_locateTCL != null)
                            AddLevel(_locateTCL, false);
                    }
                    else {
                        MessageBox.Show("That folder does not appear to contain level data (a .TCL file)", "Thumper Mod Loader");
                    }
                }
                else if (File.Exists(dir)) {
                    FileInfo _locateTCL = new FileInfo(dir);
                    AddLevel(_locateTCL, false);
                }
            }
        }

        private void picSplashScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                var filename = "image.jpg";
                var path = Path.Combine(Path.GetTempPath(), filename);
                picSplashScreen.Image.Save(path);
                var paths = new[] { path };
                picSplashScreen.DoDragDrop(new DataObject(DataFormats.FileDrop, paths), DragDropEffects.Copy);
                File.Delete(path);
            }
        }
        #endregion
        #region Buttons
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
            cfd_lvl.Filter = "Thumper Custom Level (*.TCL; LEVEL DETAILS.txt)|*.TCL;LEVEL DETAILS.txt";
            cfd_lvl.InitialDirectory = Application.StartupPath;
            if (cfd_lvl.ShowDialog() == DialogResult.OK)
                AddLevel(new FileInfo(cfd_lvl.FileName), false);
        }

        private void btnLevelRemove_Click(object sender, EventArgs e)
        {
            // get selected row index
            if (dgvLevels.GetCellCount(DataGridViewElementStates.Selected) == 0)
                return;

            int i = dgvLevels.SelectedRows[0].Index;
            // get level traits
            var Level = LoadedLevels[i];
            // remove from loaded levels
            LoadedLevels.Remove(Level);
            ///Remove the path from the apps internal list so it doesn't load at reload
            Properties.Settings.Default.level_paths.Remove(Level.FilePath.FullName);
            Properties.Settings.Default.Save();
            // disable remove button if no levels are left
            btnLevelRemove.Enabled = LoadedLevels.Count != 0;
            // changes made
            ChangesMade = true;
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
                ChangesMade = true;

            } catch { }
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
                ChangesMade = true;
            } catch { }
        }

        private void btnModMode_Click(object sender, EventArgs e)
        {
            if (Thumper_Running())
                return;
            if (Properties.Settings.Default.game_dir == "") {
                MessageBox.Show("Please select a game directory before enabling mods. You can do so from the OPTIONS menu.", "Error");
                return;
            }
            if (LoadedLevels.Count == 0 && !Properties.Settings.Default.mod_mode) {
                MessageBox.Show("No levels loaded. Please add one first.", "Error");
                return;
            }

            btnModMode.Text = "Please Wait...";

            // turn it on
            if (!Properties.Settings.Default.mod_mode) {
                ModModeON();
            }
            // turn it off
            else {
                ModModeOFF();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvLevels.Rows.Count > 0) {
                Backup_SaveData(Properties.Settings.Default.game_dir);
                Make_Custom_Levels(Properties.Settings.Default.game_dir);
                ///Make_Custom_Savedata(Properties.Settings.Default.game_dir);

            }
            else if (Properties.Settings.Default.mod_mode) ModModeOFF();

            ChangesMade = false;
        }

        private void btnSplashScreen_Click(object sender, EventArgs e)
        {
            // open file dialog
            if (ofd_img.ShowDialog() != DialogResult.OK) return;

            // load dds
            DDSImage img = null;
            bool failed = false;
            try { img = DDSImage.Load(ofd_img.FileName); } catch { failed = true; }

            // if failed to load dds / invalid dds
            if (failed || img == null || img.Images.Length == 0) {
                MessageBox.Show("Failed to load this DDS image.\nMake sure your DDS is a standard resolution. " +
                    "The original image has a resolution of 512x512, and is recommended.", "Error");
                return;
            }

            // write dds to splash screen file
            List<byte> data = new();
            data.AddRange(new byte[] { 14, 0, 0, 0 });
            data.AddRange(File.ReadAllBytes(ofd_img.FileName));
            File.WriteAllBytes("lib/b868db07.pc", data.ToArray());
            LoadSplashScreen();

            // require update
            ChangesMade = true;
        }

        private void btnSplashScreenReset_Click(object sender, EventArgs e)
        {
            File.Copy("lib/cocoasplash.pc", "lib/b868db07.pc", true);
            LoadSplashScreen();
            ChangesMade = true;
        }

        private void BtnHash_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            byte[] bytes = BitConverter.GetBytes(Hash32(textBox1.Text));
            Array.Reverse(bytes);
            foreach (byte b in bytes)
                textBox2.Text += b.ToString("X").PadLeft(2, '0').ToLower();

            if (textBox2.Text[0] == '0')
                textBox2.Text = textBox2.Text.Substring(1);
        }

        private void changeGameDirToolStripMenuItem_Click(object sender, EventArgs e) => Read_Config(false);
        private void hashPanelToolStripMenuItem_Click(object sender, EventArgs e) => panelHash.Visible = !panelHash.Visible;
        private void btnHashClose_Click(object sender, EventArgs e) => panelHash.Visible = false;
        private void lblCustomDiffHelp_Click(object sender, EventArgs e) => new ImageMessageBox("difficultyhelp").Show();
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutBox1().Show();
        private void discordServerToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://discord.com/invite/gTQbquY");
        private void githubToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/CocoaMix86/Thumper-Custom-Level-Editor");
        private void donateTipToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://ko-fi.com/I2I5ZZBRH");

        private void thumpNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            if (tnet == null || tnet.IsDisposed) tnet = new ThumpNet(this);
            tnet.Show();
            tnet.SetDesktopLocation(Location.X + Width, Location.Y);
            tnet.Select();
            */
        }

        private void resetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
            Application.Restart();
        }
        #endregion
        #region Methods
        ///
        /// METHODS
        ///
        void ModModeOFF()
        {
            Restore_Levels(Properties.Settings.Default.game_dir);
            ///Restore_Savedata(Properties.Settings.Default.game_dir);
            //set mod mode property in exe and save it
            Properties.Settings.Default.mod_mode = false;
            Properties.Settings.Default.Save();
            //update visual elements on the form
            btnModMode.BackColor = Color.FromArgb(64, 0, 0);
            btnModMode.ForeColor = Color.Crimson;
            btnModMode.Text = "is OFF";
            btnUpdate.Enabled = false;
            btnUpdate.Visible = false;
        }

        void ModModeON()
        {
            ///Backup_SaveData(Properties.Settings.Default.game_dir);
            Make_Custom_Levels(Properties.Settings.Default.game_dir);
            ///Make_Custom_Savedata(Properties.Settings.Default.game_dir);
            //set mod mode property in exe and save it
            Properties.Settings.Default.mod_mode = true;
            Properties.Settings.Default.Save();
            //update visual elements on the form
            btnModMode.BackColor = Color.YellowGreen;
            btnModMode.ForeColor = Color.White;
            btnModMode.Text = "is ON";
            btnUpdate.Enabled = true;
            btnUpdate.Visible = true;
            ChangesMade = false;
        }

        public void AddLevel(FileInfo TCL, bool startup)
        {
            dynamic ProjectJSON;
            dynamic MasterJSON;
            int sublevels = 0;
            //create dynamic object from parsed JSON
            //this allows me to call each value further down
            if (!TCL.Exists) {
                MessageBox.Show($@"Could not find the custom level {TCL.Name} at {TCL.DirectoryName}. Was it deleted?", "Thumper Mod Loader");
                return;
            }

            /*if (TCL.Directory.GetFiles("LEVEL DETAILS.txt", SearchOption.AllDirectories).Any()) {
                MessageBox.Show($"This custom level contains a LEVEL DETAILS.txt file. Levels created with TCLE v2 are not compatible with mod loader 3.0 and higher.", "Thumper Mod Loader");
                return;
            }*/

            ProjectJSON = LoadFileLock(TCL.FullName);
            //try-catch block on parsing master, in case it has issues
            try {
                FileInfo _locateMaster = TCL.Name == "LEVEL DETAILS.txt" ? TCL.Directory.GetFiles("master_sequin.txt").FirstOrDefault() : TCL.Directory.GetFiles("*.master", SearchOption.AllDirectories).FirstOrDefault();
                if (_locateMaster != null)
                    MasterJSON = LoadFileLock(_locateMaster.FullName);
                else {
                    MessageBox.Show($@"No .Master file exists for {TCL.Name}.", "Thumper Mod Loader");
                    return;
                }
            } catch (Exception ex) {
                MessageBox.Show($"error parsing:\n{ex.Message} in .master file the selected level\n\nLEVEL NOT ADDED", "Thumper Mod Loader");
                return;
            }

            //check which sublevels have checkpoint enabled. This determines how many sublevels exist
            foreach (var lvl in MasterJSON["groupings"]) {
                if ((string)lvl["checkpoint"] == "True")
                    sublevels++;
            }
            //add level to the List, initializing each value from parsed JSON
            LevelTraits NewLevel = new LevelTraits() {
                Name = ProjectJSON.level_name,
                Difficulty = ProjectJSON.difficulty,
                Description = ProjectJSON.description,
                FilePath = TCL,
                Authors = ProjectJSON.author,
                Sublevels = sublevels,
                EditorVersion = TCL.Extension == ".txt" ? 2 : 3
            };
            NewLevel.thumbnail = NewLevel.FilePath.Directory.GetFiles("thumbnail.png", SearchOption.AllDirectories).Any() ? Image.FromFile(NewLevel.FilePath.Directory.GetFiles("thumbnail.png", SearchOption.AllDirectories).First().FullName) : null;
            LoadedLevels.Add(NewLevel);
            dgvLevels.Rows[dgvLevels.Rows.Count - 1].Selected = true;
            ///Add level to apps internal list of loaded levels
            ///It uses this to repopulate the list next time it closes/opens
            if (!startup) {
                Properties.Settings.Default.level_paths.Add(TCL.FullName);
                Properties.Settings.Default.Save();
            }

            btnLevelRemove.Enabled = true;
            ChangesMade = true;
        }

        public void DoubleBufferForms(DataGridView grid)
        {
            //double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
            //used to significantly improve rendering performance
            if (!SystemInformation.TerminalServerSession) {
                Type dgvType = grid.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(grid, true, null);
            }
        }

        void LoadSplashScreen()
        {
            List<byte> data = File.ReadAllBytes("lib/b868db07.pc").ToList();
            data.RemoveRange(0, 4);
            DDSImage img = DDSImage.Load(data.ToArray());
            if (img.Images.Length > 0) {
                picSplashScreen.Image = img.Images[0];
            }
        }
        #endregion

        private void chkNewTitleScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.mod_mode) {
                if (chkNewTitleScreen.Checked) {
                    File.Copy("lib/d0d6149c.pc", $@"{Properties.Settings.Default.game_dir}\cache\{Path.GetFileName("lib/d0d6149c.pc")}", true);
                }
                else {
                    File.Copy("lib/original/d0d6149c.pc", $@"{Properties.Settings.Default.game_dir}\cache\{Path.GetFileName("lib/original/d0d6149c.pc")}", true);
                }
            }
        }
    }
}
