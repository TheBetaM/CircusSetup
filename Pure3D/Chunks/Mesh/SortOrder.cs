using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x122000)]
    public class SortOrder : Chunk
    {
        public float Val1;
        public float Val2;
        public SortOrder(File file, uint type) : base(file, type)
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
            return $"Sort Order {Val1} / {Val2}";
        }
    }
}