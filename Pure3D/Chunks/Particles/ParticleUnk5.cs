using System.Collections.Generic;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(0x15B03)]
    public class ParticleUnk5 : Unknown
    {
        public ParticleUnk5(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle Unk5";
        }
    }
}
