using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x14000)]
    public class Locator : Named
    {
        public uint Version;
        public Vector3 Position;

        public Locator(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            Position = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            Util.WriteVector3(writer, Position);
        }

        public override string ToString()
        {
            return $"Locator: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Locator: {Name}");
            Lines.AppendLine($"X: {Position.X}");
            Lines.AppendLine($"Y: {Position.Y}");
            Lines.AppendLine($"Z: {Position.Z}");

            return Lines.ToString();
        }
    }
}
