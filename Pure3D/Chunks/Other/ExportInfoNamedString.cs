using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x7031)]
    public class ExportInfoNamedString : Named
    {
        public string Value;
        public ulong Value_padding;

        public ExportInfoNamedString(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Value = Util.ReadString(reader, ref Value_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            Util.WriteString(writer, Value, Value_padding);
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Export Info Named String");
            Lines.AppendLine(Name);
            Lines.AppendLine(Value);

            return Lines.ToString();
        }
    }
}