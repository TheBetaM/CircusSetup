using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x2380)]
    public class LightGroup : Named
    {
        public uint LightCount;
        public List<string> LightsList = new List<string>();
        public List<ulong> LightsList_padding = new List<ulong>();

        public LightGroup(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            LightCount = reader.ReadUInt32();
            for (int i = 0; i < LightCount; i++)
            {
                ulong pad = 0;
                LightsList.Add(Util.ReadString(reader, ref pad));
                LightsList_padding.Add(pad);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(LightsList.Count);
            for (int i = 0; i < LightsList.Count; i++)
            {
                Util.WriteString(writer, LightsList[i], LightsList_padding[i]);
            }
        }

        public override string ToString()
        {
            return $"LightGroup: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"LightGroup: {Name}");
            Lines.AppendLine($"Number of Lights: {LightCount}");
            foreach (var line in LightsList)
            {
                Lines.AppendLine(line);
            }

            return Lines.ToString();
        }
    }
}
