using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x0701000B)]
    public class CollisionVolumeName : Named
    {
        public CollisionVolumeName(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Volume Name: {Name}";
        }
    }
}