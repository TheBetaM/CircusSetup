using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000100)]
    public class Reverb1 : Chunk
    {
        public string Name;
        public List<float> Params = new List<float>();
        public List<uint> Params2 = new List<uint>();

        public Reverb1(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            for (int i = 0; i < 5; i++)
            {
                Params.Add(reader.ReadSingle());
            }
            for (int i = 0; i < 7; i++)
            {
                Params2.Add(reader.ReadUInt32());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray());
            writer.Write((byte)0);
            for (int i = 0; i < Params.Count; i++)
            {
                writer.Write(Params[i]);
            }
            for (int i = 0; i < Params2.Count; i++)
            {
                writer.Write(Params2[i]);
            }
        }

        public override string ToString()
        {
            return $"Reverb1 {Name}";
        }
    }
}
