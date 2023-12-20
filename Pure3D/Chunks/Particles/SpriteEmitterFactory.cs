using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15806)]
    public class SpriteEmitterFactory : Unknown
    {
        public SpriteEmitterFactory(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Sprite Emitter";
        }
    }
}
