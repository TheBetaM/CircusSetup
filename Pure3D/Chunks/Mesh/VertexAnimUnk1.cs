using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x10F01)]
    public class VertexAnimUnk1 : Unknown
    {
        public VertexAnimUnk1(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Vertex Anim Unk1";
        }
    }
}