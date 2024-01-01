using System.Collections.Generic;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(0x15B02)]
    public class ParticleUnk4 : Chunk
    {
        public uint UnkInt1;
        public float UnkFloat1;
        public float UnkFloat2;
        public ParticleUnk4(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkFloat1 = reader.ReadSingle();
            UnkFloat2 = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt1);
            writer.Write(UnkFloat1);
            writer.Write(UnkFloat2);
        }

        public override string ToString()
        {
            return $"Particle Unk4 {UnkInt1}/{UnkFloat1}/{UnkFloat2}";
        }
    }
}
