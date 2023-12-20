using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    public class Unknown : Chunk
    {
        public byte[] Data;
        private uint unknownType;
        bool DebugUnk = false;

        public Unknown(File file, uint type) : base(file, type)
        {
            unknownType = type;
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Data = reader.ReadBytes((int)length);
            ToString();
            if (DebugUnk)
            {
                Debug.WriteLine(ToString());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override string ToString()
        {
            //return $"Unknown Chunk (TypeID: {unknownType}) (Len: {Data.Length})";
            //DebugUnk = true;
            if (Data == null) return $"Unknown (ID: 0x{unknownType:X8}) ERR";
            return $"Unknown (ID: 0x{unknownType:X8}) (Size: {Data.Length})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Unknown Chunk");
            Lines.AppendLine($"Type: {unknownType}");
            Lines.AppendLine($"Type: 0x{unknownType:X8}");
            Lines.AppendLine($"Length: {Data.Length}");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }
    }
}
