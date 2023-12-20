using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10013)]
    public class NativeIndexList : Unknown
    {
        public NativeIndexList(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Native IndexList";
        }
    }
}