using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pure3D.Chunks
{
    [ChunkType(0x0802000A)]
    public class StatePropContainer : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint UnkInt3;
        public string PropName;
        public ulong PropName_padding;

        public StatePropContainer(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            PropName = Util.ReadString(reader, ref PropName_padding);
            UnkInt2 = reader.ReadUInt32();
            UnkInt3 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"State Prop Container: {Name} / {PropName}";
        }
    }
}