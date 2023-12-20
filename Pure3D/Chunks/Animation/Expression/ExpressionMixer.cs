using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x21002)]
    public class ExpressionMixer : Named
    {
        public uint Version;

        public string ModelName;
        public ulong ModelName_padding;
        public string GroupName;
        public ulong GroupName_padding;
        public uint UnkInt;

        public ExpressionMixer(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt = reader.ReadUInt32();
            ModelName = Util.ReadString(reader, ref ModelName_padding);
            GroupName = Util.ReadString(reader, ref GroupName_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Expression Mixer {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Expression Mixer: {Name}");
            Lines.AppendLine($"Model: {ModelName}");
            Lines.AppendLine($"Group: {GroupName}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"UnkInt: {UnkInt}");

            return Lines.ToString();
        }
    }
}