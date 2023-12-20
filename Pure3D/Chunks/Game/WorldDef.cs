using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks.Game
{
    [ChunkType(0x3F00020)]
    public class WorldDef : Named
    {
        public byte[] Data;

        public WorldDef(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Data = reader.ReadBytes((int)length);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"WorldDef";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"WorldDef");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }
    }
}
