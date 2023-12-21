using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x7030)]
    public class ExportInfo : Named
    {

        public ExportInfo(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
        }

        public override string ToString()
        {
            return $"Export Info: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Export Info");
            Lines.AppendLine(Name);

            return Lines.ToString();
        }
    }
}