using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1002A)]
    public class MeshDataBBox2 : Chunk
    {
        public Vector3 Vec1;
        public Vector3 Vec2;
        public float UnkFloat;

        public MeshDataBBox2(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Vec1 = Util.ReadVector3(reader);
            Vec2 = Util.ReadVector3(reader);
            UnkFloat = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, Vec1);
            Util.WriteVector3(writer, Vec2);
            writer.Write(UnkFloat);
        }

        public override string ToString()
        {
            return $"MeshDataBBox2";
        }
    }
}