using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10027)]
    public class MeshDataUnk2 : Unknown
    {
        public MeshDataUnk2(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"MeshDataUnk2";
        }
    }
}