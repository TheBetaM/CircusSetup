using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4517)]
    public class CompositeDrawableEffectList : ListChunk
    {
        public CompositeDrawableEffectList(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            return $"Composite Drawable Effect List";
        }
    }
}
