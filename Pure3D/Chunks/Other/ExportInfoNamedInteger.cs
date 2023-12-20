using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x7032)]
    public class ExportInfoNamedInteger : Named
    {
        public uint Value;

        public ExportInfoNamedInteger(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Value = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Value);
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Export Info Named Integer");
            Lines.AppendLine(Name);
            Lines.AppendLine(Value.ToString());

            return Lines.ToString();
        }
    }
}