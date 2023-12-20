using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10014)]
    public class NativeVertexDescription : Unknown
    {
        public int Version;

        public NativeVertexDescription(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Native VertexDesc";
        }
    }
}