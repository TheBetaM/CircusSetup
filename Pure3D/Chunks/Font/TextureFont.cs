using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x22000)]
    public class TextureFont : Unknown
    {
        public TextureFont(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Texture Font";
        }
    }
}