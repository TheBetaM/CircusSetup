using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x4503)]
    public class SkeletonJointMirrorMap : Chunk
    {
        public uint MappedJointIndex;
        public float XAxisMap;
        public float YAxisMap;
        public float ZAxisMap;

        public SkeletonJointMirrorMap(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            MappedJointIndex = reader.ReadUInt32();
            XAxisMap = reader.ReadSingle();
            YAxisMap = reader.ReadSingle();
            ZAxisMap = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(MappedJointIndex);
            writer.Write(XAxisMap);
            writer.Write(YAxisMap);
            writer.Write(ZAxisMap);
        }

        public override string ToString()
        {
            return $"Skeleton Joint Mirror Map";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Skeleton Joint Mirror Map");
            Lines.AppendLine($"MappedJointIndex {MappedJointIndex}");
            Lines.AppendLine($"XAxisMap {XAxisMap}");
            Lines.AppendLine($"YAxisMap {YAxisMap}");
            Lines.AppendLine($"ZAxisMap {ZAxisMap}");

            return Lines.ToString();
        }
    }
}
