using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000)]
    public class History : Chunk
    {
        public List<string> HistoryText = new List<string>();
        public List<ulong> HistoryPadding = new List<ulong>();
        public ushort TextCount;

        public History(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            TextCount = reader.ReadUInt16();
            for (int i = 0; i < TextCount; i++)
            {
                ulong Pad = 0;
                HistoryText.Add(Util.ReadString(reader, ref Pad));
                HistoryPadding.Add(Pad);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            if (HistoryText.Count != 0)
            {
                return $"History Text: ({HistoryText.Count}) {HistoryText[0]}";
            }
            else
            {
                return $"History Text";
            }
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"History Text");
            Lines.AppendLine($"Lines {TextCount}");
            for (int i = 0; i < HistoryText.Count; i++)
            {
                Lines.AppendLine(HistoryText[i]);
            }

            return Lines.ToString();
        }
    }
}