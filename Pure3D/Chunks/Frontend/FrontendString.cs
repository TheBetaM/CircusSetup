using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x1800B)]
    public class FrontendString : Chunk
    {
        public string BibleName;
        public ulong BibleName_padding;
        public string BibleText;
        public ulong BibleText_padding;

        public FrontendString(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            BibleName = Util.ReadString(reader, ref BibleName_padding);
            BibleText = Util.ReadString(reader, ref BibleText_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteString(writer, BibleName, BibleName_padding);
            Util.WriteString(writer, BibleText, BibleText_padding);
        }

        public override string ToString()
        {
            return $"FE String - {BibleName}: {BibleText}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend String");
            Lines.AppendLine($"BibleName: {BibleName}");
            Lines.AppendLine($"BibleText: {BibleText}");

            return Lines.ToString();
        }
    }
}