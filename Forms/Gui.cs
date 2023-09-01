using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing;

namespace Thumper_Modding_Tool_resharp
{
	public partial class ThumperModdingTool
	{
		private readonly CommonOpenFileDialog cfd_game = new CommonOpenFileDialog() { IsFolderPicker = true, Multiselect = false };
		private bool Thumper_Running()
        {
			//get list of current running processes
			Process[] processlist = Process.GetProcesses();
			//iterate over each to see if Thumper is running at all
			//if it is, we don't want to enable mod mode, as that will break stuff
			foreach (Process _p in processlist) {
				if (_p.ProcessName == "THUMPER_dx9" || _p.ProcessName == "THUMPER_win8" || _p.ProcessName == "THUMPER_win10") {
					MessageBox.Show("Thumper is currently running. Please close it before enabling mods.");
					return true;
				}
			}
			//if Thumper is not found to be running, return
			return false;
		}

		private void Read_Config(bool skip)
		{
			string _S = Properties.Settings.Default.game_dir;
			if (skip && Properties.Settings.Default.game_dir != "none")
				return;

            cfd_game.Title = "Select your Thumper Installation Directory";
            if (Properties.Settings.Default.game_dir == "none")
                cfd_game.InitialDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\Thumper";
            //if it's not empty, initialize the FolderBrowser to be whatever was selected last
            else
                cfd_game.InitialDirectory = Properties.Settings.Default.game_dir;
            //show FolderBrowser, and then set "game_dir" to whatever is chosen
            if (cfd_game.ShowDialog() == CommonFileDialogResult.Ok)
                Properties.Settings.Default.game_dir = cfd_game.FileName;

			Properties.Settings.Default.Save();
		}
	}

	public class MenuColorTable : ProfessionalColorTable
	{
		// Top menu item, before selected
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(12, 0, 0);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(12, 0, 0);
        public override Color MenuItemBorder => Color.FromArgb(100, 0, 0);
		// Top menu item, selected
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(12, 0, 0);
        public override Color MenuItemPressedGradientMiddle => Color.FromArgb(12, 0, 0);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(12, 0, 0);
		// Drop down items
        public override Color MenuItemSelected => Color.FromArgb(12, 0, 0);
        public override Color MenuBorder => Color.FromArgb(100, 0, 0);
        public override Color ToolStripDropDownBackground => Color.FromArgb(40,40,40);
    }
}