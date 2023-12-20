using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1800D)]
    public class FrontendTextBible : Named
    {
        public uint LanguageCount;
        public string Languages;
        public ulong Languages_padding;

        public FrontendTextBible(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            LanguageCount = reader.ReadUInt32();
            Languages = Util.ReadString(reader, ref Languages_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(LanguageCount);
            Util.WriteString(writer, Languages, Languages_padding);
        }

        public override string ToString()
        {
            return $"FE Text Bible: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Text Bible");
            Lines.AppendLine($"LanguageCount: {LanguageCount}");
            Lines.AppendLine($"LanguagesString: {Languages}");

            return Lines.ToString();
        }
    }
}