using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000006)]
    public class FencePositionPalette : Chunk
    {
        public List<ushort> Indices = new List<ushort>();
        public List<ushort> Flags = new List<ushort>();
        public FencePositionPalette(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint Count = reader.ReadUInt32();
            for (int i = 0; i <  Count; i++)
            {
                Indices.Add(reader.ReadUInt16());
                Indices.Add(reader.ReadUInt16());
                Indices.Add(reader.ReadUInt16());
                Flags.Add(reader.ReadUInt16());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Fence Position Palette: {Indices.Count}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Fence Position Palette");
            Lines.AppendLine($"Count: {Indices.Count}");
            for (int i = 0; i < Indices.Count; i++)
            {
                Lines.AppendLine($"#{i}: {Indices[i]}");
            }
            Lines.AppendLine($"Flags");
            for (int i = 0; i < Flags.Count; i++)
            {
                Lines.AppendLine($"#{i}: {Flags[i]}");
            }

            return Lines.ToString();
        }
    }
}