using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000A)]
    public class IndexList : Chunk
    {
        public uint[] Indices;

        public IndexList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Indices = new uint[len];
            for (int i = 0; i < len; i++)
                Indices[i] = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Indices.Length);
            for (int i = 0; i < Indices.Length; i++)
            {
                writer.Write(Indices[i]);
            }
        }

        public override string ToString()
        {
            return $"Indices List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Indices List");
            Lines.AppendLine($"Length: {Indices.Length}");
            for (int i = 0; i < Indices.Length; i++)
            {
                Lines.AppendLine($"Ind{i}: {Indices[i]}");
            }

            return Lines.ToString();
        }
    }
}
