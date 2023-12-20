using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0xD8532103)]
    public class LevelPacksLevelName : Named
    {
        public uint UnkInt1;

        public LevelPacksLevelName(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Level Packs Level Name: {Name}";
        }
    }
}
