using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x123002)]
    public class CompositeDrawableMOMUnk : Unknown
    {
        public CompositeDrawableMOMUnk(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"CompositeDrawableMOMUnk";
        }
    }
}