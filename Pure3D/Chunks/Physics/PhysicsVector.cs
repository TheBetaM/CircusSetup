using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7011002)]
    public class PhysicsVector : Chunk
    {
        public Vector3 Vector;

        public PhysicsVector(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Vector = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, Vector);
        }

        public override string ToString()
        {
            return $"Physics Vector";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Physics Vector");
            Lines.AppendLine($"X: {Vector.X}");
            Lines.AppendLine($"Y: {Vector.Y}");
            Lines.AppendLine($"Z: {Vector.Z}");

            return Lines.ToString();
        }
    }
}
