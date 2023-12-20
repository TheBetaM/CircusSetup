using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5432100)]
    public class CameraDataVector1 : Named
    {
        public List<float> VectorData = new List<float>();

        public CameraDataVector1(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            for (int i = 0; i < 24; i++)
            {
                VectorData.Add(reader.ReadSingle());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"CameraDataVector1Data {Name}";
        }
    }

    [ChunkType(0x5432101)]
    public class CameraDataVector2 : CameraDataVector1
    {
        public CameraDataVector2(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            return $"CameraDataVector2Data {Name}";
        }
    }

    [ChunkType(0x5432102)]
    public class CameraDataVector3 : CameraDataVector1
    {
        public CameraDataVector3(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            return $"CameraDataVector3Data {Name}";
        }
    }
}
