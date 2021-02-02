using System;
using System.Linq;
using System.Text;
using System.IO;

namespace Thumper_Modding_Tool_resharp
{
	public partial class ThumperModdingTool
	{
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
			byte bytes = val == "1" || val == "True" ? 1 : 0;
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
	}
}