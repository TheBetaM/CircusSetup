using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x18011)]
    public class FrontendButton : Named
    {
        public uint UnkInt;
        public byte UnkByte1;
        public byte UnkByte2;
        public byte UnkByte3;


        public FrontendButton(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkInt = reader.ReadUInt32();
            UnkByte1 = reader.ReadByte();
            UnkByte2 = reader.ReadByte();
            UnkByte3 = reader.ReadByte();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(UnkInt);
            writer.Write(UnkByte1);
            writer.Write(UnkByte2);
            writer.Write(UnkByte3);
        }

        public override string ToString()
        {
            return $"FE Button: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Button {Name}");
            Lines.AppendLine($"Int: {UnkInt}");
            Lines.AppendLine($"Byte1: {UnkByte1}");
            Lines.AppendLine($"Byte2: {UnkByte2}");
            Lines.AppendLine($"Byte3: {UnkByte3}");

            return Lines.ToString();
        }
    }

    [ChunkType(0x18012)]
    public class FrontendButton2 : Named
    {
        public uint UnkInt;
        public byte UnkByte1;
        public byte UnkByte2;
        public byte UnkByte3;


        public FrontendButton2(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkInt = reader.ReadUInt32();
            UnkByte1 = reader.ReadByte();
            UnkByte2 = reader.ReadByte();
            UnkByte3 = reader.ReadByte();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(UnkInt);
            writer.Write(UnkByte1);
            writer.Write(UnkByte2);
            writer.Write(UnkByte3);
        }

        public override string ToString()
        {
            return $"FE Button 2: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Button 2 {Name}");
            Lines.AppendLine($"Int: {UnkInt}");
            Lines.AppendLine($"Byte1: {UnkByte1}");
            Lines.AppendLine($"Byte2: {UnkByte2}");
            Lines.AppendLine($"Byte3: {UnkByte3}");

            return Lines.ToString();
        }
    }
}