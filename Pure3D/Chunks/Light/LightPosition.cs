using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x13002)]
    public class LightPosition : Chunk
    {
        public Vector3 Position = new Vector3();

        public LightPosition(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Position = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, Position);
        }

        public override string ToString()
        {
            return $"Light Position {Position}";
        }
    }
}