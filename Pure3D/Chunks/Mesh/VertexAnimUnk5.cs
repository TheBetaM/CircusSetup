using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x10F02)]
    public class VertexAnimUnk5 : Unknown
    {
        public VertexAnimUnk5(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Vertex Anim Unk5";
        }
    }
}