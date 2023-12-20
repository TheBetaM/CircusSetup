using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x2201)]
    public class CameraUnk1 : Unknown
    {
        public CameraUnk1(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"CameraUnk1";
        }
    }
}