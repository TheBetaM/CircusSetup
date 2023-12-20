using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010000)]
    public class CollisionObject : Unknown
    {
        public CollisionObject(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Object";
        }
    }
}