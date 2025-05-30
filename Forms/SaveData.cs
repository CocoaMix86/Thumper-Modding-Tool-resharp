using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows;

namespace Thumper_Mod_Loader
{
	public partial class ThumperModdingTool
	{
        public static List<LevelRecord> LevelRecords = new();

        public static void Backup_SaveData(string game_dir)
		{
            string savefile = "";
            try {
                var backup_time = DateTime.Now.ToString().Replace(":", "").Replace("/", "-");
                if (!Directory.Exists(@"level records"))
                    Directory.CreateDirectory(@"level records");

                int index = 0;
                string dataindex = Directory.GetFiles($@"{game_dir}\savedata\", "data.index", SearchOption.AllDirectories).FirstOrDefault();
                if (dataindex != null)
                    index = File.ReadAllBytes(dataindex)[8];
                savefile = Directory.GetFiles($@"{game_dir}\savedata\", $"data_{index}.sav", SearchOption.AllDirectories).FirstOrDefault();
                if (savefile == null)
                    goto skipbackup;
            }
            catch (Exception) {
                goto skipbackup;
            }

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
                lr.IntegrityHash = lr.GetIntegrityHash();
                if (File.Exists($@"level records\{lr.Name}.record")) {
                    LevelRecord existingrecord = new();
                    try {
                        existingrecord = JsonConvert.DeserializeObject<LevelRecord>(File.ReadAllText($@"level records\{lr.Name}.record"));
                    } catch (Exception) {
                        lr.ResetRecord();
                        File.WriteAllText($@"level records\{lr.Name}.record", JsonConvert.SerializeObject(lr, Formatting.Indented));
                        return;
                    }
                    if (existingrecord.Name == null || existingrecord.Rank == null || existingrecord.RankPlus == null || existingrecord.Score == null || existingrecord.ScorePlus == null || existingrecord.IntegrityHash == null) {
                        lr.ResetRecord();
                        File.WriteAllText($@"level records\{lr.Name}.record", JsonConvert.SerializeObject(lr, Formatting.Indented));
                        return;
                    }
                    if (existingrecord.GetIntegrityHash() != existingrecord.IntegrityHash) {
                        lr.ResetRecord();
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
        skipbackup:;
        }

        public static void LoadRecords()
        {
            if (!Directory.Exists(@"level records"))
                Directory.CreateDirectory(@"level records");
            LevelRecords.Clear();
            foreach (string _record in Directory.GetFiles($@"level records\", "*.record", SearchOption.AllDirectories)) {
                LevelRecord existingrecord = new();
                try {
                    existingrecord = JsonConvert.DeserializeObject<LevelRecord>(File.ReadAllText(_record));
                    //check if all fields of record are ok or have been tampered
                    if (existingrecord.Name == null || existingrecord.Rank == null || existingrecord.RankPlus == null || existingrecord.Score == null || existingrecord.ScorePlus == null || existingrecord.IntegrityHash == null) {
                        existingrecord.Name = Path.GetFileNameWithoutExtension(_record);
                        existingrecord.ResetRecord();
                        File.WriteAllText(_record, JsonConvert.SerializeObject(existingrecord, Formatting.Indented));
                    }
                    else if (existingrecord.GetIntegrityHash() != existingrecord.IntegrityHash) {
                        existingrecord.ResetRecord();
                        File.WriteAllText(_record, JsonConvert.SerializeObject(existingrecord, Formatting.Indented));
                    }
                } catch (Exception) {
                    existingrecord.Name = Path.GetFileNameWithoutExtension(_record);
                    existingrecord.ResetRecord();
                    File.WriteAllText(_record, JsonConvert.SerializeObject(existingrecord, Formatting.Indented));
                }

                LevelRecords.Add(existingrecord);
            }
        }

        public static void Create_SaveData()
        {
            string saveloc = "";
            try {
                saveloc = Directory.GetFiles($@"{Properties.Settings.Default.game_dir}\savedata", "*.sav", SearchOption.AllDirectories).FirstOrDefault();
            }
            catch (Exception) {
                MessageBox.Show("The mod loader was unable to locate your save data. Try launching the game unmodded first and open the leaderboards. Then exit the game and try again. If the error persists, make sure you set your Game Directory to the correct path.", "Thumper Mod Loader");
                return;
            }
            if (saveloc == null) {
                MessageBox.Show("The mod loader was unable to locate your save data. Try launching the game unmodded first and open the leaderboards. Then exit the game and try again. If the error persists, make sure you set your Game Directory to the correct path.", "Thumper Mod Loader");
                return;
            }
            try {
                if (saveloc.EndsWith("data_0.sav")) {
                    File.Delete($@"{Path.GetDirectoryName(saveloc)}\data_1.sav");
                }
                else if (saveloc.EndsWith("data_1.sav")) {
                    File.Delete($@"{Path.GetDirectoryName(saveloc)}\data_0.sav");
                }
            } catch (Exception ex) {
                MessageBox.Show("Either one of your Thumper save files are open/in use and could not be edited. Please make sure they are closed and try again.", "Thumper Mod Loader");
                return;
            }

            List<LevelRecord> RecordsToWrite = new();
            foreach (LevelTraits lt in LoadedLevels) {
                if (LevelRecords.FirstOrDefault(x => x.Name == lt.Name) is LevelRecord lr)
                    RecordsToWrite.Add(lr);
                else {
                    LevelRecord _newrecord = new() { Name = lt.Name };
                    File.WriteAllText($@"level records\{lt.Name}.record", JsonConvert.SerializeObject(_newrecord, Formatting.Indented));
                    RecordsToWrite.Add(_newrecord);
                }
            }
            //need to add level 3
            if (LevelRecords.FirstOrDefault(x => x.Name == "level3") is LevelRecord lr3)
                RecordsToWrite.Add(lr3);
            else {
                LevelRecord level3 = new() { Name = "level3" };
                File.WriteAllText($@"level records\level3.record", JsonConvert.SerializeObject(level3, Formatting.Indented));
                RecordsToWrite.Add(level3);
            }
            //locate the last null terminator and then copy all bytes after it. Important to store user control preferences
            byte[] savefooter = File.ReadAllBytes(saveloc);
            List<int> terminators = Search(savefooter, new byte[] { 0xff, 0xff, 0xff, 0xff });
            int offset = 4; //depending what's at the end of the records, we either need to skip 4 or 8 bytes
            if (savefooter[terminators.Last() + 4] == 0x00)
                offset = 8;
            savefooter = savefooter.AsSpan(terminators.Last() + offset).ToArray();
            //
            using (FileStream f = File.Open(saveloc, FileMode.Create, FileAccess.Write, FileShare.None)) {
                Write_Int(f, 65); //header
                int sumofbytes = RecordsToWrite.Sum(x => x.Name.Length) + (RecordsToWrite.Sum(x => x.Rank.Length + x.RankPlus.Length) * 2) + (5 * 4) + (((13 * 4) + 1) * RecordsToWrite.Count) + savefooter.Length;
                Write_Int(f, sumofbytes); //total bytes of file
                Write_Int(f, (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds); //unix timestamp
                Write_Int(f, 0); //0
                Write_Int(f, RecordsToWrite.Count); //number of levels
                ///Level structure
                foreach (LevelRecord lr in RecordsToWrite) {
                    Write_String(f, lr.Name); //level name
                    Write_String(f, lr.Rank); //level play rank
                    Write_Int(f, lr.Score ?? 0); //level play score
                    Write_String(f, lr.Rank); //level play rank
                    Write_Bool(f, true); //true
                    Write_Int(f, (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds); //unix timestamp
                    Write_Int(f, 0); //0
                    Write_Int(f, lr.ScorePlus ?? 0); //level play+ score
                    Write_String(f, lr.RankPlus); //level play+ rank
                    Write_String(f, lr.RankPlus); //level play+ rank again
                    Write_Int(f, -1); //null terminator
                    Write_Int(f, 0); //number of sublevels in level play [Optional]
                    Write_Int(f, -1); //null terminator
                    Write_Int(f, 0); //number of sublevels in level play+ [Optional]
                }
                ///Save file footer
                f.Write(savefooter, 0, savefooter.Length);
            }

            if (saveloc.EndsWith("data_0.sav")) {
                File.Copy(saveloc, $@"{Path.GetDirectoryName(saveloc)}\data_1.sav");
            }
            else if (saveloc.EndsWith("data_1.sav")) {
                File.Copy(saveloc, $@"{Path.GetDirectoryName(saveloc)}\data_0.sav");
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