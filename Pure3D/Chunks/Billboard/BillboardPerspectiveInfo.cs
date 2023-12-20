using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x17004)]
    public class BillboardPerspectiveInfo : Chunk
    {
        public uint Version;
        public uint Perspective;

        public BillboardPerspectiveInfo(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            Perspective = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Perspective);
        }

        public override string ToString()
        {
            return $"Billboard Perspective Info";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Billboard Perspective Info");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"Perspective: {Perspective}");

            return Lines.ToString();
        }
    }
}
