using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7016014)]
    public class CollisionSaturationDataAttribute : Unknown
    {

        public CollisionSaturationDataAttribute(File file, uint type) : base(file, type)
        {

        }

        /*
        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                Index1.Add(reader.ReadUInt16());
                Index2.Add(reader.ReadUInt16());
            }
        }
        */

        public override string ToString()
        {
            return $"Collision Saturation Data Attribute";
        }
    }
}