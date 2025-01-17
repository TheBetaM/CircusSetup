using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17008)]
    public class BillboardCTTRUnk4 : Unknown
    {
        //uint,uint,short,short

        public BillboardCTTRUnk4(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Billboard CTTR Unk4";
        }
    }
}