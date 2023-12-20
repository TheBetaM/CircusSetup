using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x122001)]
    public class SortOrderUnkVector : Unknown
    {
        public SortOrderUnkVector(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"SortOrderUnkVector";
        }
    }
}