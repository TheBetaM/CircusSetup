using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x1010000)]
    public class ListenerSettings : Chunk
    {
        public string Name;
        public Vector3 Params = new Vector3();

        public ListenerSettings(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            int NameLength = reader.ReadInt32();
            Name = new string(reader.ReadChars(NameLength));
            reader.ReadByte();
            Params = Util.ReadVector3(reader);
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray());
            writer.Write((byte)0);
            Util.WriteVector3(writer, Params);
        }

        public override string ToString()
        {
            return $"ListenerSettings {Name}";
        }
    }
}
