using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10026)]
    public class MeshDataUnk1 : Unknown
    {
        public MeshDataUnk1(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk1";
        }
    }
}