using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x0802000A)]
    public class StatePropContainer : Unknown
    {
        public StatePropContainer(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"State Prop Container";
        }
    }
}