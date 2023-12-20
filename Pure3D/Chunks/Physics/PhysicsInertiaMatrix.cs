using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7011001)]
    public class PhysicsInertiaMatrix : Chunk
    {
        public Vector3 X;
        public Vector3 Y;

        public PhysicsInertiaMatrix(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            X = Util.ReadVector3(reader);
            Y = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            Util.WriteVector3(writer, X);
            Util.WriteVector3(writer, Y);
        }

        public override string ToString()
        {
            return "Physics Inertia Matrix";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Physics Inertia Matrix");
            Lines.AppendLine($"X: {X}");
            Lines.AppendLine($"Y: {Y}");

            return Lines.ToString();
        }
    }
}
