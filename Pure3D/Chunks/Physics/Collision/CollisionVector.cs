using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010007)]
    public class CollisionVector : Unknown
    {
        public CollisionVector(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Vector";
        }
    }
}