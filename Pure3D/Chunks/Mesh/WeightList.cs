using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000C)]
    public class WeightList : Chunk
    {
        public Vector3[] Weights;

        public WeightList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            ;
            uint len = reader.ReadUInt32();
            Weights = new Vector3[len];
            for (int i = 0; i < len; i++)
            {
                Weights[i] = Util.ReadVector3(reader);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Weights.Length);
            for (int i = 0; i < Weights.Length; i++)
            {
                Util.WriteVector3(writer, Weights[i]);
            }
        }

        public override string ToString()
        {
            return $"Weight List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Weight List");
            Lines.AppendLine($"Count: {Weights.Length}");
            for (int i = 0; i < Weights.Length; i++)
            {
                Lines.AppendLine($"Weight{i}: {Weights[i]}");
            }

            return Lines.ToString();
        }
    }
}