using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1700E)]
    public class BillboardCTTRUnk8 : Unknown
    {
        public BillboardCTTRUnk8(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Billboard CTTR Unk8";
        }
    }
}