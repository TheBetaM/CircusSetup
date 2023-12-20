using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x18023)]
    public class FrontendLabel : Named
    {
        public uint Version;
        public int PositionX;
        public int PositionY;
        public uint Width;
        public uint Height;
        public uint UnkInt5;
        public uint UnkInt6;
        public byte UnkByte2;
        public int UnkInt7;
        public uint UnkInt8;
        public uint UnkInt9;
        public string FontName;
        public ulong FontName_padding;
        public uint UnkInt1;
        public byte UnkByte1;
        public uint UnkInt2;
        public int UnkInt3;
        public uint UnkInt4;

        //Titans
        public uint UnkInt10;

        public FrontendLabel(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long pos = reader.BaseStream.Position;
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            PositionX = reader.ReadInt32();
            PositionY = reader.ReadInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            UnkInt5 = reader.ReadUInt32();
            UnkInt6 = reader.ReadUInt32();
            UnkByte2 = reader.ReadByte();
            UnkInt7 = reader.ReadInt32();
            UnkInt8 = reader.ReadUInt32();
            UnkInt9 = reader.ReadUInt32();
            FontName = Util.ReadString(reader, ref FontName_padding);
            UnkInt1 = reader.ReadUInt32();
            UnkByte1 = reader.ReadByte();
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadInt32();
            UnkInt4 = reader.ReadUInt32();
            //Titans
            if (reader.BaseStream.Position != pos + length)
            {
                UnkInt10 = reader.ReadUInt32();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Label: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Label: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Font: {FontName}");
            Lines.AppendLine($"Position: {PositionX} / {PositionY}");
            Lines.AppendLine($"Size: {Width} / {Height}");
            Lines.AppendLine($"Int5: {UnkInt5}");
            Lines.AppendLine($"Int6: {UnkInt6}");
            Lines.AppendLine($"Byte2: {UnkByte2}");
            Lines.AppendLine($"Int7: {UnkInt7}");
            Lines.AppendLine($"Int8: {UnkInt8}");
            Lines.AppendLine($"Int9: {UnkInt9}");
            Lines.AppendLine($"Int1: {UnkInt1}");
            Lines.AppendLine($"Byte1: {UnkByte1}");
            Lines.AppendLine($"Int2: {UnkInt2}");
            Lines.AppendLine($"Int3: {UnkInt3}");
            Lines.AppendLine($"Int4: {UnkInt4}");

            return Lines.ToString();
        }
    }
}