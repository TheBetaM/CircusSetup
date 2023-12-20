using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x101000A)]
    public class MovieVolume : Chunk
    {
        public string Name;
        public float Volume;

        public MovieVolume(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            Volume = reader.ReadSingle();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray());
            writer.Write((byte)0);
            writer.Write(Volume);
        }

        public override string ToString()
        {
            return $"Movie Volume {Name} {Volume}";
        }
    }
}
