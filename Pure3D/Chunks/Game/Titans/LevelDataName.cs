using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Pure3D;
using Pure3D.Chunks;

namespace Pure3D.Chunks
{
    [ChunkType(0x5500001)]
    public class LevelDataName : Named
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public uint ItemCount1;
        public uint ItemCount2;

        public string StartingRoomName;
        public ulong StartingRoomName_padding;

        public LevelDataName(File file, uint type) : base(file, type)
        {
        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            if (UnkInt1 != 2)
            {
                // MoM
                ItemCount1 = reader.ReadUInt32();
                ItemCount2 = reader.ReadUInt32();
                base.ReadHeader(reader, length);
            }
            else
            {
                ItemCount1 = reader.ReadUInt32();
                ItemCount2 = reader.ReadUInt32();
                UnkInt2 = reader.ReadUInt32();
                base.ReadHeader(reader, length);
                StartingRoomName = Util.ReadString(reader, ref StartingRoomName_padding);
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Level Name: {Name} / Start: {StartingRoomName}";
        }
    }
}
