using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x13000)]
    public class Light : Named
    {
        public uint Bitfield;
        public uint Type;
        public byte R;
        public byte G;
        public byte B;
        public byte A;
        public float UnkFloat;
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;

        public Light(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Bitfield = reader.ReadUInt32();
            Type = reader.ReadUInt32();
            R = reader.ReadByte();
            G = reader.ReadByte();
            B = reader.ReadByte();
            A = reader.ReadByte();
            UnkFloat = reader.ReadSingle();
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Light: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Light {Name}");
            Lines.AppendLine($"Type: {Type}");
            Lines.AppendLine($"Color: {R} / {G} / {B} / {A}");
            Lines.AppendLine($"Bitfield: {Bitfield:X8}");
            Lines.AppendLine($"UnkFloat: {UnkFloat}");
            Lines.AppendLine($"UnkInt1: {UnkInt1}");
            Lines.AppendLine($"UnkInt1: {UnkInt2}");
            Lines.AppendLine($"UnkInt1: {UnkInt3}");

            return Lines.ToString();
        }
    }
}