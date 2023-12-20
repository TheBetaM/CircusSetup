using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000008)]
    public class FenceUnk2 : Unknown
    {
        public FenceUnk2(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Fence Unk2";
        }
    }
}