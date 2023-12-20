using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x10021)]
    public class VertexCompressionHint : Unknown
    {
        public VertexCompressionHint(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Vertex Compression Hint";
        }
    }
}