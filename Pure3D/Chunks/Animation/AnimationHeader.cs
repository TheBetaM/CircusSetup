using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x121006)]
    public class AnimationHeader : Chunk
    {
        public uint Version;
        public uint GroupCount;

        public AnimationHeader(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Version = reader.ReadUInt32();
            GroupCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Animation Header {Version} / {GroupCount}";
        }
    }
}
