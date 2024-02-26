using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17009)]
    public class BillboardTextureUV : Unknown
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;
        public float[] UnkFloat1;
        public float[] UnkFloat2;
        public float UnkFloat3;
        public float UnkFloat4;
        public BillboardTextureUV(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            UnkFloat1 = new float[4];
            UnkFloat2 = new float[4];
            for (int i = 0; i < 4; i++)
            {
                UnkFloat1[i] = reader.ReadSingle();
                UnkFloat2[i] = reader.ReadSingle();
            }
            UnkFloat3 = reader.ReadSingle();
            UnkFloat4 = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Billboard Texture UV";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Texture UV");
            Lines.AppendLine($"UnkInt1: {UnkInt1}");
            Lines.AppendLine($"UnkInt2: {UnkInt2}");
            Lines.AppendLine($"UnkInt3: {UnkInt3}");
            for (int i = 0; i < 4; i++)
            {
                Lines.AppendLine($"UnkFloat1 {i}: {UnkFloat1[i]}");
                Lines.AppendLine($"UnkFloat2 {i}: {UnkFloat2[i]}");
            }
            Lines.AppendLine($"UnkFloat3: {UnkFloat3}");
            Lines.AppendLine($"UnkFloat4: {UnkFloat4}");

            return Lines.ToString();
        }
    }
}