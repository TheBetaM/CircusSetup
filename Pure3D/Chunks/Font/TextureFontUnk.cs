using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x22005)]
    public class TextureFontUnk : Unknown
    {
        public TextureFontUnk(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"TextureFontUnk";
        }
    }
}