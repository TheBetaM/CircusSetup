using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020020)]
    public class StatePropInstance : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;

        public StatePropInstance(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"SP Instance: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"State Prop Instance: {Name}");

            return Lines.ToString();
        }
    }
}