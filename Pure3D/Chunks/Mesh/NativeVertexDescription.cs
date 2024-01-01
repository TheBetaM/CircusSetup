using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10014)]
    public class NativeVertexDescription : Unknown
    {
        public int Version;
        public int UnkParam;
        public int VifSize;

        public NativeVertexDescription(File file, uint type) : base(file, type)
        {
            
        }

        public override string ToString()
        {
            return $"Native VertexDesc V {Version:X} P {UnkParam} Size {VifSize}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Native Vertex Description");
            Lines.AppendLine($"Header: 0x{Version:X8}");
            Lines.AppendLine($"Param: 0x{UnkParam:X8}");
            Lines.AppendLine($"VifSize: 0x{VifSize:X8}");
            Lines.AppendLine($"Length: {Data.Length}");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadInt32();
            UnkParam = reader.ReadInt32();
            VifSize = reader.ReadInt32();
            Data = reader.ReadBytes((int)length - 12);
        }

    }
}