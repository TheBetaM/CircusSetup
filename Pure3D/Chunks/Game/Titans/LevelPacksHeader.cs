using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0xD8532100)]
    public class LevelPacksHeader : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;
        public string CacheName;
        public ulong CacheName_padding;

        public LevelPacksHeader(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt3 = reader.ReadUInt32();
            CacheName = Util.ReadString(reader, ref CacheName_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Level Packs Header: {Name} / {CacheName}";
        }
    }
}
