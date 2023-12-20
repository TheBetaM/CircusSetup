using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000D)]
    public class MatrixPalette : Chunk
    {
        public uint[] Matrices;

        public MatrixPalette(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            uint len = reader.ReadUInt32();
            Matrices = new uint[len];
            for (int i = 0; i < len; i++)
                Matrices[i] = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write((uint)Matrices.Length);
            for (int i = 0; i < Matrices.Length; i++)
            {
                writer.Write(Matrices[i]);
            }
        }

        public override string ToString()
        {
            return $"Matrix Palette";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Matrix Palette");
            Lines.AppendLine($"Length: {Matrices.Length}");
            for (int i = 0; i < Matrices.Length; i++)
            {
                Lines.AppendLine($"Palette{i}: {Matrices[i]}");
            }

            return Lines.ToString();
        }
    }
}
