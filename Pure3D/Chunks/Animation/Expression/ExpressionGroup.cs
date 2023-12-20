using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x21001)]
    public class ExpressionGroup : Named
    {
        public uint Version;

        public string ModelName;
        public ulong ModelName_padding;
        public List<uint> Values = new List<uint>();

        public ExpressionGroup(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            ModelName = Util.ReadString(reader, ref ModelName_padding);
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Values.Add(reader.ReadUInt32());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Expression Group {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Expression Group: {Name}");
            Lines.AppendLine($"Model: {ModelName}");
            Lines.AppendLine($"Version: {Version}");

            return Lines.ToString();
        }
    }
}