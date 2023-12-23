using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks.Game
{
    [ChunkType(0x3F00020)]
    public class WorldDef : Named
    {
        public ushort UnkShort1;
        public byte[] Data;
        public uint UnkPlatformSpecific;

        public WorldDef(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            UnkShort1 = reader.ReadUInt16();
            UnkPlatformSpecific = reader.ReadUInt32(); // the only value that differs between consoles
            ushort[] UnkShorts = new ushort[17];
            for (int i = 0; i < UnkShorts.Length; i++)
            {
                UnkShorts[i] = reader.ReadUInt16();
            }
            Data = reader.ReadBytes((int)(length - reader.BaseStream.Position));
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"WorldDef {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"WorldDef {Name}");
            Lines.AppendLine($"UnkPlatformSpecific {UnkPlatformSpecific}");
            //Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }
    }
}
