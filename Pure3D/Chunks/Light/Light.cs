using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x13000)]
    public class Light : Unknown
    {
        public Light(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Light";
        }
    }
}