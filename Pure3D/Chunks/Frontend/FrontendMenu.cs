using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x18010)]
    public class FrontendMenu : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public byte UnkByte1;
        public byte UnkByte2;
        public byte[] Extra;


        public FrontendMenu(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long pos = reader.BaseStream.Position;
            base.ReadHeader(reader, length);
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
            UnkByte1 = reader.ReadByte();
            UnkByte2 = reader.ReadByte();
            //Titans
            if (reader.BaseStream.Position != pos + length)
            {
                Extra = reader.ReadBytes(5);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(UnkInt1);
            writer.Write(UnkInt2);
            writer.Write(UnkByte1);
            writer.Write(UnkByte2);
            if (Extra != null)
            {
                writer.Write(Extra);
            }
        }

        public override string ToString()
        {
            return $"FE Menu: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Menu {Name}");
            Lines.AppendLine($"Int1: {UnkInt1}");
            Lines.AppendLine($"Int2: {UnkInt2}");
            Lines.AppendLine($"Byte1: {UnkByte1}");
            Lines.AppendLine($"Byte2: {UnkByte2}");

            return Lines.ToString();
        }
    }
}