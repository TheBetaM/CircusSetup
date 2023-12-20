using System.Collections.Generic;
using System.IO;

namespace Pure3D.Chunks
{
    [ChunkType(0x15B02)]
    public class ParticleUnk4 : Unknown
    {
        public ParticleUnk4(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle Unk4";
        }
    }
}
