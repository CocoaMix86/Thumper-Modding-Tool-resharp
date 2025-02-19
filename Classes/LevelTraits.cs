using System;
using System.IO;

namespace Thumper_Mod_Loader
{
	public class LevelTraits
	{
		public string Name { get; set; }
		public string Difficulty { get; set; }
		public string Description { get; set; }
		public FileInfo FilePath { get; set; }
		public string Authors { get; set; }
		public int Sublevels { get; set; }
	}
}
