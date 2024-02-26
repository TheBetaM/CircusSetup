using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10027)]
    public class MultiColor128ListFlag : Chunk
    {
        public bool Value;
        public MultiColor128ListFlag(File file, uint type) : base(file, type)
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
            return $"MultiColor128: {Value}";
        }
    }
}