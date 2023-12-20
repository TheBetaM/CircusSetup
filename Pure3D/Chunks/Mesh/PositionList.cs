using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10005)]
    public class PositionList : Chunk
    {
        public Vector3[] Positions;

        public PositionList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Positions = new Vector3[len];
            for (int i = 0; i < len; i++)
                Positions[i] = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Positions.Length);
            for (int i = 0; i < Positions.Length; i++)
            {
                Util.WriteVector3(writer, Positions[i]);
            }
        }

        public override string ToString()
        {
            return $"Position List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Position List");
            Lines.AppendLine($"Length: {Positions.Length}");
            for (int i = 0; i < Positions.Length; i++)
            {
                Lines.AppendLine($"Pos{i}: {Positions[i]}");
            }

            return Lines.ToString();
        }
    }
}
