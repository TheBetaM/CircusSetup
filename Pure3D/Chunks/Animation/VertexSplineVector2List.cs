using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x121401)]
    public class VertexSplineVector2List : Named
    {
        public byte[] Remain;

        public VertexSplineVector2List(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long startpos = reader.BaseStream.Position + length;
            base.ReadHeader(reader, length);

            long rest = startpos - reader.BaseStream.Position;
            Remain = reader.ReadBytes((int)rest);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Vertex Spline Vector2 List: {Name}";
        }
    }
}