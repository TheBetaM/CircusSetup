using System.Collections.Generic;
using System.IO;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x15805)]
    public class BaseEmitterFactory : Unknown
    {
        public BaseEmitterFactory(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"BaseEmitter";
        }
    }
}
