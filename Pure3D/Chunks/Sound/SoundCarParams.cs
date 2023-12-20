using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x3F000F00)]
    public class SoundCarParams : Chunk
    {
        public string Name;
        public List<float> Params = new List<float>();

        public SoundCarParams(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            for (int i = 0; i < 18; i++)
            {
                Params.Add(reader.ReadSingle());
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
        }

        public override string ToString()
        {
            return $"Sound Car Parameters - {Name}";
        }
    }
}
