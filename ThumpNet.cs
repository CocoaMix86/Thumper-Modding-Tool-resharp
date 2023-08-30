using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        private void ThumpNet_Load(object sender, EventArgs e)
        {
            LoadThumpNetAsync();
            if (Settings.Default.thumpnet_compactview)
            {
                compactViewToolStripMenuItem.Text = "Detailed View";
            }
            else
            {
                compactViewToolStripMenuItem.Text = "Compact View";
            }
        }
        WebClient wc;
        BackgroundWorker bgw;
        Action bgw_cb;
        List<ThumpNetLevel> GlobalLevels;
        List<ThumpNetLevel> SearchLevels;
        bool oldest_first = false;
        void LoadThumpNetAsync()
        {
            pnl_levels.Controls.Clear();
            Text = "ThumpNet [Loading...]";
            if (bgw != null)
            {
                if (bgw.IsBusy)
                {
                    bgw_cb = LoadThumpNetAsync;
                    bgw.CancelAsync();
                    return;
                }
                else
                {
                    bgw.Dispose();
                }
            }
            bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                if (e.Cancelled)
                {
                    if (bgw_cb != null)
                    {
                        bgw_cb();
                        bgw_cb = null;
                    }
                }

                Text = "ThumpNet";
            };
            bgw.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                LoadThumpNet(sender, e);
            };
            bgw.RunWorkerAsync();
        }

        void LoadThumpNet(object sender, DoWorkEventArgs e)
        {
            Invoke(new Action(() => { reloadToolStripMenuItem.Enabled = false; }));
            string db_url = "https://docs.google.com/spreadsheets/d/19SbuARLhHfxTcZXDEGzxeQIdpJR0acTPTNtwrnwUtUI/export?format=tsv";
            if (wc != null) wc.Dispose();
            wc = new WebClient();
            string db = wc.DownloadString(db_url);
            string[] rows = db.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Skip(1).ToArray();
            GlobalLevels = new List<ThumpNetLevel>();
            foreach (string row in rows)
            {
                string[] data = row.Split('\t');
                if (data.Length != 8) continue;
                ThumpNetLevel Level = new ThumpNetLevel();
                string[] date_time = data[0].Split(' ');
                if (date_time.Length != 2) continue;
                int[] date_values = date_time[0].Split('/').Select(int.Parse).ToArray();
                int[] time_values = date_time[1].Split(':').Select(int.Parse).ToArray();
                Level.DateUTC = new DateTime(date_values[0], date_values[1], date_values[2], time_values[0], time_values[1], time_values[2]);
                Level.Name = data[1];
                Level.Author = data[2];
                Level.Difficulty = int.Parse(data[3]);
                Level.Song = data[4];
                Level.Description = data[5];
                Level.ThumbnailURL = data[6];
                Level.DownloadURL = data[7];
                if (string.IsNullOrWhiteSpace(Level.DownloadURL)) continue;
                GlobalLevels.Add(Level);
            }
            
            if (bgw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            Invoke(new Action(() => { 
                try { 
                    UpdateLevelList(GlobalLevels);
                    reloadToolStripMenuItem.Enabled = true;
                    SearchLevels = new List<ThumpNetLevel>(GlobalLevels);
                } catch { } 
            }));
        }

        void UpdateLevelList(List<ThumpNetLevel> Levels)
        {
            pnl_levels.Controls.Clear();
            if (Levels == null) return;
            Directory.CreateDirectory($@"ThumpNet\Cache\");
            bool cmpct = Settings.Default.thumpnet_compactview;

            if (oldest_first)
            {
                Levels.Sort((x, y) => DateTime.Compare(x.DateUTC, y.DateUTC));
            }
            else
            {
                Levels.Sort((x, y) => DateTime.Compare(y.DateUTC, x.DateUTC));
            }
            foreach (ThumpNetLevel Level in Levels)
            {
                Panel panel = new Panel();
                panel.Size = new Size(256, cmpct ? 80 : 224);
                //panel.BackColor = Color.FromArgb(32, 0, 0);

                int offset = 0;
                if (!cmpct)
                {
                    offset = 144;
                    PictureBox thumbnail = new PictureBox();
                    thumbnail.SizeMode = PictureBoxSizeMode.Zoom;
                    thumbnail.Size = new Size(256,144);
                    thumbnail.Location = new Point(0, 0);
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
                    
                    panel.Controls.Add(thumbnail);
                }

                Label name = new Label();
                name.Text = Level.Name;
                name.AutoSize = true;
                name.ForeColor = Color.White;
                name.Font = new Font("Trebuchet MS", 12, FontStyle.Bold);
                name.Location = new Point(0, 3+offset);
                Label author = new Label();
                author.Text = $"{Level.Author} • {ThumperModdingTool.DateTime_Ago(Level.DateUTC.ToLocalTime())}";
                author.AutoSize = true;
                author.ForeColor = Color.White;
                author.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
                author.Location = new Point(0, 25+offset);
                panel.Controls.Add(author);
                panel.Controls.Add(name);
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
            }
        }

        public class ThumpNetLevel
        {
            public DateTime DateUTC;
            public string Name, Author, Song, Description, ThumbnailURL, DownloadURL;
            public int Difficulty;
        }

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
                UpdateLevelList(SearchLevels);
            }
            else
            {
                Settings.Default.thumpnet_compactview = true;
                Settings.Default.Save();
                compactViewToolStripMenuItem.Text = "Detailed View";
                UpdateLevelList(SearchLevels);
            }
        }

        private void newestFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newestFirstToolStripMenuItem.Checked = true;
            oldestFirstToolStripMenuItem.Checked = false;
            oldest_first = false;
            UpdateLevelList(SearchLevels);
        }

        private void oldestFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newestFirstToolStripMenuItem.Checked = false;
            oldestFirstToolStripMenuItem.Checked = true;
            oldest_first = true;
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

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchLevels.Clear();

            if (txtSearch.Text == "") {
                SearchLevels = new List<ThumpNetLevel>(GlobalLevels);
                UpdateLevelList(GlobalLevels);
                return;
            }

            foreach (ThumpNetLevel lvl in GlobalLevels) {
                if (lvl.Name.ToLower().Contains(txtSearch.Text.ToLower())) {
                    SearchLevels.Add(lvl);
                }
            }
            UpdateLevelList(SearchLevels);
        }
    }
}
