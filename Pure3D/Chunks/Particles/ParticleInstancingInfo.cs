using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x1580B)]
    public class ParticleInstancingInfo : Unknown
    {
        public ParticleInstancingInfo(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Particle Instancing Info";
        }
    }
}
