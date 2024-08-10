using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Mod_Loader
{
    public class ObjLib
    {
        int type;
        int unk0, unk1, unk2, unk3;
        List<string> ObjectLibraries;
        string Filename;
        public ObjLib(byte[] bytes)
        {
            int ftype = BitConverter.ToInt32(bytes, 0);
            if (ftype != 8) throw new Exception("ObjLib must have filetype 8");
            type = BitConverter.ToInt32(bytes, 4);
            unk0 = BitConverter.ToInt32(bytes, 8);
            unk1 = BitConverter.ToInt32(bytes, 12);
            unk2 = BitConverter.ToInt32(bytes, 16);
            unk3 = BitConverter.ToInt32(bytes, 20);

            ObjectLibraries = new List<string>();
            int num_objects = BitConverter.ToInt32(bytes, 24);
            int pos = 28;
            for (int i = 0; i <= num_objects; i++)
            {
                int len = BitConverter.ToInt32(bytes, pos);
                pos += 4;
                if (len == 0)
                {
                    len = BitConverter.ToInt32(bytes, pos);
                    pos += 4;
                }
                string str = BitConverter.ToString(bytes, pos, len);
                if (i == num_objects) Filename = str;
                else ObjectLibraries.Add(str);
                pos += len;
            }
        }
        public byte[] ToBytes()
        {
            List<byte> bytes = new();

            bytes.AddRange(BitConverter.GetBytes(8));
            bytes.AddRange(BitConverter.GetBytes(type));
            bytes.AddRange(BitConverter.GetBytes(unk0));
            bytes.AddRange(BitConverter.GetBytes(unk1));
            bytes.AddRange(BitConverter.GetBytes(unk2));
            bytes.AddRange(BitConverter.GetBytes(unk3));

            bytes.AddRange(BitConverter.GetBytes(ObjectLibraries.Count));
            foreach (string obj in ObjectLibraries)
            {
                bytes.AddRange(BitConverter.GetBytes(0));
                bytes.AddRange(BitConverter.GetBytes(obj.Length));
                bytes.AddRange(Encoding.ASCII.GetBytes(obj));
            }
            bytes.AddRange(BitConverter.GetBytes(Filename.Length));
            bytes.AddRange(Encoding.ASCII.GetBytes(Filename));

            return bytes.ToArray();
        }
    }

    public enum ObjLibType : uint
    {
        GFX = 0x4314a51b,
        SEQ = 0x484595b0,
        OBJ = 0x19621c9d,
        LVL = 0x9e4d370b,
        AVA = 0x4f6274e6
    }
}
