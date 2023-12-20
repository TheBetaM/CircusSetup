using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1001A)]
    public class ShadowMesh : Unknown
    {
        public ShadowMesh(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Shadow Mesh";
        }
    }
}