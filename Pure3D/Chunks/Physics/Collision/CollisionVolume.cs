using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010001)]
    public class CollisionVolume : Chunk
    {
        public Vector3 vector = new Vector3();
        
        public CollisionVolume(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }
        
        public override string ToString()
        {
            return $"Collision Volume {vector.X}/{vector.Y}/{vector.Z}";
        }
    }
}