using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000B)]
    public class MatrixList : Chunk
    {
        public byte[][] Matrices;

        public MatrixList(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Matrices = new byte[len][];
            for (int i = 0; i < len; i++)
            {
                Matrices[i] = new byte[4];
                Matrices[i][0] = reader.ReadByte();
                Matrices[i][1] = reader.ReadByte();
                Matrices[i][2] = reader.ReadByte();
                Matrices[i][3] = reader.ReadByte();
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Matrices.Length);
            for (int i = 0; i < Matrices.Length; i++)
            {
                writer.Write(Matrices[i][0]);
                writer.Write(Matrices[i][1]);
                writer.Write(Matrices[i][2]);
                writer.Write(Matrices[i][3]);
            }
        }

        public override string ToString()
        {
            return $"Matrix List";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Matrix List");
            Lines.AppendLine($"Length: {Matrices.Length}");
            for (int i = 0; i < Matrices.Length; i++)
            {
                //Lines.AppendLine($"Matrix{i}: {Matrices[i].ToLine()}");
                Lines.AppendLine($"Matrix{i}: {Matrices[i][0]:X2} /  {Matrices[i][1]:X2} /  {Matrices[i][2]:X2} /  {Matrices[i][3]:X2}");
            }

            return Lines.ToString();
        }
    }
}
