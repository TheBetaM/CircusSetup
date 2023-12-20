using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x3F00100)]
    public class AnimationLipSync : Unknown
    {
        public AnimationLipSync(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Animation Lip Sync";
        }
    }
}
