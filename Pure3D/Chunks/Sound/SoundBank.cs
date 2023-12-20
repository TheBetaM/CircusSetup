using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System;

namespace Pure3D.Chunks
{
    //[ChunkType(0x1000001)]
    public class SoundBank : Chunk
    {
        public string Name;


        public SoundBank(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Name = Util.ReadString2(reader);
            uint RuntimeCount = reader.ReadUInt32();
            uint ParamCount = reader.ReadUInt32();

        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Sound Bank {Name}";
        }
    }
}
