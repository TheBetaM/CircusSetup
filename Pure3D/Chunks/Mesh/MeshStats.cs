using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1001D)]
    public class MeshStats : Unknown
    {
        public MeshStats(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Mesh Stats";
        }
    }
}
