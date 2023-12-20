using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010004)]
    public class CollisionOrientedBoundingBox : Unknown
    {
        public CollisionOrientedBoundingBox(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Oriented Bounding Box";
        }
    }
}