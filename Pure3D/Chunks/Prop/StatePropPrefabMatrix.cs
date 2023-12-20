using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x05000004)]
    public class StatePropPrefabMatrix : Chunk
    {
        public Matrix4x4 Matrix;
        public uint UnkInt;

        public StatePropPrefabMatrix(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Matrix = Util.ReadMatrix(reader);
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Prefab Matrix";
        }
    }

    [ChunkType(0x05000005)]
    public class StatePropPrefabMatrix2 : Chunk
    {
        public Matrix4x4 Matrix;
        public uint UnkInt;

        public StatePropPrefabMatrix2(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Matrix = Util.ReadMatrix(reader);
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Prefab Matrix2";
        }
    }
}