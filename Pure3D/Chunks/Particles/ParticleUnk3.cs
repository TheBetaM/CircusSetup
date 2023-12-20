using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15B00)]
    public class ParticleUnk3 : Unknown
    {
        public ParticleUnk3(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle Unk3";
        }
    }
}
