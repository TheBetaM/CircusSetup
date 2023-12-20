using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7016000)]
    public class CollisionAttributes : Chunk
    {
        public uint UnkInt;
        public uint Count;

        public CollisionAttributes(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt = reader.ReadUInt32();
            Count = reader.ReadUInt32();        
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Collision Attributes: {Count}";
        }
    }
}