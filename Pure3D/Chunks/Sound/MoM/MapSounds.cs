using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1010005)]
    public class MapSounds : Unknown
    {

        public MapSounds(File file, uint type) : base(file, type)
        {

        }

        public override string ToString()
        {
            return $"Map Sounds";
        }
    }
}