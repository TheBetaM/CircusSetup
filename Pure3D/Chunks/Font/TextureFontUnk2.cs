using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x22006)]
    public class TextureFontUnk2 : Unknown
    {
        public TextureFontUnk2(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"TextureFontUnk2";
        }
    }
}