using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x13004)]
    public class LightShadow : Unknown
    {
        public uint Shadow;

        public LightShadow(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Shadow = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Shadow);
        }

        public override string ToString()
        {
            return $"Light Shadow {Shadow}";
        }
    }
}