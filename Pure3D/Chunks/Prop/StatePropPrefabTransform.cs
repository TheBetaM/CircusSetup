using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x05000001)]
    public class StatePropPrefabTransform : Named
    {
        public Matrix4x4 Matrix;

        public StatePropPrefabTransform(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Matrix = Util.ReadMatrix(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Prefab Transform: {Name}";
        }
    }
}