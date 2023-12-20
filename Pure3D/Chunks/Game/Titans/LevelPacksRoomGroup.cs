using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0xD8532102)]
    public class LevelPacksRoomGroup : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;
        public uint ItemCount;

        public LevelPacksRoomGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            ItemCount = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Level Packs Room Group: {Name} / {ItemCount}";
        }
    }
}
