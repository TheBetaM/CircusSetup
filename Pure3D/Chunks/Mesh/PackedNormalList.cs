using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10010)]
    public class PackedNormalList : Chunk
    {
        public uint NormalsCount;
        public byte[] Normals;

        public PackedNormalList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            NormalsCount = reader.ReadUInt32();
            Normals = reader.ReadBytes((int)NormalsCount);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Normals.Length);
            writer.Write(Normals);
        }

        public override string ToString()
        {
            return $"Packed Normal List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Packed Normal List");
            Lines.AppendLine($"Length: {Normals.Length}");
            for (int i = 0; i < Normals.Length; i++)
            {
                Lines.AppendLine($"Normal{i}: {Normals[i]}");
            }

            return Lines.ToString();
        }
    }
}
