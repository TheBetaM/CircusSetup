using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10003)]
    public class BoundingBox : Chunk
    {
        public Vector3 Low;
        public Vector3 High;

        public BoundingBox(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Low = Util.ReadVector3(reader);
            High = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, Low);
            Util.WriteVector3(writer, High);
        }

        public override string ToString()
        {
            return "Bounding Box";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Bounding Box");
            Lines.AppendLine($"Low: {Low}");
            Lines.AppendLine($"High: {High}");

            return Lines.ToString();
        }
    }
}
