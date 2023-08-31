using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Thumper_Modding_Tool_resharp.Properties;

namespace Thumper_Modding_Tool_resharp
{
    public partial class ThumpNet : Form
    {
        private ThumperModdingTool ThumperModdingTool;
        public ThumpNet(ThumperModdingTool _ThumperModdingTool)
        {
            InitializeComponent();
            ThumperModdingTool = _ThumperModdingTool;
        }

        public class ThumpNetLevel
        {
            public DateTime DateUTC;
            public string Name, Author, Song, Description, ThumbnailURL, DownloadURL;
            public int Difficulty;
        }

        WebClient wc;
        BackgroundWorker bgw;
        Action bgw_cb;

        List<ThumpNetLevel> Levels;
        string sortorder = "alpha";
        List<Image> rankicons = new List<Image> { Resources.d0, Resources.d1, Resources.d2, Resources.d3, Resources.d4, Resources.d5, Resources.d6, Resources.d7 };

        private void ThumpNet_Load(object sender, EventArgs e)
        {
            LoadThumpNetAsync();
            if (Settings.Default.thumpnet_compactview) compactViewToolStripMenuItem.Text = "Detailed View";
            else compactViewToolStripMenuItem.Text = "Compact View";
        }

        #region METHODS


        void LoadThumpNetAsync()
        {
            // disable reload button, clear list and update text
            reloadToolStripMenuItem.Enabled = false;
            pnl_levels.Controls.Clear();
            Text = "ThumpNet [Loading...]";

            // validate bgw
            if (bgw != null)
            {
                // if theres already a bgw and its running, cancel it first and then run this function afterwards
                if (bgw.IsBusy)
                {
                    bgw_cb = LoadThumpNetAsync;
                    bgw.CancelAsync();
                    return;
                }
                else bgw.Dispose();
            }

            // setup bgw
            bgw = new BackgroundWorker() { WorkerSupportsCancellation = true };
            bgw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                // if the bgw was cancelled and there is a callback, call it
                if (e.Cancelled)
                {
                    if (bgw_cb != null)
                    {
                        bgw_cb();
                        bgw_cb = null;
                    }
                   
                    return;
                }

                Text = "ThumpNet";
                UpdateLevelList();
                reloadToolStripMenuItem.Enabled = true;
            };
            bgw.DoWork += (object sender, DoWorkEventArgs e) => LoadThumpNet(sender, e);

            // run bgw
            bgw.RunWorkerAsync();
        }

        void LoadThumpNet(object sender, DoWorkEventArgs e)
        {
            // download level database
            string db_url = "https://docs.google.com/spreadsheets/d/19SbuARLhHfxTcZXDEGzxeQIdpJR0acTPTNtwrnwUtUI/export?format=tsv";
            wc?.Dispose();
            wc = new WebClient();
            string db;
            try { db = wc.DownloadString(db_url); }
            catch
            {
                MessageBox.Show("Failed to download latest levels. Are you connected to the internet?", "Error");
                Invoke(new Action(() => Close()));
                return;
            }

             // split by new line and remove the header
            string[] rows = db.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Skip(1).ToArray();

            // init global levels
            Levels = new List<ThumpNetLevel>();

            // add each level from database
            foreach (string row in rows)
            {
                // validate 8 columns + make sure has download url
                string[] data = row.Split('\t');
                if (data.Length != 8) continue;

                if (string.IsNullOrWhiteSpace(data[7])) continue;

                // setup utc datetime
                DateTime dt = DateTime.ParseExact(data[0], "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
;
                // add data
                ThumpNetLevel Level = new()
                {
                    DateUTC = dt,
                    Name = data[1],
                    Author = data[2],
                    Difficulty = int.Parse(data[3]),
                    Song = data[4],
                    Description = data[5],
                    ThumbnailURL = data[6],
                    DownloadURL = data[7]
                };

                // add to global levels
                Levels.Add(Level);
            }
            
            // if trying to cancel
            if (bgw.CancellationPending) e.Cancel = true;
        }

        void UpdateLevelList()
        {
            pnl_levels.Controls.Clear();
            if (Levels == null) return;
            Directory.CreateDirectory($@"ThumpNet\Cache\");
            bool cmpct = Settings.Default.thumpnet_compactview;


            // sort by oldest or newest datetime
            if (sortorder == "oldest")
            {
                Levels.Sort((x, y) => DateTime.Compare(x.DateUTC, y.DateUTC));
            }
            else if (sortorder == "newest") {
                Levels.Sort((x, y) => DateTime.Compare(y.DateUTC, x.DateUTC));
            }
            else if (sortorder == "alpha") {
                Levels.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
            else if (sortorder == "difficulty") {
                Levels.Sort((x, y) => y.Difficulty.CompareTo(x.Difficulty));
            }

            int i = 0;
            foreach (ThumpNetLevel Level in Levels)
            {
                Panel panel = new Panel();
                panel.Tag = i;
                panel.Visible = TitleAuthorContainsSearch(Level);
                panel.Size = new Size(256, cmpct ? 80 : 224);
                //panel.BackColor = Color.FromArgb(32, 0, 0);

                //offset is for where to display the author and level name
                //compact mode is 0 since no thumbnail is displayed
                int offset = 0;
                if (!cmpct)
                {
                    offset = 144;
                    //This pciturebox contains the level thumbnail
                    PictureBox thumbnail = new PictureBox();
                    thumbnail.SizeMode = PictureBoxSizeMode.Zoom;
                    thumbnail.Size = new Size(256,144);
                    thumbnail.Location = new Point(0, 0);
                    //if no thumbnail URL found, write level name on black background
                    if (string.IsNullOrWhiteSpace(Level.ThumbnailURL))
                    {
                        Bitmap bmp = new Bitmap(256, 144);
                        Graphics g = Graphics.FromImage(bmp);
                        using SolidBrush b = new SolidBrush(Color.Black);
                        using SolidBrush w = new SolidBrush(Color.White);
                        using Font f = new Font("Trebuchet MS", 12, FontStyle.Bold);
                        Size txtsz = TextRenderer.MeasureText(Level.Name, f);
                        g.FillRectangle(b, g.ClipBounds);
                        g.DrawString(Level.Name, f, w, 128 - txtsz.Width / 2, 72 - txtsz.Height / 2);
                        thumbnail.Image = bmp;
                    }
                    //if thumbnail URL is found, download image
                    else
                    {
                        string cache_fn = $@"ThumpNet\Cache\{Level.Author}_{Level.Name}.thumb";
                        if (!File.Exists(cache_fn))
                        {
                            WebClient wc_thumb = new WebClient();
                            wc_thumb.DownloadFileCompleted += (sender, e) =>
                            {
                                thumbnail.Image = Image.FromFile(cache_fn);
                                wc_thumb.Dispose();
                            };
                            wc_thumb.DownloadFileAsync(new Uri(Level.ThumbnailURL), cache_fn);
                        }
                        else
                        {
                            thumbnail.Image = Image.FromFile(cache_fn);
                        }
                    }
                    //add thumbail to panel so it displays on top
                    panel.Controls.Add(thumbnail);
                }

                //contains the rank icon
                PictureBox rankicon = new PictureBox();
                rankicon.SizeMode = PictureBoxSizeMode.Zoom;
                rankicon.Size = new Size(25, 25);
                rankicon.Location = new Point(-2, 2+offset);
                rankicon.Image = rankicons[Level.Difficulty];
                toolTip1.SetToolTip(rankicon, $@"Difficulty: {"".PadLeft(Level.Difficulty, '◆').PadRight(7, '◇')}");
                //level name
                Label name = new Label();
                name.Text = Level.Name;
                name.AutoSize = true;
                name.ForeColor = Color.White;
                name.Font = new Font("Trebuchet MS", 12, FontStyle.Bold);
                name.Location = new Point(20, 3+offset);
                //author name
                Label author = new Label();
                author.Text = $"{Level.Author} • {ThumperModdingTool.DateTime_Ago(Level.DateUTC)}";
                author.AutoSize = true;
                author.ForeColor = Color.White;
                author.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
                author.Location = new Point(0, 25+offset);
                //add controls to panel so they're visible
                panel.Controls.Add(author);
                panel.Controls.Add(name);
                panel.Controls.Add(rankicon);
                //download and add level button
                Button load = new Button();
                load.Text = "Download";
                load.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                load.Size = new Size(256, 32);
                load.ForeColor = Color.Crimson;
                load.FlatStyle = FlatStyle.Flat;
                load.BackColor = Color.FromArgb(64, 0, 0);
                load.Location = new Point(0, 45+offset);

                string fn = $@"ThumpNet\Cache\{Level.Author}_{Level.Name}";
                string info_fn = $"{fn}.info";
                string zip_fn = $"{fn}.zip";
                bool download = true;
                bool extract = false;

                Action update_text = () =>
                {
                    if (File.Exists(info_fn))
                    {
                        string[] info = File.ReadAllLines(info_fn);
                        if (info[0] != Level.DownloadURL)
                        {
                            load.Text = "Update";
                            download = true;
                        }
                        else
                        {
                            load.Text = "Add to List";
                            load.ForeColor = Color.Crimson;
                            load.BackColor = Color.FromArgb(64, 0, 0);
                            download = false;
                            if (info.Length < 2 || info[1] == "0")
                            {
                                extract = true;
                            }
                            else
                            {
                                foreach (LevelTraits l in ThumperModdingTool.LoadedLevels)
                                {
                                    if (l.path == fn)
                                    {
                                        load.Text = "Already Added";
                                        load.ForeColor = Color.White;
                                        load.BackColor = Color.Green;
                                        download = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    load.Enabled = true;
                    pnl_levels.ScrollControlIntoView(load);
                };
                update_text();


                load.Click += (sender, e) =>
                {
                    load.Enabled = false;
                    load.Text = "...";

                    Action load_folder = () =>
                    {
                        string[] dirs = Directory.GetDirectories(fn);
                        string[] files = Directory.GetFiles(fn);
                        if (files.Length == 0 && dirs.Length == 1)
                        {
                            string new_fn = $"{fn}_empty";
                            Directory.Move(fn, new_fn);
                            dirs = Directory.GetDirectories(new_fn);
                            Directory.Move(dirs[0], fn);
                            Directory.Delete(new_fn);
                        }
                        if (ThumperModdingTool.LoadedLevels.Count == 8)
                        {
                            MessageBox.Show("There is already 8 levels loaded. This level has been cached, so remove one of your other pre-laoded levels then you can load this.");
                        }
                        else
                        {
                            ThumperModdingTool.AddLevel(fn, false);
                        }
                        update_text();
                    };

                    Action load_zip = () =>
                    {
                        load.Text = "Extracting...";
                        BackgroundWorker bgw_extract = new BackgroundWorker();
                        bgw_extract.DoWork += (object sender, DoWorkEventArgs e) =>
                        {
                            if (Directory.Exists(fn)) Directory.Delete(fn, true);
                            Directory.CreateDirectory(fn);
                            System.IO.Compression.ZipFile.ExtractToDirectory(zip_fn, fn);
                            File.Delete(zip_fn);
                        };
                        bgw_extract.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
                        {
                            File.WriteAllText(info_fn, Level.DownloadURL + Environment.NewLine + "1");
                            load_folder();
                            bgw_extract.Dispose();
                        };
                        bgw_extract.RunWorkerAsync();
                    };

                    Action do_download = () =>
                    {
                        // this cant be cancelled right now, need to implement that but too lazy right now.
                        WebClient wc_dl = new WebClient();
                        wc_dl.Proxy = null;
                        wc_dl.DownloadFileCompleted += (sender, e) =>
                        {
                            File.WriteAllText(info_fn, Level.DownloadURL + Environment.NewLine + "0");
                            load_zip();
                            wc_dl.Dispose();
                        };
                        wc_dl.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                        {
                            load.Text = $"{e.ProgressPercentage}%";
                        };
                        if (File.Exists(zip_fn)) File.Delete(zip_fn);
                        wc_dl.DownloadFileAsync(new Uri(Level.DownloadURL), zip_fn);
                    };

                    if (download)
                    {
                        do_download();
                    }
                    else
                    {
                        if (!Directory.Exists(fn) || extract)
                        {
                            if (File.Exists(zip_fn))
                            {
                                load_zip();
                            }
                            else
                            {
                                do_download();
                            }
                        }
                        else
                        {
                            load_folder();
                        }
                    }
                    
                };

                panel.Controls.Add(load);


                pnl_levels.Controls.Add(panel);

                i++;
            }
        }

        bool TitleAuthorContainsSearch(ThumpNetLevel lvl)
        {
            if (txtSearch.Text == "") return true;

            if (lvl.Name.ToLower().Contains(txtSearch.Text.ToLower()) ||
                lvl.Author.ToLower().Contains(txtSearch.Text.ToLower()))
                return true;

            return false;
        }
        #endregion

        #region EVENTS
        private void ThumpNet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgw != null && bgw.IsBusy)
            {
                bgw.CancelAsync();
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadThumpNetAsync();
        }

        private void compactViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.thumpnet_compactview)
            {
                Settings.Default.thumpnet_compactview = false;
                Settings.Default.Save();
                compactViewToolStripMenuItem.Text = "Compact View";
                UpdateLevelList();
            }
            else
            {
                Settings.Default.thumpnet_compactview = true;
                Settings.Default.Save();
                compactViewToolStripMenuItem.Text = "Detailed View";
                UpdateLevelList();
            }
        }

        private void newestFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oldestFirstToolStripMenuItem.Checked = false;
            alphabeticalToolStripMenuItem.Checked = false;
            difficultyToolStripMenuItem.Checked = false;
            sortorder = "newest";
            UpdateLevelList(SearchLevels);
        }

        private void oldestFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newestFirstToolStripMenuItem.Checked = false;
            alphabeticalToolStripMenuItem.Checked = false;
            difficultyToolStripMenuItem.Checked = false;
            sortorder = "oldest";
            UpdateLevelList(SearchLevels);
        }

        private void alphabeticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newestFirstToolStripMenuItem.Checked = false;
            oldestFirstToolStripMenuItem.Checked = false;
            difficultyToolStripMenuItem.Checked = false;
            sortorder = "alpha";
            UpdateLevelList(SearchLevels);
        }

        private void difficultyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newestFirstToolStripMenuItem.Checked = false;
            oldestFirstToolStripMenuItem.Checked = false;
            alphabeticalToolStripMenuItem.Checked = false;
            sortorder = "difficulty";
            UpdateLevelList(SearchLevels);
        }

        private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists($@"ThumpNet\Cache\"))
            {
                var files = Directory.GetFiles($@"ThumpNet\Cache\");
                var folders = Directory.GetDirectories($@"ThumpNet\Cache\");
                if (files.Length > 0 || folders.Length > 0)
                {
                    if (MessageBox.Show(
                        $"Warning: You're about to remove {files.Length} file(s) and {folders.Length} folder(s)." +
                        "\nDo you want to continue?",
                        "Clear ThumpNet Cache",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.Delete($@"ThumpNet\Cache\", true);
                        }
                        catch { }

                        LoadThumpNetAsync();
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // make sure theres > 0 levels, and also its the same as our level object
            if (pnl_levels.Controls.Count == 0) return;
            if (pnl_levels.Controls.Count != Levels.Count) return;

            // make each panel visible depending on the search query
            foreach (Panel lvl in pnl_levels.Controls)
                lvl.Visible = TitleAuthorContainsSearch(Levels[(int)lvl.Tag]);
        }
        #endregion
    }
}
