using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7000006)]
    public class FenceUnk1 : Unknown
    {
        public FenceUnk1(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Fence Unk1";
        }
    }
}