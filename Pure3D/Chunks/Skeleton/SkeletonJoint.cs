using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4501)]
    public class SkeletonJoint : Named
    {
        public uint SkeletonParent;
        public int DOF;
        public int FreeAxis;
        public int PrimaryAxis;
        public int SecondaryAxis;
        public int TwistAxis;
        public Matrix4x4 RestPose;

        public SkeletonJoint(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            SkeletonParent = reader.ReadUInt32();
            DOF = reader.ReadInt32();
            FreeAxis = reader.ReadInt32();
            PrimaryAxis = reader.ReadInt32();
            SecondaryAxis = reader.ReadInt32();
            TwistAxis = reader.ReadInt32();
            RestPose = Util.ReadMatrix(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(SkeletonParent);
            writer.Write(DOF);
            writer.Write(FreeAxis);
            writer.Write(PrimaryAxis);
            writer.Write(SecondaryAxis);
            writer.Write(TwistAxis);
            Util.WriteMatrix(writer, RestPose);
        }

        public override string ToString()
        {
            return $"Skeleton Joint: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Skeleton Joint: {Name}");
            Lines.AppendLine($"Parent Joint: {SkeletonParent}");
            Lines.AppendLine($"DOF: {DOF}");
            Lines.AppendLine($"FreeAxis: {FreeAxis}");
            Lines.AppendLine($"PrimaryAxis: {PrimaryAxis}");
            Lines.AppendLine($"SecondaryAxis: {SecondaryAxis}");
            Lines.AppendLine($"TwistAxis: {TwistAxis}");
            Lines.AppendLine($"RestPose: {RestPose}");

            return Lines.ToString();
        }
    }
}
