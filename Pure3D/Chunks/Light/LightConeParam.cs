using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x13003)]
    public class LightConeParam : Chunk
    {
        public Vector4 Vec = new Vector4();
        public LightConeParam(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Vec.X = reader.ReadSingle();
            Vec.Y = reader.ReadSingle();
            Vec.Z = reader.ReadSingle();
            Vec.W = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Vec.X);
            writer.Write(Vec.Y);
            writer.Write(Vec.Z);
            writer.Write(Vec.W);
        }

        public override string ToString()
        {
            return $"Light Cone Param {Vec}";
        }
    }
}