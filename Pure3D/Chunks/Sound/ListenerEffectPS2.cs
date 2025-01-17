using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1000101)]
    public class ListenerEffectPS2 : Chunk
    {
        public string Name;
        public List<float> Params = new List<float>();
        public uint UnkInt;

        public ListenerEffectPS2(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            for (int i = 0; i < 3; i++)
            {
                Params.Add(reader.ReadSingle());
            }
            UnkInt = reader.ReadUInt32();
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
            writer.Write(UnkInt);
        }

        public override string ToString()
        {
            return $"ListenerEffectPS2 {Name}";
        }
    }
}
