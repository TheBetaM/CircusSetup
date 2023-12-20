using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x18002)]
    public class FrontendPage : Named
    {
        public uint Version;
        public uint Width;
        public uint Height;

        public FrontendPage(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Page: {Name} ({Width}x{Height})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Page {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Width: {Width}");
            Lines.AppendLine($"Height: {Height}");

            return Lines.ToString();
        }
    }
}
