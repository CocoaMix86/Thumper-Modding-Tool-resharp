using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Thumper_Mod_Loader
{
	public partial class ThumperModdingTool
	{
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private uint Hash32(string s)
		{
			//this hashes stuff. Don't know why it does it this why.
			//this is ripped directly from the game's code
			uint h = 0x811c9dc5;
			foreach (char c in s)
				h = ((h ^ c) * 0x1000193) & 0xffffffff;
			h = (h * 0x2001) & 0xffffffff;
			h = (h ^ (h >> 0x7)) & 0xffffffff;
			h = (h * 0x9) & 0xffffffff;
			h = (h ^ (h >> 0x11)) & 0xffffffff;
			h = (h * 0x21) & 0xffffffff;

			return h;
		}

		private void Write_Int(FileStream f, int val)
		{
			//convert int to bytes and append to file
			byte[] bytes = BitConverter.GetBytes((int)val);
			f.Write(bytes, 0, bytes.Length);
		}

		private void Write_Bool(FileStream f, string val)
		{
			byte bytes = val == "1" || val == "True" ? (byte)1 : (byte)0;
			f.WriteByte(bytes);
		}

		private void Write_Float(FileStream f, float val)
		{
			byte[] bytes = BitConverter.GetBytes((float)val);
			f.Write(bytes, 0, bytes.Length);
		}

		private void Write_Color(FileStream f, dynamic val)
		{
			Write_Float(f, (float)val[0]);
			Write_Float(f, (float)val[1]);
			Write_Float(f, (float)val[2]);
			Write_Float(f, (float)val[3]);
		}

		private void Write_Vec3(FileStream f, dynamic val)
		{
			Write_Float(f, (float)val[0]);
			Write_Float(f, (float)val[1]);
			Write_Float(f, (float)val[2]);
		}

		private void Write_String(FileStream f, string val)
		{
			//In the .pc file, strings are preceeded by their length
			Write_Int(f, val.Length);
			f.Write(Encoding.ASCII.GetBytes(val), 0, val.Length);
		}

		private void Write_Hash(FileStream f, string val)
		{
			//pass the string to the hash function first before writing to file
			byte[] bytes = BitConverter.GetBytes((uint)Hash32(val));
			f.Write(bytes, 0, bytes.Length);
		}

		private void Write_Hex(FileStream f, string val)
		{
			byte[] bytes = StringToByteArray(val);
			f.Write(bytes, 0, bytes.Length);
		}

		private void Write_Hex_Reverse(FileStream f, string val)
		{
			byte[] bytes = StringToByteArray(val);
			bytes = bytes.Reverse().ToArray();
			f.Write(bytes, 0, bytes.Length);
		}

		public static string DateTime_Ago(DateTime dt)
		{
			// lazy copy paste from stackoverflow
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1 year ago" : years + " years ago";
            }
        }

		public dynamic LoadFileLock(string _selectedfilename)
		{
			dynamic _load;
			///reference:
			///https://stackoverflow.com/questions/1389155/easiest-way-to-read-text-file-which-is-locked-by-another-application
			using (var fileStream = new FileStream(_selectedfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var textReader = new StreamReader(fileStream)) {
				_load = JsonConvert.DeserializeObject(Regex.Replace(textReader.ReadToEnd(), "#.*", ""));
			}

			return _load;
		}
	}
}