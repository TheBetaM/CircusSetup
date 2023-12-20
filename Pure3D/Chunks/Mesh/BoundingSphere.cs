using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10004)]
    public class BoundingSphere : Chunk
    {
        public Vector3 Centre;
        public float Radius;

        public BoundingSphere(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Centre = Util.ReadVector3(reader);
            Radius = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, Centre);
            writer.Write(Radius);
        }

        public override string ToString()
        {
            return "Bounding Sphere";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Bounding Sphere");
            Lines.AppendLine($"Centre: {Centre}");
            Lines.AppendLine($"Radius: {Radius}");

            return Lines.ToString();
        }
    }
}
