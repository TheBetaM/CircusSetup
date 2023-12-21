using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x122001)]
    public class SortOrderUnkVector : Chunk
    {
        public float Val1;
        public float Val2;
        public SortOrderUnkVector(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Val1 = reader.ReadSingle();
            Val2 = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Val1);
            writer.Write(Val2);
        }

        public override string ToString()
        {
            return $"Sort Order Unk {Val1} / {Val2}";
        }
    }
}