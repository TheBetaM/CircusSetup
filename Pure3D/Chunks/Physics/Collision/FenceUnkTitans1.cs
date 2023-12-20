using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000748)]
    public class FenceUnkTitans1 : Unknown
    {
        public FenceUnkTitans1(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Fence UnkTitans1";
        }
    }
}