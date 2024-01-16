using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            menuStrip1.Renderer = new MyRenderer();
            toolstripSortButtons.Renderer = new ToolStripOverride();
            compactView = Settings.Default.thumpnet_compactview;

            // Check for a local copy of thumpnet
            // If a local version is accessable, use it
            // Otherwise use the main
            LevelRequestBody countRequestBody = new LevelRequestBody();
            countRequestBody.offset = 0;
            countRequestBody.limit = 0;

            urlBase = "http://127.0.0.2";
            Console.WriteLine("Attempting local thumpnet connection");
            var responseObject = MakeLevelPostRequest(countRequestBody);

            // Local copy not found
            if (responseObject == null) {
                urlBase = "https://thumpnet.anthofoxo.xyz";
                Console.WriteLine("Attempting remote thumpnet connection");
                responseObject = MakeLevelPostRequest(countRequestBody);

                if (responseObject == null) throw new ApplicationException("ThumpNet is not accessable");
                else Console.WriteLine("Using remote thumpnet");

            } else Console.WriteLine("Using local thumpnet");

            numLevels = responseObject.count;
            urlDl = urlBase + "/cdn/";
        }

        public class ThumpNetLevel
        {
            public DateTime DateUTC;
            public string? ThumbnailURL;
            public string Name, Author, Song, Description, DownloadURL;
            public int Difficulty;
            public Panel LevelPanel;
            public bool DescriptionExpanded;
        }

        BackgroundWorker bgw;
        Action bgw_cb;

        List<ThumpNetLevel> Levels;
        string sortorder = "time";
        bool sortdirection = true;
        List<Image> rankicons = new List<Image> { Resources.d0, Resources.d1, Resources.d2, Resources.d3, Resources.d4, Resources.d5, Resources.d6, Resources.d7 };
        bool compactView;

        int numLevels;
        string urlBase;
        string urlDl;

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
                LevelPanelSort();
                reloadToolStripMenuItem.Enabled = true;
                if (Settings.Default.thumpnet_compactview)
                    SetPanelCompact();
            };
            bgw.DoWork += (object sender, DoWorkEventArgs e) => LoadThumpNet(sender, e);

            // run bgw
            bgw.RunWorkerAsync();
        }


        public class LevelRequestBody
        {
            public int? offset;
            public int? limit;
            public List<string> expand = new();
        }

        public class ExpandedUser
        {
            public int id;
            public string username;

            public override string ToString()
            {
                return username;
            }
        }

        public class LevelResponseBodyLevel
        {
            public int id;
            public string name;
            public string? content;
            public string? thumbnail;
            public string? description;
            public int? difficulty;
            public int? bpm;
            public int? sublevels;
            public string? song;
            public string timestamp;
            public ExpandedUser uploader;
            public IList<ExpandedUser>? authors;
        }

        public class LevelResponseBody
        {
            public int count;
            public IList<LevelResponseBodyLevel> levels;
        }

        LevelResponseBody? MakeLevelPostRequest(LevelRequestBody requestBody)
        {
            try
            {
                string jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                using var client = new HttpClient();
                HttpResponseMessage httpResponse = client.PostAsync($"{urlBase}/api/v1/level/", new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")).Result;

                if (httpResponse.StatusCode != HttpStatusCode.OK) return null;

                string responseBody = httpResponse.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<LevelResponseBody>(responseBody);
            } catch(Exception) {
                return null;
            }
        }

        void LoadThumpNet(object sender, DoWorkEventArgs e)
        {
            // init global levels
            Levels = new List<ThumpNetLevel>();

            LevelRequestBody requestBody = new LevelRequestBody();
            requestBody.expand.Add("level");
            requestBody.expand.Add("user");
            requestBody.offset = 0;
            requestBody.limit = numLevels;

            LevelResponseBody levels = MakeLevelPostRequest(requestBody);

            // add each level from database
            foreach (var level in levels.levels)
            {
                if(level.content == null) continue;

                // setup utc datetime
                DateTime dt = DateTime.ParseExact(level.timestamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

                var authorsIList = level.authors ?? new List<ExpandedUser>();

                string authorString;
                if (authorsIList.Count == 0) authorString = level.uploader.username;
                else authorString = string.Join(", ", authorsIList);

                // add data
                ThumpNetLevel Level = new()
                {
                    DateUTC = dt,
                    Name = level.name,
                    Author = authorString,
                    Difficulty = level.difficulty ?? 0,
                    Song = level.song ?? "",
                    Description = level.description ?? "",
                    ThumbnailURL = level.thumbnail,

                    // Important, thumbails are updated to use uuid names, this ensures thumbnails will work for all level names
                    // Downloaded content/zip files do not do this, please fix :>
                    DownloadURL = (level.content == null) ? "" : $"{urlDl}{level.content}",
                    //
                    DescriptionExpanded = false
                };

                // add to global levels
                Levels.Add(Level);
            }

            // if trying to cancel
            if (bgw.CancellationPending) e.Cancel = true;
        }

        void UpdateLevelList()
        {
            if (Levels == null) return;
            Directory.CreateDirectory($@"ThumpNet\Cache\");

            //id per panel. Helps with search
            int i = 0;
            foreach (ThumpNetLevel Level in Levels)
            {
                Level.LevelPanel = new Panel();
                Panel panel = Level.LevelPanel;
                panel.Name = i.ToString();
                panel.Visible = TitleAuthorContainsSearch(Level);
                panel.Size = new Size(256, /*cmpct ? 80 :*/ 224);
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.MouseEnter += levelpanel_MouseEnter;
                panel.MouseLeave += levelpanel_MouseLeave;
                //panel.BackColor = Color.FromArgb(32, 0, 0);

                //offset is for where to display the author and level name
                //compact mode is 0 since no thumbnail is displayed
                int offset = 0;
                //if (!cmpct)
                {
                    offset = 144;
                    //This pciturebox contains the level thumbnail
                    PictureBox thumbnail = new PictureBox();
                    thumbnail.Tag = "thumbnail";
                    thumbnail.Name = "thumbnail";
                    thumbnail.SizeMode = PictureBoxSizeMode.Zoom;
                    thumbnail.Size = new Size(256,144);
                    thumbnail.Location = new Point(0, 0);
                    //if no thumbnail URL found, write level name on black background
                    if (Level.ThumbnailURL == null)
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
                        //string cache_fn = $@"ThumpNet\Cache\{Level.Author}_{Level.Name}.thumb";
                        string cache_filename = $@"ThumpNet\Cache\{Level.ThumbnailURL}.thumb";
                        if (!File.Exists(cache_filename))
                        {
                            WebClient wc_thumb = new WebClient();
                            wc_thumb.DownloadFileCompleted += (sender, e) =>
                            {
                                try
                                {
                                    thumbnail.Image = Image.FromFile(cache_filename);
                                    wc_thumb.Dispose();
                                } catch(Exception e2) {
                                    Console.WriteLine(e2.Message);
                                }
                            };
                            Uri uri = new Uri($"{urlDl}{Level.ThumbnailURL}");
                            wc_thumb.DownloadFileAsync(uri, cache_filename);
                        }
                        else
                        {
                            try
                            {
                                thumbnail.Image = Image.FromFile(cache_filename);
                            } catch (OutOfMemoryException e2) {
                                Console.WriteLine(e2.Message);
                            }
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
                rankicon.Anchor = AnchorStyles.Bottom;
                toolTip1.SetToolTip(rankicon, $@"Difficulty: {"".PadLeft(Level.Difficulty, '◆').PadRight(7, '◇')}");
                //info/description button
                PictureBox infoicon = new PictureBox();
                infoicon.SizeMode = PictureBoxSizeMode.Zoom;
                infoicon.Size = new Size(15, 15);
                infoicon.Location = new Point(panel.Width - 17, 2 + offset);
                infoicon.Image = Resources.icon_info_32;
                infoicon.Anchor = AnchorStyles.Bottom;
                infoicon.Click += infoicon_Click;
                infoicon.Cursor = System.Windows.Forms.Cursors.Hand;
                //level name
                Label name = new Label();
                name.Text = Level.Name;
                name.AutoSize = true;
                name.ForeColor = Color.White;
                name.Font = new Font("Trebuchet MS", 12, FontStyle.Bold);
                name.Location = new Point(20, 3+offset);
                name.Anchor = AnchorStyles.Bottom;
                //author name
                Label author = new Label();
                author.Text = $"{Level.Author} • {ThumperModdingTool.DateTime_Ago(Level.DateUTC)}";
                author.Name = "labelAuthor";
                author.AutoSize = true;
                author.ForeColor = Color.White;
                author.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
                author.Location = new Point(0, 25+offset);
                author.Cursor = System.Windows.Forms.Cursors.Hand;
                author.Click += (sender, e) => { txtSearch.Text = (sender as Label).Text.Split(new string[] { " • " }, StringSplitOptions.None)[0]; };
                author.Anchor = AnchorStyles.Bottom;
                //toolTip1.SetToolTip(author, $"Uploaded: {Level.DateUTC}");
                //add controls to panel so they're visible
                panel.Controls.Add(infoicon);
                panel.Controls.Add(author);
                panel.Controls.Add(name);
                panel.Controls.Add(rankicon);
                //download and add level button
                Button load = new Button();
                load.Text = "Download";
                load.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                load.Size = new Size(252, 32);
                load.ForeColor = Color.Crimson;
                load.FlatStyle = FlatStyle.Flat;
                load.BackColor = Color.FromArgb(64, 0, 0);
                load.Location = new Point(2, 45+offset);
                load.Paint += Button1_Paint;
                load.Anchor = AnchorStyles.Bottom;

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
                        do_download();
                    else
                    {
                        if (!Directory.Exists(fn) || extract)
                        {
                            if (File.Exists(zip_fn))
                                load_zip();
                            else
                                do_download();
                        }
                        else
                            load_folder();
                    }
                };
                panel.Controls.Add(load);

                i++;
            }
        }

        void LevelPanelSort()
        {
            pnl_levels.Controls.Clear();


            // sort by oldest or newest datetime
            if (sortorder == "time") {
                Levels.Sort((x, y) => DateTime.Compare(x.DateUTC, y.DateUTC));
            }
            else if (sortorder == "alphabetical") {
                Levels.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
            else if (sortorder == "difficulty") {
                Levels.Sort((x, y) => y.Difficulty.CompareTo(x.Difficulty));
            }
            else if (sortorder == "author") {
                Levels.Sort((x, y) => x.Author.CompareTo(y.Author));
            }

            if (!sortdirection)
                Levels.Reverse();

            foreach (ThumpNetLevel Level in Levels) {
                pnl_levels.Controls.Add(Level.LevelPanel);
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
            if (compactView)
            {
                Settings.Default.thumpnet_compactview = false;
                compactView = false;
                Settings.Default.Save();
                compactViewToolStripMenuItem.Text = "Compact View";
                //LevelPanelSort();
                foreach (ThumpNetLevel Level in Levels) {
                    Level.LevelPanel.Height += 144;
                    Level.LevelPanel.Controls.Find("thumbnail", true)[0].Visible = true;
                }
            }
            else
            {
                Settings.Default.thumpnet_compactview = true;
                compactView = true;
                Settings.Default.Save();
                compactViewToolStripMenuItem.Text = "Detailed View";
                //LevelPanelSort();
                SetPanelCompact();
            }
        }

        private void SetPanelCompact()
        {
            foreach (ThumpNetLevel Level in Levels) {
                Level.LevelPanel.Height -= 144;
                Level.LevelPanel.Controls.Find("thumbnail", true)[0].Visible = false;
            }
        }

        private void toolstripSort_Click(object sender, EventArgs e)
        {
            sortorder = (sender as ToolStripButton).Tag.ToString();
            LevelPanelSort();
        }

        private void toolstripSortDirection_Click(object sender, EventArgs e)
        {
            sortdirection = !sortdirection;
            LevelPanelSort();
        }

        //Clicking Options will total the cache in MB and then display it on the clear cache button
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get array of all file names.
            string[] files = Directory.GetFiles($@"ThumpNet\Cache\", "*.*", SearchOption.AllDirectories);

            // Calculate total bytes of all files in a loop.
            long foldersize = 0;
            foreach (string name in files) {
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                foldersize += info.Length;
            }

            clearCacheToolStripMenuItem.Text = $@"Clear Cache ({(foldersize/1024/1024).ToString()}MB)";
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

        private void openCacheFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists($@"ThumpNet\Cache\")) {
                System.Diagnostics.Process.Start("explorer.exe", $@"ThumpNet\Cache\");
            }
            else {
                MessageBox.Show($@"Could not locate ThumpNet\Cache\");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // make each panel visible depending on the search query
            foreach (ThumpNetLevel Level in Levels) {
                if (TitleAuthorContainsSearch(Level)) {
                    Level.LevelPanel.Visible = true;
                }
                else
                    Level.LevelPanel.Visible = false;
                //Level.LevelPanel.Visible = TitleAuthorContainsSearch(Level);
            }
        }
        #endregion

        private void btnTxtSearchClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void Button1_Paint(object sender, PaintEventArgs e)
        {
            dynamic btn = (Button)sender;
            dynamic drawBrush = new SolidBrush(btn.ForeColor);
            dynamic sf = new StringFormat {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            if (btn.Text.Contains("%"))
                e.Graphics.DrawString(btn.Text, btn.Font, drawBrush, e.ClipRectangle, sf);
            drawBrush.Dispose();
            sf.Dispose();
        }

        private void levelpanel_MouseEnter(object sender, EventArgs e)
        {
            (sender as Panel).BackColor = Color.FromArgb(60, 60, 60);
        }
        private void levelpanel_MouseLeave(object sender, EventArgs e)
        {
            (sender as Panel).BackColor = Color.FromArgb(40, 40, 40);
        }

        private void infoicon_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            Panel panel = (Panel)c.Parent;
            ThumpNetLevel Level = Levels.First(x => x.LevelPanel.Name == panel.Name);
            if (!Level.DescriptionExpanded) {
                Label description = new Label();
                description.Text = Level.Description;
                description.MaximumSize = new Size(256, 1000);
                description.Location = new Point(1, compactView ? 0 : 144);
                description.Name = "description";
                description.Font = new Font("Trebuchet MS", 8, FontStyle.Italic);
                description.ForeColor = Color.White;
                description.AutoSize = true;
                panel.Controls.Add(description);
                panel.Height += description.Height;
                description.Anchor = AnchorStyles.Bottom;
                Level.DescriptionExpanded = true;

                Label author = (Label)panel.Controls.Find("labelAuthor", true)[0];
                author.Text = $"{Level.Author} • {Level.DateUTC.ToShortDateString()}";
            }
            else {
                Control toremove = panel.Controls.Find("description", true)[0];
                panel.Height -= toremove.Height;
                panel.Controls.Remove(toremove);
                Level.DescriptionExpanded = false;

                Label author = (Label)panel.Controls.Find("labelAuthor", true)[0];
                author.Text = $"{Level.Author} • {ThumperModdingTool.DateTime_Ago(Level.DateUTC)}";
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
