using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000106)]
    public class Reverb7 : Chunk
    {
        public string Name;
        public uint UnkInt;
        public float UnkFloat;

        public Reverb7(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            UnkFloat = reader.ReadSingle();
            UnkInt = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray());
            writer.Write((byte)0);
            writer.Write(UnkFloat);
            writer.Write(UnkInt);
        }

        public override string ToString()
        {
            return $"Reverb7 {Name}";
        }
    }
}
