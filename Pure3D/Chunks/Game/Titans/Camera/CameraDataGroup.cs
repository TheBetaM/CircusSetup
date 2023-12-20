using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5432104)]
    public class CameraDataGroup : Chunk
    {
        public uint ItemCount;

        public CameraDataGroup(File file, uint type) : base(file, type)
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
            return $"CameraDataGroup: {ItemCount}";
        }
    }

    [ChunkType(0x5432107)]
    public class CameraDataGroupHeader : Chunk
    {
        public uint ItemCount;

        public CameraDataGroupHeader(File file, uint type) : base(file, type)
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
            return $"CameraDataGroupHeader: {ItemCount}";
        }
    }
}
