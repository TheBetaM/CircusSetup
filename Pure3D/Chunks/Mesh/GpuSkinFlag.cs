using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1002B)]
    public class GpuSkinFlag : Chunk
    {
        public bool Value;
        public GpuSkinFlag(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Value = reader.ReadUInt32() != 0;
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Value ? (uint)1 : (uint)0);
        }

        public override string ToString()
        {
            return $"GPU SKin: {Value}";
        }
    }
}