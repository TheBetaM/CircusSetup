using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    [ChunkType(0x121201)]
    public class FrameController : Named
    {
        public uint UnkInt1; // autoplay?
        public uint UnkInt2;
        public uint UnkInt3; // count?
        public string Parameter;

        public string ModelName;
        public ulong ModelName_padding;
        public string AnimName;
        public ulong AnimName_padding;

        public FrameController(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            Parameter = Encoding.ASCII.GetString(reader.ReadBytes(8));
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
            ModelName = Util.ReadString(reader, ref ModelName_padding);
            AnimName = Util.ReadString(reader, ref AnimName_padding);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Frame Controller: {Parameter} {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frame Controller {Name}");
            Lines.AppendLine($"Model/Object: {ModelName}");
            Lines.AppendLine($"Anim: {AnimName}");
            Lines.AppendLine($"Parameter: {Parameter}");
            Lines.AppendLine($"Ints: {UnkInt1} {UnkInt2} {UnkInt3}");

            return Lines.ToString();
        }
    }
}