using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    public class ListChunk : Chunk
    {
        public uint NumElements; // should be # of children.

        public ListChunk(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            NumElements = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(NumElements);
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine(ToString());
            Lines.AppendLine($"Elements: {NumElements}");

            return Lines.ToString();
        }
    }
}
