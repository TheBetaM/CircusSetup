using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x08020009)]
    public class StatePropState : Unknown
    {
        public StatePropState(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"State Prop State";
        }
    }
}