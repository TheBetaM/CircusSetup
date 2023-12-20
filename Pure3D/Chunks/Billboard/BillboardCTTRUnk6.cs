using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1700D)]
    public class BillboardCTTRUnk6 : Unknown
    {
        public BillboardCTTRUnk6(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Billboard CTTR Unk6";
        }
    }
}