using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010021)]
    public class CollisionVolumeOwner : Unknown
    {
        public CollisionVolumeOwner(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Volume Owner";
        }
    }
}