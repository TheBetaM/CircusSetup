using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17006)]
    public class BillboardQuadGroupCTTR : Named
    {
        public uint UnkInt1;
        public string MaterialName;
        public ulong MaterialName_padding;
        public uint UnkInt2;
        public uint UnkInt3;
        public uint UnkInt4;
        public uint UnkInt5;
        public uint UnkInt6;
        public BillboardQuadGroupCTTR(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            MaterialName = Util.ReadString(reader, ref MaterialName_padding);
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            UnkInt4 = reader.ReadUInt32();
            UnkInt5 = reader.ReadUInt32();
            UnkInt6 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Billboard Quad Group: {Name}";
        }
    }
}