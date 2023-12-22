using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17005)]
    public class BillboardCTTRUnk1 : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;
        public uint UnkInt4;
        public uint UnkInt5;
        public float UnkFloat1;
        public float UnkFloat2;
        public float UnkFloat3;
        public uint UnkInt6;

        public BillboardCTTRUnk1(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long pos = reader.BaseStream.Position;
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            UnkInt4 = reader.ReadUInt32();
            UnkInt5 = reader.ReadUInt32();
            UnkFloat1 = reader.ReadSingle();
            UnkFloat2 = reader.ReadSingle();
            UnkFloat3 = reader.ReadSingle();
            // MoM
            if (reader.BaseStream.Position != pos + length)
            {
                UnkInt6 = reader.ReadUInt32();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Billboard CTTR Unk1: {Name}";
        }
    }
}