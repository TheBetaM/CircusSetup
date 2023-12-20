using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10007)]
    public class UVList : Chunk
    {
        public uint Channel;
        public Vector2[] UVs;

        public UVList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Channel = reader.ReadUInt32();
            UVs = new Vector2[len];
            for (int i = 0; i < len; i++)
                UVs[i] = Util.ReadVector2(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)UVs.Length);
            writer.Write(Channel);
            for (int i = 0; i < UVs.Length; i++)
            {
                Util.WriteVector2(writer, UVs[i]);
            }
        }

        public override string ToString()
        {
            return $"UV List (Channel: {Channel})";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"UV List");
            Lines.AppendLine($"Channel: {Channel}");
            Lines.AppendLine($"Count: {UVs.Length}");
            for (int i = 0; i < UVs.Length; i++)
            {
                Lines.AppendLine($"UV{i}: {UVs[i]}");
            }

            return Lines.ToString();
        }
    }
}
