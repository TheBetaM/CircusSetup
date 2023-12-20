using System.Collections.Generic;
using System.IO;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x4519)]
    public class CompositeDrawableSortOrder : Chunk
    {
        public float SortOrder;

        public CompositeDrawableSortOrder(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            SortOrder = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(SortOrder);
        }

        public override string ToString()
        {
            return $"Composite Drawable Sort Order ({SortOrder})";
        }
    }
}
