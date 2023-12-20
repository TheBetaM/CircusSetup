using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1700A)]
    public class BillboardCTTRUnk5 : Unknown
    {
        public BillboardCTTRUnk5(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Billboard CTTR Unk5";
        }
    }
}