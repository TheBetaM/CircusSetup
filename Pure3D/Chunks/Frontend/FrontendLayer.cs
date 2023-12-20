using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x18020)]
    public class FrontendLayer : Named
    {
        public uint Unk1;
        public uint Unk2;
        public uint Unk3;
        public uint Unk4;

        public FrontendLayer(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Unk1 = reader.ReadUInt32();
            Unk2 = reader.ReadUInt32();
            Unk3 = reader.ReadUInt32();
            Unk4 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"FE Layer: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Frontend Layer: {Name}");
            Lines.AppendLine($"Unk1: {Unk1}");
            Lines.AppendLine($"Unk2: {Unk2}");
            Lines.AppendLine($"Unk3: {Unk3}");
            Lines.AppendLine($"Unk4: {Unk4}");

            return Lines.ToString();
        }
    }
}