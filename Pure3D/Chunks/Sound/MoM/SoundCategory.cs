using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1010008)]
    public class SoundCategory : Unknown
    {

        public SoundCategory(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Sound Category";
        }
    }
}