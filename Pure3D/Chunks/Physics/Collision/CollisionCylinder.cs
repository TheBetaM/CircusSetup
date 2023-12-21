using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010003)]
    public class CollisionCylinder : Chunk
    {
        public float Val1;
        public float Val2;
        public ushort Val3;
        public CollisionCylinder(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Val1 = reader.ReadSingle();
            Val2 = reader.ReadSingle();
            Val3 = reader.ReadUInt16();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Collision Cylinder {Val1}/{Val2}/{Val3}";
        }
    }
}