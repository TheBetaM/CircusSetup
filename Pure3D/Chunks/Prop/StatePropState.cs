using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x08020009)]
    public class StatePropState : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint NumVisibilites;
        public uint NumFrameControllers;
        public uint UnkInt3;
        public uint UnkInt4;
        public float UnkFloat1;
        public uint UnkInt5;
        public uint UnkInt6;
        public uint UnkInt7;
        public uint UnkInt8;
        public string UnkName;
        public ulong UnkName_padding;
        public StatePropState(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long pos = reader.BaseStream.Position;
            base.ReadHeader(reader, length);
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            NumVisibilites = reader.ReadUInt32();
            NumFrameControllers = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            UnkInt4 = reader.ReadUInt32();
            UnkFloat1 = reader.ReadSingle();
            UnkInt5 = reader.ReadUInt32();
            UnkInt6 = reader.ReadUInt32();
            // Titans
            if (reader.BaseStream.Position != pos + length)
            {
                UnkInt7 = reader.ReadUInt32();
                UnkInt8 = reader.ReadUInt32();
            }
            // MoM
            if (reader.BaseStream.Position != pos + length)
            {
                UnkInt8 = 0;
                reader.BaseStream.Position = pos;
                base.ReadHeader(reader, length);
                UnkInt1 = reader.ReadUInt32();
                UnkName = Util.ReadString(reader, ref UnkName_padding);
                NumVisibilites = reader.ReadUInt32();
                NumFrameControllers = reader.ReadUInt32();
                UnkInt2 = reader.ReadUInt32();
                UnkInt3 = reader.ReadUInt32();
                UnkInt4 = reader.ReadUInt32();
                UnkFloat1 = reader.ReadSingle();
                UnkInt5 = reader.ReadUInt32();
                UnkInt6 = reader.ReadUInt32();
                UnkInt7 = reader.ReadUInt32();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"State: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"State Prop State: {Name}");
            Lines.AppendLine($"UnkInt1: {UnkInt1}");
            Lines.AppendLine($"UnkInt2: {UnkInt2}");
            Lines.AppendLine($"UnkInt3: {UnkInt3}");
            Lines.AppendLine($"UnkInt4: {UnkInt4}");
            Lines.AppendLine($"UnkInt5: {UnkInt5}");
            Lines.AppendLine($"UnkInt6: {UnkInt6}");
            Lines.AppendLine($"UnkFloat1: {UnkFloat1}");
            Lines.AppendLine($"Visibilities: {NumVisibilites}");
            Lines.AppendLine($"Frame Controllers: {NumFrameControllers}");
            Lines.AppendLine($"UnkInt7: {UnkInt7}");
            Lines.AppendLine($"UnkName: {UnkName}");
            Lines.AppendLine($"UnkInt8: {UnkInt8}");

            return Lines.ToString();
        }
    }
}