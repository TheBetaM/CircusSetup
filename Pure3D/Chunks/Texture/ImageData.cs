using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x19002)]
    public class ImageData : Chunk
    {
        public byte[] Data;

        public ImageData(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Data = reader.ReadBytes((int)len);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Data.Length);
            writer.Write(Data);
        }

        public override string ToString()
        {
            return $"Image Data";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Image Data");
            Lines.AppendLine($"Length: {Data.Length}");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }
    }
}
