using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010006)]
    public class CollisionAxisAlignedBoundingBox : Unknown
    {
        public CollisionAxisAlignedBoundingBox(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Axis Aligned Bounding Box";
        }
    }
}