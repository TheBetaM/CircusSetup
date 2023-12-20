using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7016012)]
    public class CollisionSurfaceTypeAttribute : Named
    {
        public uint UnkInt;
        public List<ushort> Index1 = new List<ushort>();
        public List<ushort> Index2 = new List<ushort>();

        public CollisionSurfaceTypeAttribute(File file, uint type) : base(file, type)
        {

        }

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

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Collision Surface Type Attribute";
        }
    }
}