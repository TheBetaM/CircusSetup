using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x05000006)]
    public class StatePropPrefabZone : Chunk
    {
        public Vector3 Vec1;
        public Vector3 Vec2;
        public uint UnkInt;

        public StatePropPrefabZone(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Vec1 = Util.ReadVector3(reader);
            Vec2 = Util.ReadVector3(reader);
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Prefab Zone";
        }
    }
}