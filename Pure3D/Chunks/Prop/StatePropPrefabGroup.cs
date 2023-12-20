using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x05000002)]
    public class StatePropPrefabGroup : Chunk
    {
        public uint ItemCount;

        public StatePropPrefabGroup(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            ItemCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(ItemCount);
        }

        public override string ToString()
        {
            return $"SP Prefab Group: {ItemCount}";
        }
    }
}