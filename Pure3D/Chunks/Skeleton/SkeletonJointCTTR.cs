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
        public int ID;
        public Vector3 UnkVec1 = new Vector3();
        public Vector3 UnkVec2 = new Vector3();
        public Vector3 UnkVec3 = new Vector3();
        public Vector3 UnkVec4 = new Vector3();
        public short UnkShort;
        public int UnkInt;

        public SkeletonJointCTTR(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            SkeletonParent = reader.ReadUInt32();
            RestPose = Util.ReadMatrix(reader);
            if (Parent is SkeletonCTTR skeleton && skeleton.Version != 0)
            {
                // Titans/MoM
                UnkVec1 = Util.ReadVector3(reader);
                UnkVec2 = Util.ReadVector3(reader);
                UnkVec3 = Util.ReadVector3(reader);
                UnkVec4 = Util.ReadVector3(reader);
                UnkShort = reader.ReadInt16();
                UnkInt = reader.ReadInt32();
            }
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

            Lines.AppendLine($"UnkVec1 {UnkVec1}");
            Lines.AppendLine($"UnkVec2 {UnkVec2}");
            Lines.AppendLine($"UnkVec3 {UnkVec3}");
            Lines.AppendLine($"UnkVec4 {UnkVec4}");
            Lines.AppendLine($"Short {UnkShort}");
            Lines.AppendLine($"Int {UnkInt}");

            return Lines.ToString();
        }
    }
}