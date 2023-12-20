using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x19007)]
    public class RawColorTextureData : Chunk
    {
        public byte[] Data;

        public RawColorTextureData(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Data = reader.ReadBytes((int)length);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override string ToString()
        {
            return $"Raw Color Texture Data";
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
