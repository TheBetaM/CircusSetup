using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15A00)]
    public class ParticleUnk1 : Unknown
    {
        public ParticleUnk1(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle Unk1";
        }
    }
}
