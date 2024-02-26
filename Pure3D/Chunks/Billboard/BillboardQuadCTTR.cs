using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17005)]
    public class BillboardQuadCTTR : Named
    {
        public uint Version;
        public uint UnkInt2;
        public uint UnkInt3;
        public uint BillboardMode;
        public uint Color;
        public float UnkFloat1;
        public float UnkFloat2;
        public float UnkFloat3;
        public uint UnkInt6;

        public BillboardQuadCTTR(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            BillboardMode = reader.ReadUInt32();
            Color = reader.ReadUInt32();
            UnkFloat1 = reader.ReadSingle();
            UnkFloat2 = reader.ReadSingle();
            UnkFloat3 = reader.ReadSingle();
            if (Version != 0)
            {
                // MoM
                UnkInt6 = reader.ReadUInt32();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Billboard Quad: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Quad CTTR {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"UnkInt2: {UnkInt2}");
            Lines.AppendLine($"UnkInt3: {UnkInt3}");
            Lines.AppendLine($"Billboard Mode: {BillboardMode}");
            Lines.AppendLine($"Color: {Color}");
            Lines.AppendLine($"UnkFloat1: {UnkFloat1}");
            Lines.AppendLine($"UnkFloat2: {UnkFloat2}");
            Lines.AppendLine($"UnkFloat3: {UnkFloat3}");
            Lines.AppendLine($"UnkInt6: {UnkInt6}");

            return Lines.ToString();
        }
    }
}