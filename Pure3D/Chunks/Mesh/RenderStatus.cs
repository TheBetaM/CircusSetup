using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10017)]
    public class RenderStatus : Chunk
    {
        public uint CastShadow;

        public RenderStatus(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            CastShadow = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(CastShadow);
        }

        public override string ToString()
        {
            return $"Render Status: {CastShadow}";
        }
    }
}