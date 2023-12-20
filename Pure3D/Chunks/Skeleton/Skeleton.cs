using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4500)]
    public class Skeleton : Named
    {
        public uint Version;
        public uint NumJoints; // should be equal to # of children

        public Skeleton(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            NumJoints = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            writer.Write(NumJoints);
        }

        public override string ToString()
        {
            return $"Skeleton: {Name} Ver: {Version} Joints: {NumJoints}";
        }
    }
}
