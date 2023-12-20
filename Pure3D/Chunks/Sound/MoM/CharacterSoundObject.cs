using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1010007)]
    public class CharacterSoundObject : Chunk
    {
        public string Name;
        public string Name2;
        public string Name3;

        public CharacterSoundObject(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            Name = Util.ReadString2(reader);
            Name2 = Util.ReadString2(reader);
            Name3 = Util.ReadString2(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Character {Name} {Name3}";
        }
    }
}
