using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x21000)]
    public class Expression : Named
    {

        public uint Version;

        public List<uint> Shapes = new List<uint>();
        public List<float> Values = new List<float>();

        public Expression(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Values.Add(reader.ReadSingle());
            }
            for (int i = 0; i < Count; i++)
            {
                Shapes.Add(reader.ReadUInt32());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Expression: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Expression: {Name}");
            for (int i = 0; i < Shapes.Count; i++)
            {
                Lines.AppendLine($"#{i}: {Shapes[i]}: {Values[i]}");
            }

            return Lines.ToString();
        }
    }
}