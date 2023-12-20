using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5432105)]
    public class CameraDataPath : Chunk
    {
        public List<Vector3> VectorData = new List<Vector3>();

        public CameraDataPath(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint ItemCount = reader.ReadUInt32();
            for (int i = 0; i < ItemCount; i++)
            {
                VectorData.Add(Util.ReadVector3(reader));
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"CameraDataPath {VectorData.Count}";
        }
    }
}
