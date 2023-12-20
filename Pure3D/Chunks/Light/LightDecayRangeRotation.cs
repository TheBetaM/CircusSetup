using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x13007)]
    public class LightDecayRangeRotation : Unknown
    {
        public LightDecayRangeRotation(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Light Decay Range Rotation";
        }
    }
}