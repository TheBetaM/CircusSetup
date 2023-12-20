using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x8020030)]
    public class StatePropSound : Unknown
    {

        public StatePropSound(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"SP Sound";
        }
    }
}