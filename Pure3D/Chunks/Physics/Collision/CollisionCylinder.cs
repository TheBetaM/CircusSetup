using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010003)]
    public class CollisionCylinder : Unknown
    {
        public CollisionCylinder(File file, uint type) : base(file, type)
        {

        }
        public override string ToString()
        {
            return $"Collision Cylinder";
        }
    }
}