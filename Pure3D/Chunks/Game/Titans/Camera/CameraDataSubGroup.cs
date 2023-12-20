using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5432106)]
    public class CameraDataSubGroup : Named
    {
        public uint ItemCount;

        public CameraDataSubGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            ItemCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"CameraDataSubGroup: {Name} / {ItemCount}";
        }
    }
}
