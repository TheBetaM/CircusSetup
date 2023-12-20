using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17006)]
    public class BillboardQuadGroupCTTR : Unknown
    {
        public BillboardQuadGroupCTTR(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Billboard Quad Group CTTR";
        }
    }
}