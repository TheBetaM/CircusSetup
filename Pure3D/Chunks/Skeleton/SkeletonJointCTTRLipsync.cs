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
    public class SkeletonPartition : Unknown
    {
        public SkeletonPartition(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            return $"Skeleton Partition";
        }
    }
}