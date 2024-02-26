using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x23002)]
    public class SkeletonPartition : Named
    {
        public List<int> Joints = new();
        public SkeletonPartition(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            uint Count = reader.ReadUInt32();
            for (int i = 0; i < Count; i++)
            {
                uint data = reader.ReadUInt32();
                for (int j = 0; j < 32; j++)
                {
                    if ((data & (1 << j)) != 0)
                    {
                        Joints.Add(i * 32 + j);
                    }
                }
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Skeleton Partition {Name} - Joints: {Joints.Count}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Skeleton Partition");
            Lines.AppendLine($"Name: {Name}");
            Lines.AppendLine($"Joints: {Joints.Count}");
            for (int i = 0; i < Joints.Count; i++)
            {
                Lines.AppendLine($"#{i}: {Joints[i]}");
            }

            return Lines.ToString();
        }
    }
}