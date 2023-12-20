using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x3F000F01)]
    public class SoundRaceParams : Chunk
    {
        public string Name;
        public float Param;

        public SoundRaceParams(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            Param = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray());
            writer.Write((byte)0);
            writer.Write(Param);
        }

        public override string ToString()
        {
            return $"Sound Race Parameters - {Name}: {Param}";
        }
    }
}
