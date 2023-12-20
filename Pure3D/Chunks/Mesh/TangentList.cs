using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10015)]
    public class TangentList : Chunk
    {
        public Vector3[] Tangents;

        public TangentList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Tangents = new Vector3[len];
            for (int i = 0; i < len; i++)
                Tangents[i] = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Tangents.Length);
            for (int i = 0; i < Tangents.Length; i++)
            {
                Util.WriteVector3(writer, Tangents[i]);
            }
        }

        public override string ToString()
        {
            return $"Tangent List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Tangent List");
            Lines.AppendLine($"Length: {Tangents.Length}");
            for (int i = 0; i < Tangents.Length; i++)
            {
                Lines.AppendLine($"Normal{i}: {Tangents[i]}");
            }

            return Lines.ToString();
        }
    }
}
