using System.Collections.Generic;
using System.IO;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x4504)]
    public class SkeletonJointBonePreserve : Chunk
    {
        public uint PreserveBoneLengths;

        public SkeletonJointBonePreserve(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            PreserveBoneLengths = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(PreserveBoneLengths);
        }

        public override string ToString()
        {
            return $"Skeleton Joint Bone Preserve {PreserveBoneLengths}";
        }
    }
}
