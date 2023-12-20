using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010022)]
    public class CollisionVolumeOwnerName : Unknown
    {
        public CollisionVolumeOwnerName(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Collision Volume Owner Name";
        }
    }
}