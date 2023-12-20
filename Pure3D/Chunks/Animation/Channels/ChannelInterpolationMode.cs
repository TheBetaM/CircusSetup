using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x121110)]
    public class ChannelInterpolationMode : Chunk
    {
        public uint Version;
        public int Mode;

        public ChannelInterpolationMode(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            Mode = reader.ReadInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Mode);
        }

        public override string ToString()
        {
            return $"Channel Interpolation Mode: {Mode}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Channel Interpolation Mode: {Mode}");
            Lines.AppendLine($"Version: {Version}");

            return Lines.ToString();
        }
    }
}
