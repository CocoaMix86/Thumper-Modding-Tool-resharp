using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Devices.Geolocation;
using System.Windows;

namespace Thumper_Mod_Loader
{
	public partial class ThumperModdingTool
	{
        public static List<LevelRecord> LevelRecords = new();
        
        public static void Backup_SaveData(string game_dir)
		{
			var backup_time = DateTime.Now.ToString().Replace(":","").Replace("/","-");
            if (!Directory.Exists(@"level records"))
                Directory.CreateDirectory(@"level records");

            int index = 0;
            string dataindex = Directory.GetFiles($@"{game_dir}\savedata\", "data.index", SearchOption.AllDirectories).FirstOrDefault();
            if (dataindex != null)
                index = File.ReadAllBytes(dataindex)[8];
            string savefile = Directory.GetFiles($@"{game_dir}\savedata\", $"data_{index}.sav", SearchOption.AllDirectories).FirstOrDefault();

            List<LevelRecord> BackupRecords = new();
            using (BinaryReader br = new(new FileStream(savefile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                br.ReadInt32(); //header
                br.ReadInt32(); //byte count
                br.ReadInt32(); //unix timestamp
                br.ReadInt32(); //0
                int levels = br.ReadInt32();
                for (int x = 0; x < levels; x++) {
                    LevelRecord thislevel = new();

                    int chars = br.ReadInt32();
                    thislevel.Name = new string(br.ReadChars(chars)); //level name

                    chars = br.ReadInt32();
                    thislevel.Rank = new string(br.ReadChars(chars)); //level play rank

                    thislevel.Score = br.ReadInt32(); //level play score
                    chars = br.ReadInt32();
                    br.ReadChars(chars); //the rank again
                    br.ReadBoolean(); //always a 1
                    br.ReadInt32(); //unix timestamp
                    br.ReadInt32(); //always 0

                    thislevel.ScorePlus = br.ReadInt32(); //level play+ score

                    chars = br.ReadInt32();
                    thislevel.RankPlus = new(br.ReadChars(chars)); //level play+ rank

                    chars = br.ReadInt32();
                    br.ReadChars(chars); //the rank again
                    br.ReadInt32(); //-1

                    int sublevels = br.ReadInt32(); //num sublevels in level
                    for (int y = 0; y < sublevels; y++) {
                        chars = br.ReadInt32();
                        br.ReadChars(chars); //sublevel rank
                        br.ReadInt32(); //-1
                    }
                    br.ReadInt32(); //-1

                    int sublevels2 = br.ReadInt32(); //num sublevels in level+
                    for (int y = 0; y < sublevels2; y++) {
                        chars = br.ReadInt32();
                        br.ReadChars(chars); //sublevel rank
                        br.ReadInt32(); //-1
                    }
                    //
                    BackupRecords.Add(thislevel);
                }
            }

            foreach (LevelRecord lr in BackupRecords) {
                if (File.Exists($@"level records\{lr.Name}.record")) {
                    LevelRecord existingrecord = (LevelRecord)JsonConvert.DeserializeObject(File.ReadAllText($@"level records\{lr.Name}.record"));
                    if (existingrecord.Name == null || existingrecord.Rank == null || existingrecord.RankPlus == null || existingrecord.Score == null || existingrecord.ScorePlus == null || existingrecord.IntegrityHash == null) {
                        File.WriteAllText($@"level records\{lr.Name}.record", JsonConvert.SerializeObject(lr, Formatting.Indented));
                        return;
                    }
                    if (LevelRecord.GetHashString($"{existingrecord.Name}{existingrecord.Rank}{existingrecord.RankPlus}{existingrecord.Score}{existingrecord.ScorePlus}{86}{new Random(Properties.Resources.d6.GetPixel(32, 32).R).Next()}") != existingrecord.IntegrityHash) {
                        File.WriteAllText($@"level records\{lr.Name}.record", JsonConvert.SerializeObject(lr, Formatting.Indented));
                        return;
                    }
                    if (existingrecord.Score > lr.Score) {
                        lr.Score = existingrecord.Score;
                        lr.Rank = existingrecord.Rank;
                    }
                    if (existingrecord.ScorePlus > lr.Score) {
                        lr.ScorePlus = existingrecord.ScorePlus;
                        lr.RankPlus = existingrecord.RankPlus;
                    }
                }
                //finally, write the record to file
                File.WriteAllText($@"level records\{lr.Name}.record", JsonConvert.SerializeObject(lr, Formatting.Indented));
            }
        }

        public static void LoadRecords()
        {
            LevelRecords.Clear();
            foreach (string _record in Directory.GetFiles($@"level records\", "*.record", SearchOption.AllDirectories)) {
                LevelRecord existingrecord = (LevelRecord)JsonConvert.DeserializeObject(File.ReadAllText(_record));
                //check if all fields of record are ok or have been tampered
                if (existingrecord.Name == null || existingrecord.Rank == null || existingrecord.RankPlus == null || existingrecord.Score == null || existingrecord.ScorePlus == null || existingrecord.IntegrityHash == null) {
                    existingrecord = ResetRecord(existingrecord);
                    File.WriteAllText(_record, JsonConvert.SerializeObject(existingrecord, Formatting.Indented));
                }
                else if (LevelRecord.GetHashString($"{existingrecord.Name}{existingrecord.Rank}{existingrecord.RankPlus}{existingrecord.Score}{existingrecord.ScorePlus}{86}{new Random(Properties.Resources.d6.GetPixel(32, 32).R).Next()}") != existingrecord.IntegrityHash) {
                    existingrecord = ResetRecord(existingrecord);
                    File.WriteAllText(_record, JsonConvert.SerializeObject(existingrecord, Formatting.Indented));
                }

                LevelRecords.Add(existingrecord);
            }
        }

        public static LevelRecord ResetRecord(LevelRecord record)
        {
            record.Rank = "RANK_C";
            record.RankPlus = "RANK_C";
            record.Score = 0;
            record.ScorePlus = 0;
            return record;
        }

        public static void Create_SaveData()
        {
            string saveloc = Directory.GetFiles($@"{Properties.Settings.Default.game_dir}\savedata", "*.sav", SearchOption.AllDirectories).FirstOrDefault();
            if (saveloc == null) {
                MessageBox.Show("The mod loader was unable to locate your save data. Try launching the game unmodded first and open the leaderboards. The exit the game and try again. If the error persists, make sure you set your Game Directory to the correct path.", "Thumper Mod Loader");
                return;
            }
            saveloc = Path.GetDirectoryName(saveloc);

            List<LevelRecord> RecordsToWrite = new();
            foreach (LevelTraits lt in LoadedLevels) {
                if (LevelRecords.First(x => x.Name == lt.Name) is LevelRecord lr)
                    RecordsToWrite.Add(lr);
                else {
                    LevelRecord _newrecord = new() { Name = lt.Name };
                    File.WriteAllText($@"level records\{lt.Name}.record", JsonConvert.SerializeObject(_newrecord, Formatting.Indented));
                    RecordsToWrite.Add(_newrecord);
                }
            }
        }

        /// via Microsoft
        /// https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new(sourceDirName);

            if (!dir.Exists) {
                return;
                ///throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }
            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        /*
        private void Make_Custom_Savedata(string game_dir)
		{
            foreach (string filename in Directory.EnumerateFiles($@"{game_dir}/savedata", "*.*", SearchOption.AllDirectories)) {
                if (Path.GetFileName(filename) == "data.index") {
                    using (FileStream f = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                        f.Write(Properties.Resources.data, 0, Properties.Resources.data.Length);
                    }
                }
                if (Path.GetFileName(filename) == "data_0.sav") {
                    using (FileStream f = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                        f.Write(Properties.Resources.data_0, 0, Properties.Resources.data_0.Length);
                    }
                }
            }
        }

        private void Restore_Savedata(string game_dir)
        {
            if (Directory.Exists(@"backup")) {
                string last_backup_time = new DirectoryInfo(@"backup").GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First().ToString();
                DirectoryCopy($@"backup/{last_backup_time}", $@"{game_dir}/savedata", true);
            }
        }
        */
    }
}