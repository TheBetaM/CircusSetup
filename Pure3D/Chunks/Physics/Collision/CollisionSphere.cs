using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010002)]
    public class CollisionSphere : Chunk
    {
        public float Radius;

        public CollisionSphere(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Radius = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Radius);
        }

        public override string ToString()
        {
            return $"Collision Sphere: Radius {Radius}";
        }
    }
}
