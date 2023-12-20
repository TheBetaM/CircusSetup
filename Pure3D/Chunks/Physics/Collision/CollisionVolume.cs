using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010001)]
    public class CollisionVolume : Unknown
    {
        public CollisionVolume(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Volume";
        }
    }
}