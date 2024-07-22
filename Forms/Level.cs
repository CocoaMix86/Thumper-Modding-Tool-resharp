using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace Thumper_Modding_Tool_resharp
{
	public partial class ThumperModdingTool
	{
		public JsonLoadSettings jo = new() { CommentHandling = CommentHandling.Load};

		List<string> file_types = new() { "gate", "leaf", "lvl", "master", "xfm", "config" };
		List<string> file_special = new() { "spn", "samp" };

		List<string> list_cache_filename = new() { 
			"23490781.pc", 
			"8b497b8b.pc", 
			"feac0d77.pc", 
			"abe9847b.pc", 
			"ca760d3f.pc", 
			"10700608.pc", 
			"4c3c80cf.pc", 
			"7394d1b7.pc" };

		List<string> list_config_cache_filename = new() { 
			"f296f3ab.pc", 
			"c2b80908.pc", 
			"a9045489.pc", 
			"7d5398a1.pc", 
			"4f9698fc.pc", 
			"14f2baa5.pc", 
			"7b0b72e8.pc", 
			"f9ac5eb5.pc" };

		List<string> trait_types = new() {
			"kTraitInt",
			"kTraitBool",
			"kTraitFloat",
			"kTraitColor",
			"kTraitObj",
			"kTraitVec3",
			"kTraitPath",
			"kTraitEnum",
			"kTraitAction",
			"kTraitObjVec",
			"kTraitString",
			"kTraitCue",
			"kTraitEvent",
			"kTraitSym",
			"kTraitList",
			"kTraitTraitPath",
			"kTraitQuat",
			"kTraitChildLib",
			"kTraitComponent",
			"kNumTraitTypes"
		};

		List<string> obj_types = new() {
			"SequinLeaf",
			"SequinLevel",
			"SequinGate",
			"SequinMaster",
			"EntitySpawner",
			"Sample",
			"Xfmer"
		};

		///
		/// Restores level files to original state
		/// 
		private void Restore_Levels(string game_dir)
		{
			// in order of appearance (0 through 6 in the array)
			// level text
			// level 4 (nulls it out? why?)
			// level list file
			// title screen
			// drool logo () [these are DDS images, just remove first 4 bytes to view]
			// drool logo (startup)
			List<string> src_filenames = new() { "lib/original/2e7b0500.pc", "lib/original/e0c51024.pc", "lib/original/f78b7d78.pc", "lib/original/d0d6149c.pc", "lib/original/aefa4352.pc", "lib/original/b868db07.pc" };
			List<string> custom_filenames = new();

			foreach (string file in Directory.EnumerateFiles(@"out", "*.*", SearchOption.AllDirectories))
				custom_filenames.Add(file);
			//copy original files back to cache
			foreach (string src_filename in src_filenames)
				File.Copy(src_filename, $@"{game_dir}/cache/{Path.GetFileName(src_filename)}", true);
			//delete modified level .pc files. The game will regenerate them
			foreach (string custom_filename in custom_filenames) {
				var dst_filename = Path.GetFileName(custom_filename);
				if (File.Exists($@"{game_dir}/cache/{dst_filename}"))
					File.Delete($@"{game_dir}/cache/{dst_filename}");
			}
			//delete extra audio files added previously
			if (Properties.Settings.Default.loaded_files is null)
				return;

			foreach (string audio_filename in Properties.Settings.Default.loaded_files) {
				var dst_filename = Path.GetFileName(audio_filename);
				if (File.Exists($@"{game_dir}/cache/{dst_filename}"))
					File.Delete($@"{game_dir}/cache/{dst_filename}");
			}
		}

		///
		/// The primary code block that turns the custom level data into a format the game can read
		/// 
		private void Make_Custom_Levels(string game_dir)
		{
			//string[] out_files = Directory.GetFiles("C:\\Users\\booge\\Documents\\GitHub\\Thumper-Modding-Tool-resharp\\bin\\Debug\\full_out\\cache");
			//List<string> gfiles = Directory.GetFiles(Path.Combine(game_dir, "cache")).ToList();
			//List<string> gf = new List<string>();
			//foreach (var f in gfiles)
			//{
			//	gf.Add(Path.GetFileName(f));
			//}

   //         foreach (var f in out_files)
			//{
			//	if (gf.Contains(Path.GetFileName(f)))
			//	{
   //                 File.Copy(f, $"{new FileInfo(f).Directory.FullName}\\important\\{Path.GetFileName(f)}");
   //             }
			//}

			//MessageBox.Show("Finished");
			//return;

            int menulength = 2548;
			List<string> src_filenames = new() { "lib/2e7b0500.pc", "lib/e0c51024.pc", "lib/f78b7d78.pc", "lib/d0d6149c.pc", "lib/aefa4352.pc", "lib/b868db07.pc",
				"lib/ae685f16.pc" };
			//these hashes are literally "customlevel#" hashed
				List<string> menu_hashes = new() { "1DCB06CE", "2D5C3C41", "273EA275", "EBA1CBD7", "1F8AD438", "DDF57F91", "9402A958", "FB3C6A42", "85E4559B" };
			List<string> menu_names = new();
			//clear \out\ directory so that old level data is not stored anymore
			DirectoryInfo _out = new(@"out");
			foreach (string file in Directory.EnumerateFiles(@"out", "*.*", SearchOption.AllDirectories)) File.Delete(file);
			foreach (DirectoryInfo dir in _out.GetDirectories()) dir.Delete();

			//loop over each selected level
			foreach (LevelTraits level_name in LoadedLevels) {
				dynamic level_config = null;
				var objs = new List<dynamic>();
				var obj_count = 0;
				dynamic new_objs = null;
				string errorlist = "";

				if (!Directory.Exists(level_name.path))
				{
					MessageBox.Show($"The level \"{level_name.name}\" no longer exists, please add it again and update.");
					return;
				}
				//check if the custom level folder contains custom audio. This needs to be put into Thumper's cache folder
				if (Directory.Exists(@$"{level_name.path}\extras")) {
                    Properties.Settings.Default.loaded_files = new List<string>();
                    foreach (string filename in Directory.GetFiles(@$"{level_name.path}\extras")) {
						File.Copy(filename, $@"{game_dir}\cache\{Path.GetFileName(filename)}", true);
						//audiofiles.Add(filename);
						Properties.Settings.Default.loaded_files.Add(filename);
					}

                    Properties.Settings.Default.Save();
				}
				//iterate over each file in the custom level directory
				//filter out files that do not match the <file_types> list
				foreach (string filename in Directory.GetFiles(level_name.path)) {
					if (file_types.Contains(Path.GetFileName(filename).Split('_')[0])) {
						//read file and store JSON in dynamic object
						try {
							//new_objs = JObject.Parse(Regex.Replace(File.ReadAllText(filename), @"#.*", ""), jo);
							new_objs = LoadFileLock(filename);
						}
						catch (Exception ex) { 
							errorlist += $"error parsing:\n{ex.Message} in file \"{Path.GetFileName(filename)}\" in level \"{level_name.name}\"\n\n";
							continue;
						}
						//LevelLib contains important info for where the final level files go, so it gets put into its own object
						if (new_objs.obj_type == "LevelLib") {
							level_config = new_objs;
							obj_count--;
						}
						objs.Add(new_objs);
						obj_count++;
					}
					//these file types require different processing to get the data
					else if (file_special.Contains(Path.GetFileName(filename).Split('_')[0])) {
						try {
							//new_objs = JsonConvert.DeserializeObject(File.ReadAllText(filename));
							new_objs = LoadFileLock(filename);
						}
						catch (Exception ex) {
							errorlist += $"error parsing:\n{ex.Message} in file \"{Path.GetFileName(filename)}\" in level \"{level_name.name}\"\n\n";
							continue;
						}
						//spn_ and samp_ files contain multiple entries, inside the "multi":[] list
						foreach (var _v in new_objs.items) {
							objs.Add(_v);
							obj_count++;
						}
					}
				}
				//if errors exist, show the error, then return to stop further processing
				if (errorlist.Contains("error parsing")) {
					MessageBox.Show(errorlist + "CUSTOM LEVELS NOT UPDATED", "Level load error");
					return;
				}

				//var cache_filename = $@"out\{level_name.folder_name}\{level_config["cache_filename"]}";
				var cache_filename = $@"out\{level_name.folder_name}\{list_cache_filename[LoadedLevels.IndexOf(level_name)]}";
				Directory.CreateDirectory($@"out\{level_name.folder_name}");
				byte[] bytes;

				using (FileStream f = File.Open(cache_filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
					//write header information to level .pc file
					bytes = File.ReadAllBytes(@"lib/header.objlib");
					f.Write(bytes, 0, bytes.Length);

					//write objlib path to level .pc file
					//Write_String(f, (string)level_config["objlib_path"]);
					Write_String(f, $"levels/custom/level{LoadedLevels.IndexOf(level_name)+1}.objlib");

					//write "basic list of objects #1" information to level .pc file
					bytes = File.ReadAllBytes(@"lib/obj_list_1.objlib");
					f.Write(bytes, 0, bytes.Length);
					//write to file the amount of objects that exists (after this number)
					//this includes everything in "obj_list_2.objlib" (63) and obj_count
					Write_Int(f, 63 + obj_count);
					//write "basic list of objects #2" information to level .pc file
					bytes = File.ReadAllBytes(@"lib/obj_list_2.objlib");
					f.Write(bytes, 0, bytes.Length);
					//write every object to the .pc file, hashing its name
					foreach (var obj in objs) {
						if (obj_types.Contains((string)obj["obj_type"])) {
							if ((string)obj["obj_type"] != "Xfmer") {
								Write_Hash(f, (string)obj["obj_type"]);
								Write_String(f, (string)obj["obj_name"]);
							}
							else {
								Write_Hash(f, (string)obj["obj_type"]);
								Write_String(f, $"levels/custom/level{LoadedLevels.IndexOf(level_name) + 1}.xfm");
							}
						}
					}

					//bytes = File.ReadAllBytes($@"lib/obj_def_customlevel{LoadedLevels.IndexOf(level_name) + 1}.objlib");
					bytes = File.ReadAllBytes($@"lib/obj_def_customlevel.objlib");
					f.Write(bytes, 0, bytes.Length);
					//iterate over every loaded object, and write its data to .pc file in specific formats.
					//format is different per object. I myself am not exactly sure how it works, but this is how it's done
					foreach (var obj in objs) {
						if (obj["obj_type"] == "SequinLeaf")
							Write_Leaf(f, obj);
						else if (obj["obj_type"] == "SequinLevel") {
							Write_Lvl_Header(f);
							Write_Approach_Anim_Comp(f, obj);
							Write_Lvl_Comp(f, obj);
							Write_Lvl_Footer(f, obj);
						}
						else if (obj["obj_type"] == "SequinGate")
							Write_Gate(f, obj);
						else if (obj["obj_type"] == "SequinMaster")
							Write_Master(f, obj);
						else if (obj["obj_type"] == "EntitySpawner")
							Write_Spn(f, obj);
						else if (obj["obj_type"] == "Sample")
							Write_Samp(f, obj);
						else if (obj["obj_type"] == "Xfmer") {
							Write_Xfm_Header(f);
							Write_Xfm_Comp(f, obj);
						}
					}

					//close out file with footer info #1
					bytes = File.ReadAllBytes(@"lib/footer_1.objlib");
					f.Write(bytes, 0, bytes.Length);
					//write file bpm
					Write_Float(f, (float)level_config["bpm"]);
					//close out file with footer info #2
					bytes = File.ReadAllBytes(@"lib/footer_2.objlib");
					f.Write(bytes, 0, bytes.Length);

					src_filenames.Add(cache_filename);
				}


				//var config_cache_filename = $@"out\{level_name.folder_name}\{level_config["config_cache_filename"]}";
				var config_cache_filename = $@"out\{level_name.folder_name}\{list_config_cache_filename[LoadedLevels.IndexOf(level_name)]}";
				using (FileStream f = File.Open(config_cache_filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
					Write_Int(f, 9);
					Write_Int(f, level_config["level_sections"].Count);
					foreach (var level_section in level_config["level_sections"]) {
						Write_String(f, (string)level_section);
					}
					Write_Color(f, level_config["rails_color"]);
					Write_Color(f, level_config["rails_glow_color"]);
					Write_Color(f, level_config["path_color"]);
					Write_Color(f, level_config["joy_color"]);

					src_filenames.Add(config_cache_filename);
				}
				menu_names.Add(level_name.name);
				menulength += level_name.name.Length + 1;
			}

			//Update menu names
			using (FileStream f = File.Open(@"lib\2e7b0500.pc", FileMode.Create, FileAccess.Write, FileShare.None)) {
				byte[] bytes;
				int pos = 2548;
				//write menu headers/pointers
				Write_Int(f, 6);
				Write_Int(f, 183);
				Write_Int(f, menulength + ((9 - menu_names.Count) * ("no level".Length + 1)));
				//write all menu strings. Don't edit this
				bytes = File.ReadAllBytes(@"lib/menu1.objlib");
				f.Write(bytes, 0, bytes.Length);
				//user set level names
				for (int x = 0; x < 9; x++) {
					if (x < menu_names.Count)
						f.Write(Encoding.ASCII.GetBytes(menu_names[x]), 0, menu_names[x].Length);
					else
						f.Write(Encoding.ASCII.GetBytes($@"no level"), 0, "no level".Length);
					f.Write(new byte[] { 0x00 }, 0, 1);
				}
				//write menu hashes and positions. Don't edit this
				bytes = File.ReadAllBytes(@"lib/menu2.objlib");
				f.Write(bytes, 0, bytes.Length);

				//write custom hashes and string position in file.
				for (int x = 0; x < 9; x++) {
					Write_Hex(f, menu_hashes[x]);
					Write_Int(f, pos);
					if (x < menu_names.Count)
						pos += menu_names[x].Length + 1;
					else
						pos += "no level".Length + 1;
				}
			}
			
			//Update this file to adjust how the menu looks
			using (FileStream f = File.Open(@"lib\f78b7d78.pc", FileMode.Create, FileAccess.Write, FileShare.None)) {
				Write_Int(f, 16);
				Write_Int(f, menu_names.Count + 1);
				//write blocks of each level
				for (int x = 0; x < menu_names.Count; x++) {
					Write_String(f, $@"customlevel{x + 1}");
					Write_Int(f, 0);
					Write_String(f, $@"levels/custom/level{x + 1}.objlib");
					if (x == menu_names.Count - 1)
						Write_String(f, "level3");
					else
						Write_String(f, $@"customlevel{x + 2}");
					Write_Bool(f, "True");
					Write_Bool(f, "True");
					Write_Bool(f, "False");
					Write_Int(f, x);
					Write_Int(f, x + 10);
				}
				//write level 3 data. This is required to make the menu NOT crash
				Write_String(f, "level3");
				Write_Int(f, 0);
				Write_String(f, "levels/level3/level_3a.objlib");
				Write_Int(f, 0);
				Write_Bool(f, "True");
				Write_Bool(f, "True");
				Write_Bool(f, "True");
				Write_Int(f, menu_names.Count);
				Write_Int(f, menu_names.Count + 10);
			}

			//copy new .pc files to game directory
			foreach (string src_filename in src_filenames) {
				File.Copy(src_filename, $@"{game_dir}\cache\{Path.GetFileName(src_filename)}", true);
			}
		}


		/// 
		/// Methods below are used by the main block to manipulate the level data into the correct forms
		/// And to write data to files
		///
		private void Write_Param_Path(FileStream f, string param_path, string param_path_hash)
		{
			Write_Int(f, 1);
			string param;
			string param_name;
			string param_idx;

			if (!string.IsNullOrEmpty(param_path))
				param = param_path;
			else
				param = param_path_hash;
			//a few specific param paths have a ',' followed by a number. In these special cases, split on ','
			//[0] is the param_name and [1] is the value
			if (param.Contains(",")) {
				var _p = param.Split(',');
				param_name = _p[0];
				param_idx = _p[1];
			}
			//if the param_path does not have a ',', idx is -1
			else {
				param_name = param;
				param_idx = "-1";
			}
			//depending if the string is plain text or hex-hash, write it to .pc file differently
			if (!string.IsNullOrEmpty(param_path))
				Write_Hash(f, param_name);
			else
				Write_Hex_Reverse(f, param_name);

			Write_Int(f, int.Parse(param_idx));
		}

		private void Write_Data_Point_Value (FileStream f, string val, string trait_type)
		{
			if (trait_type == "kTraitInt")
				Write_Int(f, int.Parse(val));
			else if (trait_type == "kTraitBool" || trait_type == "kTraitAction")
				Write_Bool(f, val);
			else if (trait_type == "kTraitFloat")
				Write_Float(f, float.Parse(val));
			else if (trait_type == "kTraitColor") {
				Color _c = Color.FromArgb(int.Parse(val));
				Write_Float(f, _c.R != 0 ? _c.R / 255f : 0);
				Write_Float(f, _c.G != 0 ? _c.G / 255f : 0);
				Write_Float(f, _c.B != 0 ? _c.B / 255f : 0);
				Write_Float(f, _c.A != 0 ? _c.A / 255f : 0);
				//Write_Float(f, 4278190335);
			}
		}

		private void Write_Sequencer_Objects(FileStream f, dynamic obj)
		{
			int beat_cnt = obj["beat_cnt"] ?? 0;
			dynamic seq_objs = obj["seq_objs"];
			//write amount of seq_objs (different tracks) to .pc file
			Write_Int(f, seq_objs.Count);

			foreach (var _obj in seq_objs) {
				///header of object
				Write_String(f, (string)_obj["obj_name"]);
				Write_Param_Path(f, (string)_obj["param_path"], (string)_obj["param_path_hash"]);
				Write_Int(f, trait_types.IndexOf((string)_obj["trait_type"]));

				///data points of object
				//
				var interp = _obj.ContainsKey("default_interp") ? (string)_obj["default_interp"] : "kTraitInterpLinear";
				if (interp == null)
					interp = "kTraitInterpLinear";
				var ease = _obj.ContainsKey("default_ease") ? (string)_obj["default_ease"] : "kEaseInOut";
				//Data points written different depending on STEP
				if (_obj["step"] == "True") {
					//STEP true = value updates every beat, and if no value is set for a beat, it'll use _obj.default
					Write_Int(f, beat_cnt);
					for (int i = 0; i < beat_cnt; i++) {
						Write_Float(f, i);
						//check if data_points contains an entry for beat `i`. If yes, write it
						if (_obj["data_points"].ContainsKey(i.ToString()))
							Write_Data_Point_Value(f, (string)_obj["data_points"][i.ToString()], (string)_obj["trait_type"]);
						else
							Write_Data_Point_Value(f, (string)_obj["default"], (string)_obj["trait_type"]);
						//write these after every beat for some reason
						Write_String(f, interp);
						Write_String(f, ease);
					}
				}
				else {
					//STEP false = value interpolates between values set on beats. Default is ignored.
					Write_Int(f, Enumerable.Count<dynamic>(_obj["data_points"]));
					int _iii = 0;
					foreach (var _v in _obj["data_points"]) {
						JProperty _p = _v;
						Write_Float(f, float.Parse(_p.Name)); _iii++;
						Write_Data_Point_Value(f, (string)_v, (string)_obj["trait_type"]);
						Write_String(f, interp);
						Write_String(f, ease);
					}
				}
				Write_Int(f, 0);

				///footer of object
				JArray _footer = _obj["footer"].GetType() == typeof(JArray) ? _obj["footer"] : JArray.FromObject(((string)_obj["footer"]).Replace("[", "").Replace("]", "").Replace("'", "").Split(','));
				Write_Int(f, (int)_footer[0]);
				Write_Int(f, (int)_footer[1]);
				Write_Int(f, (int)_footer[2]);
				Write_Int(f, (int)_footer[3]);
				Write_Int(f, (int)_footer[4]);
				Write_String(f, (string)_footer[5]);
				Write_String(f, (string)_footer[6]);
				Write_Bool(f, (string)_footer[7]);
				Write_Bool(f, (string)_footer[8]);
				Write_Int(f, (int)_footer[9]);
				Write_Float(f, (float)_footer[10]);
				Write_Float(f, (float)_footer[11]);
				Write_Float(f, (float)_footer[12]);
				Write_Float(f, (float)_footer[13]);
				Write_Float(f, (float)_footer[14]);
				Write_Bool(f, (string)_footer[15]);
				Write_Bool(f, (string)_footer[16]);
				Write_Bool(f, (string)_footer[17]);
			}
		}

		private void Write_Anim_Comp(FileStream f)
		{
			Write_Hash(f, "AnimComp");
			Write_Int(f, 1);
			Write_Float(f, 0);
			Write_String(f, "kTimeBeats");
		}

		private void Write_Approach_Anim_Comp(FileStream f, dynamic obj)
		{
			Write_Hash(f, "ApproachAnimComp");
			Write_Int(f, 1);
			Write_Float(f, 0);
			Write_String(f, "kTimeBeats");
			Write_Int(f, 0);
			Write_Int(f, (int)obj["approach_beats"]);
		}

		private void Write_Xfm_Header(FileStream f)
		{
			Write_Int(f, 4);
			Write_Int(f, 4);
			Write_Int(f, 1);
		}

		private void Write_Xfm_Comp(FileStream f, dynamic obj)
		{
			Write_Hash(f, "XfmComp");
			Write_Int(f, 1);
			Write_String(f, (string)obj["xfm_name"]);
			Write_String(f, (string)obj["constraint"]);
			Write_Vec3(f, obj["pos"]);
			Write_Vec3(f, obj["rot_x"]);
			Write_Vec3(f, obj["rot_y"]);
			Write_Vec3(f, obj["rot_z"]);
			Write_Vec3(f, obj["scale"]);
		}

		private void Write_Leaf(FileStream f, dynamic obj)
		{
			///header
			//honestly don't know what these values do
			Write_Int(f, 34);
			Write_Int(f, 33);
			Write_Int(f, 4);
			Write_Int(f, 2);

			///Anim_Comp
			Write_Anim_Comp(f);

			///comp
			Write_Hash(f, "EditStateComp");
			Write_Sequencer_Objects(f, obj);

			///footer
			int beat_cnt = obj["beat_cnt"];
			Write_Int(f, 0);
			Write_Int(f, beat_cnt);
			for (int i = 0; i < beat_cnt * 3; i++)
				Write_Int(f, 0);
			Write_Int(f, 0);
			Write_Int(f, 0);
			Write_Int(f, 0);
		}

		private void Write_Lvl_Header(FileStream f)
		{
			//honestly don't know what these values do
			Write_Int(f, 51);
			Write_Int(f, 33);
			Write_Int(f, 4);
			Write_Int(f, 2);
		}

		private void Write_Lvl_Comp(FileStream f, dynamic obj)
		{
			Write_Hash(f, "EditStateComp");
			Write_Sequencer_Objects(f, obj);

			//.leaf sequence
			Write_Int(f, 0);
			Write_String(f, "kMovePhaseRepeatChild");
			Write_Int(f, 0);
			int last_beat_cnt = 0;
			//iterate over each leaf in the lvl file and write data to file
			foreach (var leaf in obj["leaf_seq"]) {
				Write_Bool(f, "True");
				Write_Int(f, 0);
				Write_Int(f, (int)leaf["beat_cnt"]);
				Write_Bool(f, "False");
				Write_String(f, (string)leaf["leaf_name"]);
				Write_String(f, (string)leaf["main_path"]);
				Write_Int(f, leaf["sub_paths"].Count);
				foreach (var sub_path in leaf["sub_paths"]) {
					Write_String(f, (string)sub_path);
					Write_Int(f, 0);
				}
				Write_String(f, "kStepGameplay");
				Write_Int(f, last_beat_cnt);
				Write_Vec3(f, leaf["pos"]);
				Write_Vec3(f, leaf["rot_x"]);
				Write_Vec3(f, leaf["rot_y"]);
				Write_Vec3(f, leaf["rot_z"]);
				Write_Vec3(f, leaf["scale"]);
				Write_Hex(f, "0000");
				last_beat_cnt = (int)leaf["beat_cnt"];
			}

			Write_Bool(f, "False");
			//write loops
			Write_Int(f, (int)obj["loops"].Count);
			foreach (var loop in obj["loops"]) {
				Write_String(f, (string)loop["samp_name"]);
				Write_Int(f, (int)loop["beats_per_loop"]);
				Write_Int(f, 0);
			}
		}

		private void Write_Lvl_Footer(FileStream f, dynamic obj)
		{
			Write_Bool(f, "False");
			Write_Float(f, (float)obj["volume"]);
			Write_Int(f, 0);
			Write_Int(f, 0);
			Write_String(f, "kNumTraitTypes");
			Write_Bool(f,(string)obj["input_allowed"]);
			Write_String(f, (string)obj["tutorial_type"]);
			Write_Vec3(f, obj["start_angle_fracs"]);
		}

		private void Write_Gate(FileStream f, dynamic obj)
		{
			///header
			Write_Int(f, 26);
			Write_Int(f, 4);
			Write_Int(f, 1);

			///comp
			Write_Hash(f, "EditStateComp");
			Write_String(f, (string)obj["spn_name"]);
			Write_Param_Path(f, (string)obj["param_path"], (string)obj["param_path_hash"]);

			Write_Int(f, obj["boss_patterns"].Count);
			foreach (var boss_pattern in obj["boss_patterns"]) {
				if (boss_pattern.ContainsKey("node_name"))
					Write_Hash(f, (string)boss_pattern["node_name"]);
				else
					Write_Hex_Reverse(f, (string)boss_pattern["node_name_hash"]);
				Write_String(f, (string)boss_pattern["lvl_name"]);
				Write_Bool(f, "True");
				Write_String(f, (string)boss_pattern["sentry_type"]);
				Write_Hex(f, "00000000");
				Write_Int(f, (int)boss_pattern["bucket_num"]);
			}

			///footer
			Write_String(f, (string)obj["pre_lvl_name"]);
			Write_String(f, (string)obj["post_lvl_name"]);
			Write_String(f, (string)obj["restart_lvl_name"]);
			Write_Int(f, 0);
			Write_String(f, (string)obj["section_type"]);
			Write_Float(f, 9);
			Write_String(f, (string)obj["random_type"]);
		}

		private void Write_Master(FileStream f, dynamic obj)
		{
			///header
			Write_Int(f, 33);
			Write_Int(f, 33);
			Write_Int(f, 4);
			Write_Int(f, 2);

			///Anim_Comp
			Write_Anim_Comp(f);

			///comp
			Write_Hash(f, "EditStateComp");
			Write_Int(f, 0);
			Write_Float(f, (float)300);
			Write_String(f, (string)obj["skybox_name"]);
			Write_String(f, (string)obj["intro_lvl_name"]);

			//lvl/.gate groupings
			int isolated = 0;
			foreach (var grouping in obj["groupings"])
			{
				if (grouping["isolate"] == obj["isolate_tracks"])
					isolated++;
			}
			//Write_Int(f, obj["groupings"].Count);
			Write_Int(f, isolated);
			foreach (var grouping in obj["groupings"]) {
				//If track isolation is enabled, only add the isolated tracks to the level.
				//If it's off, isolate_tracks will be False, and so will all instances grouping["isolate"]
				if (grouping["isolate"] == obj["isolate_tracks"])
				{
					Write_String(f, (string)grouping["lvl_name"]);
					Write_String(f, (string)grouping["gate_name"]);
					Write_Bool(f, (string)grouping["checkpoint"]);
					Write_String(f, (string)grouping["checkpoint_leader_lvl_name"]);
					Write_String(f, (string)grouping["rest_lvl_name"]);
					Write_Hex(f, "01000100000001");
					Write_Bool(f, (string)grouping["play_plus"]);
				}
			}

			///footer
			Write_Bool(f, "False");
			Write_Bool(f, "True");
			Write_Int(f, 3);
			Write_Int(f, 50);
			Write_Int(f, 8);
			Write_Int(f, 1);
			Write_Float(f, 0.6F);
			Write_Float(f, 0.5F);
			Write_Float(f, 0.5F);
			Write_String(f, (string)obj["checkpoint_lvl_name"]);
			Write_String(f, "path.gameplay");
		}

		private void Write_Spn(FileStream f, dynamic obj)
		{
			///header
			Write_Int(f, 1);
			Write_Int(f, 4);
			Write_Int(f, 2);

			///comp
			Write_Hash(f, "EditStateComp");

			///xfm comp
			Write_Xfm_Comp(f, obj);

			///footer
			Write_Int(f, 0);
			Write_String(f, (string)obj["objlib_path"]);
			Write_String(f, (string)obj["bucket"]);
		}

		private void Write_Samp(FileStream f, dynamic obj)
		{
			///header
			Write_Int(f, 12);
			Write_Int(f, 4);
			Write_Int(f, 1);

			///comp
			Write_Hash(f, "EditStateComp");
			Write_String(f, (string)obj["mode"]);
			Write_Int(f, 0);
			Write_String(f, (string)obj["path"]);
			Write_Hex(f, "0000000000");
			Write_Float(f, (float)obj["volume"]);
			Write_Float(f, (float)obj["pitch"]);
			Write_Float(f, (float)obj["pan"]);
			Write_Float(f, (float)obj["offset"]);
			Write_String(f, (string)obj["channel_group"]);
		}
	}
}