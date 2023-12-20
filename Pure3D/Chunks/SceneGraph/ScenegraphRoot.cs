using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;

namespace Pure3D.Chunks
{
    [ChunkType(0x12010B)]
    public class ScenegraphRoot : Chunk
    {
        public uint Version;

        public ScenegraphRoot(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Version);
        }

        public override string ToString()
        {
            return $"Scenegraph Root Ver. {Version}";
        }
    }
}