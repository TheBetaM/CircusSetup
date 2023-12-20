using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x123001)]
    public class CompositeDrawablePrimitive : Named
    {

        public uint UnkInt1; // usually zero
        public uint UnkInt2; // usually zero
        public uint UnkInt3; // non-zero
        public uint UnkInt4; // Skeleton bone

        public CompositeDrawablePrimitive(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt3 = reader.ReadUInt32();
            UnkInt4 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Primitive {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Primitive {Name}");
            Lines.AppendLine($"Ints: {UnkInt1} {UnkInt2} {UnkInt3}");
            Lines.AppendLine($"Attached to joint: {UnkInt4}");

            return Lines.ToString();
        }
    }
}