using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x121305)]
    public class VertexAnimHeader : Chunk
    {
        public uint UnkInt;
        public List<int> IDs = new List<int>();
        public List<int> Values = new List<int>();
        
        public VertexAnimHeader(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt = reader.ReadUInt32();
            uint count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                IDs.Add(reader.ReadInt32());
            }
            for (int i = 0; i < count; i++)
            {
                Values.Add(reader.ReadInt32());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Vertex Anim Header";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Vertex Anim Header: {UnkInt}");
            for (int i = 0; i < IDs.Count; i++)
            {
                Lines.AppendLine($"#{i}: {IDs[i]}: {Values[i]}");
            }

            return Lines.ToString();
        }
    }
}