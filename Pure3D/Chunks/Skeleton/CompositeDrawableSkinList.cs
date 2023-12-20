using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x4513)]
    public class CompositeDrawableSkinList : ListChunk
    {
        public CompositeDrawableSkinList(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            return $"Composite Drawable Skin List";
        }
    }
}
