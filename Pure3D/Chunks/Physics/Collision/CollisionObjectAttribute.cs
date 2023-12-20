using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010023)]
    public class CollisionObjectAttribute : Unknown
    {
        public CollisionObjectAttribute(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Object Attribute";
        }
    }
}