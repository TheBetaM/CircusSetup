using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x13001)]
    public class LightDirection : Chunk
    {
        public Vector3 Direction = new Vector3();

        public LightDirection(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Direction = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, Direction);
        }

        public override string ToString()
        {
            return $"Light Direction {Direction}";
        }
    }
}