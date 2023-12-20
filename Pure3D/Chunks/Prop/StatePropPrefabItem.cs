using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x05000008)]
    public class StatePropPrefabItem2 : Named
    {

        public StatePropPrefabItem2(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
        }

        public override string ToString()
        {
            return $"SP Prefab Item2: {Name}";
        }
    }

    [ChunkType(0x05000007)]
    public class StatePropPrefabItem1 : Named
    {
        public uint UnkInt;

        public StatePropPrefabItem1(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(UnkInt);
        }

        public override string ToString()
        {
            return $"SP Prefab Item1: {Name}";
        }
    }
}