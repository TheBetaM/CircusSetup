using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x13008)]
    public class IlluminationType : Unknown
    {
        public uint Type;

        public IlluminationType(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Type = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Type);
        }

        public override string ToString()
        {
            return $"Illumination Type {Type}";
        }
    }
}