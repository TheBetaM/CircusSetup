using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x05000003)]
    public class StatePropPrefabOffset : Chunk
    {
        public Vector4 Vector;
        public uint UnkInt;

        public StatePropPrefabOffset(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Vector = Util.ReadVector4(reader);
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Prefab Offset {Vector.X}/{Vector.Y}/{Vector.Z}/{Vector.W}";
        }
    }
}