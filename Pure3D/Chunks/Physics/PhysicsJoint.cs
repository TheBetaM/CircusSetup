using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7011020)]
    public class PhysicsJoint : Chunk
    {
        public uint Index;
        public float Volume;
        public float Stiffness;
        public float MaxAngle;
        public float MinAngle;
        public uint DOF;

        public PhysicsJoint(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Index = reader.ReadUInt32();
            Volume = reader.ReadSingle();
            Stiffness = reader.ReadSingle();
            MaxAngle = reader.ReadSingle();
            MinAngle = reader.ReadSingle();
            DOF = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Volume);
            writer.Write(Stiffness);
            writer.Write(MaxAngle);
            writer.Write(MinAngle);
            writer.Write(DOF);
        }

        public override string ToString()
        {
            return $"Physics Joint {Index}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Physics Joint");
            Lines.AppendLine($"Index: {Index}");
            Lines.AppendLine($"Volume: {Volume}");
            Lines.AppendLine($"Stiffness: {Stiffness}");
            Lines.AppendLine($"MaxAngle: {MaxAngle}");
            Lines.AppendLine($"MinAngle: {MinAngle}");
            Lines.AppendLine($"DOF: {DOF}");

            return Lines.ToString();
        }
    }
}
