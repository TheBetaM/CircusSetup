using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15B00)]
    public class ParticlePointGenerator : Chunk
    {
        public uint UnkInt1;
        public float UnkFloat1;
        public float UnkFloat2;
        public float UnkFloat3;
        public ParticlePointGenerator(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkFloat1 = reader.ReadSingle();
            UnkFloat2 = reader.ReadSingle();
            UnkFloat3 = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt1);
            writer.Write(UnkFloat1);
            writer.Write(UnkFloat2);
            writer.Write(UnkFloat3);
        }

        public override string ToString()
        {
            return $"Particle Point Generator";
        }
    }
}
