using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15B01)]
    public class ParticleUnk2 : Unknown
    {
        public ParticleUnk2(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle Unk2";
        }
    }
}
