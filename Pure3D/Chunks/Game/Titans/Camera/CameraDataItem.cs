using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5432103)]
    public class CameraDataItem : Chunk
    {
        public uint ItemIndex; // item ID can overlap between different groups

        public CameraDataItem(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            ItemIndex = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(ItemIndex);
        }

        public override string ToString()
        {
            return $"Camera Data Item ID: {ItemIndex}";
        }
    }
}
