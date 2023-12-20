using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x7016015)]
    public class CollisionTintColorAttribute : Named
    {
        public uint UnkInt;
        public List<ushort> Index1 = new List<ushort>();
        public List<Vector3> Colors = new List<Vector3>();

        public CollisionTintColorAttribute(File file, uint type) : base(file, type)
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
            }
            for (int i = 0; i < Count; i++)
            {
                Colors.Add(Util.ReadVector3(reader));
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Collision Tint Color Attribute";
        }
    }
}