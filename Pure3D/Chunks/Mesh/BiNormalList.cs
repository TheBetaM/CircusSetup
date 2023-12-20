using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x10016)]
    public class BiNormalList : Chunk
    {
        public Vector3[] Normals;

        public BiNormalList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Normals = new Vector3[len];
            for (int i = 0; i < len; i++)
                Normals[i] = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Normals.Length);
            for (int i = 0; i < Normals.Length; i++)
            {
                Util.WriteVector3(writer, Normals[i]);
            }
        }

        public override string ToString()
        {
            return $"BiNormal List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"BiNormal List");
            Lines.AppendLine($"Length: {Normals.Length}");
            for (int i = 0; i < Normals.Length; i++)
            {
                Lines.AppendLine($"Normal{i}: {Normals[i]}");
            }

            return Lines.ToString();
        }
    }
}
