using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x23001)]
    public class SkeletonJointCTTR : Named
    {
        public uint SkeletonParent;
        public Matrix4x4 RestPose;
        public byte[] Remain;
        public int ID;

        public SkeletonJointCTTR(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long startpos = reader.BaseStream.Position + length;
            base.ReadHeader(reader, length);
            SkeletonParent = reader.ReadUInt32();
            RestPose = Util.ReadMatrix(reader);
            // Titans/MoM
            long rest = startpos - reader.BaseStream.Position;
            Remain = reader.ReadBytes((int)rest);
            foreach (var check in Parent.GetChildren<SkeletonJointCTTR>())
            {
                if (check == this)
                {
                    break;
                }
                ID++;
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(SkeletonParent);
            Util.WriteMatrix(writer, RestPose);
        }

        public override string ToString()
        {
            return $"Joint {ID}: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Joint CTTR: {Name}");
            Lines.AppendLine($"Parent Joint ID: {SkeletonParent}");
            Lines.AppendLine($"ID: {ID}");

            string SkeletonParentName = "";
            if (SkeletonParent > 0)
            {
                SkeletonParentName = Parent.GetChildren<SkeletonJointCTTR>()[SkeletonParent].Name;
            }
            else
            {
                SkeletonParentName = "Root";
            }
            Lines.AppendLine($"Parent Joint: {SkeletonParentName}");

            Lines.AppendLine($"RestPose: {RestPose}");
            Matrix4x4.Decompose(RestPose, out var scale, out var rot, out var pos);
            Lines.AppendLine($"Scale {scale}");
            Lines.AppendLine($"Rot {rot}");
            Lines.AppendLine($"Pos {pos}");

            return Lines.ToString();
        }
    }
}