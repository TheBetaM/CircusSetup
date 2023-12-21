using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1580C)]
    public class ParticleSystem : Named
    {
        public uint UnkInt1;
        public float UnkFloat1;
        public uint UnkInt2;
        public uint UnkInt3;
        public uint UnkInt4;
        public uint UnkInt5;
        public uint UnkInt6;
        public float UnkFloat2;
        public uint UnkInt7;
        public uint UnkInt8;
        public uint UnkInt9;
        public uint UnkInt10;
        

        public ParticleSystem(File file, uint type) : base(file, type)
        {

        }
        
        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkFloat1 = reader.ReadSingle();
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            UnkInt4 = reader.ReadUInt32();
            UnkInt5 = reader.ReadUInt32();
            UnkInt6 = reader.ReadUInt32();
            UnkFloat2 = reader.ReadSingle();
            UnkInt7 = reader.ReadUInt32();
            UnkInt8 = reader.ReadUInt32();
            UnkInt9 = reader.ReadUInt32();
            UnkInt10 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Particle System: {Name}";
        }
    }
}
