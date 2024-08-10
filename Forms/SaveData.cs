using System;
using System.Data;
using System.Linq;
using System.IO;

namespace Thumper_Mod_Loader
{
	public partial class ThumperModdingTool
	{
		int max_backup_count = 20;

        public void Backup_SaveData(string game_dir)
		{
			var backup_time = DateTime.Now.ToString().Replace(":","").Replace("/","-");
			Directory.CreateDirectory($@"backup\\{backup_time}");
			DirectoryCopy($"{game_dir}\\savedata", $"backup\\{backup_time}", true);

            var backups = new DirectoryInfo(@"backup").GetDirectories();
            if (backups.Count() > max_backup_count) {
                Directory.Delete($@"backup/{backups.OrderByDescending(d => d.LastWriteTimeUtc).Last()}/", true);
			}
        }

        /// via Microsoft
        /// https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new(sourceDirName);

            if (!dir.Exists) 
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: "+ sourceDirName);

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
    }
}