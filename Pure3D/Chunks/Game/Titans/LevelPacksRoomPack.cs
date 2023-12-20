using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0xD8532101)]
    public class LevelPacksRoomPack : Named
    {
        public uint UnkInt1;
        public uint UnkInt2; // MoM only

        public LevelPacksRoomPack(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long pos = reader.BaseStream.Position;
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            if (reader.BaseStream.Position != pos + length)
            {
                UnkInt2 = reader.ReadUInt32();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Room Pack: {Name}";
        }
    }
}
