using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15808)]
    public class ParticleAnimation : Unknown
    {
        public ParticleAnimation(File file, uint type) : base(file, type)
        {
        }

        public override string ToString()
        {
            return $"Particle Animation";
        }
    }
}
