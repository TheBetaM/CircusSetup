using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10013)]
    public class NativeIndexList : Unknown
    {
        public int Version;
        public int UnkParam;
        public int VifSize;
        public List<ushort> Indices = new List<ushort>();

        public NativeIndexList(File file, uint type) : base(file, type)
        {
            
        }

        public override string ToString()
        {
            return $"Native IndexList V {Version:X} P {UnkParam} Size {VifSize}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Native Index List");
            Lines.AppendLine($"Header: 0x{Version:X8}");
            Lines.AppendLine($"Param: 0x{UnkParam:X8}");
            Lines.AppendLine($"VifSize: 0x{VifSize:X8}");
            Lines.AppendLine($"Indices: {Indices.Count}");
            for (int i = 0; i < Indices.Count; i++)
            {
                Lines.AppendLine($"#{i}: {Indices[i]}");
            }
            Lines.AppendLine($"Data Length: {Data.Length}");
            Lines.AppendLine(Data.ToLine());

            return Lines.ToString();
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadInt32();
            UnkParam = reader.ReadInt32();
            VifSize = reader.ReadInt32();
            if (Version == 0x00020001)
            {
                Data = new byte[0];
                ReadXBOX(reader);
            }
            else
            {
                Data = reader.ReadBytes((int)length - 12);
            }
        }

        void ReadXBOX(BinaryReader reader)
        {
            var prim = (PrimitiveGroup)Parent;
            for (int i = 0; i < prim.NumIndices; i++)
            {
                Indices.Add(reader.ReadUInt16());
            }
        }
    }
}