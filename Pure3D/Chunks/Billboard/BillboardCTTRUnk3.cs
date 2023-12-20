using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x17009)]
    public class BillboardCTTRUnk3 : Unknown
    {
        public BillboardCTTRUnk3(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Billboard CTTR Unk3";
        }
    }
}