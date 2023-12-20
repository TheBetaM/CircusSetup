using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x802000B)]
    public class StatePropEffect : Unknown
    {

        public StatePropEffect(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"SP Effect";
        }
    }
}