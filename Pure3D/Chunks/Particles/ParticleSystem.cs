using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1580C)]
    public class ParticleSystem : Unknown
    {
        public ParticleSystem(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle System";
        }
    }
}
