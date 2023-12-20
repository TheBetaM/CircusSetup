using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x701000A)]
    public class FenceHeader : Chunk
    {
        public uint Unk1;
        public uint Unk2;
        public uint Unk3;
        public Vector3 Vector1;
        public Vector3 Vector2;
        public FenceHeader(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Unk1 = reader.ReadUInt32();
            Unk2 = reader.ReadUInt32();
            Unk3 = reader.ReadUInt32();
            Vector1 = Util.ReadVector3(reader);
            Vector2 = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Fence Header";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Fence Header");
            Lines.AppendLine($"Unk1: {Unk1}");
            Lines.AppendLine($"Unk2: {Unk2}");
            Lines.AppendLine($"Unk3: {Unk3}");
            Lines.AppendLine($"Vector1: {Vector1}");
            Lines.AppendLine($"Vector2: {Vector2}");

            return Lines.ToString();
        }
    }
}