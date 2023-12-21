using System.Collections.Generic;
using System.IO;
using System.Text;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010000)]
    public class CollisionObject : Named
    {
        public uint Version;
        public string MaterialName;
        public ulong MaterialName_padding;
        public uint UnkInt1;
        public uint UnkInt2;

        public CollisionObject(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            MaterialName = Util.ReadString(reader, ref MaterialName_padding);
            UnkInt1 = reader.ReadUInt32();
            UnkInt2 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Collision Object: {Name}";
        }
    }
}