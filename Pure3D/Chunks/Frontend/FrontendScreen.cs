using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x18001)]
    public class FrontendScreen : Named
    {
        public uint Version;
        public List<string> Pages = new List<string>();
        public List<ulong> Pages_Padding = new List<ulong>();

        public FrontendScreen(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            uint PageCount = reader.ReadUInt32();
            for (int i = 0; i < PageCount; i++)
            {
                ulong Pad = 0;
                Pages.Add(Util.ReadString(reader, ref Pad));
                Pages_Padding.Add(Pad);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Screen: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Screen {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Pages: {Pages.Count}");
            for (int i = 0; i < Pages.Count; i++)
            {
                Lines.AppendLine($"Page{i}: {Pages[i]}");
            }

            return Lines.ToString();
        }
    }
}