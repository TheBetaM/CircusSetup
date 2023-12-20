using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x122000)]
    public class SortOrder : Unknown
    {
        public SortOrder(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Sort Order";
        }
    }
}