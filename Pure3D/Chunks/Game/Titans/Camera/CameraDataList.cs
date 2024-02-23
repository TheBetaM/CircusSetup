using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5432104)]
    public class CameraDataList : Chunk
    {
        public uint ItemCount;

        public CameraDataList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            ItemCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(ItemCount);
        }

        public override string ToString()
        {
            return $"Camera Data List: {ItemCount} groups";
        }
    }

    [ChunkType(0x5432107)]
    public class CameraDataListHeader : Chunk
    {
        public uint ItemCount;

        public CameraDataListHeader(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            ItemCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(ItemCount);
        }

        public override string ToString()
        {
            return $"Camera Data List Header: {ItemCount} total items";
        }
    }
}
