using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x13003)]
    public class LightConeParam : Unknown
    {
        public LightConeParam(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"LightConeParam";
        }
    }
}